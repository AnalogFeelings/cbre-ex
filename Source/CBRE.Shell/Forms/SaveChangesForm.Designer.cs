namespace CBRE.Shell.Forms
{
    partial class SaveChangesForm
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
            this.CancelButton = new System.Windows.Forms.Button();
            this.DiscardButton = new System.Windows.Forms.Button();
            this.SaveAllButton = new System.Windows.Forms.Button();
            this.DocumentList = new System.Windows.Forms.ListBox();
            this.UnsavedChangesLabel = new System.Windows.Forms.Label();
            this.systemBitmap = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.systemBitmap)).BeginInit();
            this.SuspendLayout();
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CancelButton.Location = new System.Drawing.Point(269, 185);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 0;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelClicked);
            // 
            // DiscardButton
            // 
            this.DiscardButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DiscardButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DiscardButton.Location = new System.Drawing.Point(188, 185);
            this.DiscardButton.Name = "DiscardButton";
            this.DiscardButton.Size = new System.Drawing.Size(75, 23);
            this.DiscardButton.TabIndex = 0;
            this.DiscardButton.Text = "Discard all";
            this.DiscardButton.UseVisualStyleBackColor = true;
            this.DiscardButton.Click += new System.EventHandler(this.DiscardAllClicked);
            // 
            // SaveAllButton
            // 
            this.SaveAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveAllButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveAllButton.Location = new System.Drawing.Point(107, 185);
            this.SaveAllButton.Name = "SaveAllButton";
            this.SaveAllButton.Size = new System.Drawing.Size(75, 23);
            this.SaveAllButton.TabIndex = 0;
            this.SaveAllButton.Text = "Save all";
            this.SaveAllButton.UseVisualStyleBackColor = true;
            this.SaveAllButton.Click += new System.EventHandler(this.SaveAllClicked);
            // 
            // DocumentList
            // 
            this.DocumentList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DocumentList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DocumentList.FormattingEnabled = true;
            this.DocumentList.IntegralHeight = false;
            this.DocumentList.ItemHeight = 15;
            this.DocumentList.Location = new System.Drawing.Point(12, 56);
            this.DocumentList.Name = "DocumentList";
            this.DocumentList.Size = new System.Drawing.Size(332, 123);
            this.DocumentList.TabIndex = 1;
            // 
            // UnsavedChangesLabel
            // 
            this.UnsavedChangesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UnsavedChangesLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UnsavedChangesLabel.Location = new System.Drawing.Point(50, 12);
            this.UnsavedChangesLabel.Name = "UnsavedChangesLabel";
            this.UnsavedChangesLabel.Size = new System.Drawing.Size(294, 32);
            this.UnsavedChangesLabel.TabIndex = 2;
            this.UnsavedChangesLabel.Text = "Some documents have unsaved changes. Would you like to save or discard these chan" +
    "ges?";
            this.UnsavedChangesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // systemBitmap
            // 
            this.systemBitmap.Location = new System.Drawing.Point(12, 12);
            this.systemBitmap.Margin = new System.Windows.Forms.Padding(3, 3, 3, 9);
            this.systemBitmap.Name = "systemBitmap";
            this.systemBitmap.Size = new System.Drawing.Size(32, 32);
            this.systemBitmap.TabIndex = 14;
            this.systemBitmap.TabStop = false;
            // 
            // SaveChangesForm
            // 
            this.AcceptButton = this.SaveAllButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 220);
            this.Controls.Add(this.systemBitmap);
            this.Controls.Add(this.UnsavedChangesLabel);
            this.Controls.Add(this.DocumentList);
            this.Controls.Add(this.SaveAllButton);
            this.Controls.Add(this.DiscardButton);
            this.Controls.Add(this.CancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaveChangesForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Unsaved changes";
            ((System.ComponentModel.ISupportInitialize)(this.systemBitmap)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button DiscardButton;
        private System.Windows.Forms.Button SaveAllButton;
        private System.Windows.Forms.ListBox DocumentList;
        private System.Windows.Forms.Label UnsavedChangesLabel;
        private System.Windows.Forms.PictureBox systemBitmap;
    }
}