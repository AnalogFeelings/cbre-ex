using CBRE.Localization;

namespace CBRE.Editor.UI
{
    partial class PasteSpecialDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasteSpecialDialog));
			this.label1 = new System.Windows.Forms.Label();
			this.NumCopies = new System.Windows.Forms.NumericUpDown();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.StartSelection = new System.Windows.Forms.RadioButton();
			this.StartOriginal = new System.Windows.Forms.RadioButton();
			this.StartOrigin = new System.Windows.Forms.RadioButton();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.GroupAll = new System.Windows.Forms.RadioButton();
			this.GroupIndividual = new System.Windows.Forms.RadioButton();
			this.GroupNone = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.OffsetY = new System.Windows.Forms.NumericUpDown();
			this.OffsetZ = new System.Windows.Forms.NumericUpDown();
			this.OffsetX = new System.Windows.Forms.NumericUpDown();
			this.SourceOffsetZButton = new System.Windows.Forms.Button();
			this.ZeroOffsetZButton = new System.Windows.Forms.Button();
			this.SourceOffsetYButton = new System.Windows.Forms.Button();
			this.ZeroOffsetYButton = new System.Windows.Forms.Button();
			this.SourceOffsetXButton = new System.Windows.Forms.Button();
			this.ZeroOffsetXButton = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.UniqueEntityNames = new System.Windows.Forms.CheckBox();
			this.PrefixEntityNamesCheckbox = new System.Windows.Forms.CheckBox();
			this.EntityPrefix = new System.Windows.Forms.TextBox();
			this.OkButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.RotationY = new System.Windows.Forms.NumericUpDown();
			this.RotationZ = new System.Windows.Forms.NumericUpDown();
			this.RotationX = new System.Windows.Forms.NumericUpDown();
			this.ZeroRotationZButton = new System.Windows.Forms.Button();
			this.ZeroRotationYButton = new System.Windows.Forms.Button();
			this.ZeroRotationXButton = new System.Windows.Forms.Button();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.NumCopies)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.OffsetY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.OffsetZ)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.OffsetX)).BeginInit();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.RotationY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.RotationZ)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.RotationX)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(147, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = Local.LocalString("paste_special.number");
			// 
			// NumCopies
			// 
			this.NumCopies.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.NumCopies.Location = new System.Drawing.Point(165, 7);
			this.NumCopies.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
			this.NumCopies.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.NumCopies.Name = "NumCopies";
			this.NumCopies.Size = new System.Drawing.Size(57, 23);
			this.NumCopies.TabIndex = 1;
			this.NumCopies.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.StartSelection);
			this.groupBox1.Controls.Add(this.StartOriginal);
			this.groupBox1.Controls.Add(this.StartOrigin);
			this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(7, 33);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(220, 101);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = Local.LocalString("paste_special.start_point");
			// 
			// StartSelection
			// 
			this.StartSelection.AutoSize = true;
			this.StartSelection.Location = new System.Drawing.Point(5, 72);
			this.StartSelection.Name = "StartSelection";
			this.StartSelection.Size = new System.Drawing.Size(203, 19);
			this.StartSelection.TabIndex = 0;
			this.StartSelection.Text = Local.LocalString("paste_special.start_point.center_of_current");
			this.StartSelection.UseVisualStyleBackColor = true;
			// 
			// StartOriginal
			// 
			this.StartOriginal.AutoSize = true;
			this.StartOriginal.Checked = true;
			this.StartOriginal.Location = new System.Drawing.Point(5, 47);
			this.StartOriginal.Name = "StartOriginal";
			this.StartOriginal.Size = new System.Drawing.Size(155, 19);
			this.StartOriginal.TabIndex = 0;
			this.StartOriginal.TabStop = true;
			this.StartOriginal.Text = Local.LocalString("paste_special.start_point.center_of_original");
			this.StartOriginal.UseVisualStyleBackColor = true;
			// 
			// StartOrigin
			// 
			this.StartOrigin.AutoSize = true;
			this.StartOrigin.Location = new System.Drawing.Point(6, 22);
			this.StartOrigin.Name = "StartOrigin";
			this.StartOrigin.Size = new System.Drawing.Size(123, 19);
			this.StartOrigin.TabIndex = 0;
			this.StartOrigin.Text = Local.LocalString("paste_special.start_point.map_origin");
			this.StartOrigin.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.GroupAll);
			this.groupBox2.Controls.Add(this.GroupIndividual);
			this.groupBox2.Controls.Add(this.GroupNone);
			this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(233, 33);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(215, 101);
			this.groupBox2.TabIndex = 3;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = Local.LocalString("paste_special.grouping");
			// 
			// GroupAll
			// 
			this.GroupAll.AutoSize = true;
			this.GroupAll.Location = new System.Drawing.Point(11, 72);
			this.GroupAll.Name = "GroupAll";
			this.GroupAll.Size = new System.Drawing.Size(110, 19);
			this.GroupAll.TabIndex = 0;
			this.GroupAll.Text = Local.LocalString("paste_special.grouping.all");
			this.GroupAll.UseVisualStyleBackColor = true;
			// 
			// GroupIndividual
			// 
			this.GroupIndividual.AutoSize = true;
			this.GroupIndividual.Checked = true;
			this.GroupIndividual.Location = new System.Drawing.Point(11, 47);
			this.GroupIndividual.Name = "GroupIndividual";
			this.GroupIndividual.Size = new System.Drawing.Size(150, 19);
			this.GroupIndividual.TabIndex = 0;
			this.GroupIndividual.TabStop = true;
			this.GroupIndividual.Text = Local.LocalString("paste_special.grouping.individual");
			this.GroupIndividual.UseVisualStyleBackColor = true;
			// 
			// GroupNone
			// 
			this.GroupNone.AutoSize = true;
			this.GroupNone.Location = new System.Drawing.Point(11, 22);
			this.GroupNone.Name = "GroupNone";
			this.GroupNone.Size = new System.Drawing.Size(93, 19);
			this.GroupNone.TabIndex = 0;
			this.GroupNone.Text = Local.LocalString("paste_special.grouping.no");
			this.GroupNone.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.OffsetY);
			this.groupBox3.Controls.Add(this.OffsetZ);
			this.groupBox3.Controls.Add(this.OffsetX);
			this.groupBox3.Controls.Add(this.SourceOffsetZButton);
			this.groupBox3.Controls.Add(this.ZeroOffsetZButton);
			this.groupBox3.Controls.Add(this.SourceOffsetYButton);
			this.groupBox3.Controls.Add(this.ZeroOffsetYButton);
			this.groupBox3.Controls.Add(this.SourceOffsetXButton);
			this.groupBox3.Controls.Add(this.ZeroOffsetXButton);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.label7);
			this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox3.Location = new System.Drawing.Point(7, 140);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(220, 104);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = Local.LocalString("paste_special.offset");
			// 
			// OffsetY
			// 
			this.OffsetY.DecimalPlaces = 2;
			this.OffsetY.Location = new System.Drawing.Point(29, 46);
			this.OffsetY.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
			this.OffsetY.Minimum = new decimal(new int[] {
            16384,
            0,
            0,
            -2147483648});
			this.OffsetY.Name = "OffsetY";
			this.OffsetY.Size = new System.Drawing.Size(66, 23);
			this.OffsetY.TabIndex = 14;
			// 
			// OffsetZ
			// 
			this.OffsetZ.DecimalPlaces = 2;
			this.OffsetZ.Location = new System.Drawing.Point(29, 72);
			this.OffsetZ.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
			this.OffsetZ.Minimum = new decimal(new int[] {
            16384,
            0,
            0,
            -2147483648});
			this.OffsetZ.Name = "OffsetZ";
			this.OffsetZ.Size = new System.Drawing.Size(66, 23);
			this.OffsetZ.TabIndex = 14;
			// 
			// OffsetX
			// 
			this.OffsetX.DecimalPlaces = 2;
			this.OffsetX.Location = new System.Drawing.Point(29, 20);
			this.OffsetX.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
			this.OffsetX.Minimum = new decimal(new int[] {
            16384,
            0,
            0,
            -2147483648});
			this.OffsetX.Name = "OffsetX";
			this.OffsetX.Size = new System.Drawing.Size(66, 23);
			this.OffsetX.TabIndex = 14;
			// 
			// SourceOffsetZButton
			// 
			this.SourceOffsetZButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SourceOffsetZButton.Location = new System.Drawing.Point(132, 72);
			this.SourceOffsetZButton.Name = "SourceOffsetZButton";
			this.SourceOffsetZButton.Size = new System.Drawing.Size(70, 23);
			this.SourceOffsetZButton.TabIndex = 13;
			this.SourceOffsetZButton.Text = Local.LocalString("object_properties.source");
			this.SourceOffsetZButton.UseVisualStyleBackColor = true;
			// 
			// ZeroOffsetZButton
			// 
			this.ZeroOffsetZButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ZeroOffsetZButton.Location = new System.Drawing.Point(101, 72);
			this.ZeroOffsetZButton.Name = "ZeroOffsetZButton";
			this.ZeroOffsetZButton.Size = new System.Drawing.Size(25, 23);
			this.ZeroOffsetZButton.TabIndex = 13;
			this.ZeroOffsetZButton.Text = "0";
			this.ZeroOffsetZButton.UseVisualStyleBackColor = true;
			// 
			// SourceOffsetYButton
			// 
			this.SourceOffsetYButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SourceOffsetYButton.Location = new System.Drawing.Point(132, 46);
			this.SourceOffsetYButton.Name = "SourceOffsetYButton";
			this.SourceOffsetYButton.Size = new System.Drawing.Size(70, 23);
			this.SourceOffsetYButton.TabIndex = 13;
			this.SourceOffsetYButton.Text = Local.LocalString("object_properties.source");
			this.SourceOffsetYButton.UseVisualStyleBackColor = true;
			// 
			// ZeroOffsetYButton
			// 
			this.ZeroOffsetYButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ZeroOffsetYButton.Location = new System.Drawing.Point(101, 46);
			this.ZeroOffsetYButton.Name = "ZeroOffsetYButton";
			this.ZeroOffsetYButton.Size = new System.Drawing.Size(25, 23);
			this.ZeroOffsetYButton.TabIndex = 13;
			this.ZeroOffsetYButton.Text = "0";
			this.ZeroOffsetYButton.UseVisualStyleBackColor = true;
			// 
			// SourceOffsetXButton
			// 
			this.SourceOffsetXButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.SourceOffsetXButton.Location = new System.Drawing.Point(132, 20);
			this.SourceOffsetXButton.Name = "SourceOffsetXButton";
			this.SourceOffsetXButton.Size = new System.Drawing.Size(70, 23);
			this.SourceOffsetXButton.TabIndex = 13;
			this.SourceOffsetXButton.Text = Local.LocalString("object_properties.source");
			this.SourceOffsetXButton.UseVisualStyleBackColor = true;
			// 
			// ZeroOffsetXButton
			// 
			this.ZeroOffsetXButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ZeroOffsetXButton.Location = new System.Drawing.Point(101, 20);
			this.ZeroOffsetXButton.Name = "ZeroOffsetXButton";
			this.ZeroOffsetXButton.Size = new System.Drawing.Size(25, 23);
			this.ZeroOffsetXButton.TabIndex = 13;
			this.ZeroOffsetXButton.Text = "0";
			this.ZeroOffsetXButton.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 75);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(17, 15);
			this.label5.TabIndex = 12;
			this.label5.Text = "Z:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 50);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(17, 15);
			this.label6.TabIndex = 10;
			this.label6.Text = "Y:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(6, 24);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(17, 15);
			this.label7.TabIndex = 8;
			this.label7.Text = "X:";
			// 
			// UniqueEntityNames
			// 
			this.UniqueEntityNames.AutoSize = true;
			this.UniqueEntityNames.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.UniqueEntityNames.Location = new System.Drawing.Point(12, 254);
			this.UniqueEntityNames.Name = "UniqueEntityNames";
			this.UniqueEntityNames.Size = new System.Drawing.Size(166, 19);
			this.UniqueEntityNames.TabIndex = 15;
			this.UniqueEntityNames.Text = Local.LocalString("paste_special.unique_name");
			this.UniqueEntityNames.UseVisualStyleBackColor = true;
			// 
			// PrefixEntityNamesCheckbox
			// 
			this.PrefixEntityNamesCheckbox.AutoSize = true;
			this.PrefixEntityNamesCheckbox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PrefixEntityNamesCheckbox.Location = new System.Drawing.Point(12, 279);
			this.PrefixEntityNamesCheckbox.Name = "PrefixEntityNamesCheckbox";
			this.PrefixEntityNamesCheckbox.Size = new System.Drawing.Size(166, 19);
			this.PrefixEntityNamesCheckbox.TabIndex = 15;
			this.PrefixEntityNamesCheckbox.Text = Local.LocalString("paste_special.prefix");
			this.PrefixEntityNamesCheckbox.UseVisualStyleBackColor = true;
			// 
			// EntityPrefix
			// 
			this.EntityPrefix.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.EntityPrefix.Location = new System.Drawing.Point(184, 277);
			this.EntityPrefix.Name = "EntityPrefix";
			this.EntityPrefix.Size = new System.Drawing.Size(78, 23);
			this.EntityPrefix.TabIndex = 11;
			// 
			// OkButton
			// 
			this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.OkButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OkButton.Location = new System.Drawing.Point(292, 277);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(75, 23);
			this.OkButton.TabIndex = 16;
			this.OkButton.Text = Local.LocalString("ok");
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += new System.EventHandler(this.OkButtonClicked);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelButton.Location = new System.Drawing.Point(373, 277);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 16;
			this.cancelButton.Text = Local.LocalString("cancel");
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.RotationY);
			this.groupBox4.Controls.Add(this.RotationZ);
			this.groupBox4.Controls.Add(this.RotationX);
			this.groupBox4.Controls.Add(this.ZeroRotationZButton);
			this.groupBox4.Controls.Add(this.ZeroRotationYButton);
			this.groupBox4.Controls.Add(this.ZeroRotationXButton);
			this.groupBox4.Controls.Add(this.label8);
			this.groupBox4.Controls.Add(this.label9);
			this.groupBox4.Controls.Add(this.label10);
			this.groupBox4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox4.Location = new System.Drawing.Point(233, 140);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(215, 104);
			this.groupBox4.TabIndex = 15;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = Local.LocalString("paste_special.rotation");
			// 
			// RotationY
			// 
			this.RotationY.DecimalPlaces = 2;
			this.RotationY.Location = new System.Drawing.Point(29, 46);
			this.RotationY.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
			this.RotationY.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
			this.RotationY.Name = "RotationY";
			this.RotationY.Size = new System.Drawing.Size(66, 23);
			this.RotationY.TabIndex = 14;
			// 
			// RotationZ
			// 
			this.RotationZ.DecimalPlaces = 2;
			this.RotationZ.Location = new System.Drawing.Point(29, 72);
			this.RotationZ.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
			this.RotationZ.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
			this.RotationZ.Name = "RotationZ";
			this.RotationZ.Size = new System.Drawing.Size(66, 23);
			this.RotationZ.TabIndex = 14;
			// 
			// RotationX
			// 
			this.RotationX.DecimalPlaces = 2;
			this.RotationX.Location = new System.Drawing.Point(29, 20);
			this.RotationX.Maximum = new decimal(new int[] {
            360,
            0,
            0,
            0});
			this.RotationX.Minimum = new decimal(new int[] {
            360,
            0,
            0,
            -2147483648});
			this.RotationX.Name = "RotationX";
			this.RotationX.Size = new System.Drawing.Size(66, 23);
			this.RotationX.TabIndex = 14;
			// 
			// ZeroRotationZButton
			// 
			this.ZeroRotationZButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ZeroRotationZButton.Location = new System.Drawing.Point(102, 72);
			this.ZeroRotationZButton.Name = "ZeroRotationZButton";
			this.ZeroRotationZButton.Size = new System.Drawing.Size(25, 23);
			this.ZeroRotationZButton.TabIndex = 13;
			this.ZeroRotationZButton.Text = "0";
			this.ZeroRotationZButton.UseVisualStyleBackColor = true;
			// 
			// ZeroRotationYButton
			// 
			this.ZeroRotationYButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ZeroRotationYButton.Location = new System.Drawing.Point(101, 46);
			this.ZeroRotationYButton.Name = "ZeroRotationYButton";
			this.ZeroRotationYButton.Size = new System.Drawing.Size(25, 23);
			this.ZeroRotationYButton.TabIndex = 13;
			this.ZeroRotationYButton.Text = "0";
			this.ZeroRotationYButton.UseVisualStyleBackColor = true;
			// 
			// ZeroRotationXButton
			// 
			this.ZeroRotationXButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ZeroRotationXButton.Location = new System.Drawing.Point(101, 20);
			this.ZeroRotationXButton.Name = "ZeroRotationXButton";
			this.ZeroRotationXButton.Size = new System.Drawing.Size(25, 23);
			this.ZeroRotationXButton.TabIndex = 13;
			this.ZeroRotationXButton.Text = "0";
			this.ZeroRotationXButton.UseVisualStyleBackColor = true;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(6, 75);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(17, 15);
			this.label8.TabIndex = 12;
			this.label8.Text = "Z:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(6, 48);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(17, 15);
			this.label9.TabIndex = 10;
			this.label9.Text = "Y:";
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(6, 24);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(17, 15);
			this.label10.TabIndex = 8;
			this.label10.Text = "X:";
			// 
			// PasteSpecialDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(460, 312);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.PrefixEntityNamesCheckbox);
			this.Controls.Add(this.UniqueEntityNames);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.NumCopies);
			this.Controls.Add(this.EntityPrefix);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "PasteSpecialDialog";
			this.ShowInTaskbar = false;
			this.Text = Local.LocalString("paste_special");
			((System.ComponentModel.ISupportInitialize)(this.NumCopies)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.OffsetY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.OffsetZ)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.OffsetX)).EndInit();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.RotationY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.RotationZ)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.RotationX)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown NumCopies;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton StartSelection;
        private System.Windows.Forms.RadioButton StartOriginal;
        private System.Windows.Forms.RadioButton StartOrigin;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton GroupAll;
        private System.Windows.Forms.RadioButton GroupIndividual;
        private System.Windows.Forms.RadioButton GroupNone;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button SourceOffsetZButton;
        private System.Windows.Forms.Button ZeroOffsetZButton;
        private System.Windows.Forms.Button SourceOffsetYButton;
        private System.Windows.Forms.Button ZeroOffsetYButton;
        private System.Windows.Forms.Button SourceOffsetXButton;
        private System.Windows.Forms.Button ZeroOffsetXButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox UniqueEntityNames;
        private System.Windows.Forms.CheckBox PrefixEntityNamesCheckbox;
        private System.Windows.Forms.TextBox EntityPrefix;
        private System.Windows.Forms.Button OkButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.NumericUpDown OffsetY;
        private System.Windows.Forms.NumericUpDown OffsetZ;
        private System.Windows.Forms.NumericUpDown OffsetX;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown RotationY;
        private System.Windows.Forms.NumericUpDown RotationZ;
        private System.Windows.Forms.NumericUpDown RotationX;
        private System.Windows.Forms.Button ZeroRotationZButton;
        private System.Windows.Forms.Button ZeroRotationYButton;
        private System.Windows.Forms.Button ZeroRotationXButton;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}