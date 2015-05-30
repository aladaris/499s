using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _499.InteractionHandlers {

    public class Spectrum {
        private Midi.Control _midiCC;

        public Midi.Control CCValue { get { return _midiCC; } }
        public int Id { get; set; }
        public int Layer { get; set; }

        public Spectrum(int id, int layer, Midi.Control cc) {
            Id = id;
            Layer = layer;
            _midiCC = cc;
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
        private Spectrum[] _spectrums;
        private Spectrum _currentSpectrum;
        private Spectrum _oldSpect;
        private SPECTRUM_HANDLER_STATUS _status = SPECTRUM_HANDLER_STATUS.IDLE;
        private MidiKnob _knobFade;
        private int _fadeInTime;
        private int _fadeOutTime;
        private Timer _duration; // Duration (without fadein and fadeout)
        private Timer _rester;  // We disable the handler for _resTime milliseconds after it stops
        // events
        public event SendMidiControlChangeHandler SendControlChange;

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
            _duration = new Timer(duration);
            _duration.AutoReset = false;
            _duration.Elapsed += DurationEnded;
            _rester = new Timer(rest);
            _rester.AutoReset = false;
            _rester.Elapsed += RestingEnded;
        }

        public void Reset() {
            _nUsers = 0;
            _knobFade.Stop();
            _duration.Stop();
            _rester.Stop();
            if (SendControlChange != null)
                SendControlChange(_knobFade.CCValue, 0);

        }

        public bool NewUser() {
            _nUsers++;
            if (_status == SPECTRUM_HANDLER_STATUS.IDLE) {
                _status = SPECTRUM_HANDLER_STATUS.BLOCKED;
                _currentSpectrum = GetCorrespondingSpectrum();
                // Setting the fade in properties
                _knobFade.CCValue = _currentSpectrum.CCValue;
                _knobFade.SetRange(0, 127);
                _knobFade.Duration = _fadeInTime;
                _knobFade.KnobEndRunning += FadeInEnded;
                _knobFade.Start();
                return true;
            } else if (_status == SPECTRUM_HANDLER_STATUS.SHOWING) {
                _status = SPECTRUM_HANDLER_STATUS.BLOCKED;
                _oldSpect = _currentSpectrum;
                _currentSpectrum = GetCorrespondingSpectrum();
                if (_currentSpectrum.Layer > _oldSpect.Layer) {
                    if (!_knobFade.IsRunning) {
                        _duration.Elapsed -= DurationEnded;
                        _duration.Stop();
                        // Setting the fade in properties
                        _knobFade.CCValue = _currentSpectrum.CCValue;
                        _knobFade.SetRange(0, 127);
                        _knobFade.Duration = _fadeInTime;
                        _knobFade.KnobEndRunning += FadeInEndedFromShowing;
                        _knobFade.Start();
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
                        _knobFade.Start();
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

        private void FadeInEnded(int id) {
            if (_duration != null) {
                if (!_duration.Enabled) {
                    if (_status == SPECTRUM_HANDLER_STATUS.BLOCKED) {
                        _status = SPECTRUM_HANDLER_STATUS.SHOWING;
                        _knobFade.KnobEndRunning -= FadeInEnded;
                        _duration.Start();
                    }
                }
            }
        }

        private void FadeInEndedFromShowing(int id) {
            if (_status == SPECTRUM_HANDLER_STATUS.BLOCKED) {
                if (_currentSpectrum.Layer > _oldSpect.Layer)
                    SendMidiControlChange(_oldSpect.CCValue, 0);  // Apagamos el spectrum anterior (si vamos de abajo a arriba)
                _knobFade.KnobEndRunning -= FadeInEndedFromShowing;
                _duration.Elapsed += DurationEnded;
                _duration.AutoReset = false;
                FadeInEnded(id);
            }
        }

        private void DurationEnded(object sender, ElapsedEventArgs e) {
            if (!_knobFade.IsRunning) {
                if (_status == SPECTRUM_HANDLER_STATUS.SHOWING) {
                    _status = SPECTRUM_HANDLER_STATUS.BLOCKED;
                    _knobFade.CCValue = _currentSpectrum.CCValue;
                    // Setting the fade out properties
                    _knobFade.SetRange(127, 0);
                    _knobFade.Duration = _fadeOutTime;
                    _knobFade.KnobEndRunning += FadeOutEnded;
                    _knobFade.Start();
                }
            }
        }

        private void FadeOutEnded(int id) {
            if (!_knobFade.IsRunning) {
                if (!_rester.Enabled) {
                    if (_status == SPECTRUM_HANDLER_STATUS.BLOCKED) {
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
