using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Midi;

namespace _499.InteractionHandlers {
    public class InteractionSystemHandler {
        private OutputDevice _midiOut;
        private Channel _channel;
        private const Pitch RESET_NOTE = Pitch.C9;
        // Hadlers
        private FlaresHandler _flares;
        private SpectrumHandler _spectrums;
        private TimeTravelHandler _timeTravel;
        private GlowHandler _glow;

        public InteractionSystemHandler(int midi_out_id, Channel channel) {
            if ((midi_out_id >= 0) && (midi_out_id < OutputDevice.InstalledDevices.Count)) {
                _midiOut = OutputDevice.InstalledDevices[midi_out_id];
                _midiOut.Open();
            } else
                throw new IndexOutOfRangeException("Index must be grater than zero and less than " + OutputDevice.InstalledDevices.Count.ToString());
            _channel = channel;
            // FLARES
            _flares = new FlaresHandler(6, 6, 2, 2);
            _flares.LeftVideoClips[0] = new VideoClip(0, "Flare_01", 20000, 7, Pitch.A1);
            _flares.LeftVideoClips[1] = new VideoClip(1, "Flare_02", 19500, 8, Pitch.A2);
            _flares.LeftVideoClips[2] = new VideoClip(2, "Flare_03", 24000, 8, Pitch.A3);
            _flares.LeftVideoClips[3] = new VideoClip(3, "Flare_04", 42000, 7, Pitch.A4);
            _flares.LeftVideoClips[4] = new VideoClip(4, "Flare_05", 14057, 7, Pitch.A5);
            _flares.LeftVideoClips[5] = new VideoClip(5, "Flare_06", 11987, 8, Pitch.A6);
            _flares.RightVideoClips[0] = new VideoClip(6, "Flare_07", 10000, 6, Pitch.B1);
            _flares.RightVideoClips[1] = new VideoClip(7, "Flare_08",  8719, 6, Pitch.B2);
            _flares.RightVideoClips[2] = new VideoClip(8, "Flare_10", 13250, 6, Pitch.B3);
            _flares.RightVideoClips[3] = new VideoClip(9, "Flare_11",  9250, 5, Pitch.B4);
            _flares.RightVideoClips[4] = new VideoClip(10, "Flare_12", 11500, 5, Pitch.B5);
            _flares.RightVideoClips[5] = new VideoClip(11, "Flare_13", 12365, 5, Pitch.B6);

            _flares.OnVideoClipPlay += OnMidiNote;
            // SPECTRUMS
            _spectrums = new SpectrumHandler(3, 2000, 500, 800, 1000);  // NOTE: Un knob a menos de 500 ms, se vuelve loco??
            _spectrums.Spectrums[0] = new Spectrum(0, 2, Control.Volume, Pitch.CSharp1, Pitch.DSharp1, CalculateHueMidiValue(0.36f));
            _spectrums.Spectrums[1] = new Spectrum(1, 3, Control.TremoloLevel, Pitch.CSharp2, Pitch.DSharp2, CalculateHueMidiValue(0.16f));
            _spectrums.Spectrums[2] = new Spectrum(2, 4, Control.SustainPedal, Pitch.CSharp3, Pitch.DSharp3, CalculateHueMidiValue(0f));
            _spectrums.SendControlChange += OnControlChange;
            _spectrums.SendMidiNote += OnMidiNote;
            // TIME TRAVEL
            _timeTravel = new TimeTravelHandler(Control.ModulationWheel, 6, 25, 150, 75);  // Valores dependientes de la configuracion de Resolume (Velocidad entre [0.5, 3])
            _timeTravel.SendControlChange += OnControlChange;
            _timeTravel.SendMidiOn += OnMidiNote;
            // GLOW
            _glow = new GlowHandler(Control.CelesteLevel, 300, Control.ChorusLevel, 200);
            _glow.SendControlChange += OnControlChange;
            _glow.SendMidiOn += OnMidiNote;

            this.Reset();
        }

        #region Properties
        public int MidiOutPutId {
            set {
                if ((value > 0) && (value < OutputDevice.InstalledDevices.Count)) {
                    if (_midiOut != null)
                        if (_midiOut.IsOpen)
                            _midiOut.Close();
                    _midiOut = OutputDevice.InstalledDevices[value];
                    _midiOut.Open();
                }

            }
        }
        #endregion

        public void Reset() {
            if (_midiOut != null) {
                if (_midiOut.IsOpen) {
                    _midiOut.Close();
                }
                _midiOut.Open();
            }
            OnMidiNote(RESET_NOTE);
            _flares.Reset();
            _glow.Reset();
            _spectrums.Reset();
            _timeTravel.Reset();
        }

        private byte CalculateHueMidiValue(float hue) {
            if ((hue >= 0f) && (hue <= 1f)) {
                return (byte)(Math.Round(hue * 127));
            }
            return 0;
        }

        public string GetHandlersStatus() {
            string texto = "FlaresHandler:\n";
            texto += "  UserNum = " + _flares.UserNum.ToString() + '\n';
            texto += "  PlayingLeft = " + _flares.PlayingLeft.ToString() + '\n';
            texto += "  PlayingRight = " + _flares.PlayingRight.ToString() + '\n';
            texto += "\nSpectrumHanlder:\n";
            texto += "  Status = ";
            switch (_spectrums.Status) {
                case SPECTRUM_HANDLER_STATUS.IDLE: texto += "IDLE\n"; break;
                case SPECTRUM_HANDLER_STATUS.BLOCKED: texto += "BLOCKED\n"; break;
                case SPECTRUM_HANDLER_STATUS.RESTING: texto += "RESTING\n"; break;
                case SPECTRUM_HANDLER_STATUS.SHOWING: texto += "SHOWING\n"; break;
            }
            texto += "\nTimeTravelHanlder:\n";
            texto += "  UserNum = " + _timeTravel.UserNum.ToString() + '\n';
            texto += "  TotalCount = " + _timeTravel.TotalCount.ToString() + '\n';
            texto += "  Working = " + _timeTravel.Working.ToString() + '\n';

            texto += "\nGlowHanlder:\n";
            texto += "  UserNum = " + _glow.UserNum.ToString() + '\n';
            texto += "  Status = ";
            switch (_glow.Status) {
                case GLOW_HANDLER_STATUS.IDLE: texto += "IDLE\n"; break;
                case GLOW_HANDLER_STATUS.BLOCKED: texto += "BLOCKED\n"; break;
                case GLOW_HANDLER_STATUS.SHOWING: texto += "SHOWING\n"; break;
            }

            return texto;
        }

        #region Flares
        public bool NewUserFlares(FLARE_SIDE side) {
            if ((_spectrums.Status == SPECTRUM_HANDLER_STATUS.IDLE)||(_spectrums.Status == SPECTRUM_HANDLER_STATUS.RESTING))
                if ((_timeTravel.Status == TT_STATUS.IDLE)&&(_timeTravel.TotalCount == 0))
                    return _flares.NewUser(side);
            return false;
        }
        public bool RemoveUserFlares() {
            return _flares.RemoveUser();
        }
        #endregion

        #region Spectrums
        public bool NewUserSpectrums() {
            if ((_flares.PlayingLeft == 0)&&(_flares.PlayingRight == 0))
                return _spectrums.NewUser();
            return false;
        }
        public bool RemoveUserSpectrums() {
            return _spectrums.RemoveUser();
        }
        #endregion

        #region Time Travel
        public bool NewUserTimeTravel(TT_SIDE side) {
            if ((_flares.PlayingLeft == 0) && (_flares.PlayingRight == 0))
                return _timeTravel.NewUser(side);
            return false;
        }
        public bool RemoveUserTimeTravel(TT_SIDE side) {
            if (_timeTravel.UserNum > 0)
                return _timeTravel.RemoveUser(side);
            return false;
        }
        #endregion

        #region Glow
        public bool NewUserGlow() {
            return _glow.NewUser();
        }
        public bool RemoveUserGlow() {
            return _glow.RemoveUser();
        }
        #endregion

        #region EventHandlers
        private void OnMidiNote(Pitch note) {
            if (_midiOut != null)
                if (_midiOut.IsOpen) {
                    _midiOut.SendNoteOn(_channel, note, 127);
                    _midiOut.SendNoteOff(_channel, note, 127);
                }
        }

        private void OnControlChange(Control control, int value) {
            if (_midiOut != null)
                if (_midiOut.IsOpen)
                    _midiOut.SendControlChange(_channel, control, value);
        }
        #endregion


    }
}
