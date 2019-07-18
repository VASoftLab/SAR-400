namespace SARSimulatorControl
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxConnection = new System.Windows.Forms.GroupBox();
            this.buttonConnection = new System.Windows.Forms.Button();
            this.labelPort = new System.Windows.Forms.Label();
            this.labelIP = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.statusStripStatus = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelConnectionStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBoxCommand = new System.Windows.Forms.GroupBox();
            this.textBoxCommand = new System.Windows.Forms.TextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.labelRobotAnswer = new System.Windows.Forms.Label();
            this.textBoxTime = new System.Windows.Forms.TextBox();
            this.checkBoxReset = new System.Windows.Forms.CheckBox();
            this.groupBoxConnection.SuspendLayout();
            this.statusStripStatus.SuspendLayout();
            this.groupBoxCommand.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxConnection
            // 
            this.groupBoxConnection.Controls.Add(this.buttonConnection);
            this.groupBoxConnection.Controls.Add(this.labelPort);
            this.groupBoxConnection.Controls.Add(this.labelIP);
            this.groupBoxConnection.Controls.Add(this.textBoxPort);
            this.groupBoxConnection.Controls.Add(this.textBoxIP);
            this.groupBoxConnection.Location = new System.Drawing.Point(12, 12);
            this.groupBoxConnection.Name = "groupBoxConnection";
            this.groupBoxConnection.Size = new System.Drawing.Size(260, 96);
            this.groupBoxConnection.TabIndex = 0;
            this.groupBoxConnection.TabStop = false;
            this.groupBoxConnection.Text = "Connection";
            // 
            // buttonConnection
            // 
            this.buttonConnection.Location = new System.Drawing.Point(16, 48);
            this.buttonConnection.Name = "buttonConnection";
            this.buttonConnection.Size = new System.Drawing.Size(225, 29);
            this.buttonConnection.TabIndex = 0;
            this.buttonConnection.Text = "CONNECT";
            this.buttonConnection.UseVisualStyleBackColor = true;
            this.buttonConnection.Click += new System.EventHandler(this.buttonConnection_Click);
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(137, 23);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(35, 16);
            this.labelPort.TabIndex = 3;
            this.labelPort.Text = "Port:";
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(16, 23);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(20, 16);
            this.labelIP.TabIndex = 2;
            this.labelIP.Text = "IP";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(178, 20);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(63, 22);
            this.textBoxPort.TabIndex = 2;
            this.textBoxPort.TabStop = false;
            this.textBoxPort.Text = "10099";
            this.textBoxPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(42, 20);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(89, 22);
            this.textBoxIP.TabIndex = 1;
            this.textBoxIP.TabStop = false;
            this.textBoxIP.Text = "127.0.0.1";
            this.textBoxIP.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(395, 327);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(103, 29);
            this.buttonClose.TabIndex = 3;
            this.buttonClose.Text = "CLOSE";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // statusStripStatus
            // 
            this.statusStripStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelConnectionStatus});
            this.statusStripStatus.Location = new System.Drawing.Point(0, 368);
            this.statusStripStatus.Name = "statusStripStatus";
            this.statusStripStatus.Size = new System.Drawing.Size(510, 22);
            this.statusStripStatus.SizingGrip = false;
            this.statusStripStatus.TabIndex = 6;
            this.statusStripStatus.Text = "statusStrip1";
            // 
            // toolStripStatusLabelConnectionStatus
            // 
            this.toolStripStatusLabelConnectionStatus.Name = "toolStripStatusLabelConnectionStatus";
            this.toolStripStatusLabelConnectionStatus.Size = new System.Drawing.Size(51, 17);
            this.toolStripStatusLabelConnectionStatus.Text = "SAR-400";
            // 
            // groupBoxCommand
            // 
            this.groupBoxCommand.Controls.Add(this.checkBoxReset);
            this.groupBoxCommand.Controls.Add(this.textBoxTime);
            this.groupBoxCommand.Controls.Add(this.labelRobotAnswer);
            this.groupBoxCommand.Controls.Add(this.buttonSend);
            this.groupBoxCommand.Controls.Add(this.textBoxCommand);
            this.groupBoxCommand.Location = new System.Drawing.Point(12, 114);
            this.groupBoxCommand.Name = "groupBoxCommand";
            this.groupBoxCommand.Size = new System.Drawing.Size(486, 105);
            this.groupBoxCommand.TabIndex = 7;
            this.groupBoxCommand.TabStop = false;
            this.groupBoxCommand.Text = "Command";
            // 
            // textBoxCommand
            // 
            this.textBoxCommand.Location = new System.Drawing.Point(16, 31);
            this.textBoxCommand.Name = "textBoxCommand";
            this.textBoxCommand.Size = new System.Drawing.Size(406, 22);
            this.textBoxCommand.TabIndex = 0;
            this.textBoxCommand.TabStop = false;
            this.textBoxCommand.Text = "ROBOT:MOTORS:R.ShoulderF;R.Elbow:POSSET:-45;-90";
            // 
            // buttonSend
            // 
            this.buttonSend.Enabled = false;
            this.buttonSend.Location = new System.Drawing.Point(364, 59);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(103, 29);
            this.buttonSend.TabIndex = 1;
            this.buttonSend.Text = "SEND";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // labelRobotAnswer
            // 
            this.labelRobotAnswer.AutoSize = true;
            this.labelRobotAnswer.ForeColor = System.Drawing.Color.Gray;
            this.labelRobotAnswer.Location = new System.Drawing.Point(16, 59);
            this.labelRobotAnswer.Name = "labelRobotAnswer";
            this.labelRobotAnswer.Size = new System.Drawing.Size(17, 16);
            this.labelRobotAnswer.TabIndex = 2;
            this.labelRobotAnswer.Text = "...";
            // 
            // textBoxTime
            // 
            this.textBoxTime.Location = new System.Drawing.Point(428, 31);
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.Size = new System.Drawing.Size(39, 22);
            this.textBoxTime.TabIndex = 3;
            this.textBoxTime.TabStop = false;
            this.textBoxTime.Text = "10";
            this.textBoxTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBoxReset
            // 
            this.checkBoxReset.AutoSize = true;
            this.checkBoxReset.Location = new System.Drawing.Point(295, 63);
            this.checkBoxReset.Name = "checkBoxReset";
            this.checkBoxReset.Size = new System.Drawing.Size(63, 20);
            this.checkBoxReset.TabIndex = 4;
            this.checkBoxReset.Text = "Reset";
            this.checkBoxReset.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 390);
            this.Controls.Add(this.groupBoxCommand);
            this.Controls.Add(this.statusStripStatus);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.groupBoxConnection);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simulator Control";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBoxConnection.ResumeLayout(false);
            this.groupBoxConnection.PerformLayout();
            this.statusStripStatus.ResumeLayout(false);
            this.statusStripStatus.PerformLayout();
            this.groupBoxCommand.ResumeLayout(false);
            this.groupBoxCommand.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxConnection;
        private System.Windows.Forms.Button buttonConnection;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.StatusStrip statusStripStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelConnectionStatus;
        private System.Windows.Forms.GroupBox groupBoxCommand;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.TextBox textBoxCommand;
        private System.Windows.Forms.Label labelRobotAnswer;
        private System.Windows.Forms.TextBox textBoxTime;
        private System.Windows.Forms.CheckBox checkBoxReset;
    }
}

