using CBRE.Localization;

namespace CBRE.Editor.UI.Sidebar
{
    partial class VisgroupSidebarPanel
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
			this.ShowAllButton = new System.Windows.Forms.Button();
			this.SelectButton = new System.Windows.Forms.Button();
			this.EditButton = new System.Windows.Forms.Button();
			this.NewButton = new System.Windows.Forms.Button();
			this.VisgroupPanel = new CBRE.Editor.Visgroups.VisgroupPanel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ShowAllButton
			// 
			this.ShowAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ShowAllButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ShowAllButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ShowAllButton.Location = new System.Drawing.Point(96, 3);
			this.ShowAllButton.Name = "ShowAllButton";
			this.ShowAllButton.Size = new System.Drawing.Size(52, 23);
			this.ShowAllButton.TabIndex = 5;
			this.ShowAllButton.Tag = "ShowAll";
			this.ShowAllButton.Text = Local.LocalString("entity_report.show_all");
			this.ShowAllButton.UseVisualStyleBackColor = true;
			this.ShowAllButton.Click += new System.EventHandler(this.ShowAllButtonClicked);
			// 
			// SelectButton
			// 
			this.SelectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.SelectButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SelectButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SelectButton.Location = new System.Drawing.Point(51, 3);
			this.SelectButton.Name = "SelectButton";
			this.SelectButton.Size = new System.Drawing.Size(39, 23);
			this.SelectButton.TabIndex = 6;
			this.SelectButton.Tag = "Select";
			this.SelectButton.Text = Local.LocalString("tool.texture.select");
			this.SelectButton.UseVisualStyleBackColor = true;
			this.SelectButton.Click += new System.EventHandler(this.SelectButtonClicked);
			// 
			// EditButton
			// 
			this.EditButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.EditButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.EditButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.EditButton.Location = new System.Drawing.Point(3, 3);
			this.EditButton.Name = "EditButton";
			this.EditButton.Size = new System.Drawing.Size(42, 23);
			this.EditButton.TabIndex = 7;
			this.EditButton.Tag = "Edit";
			this.EditButton.Text = Local.LocalString("edit");
			this.EditButton.UseVisualStyleBackColor = true;
			this.EditButton.Click += new System.EventHandler(this.EditButtonClicked);
			// 
			// NewButton
			// 
			this.NewButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.NewButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.NewButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.NewButton.Location = new System.Drawing.Point(154, 3);
			this.NewButton.Name = "NewButton";
			this.NewButton.Size = new System.Drawing.Size(37, 23);
			this.NewButton.TabIndex = 9;
			this.NewButton.Tag = "New";
			this.NewButton.Text = Local.LocalString("new");
			this.NewButton.UseVisualStyleBackColor = true;
			this.NewButton.Click += new System.EventHandler(this.NewButtonClicked);
			// 
			// VisgroupPanel
			// 
			this.VisgroupPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.VisgroupPanel.DisableAutomatic = false;
			this.VisgroupPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.VisgroupPanel.HideAutomatic = false;
			this.VisgroupPanel.Location = new System.Drawing.Point(3, 3);
			this.VisgroupPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
			this.VisgroupPanel.Name = "VisgroupPanel";
			this.VisgroupPanel.ShowCheckboxes = true;
			this.VisgroupPanel.ShowHidden = false;
			this.VisgroupPanel.Size = new System.Drawing.Size(194, 166);
			this.VisgroupPanel.SortAutomaticFirst = true;
			this.VisgroupPanel.TabIndex = 8;
			this.VisgroupPanel.VisgroupToggled += new CBRE.Editor.Visgroups.VisgroupPanel.VisgroupToggledEventHandler(this.VisgroupToggled);
			this.VisgroupPanel.VisgroupSelected += new CBRE.Editor.Visgroups.VisgroupPanel.VisgroupSelectedEventHandler(this.VisgroupSelected);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 4;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.5F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.5F));
			this.tableLayoutPanel1.Controls.Add(this.EditButton, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.SelectButton, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.ShowAllButton, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.NewButton, 3, 0);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 175);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(194, 29);
			this.tableLayoutPanel1.TabIndex = 10;
			// 
			// VisgroupSidebarPanel
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.VisgroupPanel);
			this.MaximumSize = new System.Drawing.Size(0, 250);
			this.MinimumSize = new System.Drawing.Size(200, 165);
			this.Name = "VisgroupSidebarPanel";
			this.Size = new System.Drawing.Size(200, 207);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ShowAllButton;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Button EditButton;
        private CBRE.Editor.Visgroups.VisgroupPanel VisgroupPanel;
        private System.Windows.Forms.Button NewButton;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
	}
}
