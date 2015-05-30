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

        public InteractionSystemHandler _mainHandler;

        public MainForm() {
            InitializeComponent();
            LoadMidiOutDevicesComboBox();
            _mainHandler = new InteractionSystemHandler(2, Channel.Channel1);
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
            _mainHandler.MidiOutPutId = ((ComboBox)sender).SelectedIndex;
        }

        private void HandUpHandler(object sender, EventArgs e) {
            CheckBox cb = sender as CheckBox;
            if (cb != null) {
                if (cb.Checked) {
                    switch (cb.Text[0]){
                        case 'L':
                            _mainHandler.NewUserFlares(FLARE_SIDE.LEFT); break;
                        case 'B':
                            _mainHandler.NewUserFlares(FLARE_SIDE.CENTER); break;
                        case 'R':
                            _mainHandler.NewUserFlares(FLARE_SIDE.RIGHT); break;
                    }
                    // GUI
                    _499.InteractionHandlers.Utils.DisableAllButMe(cb, typeof(CheckBox));
                } else {
                    _mainHandler.RemoveUserFlares();
                    // GUI
                    _499.InteractionHandlers.Utils.EnableAllButMe(cb, typeof(CheckBox));
                }
            }
        }

        private void AirHugHandler(object sender, EventArgs e) {
            CheckBox cb = sender as CheckBox;
            if (cb != null) {
                if (cb.Checked) {
                    _mainHandler.NewUserGlow();
                    // GUI
                    _499.InteractionHandlers.Utils.DisableAllButMe(cb, typeof(CheckBox));
                } else {
                    _mainHandler.RemoveUserGlow();
                    // GUI
                    _499.InteractionHandlers.Utils.EnableAllButMe(cb, typeof(CheckBox));
                }
            }
        }

        private void AerobicsHandler(object sender, EventArgs e) {
            CheckBox cb = sender as CheckBox;
            if (cb != null) {
                if (cb.Checked) {
                    _mainHandler.NewUserSpectrums();
                    // GUI
                    _499.InteractionHandlers.Utils.DisableAllButMe(cb, typeof(CheckBox));
                } else {
                    _mainHandler.RemoveUserSpectrums();
                    // GUI
                    _499.InteractionHandlers.Utils.EnableAllButMe(cb, typeof(CheckBox));
                }
            }
        }

        private void THandsHandler(object sender, EventArgs e) {
            CheckBox cb = sender as CheckBox;
            if (cb != null) {
                if (cb.Checked) {
                    switch (cb.Text[0]) {
                        case 'L':
                            _mainHandler.NewUserTimeTravel(TT_SIDE.LEFT); break;
                        case 'R':
                            _mainHandler.NewUserTimeTravel(TT_SIDE.RIGHT); break;
                    }
                } else {
                    switch (cb.Text[0]) {
                        case 'L': _mainHandler.RemoveUserTimeTravel(TT_SIDE.LEFT); break;
                        case 'R': _mainHandler.RemoveUserTimeTravel(TT_SIDE.RIGHT); break;
                    }
                }
            }
        }

        // Update del estado de los Handlers
        private void richTextBox1_MouseUp(object sender, MouseEventArgs e) {
            ((RichTextBox)sender).Text = _mainHandler.GetHandlersStatus();
        }       
    }
}
