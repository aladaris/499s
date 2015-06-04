using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace _499.InteractionHandlers {

    public enum GLOW_HANDLER_STATUS {
        IDLE,    // No se está mostrando glow.
        BLOCKED, // Haciendo fadin o fadeout (o cualquier otra acción ininterrumpible).
        SHOWING  // Mostrando glow. Después del fadein y antes del  fadeout.
    }

    public class GlowHandler {
        private int _nUsers = 0;
        private const int MAXUSERS = 6;
        private MidiKnob _knobTransparency;
        private readonly Midi.Pitch _glowClipPlay;  // Note to play the glow clip in Resolume
        private readonly Midi.Pitch _glowClipStop;  // Note to stop the glow clip in Resolume
        private byte _currentTransparencyValue = 0;
        private byte _transparencyDelta = (byte)(128 / MAXUSERS);
        private Timer _loopUpdaterTimer;
        private GLOW_HANDLER_STATUS _status;
        private int _pendingUsers = 0;  // Usuarios añadidos/eliminados mientras se estaba en BLOCKED?

        public int UserNum {
            get { return _nUsers; }
            set {
                PrevUserNum = _nUsers;
                _nUsers = value;
            }
        }
        public int PrevUserNum { get; private set; }
        public GLOW_HANDLER_STATUS Status {
            get { return _status; }
            private set {
                switch (value) {
                    case GLOW_HANDLER_STATUS.IDLE:
                        _loopUpdaterTimer.Start();
                        break;
                    case GLOW_HANDLER_STATUS.SHOWING:
                        _loopUpdaterTimer.Start();
                        break;
                    case GLOW_HANDLER_STATUS.BLOCKED:
                        _loopUpdaterTimer.Stop();
                        break;
                }
                _status = value;
            }
        }
        // events
        public event SendMidiControlChangeHandler SendControlChange;
        public event SendMidiNoteHandler SendMidiOn;

        public GlowHandler(Midi.Control transparency_cc = Midi.Control.CelesteLevel, double transparency_duration = 300, double parameters1_duration = 200,Midi.Pitch glow_play_note = Midi.Pitch.G1, Midi.Pitch glow_stop_note = Midi.Pitch.G0) {
            _knobTransparency = new MidiKnob(0, transparency_cc, 0, 127, transparency_duration);
            _knobTransparency.KnobEndRunning += OnInitialTransparencyKnobEnd;
            _knobTransparency.SendMidiControlChange += SendMidiControlChange;
            _loopUpdaterTimer = new Timer(1000);
            _loopUpdaterTimer.Elapsed += LoopTimerTick;
            _glowClipPlay = glow_play_note;
            _glowClipStop = glow_stop_note;
            Status = GLOW_HANDLER_STATUS.IDLE;
        }

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <returns></returns>
        public bool NewUser() {
            UserNum++;
            _pendingUsers++;
            switch (Status) {
                // Glow Start
                case GLOW_HANDLER_STATUS.IDLE:
                    if (UserNum > 0) {
                        StartFadeIn();
                        return true;
                    }
                    break;
                case GLOW_HANDLER_STATUS.SHOWING:
                    return UpdateGlowLoop();
            }
            return false;
        }

        /// <summary>
        /// Removes a user
        /// </summary>
        /// <returns></returns>
        public bool RemoveUser() {
            UserNum = ((UserNum - 1) < 0) ? 0 : UserNum - 1;
            _pendingUsers++;
            switch (Status) {
                case GLOW_HANDLER_STATUS.SHOWING:
                    return UpdateGlowLoop();
            }
            return false;
        }

        public void Reset() {
            _status = GLOW_HANDLER_STATUS.BLOCKED;
            UserNum = 0;
            _knobTransparency.Stop();
            if (SendControlChange != null)
                SendControlChange(_knobTransparency.CCValue, 0);
            if (SendMidiOn != null)
                SendMidiOn(_glowClipStop);
            _status = GLOW_HANDLER_STATUS.IDLE;
        }

        /// <summary>
        /// Updates the glow transparency
        /// </summary>
        /// <returns></returns>
        private bool UpdateGlowLoop() {
            switch (Status) {
                case GLOW_HANDLER_STATUS.IDLE:
                    if (UserNum > 0)
                        StartFadeIn();
                    break;
                case GLOW_HANDLER_STATUS.SHOWING:
                    if (_nUsers > 0) {
                        SetGlowTransparency();
                    } else {
                        StartFadeOut();
                    }
                    break;
            }
            return false;
        }

        /// <summary>
        /// Tick, del controlador de Usuarios pendientes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoopTimerTick(object sender, ElapsedEventArgs e) {
            if (Status != GLOW_HANDLER_STATUS.BLOCKED) {
                if ((_pendingUsers != 0) || ((Status == GLOW_HANDLER_STATUS.IDLE) && (UserNum > 0))) {
                    if (UserNum > 0) {
                        UpdateGlowLoop();
                    } else {
                        StartFadeOut();
                    }
                }
                _pendingUsers = 0;
            }
        }

        #region Layer Transparency

        /// <summary>
        /// Gets the apropiated value for the layer transparency, according to the Usernumber
        /// </summary>
        /// <returns></returns>
        private byte GetCorrespondingTransparencyValue() {
            byte result = (byte)(_transparencyDelta * UserNum);
            return result;
        }

        /// <summary>
        /// Fade In
        /// </summary>
        private void StartFadeIn() {
            if (!_knobTransparency.IsRunning) {
                Status = GLOW_HANDLER_STATUS.BLOCKED;
                _pendingUsers--;
                if (SendMidiOn != null)
                    SendMidiOn(_glowClipPlay);
                _knobTransparency.KnobEndRunning += OnInitialTransparencyKnobEnd;
                _knobTransparency.SetRange(_currentTransparencyValue, GetCorrespondingTransparencyValue());
                _knobTransparency.Start();
            }
        }

        /// <summary>
        /// Fade Out
        /// </summary>
        private void StartFadeOut() {
            if (!_knobTransparency.IsRunning) {
                Status = GLOW_HANDLER_STATUS.BLOCKED;
                _pendingUsers--;
                _knobTransparency.KnobEndRunning += OnFinalTransparencyKnobEnd;
                _knobTransparency.SetRange(_currentTransparencyValue, GetCorrespondingTransparencyValue());
                _knobTransparency.Start();
            }
        }

        /// <summary>
        /// Set Transparency Amount
        /// </summary>
        private void SetGlowTransparency() {
            if (!_knobTransparency.IsRunning) {
                Status = GLOW_HANDLER_STATUS.BLOCKED;
                _pendingUsers--;
                _knobTransparency.KnobEndRunning += OnTransparencySetEnd;
                _knobTransparency.SetRange(_currentTransparencyValue, GetCorrespondingTransparencyValue());
                _knobTransparency.Start();
            }
        }

        /// <summary>
        /// End of Fade In proccess.
        /// </summary>
        /// <param name="id"></param>
        private void OnInitialTransparencyKnobEnd(int id) {
            _knobTransparency.KnobEndRunning -= OnInitialTransparencyKnobEnd;
            if (Status == GLOW_HANDLER_STATUS.BLOCKED) {
                _currentTransparencyValue = _knobTransparency.FinalValue;
                Status = GLOW_HANDLER_STATUS.SHOWING;
                // TODO: Este if ?????
                if (UserNum <= 0)
                    UpdateGlowLoop();
            }

        }

        /// <summary>
        /// End of Fade Out proccess.
        /// </summary>
        /// <param name="id"></param>
        private void OnFinalTransparencyKnobEnd(int id) {
            _knobTransparency.KnobEndRunning -= OnFinalTransparencyKnobEnd;
            if (Status == GLOW_HANDLER_STATUS.BLOCKED) {
                _currentTransparencyValue = _knobTransparency.FinalValue;
                if (SendMidiOn != null)
                    SendMidiOn(_glowClipStop);
                Status = GLOW_HANDLER_STATUS.IDLE;
            }
        }

        /// <summary>
        /// End of transparency amount setting proccess.
        /// </summary>
        /// <param name="id"></param>
        private void OnTransparencySetEnd(int id) {
            _knobTransparency.KnobEndRunning -= OnTransparencySetEnd;
            if (Status == GLOW_HANDLER_STATUS.BLOCKED) {
                _currentTransparencyValue = _knobTransparency.FinalValue;
                Status = GLOW_HANDLER_STATUS.SHOWING;
                if (UserNum <= 0) {
                    UpdateGlowLoop();
                }
            }
        }
        #endregion

        private void SendMidiControlChange(Midi.Control control, int value) {
            if (SendControlChange != null)
                SendControlChange(control, value);
        }

        
    }
}
