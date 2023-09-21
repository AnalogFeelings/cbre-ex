namespace CBRE.BspEditor.Editing.Components
{
    partial class AboutDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.CreditsBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::CBRE.BspEditor.Editing.Properties.Resources.CBRE_Icon;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(493, 202);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 217);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(493, 41);
            this.label1.TabIndex = 3;
            this.label1.Text = "CBRE-EX";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // VersionLabel
            // 
            this.VersionLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.VersionLabel.Location = new System.Drawing.Point(12, 258);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(493, 20);
            this.VersionLabel.TabIndex = 4;
            this.VersionLabel.Text = "Version (version)";
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label11.Location = new System.Drawing.Point(18, 287);
            this.label11.Margin = new System.Windows.Forms.Padding(9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(481, 2);
            this.label11.TabIndex = 9;
            // 
            // CreditsBox
            // 
            this.CreditsBox.BackColor = System.Drawing.SystemColors.Window;
            this.CreditsBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CreditsBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreditsBox.Location = new System.Drawing.Point(12, 298);
            this.CreditsBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.CreditsBox.Name = "CreditsBox";
            this.CreditsBox.ReadOnly = true;
            this.CreditsBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.CreditsBox.Size = new System.Drawing.Size(493, 196);
            this.CreditsBox.TabIndex = 13;
            this.CreditsBox.TabStop = false;
            this.CreditsBox.Text = resources.GetString("CreditsBox.Text");
            // 
            // AboutDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 506);
            this.Controls.Add(this.CreditsBox);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.VersionLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About CBRE-EX";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RichTextBox CreditsBox;
    }
}