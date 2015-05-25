using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _499.InteractionHandlers {

    public enum TT_SIDE {LEFT, RIGHT }
    public enum TT_STATUS { REWIND, IDLE, SPEEDING_UP, CHANGING_DIRECTION, FASTFORWARD }

    public class TimeTravelHandler {
        private readonly int _maxUsers;
        private int _nUsersRewind = 0;
        private int _nUsersFForward = 0;
        private TT_STATUS _prevStatus = TT_STATUS.IDLE;
        private TT_STATUS _status = TT_STATUS.IDLE;
        private byte _currentSpeed;
        private const byte _minSpeed = 0;
        private readonly byte _idleSpeed;
        private const byte _maxSpeed = 127;
        private readonly byte _speedDelta;
        private MidiKnob _knobSpeed;
        private readonly int _directionChangeDuration;
        private readonly int _speedChangeDuration;
        // events
        public event SendMidiControlChangeHandler SendControlChange;
        public event PlayVideoClipHandler SendMidiOn;  // TODO: Refactor el nombre. Algo como Send MidiOn

        public int UserNum { get { return _nUsersRewind + _nUsersFForward; } }
        public Midi.Control CCValue { get; set; }
        public Midi.Pitch FFMidinote { get; set; }
        public Midi.Pitch RewMidinote { get; set; }

        public TimeTravelHandler(Midi.Control cc_value, int max_users, byte idle_speed_value = 25, int speed_change_duration = 500, int direction_change_duration = 300, Midi.Pitch rewind = Midi.Pitch.D0, Midi.Pitch fforward = Midi.Pitch.D1) {
            _maxUsers = max_users;
            _speedDelta = (byte)(_maxSpeed / (byte)_maxUsers);
            _idleSpeed = idle_speed_value;
            _currentSpeed = _idleSpeed;
            _directionChangeDuration = direction_change_duration;
            _speedChangeDuration = speed_change_duration;
            CCValue = cc_value;
            _knobSpeed = new MidiKnob(0, CCValue, _minSpeed, _maxSpeed, _speedChangeDuration);
            _knobSpeed.SendMidiControlChange += SendMidiControlChange;
            RewMidinote = rewind;
            FFMidinote = fforward;
        }

        public void GoIdle() {
            // TODO: Salir de cualquier estado apra ir a IDLE
            switch (_status) {
                case TT_STATUS.IDLE:
                    if (!_knobSpeed.IsRunning) {
                        _knobSpeed.Duration = _speedChangeDuration / 2;
                        _knobSpeed.SetRange(0, _idleSpeed);
                        _knobSpeed.KnobEndRunning += EndSpeedChanging;
                        _knobSpeed.Start();
                    }
                    break;
                case TT_STATUS.FASTFORWARD:
                    if (!_knobSpeed.IsRunning) {
                        _nUsersFForward = 0;
                        _knobSpeed.Duration = _speedChangeDuration / 2;
                        _knobSpeed.SetRange(0, _idleSpeed);
                        _knobSpeed.KnobEndRunning += EndSpeedChanging;
                        _knobSpeed.Start();
                    }
                    break;
            }
            if (SendMidiOn != null)
                SendMidiOn(FFMidinote);
            _currentSpeed = _idleSpeed;
            _status = TT_STATUS.IDLE;
        }

        public bool NewUser(TT_SIDE side) {
            switch (_status) {
                case TT_STATUS.IDLE:
                    switch (side) {
                        case TT_SIDE.LEFT:
                            _nUsersRewind++;
                            ChangeDirection();
                            break;
                        case TT_SIDE.RIGHT:
                            _prevStatus = _status;
                            _status = TT_STATUS.SPEEDING_UP;
                            _nUsersFForward++;
                            if (!_knobSpeed.IsRunning) {
                                _knobSpeed.Duration = _speedChangeDuration;
                                _knobSpeed.SetRange(_currentSpeed, GetNewSpeed());
                                _knobSpeed.KnobEndRunning += EndSpeedChanging;
                                _knobSpeed.Start();
                                return true;
                            }
                            break;
                    }
                    break;
                case TT_STATUS.FASTFORWARD:
                    switch (side) {
                        case TT_SIDE.LEFT: break;
                        case TT_SIDE.RIGHT:
                            _nUsersFForward++;
                            if (UserNum <= _maxUsers) {
                                _prevStatus = _status;
                                _status = TT_STATUS.SPEEDING_UP;
                                if (!_knobSpeed.IsRunning) {
                                    _knobSpeed.Duration = _speedChangeDuration;
                                    _knobSpeed.SetRange(_currentSpeed, GetNewSpeed());
                                    _knobSpeed.KnobEndRunning += EndSpeedChanging;
                                    _knobSpeed.Start();
                                    return true;
                                }
                            } else {
                                _nUsersFForward--;
                                return false;
                            }
                            break;
                    }
                    break;

                case TT_STATUS.REWIND:
                    switch (side) {
                        case TT_SIDE.LEFT:
                            _nUsersRewind++;
                            if (UserNum <= _maxUsers) {
                                _prevStatus = _status;
                                _status = TT_STATUS.SPEEDING_UP;
                                if (!_knobSpeed.IsRunning) {
                                    _knobSpeed.Duration = _speedChangeDuration;
                                    _knobSpeed.SetRange(_currentSpeed, GetNewSpeed());
                                    _knobSpeed.KnobEndRunning += EndSpeedChanging;
                                    _knobSpeed.Start();
                                    return true;
                                }
                            } else {
                                _nUsersRewind--;
                                return false;
                            }
                            break;
                        case TT_SIDE.RIGHT: break;
                    }
                    break;
            }
            return false;
        }


        private void EndSpeedChanging(int id) {
            if (_status == TT_STATUS.SPEEDING_UP) {
                _status = _prevStatus;
                _knobSpeed.KnobEndRunning -= EndSpeedChanging;
                _currentSpeed = GetNewSpeed();
            }
        }

        private void ChangeDirection() {
            switch (_status) {
                case TT_STATUS.IDLE:
                    if (!_knobSpeed.IsRunning) {
                        _prevStatus = _status;
                        _status = TT_STATUS.CHANGING_DIRECTION;
                        _knobSpeed.Duration = _directionChangeDuration;
                        _knobSpeed.SetRange(_currentSpeed, 0);
                        _knobSpeed.KnobEndRunning += ChangeDirectionSlowDownEnd;
                        _knobSpeed.Start();
                    }
                    break;
            }
        }

        private void ChangeDirectionSlowDownEnd(int id) {
            if (_status == TT_STATUS.CHANGING_DIRECTION) {
                switch (_prevStatus) {
                    case TT_STATUS.IDLE:
                        if (SendMidiOn != null)
                            SendMidiOn(RewMidinote);  // Mandamos el comando de cambio de sentido
                        break;
                    case TT_STATUS.REWIND:
                        if (SendMidiOn != null)
                            SendMidiOn(FFMidinote);
                        break;
                }
                if (!_knobSpeed.IsRunning) {
                    _knobSpeed.Duration = _directionChangeDuration;
                    _knobSpeed.SetRange(0, _idleSpeed);
                    _knobSpeed.KnobEndRunning -= ChangeDirectionSlowDownEnd;
                    _knobSpeed.KnobEndRunning += ChangeDirectionSpeedUpEnd;
                    _knobSpeed.Start();
                }
            }
        }

        private void ChangeDirectionSpeedUpEnd(int id) {
            if (_status == TT_STATUS.CHANGING_DIRECTION) {
                _knobSpeed.KnobEndRunning -= ChangeDirectionSpeedUpEnd;
                switch (_prevStatus) {
                    case TT_STATUS.IDLE:
                        _status = TT_STATUS.REWIND;
                        break;
                    case TT_STATUS.REWIND:
                        _status = TT_STATUS.IDLE;
                        break;
                }
                // TODO: Manejo de cambios durante el CHANGING_DIRECTION
            }
        }



        private void SendMidiControlChange(Midi.Control control, int value) {
            if (SendControlChange != null)
                SendControlChange(control, value);
        }

        private byte GetNewSpeed() {
            int newSpeed = _currentSpeed + _speedDelta;
            if (newSpeed > _maxSpeed)
                newSpeed = _maxSpeed;
            return (byte)newSpeed;
        }

    }
}
