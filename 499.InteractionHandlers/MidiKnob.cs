using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

using Midi;

namespace _499.InteractionHandlers {

    public delegate void SendMidiControlChangeHandler(Control control, int value);

    /// <summary>
    /// Clase que representa un knob (de un controlador), moviendose de un valor MIDI a otro
    /// de forma continua, durante un tiempo especificado.
    /// </summary>
    public class MidiKnob {
        private int _id;
        private OutputDevice _midiOut;
        private Control _ccValue;
        private byte _initVal;
        private byte _endVal;
        private byte _currentValue;
        private sbyte _upOrDown;  // -1 From higher to lower value. 1 From lower to higher value.
        private byte _stepsCount;  // Numero de pasos en el intervalo [_initVal, _endVal]
        private int _duration;  // Milisegundos
        private Timer _timer;
        // Events
        public delegate void KnobEndRunningHandler(int id);
        public event KnobEndRunningHandler KnobEndRunning;
        public event SendMidiControlChangeHandler SendMidiControlChange;

        /// <summary>
        /// Is the knob running (sending midi messages).
        /// </summary>
        public bool IsRunning {
            get {
                if (_timer.Enabled) {
                    if (_upOrDown < 0) {
                        if (_currentValue > _endVal)
                            return true;
                    } else if (_upOrDown > 0) {
                        if (_currentValue < _endVal)
                            return true;
                    }
                }
                return false;
            }
        }

        public Control CCValue { get { return _ccValue; } set { _ccValue = value; } }

        /// <param name="id">Knob id</param>
        /// <param name="midi_out">Midi out device</param>
        /// <param name="cc_value">CC parameter</param>
        /// <param name="init">Initial value to send</param>
        /// <param name="end">Final value to send</param>
        /// <param name="time">Running time in milliseconds</param>
        public MidiKnob(int id, Control cc_value, byte init, byte end, int time) {
            _id = id;
            //_midiOut = midi_out;
            _ccValue = cc_value;
            _initVal = init;
            _endVal = end;
            _currentValue = _initVal;
            _duration = time;
            // From lower to higher value
            if (_initVal < _endVal) {
                _stepsCount = (byte)(_endVal - _initVal);
                _upOrDown = 1;
                // From higher to lower value
            } else if (_endVal < _initVal) {
                _stepsCount = (byte)(_initVal - _endVal);
                _upOrDown = -1;
            } else
                throw new IndexOutOfRangeException("Start and end value can't be at the same value.");
            double timeDelta = _duration / (double)_stepsCount;
            _timer = new Timer(timeDelta);
        }

        /// <summary>
        /// Starts the knob action.
        /// </summary>
        public void Start() {
            //if (_midiOut != null) {
                _currentValue = _initVal;
                _timer.Elapsed += ClockTick;
                _timer.Start();
            //}
        }

        private void ClockTick(object sender, ElapsedEventArgs e) {
            //if (_midiOut.IsOpen) {
                //_midiOut.SendControlChange(Channel.Channel1, _ccValue, _currentValue);
            if (SendMidiControlChange != null)
                SendMidiControlChange(_ccValue, _currentValue);
            switch (_upOrDown) {
                case -1: _currentValue--; break;
                case 1: _currentValue++; break;
            }
            if (!IsRunning) {
                _timer.Stop();
                _timer.Elapsed -= ClockTick;
                if (KnobEndRunning != null) {
                    KnobEndRunning(_id);
                }
            }
            //}
        }


    }
}
