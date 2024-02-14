using CBRE.Localization;

namespace CBRE.Editor.Tools.VMTool
{
    partial class StandardControl
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
			this.MergeButton = new System.Windows.Forms.Button();
			this.AutoMerge = new System.Windows.Forms.CheckBox();
			this.SplitButton = new System.Windows.Forms.Button();
			this.MergeResultsLabel = new CBRE.Editor.Tools.VMTool.FadeLabel();
			this.SuspendLayout();
			// 
			// MergeButton
			// 
			this.MergeButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.MergeButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MergeButton.Location = new System.Drawing.Point(3, 3);
			this.MergeButton.Name = "MergeButton";
			this.MergeButton.Size = new System.Drawing.Size(160, 23);
			this.MergeButton.TabIndex = 8;
			this.MergeButton.Text = Local.LocalString("tool.merge_overlap_vertices");
			this.MergeButton.UseVisualStyleBackColor = true;
			this.MergeButton.Click += new System.EventHandler(this.MergeButtonClicked);
			// 
			// AutoMerge
			// 
			this.AutoMerge.AutoSize = true;
			this.AutoMerge.Checked = true;
			this.AutoMerge.CheckState = System.Windows.Forms.CheckState.Checked;
			this.AutoMerge.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.AutoMerge.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AutoMerge.Location = new System.Drawing.Point(3, 32);
			this.AutoMerge.Name = "AutoMerge";
			this.AutoMerge.Size = new System.Drawing.Size(141, 20);
			this.AutoMerge.TabIndex = 7;
			this.AutoMerge.Text = Local.LocalString("tool.merge_auto");
			this.AutoMerge.UseVisualStyleBackColor = true;
			// 
			// SplitButton
			// 
			this.SplitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.SplitButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SplitButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SplitButton.Location = new System.Drawing.Point(3, 76);
			this.SplitButton.Name = "SplitButton";
			this.SplitButton.Size = new System.Drawing.Size(76, 25);
			this.SplitButton.TabIndex = 8;
			this.SplitButton.Text = Local.LocalString("tool.split_face");
			this.SplitButton.UseVisualStyleBackColor = true;
			this.SplitButton.Click += new System.EventHandler(this.SplitButtonClicked);
			// 
			// MergeResultsLabel
			// 
			this.MergeResultsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.MergeResultsLabel.FadeTime = 2000;
			this.MergeResultsLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.MergeResultsLabel.Location = new System.Drawing.Point(3, 55);
			this.MergeResultsLabel.Name = "MergeResultsLabel";
			this.MergeResultsLabel.Size = new System.Drawing.Size(194, 18);
			this.MergeResultsLabel.TabIndex = 9;
			this.MergeResultsLabel.Text = Local.LocalString("tool.merge_result");
			this.MergeResultsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// StandardControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.MergeResultsLabel);
			this.Controls.Add(this.SplitButton);
			this.Controls.Add(this.MergeButton);
			this.Controls.Add(this.AutoMerge);
			this.Name = "StandardControl";
			this.Size = new System.Drawing.Size(200, 104);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button MergeButton;
        private System.Windows.Forms.CheckBox AutoMerge;
        private FadeLabel MergeResultsLabel;
		private System.Windows.Forms.Button SplitButton;
	}
}
