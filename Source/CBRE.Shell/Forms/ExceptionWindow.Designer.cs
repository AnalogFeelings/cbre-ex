namespace CBRE.Shell.Forms
{
    partial class ExceptionWindow
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
            this.systemBitmap = new System.Windows.Forms.PictureBox();
            this.headerLabel = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.reportButton = new System.Windows.Forms.Button();
            this.copyButton = new System.Windows.Forms.Button();
            this.fullError = new CBRE.Shell.Controls.ReadOnlyTextBox();
            this.editorVersion = new CBRE.Shell.Controls.ReadOnlyTextBox();
            this.runtimeVersion = new CBRE.Shell.Controls.ReadOnlyTextBox();
            this.operatingSystem = new CBRE.Shell.Controls.ReadOnlyTextBox();
            this.availableMemory = new CBRE.Shell.Controls.ReadOnlyTextBox();
            this.processorName = new CBRE.Shell.Controls.ReadOnlyTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.systemBitmap)).BeginInit();
            this.SuspendLayout();
            // 
            // systemBitmap
            // 
            this.systemBitmap.Location = new System.Drawing.Point(12, 12);
            this.systemBitmap.Margin = new System.Windows.Forms.Padding(3, 3, 3, 9);
            this.systemBitmap.Name = "systemBitmap";
            this.systemBitmap.Size = new System.Drawing.Size(32, 32);
            this.systemBitmap.TabIndex = 13;
            this.systemBitmap.TabStop = false;
            // 
            // headerLabel
            // 
            this.headerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headerLabel.Location = new System.Drawing.Point(50, 12);
            this.headerLabel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(732, 32);
            this.headerLabel.TabIndex = 14;
            this.headerLabel.Text = "CBRE-EX has encountered an error but couldn\'t recover from it. CBRE-EX will try t" +
    "o continue running. More information below.\r\n";
            this.headerLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 15);
            this.label4.TabIndex = 16;
            this.label4.Text = "System Processor";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 15);
            this.label1.TabIndex = 18;
            this.label1.Text = "Available Memory";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(10, 113);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 15);
            this.label5.TabIndex = 20;
            this.label5.Text = "Operating System";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(39, 142);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = ".NET Version";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(17, 171);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 15);
            this.label6.TabIndex = 24;
            this.label6.Text = "CBRE-EX Version";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(8, 198);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(103, 15);
            this.label7.TabIndex = 26;
            this.label7.Text = "Full Error Message";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cancelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelButton.Location = new System.Drawing.Point(8, 456);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(103, 23);
            this.cancelButton.TabIndex = 27;
            this.cancelButton.Text = "Close";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // reportButton
            // 
            this.reportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.reportButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.reportButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reportButton.Location = new System.Drawing.Point(8, 427);
            this.reportButton.Name = "reportButton";
            this.reportButton.Size = new System.Drawing.Size(103, 23);
            this.reportButton.TabIndex = 28;
            this.reportButton.Text = "Report Bug...";
            this.reportButton.UseVisualStyleBackColor = true;
            this.reportButton.Click += new System.EventHandler(this.reportButton_Click);
            // 
            // copyButton
            // 
            this.copyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.copyButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.copyButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyButton.Location = new System.Drawing.Point(8, 398);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(103, 23);
            this.copyButton.TabIndex = 29;
            this.copyButton.Text = "Copy Error";
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.copyButton_Click);
            // 
            // fullError
            // 
            this.fullError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fullError.BackColor = System.Drawing.SystemColors.Window;
            this.fullError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fullError.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fullError.Location = new System.Drawing.Point(117, 198);
            this.fullError.Multiline = true;
            this.fullError.Name = "fullError";
            this.fullError.ReadOnly = true;
            this.fullError.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.fullError.Size = new System.Drawing.Size(665, 281);
            this.fullError.TabIndex = 25;
            // 
            // editorVersion
            // 
            this.editorVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editorVersion.BackColor = System.Drawing.SystemColors.Window;
            this.editorVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.editorVersion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.editorVersion.Location = new System.Drawing.Point(117, 169);
            this.editorVersion.Name = "editorVersion";
            this.editorVersion.ReadOnly = true;
            this.editorVersion.Size = new System.Drawing.Size(665, 23);
            this.editorVersion.TabIndex = 23;
            // 
            // runtimeVersion
            // 
            this.runtimeVersion.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.runtimeVersion.BackColor = System.Drawing.SystemColors.Window;
            this.runtimeVersion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.runtimeVersion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runtimeVersion.Location = new System.Drawing.Point(117, 140);
            this.runtimeVersion.Name = "runtimeVersion";
            this.runtimeVersion.ReadOnly = true;
            this.runtimeVersion.Size = new System.Drawing.Size(665, 23);
            this.runtimeVersion.TabIndex = 21;
            // 
            // operatingSystem
            // 
            this.operatingSystem.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.operatingSystem.BackColor = System.Drawing.SystemColors.Window;
            this.operatingSystem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.operatingSystem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.operatingSystem.Location = new System.Drawing.Point(117, 111);
            this.operatingSystem.Name = "operatingSystem";
            this.operatingSystem.ReadOnly = true;
            this.operatingSystem.Size = new System.Drawing.Size(665, 23);
            this.operatingSystem.TabIndex = 19;
            // 
            // availableMemory
            // 
            this.availableMemory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.availableMemory.BackColor = System.Drawing.SystemColors.Window;
            this.availableMemory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.availableMemory.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.availableMemory.Location = new System.Drawing.Point(117, 82);
            this.availableMemory.Name = "availableMemory";
            this.availableMemory.ReadOnly = true;
            this.availableMemory.Size = new System.Drawing.Size(665, 23);
            this.availableMemory.TabIndex = 17;
            // 
            // processorName
            // 
            this.processorName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.processorName.BackColor = System.Drawing.SystemColors.Window;
            this.processorName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.processorName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processorName.Location = new System.Drawing.Point(117, 53);
            this.processorName.Name = "processorName";
            this.processorName.ReadOnly = true;
            this.processorName.Size = new System.Drawing.Size(665, 23);
            this.processorName.TabIndex = 15;
            // 
            // ExceptionWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 491);
            this.Controls.Add(this.copyButton);
            this.Controls.Add(this.reportButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.fullError);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.editorVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.runtimeVersion);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.operatingSystem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.availableMemory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.processorName);
            this.Controls.Add(this.headerLabel);
            this.Controls.Add(this.systemBitmap);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(810, 530);
            this.Name = "ExceptionWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Error!";
            ((System.ComponentModel.ISupportInitialize)(this.systemBitmap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox systemBitmap;
        private System.Windows.Forms.Label headerLabel;
        private Controls.ReadOnlyTextBox processorName;
        private System.Windows.Forms.Label label4;
        private Controls.ReadOnlyTextBox availableMemory;
        private System.Windows.Forms.Label label1;
        private Controls.ReadOnlyTextBox operatingSystem;
        private System.Windows.Forms.Label label5;
        private Controls.ReadOnlyTextBox runtimeVersion;
        private System.Windows.Forms.Label label2;
        private Controls.ReadOnlyTextBox editorVersion;
        private System.Windows.Forms.Label label6;
        private Controls.ReadOnlyTextBox fullError;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button reportButton;
        private System.Windows.Forms.Button copyButton;
    }
}