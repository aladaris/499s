using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using _499.InteractionHandlers;
using Lib.GestureTrigger;

namespace MainAplication {
    public partial class Form1 : Form {

        private InteractionSystemHandler _interactHandler;

        public Form1() {
            InitializeComponent();
            _interactHandler = new InteractionSystemHandler(0, Midi.Channel.Channel1);
        }

        private void Form1_Load(object sender, EventArgs e) {
            LoadMidiOutDevicesComboBox();
        }

        private void LoadMidiOutDevicesComboBox() {
            for (int i = 0; i < Midi.OutputDevice.InstalledDevices.Count; i++) {
                cb_MidiOutDevices.Items.Add(Midi.OutputDevice.InstalledDevices[i].Name);
            }
        }

        private void SelectedMidiDeviceChanged(object sender, EventArgs e) {
            if (!b_Reset.Enabled)
                b_Reset.Enabled = true;
            _interactHandler.MidiOutPutId = ((ComboBox)sender).SelectedIndex;
        }

        private void HandlerInfoUpdate(object sender, MouseEventArgs e) {
            ((RichTextBox)sender).Text = _interactHandler.GetHandlersStatus();
        }

        private void ResetAll(object sender, EventArgs e) {
            ((Button)sender).Text = "Reset All";
            _interactHandler.Reset();
        }
    }
}
