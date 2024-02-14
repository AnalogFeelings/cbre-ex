using CBRE.Localization;

namespace CBRE.Editor.UI
{
    partial class TransformDialog
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
			this.Rotate = new System.Windows.Forms.RadioButton();
			this.Translate = new System.Windows.Forms.RadioButton();
			this.scale = new System.Windows.Forms.RadioButton();
			this.ValueY = new System.Windows.Forms.NumericUpDown();
			this.ValueZ = new System.Windows.Forms.NumericUpDown();
			this.ValueX = new System.Windows.Forms.NumericUpDown();
			this.SourceValueZButton = new System.Windows.Forms.Button();
			this.ZeroValueZButton = new System.Windows.Forms.Button();
			this.SourceValueYButton = new System.Windows.Forms.Button();
			this.ZeroValueYButton = new System.Windows.Forms.Button();
			this.SourceValueXButton = new System.Windows.Forms.Button();
			this.ZeroValueXButton = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.OkButton = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.ValueY)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ValueZ)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.ValueX)).BeginInit();
			this.SuspendLayout();
			// 
			// Rotate
			// 
			this.Rotate.AutoSize = true;
			this.Rotate.Checked = true;
			this.Rotate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Rotate.Location = new System.Drawing.Point(12, 11);
			this.Rotate.Name = "Rotate";
			this.Rotate.Size = new System.Drawing.Size(59, 19);
			this.Rotate.TabIndex = 0;
			this.Rotate.TabStop = true;
			this.Rotate.Text = Local.LocalString("tool.rotate");
			this.Rotate.UseVisualStyleBackColor = true;
			this.Rotate.Click += new System.EventHandler(this.TypeChanged);
			// 
			// Translate
			// 
			this.Translate.AutoSize = true;
			this.Translate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Translate.Location = new System.Drawing.Point(12, 37);
			this.Translate.Name = "Translate";
			this.Translate.Size = new System.Drawing.Size(71, 19);
			this.Translate.TabIndex = 0;
			this.Translate.Text = Local.LocalString("transform.translate");
			this.Translate.UseVisualStyleBackColor = true;
			this.Translate.Click += new System.EventHandler(this.TypeChanged);
			// 
			// scale
			// 
			this.scale.AutoSize = true;
			this.scale.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.scale.Location = new System.Drawing.Point(12, 63);
			this.scale.Name = "scale";
			this.scale.Size = new System.Drawing.Size(52, 19);
			this.scale.TabIndex = 0;
			this.scale.Text = Local.LocalString("data.model.scale");
			this.scale.UseVisualStyleBackColor = true;
			this.scale.Click += new System.EventHandler(this.TypeChanged);
			// 
			// ValueY
			// 
			this.ValueY.DecimalPlaces = 2;
			this.ValueY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ValueY.Location = new System.Drawing.Point(113, 37);
			this.ValueY.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
			this.ValueY.Minimum = new decimal(new int[] {
            16384,
            0,
            0,
            -2147483648});
			this.ValueY.Name = "ValueY";
			this.ValueY.Size = new System.Drawing.Size(66, 23);
			this.ValueY.TabIndex = 24;
			// 
			// ValueZ
			// 
			this.ValueZ.DecimalPlaces = 2;
			this.ValueZ.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ValueZ.Location = new System.Drawing.Point(113, 63);
			this.ValueZ.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
			this.ValueZ.Minimum = new decimal(new int[] {
            16384,
            0,
            0,
            -2147483648});
			this.ValueZ.Name = "ValueZ";
			this.ValueZ.Size = new System.Drawing.Size(66, 23);
			this.ValueZ.TabIndex = 25;
			// 
			// ValueX
			// 
			this.ValueX.DecimalPlaces = 2;
			this.ValueX.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ValueX.Location = new System.Drawing.Point(113, 11);
			this.ValueX.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
			this.ValueX.Minimum = new decimal(new int[] {
            16384,
            0,
            0,
            -2147483648});
			this.ValueX.Name = "ValueX";
			this.ValueX.Size = new System.Drawing.Size(66, 23);
			this.ValueX.TabIndex = 26;
			// 
			// SourceValueZButton
			// 
			this.SourceValueZButton.Location = new System.Drawing.Point(216, 63);
			this.SourceValueZButton.Name = "SourceValueZButton";
			this.SourceValueZButton.Size = new System.Drawing.Size(50, 23);
			this.SourceValueZButton.TabIndex = 18;
			this.SourceValueZButton.Text = Local.LocalString("object_properties.source");
			this.SourceValueZButton.UseVisualStyleBackColor = true;
			// 
			// ZeroValueZButton
			// 
			this.ZeroValueZButton.Location = new System.Drawing.Point(185, 63);
			this.ZeroValueZButton.Name = "ZeroValueZButton";
			this.ZeroValueZButton.Size = new System.Drawing.Size(25, 23);
			this.ZeroValueZButton.TabIndex = 19;
			this.ZeroValueZButton.Text = "0";
			this.ZeroValueZButton.UseVisualStyleBackColor = true;
			// 
			// SourceValueYButton
			// 
			this.SourceValueYButton.Location = new System.Drawing.Point(216, 37);
			this.SourceValueYButton.Name = "SourceValueYButton";
			this.SourceValueYButton.Size = new System.Drawing.Size(50, 23);
			this.SourceValueYButton.TabIndex = 20;
			this.SourceValueYButton.Text = Local.LocalString("object_properties.source");
			this.SourceValueYButton.UseVisualStyleBackColor = true;
			// 
			// ZeroValueYButton
			// 
			this.ZeroValueYButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ZeroValueYButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ZeroValueYButton.Location = new System.Drawing.Point(185, 37);
			this.ZeroValueYButton.Name = "ZeroValueYButton";
			this.ZeroValueYButton.Size = new System.Drawing.Size(25, 23);
			this.ZeroValueYButton.TabIndex = 21;
			this.ZeroValueYButton.Text = "0";
			this.ZeroValueYButton.UseVisualStyleBackColor = true;
			// 
			// SourceValueXButton
			// 
			this.SourceValueXButton.Location = new System.Drawing.Point(216, 10);
			this.SourceValueXButton.Name = "SourceValueXButton";
			this.SourceValueXButton.Size = new System.Drawing.Size(50, 23);
			this.SourceValueXButton.TabIndex = 22;
			this.SourceValueXButton.Text = Local.LocalString("object_properties.source");
			this.SourceValueXButton.UseVisualStyleBackColor = true;
			// 
			// ZeroValueXButton
			// 
			this.ZeroValueXButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ZeroValueXButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ZeroValueXButton.Location = new System.Drawing.Point(185, 11);
			this.ZeroValueXButton.Name = "ZeroValueXButton";
			this.ZeroValueXButton.Size = new System.Drawing.Size(25, 23);
			this.ZeroValueXButton.TabIndex = 23;
			this.ZeroValueXButton.Text = "0";
			this.ZeroValueXButton.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(90, 66);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(17, 15);
			this.label5.TabIndex = 17;
			this.label5.Text = "Z:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(90, 39);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(17, 15);
			this.label6.TabIndex = 16;
			this.label6.Text = "Y:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(90, 14);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(17, 15);
			this.label7.TabIndex = 15;
			this.label7.Text = "X:";
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(191, 106);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 27;
			this.cancelButton.Text = Local.LocalString("cancel");
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// OkButton
			// 
			this.OkButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkButton.Location = new System.Drawing.Point(110, 106);
			this.OkButton.Name = "OkButton";
			this.OkButton.Size = new System.Drawing.Size(75, 23);
			this.OkButton.TabIndex = 28;
			this.OkButton.Text = Local.LocalString("ok");
			this.OkButton.UseVisualStyleBackColor = true;
			this.OkButton.Click += new System.EventHandler(this.OkButtonClicked);
			// 
			// TransformDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(278, 141);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.OkButton);
			this.Controls.Add(this.ValueY);
			this.Controls.Add(this.ValueZ);
			this.Controls.Add(this.ValueX);
			this.Controls.Add(this.SourceValueZButton);
			this.Controls.Add(this.ZeroValueZButton);
			this.Controls.Add(this.SourceValueYButton);
			this.Controls.Add(this.ZeroValueYButton);
			this.Controls.Add(this.SourceValueXButton);
			this.Controls.Add(this.ZeroValueXButton);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.scale);
			this.Controls.Add(this.Translate);
			this.Controls.Add(this.Rotate);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TransformDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = Local.LocalString("transform");
			((System.ComponentModel.ISupportInitialize)(this.ValueY)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ValueZ)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.ValueX)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton Rotate;
        private System.Windows.Forms.RadioButton Translate;
        private System.Windows.Forms.RadioButton scale;
        private System.Windows.Forms.NumericUpDown ValueY;
        private System.Windows.Forms.NumericUpDown ValueZ;
        private System.Windows.Forms.NumericUpDown ValueX;
        private System.Windows.Forms.Button SourceValueZButton;
        private System.Windows.Forms.Button ZeroValueZButton;
        private System.Windows.Forms.Button SourceValueYButton;
        private System.Windows.Forms.Button ZeroValueYButton;
        private System.Windows.Forms.Button SourceValueXButton;
        private System.Windows.Forms.Button ZeroValueXButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button OkButton;
    }
}