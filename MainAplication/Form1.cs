using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Kinect;

using _499.InteractionHandlers;

namespace MainAplication {
    public partial class Form1 : Form {

        private InteractionSystemHandler _interactHandler;
        private KinectHandler.KinectHanlder _kinectHandler;

        public Form1() {
            InitializeComponent();
            _interactHandler = new InteractionSystemHandler(0, Midi.Channel.Channel1);
            _kinectHandler = new KinectHandler.KinectHanlder();
            _kinectHandler.OnGestureTrigger += GestureTriggered;
            _kinectHandler.OnKinectAviableChange += KinectAvailableChanged;
        }

        private void KinectAvailableChanged(object sender, IsAvailableChangedEventArgs e) {
            if (e.IsAvailable)
                lab_kinectStatus.Text = "Kinect Status: Enabled";
            else
                lab_kinectStatus.Text = "Kinect Status: Disabled";
        }

        private void GestureTriggered(int ui_id, ulong trackingId, KinectHandler.INTERACTION interaction, bool detected) {
            //MessageBox.Show(detected.ToString() + "  ID: " + trackingId.ToString() + "  Interaction: " + interaction.ToString());
            UpdateGestureGUITables(ui_id, interaction, detected);
            switch (interaction) {
                case KinectHandler.INTERACTION.FLARE_L:
                    if (detected) {
                        _interactHandler.NewUserFlares(FLARE_SIDE.LEFT);
                    } else {
                        _interactHandler.RemoveUserFlares();
                    }
                    break;
                case KinectHandler.INTERACTION.FLARE_R:
                    if (detected) {
                        _interactHandler.NewUserFlares(FLARE_SIDE.RIGHT);
                    } else {
                        _interactHandler.RemoveUserFlares();
                    }
                    break;
                case KinectHandler.INTERACTION.FLARE_C:
                    if (detected) {
                        _interactHandler.NewUserFlares(FLARE_SIDE.CENTER);
                    } else {
                        _interactHandler.RemoveUserFlares();
                    }
                    break;
                case KinectHandler.INTERACTION.TIMETRAVEL_REW:
                    if (detected) {
                        _interactHandler.NewUserTimeTravel(TT_SIDE.LEFT);
                    } else {
                        _interactHandler.RemoveUserTimeTravel(TT_SIDE.LEFT);
                    }
                    break;
                case KinectHandler.INTERACTION.TIMETRAVEL_FF:
                    if (detected) {
                        _interactHandler.NewUserTimeTravel(TT_SIDE.RIGHT);
                    } else {
                        _interactHandler.RemoveUserTimeTravel(TT_SIDE.RIGHT);
                    }
                    break;
                case KinectHandler.INTERACTION.GLOW:
                    if (detected) {
                        _interactHandler.NewUserGlow();
                    } else {
                        _interactHandler.RemoveUserGlow();
                    }
                    break;
                case KinectHandler.INTERACTION.SPECTRUM:
                    if (detected) {
                        _interactHandler.NewUserSpectrums();
                    } else {
                        _interactHandler.RemoveUserSpectrums();
                    }
                    break;

            }

            // GUI
            richTextBox_LogInfo.SetPropertyThreadSafe(() => richTextBox_LogInfo.Text, _interactHandler.GetHandlersStatus());

            // Update Table colors player 1
            if (ui_id == 0) {
                
            }
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

        private void UpdateGestureGUITables(int ui_id, KinectHandler.INTERACTION interaction, bool detected) {
            Color col = detected ? Color.Green : Color.Transparent;
            switch (interaction) {
                case KinectHandler.INTERACTION.FLARE_L:
                    switch (ui_id) {
                        case 0:
                            lab_YL_P1.SetPropertyThreadSafe(() => lab_YL_P1.BackColor, col);
                            break;
                        case 1:
                            lab_YL_P2.SetPropertyThreadSafe(() => lab_YL_P2.BackColor, col);
                            break;
                        case 2:
                            lab_YL_P3.SetPropertyThreadSafe(() => lab_YL_P3.BackColor, col);
                            break;
                        case 3:
                            lab_YL_P4.SetPropertyThreadSafe(() => lab_YL_P4.BackColor, col);
                            break;
                        case 4:
                            lab_YL_P5.SetPropertyThreadSafe(() => lab_YL_P5.BackColor, col);
                            break;
                        case 5:
                            lab_YL_P6.SetPropertyThreadSafe(() => lab_YL_P6.BackColor, col);
                            break;
                    }
                    break;
                case KinectHandler.INTERACTION.FLARE_R:
                    switch (ui_id) {
                        case 0:
                            lab_YR_P1.SetPropertyThreadSafe(() => lab_YR_P1.BackColor, col);
                            break;
                        case 1:
                            lab_YR_P2.SetPropertyThreadSafe(() => lab_YR_P2.BackColor, col);
                            break;
                        case 2:
                            lab_YR_P3.SetPropertyThreadSafe(() => lab_YR_P3.BackColor, col);
                            break;
                        case 3:
                            lab_YR_P4.SetPropertyThreadSafe(() => lab_YR_P4.BackColor, col);
                            break;
                        case 4:
                            lab_YR_P5.SetPropertyThreadSafe(() => lab_YR_P5.BackColor, col);
                            break;
                        case 5:
                            lab_YR_P6.SetPropertyThreadSafe(() => lab_YR_P6.BackColor, col);
                            break;
                    }
                    break;
                case KinectHandler.INTERACTION.FLARE_C:
                    switch (ui_id) {
                        case 0:
                            lab_YF_P1.SetPropertyThreadSafe(() => lab_YF_P1.BackColor, col);
                            break;
                        case 1:
                            lab_YF_P2.SetPropertyThreadSafe(() => lab_YF_P2.BackColor, col);
                            break;
                        case 2:
                            lab_YF_P3.SetPropertyThreadSafe(() => lab_YF_P3.BackColor, col);
                            break;
                        case 3:
                            lab_YF_P4.SetPropertyThreadSafe(() => lab_YF_P4.BackColor, col);
                            break;
                        case 4:
                            lab_YF_P5.SetPropertyThreadSafe(() => lab_YF_P5.BackColor, col);
                            break;
                        case 5:
                            lab_YF_P6.SetPropertyThreadSafe(() => lab_YF_P6.BackColor, col);
                            break;
                    }
                    break;
                case KinectHandler.INTERACTION.TIMETRAVEL_REW:
                    switch (ui_id) {
                        case 0:
                            lab_TL_P1.SetPropertyThreadSafe(() => lab_TL_P1.BackColor, col);
                            break;
                        case 1:
                            lab_TL_P2.SetPropertyThreadSafe(() => lab_TL_P2.BackColor, col);
                            break;
                        case 2:
                            lab_TL_P3.SetPropertyThreadSafe(() => lab_TL_P3.BackColor, col);
                            break;
                        case 3:
                            lab_TL_P4.SetPropertyThreadSafe(() => lab_TL_P4.BackColor, col);
                            break;
                        case 4:
                            lab_TL_P5.SetPropertyThreadSafe(() => lab_TL_P5.BackColor, col);
                            break;
                        case 5:
                            lab_TL_P6.SetPropertyThreadSafe(() => lab_TL_P6.BackColor, col);
                            break;
                    }
                    break;
                case KinectHandler.INTERACTION.TIMETRAVEL_FF:
                    switch (ui_id) {
                        case 0:
                            lab_TR_P1.SetPropertyThreadSafe(() => lab_TR_P1.BackColor, col);
                            break;
                        case 1:
                            lab_TR_P2.SetPropertyThreadSafe(() => lab_TR_P2.BackColor, col);
                            break;
                        case 2:
                            lab_TR_P3.SetPropertyThreadSafe(() => lab_TR_P3.BackColor, col);
                            break;
                        case 3:
                            lab_TR_P4.SetPropertyThreadSafe(() => lab_TR_P4.BackColor, col);
                            break;
                        case 4:
                            lab_TR_P5.SetPropertyThreadSafe(() => lab_TR_P5.BackColor, col);
                            break;
                        case 5:
                            lab_TR_P6.SetPropertyThreadSafe(() => lab_TR_P6.BackColor, col);
                            break;
                    }
                    break;
                case KinectHandler.INTERACTION.GLOW:
                    switch (ui_id) {
                        case 0:
                            lab_TF_P1.SetPropertyThreadSafe(() => lab_TF_P1.BackColor, col);
                            break;
                        case 1:
                            lab_TF_P2.SetPropertyThreadSafe(() => lab_TF_P2.BackColor, col);
                            break;
                        case 2:
                            lab_TF_P3.SetPropertyThreadSafe(() => lab_TF_P3.BackColor, col);
                            break;
                        case 3:
                            lab_TF_P4.SetPropertyThreadSafe(() => lab_TF_P4.BackColor, col);
                            break;
                        case 4:
                            lab_TF_P5.SetPropertyThreadSafe(() => lab_TF_P5.BackColor, col);
                            break;
                        case 5:
                            lab_TF_P6.SetPropertyThreadSafe(() => lab_TF_P6.BackColor, col);
                            break;
                    }
                    break;
                case KinectHandler.INTERACTION.SPECTRUM:
                    switch (ui_id) {
                        case 0:
                            lab_W_P1.SetPropertyThreadSafe(() => lab_W_P1.BackColor, col);
                            break;
                        case 1:
                            lab_W_P2.SetPropertyThreadSafe(() => lab_W_P2.BackColor, col);
                            break;
                        case 2:
                            lab_W_P3.SetPropertyThreadSafe(() => lab_W_P3.BackColor, col);
                            break;
                        case 3:
                            lab_W_P4.SetPropertyThreadSafe(() => lab_W_P4.BackColor, col);
                            break;
                        case 4:
                            lab_W_P5.SetPropertyThreadSafe(() => lab_W_P5.BackColor, col);
                            break;
                        case 5:
                            lab_W_P6.SetPropertyThreadSafe(() => lab_W_P6.BackColor, col);
                            break;
                    }
                    break;
            }

        }
    }
}
