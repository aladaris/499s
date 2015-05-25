using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Midi;

namespace _499.InteractionHandlers {
    public class SystemHandler {
        OutputDevice _midiOut;
        Channel _channel;
        // Hadlers
        FlaresHandler _flares;
        SpectrumHandler _spectrums;

        public SystemHandler(int midi_out_id, Channel channel) {
            if ((midi_out_id >= 0) && (midi_out_id < OutputDevice.InstalledDevices.Count)) {
                _midiOut = OutputDevice.InstalledDevices[midi_out_id];
                _midiOut.Open();
            } else
                throw new IndexOutOfRangeException("Index must be grater than zero and less than " + OutputDevice.InstalledDevices.Count.ToString());
            _channel = channel;
            // FLARES
            _flares = new FlaresHandler(3, 3, 2, 2);
            _flares.LeftVideoClips[0] = new VideoClip(0, "Flare_L_0", 1, 0, Pitch.A1);
            _flares.LeftVideoClips[1] = new VideoClip(1, "Flare_L_1", 1, 0, Pitch.A2);
            _flares.LeftVideoClips[2] = new VideoClip(2, "Flare_L_2", 1, 0, Pitch.A3);
            _flares.RightVideoClips[0] = new VideoClip(0, "Flare_R_0", 1, 0, Pitch.B1);
            _flares.RightVideoClips[1] = new VideoClip(1, "Flare_R_1", 1, 0, Pitch.B2);
            _flares.RightVideoClips[2] = new VideoClip(2, "Flare_R_2", 1, 0, Pitch.B3);
            _flares.OnVideoClipPlay += OnVideoClipPlay;
            // SPECTRUMS
            _spectrums = new SpectrumHandler(3, 1500, 500, 800, 1000);  // NOTE: Un knob a menos de 500 ms, se vuelve loco??
            _spectrums.Spectrums[0] = new Spectrum(0, 2, Control.Volume);
            _spectrums.Spectrums[1] = new Spectrum(1, 3, Control.TremoloLevel);
            _spectrums.Spectrums[2] = new Spectrum(2, 4, Control.SustainPedal);
            _spectrums.SendControlChange += OnControlChange;
        }

        #region Properties
        public int MidiOutPutId {
            set {
                if ((value > 0) && (value < OutputDevice.InstalledDevices.Count)) {
                    if (_midiOut.IsOpen)
                        _midiOut.Close();
                    _midiOut = OutputDevice.InstalledDevices[value];
                    _midiOut.Open();
                }

            }
        }
        #endregion

        #region Flares
        public bool NewUserFlares(FLARE_SIDE side) {
            return _flares.NewUser(side);
        }
        public bool RemoveUserFlares() {
            return _flares.RemoveUser();
        }
        #endregion

        #region Spectrums
        public bool NewUserSpectrums() {
            return _spectrums.NewUser();
        }
        public bool RemoveUserSpectrums() {
            return _spectrums.RemoveUser();
        }
        #endregion

        #region EventHandlers
        private void OnVideoClipPlay(Pitch note) {
            if (_midiOut != null)
                if (_midiOut.IsOpen)
                    _midiOut.SendNoteOn(_channel, note, 127);
        }

        private void OnControlChange(Control control, int value) {
            if (_midiOut != null)
                if (_midiOut.IsOpen)
                    _midiOut.SendControlChange(_channel, control, value);
        }
        #endregion

    }
}
