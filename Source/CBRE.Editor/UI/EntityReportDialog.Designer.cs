using CBRE.Localization;

namespace CBRE.Editor.UI
{
    partial class EntityReportDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EntityReportDialog));
			this.EntityList = new System.Windows.Forms.ListView();
			this.ClassNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.EntityNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ResetFiltersButton = new System.Windows.Forms.Button();
			this.IncludeHidden = new System.Windows.Forms.CheckBox();
			this.FilterClassExact = new System.Windows.Forms.CheckBox();
			this.FilterKeyValueExact = new System.Windows.Forms.CheckBox();
			this.label2 = new System.Windows.Forms.Label();
			this.FilterValue = new System.Windows.Forms.TextBox();
			this.FilterClass = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.FilterKey = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.TypeBrush = new System.Windows.Forms.RadioButton();
			this.TypePoint = new System.Windows.Forms.RadioButton();
			this.TypeAll = new System.Windows.Forms.RadioButton();
			this.GoToButton = new System.Windows.Forms.Button();
			this.DeleteButton = new System.Windows.Forms.Button();
			this.PropertiesButton = new System.Windows.Forms.Button();
			this.CloseButton = new System.Windows.Forms.Button();
			this.FollowSelection = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// EntityList
			// 
			this.EntityList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.EntityList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ClassNameHeader,
            this.EntityNameHeader});
			this.EntityList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.EntityList.FullRowSelect = true;
			this.EntityList.HideSelection = false;
			this.EntityList.Location = new System.Drawing.Point(12, 12);
			this.EntityList.MultiSelect = false;
			this.EntityList.Name = "EntityList";
			this.EntityList.Size = new System.Drawing.Size(308, 242);
			this.EntityList.TabIndex = 0;
			this.EntityList.UseCompatibleStateImageBehavior = false;
			this.EntityList.View = System.Windows.Forms.View.Details;
			this.EntityList.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.SortByColumn);
			// 
			// ClassNameHeader
			// 
			this.ClassNameHeader.Text = Local.LocalString("object_properties.class");
			this.ClassNameHeader.Width = 107;
			// 
			// EntityNameHeader
			// 
			this.EntityNameHeader.Text = Local.LocalString("entity_report.name");
			this.EntityNameHeader.Width = 153;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.ResetFiltersButton);
			this.groupBox1.Controls.Add(this.IncludeHidden);
			this.groupBox1.Controls.Add(this.FilterClassExact);
			this.groupBox1.Controls.Add(this.FilterKeyValueExact);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.FilterValue);
			this.groupBox1.Controls.Add(this.FilterClass);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.FilterKey);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.TypeBrush);
			this.groupBox1.Controls.Add(this.TypePoint);
			this.groupBox1.Controls.Add(this.TypeAll);
			this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(326, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(178, 242);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = Local.LocalString("entity_report.filter");
			// 
			// ResetFiltersButton
			// 
			this.ResetFiltersButton.Location = new System.Drawing.Point(49, 204);
			this.ResetFiltersButton.Name = "ResetFiltersButton";
			this.ResetFiltersButton.Size = new System.Drawing.Size(75, 23);
			this.ResetFiltersButton.TabIndex = 6;
			this.ResetFiltersButton.Text = Local.LocalString("reset");
			this.ResetFiltersButton.UseVisualStyleBackColor = true;
			this.ResetFiltersButton.Click += new System.EventHandler(this.ResetFilters);
			// 
			// IncludeHidden
			// 
			this.IncludeHidden.AutoSize = true;
			this.IncludeHidden.Checked = true;
			this.IncludeHidden.CheckState = System.Windows.Forms.CheckState.Checked;
			this.IncludeHidden.Location = new System.Drawing.Point(11, 88);
			this.IncludeHidden.Name = "IncludeHidden";
			this.IncludeHidden.Size = new System.Drawing.Size(146, 19);
			this.IncludeHidden.TabIndex = 5;
			this.IncludeHidden.Text = Local.LocalString("entity_report.include_hidden_objects");
			this.IncludeHidden.UseVisualStyleBackColor = true;
			this.IncludeHidden.CheckedChanged += new System.EventHandler(this.FiltersChanged);
			// 
			// FilterClassExact
			// 
			this.FilterClassExact.AutoSize = true;
			this.FilterClassExact.Location = new System.Drawing.Point(119, 158);
			this.FilterClassExact.Name = "FilterClassExact";
			this.FilterClassExact.Size = new System.Drawing.Size(54, 19);
			this.FilterClassExact.TabIndex = 4;
			this.FilterClassExact.Text = Local.LocalString("entity_report.exact");
			this.FilterClassExact.UseVisualStyleBackColor = true;
			this.FilterClassExact.CheckedChanged += new System.EventHandler(this.FiltersChanged);
			// 
			// FilterKeyValueExact
			// 
			this.FilterKeyValueExact.AutoSize = true;
			this.FilterKeyValueExact.Location = new System.Drawing.Point(119, 112);
			this.FilterKeyValueExact.Name = "FilterKeyValueExact";
			this.FilterKeyValueExact.Size = new System.Drawing.Size(54, 19);
			this.FilterKeyValueExact.TabIndex = 4;
			this.FilterKeyValueExact.Text = Local.LocalString("entity_report.exact");
			this.FilterKeyValueExact.UseVisualStyleBackColor = true;
			this.FilterKeyValueExact.CheckedChanged += new System.EventHandler(this.FiltersChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(83, 135);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(15, 15);
			this.label2.TabIndex = 3;
			this.label2.Text = "=";
			// 
			// FilterValue
			// 
			this.FilterValue.Location = new System.Drawing.Point(102, 132);
			this.FilterValue.Name = "FilterValue";
			this.FilterValue.Size = new System.Drawing.Size(66, 23);
			this.FilterValue.TabIndex = 2;
			this.FilterValue.TextChanged += new System.EventHandler(this.FiltersChanged);
			// 
			// FilterClass
			// 
			this.FilterClass.Location = new System.Drawing.Point(11, 178);
			this.FilterClass.Name = "FilterClass";
			this.FilterClass.Size = new System.Drawing.Size(157, 23);
			this.FilterClass.TabIndex = 2;
			this.FilterClass.TextChanged += new System.EventHandler(this.FiltersChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 159);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 15);
			this.label3.TabIndex = 1;
			this.label3.Text = Local.LocalString("entity_report.filter_by_class");
			// 
			// FilterKey
			// 
			this.FilterKey.Location = new System.Drawing.Point(11, 132);
			this.FilterKey.Name = "FilterKey";
			this.FilterKey.Size = new System.Drawing.Size(66, 23);
			this.FilterKey.TabIndex = 2;
			this.FilterKey.TextChanged += new System.EventHandler(this.FiltersChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 113);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(106, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = Local.LocalString("entity_report.filter_by_keyvalue");
			// 
			// TypeBrush
			// 
			this.TypeBrush.AutoSize = true;
			this.TypeBrush.Location = new System.Drawing.Point(11, 65);
			this.TypeBrush.Name = "TypeBrush";
			this.TypeBrush.Size = new System.Drawing.Size(124, 19);
			this.TypeBrush.TabIndex = 0;
			this.TypeBrush.Text = Local.LocalString("entity_report.brush_entities_only");
			this.TypeBrush.UseVisualStyleBackColor = true;
			this.TypeBrush.Click += new System.EventHandler(this.FiltersChanged);
			// 
			// TypePoint
			// 
			this.TypePoint.AutoSize = true;
			this.TypePoint.Location = new System.Drawing.Point(11, 42);
			this.TypePoint.Name = "TypePoint";
			this.TypePoint.Size = new System.Drawing.Size(122, 19);
			this.TypePoint.TabIndex = 0;
			this.TypePoint.Text = Local.LocalString("entity_report.point_entities_only");
			this.TypePoint.UseVisualStyleBackColor = true;
			this.TypePoint.Click += new System.EventHandler(this.FiltersChanged);
			// 
			// TypeAll
			// 
			this.TypeAll.AutoSize = true;
			this.TypeAll.Checked = true;
			this.TypeAll.Location = new System.Drawing.Point(11, 19);
			this.TypeAll.Name = "TypeAll";
			this.TypeAll.Size = new System.Drawing.Size(71, 19);
			this.TypeAll.TabIndex = 0;
			this.TypeAll.TabStop = true;
			this.TypeAll.Text = Local.LocalString("entity_report.show_all");
			this.TypeAll.UseVisualStyleBackColor = true;
			this.TypeAll.Click += new System.EventHandler(this.FiltersChanged);
			// 
			// GoToButton
			// 
			this.GoToButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.GoToButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.GoToButton.Location = new System.Drawing.Point(12, 260);
			this.GoToButton.Name = "GoToButton";
			this.GoToButton.Size = new System.Drawing.Size(75, 23);
			this.GoToButton.TabIndex = 2;
			this.GoToButton.Text = Local.LocalString("entity_report.goto");
			this.GoToButton.UseVisualStyleBackColor = true;
			this.GoToButton.Click += new System.EventHandler(this.GoToSelectedEntity);
			// 
			// DeleteButton
			// 
			this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.DeleteButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DeleteButton.Location = new System.Drawing.Point(93, 260);
			this.DeleteButton.Name = "DeleteButton";
			this.DeleteButton.Size = new System.Drawing.Size(75, 23);
			this.DeleteButton.TabIndex = 3;
			this.DeleteButton.Text = Local.LocalString("delete");
			this.DeleteButton.UseVisualStyleBackColor = true;
			this.DeleteButton.Click += new System.EventHandler(this.DeleteSelectedEntity);
			// 
			// PropertiesButton
			// 
			this.PropertiesButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.PropertiesButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PropertiesButton.Location = new System.Drawing.Point(174, 260);
			this.PropertiesButton.Name = "PropertiesButton";
			this.PropertiesButton.Size = new System.Drawing.Size(75, 23);
			this.PropertiesButton.TabIndex = 4;
			this.PropertiesButton.Text = Local.LocalString("entity_report.properties");
			this.PropertiesButton.UseVisualStyleBackColor = true;
			this.PropertiesButton.Click += new System.EventHandler(this.OpenEntityProperties);
			// 
			// CloseButton
			// 
			this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.CloseButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CloseButton.Location = new System.Drawing.Point(429, 260);
			this.CloseButton.Name = "CloseButton";
			this.CloseButton.Size = new System.Drawing.Size(75, 23);
			this.CloseButton.TabIndex = 5;
			this.CloseButton.Text = Local.LocalString("close");
			this.CloseButton.UseVisualStyleBackColor = true;
			this.CloseButton.Click += new System.EventHandler(this.CloseButtonClicked);
			// 
			// FollowSelection
			// 
			this.FollowSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.FollowSelection.AutoSize = true;
			this.FollowSelection.Checked = true;
			this.FollowSelection.CheckState = System.Windows.Forms.CheckState.Checked;
			this.FollowSelection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FollowSelection.Location = new System.Drawing.Point(255, 262);
			this.FollowSelection.Name = "FollowSelection";
			this.FollowSelection.Size = new System.Drawing.Size(111, 19);
			this.FollowSelection.TabIndex = 6;
			this.FollowSelection.Text = Local.LocalString("entity_report.follow_selection");
			this.FollowSelection.UseVisualStyleBackColor = true;
			// 
			// EntityReportDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(514, 292);
			this.Controls.Add(this.FollowSelection);
			this.Controls.Add(this.CloseButton);
			this.Controls.Add(this.PropertiesButton);
			this.Controls.Add(this.DeleteButton);
			this.Controls.Add(this.GoToButton);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.EntityList);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(530, 330);
			this.Name = "EntityReportDialog";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = Local.LocalString("entity_report");
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView EntityList;
        private System.Windows.Forms.ColumnHeader ClassNameHeader;
        private System.Windows.Forms.ColumnHeader EntityNameHeader;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton TypeBrush;
        private System.Windows.Forms.RadioButton TypePoint;
        private System.Windows.Forms.RadioButton TypeAll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox FilterValue;
        private System.Windows.Forms.TextBox FilterKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FilterClass;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox FilterClassExact;
        private System.Windows.Forms.CheckBox FilterKeyValueExact;
        private System.Windows.Forms.CheckBox IncludeHidden;
        private System.Windows.Forms.Button ResetFiltersButton;
        private System.Windows.Forms.Button GoToButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Button PropertiesButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.CheckBox FollowSelection;
    }
}