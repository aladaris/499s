using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Midi;

using _499.InteractionHandlers;

namespace ManualGestureTrigger {
    public partial class MainForm : Form {

        public FlaresHandler flares = new FlaresHandler(3, 3, 2, 2);

        public MainForm() {
            InitializeComponent();
            LoadMidiOutDevicesComboBox();

            flares.LeftVideoClips[0] = new VideoClip(0, "Flare_L_0", 1, 0, Pitch.A1);
            flares.LeftVideoClips[1] = new VideoClip(1, "Flare_L_1", 1, 0, Pitch.A2);
            flares.LeftVideoClips[2] = new VideoClip(2, "Flare_L_2", 1, 0, Pitch.A3);
            flares.RightVideoClips[0] = new VideoClip(0, "Flare_R_0", 1, 0, Pitch.B1);
            flares.RightVideoClips[1] = new VideoClip(1, "Flare_R_1", 1, 0, Pitch.B2);
            flares.RightVideoClips[2] = new VideoClip(2, "Flare_R_2", 1, 0, Pitch.B3);
            flares.OnVideoClipPlay += OnVideoClipPlay;


        }


        private void OnVideoClipPlay(Pitch note) {
            // TODO: Aquí envía el midi
            return;
        }

        private void LoadMidiOutDevicesComboBox() {
            for (int i = 0; i < OutputDevice.InstalledDevices.Count; i++) {
                cb_MidiOutDevices.Items.Add(OutputDevice.InstalledDevices[i].Name);
            }
        }

        private void MainForm_Load(object sender, EventArgs e) {
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e) {
        }

        private void cb_MidiOutDevices_SelectedIndexChanged(object sender, EventArgs e) {
            MessageBox.Show(cb_MidiOutDevices.SelectedIndex.ToString());  // DEBUG
        }

        private void LeftHandUpHandler(object sender, EventArgs e) {
            CheckBox cb = sender as CheckBox;
            if (cb != null) {
                if (cb.Checked) {
                    flares.NewUser(FLARE_SIDE.LEFT);
                    // GUI
                    _499.InteractionHandlers.Utils.DisableAllButMe(cb, typeof(CheckBox));
                } else {
                    flares.RemoveUser();
                    // GUI
                    _499.InteractionHandlers.Utils.EnableAllButMe(cb, typeof(CheckBox));
                }
            }
        }

        

    }
}
