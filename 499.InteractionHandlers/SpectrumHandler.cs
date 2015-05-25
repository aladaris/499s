using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _499.InteractionHandlers {

    public struct Spectrum {
        private int _id;
        private byte _layer;
        private Midi.Control _midiCC;

        public Midi.Control CCValue { get { return _midiCC; } }

        public Spectrum(int id, byte layer, Midi.Control cc) {
            _id = id;
            _layer = layer;
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

        public bool NewUser() {
            _nUsers++;  // TODO: Donde poner?
            if (_status == SPECTRUM_HANDLER_STATUS.IDLE) {
                _status = SPECTRUM_HANDLER_STATUS.BLOCKED;
                _currentSpectrum = GetCorrespondingSpectrum();
                _knobFade.CCValue = _currentSpectrum.CCValue;
                // Setting the fade in properties
                _knobFade.UpOrDown = 1;
                _knobFade.SetRange(0, 127);
                _knobFade.Duration = _fadeInTime;
                _knobFade.KnobEndRunning += FadeInEnded;
                _knobFade.Start();
                return true;
            } else if (_status == SPECTRUM_HANDLER_STATUS.SHOWING) {
                // TODO: Manejar toda la baina de que se escoja un video en una capa inferior (Set ese video a 127 y hacer un fadeout del actual)
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
            // TODO: Selección de spectrum
            System.Random rand = new Random();
            //return _spectrums[rand.Next(_spectrums.Length)];
            return _spectrums[0];  // Debug
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

        private void DurationEnded(object sender, ElapsedEventArgs e) {
            if (!_knobFade.IsRunning) {
                if (_status == SPECTRUM_HANDLER_STATUS.SHOWING) {
                    _status = SPECTRUM_HANDLER_STATUS.BLOCKED;
                    _knobFade.CCValue = _currentSpectrum.CCValue;
                    // Setting the fade out properties
                    _knobFade.UpOrDown = -1;
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
