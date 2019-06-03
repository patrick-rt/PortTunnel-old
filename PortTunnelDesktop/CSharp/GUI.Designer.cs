namespace PortTunnel_Desktop
{
    partial class GUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.selectSS = new System.Windows.Forms.RadioButton();
            this.selectCC = new System.Windows.Forms.RadioButton();
            this.selectSC = new System.Windows.Forms.RadioButton();
            this.txtHost1 = new System.Windows.Forms.TextBox();
            this.txtHost2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.intPort1 = new System.Windows.Forms.TextBox();
            this.intPort2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.intCP = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.intTimeout = new System.Windows.Forms.TextBox();
            this.cbSsl = new System.Windows.Forms.CheckBox();
            this.cbHttp = new System.Windows.Forms.CheckBox();
            this.txtActiveConnections = new System.Windows.Forms.Label();
            this.toolTipHelp = new System.Windows.Forms.ToolTip(this.components);
            this.cbStreamLog = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection 1:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Connection 2:";
            // 
            // selectSS
            // 
            this.selectSS.AutoSize = true;
            this.selectSS.ForeColor = System.Drawing.Color.White;
            this.selectSS.Location = new System.Drawing.Point(266, 12);
            this.selectSS.Name = "selectSS";
            this.selectSS.Size = new System.Drawing.Size(120, 17);
            this.selectSS.TabIndex = 0;
            this.selectSS.Text = "Server-Server-Mode";
            this.toolTipHelp.SetToolTip(this.selectSS, "Uses Connection 1 port as port for incomming requests\r\nUses Connection 2 port as " +
        "port for PortTunnel server");
            this.selectSS.UseVisualStyleBackColor = true;
            this.selectSS.CheckedChanged += new System.EventHandler(this.selectSS_CheckedChanged);
            // 
            // selectCC
            // 
            this.selectCC.AutoSize = true;
            this.selectCC.ForeColor = System.Drawing.Color.White;
            this.selectCC.Location = new System.Drawing.Point(145, 12);
            this.selectCC.Name = "selectCC";
            this.selectCC.Size = new System.Drawing.Size(110, 17);
            this.selectCC.TabIndex = 0;
            this.selectCC.Text = "Client-Client-Mode";
            this.toolTipHelp.SetToolTip(this.selectCC, "Uses Connection 1 as destination server with port\r\nUses Connection 2 as PortTunne" +
        "l destination server with port ");
            this.selectCC.UseVisualStyleBackColor = true;
            this.selectCC.CheckedChanged += new System.EventHandler(this.selectCC_CheckedChanged);
            // 
            // selectSC
            // 
            this.selectSC.AutoSize = true;
            this.selectSC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.selectSC.Checked = true;
            this.selectSC.ForeColor = System.Drawing.Color.White;
            this.selectSC.Location = new System.Drawing.Point(19, 12);
            this.selectSC.Name = "selectSC";
            this.selectSC.Size = new System.Drawing.Size(115, 17);
            this.selectSC.TabIndex = 0;
            this.selectSC.TabStop = true;
            this.selectSC.Text = "Server-Client-Mode";
            this.toolTipHelp.SetToolTip(this.selectSC, "Uses Connection 1 as destination server with port\r\nUses Connection 2 port as inte" +
        "rnal port\r\nControl-Port can be empty");
            this.selectSC.UseVisualStyleBackColor = false;
            this.selectSC.CheckedChanged += new System.EventHandler(this.selectSC_CheckedChanged);
            // 
            // txtHost1
            // 
            this.txtHost1.BackColor = System.Drawing.Color.Silver;
            this.txtHost1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHost1.Location = new System.Drawing.Point(91, 46);
            this.txtHost1.Name = "txtHost1";
            this.txtHost1.Size = new System.Drawing.Size(137, 20);
            this.txtHost1.TabIndex = 1;
            this.txtHost1.Text = "illumina-chemie.de";
            this.toolTipHelp.SetToolTip(this.txtHost1, "Destination server");
            // 
            // txtHost2
            // 
            this.txtHost2.BackColor = System.Drawing.Color.Silver;
            this.txtHost2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHost2.Location = new System.Drawing.Point(91, 72);
            this.txtHost2.Name = "txtHost2";
            this.txtHost2.Size = new System.Drawing.Size(137, 20);
            this.txtHost2.TabIndex = 3;
            this.txtHost2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(234, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Port:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(234, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Port:";
            // 
            // intPort1
            // 
            this.intPort1.BackColor = System.Drawing.Color.Silver;
            this.intPort1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.intPort1.Location = new System.Drawing.Point(269, 46);
            this.intPort1.Name = "intPort1";
            this.intPort1.Size = new System.Drawing.Size(67, 20);
            this.intPort1.TabIndex = 2;
            this.intPort1.Text = "80";
            this.toolTipHelp.SetToolTip(this.intPort1, "Port of server");
            // 
            // intPort2
            // 
            this.intPort2.BackColor = System.Drawing.Color.Silver;
            this.intPort2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.intPort2.Location = new System.Drawing.Point(269, 72);
            this.intPort2.Name = "intPort2";
            this.intPort2.Size = new System.Drawing.Size(67, 20);
            this.intPort2.TabIndex = 4;
            this.intPort2.Text = "80";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(12, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Control-Port:";
            // 
            // intCP
            // 
            this.intCP.BackColor = System.Drawing.Color.Silver;
            this.intCP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.intCP.Location = new System.Drawing.Point(91, 98);
            this.intCP.Name = "intCP";
            this.intCP.Size = new System.Drawing.Size(67, 20);
            this.intCP.TabIndex = 5;
            this.toolTipHelp.SetToolTip(this.intCP, "Communication port for PortTunnel (Server defined as Connection 2)");
            this.intCP.Visible = false;
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Gray;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.ForeColor = System.Drawing.Color.White;
            this.btnStart.Location = new System.Drawing.Point(12, 141);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(175, 23);
            this.btnStart.TabIndex = 9;
            this.btnStart.Text = "Start";
            this.toolTipHelp.SetToolTip(this.btnStart, "Start PortTunnel");
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.Gray;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.ForeColor = System.Drawing.Color.White;
            this.btnStop.Location = new System.Drawing.Point(216, 141);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(175, 23);
            this.btnStop.TabIndex = 10;
            this.btnStop.Text = "Stop";
            this.toolTipHelp.SetToolTip(this.btnStop, "Stop PortTunnel and close window");
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.Silver;
            this.txtLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtLog.Location = new System.Drawing.Point(12, 170);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(379, 225);
            this.txtLog.TabIndex = 0;
            this.toolTipHelp.SetToolTip(this.txtLog, "PortTunnel Log");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(215, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Timeout:";
            // 
            // intTimeout
            // 
            this.intTimeout.BackColor = System.Drawing.Color.Silver;
            this.intTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.intTimeout.Location = new System.Drawing.Point(269, 98);
            this.intTimeout.Name = "intTimeout";
            this.intTimeout.Size = new System.Drawing.Size(67, 20);
            this.intTimeout.TabIndex = 8;
            this.intTimeout.Text = "3000";
            this.toolTipHelp.SetToolTip(this.intTimeout, "Timeout for each tunnel");
            // 
            // cbSsl
            // 
            this.cbSsl.AutoSize = true;
            this.cbSsl.ForeColor = System.Drawing.Color.White;
            this.cbSsl.Location = new System.Drawing.Point(342, 73);
            this.cbSsl.Name = "cbSsl";
            this.cbSsl.Size = new System.Drawing.Size(46, 17);
            this.cbSsl.TabIndex = 7;
            this.cbSsl.Text = "SSL";
            this.toolTipHelp.SetToolTip(this.cbSsl, "Server uses SSL");
            this.cbSsl.UseVisualStyleBackColor = true;
            // 
            // cbHttp
            // 
            this.cbHttp.AutoSize = true;
            this.cbHttp.ForeColor = System.Drawing.Color.White;
            this.cbHttp.Location = new System.Drawing.Point(342, 48);
            this.cbHttp.Name = "cbHttp";
            this.cbHttp.Size = new System.Drawing.Size(55, 17);
            this.cbHttp.TabIndex = 6;
            this.cbHttp.Text = "HTTP";
            this.toolTipHelp.SetToolTip(this.cbHttp, "Server uses HTTP");
            this.cbHttp.UseVisualStyleBackColor = true;
            // 
            // txtActiveConnections
            // 
            this.txtActiveConnections.AutoSize = true;
            this.txtActiveConnections.ForeColor = System.Drawing.Color.White;
            this.txtActiveConnections.Location = new System.Drawing.Point(174, 101);
            this.txtActiveConnections.Name = "txtActiveConnections";
            this.txtActiveConnections.Size = new System.Drawing.Size(13, 13);
            this.txtActiveConnections.TabIndex = 20;
            this.txtActiveConnections.Text = "0";
            this.toolTipHelp.SetToolTip(this.txtActiveConnections, "Active tunnels");
            // 
            // cbStreamLog
            // 
            this.cbStreamLog.AutoSize = true;
            this.cbStreamLog.ForeColor = System.Drawing.Color.White;
            this.cbStreamLog.Location = new System.Drawing.Point(342, 99);
            this.cbStreamLog.Name = "cbStreamLog";
            this.cbStreamLog.Size = new System.Drawing.Size(44, 17);
            this.cbStreamLog.TabIndex = 21;
            this.cbStreamLog.Text = "Log";
            this.toolTipHelp.SetToolTip(this.cbStreamLog, "Enable / Disable logging of tunnel streams\r\nLogs are saved to ./Log/");
            this.cbStreamLog.UseVisualStyleBackColor = true;
            // 
            // GUI
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(403, 411);
            this.Controls.Add(this.cbStreamLog);
            this.Controls.Add(this.txtActiveConnections);
            this.Controls.Add(this.cbHttp);
            this.Controls.Add(this.cbSsl);
            this.Controls.Add(this.intTimeout);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.intCP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.intPort2);
            this.Controls.Add(this.intPort1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtHost2);
            this.Controls.Add(this.txtHost1);
            this.Controls.Add(this.selectSC);
            this.Controls.Add(this.selectCC);
            this.Controls.Add(this.selectSS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(419, 450);
            this.MinimumSize = new System.Drawing.Size(419, 450);
            this.Name = "GUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PortTunnel";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GUI_FormClosing);
            this.Load += new System.EventHandler(this.GUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton selectSS;
        private System.Windows.Forms.RadioButton selectCC;
        private System.Windows.Forms.RadioButton selectSC;
        private System.Windows.Forms.TextBox txtHost1;
        private System.Windows.Forms.TextBox txtHost2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox intPort1;
        private System.Windows.Forms.TextBox intPort2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox intCP;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        public System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox intTimeout;
        private System.Windows.Forms.CheckBox cbSsl;
        private System.Windows.Forms.CheckBox cbHttp;
        private System.Windows.Forms.Label txtActiveConnections;
        private System.Windows.Forms.ToolTip toolTipHelp;
        private System.Windows.Forms.CheckBox cbStreamLog;
    }
}
