﻿using CBRE.Editor.UI;
using System.Windows.Forms;

namespace CBRE.Editor.Tools.TextureTool
{
    partial class TextureApplicationForm : HotkeyForm
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
	        this.components = new System.ComponentModel.Container();
	        this.label11 = new System.Windows.Forms.Label();
	        this.label10 = new System.Windows.Forms.Label();
	        this.HideMaskCheckbox = new System.Windows.Forms.CheckBox();
	        this.RecentFilterTextbox = new System.Windows.Forms.TextBox();
	        this.label9 = new System.Windows.Forms.Label();
	        this.groupBox2 = new System.Windows.Forms.GroupBox();
	        this.AlignToFaceCheckbox = new System.Windows.Forms.CheckBox();
	        this.AlignToWorldCheckbox = new System.Windows.Forms.CheckBox();
	        this.groupBox1 = new System.Windows.Forms.GroupBox();
	        this.JustifyTopButton = new System.Windows.Forms.Button();
	        this.JustifyFitButton = new System.Windows.Forms.Button();
	        this.TreatAsOneCheckbox = new System.Windows.Forms.CheckBox();
	        this.JustifyRightButton = new System.Windows.Forms.Button();
	        this.JustifyBottomButton = new System.Windows.Forms.Button();
	        this.JustifyCenterButton = new System.Windows.Forms.Button();
	        this.JustifyLeftButton = new System.Windows.Forms.Button();
	        this.RightClickCombo = new System.Windows.Forms.ComboBox();
	        this.label7 = new System.Windows.Forms.Label();
	        this.LeftClickCombo = new System.Windows.Forms.ComboBox();
	        this.label6 = new System.Windows.Forms.Label();
	        this.ApplyButton = new System.Windows.Forms.Button();
	        this.RotationValue = new System.Windows.Forms.NumericUpDown();
	        this.ReplaceButton = new System.Windows.Forms.Button();
	        this.BrowseButton = new System.Windows.Forms.Button();
	        this.label5 = new System.Windows.Forms.Label();
	        this.TextureDetailsLabel = new System.Windows.Forms.Label();
	        this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
	        this.label1 = new System.Windows.Forms.Label();
	        this.label2 = new System.Windows.Forms.Label();
	        this.label3 = new System.Windows.Forms.Label();
	        this.label4 = new System.Windows.Forms.Label();
	        this.ScaleYValue = new System.Windows.Forms.NumericUpDown();
	        this.ShiftXValue = new System.Windows.Forms.NumericUpDown();
	        this.ShiftYValue = new System.Windows.Forms.NumericUpDown();
	        this.ScaleXValue = new System.Windows.Forms.NumericUpDown();
	        this.HoverTip = new System.Windows.Forms.ToolTip(this.components);
	        this.RecentTexturesList = new CBRE.Editor.UI.TextureListPanel();
	        this.SelectedTexturesList = new CBRE.Editor.UI.TextureListPanel();
	        this.groupBox2.SuspendLayout();
	        this.groupBox1.SuspendLayout();
	        ((System.ComponentModel.ISupportInitialize)(this.RotationValue)).BeginInit();
	        this.tableLayoutPanel1.SuspendLayout();
	        ((System.ComponentModel.ISupportInitialize)(this.ScaleYValue)).BeginInit();
	        ((System.ComponentModel.ISupportInitialize)(this.ShiftXValue)).BeginInit();
	        ((System.ComponentModel.ISupportInitialize)(this.ShiftYValue)).BeginInit();
	        ((System.ComponentModel.ISupportInitialize)(this.ScaleXValue)).BeginInit();
	        this.SuspendLayout();
	        // 
	        // label11
	        // 
	        this.label11.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.label11.Location = new System.Drawing.Point(253, 390);
	        this.label11.Name = "label11";
	        this.label11.Size = new System.Drawing.Size(20, 21);
	        this.label11.TabIndex = 35;
	        this.label11.Text = "?";
	        this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
	        this.HoverTip.SetToolTip(this.label11, "Right Click Shortcuts:\r\nShift: Apply texture to all faces on a brush\r\nAlt: Apply " + "texture with alignment to last selected face\r\nShift+Alt: Apply with alignment to" + " all faces on a brush");
	        // 
	        // label10
	        // 
	        this.label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.label10.Location = new System.Drawing.Point(253, 363);
	        this.label10.Name = "label10";
	        this.label10.Size = new System.Drawing.Size(20, 21);
	        this.label10.TabIndex = 36;
	        this.label10.Text = "?";
	        this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
	        this.HoverTip.SetToolTip(this.label10, "Left Click Shortcuts:\r\nShift: Select all faces on a brush\r\nCtrl: Select multiple " + "faces\r\nShift+Ctrl: Invert selected faces on brush");
	        // 
	        // HideMaskCheckbox
	        // 
	        this.HideMaskCheckbox.Appearance = System.Windows.Forms.Appearance.Button;
	        this.HideMaskCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.HideMaskCheckbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.HideMaskCheckbox.Location = new System.Drawing.Point(303, 152);
	        this.HideMaskCheckbox.Name = "HideMaskCheckbox";
	        this.HideMaskCheckbox.Size = new System.Drawing.Size(102, 23);
	        this.HideMaskCheckbox.TabIndex = 17;
	        this.HideMaskCheckbox.Text = "Hide Mask";
	        this.HideMaskCheckbox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
	        this.HideMaskCheckbox.UseVisualStyleBackColor = true;
	        this.HideMaskCheckbox.CheckedChanged += new System.EventHandler(this.HideMaskCheckboxToggled);
	        // 
	        // RecentFilterTextbox
	        // 
	        this.RecentFilterTextbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.RecentFilterTextbox.Location = new System.Drawing.Point(305, 385);
	        this.RecentFilterTextbox.Name = "RecentFilterTextbox";
	        this.RecentFilterTextbox.Size = new System.Drawing.Size(100, 23);
	        this.RecentFilterTextbox.TabIndex = 20;
	        this.RecentFilterTextbox.TextChanged += new System.EventHandler(this.RecentFilterTextChanged);
	        // 
	        // label9
	        // 
	        this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.label9.Location = new System.Drawing.Point(303, 360);
	        this.label9.Name = "label9";
	        this.label9.Size = new System.Drawing.Size(102, 22);
	        this.label9.TabIndex = 32;
	        this.label9.Text = "Filter Recent:";
	        this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
	        // 
	        // groupBox2
	        // 
	        this.groupBox2.Controls.Add(this.AlignToFaceCheckbox);
	        this.groupBox2.Controls.Add(this.AlignToWorldCheckbox);
	        this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.groupBox2.Location = new System.Drawing.Point(174, 97);
	        this.groupBox2.Name = "groupBox2";
	        this.groupBox2.Size = new System.Drawing.Size(122, 52);
	        this.groupBox2.TabIndex = 30;
	        this.groupBox2.TabStop = false;
	        this.groupBox2.Text = "Align";
	        // 
	        // AlignToFaceCheckbox
	        // 
	        this.AlignToFaceCheckbox.AutoSize = true;
	        this.AlignToFaceCheckbox.Location = new System.Drawing.Point(66, 22);
	        this.AlignToFaceCheckbox.Name = "AlignToFaceCheckbox";
	        this.AlignToFaceCheckbox.Size = new System.Drawing.Size(50, 19);
	        this.AlignToFaceCheckbox.TabIndex = 9;
	        this.AlignToFaceCheckbox.Text = "Face";
	        this.AlignToFaceCheckbox.UseVisualStyleBackColor = true;
	        this.AlignToFaceCheckbox.Click += new System.EventHandler(this.AlignToFaceClicked);
	        // 
	        // AlignToWorldCheckbox
	        // 
	        this.AlignToWorldCheckbox.AutoSize = true;
	        this.AlignToWorldCheckbox.Location = new System.Drawing.Point(6, 22);
	        this.AlignToWorldCheckbox.Name = "AlignToWorldCheckbox";
	        this.AlignToWorldCheckbox.Size = new System.Drawing.Size(58, 19);
	        this.AlignToWorldCheckbox.TabIndex = 8;
	        this.AlignToWorldCheckbox.Text = "World";
	        this.AlignToWorldCheckbox.UseVisualStyleBackColor = true;
	        this.AlignToWorldCheckbox.Click += new System.EventHandler(this.AlignToWorldClicked);
	        // 
	        // groupBox1
	        // 
	        this.groupBox1.Controls.Add(this.JustifyTopButton);
	        this.groupBox1.Controls.Add(this.JustifyFitButton);
	        this.groupBox1.Controls.Add(this.TreatAsOneCheckbox);
	        this.groupBox1.Controls.Add(this.JustifyRightButton);
	        this.groupBox1.Controls.Add(this.JustifyBottomButton);
	        this.groupBox1.Controls.Add(this.JustifyCenterButton);
	        this.groupBox1.Controls.Add(this.JustifyLeftButton);
	        this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.groupBox1.Location = new System.Drawing.Point(303, 12);
	        this.groupBox1.Name = "groupBox1";
	        this.groupBox1.Size = new System.Drawing.Size(102, 137);
	        this.groupBox1.TabIndex = 29;
	        this.groupBox1.TabStop = false;
	        this.groupBox1.Text = "Justify";
	        // 
	        // JustifyTopButton
	        // 
	        this.JustifyTopButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.JustifyTopButton.Location = new System.Drawing.Point(40, 15);
	        this.JustifyTopButton.Name = "JustifyTopButton";
	        this.JustifyTopButton.Size = new System.Drawing.Size(20, 20);
	        this.JustifyTopButton.TabIndex = 10;
	        this.JustifyTopButton.Text = "T";
	        this.JustifyTopButton.UseVisualStyleBackColor = true;
	        this.JustifyTopButton.Click += new System.EventHandler(this.JustifyTopClicked);
	        // 
	        // JustifyFitButton
	        // 
	        this.JustifyFitButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.JustifyFitButton.Location = new System.Drawing.Point(16, 87);
	        this.JustifyFitButton.Name = "JustifyFitButton";
	        this.JustifyFitButton.Size = new System.Drawing.Size(68, 20);
	        this.JustifyFitButton.TabIndex = 15;
	        this.JustifyFitButton.Text = "Fit";
	        this.JustifyFitButton.UseVisualStyleBackColor = true;
	        this.JustifyFitButton.Click += new System.EventHandler(this.JustifyFitClicked);
	        // 
	        // TreatAsOneCheckbox
	        // 
	        this.TreatAsOneCheckbox.Location = new System.Drawing.Point(6, 113);
	        this.TreatAsOneCheckbox.Name = "TreatAsOneCheckbox";
	        this.TreatAsOneCheckbox.Size = new System.Drawing.Size(90, 21);
	        this.TreatAsOneCheckbox.TabIndex = 16;
	        this.TreatAsOneCheckbox.Text = "Treat as One";
	        this.TreatAsOneCheckbox.UseVisualStyleBackColor = true;
	        this.TreatAsOneCheckbox.CheckedChanged += new System.EventHandler(this.TreatAsOneCheckboxToggled);
	        // 
	        // JustifyRightButton
	        // 
	        this.JustifyRightButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.JustifyRightButton.Location = new System.Drawing.Point(64, 39);
	        this.JustifyRightButton.Name = "JustifyRightButton";
	        this.JustifyRightButton.Size = new System.Drawing.Size(20, 20);
	        this.JustifyRightButton.TabIndex = 13;
	        this.JustifyRightButton.Text = "R";
	        this.JustifyRightButton.UseVisualStyleBackColor = true;
	        this.JustifyRightButton.Click += new System.EventHandler(this.JustifyRightClicked);
	        // 
	        // JustifyBottomButton
	        // 
	        this.JustifyBottomButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.JustifyBottomButton.Location = new System.Drawing.Point(40, 63);
	        this.JustifyBottomButton.Name = "JustifyBottomButton";
	        this.JustifyBottomButton.Size = new System.Drawing.Size(20, 20);
	        this.JustifyBottomButton.TabIndex = 14;
	        this.JustifyBottomButton.Text = "B";
	        this.JustifyBottomButton.UseVisualStyleBackColor = true;
	        this.JustifyBottomButton.Click += new System.EventHandler(this.JustifyBottomClicked);
	        // 
	        // JustifyCenterButton
	        // 
	        this.JustifyCenterButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.JustifyCenterButton.Location = new System.Drawing.Point(40, 39);
	        this.JustifyCenterButton.Name = "JustifyCenterButton";
	        this.JustifyCenterButton.Size = new System.Drawing.Size(20, 20);
	        this.JustifyCenterButton.TabIndex = 12;
	        this.JustifyCenterButton.Text = "C";
	        this.JustifyCenterButton.UseVisualStyleBackColor = true;
	        this.JustifyCenterButton.Click += new System.EventHandler(this.JustifyCenterClicked);
	        // 
	        // JustifyLeftButton
	        // 
	        this.JustifyLeftButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.JustifyLeftButton.Location = new System.Drawing.Point(16, 39);
	        this.JustifyLeftButton.Name = "JustifyLeftButton";
	        this.JustifyLeftButton.Size = new System.Drawing.Size(20, 20);
	        this.JustifyLeftButton.TabIndex = 11;
	        this.JustifyLeftButton.Text = "L";
	        this.JustifyLeftButton.UseVisualStyleBackColor = true;
	        this.JustifyLeftButton.Click += new System.EventHandler(this.JustifyLeftClicked);
	        // 
	        // RightClickCombo
	        // 
	        this.RightClickCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
	        this.RightClickCombo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.RightClickCombo.FormattingEnabled = true;
	        this.RightClickCombo.Items.AddRange(new object[] { "Apply Texture", "Apply Texture and Values", "Align To View" });
	        this.RightClickCombo.Location = new System.Drawing.Point(88, 390);
	        this.RightClickCombo.Name = "RightClickCombo";
	        this.RightClickCombo.Size = new System.Drawing.Size(159, 23);
	        this.RightClickCombo.TabIndex = 19;
	        this.RightClickCombo.SelectedIndexChanged += new System.EventHandler(this.RightClickComboChanged);
	        // 
	        // label7
	        // 
	        this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.label7.Location = new System.Drawing.Point(12, 390);
	        this.label7.Name = "label7";
	        this.label7.Size = new System.Drawing.Size(70, 21);
	        this.label7.TabIndex = 26;
	        this.label7.Text = "Right Click:";
	        this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
	        // 
	        // LeftClickCombo
	        // 
	        this.LeftClickCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
	        this.LeftClickCombo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.LeftClickCombo.FormattingEnabled = true;
	        this.LeftClickCombo.Items.AddRange(new object[] { "Lift and Select", "Lift", "Select" });
	        this.LeftClickCombo.Location = new System.Drawing.Point(88, 363);
	        this.LeftClickCombo.Name = "LeftClickCombo";
	        this.LeftClickCombo.Size = new System.Drawing.Size(159, 23);
	        this.LeftClickCombo.TabIndex = 18;
	        this.LeftClickCombo.SelectedIndexChanged += new System.EventHandler(this.LeftClickComboChanged);
	        // 
	        // label6
	        // 
	        this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.label6.Location = new System.Drawing.Point(12, 363);
	        this.label6.Name = "label6";
	        this.label6.Size = new System.Drawing.Size(70, 21);
	        this.label6.TabIndex = 25;
	        this.label6.Text = "Left Click:";
	        this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
	        // 
	        // ApplyButton
	        // 
	        this.ApplyButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.ApplyButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.ApplyButton.Location = new System.Drawing.Point(93, 97);
	        this.ApplyButton.Name = "ApplyButton";
	        this.ApplyButton.Size = new System.Drawing.Size(75, 52);
	        this.ApplyButton.TabIndex = 5;
	        this.ApplyButton.Text = "Apply";
	        this.ApplyButton.UseVisualStyleBackColor = true;
	        this.ApplyButton.Click += new System.EventHandler(this.ApplyButtonClicked);
	        // 
	        // RotationValue
	        // 
	        this.RotationValue.BackColor = System.Drawing.SystemColors.Window;
	        this.RotationValue.DecimalPlaces = 2;
	        this.RotationValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.RotationValue.Location = new System.Drawing.Point(237, 15);
	        this.RotationValue.Maximum = new decimal(new int[] { 360, 0, 0, 0 });
	        this.RotationValue.Minimum = new decimal(new int[] { 360, 0, 0, -2147483648 });
	        this.RotationValue.Name = "RotationValue";
	        this.RotationValue.Size = new System.Drawing.Size(57, 23);
	        this.RotationValue.TabIndex = 4;
	        this.RotationValue.ValueChanged += new System.EventHandler(this.RotationValueChanged);
	        this.RotationValue.Enter += new System.EventHandler(this.FocusTextInControl);
	        // 
	        // ReplaceButton
	        // 
	        this.ReplaceButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.ReplaceButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.ReplaceButton.Location = new System.Drawing.Point(12, 126);
	        this.ReplaceButton.Name = "ReplaceButton";
	        this.ReplaceButton.Size = new System.Drawing.Size(75, 23);
	        this.ReplaceButton.TabIndex = 7;
	        this.ReplaceButton.Text = "Replace...";
	        this.ReplaceButton.UseVisualStyleBackColor = true;
	        this.ReplaceButton.Click += new System.EventHandler(this.ReplaceButtonClicked);
	        // 
	        // BrowseButton
	        // 
	        this.BrowseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
	        this.BrowseButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.BrowseButton.Location = new System.Drawing.Point(12, 97);
	        this.BrowseButton.Name = "BrowseButton";
	        this.BrowseButton.Size = new System.Drawing.Size(75, 23);
	        this.BrowseButton.TabIndex = 6;
	        this.BrowseButton.Text = "Browse...";
	        this.BrowseButton.UseVisualStyleBackColor = true;
	        this.BrowseButton.Click += new System.EventHandler(this.BrowseButtonClicked);
	        // 
	        // label5
	        // 
	        this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.label5.Location = new System.Drawing.Point(174, 13);
	        this.label5.Name = "label5";
	        this.label5.Size = new System.Drawing.Size(59, 25);
	        this.label5.TabIndex = 17;
	        this.label5.Text = "Rotation";
	        this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
	        // 
	        // TextureDetailsLabel
	        // 
	        this.TextureDetailsLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.TextureDetailsLabel.Location = new System.Drawing.Point(12, 152);
	        this.TextureDetailsLabel.Name = "TextureDetailsLabel";
	        this.TextureDetailsLabel.Size = new System.Drawing.Size(393, 23);
	        this.TextureDetailsLabel.TabIndex = 21;
	        this.TextureDetailsLabel.Text = "TEXTURENAMEANDSIZE";
	        this.TextureDetailsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
	        // 
	        // tableLayoutPanel1
	        // 
	        this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
	        this.tableLayoutPanel1.ColumnCount = 3;
	        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
	        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
	        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 214F));
	        this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
	        this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
	        this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
	        this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
	        this.tableLayoutPanel1.Controls.Add(this.ScaleYValue, 1, 2);
	        this.tableLayoutPanel1.Controls.Add(this.ShiftXValue, 2, 1);
	        this.tableLayoutPanel1.Controls.Add(this.ShiftYValue, 2, 2);
	        this.tableLayoutPanel1.Controls.Add(this.ScaleXValue, 1, 1);
	        this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 12);
	        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
	        this.tableLayoutPanel1.RowCount = 3;
	        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.31646F));
	        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 39.24051F));
	        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35.44304F));
	        this.tableLayoutPanel1.Size = new System.Drawing.Size(156, 80);
	        this.tableLayoutPanel1.TabIndex = 20;
	        // 
	        // label1
	        // 
	        this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.label1.Location = new System.Drawing.Point(25, 1);
	        this.label1.Name = "label1";
	        this.label1.Size = new System.Drawing.Size(59, 19);
	        this.label1.TabIndex = 0;
	        this.label1.Text = "Scale";
	        this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
	        // 
	        // label2
	        // 
	        this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.label2.Location = new System.Drawing.Point(4, 21);
	        this.label2.Name = "label2";
	        this.label2.Size = new System.Drawing.Size(14, 25);
	        this.label2.TabIndex = 2;
	        this.label2.Text = "X";
	        this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
	        // 
	        // label3
	        // 
	        this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.label3.Location = new System.Drawing.Point(4, 51);
	        this.label3.Name = "label3";
	        this.label3.Size = new System.Drawing.Size(14, 26);
	        this.label3.TabIndex = 2;
	        this.label3.Text = "Y";
	        this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
	        // 
	        // label4
	        // 
	        this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.label4.Location = new System.Drawing.Point(91, 1);
	        this.label4.Name = "label4";
	        this.label4.Size = new System.Drawing.Size(59, 19);
	        this.label4.TabIndex = 0;
	        this.label4.Text = "Shift";
	        this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
	        // 
	        // ScaleYValue
	        // 
	        this.ScaleYValue.DecimalPlaces = 4;
	        this.ScaleYValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.ScaleYValue.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
	        this.ScaleYValue.Location = new System.Drawing.Point(25, 54);
	        this.ScaleYValue.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
	        this.ScaleYValue.Minimum = new decimal(new int[] { 4096, 0, 0, -2147483648 });
	        this.ScaleYValue.Name = "ScaleYValue";
	        this.ScaleYValue.Size = new System.Drawing.Size(57, 23);
	        this.ScaleYValue.TabIndex = 1;
	        this.ScaleYValue.Value = new decimal(new int[] { 100, 0, 0, 131072 });
	        this.ScaleYValue.ValueChanged += new System.EventHandler(this.ScaleYValueChanged);
	        this.ScaleYValue.Enter += new System.EventHandler(this.FocusTextInControl);
	        // 
	        // ShiftXValue
	        // 
	        this.ShiftXValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.ShiftXValue.Location = new System.Drawing.Point(91, 24);
	        this.ShiftXValue.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
	        this.ShiftXValue.Minimum = new decimal(new int[] { 4096, 0, 0, -2147483648 });
	        this.ShiftXValue.Name = "ShiftXValue";
	        this.ShiftXValue.Size = new System.Drawing.Size(59, 23);
	        this.ShiftXValue.TabIndex = 2;
	        this.ShiftXValue.Value = new decimal(new int[] { 1, 0, 0, 65536 });
	        this.ShiftXValue.ValueChanged += new System.EventHandler(this.ShiftXValueChanged);
	        this.ShiftXValue.Enter += new System.EventHandler(this.FocusTextInControl);
	        // 
	        // ShiftYValue
	        // 
	        this.ShiftYValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.ShiftYValue.Location = new System.Drawing.Point(91, 54);
	        this.ShiftYValue.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
	        this.ShiftYValue.Minimum = new decimal(new int[] { 4096, 0, 0, -2147483648 });
	        this.ShiftYValue.Name = "ShiftYValue";
	        this.ShiftYValue.Size = new System.Drawing.Size(58, 23);
	        this.ShiftYValue.TabIndex = 3;
	        this.ShiftYValue.Value = new decimal(new int[] { 1, 0, 0, 65536 });
	        this.ShiftYValue.ValueChanged += new System.EventHandler(this.ShiftYValueChanged);
	        this.ShiftYValue.Enter += new System.EventHandler(this.FocusTextInControl);
	        // 
	        // ScaleXValue
	        // 
	        this.ScaleXValue.DecimalPlaces = 4;
	        this.ScaleXValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.ScaleXValue.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
	        this.ScaleXValue.Location = new System.Drawing.Point(25, 24);
	        this.ScaleXValue.Maximum = new decimal(new int[] { 4096, 0, 0, 0 });
	        this.ScaleXValue.Minimum = new decimal(new int[] { 4096, 0, 0, -2147483648 });
	        this.ScaleXValue.Name = "ScaleXValue";
	        this.ScaleXValue.Size = new System.Drawing.Size(57, 23);
	        this.ScaleXValue.TabIndex = 0;
	        this.ScaleXValue.Value = new decimal(new int[] { 100, 0, 0, 131072 });
	        this.ScaleXValue.ValueChanged += new System.EventHandler(this.ScaleXValueChanged);
	        this.ScaleXValue.Enter += new System.EventHandler(this.FocusTextInControl);
	        // 
	        // HoverTip
	        // 
	        this.HoverTip.AutoPopDelay = 5000;
	        this.HoverTip.InitialDelay = 200;
	        this.HoverTip.IsBalloon = true;
	        this.HoverTip.ReshowDelay = 100;
	        this.HoverTip.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
	        this.HoverTip.ToolTipTitle = "Help";
	        // 
	        // RecentTexturesList
	        // 
	        this.RecentTexturesList.AllowMultipleSelection = false;
	        this.RecentTexturesList.AllowSelection = true;
	        this.RecentTexturesList.AutoScroll = true;
	        this.RecentTexturesList.BackColor = System.Drawing.Color.Black;
	        this.RecentTexturesList.EnableDrag = false;
	        this.RecentTexturesList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.RecentTexturesList.ImageSize = 64;
	        this.RecentTexturesList.Location = new System.Drawing.Point(318, 178);
	        this.RecentTexturesList.Name = "RecentTexturesList";
	        this.RecentTexturesList.Size = new System.Drawing.Size(87, 179);
	        this.RecentTexturesList.SortDescending = false;
	        this.RecentTexturesList.SortOrder = CBRE.Editor.UI.TextureListPanel.TextureSortOrder.None;
	        this.RecentTexturesList.TabIndex = 38;
	        this.RecentTexturesList.TextureSelected += new CBRE.Editor.UI.TextureListPanel.TextureSelectedEventHandler(this.TexturesListTextureSelected);
	        // 
	        // SelectedTexturesList
	        // 
	        this.SelectedTexturesList.AllowMultipleSelection = false;
	        this.SelectedTexturesList.AllowSelection = true;
	        this.SelectedTexturesList.AutoScroll = true;
	        this.SelectedTexturesList.BackColor = System.Drawing.Color.Black;
	        this.SelectedTexturesList.EnableDrag = false;
	        this.SelectedTexturesList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
	        this.SelectedTexturesList.ImageSize = 64;
	        this.SelectedTexturesList.Location = new System.Drawing.Point(12, 178);
	        this.SelectedTexturesList.Name = "SelectedTexturesList";
	        this.SelectedTexturesList.Size = new System.Drawing.Size(300, 179);
	        this.SelectedTexturesList.SortDescending = false;
	        this.SelectedTexturesList.SortOrder = CBRE.Editor.UI.TextureListPanel.TextureSortOrder.Name;
	        this.SelectedTexturesList.TabIndex = 37;
	        this.SelectedTexturesList.TextureSelected += new CBRE.Editor.UI.TextureListPanel.TextureSelectedEventHandler(this.TexturesListTextureSelected);
	        // 
	        // TextureApplicationForm
	        // 
	        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
	        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	        this.ClientSize = new System.Drawing.Size(427, 430);
	        this.Controls.Add(this.HideMaskCheckbox);
	        this.Controls.Add(this.RecentTexturesList);
	        this.Controls.Add(this.SelectedTexturesList);
	        this.Controls.Add(this.label11);
	        this.Controls.Add(this.label10);
	        this.Controls.Add(this.RecentFilterTextbox);
	        this.Controls.Add(this.label9);
	        this.Controls.Add(this.groupBox2);
	        this.Controls.Add(this.groupBox1);
	        this.Controls.Add(this.RightClickCombo);
	        this.Controls.Add(this.label7);
	        this.Controls.Add(this.LeftClickCombo);
	        this.Controls.Add(this.label6);
	        this.Controls.Add(this.ApplyButton);
	        this.Controls.Add(this.RotationValue);
	        this.Controls.Add(this.ReplaceButton);
	        this.Controls.Add(this.BrowseButton);
	        this.Controls.Add(this.label5);
	        this.Controls.Add(this.TextureDetailsLabel);
	        this.Controls.Add(this.tableLayoutPanel1);
	        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
	        this.MaximizeBox = false;
	        this.MaximumSize = new System.Drawing.Size(433, 459);
	        this.MinimizeBox = false;
	        this.MinimumSize = new System.Drawing.Size(433, 459);
	        this.Name = "TextureApplicationForm";
	        this.ShowIcon = false;
	        this.ShowInTaskbar = false;
	        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
	        this.Text = "Texture Application";
	        this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnClosing);
	        this.groupBox2.ResumeLayout(false);
	        this.groupBox2.PerformLayout();
	        this.groupBox1.ResumeLayout(false);
	        ((System.ComponentModel.ISupportInitialize)(this.RotationValue)).EndInit();
	        this.tableLayoutPanel1.ResumeLayout(false);
	        ((System.ComponentModel.ISupportInitialize)(this.ScaleYValue)).EndInit();
	        ((System.ComponentModel.ISupportInitialize)(this.ShiftXValue)).EndInit();
	        ((System.ComponentModel.ISupportInitialize)(this.ShiftYValue)).EndInit();
	        ((System.ComponentModel.ISupportInitialize)(this.ScaleXValue)).EndInit();
	        this.ResumeLayout(false);
	        this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox HideMaskCheckbox;
        private System.Windows.Forms.TextBox RecentFilterTextbox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button JustifyTopButton;
        private System.Windows.Forms.Button JustifyFitButton;
        private System.Windows.Forms.CheckBox TreatAsOneCheckbox;
        private System.Windows.Forms.Button JustifyRightButton;
        private System.Windows.Forms.Button JustifyBottomButton;
        private System.Windows.Forms.Button JustifyCenterButton;
        private System.Windows.Forms.Button JustifyLeftButton;
        private System.Windows.Forms.ComboBox RightClickCombo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox LeftClickCombo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button ApplyButton;
        private System.Windows.Forms.NumericUpDown RotationValue;
        private System.Windows.Forms.Button ReplaceButton;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label TextureDetailsLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown ScaleXValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown ScaleYValue;
        private System.Windows.Forms.NumericUpDown ShiftXValue;
        private System.Windows.Forms.NumericUpDown ShiftYValue;
        private TextureListPanel SelectedTexturesList;
        private CBRE.Editor.UI.TextureListPanel RecentTexturesList;
        private System.Windows.Forms.ToolTip HoverTip;
        private System.Windows.Forms.CheckBox AlignToFaceCheckbox;
        private System.Windows.Forms.CheckBox AlignToWorldCheckbox;
    }
}