using CBRE.Localization;

namespace CBRE.Editor.UI.Layout
{
    partial class LayoutSettings
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutSettings));
			this.TableLayout = new System.Windows.Forms.TableLayoutPanel();
			this.Rows = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.Columns = new System.Windows.Forms.NumericUpDown();
			this.label2 = new System.Windows.Forms.Label();
			this.ApplyButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.WindowDropDown = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.PresetButtonPanel = new System.Windows.Forms.FlowLayoutPanel();
			((System.ComponentModel.ISupportInitialize)(this.Rows)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.Columns)).BeginInit();
			this.SuspendLayout();
			// 
			// TableLayout
			// 
			this.TableLayout.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TableLayout.ColumnCount = 2;
			this.TableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TableLayout.Location = new System.Drawing.Point(12, 39);
			this.TableLayout.Name = "TableLayout";
			this.TableLayout.RowCount = 2;
			this.TableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.TableLayout.Size = new System.Drawing.Size(365, 251);
			this.TableLayout.TabIndex = 0;
			// 
			// Rows
			// 
			this.Rows.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Rows.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Rows.Location = new System.Drawing.Point(383, 57);
			this.Rows.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.Rows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.Rows.Name = "Rows";
			this.Rows.Size = new System.Drawing.Size(45, 23);
			this.Rows.TabIndex = 1;
			this.Rows.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.Rows.ValueChanged += new System.EventHandler(this.RowsValueChanged);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(383, 39);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 15);
			this.label1.TabIndex = 2;
			this.label1.Text = Local.LocalString("layout.rows")                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    ;
			// 
			// Columns
			// 
			this.Columns.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.Columns.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Columns.Location = new System.Drawing.Point(73, 296);
			this.Columns.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
			this.Columns.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.Columns.Name = "Columns";
			this.Columns.Size = new System.Drawing.Size(45, 23);
			this.Columns.TabIndex = 1;
			this.Columns.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
			this.Columns.ValueChanged += new System.EventHandler(this.ColumnsValueChanged);
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(12, 298);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(55, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = Local.LocalString("layout.columns");
			// 
			// ApplyButton
			// 
			this.ApplyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.ApplyButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ApplyButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ApplyButton.Location = new System.Drawing.Point(353, 296);
			this.ApplyButton.Name = "ApplyButton";
			this.ApplyButton.Size = new System.Drawing.Size(75, 23);
			this.ApplyButton.TabIndex = 3;
			this.ApplyButton.Text = Local.LocalString("apply");
			this.ApplyButton.UseVisualStyleBackColor = true;
			this.ApplyButton.Click += new System.EventHandler(this.ApplyButtonClick);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelButton.Location = new System.Drawing.Point(272, 296);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = Local.LocalString("cancel");
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
			// 
			// WindowDropDown
			// 
			this.WindowDropDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.WindowDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.WindowDropDown.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.WindowDropDown.FormattingEnabled = true;
			this.WindowDropDown.Location = new System.Drawing.Point(69, 6);
			this.WindowDropDown.Name = "WindowDropDown";
			this.WindowDropDown.Size = new System.Drawing.Size(308, 23);
			this.WindowDropDown.TabIndex = 4;
			this.WindowDropDown.SelectedIndexChanged += new System.EventHandler(this.WindowDropDownSelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(12, 9);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 15);
			this.label3.TabIndex = 2;
			this.label3.Text = Local.LocalString("layout.window_2");
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(9, 322);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(389, 45);
			this.label4.TabIndex = 2;
			this.label4.Text = Local.LocalString("layout.help");
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(12, 379);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(147, 15);
			this.label5.TabIndex = 2;
			this.label5.Text = Local.LocalString("layout.click_to_try");
			// 
			// PresetButtonPanel
			// 
			this.PresetButtonPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PresetButtonPanel.Location = new System.Drawing.Point(166, 375);
			this.PresetButtonPanel.Name = "PresetButtonPanel";
			this.PresetButtonPanel.Size = new System.Drawing.Size(262, 24);
			this.PresetButtonPanel.TabIndex = 5;
			this.PresetButtonPanel.WrapContents = false;
			// 
			// LayoutSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(440, 411);
			this.Controls.Add(this.PresetButtonPanel);
			this.Controls.Add(this.WindowDropDown);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.ApplyButton);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.Columns);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.Rows);
			this.Controls.Add(this.TableLayout);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(380, 380);
			this.Name = "LayoutSettings";
			this.Text = Local.LocalString("layout");
			((System.ComponentModel.ISupportInitialize)(this.Rows)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.Columns)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel TableLayout;
        private System.Windows.Forms.NumericUpDown Rows;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown Columns;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ComboBox WindowDropDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FlowLayoutPanel PresetButtonPanel;
    }
}