using CBRE.Localization;

namespace CBRE.Editor.Visgroups
{
    partial class VisgroupEditForm
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
			this.GroupName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.ColourPanel = new System.Windows.Forms.Panel();
			this.cancelButton = new System.Windows.Forms.Button();
			this.AddButton = new System.Windows.Forms.Button();
			this.RemoveButton = new System.Windows.Forms.Button();
			this.OkButton = new System.Windows.Forms.Button();
			this.VisgroupPanel = new CBRE.Editor.Visgroups.VisgroupPanel();
			this.SuspendLayout();
			// 
			// GroupName
			// 
			this.GroupName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.GroupName.Enabled = false;
			this.GroupName.Location = new System.Drawing.Point(296, 15);
			this.GroupName.Name = "GroupName";
			this.GroupName.Size = new System.Drawing.Size(124, 20);
			this.GroupName.TabIndex = 1;
			this.GroupName.TextChanged += new System.EventHandler(this.GroupNameChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(251, 17);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 15);
			this.label1.TabIndex = 2;
			this.label1.Text = Local.LocalString("entity_report.name");
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(251, 45);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(36, 15);
			this.label2.TabIndex = 3;
			this.label2.Text = Local.LocalString("data.light.color");
			// 
			// ColourPanel
			// 
			this.ColourPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.ColourPanel.Enabled = false;
			this.ColourPanel.Location = new System.Drawing.Point(296, 42);
			this.ColourPanel.Name = "ColourPanel";
			this.ColourPanel.Size = new System.Drawing.Size(43, 22);
			this.ColourPanel.TabIndex = 4;
			this.ColourPanel.Click += new System.EventHandler(this.ColourClicked);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelButton.Location = new System.Drawing.Point(345, 312);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = Local.LocalString("cancel");
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.CloseButtonClicked);
			// 
			// AddButton
			// 
			this.AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.AddButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.AddButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.AddButton.Location = new System.Drawing.Point(251, 109);
			this.AddButton.Name = "AddButton";
			this.AddButton.Size = new System.Drawing.Size(95, 23);
			this.AddButton.TabIndex = 5;
			this.AddButton.Text = Local.LocalString("visgroup_edit.add_group");
			this.AddButton.UseVisualStyleBackColor = true;
			this.AddButton.Click += new System.EventHandler(this.AddGroup);
			// 
			// RemoveButton
			// 
			this.RemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.RemoveButton.Enabled = false;
			this.RemoveButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.RemoveButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.RemoveButton.Location = new System.Drawing.Point(251, 138);
			this.RemoveButton.Name = "RemoveButton";
			this.RemoveButton.Size = new System.Drawing.Size(95, 23);
			this.RemoveButton.TabIndex = 5;
			this.RemoveButton.Text = Local.LocalString("visgroup_edit.remove_group");
			this.RemoveButton.UseVisualStyleBackColor = true;
			this.RemoveButton.Click += new System.EventHandler(this.RemoveGroup);
			// 
			// OkButton
			// 
			this.OkButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.OkButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OkButton.Location = new System.Drawing.Point(264, 312);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(75, 23);
			this.OkButton.TabIndex = 5;
			this.OkButton.Text = Local.LocalString("ok");
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += new System.EventHandler(this.CloseButtonClicked);
			// 
			// VisgroupPanel
			// 
			this.VisgroupPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.VisgroupPanel.DisableAutomatic = false;
			this.VisgroupPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.VisgroupPanel.HideAutomatic = true;
			this.VisgroupPanel.Location = new System.Drawing.Point(12, 12);
			this.VisgroupPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.VisgroupPanel.Name = "VisgroupPanel";
			this.VisgroupPanel.ShowCheckboxes = false;
			this.VisgroupPanel.ShowHidden = false;
			this.VisgroupPanel.Size = new System.Drawing.Size(233, 323);
			this.VisgroupPanel.SortAutomaticFirst = false;
			this.VisgroupPanel.TabIndex = 6;
			// 
			// VisgroupEditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(432, 347);
			this.Controls.Add(this.VisgroupPanel);
			this.Controls.Add(this.RemoveButton);
			this.Controls.Add(this.AddButton);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.ColourPanel);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.GroupName);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(448, 386);
			this.Name = "VisgroupEditForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = Local.LocalString("visgroup_edit");
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

#pragma warning disable 0649
        private System.Windows.Forms.TextBox GroupName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel ColourPanel;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button OkButton;
		private VisgroupPanel VisgroupPanel;
#pragma warning restore 0649
	}
}