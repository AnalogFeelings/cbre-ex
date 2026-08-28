using CBRE.Localization;

namespace CBRE.Editor.Compiling {
    partial class ExportForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportForm));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ambientRed = new System.Windows.Forms.TextBox();
            this.ambientGreen = new System.Windows.Forms.TextBox();
            this.ambientBlue = new System.Windows.Forms.TextBox();
            this.render = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.ambientColorBox = new System.Windows.Forms.Panel();
            this.ProgressLog = new System.Windows.Forms.RichTextBox();
            this.export = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.viewAfterCheckbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.modelBakeYes = new System.Windows.Forms.RadioButton();
            this.modelBakeNo = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.threadCount = new CBRE.UI.UnitedNumericUpDown();
            this.blurRadius = new CBRE.UI.UnitedNumericUpDown();
            this.downscaleFactor = new CBRE.UI.UnitedNumericUpDown();
            this.textureDims = new CBRE.UI.UnitedNumericUpDown();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.threadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.blurRadius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.downscaleFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textureDims)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = Local.LocalString("export.dimensions");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(31, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = Local.LocalString("export.downscale");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(65, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = Local.LocalString("export.blur");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(46, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = Local.LocalString("export.ambient");
            // 
            // ambientRed
            // 
            this.ambientRed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.ambientRed.Location = new System.Drawing.Point(137, 129);
            this.ambientRed.Name = "ambientRed";
            this.ambientRed.Size = new System.Drawing.Size(41, 20);
            this.ambientRed.TabIndex = 8;
            this.ambientRed.TextChanged += new System.EventHandler(this.ambientRed_TextChanged);
            this.ambientRed.LostFocus += new System.EventHandler(this.ambientRed_LostFocus);
            // 
            // ambientGreen
            // 
            this.ambientGreen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.ambientGreen.Location = new System.Drawing.Point(184, 129);
            this.ambientGreen.Name = "ambientGreen";
            this.ambientGreen.Size = new System.Drawing.Size(41, 20);
            this.ambientGreen.TabIndex = 9;
            this.ambientGreen.TextChanged += new System.EventHandler(this.ambientGreen_TextChanged);
            this.ambientGreen.LostFocus += new System.EventHandler(this.ambientGreen_LostFocus);
            // 
            // ambientBlue
            // 
            this.ambientBlue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ambientBlue.Location = new System.Drawing.Point(231, 129);
            this.ambientBlue.Name = "ambientBlue";
            this.ambientBlue.Size = new System.Drawing.Size(41, 20);
            this.ambientBlue.TabIndex = 10;
            this.ambientBlue.TextChanged += new System.EventHandler(this.ambientBlue_TextChanged);
            this.ambientBlue.LostFocus += new System.EventHandler(this.ambientBlue_LostFocus);
            // 
            // render
            // 
            this.render.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.render.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.render.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.render.Location = new System.Drawing.Point(12, 413);
            this.render.Name = "render";
            this.render.Size = new System.Drawing.Size(75, 23);
            this.render.TabIndex = 13;
            this.render.Text = Local.LocalString("export.render");
            this.render.UseVisualStyleBackColor = true;
            this.render.Click += new System.EventHandler(this.render_Click);
            // 
            // cancel
            // 
            this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel.Enabled = false;
            this.cancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancel.Location = new System.Drawing.Point(541, 413);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 16;
            this.cancel.Text = Local.LocalString("cancel");
            this.cancel.UseVisualStyleBackColor = true;
            this.cancel.Click += new System.EventHandler(this.cancel_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar.Enabled = false;
            this.ProgressBar.Location = new System.Drawing.Point(12, 384);
            this.ProgressBar.Maximum = 10000;
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(604, 23);
            this.ProgressBar.TabIndex = 14;
            // 
            // ambientColorBox
            // 
            this.ambientColorBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ambientColorBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ambientColorBox.Location = new System.Drawing.Point(298, 131);
            this.ambientColorBox.Name = "ambientColorBox";
            this.ambientColorBox.Size = new System.Drawing.Size(42, 18);
            this.ambientColorBox.TabIndex = 16;
            this.ambientColorBox.Click += new System.EventHandler(this.ambientColorBox_Click);
            // 
            // ProgressLog
            // 
            this.ProgressLog.BackColor = System.Drawing.SystemColors.Window;
            this.ProgressLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ProgressLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProgressLog.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProgressLog.HideSelection = false;
            this.ProgressLog.Location = new System.Drawing.Point(0, 0);
            this.ProgressLog.Name = "ProgressLog";
            this.ProgressLog.ReadOnly = true;
            this.ProgressLog.Size = new System.Drawing.Size(602, 188);
            this.ProgressLog.TabIndex = 17;
            this.ProgressLog.Text = "";
            // 
            // export
            // 
            this.export.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.export.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.export.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.export.Location = new System.Drawing.Point(93, 413);
            this.export.Name = "export";
            this.export.Size = new System.Drawing.Size(75, 23);
            this.export.TabIndex = 14;
            this.export.Text = Local.LocalString("export.export");
            this.export.UseVisualStyleBackColor = true;
            this.export.Click += new System.EventHandler(this.export_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(278, 132);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "=";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.ProgressLog);
            this.panel1.Location = new System.Drawing.Point(12, 187);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(604, 190);
            this.panel1.TabIndex = 20;
            // 
            // viewAfterCheckbox
            // 
            this.viewAfterCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.viewAfterCheckbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewAfterCheckbox.Location = new System.Drawing.Point(174, 414);
            this.viewAfterCheckbox.Name = "viewAfterCheckbox";
            this.viewAfterCheckbox.Size = new System.Drawing.Size(361, 23);
            this.viewAfterCheckbox.TabIndex = 15;
            this.viewAfterCheckbox.Text = Local.LocalString("export.view_output");
            this.viewAfterCheckbox.UseVisualStyleBackColor = true;
            this.viewAfterCheckbox.CheckedChanged += new System.EventHandler(this.viewAfterCheckbox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 15);
            this.label1.TabIndex = 22;
            this.label1.Text = Local.LocalString("export.bake_shadows");
            // 
            // modelBakeYes
            // 
            this.modelBakeYes.AutoSize = true;
            this.modelBakeYes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modelBakeYes.Location = new System.Drawing.Point(137, 158);
            this.modelBakeYes.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.modelBakeYes.Name = "modelBakeYes";
            this.modelBakeYes.Size = new System.Drawing.Size(42, 19);
            this.modelBakeYes.TabIndex = 11;
            this.modelBakeYes.TabStop = true;
            this.modelBakeYes.Text = Local.LocalString("yes");
            this.modelBakeYes.UseVisualStyleBackColor = true;
            this.modelBakeYes.CheckedChanged += new System.EventHandler(this.modelBakeYes_CheckedChanged);
            // 
            // modelBakeNo
            // 
            this.modelBakeNo.AutoSize = true;
            this.modelBakeNo.Checked = true;
            this.modelBakeNo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.modelBakeNo.Location = new System.Drawing.Point(185, 158);
            this.modelBakeNo.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.modelBakeNo.Name = "modelBakeNo";
            this.modelBakeNo.Size = new System.Drawing.Size(41, 19);
            this.modelBakeNo.TabIndex = 12;
            this.modelBakeNo.TabStop = true;
            this.modelBakeNo.Text = Local.LocalString("no");
            this.modelBakeNo.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(26, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(105, 15);
            this.label7.TabIndex = 24;
            this.label7.Text = Local.LocalString("export.max_thread");
            // 
            // threadCount
            // 
            this.threadCount.AddSpace = true;
            this.threadCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.threadCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.threadCount.Location = new System.Drawing.Point(137, 100);
            this.threadCount.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.threadCount.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.threadCount.Name = "threadCount";
            this.threadCount.SetCaretAtEnd = false;
            this.threadCount.Size = new System.Drawing.Size(479, 23);
            this.threadCount.TabIndex = 7;
            this.threadCount.UnitText = "threads";
            this.threadCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.threadCount.ValueChanged += new System.EventHandler(this.threadCount_ValueChanged);
            // 
            // blurRadius
            // 
            this.blurRadius.AddSpace = false;
            this.blurRadius.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.blurRadius.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blurRadius.Location = new System.Drawing.Point(137, 71);
            this.blurRadius.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.blurRadius.Name = "blurRadius";
            this.blurRadius.SetCaretAtEnd = false;
            this.blurRadius.Size = new System.Drawing.Size(479, 23);
            this.blurRadius.TabIndex = 6;
            this.blurRadius.UnitText = "px";
            this.blurRadius.ValueChanged += new System.EventHandler(this.blurRadius_ValueChanged);
            // 
            // downscaleFactor
            // 
            this.downscaleFactor.AddSpace = false;
            this.downscaleFactor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.downscaleFactor.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.downscaleFactor.Location = new System.Drawing.Point(137, 42);
            this.downscaleFactor.Maximum = new decimal(new int[] {
            128,
            0,
            0,
            0});
            this.downscaleFactor.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.downscaleFactor.Name = "downscaleFactor";
            this.downscaleFactor.SetCaretAtEnd = false;
            this.downscaleFactor.Size = new System.Drawing.Size(479, 23);
            this.downscaleFactor.TabIndex = 5;
            this.downscaleFactor.UnitText = "x";
            this.downscaleFactor.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.downscaleFactor.ValueChanged += new System.EventHandler(this.downscaleFactor_ValueChanged);
            // 
            // textureDims
            // 
            this.textureDims.AddSpace = false;
            this.textureDims.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textureDims.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textureDims.Increment = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.textureDims.Location = new System.Drawing.Point(137, 13);
            this.textureDims.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.textureDims.Minimum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.textureDims.Name = "textureDims";
            this.textureDims.SetCaretAtEnd = false;
            this.textureDims.Size = new System.Drawing.Size(479, 23);
            this.textureDims.TabIndex = 4;
            this.textureDims.UnitText = "px";
            this.textureDims.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.textureDims.ValueChanged += new System.EventHandler(this.textureDims_ValueChanged);
            // 
            // ExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 448);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.threadCount);
            this.Controls.Add(this.blurRadius);
            this.Controls.Add(this.downscaleFactor);
            this.Controls.Add(this.textureDims);
            this.Controls.Add(this.modelBakeNo);
            this.Controls.Add(this.modelBakeYes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.viewAfterCheckbox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.export);
            this.Controls.Add(this.ambientColorBox);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.render);
            this.Controls.Add(this.ambientBlue);
            this.Controls.Add(this.ambientGreen);
            this.Controls.Add(this.ambientRed);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "ExportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = Local.LocalString("export.title");
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formClosing);
            this.Load += new System.EventHandler(this.ExportForm_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.threadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.blurRadius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.downscaleFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textureDims)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.CheckBox viewAfterCheckbox;

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ambientRed;
        private System.Windows.Forms.TextBox ambientGreen;
        private System.Windows.Forms.TextBox ambientBlue;
        private System.Windows.Forms.Button render;
        private System.Windows.Forms.Button cancel;
        public System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Panel ambientColorBox;
        public System.Windows.Forms.RichTextBox ProgressLog;
        private System.Windows.Forms.Button export;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton modelBakeYes;
        private System.Windows.Forms.RadioButton modelBakeNo;
        private CBRE.UI.UnitedNumericUpDown textureDims;
        private CBRE.UI.UnitedNumericUpDown downscaleFactor;
        private CBRE.UI.UnitedNumericUpDown blurRadius;
        private CBRE.UI.UnitedNumericUpDown threadCount;
        private System.Windows.Forms.Label label7;
    }
}
