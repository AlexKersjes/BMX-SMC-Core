namespace SMCUserInterface
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.btnConnectMQTT = new System.Windows.Forms.Button();
            this.btnDisconnectMQTT = new System.Windows.Forms.Button();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.clbTags = new System.Windows.Forms.CheckedListBox();
            this.lbData = new System.Windows.Forms.ListBox();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.tbUsername = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.btnSaveFile = new System.Windows.Forms.Button();
            this.tbTopic = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SensorDataUpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sldrTimingMin = new System.Windows.Forms.TrackBar();
            this.sldrTimingMax = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sldrTimingMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sldrTimingMax)).BeginInit();
            this.SuspendLayout();
            // 
            // btnConnectMQTT
            // 
            this.btnConnectMQTT.Location = new System.Drawing.Point(7, 175);
            this.btnConnectMQTT.Name = "btnConnectMQTT";
            this.btnConnectMQTT.Size = new System.Drawing.Size(104, 23);
            this.btnConnectMQTT.TabIndex = 1;
            this.btnConnectMQTT.Text = "Connect to MQTT";
            this.btnConnectMQTT.UseVisualStyleBackColor = true;
            this.btnConnectMQTT.Click += new System.EventHandler(this.btnConnectMQTT_Click);
            // 
            // btnDisconnectMQTT
            // 
            this.btnDisconnectMQTT.Location = new System.Drawing.Point(7, 175);
            this.btnDisconnectMQTT.Name = "btnDisconnectMQTT";
            this.btnDisconnectMQTT.Size = new System.Drawing.Size(104, 23);
            this.btnDisconnectMQTT.TabIndex = 1;
            this.btnDisconnectMQTT.Text = "Disconnect fromMQTT";
            this.btnDisconnectMQTT.UseVisualStyleBackColor = true;
            this.btnDisconnectMQTT.Visible = false;
            this.btnDisconnectMQTT.Click += new System.EventHandler(this.btnDisconnectMQTT_Click);
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(8, 133);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(53, 13);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(8, 94);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Username";
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(4, 16);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(38, 13);
            this.lblServer.TabIndex = 2;
            this.lblServer.Text = "Server";
            // 
            // clbTags
            // 
            this.clbTags.FormattingEnabled = true;
            this.clbTags.Location = new System.Drawing.Point(12, 286);
            this.clbTags.Name = "clbTags";
            this.clbTags.Size = new System.Drawing.Size(118, 214);
            this.clbTags.TabIndex = 4;
            this.clbTags.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.clbTags_ItemCheck);
            // 
            // lbData
            // 
            this.lbData.FormattingEnabled = true;
            this.lbData.Location = new System.Drawing.Point(136, 15);
            this.lbData.Name = "lbData";
            this.lbData.Size = new System.Drawing.Size(1101, 485);
            this.lbData.TabIndex = 5;
            // 
            // tbServer
            // 
            this.tbServer.Location = new System.Drawing.Point(7, 32);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(100, 20);
            this.tbServer.TabIndex = 6;
            this.tbServer.Text = "broker.hivemq.com";
            // 
            // tbUsername
            // 
            this.tbUsername.Location = new System.Drawing.Point(7, 110);
            this.tbUsername.Name = "tbUsername";
            this.tbUsername.Size = new System.Drawing.Size(100, 20);
            this.tbUsername.TabIndex = 6;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(7, 149);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(100, 20);
            this.tbPassword.TabIndex = 6;
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Location = new System.Drawing.Point(7, 204);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(104, 23);
            this.btnLoadFile.TabIndex = 1;
            this.btnLoadFile.Text = "Load file";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // btnSaveFile
            // 
            this.btnSaveFile.Location = new System.Drawing.Point(7, 233);
            this.btnSaveFile.Name = "btnSaveFile";
            this.btnSaveFile.Size = new System.Drawing.Size(104, 23);
            this.btnSaveFile.TabIndex = 1;
            this.btnSaveFile.Text = "Save file";
            this.btnSaveFile.UseVisualStyleBackColor = true;
            this.btnSaveFile.Click += new System.EventHandler(this.btnSaveFile_Click);
            // 
            // tbTopic
            // 
            this.tbTopic.Location = new System.Drawing.Point(7, 71);
            this.tbTopic.Name = "tbTopic";
            this.tbTopic.Size = new System.Drawing.Size(100, 20);
            this.tbTopic.TabIndex = 8;
            this.tbTopic.Text = "smctestdata/tags";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Topic";
            // 
            // SensorDataUpdateTimer
            // 
            this.SensorDataUpdateTimer.Interval = 500;
            this.SensorDataUpdateTimer.Tick += new System.EventHandler(this.updateSensorData_Event);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblServer);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbTopic);
            this.groupBox1.Controls.Add(this.btnConnectMQTT);
            this.groupBox1.Controls.Add(this.tbPassword);
            this.groupBox1.Controls.Add(this.lblUsername);
            this.groupBox1.Controls.Add(this.tbUsername);
            this.groupBox1.Controls.Add(this.btnLoadFile);
            this.groupBox1.Controls.Add(this.tbServer);
            this.groupBox1.Controls.Add(this.btnSaveFile);
            this.groupBox1.Controls.Add(this.lblPassword);
            this.groupBox1.Controls.Add(this.btnDisconnectMQTT);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(118, 268);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            // 
            // sldrTimingMin
            // 
            this.sldrTimingMin.Location = new System.Drawing.Point(12, 520);
            this.sldrTimingMin.Name = "sldrTimingMin";
            this.sldrTimingMin.Size = new System.Drawing.Size(1225, 45);
            this.sldrTimingMin.SmallChange = 100;
            this.sldrTimingMin.TabIndex = 11;
            this.sldrTimingMin.Scroll += new System.EventHandler(this.sliderMin_Slide);
            // 
            // sldrTimingMax
            // 
            this.sldrTimingMax.Location = new System.Drawing.Point(12, 571);
            this.sldrTimingMax.Name = "sldrTimingMax";
            this.sldrTimingMax.Size = new System.Drawing.Size(1225, 45);
            this.sldrTimingMax.SmallChange = 100;
            this.sldrTimingMax.TabIndex = 12;
            this.sldrTimingMax.Scroll += new System.EventHandler(this.sliderMax_Slide);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 501);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Minimum time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 552);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Maximum time";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1245, 628);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sldrTimingMax);
            this.Controls.Add(this.sldrTimingMin);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbData);
            this.Controls.Add(this.clbTags);
            this.Name = "Form1";
            this.Text = "SMC Lib test GUI";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sldrTimingMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sldrTimingMax)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnConnectMQTT;
        private System.Windows.Forms.Button btnDisconnectMQTT;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.CheckedListBox clbTags;
        private System.Windows.Forms.ListBox lbData;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.TextBox tbUsername;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Button btnSaveFile;
        private System.Windows.Forms.TextBox tbTopic;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer SensorDataUpdateTimer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TrackBar sldrTimingMin;
        private System.Windows.Forms.TrackBar sldrTimingMax;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

