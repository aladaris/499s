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
            this.lab_kinectStatus = new System.Windows.Forms.Label();
            this.groupBox_Player1 = new System.Windows.Forms.GroupBox();
            this.table_Player1 = new System.Windows.Forms.TableLayoutPanel();
            this.lab_W_P1 = new System.Windows.Forms.Label();
            this.lab_TF_P1 = new System.Windows.Forms.Label();
            this.lab_TR_P1 = new System.Windows.Forms.Label();
            this.lab_TL_P1 = new System.Windows.Forms.Label();
            this.lab_YF_P1 = new System.Windows.Forms.Label();
            this.lab_YR_P1 = new System.Windows.Forms.Label();
            this.lab_YL_P1 = new System.Windows.Forms.Label();
            this.groupBox_Player2 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lab_W_P2 = new System.Windows.Forms.Label();
            this.lab_TF_P2 = new System.Windows.Forms.Label();
            this.lab_TR_P2 = new System.Windows.Forms.Label();
            this.lab_TL_P2 = new System.Windows.Forms.Label();
            this.lab_YF_P2 = new System.Windows.Forms.Label();
            this.lab_YR_P2 = new System.Windows.Forms.Label();
            this.lab_YL_P2 = new System.Windows.Forms.Label();
            this.groupBox_Player3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lab_W_P3 = new System.Windows.Forms.Label();
            this.lab_TF_P3 = new System.Windows.Forms.Label();
            this.lab_TR_P3 = new System.Windows.Forms.Label();
            this.lab_TL_P3 = new System.Windows.Forms.Label();
            this.lab_YF_P3 = new System.Windows.Forms.Label();
            this.lab_YR_P3 = new System.Windows.Forms.Label();
            this.lab_YL_P3 = new System.Windows.Forms.Label();
            this.groupBox_Player4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.lab_W_P4 = new System.Windows.Forms.Label();
            this.lab_TF_P4 = new System.Windows.Forms.Label();
            this.lab_TR_P4 = new System.Windows.Forms.Label();
            this.lab_TL_P4 = new System.Windows.Forms.Label();
            this.lab_YF_P4 = new System.Windows.Forms.Label();
            this.lab_YR_P4 = new System.Windows.Forms.Label();
            this.lab_YL_P4 = new System.Windows.Forms.Label();
            this.groupBox_Player5 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.lab_W_P5 = new System.Windows.Forms.Label();
            this.lab_TF_P5 = new System.Windows.Forms.Label();
            this.lab_TR_P5 = new System.Windows.Forms.Label();
            this.lab_TL_P5 = new System.Windows.Forms.Label();
            this.lab_YF_P5 = new System.Windows.Forms.Label();
            this.lab_YR_P5 = new System.Windows.Forms.Label();
            this.lab_YL_P5 = new System.Windows.Forms.Label();
            this.groupBox_Player6 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.lab_W_P6 = new System.Windows.Forms.Label();
            this.lab_TF_P6 = new System.Windows.Forms.Label();
            this.lab_TR_P6 = new System.Windows.Forms.Label();
            this.lab_TL_P6 = new System.Windows.Forms.Label();
            this.lab_YF_P6 = new System.Windows.Forms.Label();
            this.lab_YR_P6 = new System.Windows.Forms.Label();
            this.lab_YL_P6 = new System.Windows.Forms.Label();
            this.groupBox_Player1.SuspendLayout();
            this.table_Player1.SuspendLayout();
            this.groupBox_Player2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox_Player3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.groupBox_Player4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox_Player5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.groupBox_Player6.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Log Info:";
            // 
            // richTextBox_LogInfo
            // 
            this.richTextBox_LogInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox_LogInfo.Location = new System.Drawing.Point(354, 55);
            this.richTextBox_LogInfo.Name = "richTextBox_LogInfo";
            this.richTextBox_LogInfo.ReadOnly = true;
            this.richTextBox_LogInfo.Size = new System.Drawing.Size(247, 288);
            this.richTextBox_LogInfo.TabIndex = 10;
            this.richTextBox_LogInfo.Text = "Interactive Hanlder Info (Click to refresh)";
            this.richTextBox_LogInfo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.HandlerInfoUpdate);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(280, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "MIDI device:";
            // 
            // cb_MidiOutDevices
            // 
            this.cb_MidiOutDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_MidiOutDevices.FormattingEnabled = true;
            this.cb_MidiOutDevices.Location = new System.Drawing.Point(354, 11);
            this.cb_MidiOutDevices.Name = "cb_MidiOutDevices";
            this.cb_MidiOutDevices.Size = new System.Drawing.Size(247, 21);
            this.cb_MidiOutDevices.TabIndex = 8;
            this.cb_MidiOutDevices.SelectedIndexChanged += new System.EventHandler(this.SelectedMidiDeviceChanged);
            // 
            // b_Reset
            // 
            this.b_Reset.Enabled = false;
            this.b_Reset.Location = new System.Drawing.Point(273, 278);
            this.b_Reset.Name = "b_Reset";
            this.b_Reset.Size = new System.Drawing.Size(75, 65);
            this.b_Reset.TabIndex = 12;
            this.b_Reset.Text = "Start All";
            this.b_Reset.UseVisualStyleBackColor = true;
            this.b_Reset.Click += new System.EventHandler(this.ResetAll);
            // 
            // lab_kinectStatus
            // 
            this.lab_kinectStatus.AutoSize = true;
            this.lab_kinectStatus.Location = new System.Drawing.Point(12, 17);
            this.lab_kinectStatus.Name = "lab_kinectStatus";
            this.lab_kinectStatus.Size = new System.Drawing.Size(117, 13);
            this.lab_kinectStatus.TabIndex = 13;
            this.lab_kinectStatus.Text = "Kinect Status: Disabled";
            // 
            // groupBox_Player1
            // 
            this.groupBox_Player1.Controls.Add(this.table_Player1);
            this.groupBox_Player1.Location = new System.Drawing.Point(12, 33);
            this.groupBox_Player1.Name = "groupBox_Player1";
            this.groupBox_Player1.Size = new System.Drawing.Size(78, 151);
            this.groupBox_Player1.TabIndex = 14;
            this.groupBox_Player1.TabStop = false;
            this.groupBox_Player1.Text = "Player 1";
            // 
            // table_Player1
            // 
            this.table_Player1.ColumnCount = 2;
            this.table_Player1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table_Player1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.table_Player1.Controls.Add(this.lab_W_P1, 0, 3);
            this.table_Player1.Controls.Add(this.lab_TF_P1, 1, 2);
            this.table_Player1.Controls.Add(this.lab_TR_P1, 0, 2);
            this.table_Player1.Controls.Add(this.lab_TL_P1, 1, 1);
            this.table_Player1.Controls.Add(this.lab_YF_P1, 0, 1);
            this.table_Player1.Controls.Add(this.lab_YR_P1, 1, 0);
            this.table_Player1.Controls.Add(this.lab_YL_P1, 0, 0);
            this.table_Player1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table_Player1.Location = new System.Drawing.Point(3, 16);
            this.table_Player1.Name = "table_Player1";
            this.table_Player1.RowCount = 4;
            this.table_Player1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_Player1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_Player1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_Player1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.table_Player1.Size = new System.Drawing.Size(72, 132);
            this.table_Player1.TabIndex = 0;
            // 
            // lab_W_P1
            // 
            this.lab_W_P1.AutoSize = true;
            this.lab_W_P1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_W_P1.Location = new System.Drawing.Point(3, 99);
            this.lab_W_P1.Name = "lab_W_P1";
            this.lab_W_P1.Size = new System.Drawing.Size(30, 33);
            this.lab_W_P1.TabIndex = 6;
            this.lab_W_P1.Text = "W";
            this.lab_W_P1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TF_P1
            // 
            this.lab_TF_P1.AutoSize = true;
            this.lab_TF_P1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TF_P1.Location = new System.Drawing.Point(39, 66);
            this.lab_TF_P1.Name = "lab_TF_P1";
            this.lab_TF_P1.Size = new System.Drawing.Size(30, 33);
            this.lab_TF_P1.TabIndex = 5;
            this.lab_TF_P1.Text = "T_F";
            this.lab_TF_P1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TR_P1
            // 
            this.lab_TR_P1.AutoSize = true;
            this.lab_TR_P1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TR_P1.Location = new System.Drawing.Point(3, 66);
            this.lab_TR_P1.Name = "lab_TR_P1";
            this.lab_TR_P1.Size = new System.Drawing.Size(30, 33);
            this.lab_TR_P1.TabIndex = 4;
            this.lab_TR_P1.Text = "T_R";
            this.lab_TR_P1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TL_P1
            // 
            this.lab_TL_P1.AutoSize = true;
            this.lab_TL_P1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TL_P1.Location = new System.Drawing.Point(39, 33);
            this.lab_TL_P1.Name = "lab_TL_P1";
            this.lab_TL_P1.Size = new System.Drawing.Size(30, 33);
            this.lab_TL_P1.TabIndex = 3;
            this.lab_TL_P1.Text = "T_L";
            this.lab_TL_P1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YF_P1
            // 
            this.lab_YF_P1.AutoSize = true;
            this.lab_YF_P1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YF_P1.Location = new System.Drawing.Point(3, 33);
            this.lab_YF_P1.Name = "lab_YF_P1";
            this.lab_YF_P1.Size = new System.Drawing.Size(30, 33);
            this.lab_YF_P1.TabIndex = 2;
            this.lab_YF_P1.Text = "Y_F";
            this.lab_YF_P1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YR_P1
            // 
            this.lab_YR_P1.AutoSize = true;
            this.lab_YR_P1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YR_P1.Location = new System.Drawing.Point(39, 0);
            this.lab_YR_P1.Name = "lab_YR_P1";
            this.lab_YR_P1.Size = new System.Drawing.Size(30, 33);
            this.lab_YR_P1.TabIndex = 1;
            this.lab_YR_P1.Text = "Y_R";
            this.lab_YR_P1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YL_P1
            // 
            this.lab_YL_P1.AutoSize = true;
            this.lab_YL_P1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YL_P1.Location = new System.Drawing.Point(3, 0);
            this.lab_YL_P1.Name = "lab_YL_P1";
            this.lab_YL_P1.Size = new System.Drawing.Size(30, 33);
            this.lab_YL_P1.TabIndex = 0;
            this.lab_YL_P1.Text = "Y_L";
            this.lab_YL_P1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox_Player2
            // 
            this.groupBox_Player2.Controls.Add(this.tableLayoutPanel1);
            this.groupBox_Player2.Location = new System.Drawing.Point(96, 33);
            this.groupBox_Player2.Name = "groupBox_Player2";
            this.groupBox_Player2.Size = new System.Drawing.Size(78, 151);
            this.groupBox_Player2.TabIndex = 14;
            this.groupBox_Player2.TabStop = false;
            this.groupBox_Player2.Text = "Player 2";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lab_W_P2, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lab_TF_P2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lab_TR_P2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lab_TL_P2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lab_YF_P2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lab_YR_P2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lab_YL_P2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(72, 132);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lab_W_P2
            // 
            this.lab_W_P2.AutoSize = true;
            this.lab_W_P2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_W_P2.Location = new System.Drawing.Point(3, 99);
            this.lab_W_P2.Name = "lab_W_P2";
            this.lab_W_P2.Size = new System.Drawing.Size(30, 33);
            this.lab_W_P2.TabIndex = 6;
            this.lab_W_P2.Text = "W";
            this.lab_W_P2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TF_P2
            // 
            this.lab_TF_P2.AutoSize = true;
            this.lab_TF_P2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TF_P2.Location = new System.Drawing.Point(39, 66);
            this.lab_TF_P2.Name = "lab_TF_P2";
            this.lab_TF_P2.Size = new System.Drawing.Size(30, 33);
            this.lab_TF_P2.TabIndex = 5;
            this.lab_TF_P2.Text = "T_F";
            this.lab_TF_P2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TR_P2
            // 
            this.lab_TR_P2.AutoSize = true;
            this.lab_TR_P2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TR_P2.Location = new System.Drawing.Point(3, 66);
            this.lab_TR_P2.Name = "lab_TR_P2";
            this.lab_TR_P2.Size = new System.Drawing.Size(30, 33);
            this.lab_TR_P2.TabIndex = 4;
            this.lab_TR_P2.Text = "T_R";
            this.lab_TR_P2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TL_P2
            // 
            this.lab_TL_P2.AutoSize = true;
            this.lab_TL_P2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TL_P2.Location = new System.Drawing.Point(39, 33);
            this.lab_TL_P2.Name = "lab_TL_P2";
            this.lab_TL_P2.Size = new System.Drawing.Size(30, 33);
            this.lab_TL_P2.TabIndex = 3;
            this.lab_TL_P2.Text = "T_L";
            this.lab_TL_P2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YF_P2
            // 
            this.lab_YF_P2.AutoSize = true;
            this.lab_YF_P2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YF_P2.Location = new System.Drawing.Point(3, 33);
            this.lab_YF_P2.Name = "lab_YF_P2";
            this.lab_YF_P2.Size = new System.Drawing.Size(30, 33);
            this.lab_YF_P2.TabIndex = 2;
            this.lab_YF_P2.Text = "Y_F";
            this.lab_YF_P2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YR_P2
            // 
            this.lab_YR_P2.AutoSize = true;
            this.lab_YR_P2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YR_P2.Location = new System.Drawing.Point(39, 0);
            this.lab_YR_P2.Name = "lab_YR_P2";
            this.lab_YR_P2.Size = new System.Drawing.Size(30, 33);
            this.lab_YR_P2.TabIndex = 1;
            this.lab_YR_P2.Text = "Y_R";
            this.lab_YR_P2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YL_P2
            // 
            this.lab_YL_P2.AutoSize = true;
            this.lab_YL_P2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YL_P2.Location = new System.Drawing.Point(3, 0);
            this.lab_YL_P2.Name = "lab_YL_P2";
            this.lab_YL_P2.Size = new System.Drawing.Size(30, 33);
            this.lab_YL_P2.TabIndex = 0;
            this.lab_YL_P2.Text = "Y_L";
            this.lab_YL_P2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox_Player3
            // 
            this.groupBox_Player3.Controls.Add(this.tableLayoutPanel2);
            this.groupBox_Player3.Location = new System.Drawing.Point(180, 35);
            this.groupBox_Player3.Name = "groupBox_Player3";
            this.groupBox_Player3.Size = new System.Drawing.Size(78, 151);
            this.groupBox_Player3.TabIndex = 14;
            this.groupBox_Player3.TabStop = false;
            this.groupBox_Player3.Text = "Player 3";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.lab_W_P3, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.lab_TF_P3, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.lab_TR_P3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.lab_TL_P3, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.lab_YF_P3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.lab_YR_P3, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.lab_YL_P3, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(72, 132);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // lab_W_P3
            // 
            this.lab_W_P3.AutoSize = true;
            this.lab_W_P3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_W_P3.Location = new System.Drawing.Point(3, 99);
            this.lab_W_P3.Name = "lab_W_P3";
            this.lab_W_P3.Size = new System.Drawing.Size(30, 33);
            this.lab_W_P3.TabIndex = 6;
            this.lab_W_P3.Text = "W";
            this.lab_W_P3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TF_P3
            // 
            this.lab_TF_P3.AutoSize = true;
            this.lab_TF_P3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TF_P3.Location = new System.Drawing.Point(39, 66);
            this.lab_TF_P3.Name = "lab_TF_P3";
            this.lab_TF_P3.Size = new System.Drawing.Size(30, 33);
            this.lab_TF_P3.TabIndex = 5;
            this.lab_TF_P3.Text = "T_F";
            this.lab_TF_P3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TR_P3
            // 
            this.lab_TR_P3.AutoSize = true;
            this.lab_TR_P3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TR_P3.Location = new System.Drawing.Point(3, 66);
            this.lab_TR_P3.Name = "lab_TR_P3";
            this.lab_TR_P3.Size = new System.Drawing.Size(30, 33);
            this.lab_TR_P3.TabIndex = 4;
            this.lab_TR_P3.Text = "T_R";
            this.lab_TR_P3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TL_P3
            // 
            this.lab_TL_P3.AutoSize = true;
            this.lab_TL_P3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TL_P3.Location = new System.Drawing.Point(39, 33);
            this.lab_TL_P3.Name = "lab_TL_P3";
            this.lab_TL_P3.Size = new System.Drawing.Size(30, 33);
            this.lab_TL_P3.TabIndex = 3;
            this.lab_TL_P3.Text = "T_L";
            this.lab_TL_P3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YF_P3
            // 
            this.lab_YF_P3.AutoSize = true;
            this.lab_YF_P3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YF_P3.Location = new System.Drawing.Point(3, 33);
            this.lab_YF_P3.Name = "lab_YF_P3";
            this.lab_YF_P3.Size = new System.Drawing.Size(30, 33);
            this.lab_YF_P3.TabIndex = 2;
            this.lab_YF_P3.Text = "Y_F";
            this.lab_YF_P3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YR_P3
            // 
            this.lab_YR_P3.AutoSize = true;
            this.lab_YR_P3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YR_P3.Location = new System.Drawing.Point(39, 0);
            this.lab_YR_P3.Name = "lab_YR_P3";
            this.lab_YR_P3.Size = new System.Drawing.Size(30, 33);
            this.lab_YR_P3.TabIndex = 1;
            this.lab_YR_P3.Text = "Y_R";
            this.lab_YR_P3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YL_P3
            // 
            this.lab_YL_P3.AutoSize = true;
            this.lab_YL_P3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YL_P3.Location = new System.Drawing.Point(3, 0);
            this.lab_YL_P3.Name = "lab_YL_P3";
            this.lab_YL_P3.Size = new System.Drawing.Size(30, 33);
            this.lab_YL_P3.TabIndex = 0;
            this.lab_YL_P3.Text = "Y_L";
            this.lab_YL_P3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox_Player4
            // 
            this.groupBox_Player4.Controls.Add(this.tableLayoutPanel3);
            this.groupBox_Player4.Location = new System.Drawing.Point(12, 190);
            this.groupBox_Player4.Name = "groupBox_Player4";
            this.groupBox_Player4.Size = new System.Drawing.Size(78, 151);
            this.groupBox_Player4.TabIndex = 14;
            this.groupBox_Player4.TabStop = false;
            this.groupBox_Player4.Text = "Player 4";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.lab_W_P4, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.lab_TF_P4, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.lab_TR_P4, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.lab_TL_P4, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.lab_YF_P4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.lab_YR_P4, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.lab_YL_P4, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 4;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(72, 132);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // lab_W_P4
            // 
            this.lab_W_P4.AutoSize = true;
            this.lab_W_P4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_W_P4.Location = new System.Drawing.Point(3, 99);
            this.lab_W_P4.Name = "lab_W_P4";
            this.lab_W_P4.Size = new System.Drawing.Size(30, 33);
            this.lab_W_P4.TabIndex = 6;
            this.lab_W_P4.Text = "W";
            this.lab_W_P4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TF_P4
            // 
            this.lab_TF_P4.AutoSize = true;
            this.lab_TF_P4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TF_P4.Location = new System.Drawing.Point(39, 66);
            this.lab_TF_P4.Name = "lab_TF_P4";
            this.lab_TF_P4.Size = new System.Drawing.Size(30, 33);
            this.lab_TF_P4.TabIndex = 5;
            this.lab_TF_P4.Text = "T_F";
            this.lab_TF_P4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TR_P4
            // 
            this.lab_TR_P4.AutoSize = true;
            this.lab_TR_P4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TR_P4.Location = new System.Drawing.Point(3, 66);
            this.lab_TR_P4.Name = "lab_TR_P4";
            this.lab_TR_P4.Size = new System.Drawing.Size(30, 33);
            this.lab_TR_P4.TabIndex = 4;
            this.lab_TR_P4.Text = "T_R";
            this.lab_TR_P4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TL_P4
            // 
            this.lab_TL_P4.AutoSize = true;
            this.lab_TL_P4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TL_P4.Location = new System.Drawing.Point(39, 33);
            this.lab_TL_P4.Name = "lab_TL_P4";
            this.lab_TL_P4.Size = new System.Drawing.Size(30, 33);
            this.lab_TL_P4.TabIndex = 3;
            this.lab_TL_P4.Text = "T_L";
            this.lab_TL_P4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YF_P4
            // 
            this.lab_YF_P4.AutoSize = true;
            this.lab_YF_P4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YF_P4.Location = new System.Drawing.Point(3, 33);
            this.lab_YF_P4.Name = "lab_YF_P4";
            this.lab_YF_P4.Size = new System.Drawing.Size(30, 33);
            this.lab_YF_P4.TabIndex = 2;
            this.lab_YF_P4.Text = "Y_F";
            this.lab_YF_P4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YR_P4
            // 
            this.lab_YR_P4.AutoSize = true;
            this.lab_YR_P4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YR_P4.Location = new System.Drawing.Point(39, 0);
            this.lab_YR_P4.Name = "lab_YR_P4";
            this.lab_YR_P4.Size = new System.Drawing.Size(30, 33);
            this.lab_YR_P4.TabIndex = 1;
            this.lab_YR_P4.Text = "Y_R";
            this.lab_YR_P4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YL_P4
            // 
            this.lab_YL_P4.AutoSize = true;
            this.lab_YL_P4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YL_P4.Location = new System.Drawing.Point(3, 0);
            this.lab_YL_P4.Name = "lab_YL_P4";
            this.lab_YL_P4.Size = new System.Drawing.Size(30, 33);
            this.lab_YL_P4.TabIndex = 0;
            this.lab_YL_P4.Text = "Y_L";
            this.lab_YL_P4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox_Player5
            // 
            this.groupBox_Player5.Controls.Add(this.tableLayoutPanel4);
            this.groupBox_Player5.Location = new System.Drawing.Point(96, 190);
            this.groupBox_Player5.Name = "groupBox_Player5";
            this.groupBox_Player5.Size = new System.Drawing.Size(78, 151);
            this.groupBox_Player5.TabIndex = 14;
            this.groupBox_Player5.TabStop = false;
            this.groupBox_Player5.Text = "Player 5";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.lab_W_P5, 0, 3);
            this.tableLayoutPanel4.Controls.Add(this.lab_TF_P5, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.lab_TR_P5, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.lab_TL_P5, 1, 1);
            this.tableLayoutPanel4.Controls.Add(this.lab_YF_P5, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.lab_YR_P5, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.lab_YL_P5, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(72, 132);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // lab_W_P5
            // 
            this.lab_W_P5.AutoSize = true;
            this.lab_W_P5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_W_P5.Location = new System.Drawing.Point(3, 99);
            this.lab_W_P5.Name = "lab_W_P5";
            this.lab_W_P5.Size = new System.Drawing.Size(30, 33);
            this.lab_W_P5.TabIndex = 6;
            this.lab_W_P5.Text = "W";
            this.lab_W_P5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TF_P5
            // 
            this.lab_TF_P5.AutoSize = true;
            this.lab_TF_P5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TF_P5.Location = new System.Drawing.Point(39, 66);
            this.lab_TF_P5.Name = "lab_TF_P5";
            this.lab_TF_P5.Size = new System.Drawing.Size(30, 33);
            this.lab_TF_P5.TabIndex = 5;
            this.lab_TF_P5.Text = "T_F";
            this.lab_TF_P5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TR_P5
            // 
            this.lab_TR_P5.AutoSize = true;
            this.lab_TR_P5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TR_P5.Location = new System.Drawing.Point(3, 66);
            this.lab_TR_P5.Name = "lab_TR_P5";
            this.lab_TR_P5.Size = new System.Drawing.Size(30, 33);
            this.lab_TR_P5.TabIndex = 4;
            this.lab_TR_P5.Text = "T_R";
            this.lab_TR_P5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TL_P5
            // 
            this.lab_TL_P5.AutoSize = true;
            this.lab_TL_P5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TL_P5.Location = new System.Drawing.Point(39, 33);
            this.lab_TL_P5.Name = "lab_TL_P5";
            this.lab_TL_P5.Size = new System.Drawing.Size(30, 33);
            this.lab_TL_P5.TabIndex = 3;
            this.lab_TL_P5.Text = "T_L";
            this.lab_TL_P5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YF_P5
            // 
            this.lab_YF_P5.AutoSize = true;
            this.lab_YF_P5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YF_P5.Location = new System.Drawing.Point(3, 33);
            this.lab_YF_P5.Name = "lab_YF_P5";
            this.lab_YF_P5.Size = new System.Drawing.Size(30, 33);
            this.lab_YF_P5.TabIndex = 2;
            this.lab_YF_P5.Text = "Y_F";
            this.lab_YF_P5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YR_P5
            // 
            this.lab_YR_P5.AutoSize = true;
            this.lab_YR_P5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YR_P5.Location = new System.Drawing.Point(39, 0);
            this.lab_YR_P5.Name = "lab_YR_P5";
            this.lab_YR_P5.Size = new System.Drawing.Size(30, 33);
            this.lab_YR_P5.TabIndex = 1;
            this.lab_YR_P5.Text = "Y_R";
            this.lab_YR_P5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YL_P5
            // 
            this.lab_YL_P5.AutoSize = true;
            this.lab_YL_P5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YL_P5.Location = new System.Drawing.Point(3, 0);
            this.lab_YL_P5.Name = "lab_YL_P5";
            this.lab_YL_P5.Size = new System.Drawing.Size(30, 33);
            this.lab_YL_P5.TabIndex = 0;
            this.lab_YL_P5.Text = "Y_L";
            this.lab_YL_P5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox_Player6
            // 
            this.groupBox_Player6.Controls.Add(this.tableLayoutPanel5);
            this.groupBox_Player6.Location = new System.Drawing.Point(180, 192);
            this.groupBox_Player6.Name = "groupBox_Player6";
            this.groupBox_Player6.Size = new System.Drawing.Size(78, 151);
            this.groupBox_Player6.TabIndex = 14;
            this.groupBox_Player6.TabStop = false;
            this.groupBox_Player6.Text = "Player 6";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 2;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.lab_W_P6, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.lab_TF_P6, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.lab_TR_P6, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.lab_TL_P6, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.lab_YF_P6, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.lab_YR_P6, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.lab_YL_P6, 0, 0);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 4;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(72, 132);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // lab_W_P6
            // 
            this.lab_W_P6.AutoSize = true;
            this.lab_W_P6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_W_P6.Location = new System.Drawing.Point(3, 99);
            this.lab_W_P6.Name = "lab_W_P6";
            this.lab_W_P6.Size = new System.Drawing.Size(30, 33);
            this.lab_W_P6.TabIndex = 6;
            this.lab_W_P6.Text = "W";
            this.lab_W_P6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TF_P6
            // 
            this.lab_TF_P6.AutoSize = true;
            this.lab_TF_P6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TF_P6.Location = new System.Drawing.Point(39, 66);
            this.lab_TF_P6.Name = "lab_TF_P6";
            this.lab_TF_P6.Size = new System.Drawing.Size(30, 33);
            this.lab_TF_P6.TabIndex = 5;
            this.lab_TF_P6.Text = "T_F";
            this.lab_TF_P6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TR_P6
            // 
            this.lab_TR_P6.AutoSize = true;
            this.lab_TR_P6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TR_P6.Location = new System.Drawing.Point(3, 66);
            this.lab_TR_P6.Name = "lab_TR_P6";
            this.lab_TR_P6.Size = new System.Drawing.Size(30, 33);
            this.lab_TR_P6.TabIndex = 4;
            this.lab_TR_P6.Text = "T_R";
            this.lab_TR_P6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_TL_P6
            // 
            this.lab_TL_P6.AutoSize = true;
            this.lab_TL_P6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_TL_P6.Location = new System.Drawing.Point(39, 33);
            this.lab_TL_P6.Name = "lab_TL_P6";
            this.lab_TL_P6.Size = new System.Drawing.Size(30, 33);
            this.lab_TL_P6.TabIndex = 3;
            this.lab_TL_P6.Text = "T_L";
            this.lab_TL_P6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YF_P6
            // 
            this.lab_YF_P6.AutoSize = true;
            this.lab_YF_P6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YF_P6.Location = new System.Drawing.Point(3, 33);
            this.lab_YF_P6.Name = "lab_YF_P6";
            this.lab_YF_P6.Size = new System.Drawing.Size(30, 33);
            this.lab_YF_P6.TabIndex = 2;
            this.lab_YF_P6.Text = "Y_F";
            this.lab_YF_P6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YR_P6
            // 
            this.lab_YR_P6.AutoSize = true;
            this.lab_YR_P6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YR_P6.Location = new System.Drawing.Point(39, 0);
            this.lab_YR_P6.Name = "lab_YR_P6";
            this.lab_YR_P6.Size = new System.Drawing.Size(30, 33);
            this.lab_YR_P6.TabIndex = 1;
            this.lab_YR_P6.Text = "Y_R";
            this.lab_YR_P6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lab_YL_P6
            // 
            this.lab_YL_P6.AutoSize = true;
            this.lab_YL_P6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lab_YL_P6.Location = new System.Drawing.Point(3, 0);
            this.lab_YL_P6.Name = "lab_YL_P6";
            this.lab_YL_P6.Size = new System.Drawing.Size(30, 33);
            this.lab_YL_P6.TabIndex = 0;
            this.lab_YL_P6.Text = "Y_L";
            this.lab_YL_P6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 356);
            this.Controls.Add(this.groupBox_Player6);
            this.Controls.Add(this.groupBox_Player3);
            this.Controls.Add(this.groupBox_Player5);
            this.Controls.Add(this.groupBox_Player2);
            this.Controls.Add(this.groupBox_Player4);
            this.Controls.Add(this.groupBox_Player1);
            this.Controls.Add(this.lab_kinectStatus);
            this.Controls.Add(this.b_Reset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.richTextBox_LogInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_MidiOutDevices);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximumSize = new System.Drawing.Size(630, 395);
            this.MinimumSize = new System.Drawing.Size(630, 395);
            this.Name = "Form1";
            this.Text = "499  <---- CHANGE ME";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox_Player1.ResumeLayout(false);
            this.table_Player1.ResumeLayout(false);
            this.table_Player1.PerformLayout();
            this.groupBox_Player2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox_Player3.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.groupBox_Player4.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox_Player5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.groupBox_Player6.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox richTextBox_LogInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_MidiOutDevices;
        private System.Windows.Forms.Button b_Reset;
        private System.Windows.Forms.Label lab_kinectStatus;
        private System.Windows.Forms.GroupBox groupBox_Player1;
        private System.Windows.Forms.TableLayoutPanel table_Player1;
        private System.Windows.Forms.Label lab_W_P1;
        private System.Windows.Forms.Label lab_TF_P1;
        private System.Windows.Forms.Label lab_TR_P1;
        private System.Windows.Forms.Label lab_TL_P1;
        private System.Windows.Forms.Label lab_YF_P1;
        private System.Windows.Forms.Label lab_YR_P1;
        private System.Windows.Forms.Label lab_YL_P1;
        private System.Windows.Forms.GroupBox groupBox_Player2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lab_W_P2;
        private System.Windows.Forms.Label lab_TF_P2;
        private System.Windows.Forms.Label lab_TR_P2;
        private System.Windows.Forms.Label lab_TL_P2;
        private System.Windows.Forms.Label lab_YF_P2;
        private System.Windows.Forms.Label lab_YR_P2;
        private System.Windows.Forms.Label lab_YL_P2;
        private System.Windows.Forms.GroupBox groupBox_Player3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label lab_W_P3;
        private System.Windows.Forms.Label lab_TF_P3;
        private System.Windows.Forms.Label lab_TR_P3;
        private System.Windows.Forms.Label lab_TL_P3;
        private System.Windows.Forms.Label lab_YF_P3;
        private System.Windows.Forms.Label lab_YR_P3;
        private System.Windows.Forms.Label lab_YL_P3;
        private System.Windows.Forms.GroupBox groupBox_Player4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label lab_W_P4;
        private System.Windows.Forms.Label lab_TF_P4;
        private System.Windows.Forms.Label lab_TR_P4;
        private System.Windows.Forms.Label lab_TL_P4;
        private System.Windows.Forms.Label lab_YF_P4;
        private System.Windows.Forms.Label lab_YR_P4;
        private System.Windows.Forms.Label lab_YL_P4;
        private System.Windows.Forms.GroupBox groupBox_Player5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Label lab_W_P5;
        private System.Windows.Forms.Label lab_TF_P5;
        private System.Windows.Forms.Label lab_TR_P5;
        private System.Windows.Forms.Label lab_TL_P5;
        private System.Windows.Forms.Label lab_YF_P5;
        private System.Windows.Forms.Label lab_YR_P5;
        private System.Windows.Forms.Label lab_YL_P5;
        private System.Windows.Forms.GroupBox groupBox_Player6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label lab_W_P6;
        private System.Windows.Forms.Label lab_TF_P6;
        private System.Windows.Forms.Label lab_TR_P6;
        private System.Windows.Forms.Label lab_TL_P6;
        private System.Windows.Forms.Label lab_YF_P6;
        private System.Windows.Forms.Label lab_YR_P6;
        private System.Windows.Forms.Label lab_YL_P6;
    }
}

