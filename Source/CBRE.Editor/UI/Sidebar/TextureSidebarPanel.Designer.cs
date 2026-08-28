using CBRE.Localization;

namespace CBRE.Editor.UI.Sidebar
{
    partial class TextureSidebarPanel
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
			this.BrowseButton = new System.Windows.Forms.Button();
			this.ReplaceButton = new System.Windows.Forms.Button();
			this.GroupComboBox = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.ApplyButton = new System.Windows.Forms.Button();
			this.SelectionPictureBox = new System.Windows.Forms.PictureBox();
			this.SizeLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.TextureComboBox = new CBRE.Editor.UI.TextureComboBox();
			((System.ComponentModel.ISupportInitialize)(this.SelectionPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// BrowseButton
			// 
			this.BrowseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.BrowseButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BrowseButton.Location = new System.Drawing.Point(131, 151);
			this.BrowseButton.Name = "BrowseButton";
			this.BrowseButton.Size = new System.Drawing.Size(66, 22);
			this.BrowseButton.TabIndex = 11;
			this.BrowseButton.Text = Local.LocalString("browse");
			this.BrowseButton.UseVisualStyleBackColor = true;
			this.BrowseButton.Click += new System.EventHandler(this.BrowseButtonClicked);
			// 
			// ReplaceButton
			// 
			this.ReplaceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ReplaceButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ReplaceButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ReplaceButton.Location = new System.Drawing.Point(130, 179);
			this.ReplaceButton.Name = "ReplaceButton";
			this.ReplaceButton.Size = new System.Drawing.Size(66, 22);
			this.ReplaceButton.TabIndex = 12;
			this.ReplaceButton.Text = Local.LocalString("replace");
			this.ReplaceButton.UseVisualStyleBackColor = true;
			this.ReplaceButton.Click += new System.EventHandler(this.ReplaceButtonClicked);
			// 
			// GroupComboBox
			// 
			this.GroupComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.GroupComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.GroupComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.GroupComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GroupComboBox.FormattingEnabled = true;
			this.GroupComboBox.Location = new System.Drawing.Point(60, 5);
			this.GroupComboBox.Name = "GroupComboBox";
			this.GroupComboBox.Size = new System.Drawing.Size(137, 23);
			this.GroupComboBox.TabIndex = 8;
			this.GroupComboBox.SelectedIndexChanged += new System.EventHandler(this.GroupChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(9, 36);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(45, 15);
			this.label5.TabIndex = 6;
			this.label5.Text = Local.LocalString("sidebar.texture");
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(14, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 15);
			this.label4.TabIndex = 7;
			this.label4.Text = Local.LocalString("sidebar.group");
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ApplyButton
			// 
			this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ApplyButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ApplyButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ApplyButton.Location = new System.Drawing.Point(130, 123);
			this.ApplyButton.Name = "ApplyButton";
			this.ApplyButton.Size = new System.Drawing.Size(66, 22);
			this.ApplyButton.TabIndex = 11;
			this.ApplyButton.Text = Local.LocalString("apply");
			this.ApplyButton.UseVisualStyleBackColor = true;
			this.ApplyButton.Click += new System.EventHandler(this.ApplyButtonClicked);
			// 
			// SelectionPictureBox
			// 
			this.SelectionPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SelectionPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.SelectionPictureBox.Location = new System.Drawing.Point(3, 75);
			this.SelectionPictureBox.Name = "SelectionPictureBox";
			this.SelectionPictureBox.Size = new System.Drawing.Size(122, 126);
			this.SelectionPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.SelectionPictureBox.TabIndex = 10;
			this.SelectionPictureBox.TabStop = false;
			// 
			// SizeLabel
			// 
			this.SizeLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.SizeLabel.AutoSize = true;
			this.SizeLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SizeLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SizeLabel.Location = new System.Drawing.Point(131, 76);
			this.SizeLabel.Name = "SizeLabel";
			this.SizeLabel.Size = new System.Drawing.Size(27, 15);
			this.SizeLabel.TabIndex = 13;
			this.SizeLabel.Text = Local.LocalString("sidebar.size");
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Location = new System.Drawing.Point(6, 64);
			this.label1.Margin = new System.Windows.Forms.Padding(6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(188, 2);
			this.label1.TabIndex = 14;
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// TextureComboBox
			// 
			this.TextureComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TextureComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.TextureComboBox.DropDownHeight = 600;
			this.TextureComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.TextureComboBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.TextureComboBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.TextureComboBox.FormattingEnabled = true;
			this.TextureComboBox.IntegralHeight = false;
			this.TextureComboBox.ItemHeight = 15;
			this.TextureComboBox.Location = new System.Drawing.Point(60, 34);
			this.TextureComboBox.Name = "TextureComboBox";
			this.TextureComboBox.Size = new System.Drawing.Size(137, 21);
			this.TextureComboBox.TabIndex = 9;
			this.TextureComboBox.SelectionChangeCommitted += new System.EventHandler(this.TextureSelectionChanged);
			// 
			// TextureSidebarPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.SizeLabel);
			this.Controls.Add(this.ApplyButton);
			this.Controls.Add(this.BrowseButton);
			this.Controls.Add(this.ReplaceButton);
			this.Controls.Add(this.SelectionPictureBox);
			this.Controls.Add(this.TextureComboBox);
			this.Controls.Add(this.GroupComboBox);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.DoubleBuffered = true;
			this.MaximumSize = new System.Drawing.Size(0, 250);
			this.MinimumSize = new System.Drawing.Size(200, 165);
			this.Name = "TextureSidebarPanel";
			this.Size = new System.Drawing.Size(200, 204);
			((System.ComponentModel.ISupportInitialize)(this.SelectionPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Button ReplaceButton;
        private System.Windows.Forms.PictureBox SelectionPictureBox;
        private TextureComboBox TextureComboBox;
        private System.Windows.Forms.ComboBox GroupComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Label SizeLabel;
		private System.Windows.Forms.Label label1;
	}
}
