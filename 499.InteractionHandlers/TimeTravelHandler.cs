using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _499.InteractionHandlers {

    public enum TT_SIDE {LEFT, RIGHT }
    public enum TT_STATUS { REWIND, IDLE, FASTFORWARD, SPEEDING_UP, SPEEDING_DOWN, CHANGING_DIRECTION }

    public class TimeTravelHandler {
        private readonly int _maxUsers;
        private int _nUsersRewind = 0;
        private int _nUsersFForward = 0;
        private TT_STATUS _prevStatus = TT_STATUS.IDLE;
        private TT_STATUS _status = TT_STATUS.IDLE;
        private byte _currentSpeed = 0;
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
        public int TotalCount { get { return _nUsersFForward - _nUsersRewind; } }
        public Midi.Control CCValue { get; set; }
        public Midi.Pitch FFMidinote { get; set; }
        public Midi.Pitch RewMidinote { get; set; }

        public TimeTravelHandler(Midi.Control cc_value, int max_users, byte idle_speed_value = 25, int speed_change_duration = 500, int direction_change_duration = 300, Midi.Pitch rewind = Midi.Pitch.D0, Midi.Pitch fforward = Midi.Pitch.D1) {
            _maxUsers = max_users;
            _speedDelta = (byte)(_maxSpeed / (byte)_maxUsers);
            _idleSpeed = idle_speed_value;
            _directionChangeDuration = direction_change_duration;
            _speedChangeDuration = speed_change_duration;
            CCValue = cc_value;
            _knobSpeed = new MidiKnob(0, CCValue, _minSpeed, _maxSpeed, _speedChangeDuration);
            _knobSpeed.SendMidiControlChange += SendMidiControlChange;
            RewMidinote = rewind;
            FFMidinote = fforward;
        }

        /// <summary>
        /// Devuelve el sistema al estado actual.
        /// </summary>
        public void Reset() {
            _currentSpeed = 0;
            GoIdle();
            _prevStatus = TT_STATUS.IDLE;
            _nUsersFForward = 0;
            _nUsersRewind = 0;
        }

        public void GoIdle() {
            if ((_status == TT_STATUS.IDLE) || (_status == TT_STATUS.FASTFORWARD)) {
                if (!_knobSpeed.IsRunning) {
                    _knobSpeed.Duration = _speedChangeDuration / 2;
                    _knobSpeed.SetRange(_currentSpeed, _idleSpeed);
                    _knobSpeed.KnobEndRunning += EndSpeedChanging;
                    _status = TT_STATUS.SPEEDING_UP;
                    _knobSpeed.Start();
                }
            }
            if (SendMidiOn != null)
                SendMidiOn(FFMidinote);
            _currentSpeed = _idleSpeed;
            _status = TT_STATUS.IDLE;
        }

        public bool NewUser(TT_SIDE side) {
            if (!_knobSpeed.IsRunning) {
                switch (_status) {
                    case TT_STATUS.IDLE:
                        switch (side) {
                            case TT_SIDE.LEFT:
                                _nUsersRewind++;
                                if (UserNum <= _maxUsers) {
                                    ChangeDirection();
                                    return true;
                                }
                                break;
                            case TT_SIDE.RIGHT:
                                _nUsersFForward++;
                                if (UserNum <= _maxUsers) {
                                    _prevStatus = TT_STATUS.FASTFORWARD;  // Al finalinar el knob, se volverá a este estado
                                    _status = TT_STATUS.SPEEDING_UP;
                                    if (!_knobSpeed.IsRunning) {
                                        _knobSpeed.Duration = _speedChangeDuration;
                                        _knobSpeed.SetRange(_currentSpeed, GetNewSpeed());
                                        _knobSpeed.KnobEndRunning += EndSpeedChanging;
                                        _knobSpeed.Start();
                                        return true;
                                    }
                                }
                                break;
                        }
                        break;
                    case TT_STATUS.FASTFORWARD:
                        switch (side) {
                            case TT_SIDE.LEFT:
                                _nUsersRewind++;
                                if (UserNum <= _maxUsers) {
                                    if (TotalCount > 0) {  // Keep going forward, but slower
                                        _prevStatus = _status;
                                        _status = TT_STATUS.SPEEDING_DOWN;
                                        if (!_knobSpeed.IsRunning) {
                                            _knobSpeed.Duration = _speedChangeDuration;
                                            _knobSpeed.SetRange(_currentSpeed, GetNewSpeed());
                                            _knobSpeed.KnobEndRunning += EndSpeedChanging;
                                            _knobSpeed.Start();
                                            return true;
                                        }
                                    } else if (TotalCount == 0) {  // Go to IDLE
                                        GoIdle();
                                        return true;
                                    } else if (TotalCount < 0) {
                                        ChangeDirection();
                                        return true;
                                    }
                                }
                                break;
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
                                }
                                break;
                            case TT_SIDE.RIGHT:
                                _nUsersFForward++;
                                if (UserNum <= _maxUsers) {
                                    if (TotalCount < 0) {  // Keep going backwards, but slower
                                        _prevStatus = _status;
                                        _status = TT_STATUS.SPEEDING_DOWN;
                                        if (!_knobSpeed.IsRunning) {
                                            _knobSpeed.Duration = _speedChangeDuration;
                                            _knobSpeed.SetRange(_currentSpeed, GetNewSpeed());
                                            _knobSpeed.KnobEndRunning += EndSpeedChanging;
                                            _knobSpeed.Start();
                                            return true;
                                        }
                                    } else if (TotalCount == 0) {  // Go to IDLE
                                        ChangeDirection();
                                        return true;
                                    } else if (TotalCount > 0) {
                                        ChangeDirection();
                                        return true;
                                    }
                                }
                                break;
                        }
                        break;
                }
                // Reset en caso de que se sigan añadiendo usuarios sin quitar más.
                if (UserNum > (_maxUsers * 2)) {
                    Reset();
                }
            }
            return false;
        }

        public bool RemoveUser(TT_SIDE side) {
            if (!_knobSpeed.IsRunning) {
                int prevTotalCount;
                switch (_status) {
                    case TT_STATUS.IDLE:
                        prevTotalCount = TotalCount;
                        switch (side) {
                            case TT_SIDE.LEFT: _nUsersRewind--; break;
                            case TT_SIDE.RIGHT: _nUsersFForward--; break;
                        }
                        if (UserNum >= 0) {
                            if (TotalCount < 0) {
                                ChangeDirection();
                                return true;
                            } else if (TotalCount > 0) {
                                _prevStatus = TT_STATUS.FASTFORWARD;  // Al finalinar el knob, se volverá a este estado
                                // Calculamos si vamos a incrementar o disminuir la velocidad
                                if (prevTotalCount < TotalCount)
                                    _status = TT_STATUS.SPEEDING_UP;
                                else if (prevTotalCount > TotalCount)
                                    _status = TT_STATUS.SPEEDING_DOWN;

                                if (!_knobSpeed.IsRunning) {
                                    _knobSpeed.Duration = _speedChangeDuration;
                                    _knobSpeed.SetRange(_currentSpeed, GetNewSpeed());
                                    _knobSpeed.KnobEndRunning += EndSpeedChanging;
                                    _knobSpeed.Start();
                                    return true;
                                }
                            }
                        }
                        break;
                    case TT_STATUS.FASTFORWARD:
                        prevTotalCount = TotalCount;
                        switch (side) {
                            case TT_SIDE.LEFT: _nUsersRewind--; break;
                            case TT_SIDE.RIGHT: _nUsersFForward--; break;
                        }
                        if (UserNum >= 0) {
                            if (TotalCount == 0) {
                                GoIdle();
                                return true;
                            } else if (TotalCount > 0) {
                                if (prevTotalCount < TotalCount)
                                    _status = TT_STATUS.SPEEDING_UP;
                                else if (prevTotalCount > TotalCount)
                                    _status = TT_STATUS.SPEEDING_DOWN;

                                if (!_knobSpeed.IsRunning) {
                                    _knobSpeed.Duration = _speedChangeDuration;
                                    _knobSpeed.SetRange(_currentSpeed, GetNewSpeed());
                                    _knobSpeed.KnobEndRunning += EndSpeedChanging;
                                    _knobSpeed.Start();
                                    return true;
                                }
                            }
                        }
                        break;

                    case TT_STATUS.REWIND:
                        prevTotalCount = TotalCount;
                        switch (side) {
                            case TT_SIDE.LEFT: _nUsersRewind--; break;
                            case TT_SIDE.RIGHT: _nUsersFForward--; break;
                        }
                        if (UserNum >= 0) {
                            if (TotalCount == 0) {
                                ChangeDirection();
                                return true;
                            } else if (TotalCount < 0) {
                                if (prevTotalCount > TotalCount)
                                    _status = TT_STATUS.SPEEDING_UP;
                                else if (prevTotalCount < TotalCount)
                                    _status = TT_STATUS.SPEEDING_DOWN;

                                _prevStatus = TT_STATUS.REWIND;  // Al finalinar el knob, se volverá a este estado
                                if (!_knobSpeed.IsRunning) {
                                    _knobSpeed.Duration = _speedChangeDuration;
                                    _knobSpeed.SetRange(_currentSpeed, GetNewSpeed());
                                    _knobSpeed.KnobEndRunning += EndSpeedChanging;
                                    _knobSpeed.Start();
                                    return true;
                                }
                            }
                        }
                        break;
                }
                // Reset en caso de que se sigan eliminando usuarios sin poner más.
                if (UserNum < 0) {
                    Reset();
                }
            }
            return false;
        }


        private void EndSpeedChanging(int id) {
            if ((_status == TT_STATUS.SPEEDING_UP)||(_status == TT_STATUS.SPEEDING_DOWN)) {
                _knobSpeed.KnobEndRunning -= EndSpeedChanging;
                _currentSpeed = GetNewSpeed();
                _status = _prevStatus;
            }
        }

        private void ChangeDirection() {
            if ((_status == TT_STATUS.REWIND) || (_status == TT_STATUS.IDLE)) {
                if (!_knobSpeed.IsRunning) {
                    _prevStatus = _status;
                    _status = TT_STATUS.CHANGING_DIRECTION;
                    _knobSpeed.Duration = _directionChangeDuration;
                    _knobSpeed.SetRange(_currentSpeed, 0);
                    _knobSpeed.KnobEndRunning += ChangeDirectionSlowDownEnd;
                    _knobSpeed.Start();
                }
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
            }
        }



        private void SendMidiControlChange(Midi.Control control, int value) {
            if (SendControlChange != null)
                SendControlChange(control, value);
        }

        private byte GetNewSpeed() {
            int newSpeed = 0;
            if (_status == TT_STATUS.SPEEDING_UP) {
                newSpeed = _currentSpeed + _speedDelta;
                if (newSpeed > _maxSpeed)
                    newSpeed = _maxSpeed;
            } else if (_status == TT_STATUS.SPEEDING_DOWN) {
                newSpeed = _currentSpeed - _speedDelta;
                if (newSpeed < _minSpeed)
                    newSpeed = _minSpeed;
            }
            return (byte)newSpeed;
        }

    }
}
