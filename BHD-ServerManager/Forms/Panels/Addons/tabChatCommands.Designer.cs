namespace BHD_ServerManager.Forms.Panels.Addons
{
    partial class tabChatCommands
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            flowLayoutPanel1 = new FlowLayoutPanel();
            groupBox_skipMaps = new GroupBox();
            cb_ccEnableStartDelaySkipMap = new CheckBox();
            label2 = new Label();
            label4 = new Label();
            num_SkipPercentRequired = new NumericUpDown();
            num_SkipVotingMaxSessions = new NumericUpDown();
            label3 = new Label();
            num_SkipVotingStarts = new NumericUpDown();
            label1 = new Label();
            num_SkipVotingPeriod = new NumericUpDown();
            cb_ccEnableSkipping = new CheckBox();
            toolTip1 = new ToolTip(components);
            panel1 = new Panel();
            btn_ChatCommandSave = new Button();
            btn_ChatCommandsReset = new Button();
            cb_ccEnableInGameSkipMap = new CheckBox();
            flowLayoutPanel1.SuspendLayout();
            groupBox_skipMaps.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)num_SkipPercentRequired).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_SkipVotingMaxSessions).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_SkipVotingStarts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num_SkipVotingPeriod).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(groupBox_skipMaps);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(894, 334);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // groupBox_skipMaps
            // 
            groupBox_skipMaps.Controls.Add(label2);
            groupBox_skipMaps.Controls.Add(cb_ccEnableInGameSkipMap);
            groupBox_skipMaps.Controls.Add(cb_ccEnableStartDelaySkipMap);
            groupBox_skipMaps.Controls.Add(label4);
            groupBox_skipMaps.Controls.Add(num_SkipPercentRequired);
            groupBox_skipMaps.Controls.Add(num_SkipVotingMaxSessions);
            groupBox_skipMaps.Controls.Add(label3);
            groupBox_skipMaps.Controls.Add(num_SkipVotingStarts);
            groupBox_skipMaps.Controls.Add(label1);
            groupBox_skipMaps.Controls.Add(num_SkipVotingPeriod);
            groupBox_skipMaps.Controls.Add(cb_ccEnableSkipping);
            groupBox_skipMaps.Location = new Point(3, 3);
            groupBox_skipMaps.Name = "groupBox_skipMaps";
            groupBox_skipMaps.Size = new Size(247, 220);
            groupBox_skipMaps.TabIndex = 0;
            groupBox_skipMaps.TabStop = false;
            groupBox_skipMaps.Text = "Voting: Skip Map";
            // 
            // cb_ccEnableStartDelaySkipMap
            // 
            cb_ccEnableStartDelaySkipMap.AutoSize = true;
            cb_ccEnableStartDelaySkipMap.CheckAlign = ContentAlignment.MiddleRight;
            cb_ccEnableStartDelaySkipMap.Location = new Point(63, 45);
            cb_ccEnableStartDelaySkipMap.Name = "cb_ccEnableStartDelaySkipMap";
            cb_ccEnableStartDelaySkipMap.Size = new Size(171, 19);
            cb_ccEnableStartDelaySkipMap.TabIndex = 9;
            cb_ccEnableStartDelaySkipMap.Text = "Start Delay Skip Map Voting";
            toolTip1.SetToolTip(cb_ccEnableStartDelaySkipMap, "Allow players, to vote to \"skip\" the current map during the start delay.");
            cb_ccEnableStartDelaySkipMap.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 184);
            label2.Name = "label2";
            label2.Size = new Size(170, 15);
            label2.TabIndex = 4;
            label2.Text = "Required # of Votes to Pass (%)";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(67, 157);
            label4.Name = "label4";
            label4.Size = new Size(113, 15);
            label4.TabIndex = 8;
            label4.Text = "Max Voting Sessions";
            // 
            // num_SkipPercentRequired
            // 
            num_SkipPercentRequired.Location = new Point(186, 182);
            num_SkipPercentRequired.Name = "num_SkipPercentRequired";
            num_SkipPercentRequired.Size = new Size(48, 23);
            num_SkipPercentRequired.TabIndex = 3;
            num_SkipPercentRequired.TextAlign = HorizontalAlignment.Center;
            toolTip1.SetToolTip(num_SkipPercentRequired, "Number of Minutes to Collect Votes");
            num_SkipPercentRequired.Value = new decimal(new int[] { 51, 0, 0, 0 });
            // 
            // num_SkipVotingMaxSessions
            // 
            num_SkipVotingMaxSessions.Location = new Point(186, 153);
            num_SkipVotingMaxSessions.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            num_SkipVotingMaxSessions.Name = "num_SkipVotingMaxSessions";
            num_SkipVotingMaxSessions.Size = new Size(48, 23);
            num_SkipVotingMaxSessions.TabIndex = 7;
            num_SkipVotingMaxSessions.TextAlign = HorizontalAlignment.Center;
            toolTip1.SetToolTip(num_SkipVotingMaxSessions, "Number of Minutes to Collect Votes");
            num_SkipVotingMaxSessions.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(29, 99);
            label3.Name = "label3";
            label3.Size = new Size(151, 15);
            label3.TabIndex = 6;
            label3.Text = "Start Voting After (Minutes)";
            // 
            // num_SkipVotingStarts
            // 
            num_SkipVotingStarts.Location = new Point(186, 95);
            num_SkipVotingStarts.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            num_SkipVotingStarts.Name = "num_SkipVotingStarts";
            num_SkipVotingStarts.Size = new Size(48, 23);
            num_SkipVotingStarts.TabIndex = 5;
            num_SkipVotingStarts.TextAlign = HorizontalAlignment.Center;
            toolTip1.SetToolTip(num_SkipVotingStarts, "Number of Minutes to Collect Votes");
            num_SkipVotingStarts.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(48, 128);
            label1.Name = "label1";
            label1.Size = new Size(132, 15);
            label1.TabIndex = 2;
            label1.Text = "Voting Period (Minutes)";
            // 
            // num_SkipVotingPeriod
            // 
            num_SkipVotingPeriod.Location = new Point(186, 124);
            num_SkipVotingPeriod.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            num_SkipVotingPeriod.Name = "num_SkipVotingPeriod";
            num_SkipVotingPeriod.Size = new Size(48, 23);
            num_SkipVotingPeriod.TabIndex = 1;
            num_SkipVotingPeriod.TextAlign = HorizontalAlignment.Center;
            toolTip1.SetToolTip(num_SkipVotingPeriod, "Number of Minutes to Collect Votes");
            num_SkipVotingPeriod.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // cb_ccEnableSkipping
            // 
            cb_ccEnableSkipping.AutoSize = true;
            cb_ccEnableSkipping.CheckAlign = ContentAlignment.MiddleRight;
            cb_ccEnableSkipping.Location = new Point(97, 20);
            cb_ccEnableSkipping.Name = "cb_ccEnableSkipping";
            cb_ccEnableSkipping.Size = new Size(137, 19);
            cb_ccEnableSkipping.TabIndex = 0;
            cb_ccEnableSkipping.Text = "Enable Map Skipping";
            cb_ccEnableSkipping.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(btn_ChatCommandSave);
            panel1.Controls.Add(btn_ChatCommandsReset);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 305);
            panel1.Name = "panel1";
            panel1.Size = new Size(894, 29);
            panel1.TabIndex = 1;
            // 
            // btn_ChatCommandSave
            // 
            btn_ChatCommandSave.Location = new Point(84, 3);
            btn_ChatCommandSave.Name = "btn_ChatCommandSave";
            btn_ChatCommandSave.Size = new Size(75, 23);
            btn_ChatCommandSave.TabIndex = 1;
            btn_ChatCommandSave.Text = "Save";
            btn_ChatCommandSave.UseVisualStyleBackColor = true;
            btn_ChatCommandSave.Click += actionClick_SaveSettings;
            // 
            // btn_ChatCommandsReset
            // 
            btn_ChatCommandsReset.Location = new Point(3, 3);
            btn_ChatCommandsReset.Name = "btn_ChatCommandsReset";
            btn_ChatCommandsReset.Size = new Size(75, 23);
            btn_ChatCommandsReset.TabIndex = 0;
            btn_ChatCommandsReset.Text = "Reset";
            btn_ChatCommandsReset.UseVisualStyleBackColor = true;
            btn_ChatCommandsReset.Click += actionClick_ResetSettings;
            // 
            // cb_ccEnableInGameSkipMap
            // 
            cb_ccEnableInGameSkipMap.AutoSize = true;
            cb_ccEnableInGameSkipMap.CheckAlign = ContentAlignment.MiddleRight;
            cb_ccEnableInGameSkipMap.Location = new Point(73, 70);
            cb_ccEnableInGameSkipMap.Name = "cb_ccEnableInGameSkipMap";
            cb_ccEnableInGameSkipMap.Size = new Size(161, 19);
            cb_ccEnableInGameSkipMap.TabIndex = 10;
            cb_ccEnableInGameSkipMap.Text = "In-Game Skip Map Voting";
            toolTip1.SetToolTip(cb_ccEnableInGameSkipMap, "Allow players, to vote to \"skip\" the current map during the start delay.");
            cb_ccEnableInGameSkipMap.UseVisualStyleBackColor = true;
            // 
            // tabChatCommands
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel1);
            Controls.Add(flowLayoutPanel1);
            MaximumSize = new Size(894, 334);
            MinimumSize = new Size(894, 334);
            Name = "tabChatCommands";
            Size = new Size(894, 334);
            flowLayoutPanel1.ResumeLayout(false);
            groupBox_skipMaps.ResumeLayout(false);
            groupBox_skipMaps.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)num_SkipPercentRequired).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_SkipVotingMaxSessions).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_SkipVotingStarts).EndInit();
            ((System.ComponentModel.ISupportInitialize)num_SkipVotingPeriod).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private GroupBox groupBox_skipMaps;
        private CheckBox cb_ccEnableSkipping;
        private Label label2;
        private NumericUpDown num_SkipPercentRequired;
        private ToolTip toolTip1;
        private Label label1;
        private NumericUpDown num_SkipVotingPeriod;
        private Panel panel1;
        private Button btn_ChatCommandSave;
        private Button btn_ChatCommandsReset;
        private Label label4;
        private NumericUpDown num_SkipVotingMaxSessions;
        private Label label3;
        private NumericUpDown num_SkipVotingStarts;
        private CheckBox cb_ccEnableStartDelaySkipMap;
        private CheckBox cb_ccEnableInGameSkipMap;
    }
}
