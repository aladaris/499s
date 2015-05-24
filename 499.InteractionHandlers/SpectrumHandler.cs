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
        private MidiKnob _knobIn;
        private MidiKnob _knobOut;
        private Timer _duration; // Duration (without fadein and fadeout)
        private Timer _rester;  // We disable the handler for _resTime milliseconds after it stops
        // events
        public event SendMidiControlChangeHandler SendControlChange;

        public Spectrum[] Spectrums { get { return _spectrums; } }
        public SPECTRUM_HANDLER_STATUS Status { get { return _status; } }

        public SpectrumHandler(int spectrum_count, Midi.OutputDevice midi_out, int duration = 5000, int fadeIn = 1000, int fadeOut = 1000, int rest = 7500) {
            if (spectrum_count > 0)
                _spectrums = new Spectrum[spectrum_count];
            else
                throw new IndexOutOfRangeException("The handler need at least one (1) spectrum.");

            _knobIn = new MidiKnob(0, Midi.Control.Volume, 0, 127, fadeIn);
            _knobOut = new MidiKnob(1, Midi.Control.Volume, 127, 0, fadeOut);
            _knobIn.SendMidiControlChange += SendMidiControlChange;
            _knobOut.SendMidiControlChange += SendMidiControlChange;
            _knobIn.KnobEndRunning += FadeInEnded;
            _knobOut.KnobEndRunning += FadeOutEnded;
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
                _currentSpectrum = GetCorrespondingSpectrum();
                _knobIn.CCValue = _currentSpectrum.CCValue;
                _status = SPECTRUM_HANDLER_STATUS.BLOCKED;
                _knobIn.Start();
                return true;
            }
            return false;
        }

        private Spectrum GetCorrespondingSpectrum() {
            // TODO: Selección de spectrum
            //return _spectrums[_nUsers % _spectrums.Length];
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
                        _duration.Start();
                    }
                }
            }
        }

        private void DurationEnded(object sender, ElapsedEventArgs e) {
            if (!_knobOut.IsRunning) {
                if (_status == SPECTRUM_HANDLER_STATUS.SHOWING) {
                    _knobOut.CCValue = _currentSpectrum.CCValue;
                    _status = SPECTRUM_HANDLER_STATUS.BLOCKED;
                    _knobOut.Start();
                }
            }
        }

        private void FadeOutEnded(int id) {
            if (!_rester.Enabled) {
                if (_status == SPECTRUM_HANDLER_STATUS.BLOCKED) {
                    _status = SPECTRUM_HANDLER_STATUS.RESTING;
                    _rester.Start();
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
