namespace ManualGestureTrigger {
    partial class MainForm {
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
            this.cb_MidiOutDevices = new System.Windows.Forms.ComboBox();
            this.cb_leftHandUp_P1 = new System.Windows.Forms.CheckBox();
            this.cb_leftHandUp_P2 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_leftHandUp_P3 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_rightHandUp_P1 = new System.Windows.Forms.CheckBox();
            this.cb_bothHandsUp_P1 = new System.Windows.Forms.CheckBox();
            this.cb_rightHandUp_P2 = new System.Windows.Forms.CheckBox();
            this.cb_bothHandsUp_P2 = new System.Windows.Forms.CheckBox();
            this.cb_rightHandUp_P3 = new System.Windows.Forms.CheckBox();
            this.cb_bothHandsUp_P3 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // cb_MidiOutDevices
            // 
            this.cb_MidiOutDevices.FormattingEnabled = true;
            this.cb_MidiOutDevices.Location = new System.Drawing.Point(605, 30);
            this.cb_MidiOutDevices.Name = "cb_MidiOutDevices";
            this.cb_MidiOutDevices.Size = new System.Drawing.Size(160, 21);
            this.cb_MidiOutDevices.TabIndex = 0;
            this.cb_MidiOutDevices.SelectedIndexChanged += new System.EventHandler(this.cb_MidiOutDevices_SelectedIndexChanged);
            // 
            // cb_leftHandUp_P1
            // 
            this.cb_leftHandUp_P1.AutoSize = true;
            this.cb_leftHandUp_P1.Location = new System.Drawing.Point(6, 18);
            this.cb_leftHandUp_P1.Name = "cb_leftHandUp_P1";
            this.cb_leftHandUp_P1.Size = new System.Drawing.Size(78, 17);
            this.cb_leftHandUp_P1.TabIndex = 1;
            this.cb_leftHandUp_P1.Text = "L Hand Up";
            this.cb_leftHandUp_P1.UseVisualStyleBackColor = true;
            this.cb_leftHandUp_P1.CheckedChanged += new System.EventHandler(this.LeftHandUpHandler);
            // 
            // cb_leftHandUp_P2
            // 
            this.cb_leftHandUp_P2.AutoSize = true;
            this.cb_leftHandUp_P2.Location = new System.Drawing.Point(6, 19);
            this.cb_leftHandUp_P2.Name = "cb_leftHandUp_P2";
            this.cb_leftHandUp_P2.Size = new System.Drawing.Size(78, 17);
            this.cb_leftHandUp_P2.TabIndex = 1;
            this.cb_leftHandUp_P2.Text = "L Hand Up";
            this.cb_leftHandUp_P2.UseVisualStyleBackColor = true;
            this.cb_leftHandUp_P2.CheckedChanged += new System.EventHandler(this.LeftHandUpHandler);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_bothHandsUp_P1);
            this.groupBox1.Controls.Add(this.cb_rightHandUp_P1);
            this.groupBox1.Controls.Add(this.cb_leftHandUp_P1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Player 1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_bothHandsUp_P2);
            this.groupBox2.Controls.Add(this.cb_rightHandUp_P2);
            this.groupBox2.Controls.Add(this.cb_leftHandUp_P2);
            this.groupBox2.Location = new System.Drawing.Point(12, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 100);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Player 2";
            // 
            // cb_leftHandUp_P3
            // 
            this.cb_leftHandUp_P3.AutoSize = true;
            this.cb_leftHandUp_P3.Location = new System.Drawing.Point(6, 19);
            this.cb_leftHandUp_P3.Name = "cb_leftHandUp_P3";
            this.cb_leftHandUp_P3.Size = new System.Drawing.Size(78, 17);
            this.cb_leftHandUp_P3.TabIndex = 1;
            this.cb_leftHandUp_P3.Text = "L Hand Up";
            this.cb_leftHandUp_P3.UseVisualStyleBackColor = true;
            this.cb_leftHandUp_P3.CheckedChanged += new System.EventHandler(this.LeftHandUpHandler);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_bothHandsUp_P3);
            this.groupBox3.Controls.Add(this.cb_rightHandUp_P3);
            this.groupBox3.Controls.Add(this.cb_leftHandUp_P3);
            this.groupBox3.Location = new System.Drawing.Point(12, 224);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(282, 100);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Player 3";
            // 
            // cb_rightHandUp_P1
            // 
            this.cb_rightHandUp_P1.AutoSize = true;
            this.cb_rightHandUp_P1.Location = new System.Drawing.Point(195, 18);
            this.cb_rightHandUp_P1.Name = "cb_rightHandUp_P1";
            this.cb_rightHandUp_P1.Size = new System.Drawing.Size(80, 17);
            this.cb_rightHandUp_P1.TabIndex = 1;
            this.cb_rightHandUp_P1.Text = "R Hand Up";
            this.cb_rightHandUp_P1.UseVisualStyleBackColor = true;
            this.cb_rightHandUp_P1.CheckedChanged += new System.EventHandler(this.LeftHandUpHandler);
            // 
            // cb_bothHandsUp_P1
            // 
            this.cb_bothHandsUp_P1.AutoSize = true;
            this.cb_bothHandsUp_P1.Location = new System.Drawing.Point(90, 18);
            this.cb_bothHandsUp_P1.Name = "cb_bothHandsUp_P1";
            this.cb_bothHandsUp_P1.Size = new System.Drawing.Size(99, 17);
            this.cb_bothHandsUp_P1.TabIndex = 1;
            this.cb_bothHandsUp_P1.Text = "Both Hands Up";
            this.cb_bothHandsUp_P1.UseVisualStyleBackColor = true;
            this.cb_bothHandsUp_P1.CheckedChanged += new System.EventHandler(this.LeftHandUpHandler);
            // 
            // cb_rightHandUp_P2
            // 
            this.cb_rightHandUp_P2.AutoSize = true;
            this.cb_rightHandUp_P2.Location = new System.Drawing.Point(195, 19);
            this.cb_rightHandUp_P2.Name = "cb_rightHandUp_P2";
            this.cb_rightHandUp_P2.Size = new System.Drawing.Size(80, 17);
            this.cb_rightHandUp_P2.TabIndex = 1;
            this.cb_rightHandUp_P2.Text = "R Hand Up";
            this.cb_rightHandUp_P2.UseVisualStyleBackColor = true;
            this.cb_rightHandUp_P2.CheckedChanged += new System.EventHandler(this.LeftHandUpHandler);
            // 
            // cb_bothHandsUp_P2
            // 
            this.cb_bothHandsUp_P2.AutoSize = true;
            this.cb_bothHandsUp_P2.Location = new System.Drawing.Point(90, 19);
            this.cb_bothHandsUp_P2.Name = "cb_bothHandsUp_P2";
            this.cb_bothHandsUp_P2.Size = new System.Drawing.Size(99, 17);
            this.cb_bothHandsUp_P2.TabIndex = 1;
            this.cb_bothHandsUp_P2.Text = "Both Hands Up";
            this.cb_bothHandsUp_P2.UseVisualStyleBackColor = true;
            this.cb_bothHandsUp_P2.CheckedChanged += new System.EventHandler(this.LeftHandUpHandler);
            // 
            // cb_rightHandUp_P3
            // 
            this.cb_rightHandUp_P3.AutoSize = true;
            this.cb_rightHandUp_P3.Location = new System.Drawing.Point(195, 19);
            this.cb_rightHandUp_P3.Name = "cb_rightHandUp_P3";
            this.cb_rightHandUp_P3.Size = new System.Drawing.Size(80, 17);
            this.cb_rightHandUp_P3.TabIndex = 1;
            this.cb_rightHandUp_P3.Text = "R Hand Up";
            this.cb_rightHandUp_P3.UseVisualStyleBackColor = true;
            this.cb_rightHandUp_P3.CheckedChanged += new System.EventHandler(this.LeftHandUpHandler);
            // 
            // cb_bothHandsUp_P3
            // 
            this.cb_bothHandsUp_P3.AutoSize = true;
            this.cb_bothHandsUp_P3.Location = new System.Drawing.Point(90, 19);
            this.cb_bothHandsUp_P3.Name = "cb_bothHandsUp_P3";
            this.cb_bothHandsUp_P3.Size = new System.Drawing.Size(99, 17);
            this.cb_bothHandsUp_P3.TabIndex = 1;
            this.cb_bothHandsUp_P3.Text = "Both Hands Up";
            this.cb_bothHandsUp_P3.UseVisualStyleBackColor = true;
            this.cb_bothHandsUp_P3.CheckedChanged += new System.EventHandler(this.LeftHandUpHandler);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 472);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cb_MidiOutDevices);
            this.Name = "MainForm";
            this.Text = "Interaction panel";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_MidiOutDevices;
        private System.Windows.Forms.CheckBox cb_leftHandUp_P1;
        private System.Windows.Forms.CheckBox cb_leftHandUp_P2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cb_leftHandUp_P3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox cb_bothHandsUp_P1;
        private System.Windows.Forms.CheckBox cb_rightHandUp_P1;
        private System.Windows.Forms.CheckBox cb_bothHandsUp_P2;
        private System.Windows.Forms.CheckBox cb_rightHandUp_P2;
        private System.Windows.Forms.CheckBox cb_bothHandsUp_P3;
        private System.Windows.Forms.CheckBox cb_rightHandUp_P3;
    }
}

