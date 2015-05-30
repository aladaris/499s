namespace MainAplication {
    partial class Form1 {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent() {
            this.label2 = new System.Windows.Forms.Label();
            this.richTextBox_LogInfo = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_MidiOutDevices = new System.Windows.Forms.ComboBox();
            this.b_Reset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(380, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Log Info:";
            // 
            // richTextBox_LogInfo
            // 
            this.richTextBox_LogInfo.Location = new System.Drawing.Point(435, 58);
            this.richTextBox_LogInfo.Name = "richTextBox_LogInfo";
            this.richTextBox_LogInfo.Size = new System.Drawing.Size(247, 273);
            this.richTextBox_LogInfo.TabIndex = 10;
            this.richTextBox_LogInfo.Text = " Click to update handler info...";
            this.richTextBox_LogInfo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HandlerInfoUpdate);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(361, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "MIDI device:";
            // 
            // cb_MidiOutDevices
            // 
            this.cb_MidiOutDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_MidiOutDevices.FormattingEnabled = true;
            this.cb_MidiOutDevices.Location = new System.Drawing.Point(435, 14);
            this.cb_MidiOutDevices.Name = "cb_MidiOutDevices";
            this.cb_MidiOutDevices.Size = new System.Drawing.Size(247, 21);
            this.cb_MidiOutDevices.TabIndex = 8;
            this.cb_MidiOutDevices.SelectedIndexChanged += new System.EventHandler(this.SelectedMidiDeviceChanged);
            // 
            // b_Reset
            // 
            this.b_Reset.Enabled = false;
            this.b_Reset.Location = new System.Drawing.Point(12, 60);
            this.b_Reset.Name = "b_Reset";
            this.b_Reset.Size = new System.Drawing.Size(75, 65);
            this.b_Reset.TabIndex = 12;
            this.b_Reset.Text = "Start All";
            this.b_Reset.UseVisualStyleBackColor = true;
            this.b_Reset.Click += new System.EventHandler(this.ResetAll);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 347);
            this.Controls.Add(this.b_Reset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBox_LogInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_MidiOutDevices);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form1";
            this.Text = "499  <---- CHANGE ME";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTextBox_LogInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_MidiOutDevices;
        private System.Windows.Forms.Button b_Reset;
    }
}

