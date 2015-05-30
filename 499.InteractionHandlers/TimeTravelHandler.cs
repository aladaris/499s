using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _499.InteractionHandlers {

    public enum TT_SIDE {LEFT, RIGHT }
    public enum TT_STATUS { REWIND, IDLE, FASTFORWARD, SPEEDING_UP, SPEEDING_DOWN, CHANGING_DIRECTION }

    /// <summary>
    /// Generado cada vez que un TimeTraveller llega completa un ciclo de reloj establecido.
    /// </summary>
    /// <param name="traveller">Triggering time traveller.</param>
    public delegate void TimeTravellerTimedOutHandler (TimeTraveller traveller);

    public class TimeTraveller : IDisposable {
        private static int TIMEOUT_VALUE = 10000;
        private static int RETRY_VALUE = 500;
        public static int MAX_RETRIES = 10;
        private Timer _timeOut;
        public TT_SIDE Side { get; set; }
        public int Retries { get; set; }
        // events
        public event TimeTravellerTimedOutHandler OnTimeOut;

        public TimeTraveller(TT_SIDE side) {
            Side = side;
            Retries = 0;
            _timeOut = new Timer(TIMEOUT_VALUE);
            _timeOut.Elapsed += TimeOutEnded;
            _timeOut.AutoReset = false;
            _timeOut.Start();
        }

        public void StartRemoval() {
            _timeOut.Stop();
            _timeOut.Interval = RETRY_VALUE;
            _timeOut.AutoReset = false;
            _timeOut.Start();
        }

        public void Dispose(){
            _timeOut.Stop();
            _timeOut.Dispose();
        }

        private void TimeOutEnded(object sender, ElapsedEventArgs e) {
            if (OnTimeOut != null) {
                OnTimeOut(this);
            }
        }

    }

    public class TimeTravelHandler {
        private readonly int _maxUsers;
        private Queue<TimeTraveller> _usersRewind;
        private Queue<TimeTraveller> _usersFastForward;
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

        public int UserNum { get { return _usersRewind.Count + _usersFastForward.Count; } }
        public int TotalCount { get { return _usersFastForward.Count - _usersRewind.Count; } }
        public Midi.Control CCValue { get; set; }
        public Midi.Pitch FFMidinote { get; set; }
        public Midi.Pitch RewMidinote { get; set; }
        public bool Working { get; set; }
        // TODO: Working timeout. Si esta N tiempo en Working, Resetear el TimeTravel.

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
            Working = false;
            _usersRewind = new Queue<TimeTraveller>();
            _usersFastForward = new Queue<TimeTraveller>();
            GoIdle();
        }

        /// <summary>
        /// Devuelve el sistema al estado actual.
        /// </summary>
        public void Reset() {
            _currentSpeed = 0;
            GoIdle();
            _prevStatus = TT_STATUS.IDLE;
            // TODO: Mirar este clear y los timers de los TimeTravellers
            while (_usersFastForward.Count > 0) {
                _usersFastForward.Dequeue().Dispose();
            }
            _usersFastForward.Clear();
            while (_usersRewind.Count > 0) {
                _usersRewind.Dequeue().Dispose();
            }
            _usersRewind.Clear();
        }

        /// <summary>
        /// Goes (smoothly) to the IDLE state, no matter what.
        /// </summary>
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

        #region Add and remove Users
        /// <summary>
        /// Adds a user to the system.
        /// If the handler is Working, the user isn't added at all.
        /// </summary>
        /// <param name="side">Side of the pose.</param>
        /// <returns>True is the user was added to the handler.</returns>
        public bool NewUser(TT_SIDE side) {
            if (!Working) {
                TimeTraveller nUser = new TimeTraveller(side);
                switch (side) {
                    case TT_SIDE.LEFT: _usersRewind.Enqueue(nUser); break;
                    case TT_SIDE.RIGHT: _usersFastForward.Enqueue(nUser); break;
                }
                nUser.OnTimeOut += TimeTravellerTimedOut;
                return TriggerNewUserInteraction(side);
            }
            return false;

        }

        private bool TriggerNewUserInteraction(TT_SIDE side) {
            if (!_knobSpeed.IsRunning) {
                switch (_status) {
                    case TT_STATUS.IDLE:
                        switch (side) {
                            case TT_SIDE.LEFT:
                                if (UserNum <= _maxUsers) {
                                    ChangeDirection();
                                    return true;
                                }
                                break;
                            case TT_SIDE.RIGHT:
                                if (UserNum <= _maxUsers) {
                                    _status = TT_STATUS.FASTFORWARD;  // Trick para establecer a FASTFORWARD después de aumentar la velocidad
                                    return IncreaseSpeed();
                                }
                                break;
                        }
                        break;
                    case TT_STATUS.FASTFORWARD:
                        switch (side) {
                            case TT_SIDE.LEFT:
                                if (UserNum <= _maxUsers) {
                                    if (TotalCount > 0) {  // Keep going forward, but slower
                                        return DecreaseSpeed();
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
                                if (UserNum <= _maxUsers) {
                                    return IncreaseSpeed();
                                }
                                break;
                        }
                        break;

                    case TT_STATUS.REWIND:
                        switch (side) {
                            case TT_SIDE.LEFT:
                                if (UserNum <= _maxUsers) {
                                    return IncreaseSpeed();
                                }
                                break;
                            case TT_SIDE.RIGHT:
                                if (UserNum <= _maxUsers) {
                                    if (TotalCount < 0) {  // Keep going backwards, but slower
                                        return DecreaseSpeed();
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
                if (UserNum > _maxUsers) {
                    Reset();
                }
            }
            return false;
        }

        /// <summary>
        /// Removes a user from the handler.
        /// If the handler is working, the removed user will be put on pedding
        /// with its timeout timer on Retry mode.
        /// </summary>
        /// <param name="side">Pose side.</param>
        /// <returns>True if the player was removed.</returns>
        public bool RemoveUser(TT_SIDE side) {
            TimeTraveller user = null;
            switch (side) {
                case TT_SIDE.LEFT:
                    if (_usersRewind.Count > 0) {
                        try {
                            user = _usersRewind.Dequeue();
                        } catch (InvalidOperationException) {
                            return false;
                        }
                    }
                    break;
                case TT_SIDE.RIGHT:
                    if (_usersFastForward.Count > 0) {
                        try {
                            user = _usersFastForward.Dequeue();
                        } catch (InvalidOperationException) {
                            return false;
                        }
                    }
                    break;
            }
            if (user != null) {
                if (TriggerRemoveUserInteraction(user.Side)) {
                    user.Dispose();
                } else {
                    user.StartRemoval();
                }
            }
            return false;
        }

        public bool TriggerRemoveUserInteraction(TT_SIDE side) {
            if ((!Working) && (!_knobSpeed.IsRunning)) {
                switch (_status) {
                    case TT_STATUS.IDLE:
                        if (UserNum >= 0) {
                            if (TotalCount < 0) {
                                ChangeDirection();
                                return true;
                            } else if (TotalCount > 0) {
                                _status = TT_STATUS.FASTFORWARD;    // Trick para establecer a FASTFORWARD después de aumentar la velocidad
                                // Calculamos si vamos a incrementar o disminuir la velocidad
                                if (side == TT_SIDE.LEFT)
                                    return IncreaseSpeed();
                                else if (side == TT_SIDE.RIGHT)
                                    return DecreaseSpeed();
                            }
                        }
                        break;
                    case TT_STATUS.FASTFORWARD:
                        if (UserNum >= 0) {
                            if (TotalCount == 0) {
                                GoIdle();
                                return true;
                            } else if (TotalCount > 0) {
                                // Calculamos si vamos a incrementar o disminuir la velocidad
                                if (side == TT_SIDE.LEFT)
                                    return IncreaseSpeed();
                                else if (side == TT_SIDE.RIGHT)
                                    return DecreaseSpeed();
                            }
                        }
                        break;

                    case TT_STATUS.REWIND:
                        if (UserNum >= 0) {
                            if (TotalCount == 0) {
                                ChangeDirection();
                                return true;
                            } else if (TotalCount < 0) {
                                // Calculamos si vamos a incrementar o disminuir la velocidad
                                if (side == TT_SIDE.RIGHT)
                                    return IncreaseSpeed();
                                else if (side == TT_SIDE.LEFT)
                                    return DecreaseSpeed();
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
        #endregion

        #region TimeTravellers Timeout
        /// <summary>
        /// Aqui se entra si se llega a la condición de timeout.
        /// Si se puede eliminar el usuario, lo hacemos; sino, se pone en modo retry.
        /// Se repercuten los cambios en las colas de usuarios.
        /// </summary>
        /// <param name="traveller">Trigerring time traveller.</param>
        private void TimeTravellerTimedOut(TimeTraveller traveller) {
            try {
                switch (traveller.Side) {
                    case TT_SIDE.LEFT: _usersRewind.Dequeue(); break;
                    case TT_SIDE.RIGHT: _usersFastForward.Dequeue(); break;
                }
            } catch (InvalidOperationException) {
                traveller.Dispose();
                return;
            }

            if (!Working) {
                if (TriggerRemoveUserInteraction(traveller.Side)) {
                    traveller.Dispose();
                    return;
                }
            }
            traveller.StartRemoval();
            traveller.OnTimeOut -= TimeTravellerTimedOut;
            traveller.OnTimeOut += TimeTravellerRetryRemoval;
        }

        /// <summary>
        /// Lanzado con cada "Elapsed" del timer en modo retry.
        /// Si se alcanza el máximo de Retries, se elimina directamente a 'traveller'.
        /// </summary>
        /// <param name="traveller">Trigerring time traveller.</param>
        private void TimeTravellerRetryRemoval(TimeTraveller traveller) {
            traveller.Retries++;
            if (traveller.Retries > TimeTraveller.MAX_RETRIES) {
                traveller.Dispose();
                return;
            }
            if (!Working) {
                if (TriggerRemoveUserInteraction(traveller.Side)) {
                    traveller.Dispose();
                    return;
                }
            }
            traveller.StartRemoval();
        }
        #endregion

        #region Change Speed
        private bool IncreaseSpeed() {
            Working = true;
            _prevStatus = _status;  // Al finalinar el knob, se volverá a este estado
            _status = TT_STATUS.SPEEDING_UP;
            return ChangeSpeed();
        }

        private bool DecreaseSpeed() {
            Working = true;
            _prevStatus = _status;  // Al finalinar el knob, se volverá a este estado
            _status = TT_STATUS.SPEEDING_DOWN;
            return ChangeSpeed();
        }

        private bool ChangeSpeed() {
            if (Working) {
                if (!_knobSpeed.IsRunning) {
                    _knobSpeed.Duration = _speedChangeDuration;
                    _knobSpeed.SetRange(_currentSpeed, GetNewSpeed());
                    _knobSpeed.KnobEndRunning += EndSpeedChanging;
                    _knobSpeed.Start();
                    return true;
                }
            }
            return false;
        }

        private void EndSpeedChanging(int id) {
            if (Working) {
                if ((_status == TT_STATUS.SPEEDING_UP) || (_status == TT_STATUS.SPEEDING_DOWN)) {
                    _knobSpeed.KnobEndRunning -= EndSpeedChanging;
                    _currentSpeed = GetNewSpeed();
                    _status = _prevStatus;
                    Working = false;
                }
            }
        }
        #endregion

        #region Change Direction
        private void ChangeDirection() {
            Working = true;
            //if (Working) {
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
            //}
        }

        private void ChangeDirectionSlowDownEnd(int id) {
            if (Working) {
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
                        default:
                            if (SendMidiOn != null)
                                SendMidiOn(RewMidinote);  // En caso de duda, reproducimos hacia adelante
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
        }

        private void ChangeDirectionSpeedUpEnd(int id) {
            if (Working) {
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
                Working = false;
            }
        }
        #endregion

        private void SendMidiControlChange(Midi.Control control, int value) {
            if (SendControlChange != null)
                SendControlChange(control, value);
        }

        /// <summary>
        /// Gets the corresponding speed based on the status.
        /// </summary>
        /// <returns>Speed, midi value [0-127]</returns>
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
