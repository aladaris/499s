using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _499.InteractionHandlers {

    public class Spectrum {
        public Midi.Control CCValue { get; private set; }
        public Midi.Pitch NotePlay { get; private set; }
        public Midi.Pitch NoteStop { get; private set; }
        public int Id { get; set; }
        public int Layer { get; set; }
        public byte HueValue { get; set; }  // Hue value associated with this spectrum (for the Glow layer)

        public Spectrum(int id, int layer, Midi.Control cc, Midi.Pitch note_play, Midi.Pitch note_stop, byte hue) {
            Id = id;
            Layer = layer;
            CCValue = cc;
            NotePlay = note_play;
            NoteStop = note_stop;
            HueValue = hue;
        }
    }

    public enum SPECTRUM_HANDLER_STATUS {
        IDLE,    // No se están mostrando spectrums.
        BLOCKED, // Haciendo fadin o fadeout (o cualquier otra acción ininterrumpible.
        SHOWING, // Mostrando un spectrum. Después del fadein y antes del  fadeout.
        RESTING  // Al parar, permanece desactivado un tiempo.
    }

    public class SpectrumHandler {
        private int _nUsers = 0;
        public const Midi.Pitch BASE_SUN_LOOP_NOTE_PLAY = Midi.Pitch.ASharp1;  // For playing the main loop
        public const Midi.Pitch BASE_SUN_LOOP_NOTE_STOP = Midi.Pitch.ASharp2;  // For Stoping the main loop
        public const Midi.Control BASE_SUN_PLAYING_POS = Midi.Control.DataEntryMSB;  // for setting the main loop random position
        private const Midi.Control SPECTRUMS_PLAYING_POS = Midi.Control.DataEntryLSB;  // for setting the spectrums random position
        //private Midi.Pitch _previousClipStop;  // The note to turn off the previous spectrum (or base loop) after the fade in (in SHOWING)
        private Spectrum[] _spectrums;
        private Spectrum _currentSpectrum;
        private Spectrum _oldSpect = null;
        private SPECTRUM_HANDLER_STATUS _status = SPECTRUM_HANDLER_STATUS.IDLE;
        private MidiKnob _knobFade;
        private MidiKnob _knobGlowHue;
        private const byte BASE_HUE_VALUE = 77;
        private int _fadeInTime;
        private int _fadeOutTime;
        private Timer _duration; // Duration (without fadein and fadeout)
        private Timer _rester;  // We disable the handler for _resTime milliseconds after it stops
        // events
        public event SendMidiControlChangeHandler SendControlChange;
        public event SendMidiNoteHandler SendMidiNote;

        public Spectrum[] Spectrums { get { return _spectrums; } }
        public SPECTRUM_HANDLER_STATUS Status { get { return _status; } }

        public SpectrumHandler(int spectrum_count, int duration = 5000, int fadeIn = 1000, int fadeOut = 1000, int rest = 7500) {
            if (spectrum_count > 0)
                _spectrums = new Spectrum[spectrum_count];
            else
                throw new IndexOutOfRangeException("The handler need at least one (1) spectrum.");

            _fadeInTime = fadeIn;
            _fadeOutTime = fadeOut;
            _knobFade = new MidiKnob(0, Midi.Control.Volume, 0, 127, _fadeInTime);
            _knobFade.SendMidiControlChange += SendMidiControlChange;
            _knobGlowHue = new MidiKnob(0, Midi.Control.Pan, 0, 127, _fadeInTime);
            _knobGlowHue.SendMidiControlChange += SendMidiControlChange;
            _duration = new Timer(duration);
            _duration.AutoReset = false;
            _duration.Elapsed += DurationEnded;
            _rester = new Timer(rest);
            _rester.AutoReset = false;
            _rester.Elapsed += RestingEnded;
            Reset();
        }

        public void Reset() {
            _nUsers = 0;
            _knobFade.Stop();
            _duration.Stop();
            _rester.Stop();
            if (SendControlChange != null) {
                SendControlChange(_knobFade.CCValue, 0);
                SendControlChange(_knobGlowHue.CCValue, BASE_HUE_VALUE);
                for (int i = 0; i < _spectrums.Length; i++) {
                    SendControlChange(_spectrums[i].CCValue, 0);
                    if (SendMidiNote != null)
                        SendMidiNote(_spectrums[i].NoteStop);
                }
            }
            if (SendMidiNote != null) {
                SendMidiNote(BASE_SUN_LOOP_NOTE_PLAY);
            }

        }

        public bool NewUser() {
            _nUsers++;
            System.Random rand = new Random();
            if (_status == SPECTRUM_HANDLER_STATUS.IDLE) {
                _status = SPECTRUM_HANDLER_STATUS.BLOCKED;
                _currentSpectrum = GetCorrespondingSpectrum();


                ////////////////////////////////////////////////////////
                // Start the selected spectrum at a random position
                if (SendMidiNote != null)
                    SendMidiNote(_currentSpectrum.NotePlay);
                if (SendControlChange != null)
                    SendControlChange(SPECTRUMS_PLAYING_POS, rand.Next(128));

                // Setting the fade in properties
                _knobFade.CCValue = _currentSpectrum.CCValue;
                _knobFade.SetRange(0, 127);
                _knobFade.Duration = _fadeInTime;
                _knobFade.KnobEndRunning += FadeInEnded;

                if (_knobGlowHue.IsRunning)
                    _knobGlowHue.Stop();
                _knobGlowHue.SetRange(_knobGlowHue.FinalValue, _currentSpectrum.HueValue);

                _knobFade.Start();
                _knobGlowHue.Start();
                return true;
            } else if (_status == SPECTRUM_HANDLER_STATUS.SHOWING) {
                _status = SPECTRUM_HANDLER_STATUS.BLOCKED;
                _oldSpect = _currentSpectrum;
                _currentSpectrum = GetCorrespondingSpectrum();


                ////////////////////////////////////////////////////////
                // Start the selected spectrum at a random position
                if (SendMidiNote != null)
                    SendMidiNote(_currentSpectrum.NotePlay);
                if (SendControlChange != null)
                    SendControlChange(SPECTRUMS_PLAYING_POS, rand.Next(128));


                if (_currentSpectrum.Layer > _oldSpect.Layer) {
                    if (!_knobFade.IsRunning) {
                        _duration.Elapsed -= DurationEnded;
                        _duration.Stop();
                        // Setting the fade in properties
                        _knobFade.CCValue = _currentSpectrum.CCValue;
                        _knobFade.SetRange(0, 127);
                        _knobFade.Duration = _fadeInTime;
                        _knobFade.KnobEndRunning += FadeInEndedFromShowing;

                        if (_knobGlowHue.IsRunning)
                            _knobGlowHue.Stop();
                        _knobGlowHue.SetRange(_knobGlowHue.FinalValue, _currentSpectrum.HueValue);

                        _knobFade.Start();
                        _knobGlowHue.Start();
                        return true;
                    }
                } else if (_currentSpectrum.Layer < _oldSpect.Layer) {
                    if (!_knobFade.IsRunning) {
                        _duration.Elapsed -= DurationEnded;
                        _duration.Stop();
                        SendMidiControlChange(_currentSpectrum.CCValue, 127);  // Establecemos al 100% la capa a la que transicionar
                        // Setting the fade in properties
                        _knobFade.CCValue = _oldSpect.CCValue;
                        _knobFade.SetRange(127, 0);
                        _knobFade.Duration = _fadeInTime;
                        _knobFade.KnobEndRunning += FadeInEndedFromShowing;

                        if (_knobGlowHue.IsRunning)
                            _knobGlowHue.Stop();
                        _knobGlowHue.SetRange(_knobGlowHue.FinalValue, _currentSpectrum.HueValue);

                        _knobFade.Start();
                        _knobGlowHue.Start();
                        return true;
                    }
                }
            }
            return false;
        }

        public bool RemoveUser() {
            if (_nUsers - 1 < 0)
                return false;
            _nUsers--;
            return true;
        }

        private Spectrum GetCorrespondingSpectrum() {
            System.Random rand = new Random();
            Spectrum spect = _spectrums[rand.Next(_spectrums.Length)];
            if (_currentSpectrum != null) {
                while (spect.Id == _currentSpectrum.Id)
                    spect = _spectrums[rand.Next(_spectrums.Length)];
            }
            return spect;
            //return _spectrums[_nUsers - 1];  // Debug
        }

        private void SendMidiControlChange(Midi.Control control, int value) {
            if (SendControlChange != null)
                SendControlChange(control, value);
        }

        private void FadeInEnded(int id) { // We only enter here from IDLE
            if (_duration != null) {
                if (!_duration.Enabled) {
                    if (_status == SPECTRUM_HANDLER_STATUS.BLOCKED) {
                        _status = SPECTRUM_HANDLER_STATUS.SHOWING;
                        _knobFade.KnobEndRunning -= FadeInEnded;
                        _duration.Start();
                        // We stop the base loop, since we are already showing a spectrum.
                        if (SendMidiNote != null){
                                SendMidiNote(BASE_SUN_LOOP_NOTE_STOP);
                        }
                    }
                }
            }
        }

        private void FadeInEndedFromShowing(int id) {
            if (_status == SPECTRUM_HANDLER_STATUS.BLOCKED) {
                if (SendMidiNote != null)
                    SendMidiNote(_oldSpect.NoteStop);  // Apagamos el spectrum anterior

                _knobFade.KnobEndRunning -= FadeInEndedFromShowing;
                _duration.Elapsed += DurationEnded;
                _duration.AutoReset = false;
                _status = SPECTRUM_HANDLER_STATUS.SHOWING;
                _duration.Start();
            }
        }

        private void DurationEnded(object sender, ElapsedEventArgs e) {
            if (!_knobFade.IsRunning) {
                if (_status == SPECTRUM_HANDLER_STATUS.SHOWING) {
                    _status = SPECTRUM_HANDLER_STATUS.BLOCKED;

                    // We turn on the main loop at a random point
                    System.Random rand = new Random();
                    if (SendMidiNote != null)
                        SendMidiNote(BASE_SUN_LOOP_NOTE_PLAY);
                    if (SendControlChange != null)
                        SendControlChange(BASE_SUN_PLAYING_POS, rand.Next(128));
                    _oldSpect = _currentSpectrum;

                    // Setting the fade out properties
                    _knobFade.CCValue = _currentSpectrum.CCValue;
                    _knobFade.SetRange(127, 0);
                    _knobFade.Duration = _fadeOutTime;
                    _knobFade.KnobEndRunning += FadeOutEnded;

                    if (_knobGlowHue.IsRunning)
                        _knobGlowHue.Stop();
                    _knobGlowHue.SetRange(_knobGlowHue.FinalValue, BASE_HUE_VALUE);

                    _knobFade.Start();
                    _knobGlowHue.Start();
                }
            }
        }

        private void FadeOutEnded(int id) {
            if (!_knobFade.IsRunning) {
                if (!_rester.Enabled) {
                    if (_status == SPECTRUM_HANDLER_STATUS.BLOCKED) {
                        // We turn off the previous spectrum
                        if (SendMidiNote != null) {
                            if (_oldSpect != null)
                                SendMidiNote(_oldSpect.NoteStop);
                        }

                        _status = SPECTRUM_HANDLER_STATUS.RESTING;
                        _knobFade.KnobEndRunning -= FadeOutEnded;
                        _rester.Start();
                    }
                }
            }

        }

        private void RestingEnded(object sender, ElapsedEventArgs e) {
            if (_status == SPECTRUM_HANDLER_STATUS.RESTING) {
                _status = SPECTRUM_HANDLER_STATUS.IDLE;
            }
        }

    }
}
