using CBRE.Localization;

namespace CBRE.Editor.UI.Sidebar
{
    partial class EntitySidebarPanel
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
			this.label2 = new System.Windows.Forms.Label();
			this.EntityTypeList = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(3, 3);
			this.label2.Margin = new System.Windows.Forms.Padding(3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(64, 15);
			this.label2.TabIndex = 9;
			this.label2.Text = Local.LocalString("entity.type");
			this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			// 
			// EntityTypeList
			// 
			this.EntityTypeList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.EntityTypeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.EntityTypeList.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.EntityTypeList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.EntityTypeList.FormattingEnabled = true;
			this.EntityTypeList.Location = new System.Drawing.Point(3, 24);
			this.EntityTypeList.Name = "EntityTypeList";
			this.EntityTypeList.Size = new System.Drawing.Size(194, 23);
			this.EntityTypeList.TabIndex = 5;
			// 
			// EntitySidebarPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.EntityTypeList);
			this.MinimumSize = new System.Drawing.Size(200, 50);
			this.Name = "EntitySidebarPanel";
			this.Size = new System.Drawing.Size(200, 50);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox EntityTypeList;
    }
}
