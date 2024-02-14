using CBRE.Localization;
using System.Text.RegularExpressions;

namespace CBRE.Editor.Tools.VMTool
{
    partial class EditFaceControl
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
			this.BevelButton = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.BevelValue = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.PokeFaceButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.PokeFaceCount = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)(this.BevelValue)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PokeFaceCount)).BeginInit();
			this.SuspendLayout();
			// 
			// BevelButton
			// 
			this.BevelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.BevelButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BevelButton.Location = new System.Drawing.Point(158, 50);
			this.BevelButton.Name = "BevelButton";
			this.BevelButton.Size = new System.Drawing.Size(50, 23);
			this.BevelButton.TabIndex = 9;
			this.BevelButton.Text = Local.LocalString("bevel");
			this.BevelButton.UseVisualStyleBackColor = true;
			this.BevelButton.Click += new System.EventHandler(this.BevelButtonClicked);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(4, 4);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(111, 15);
			this.label5.TabIndex = 3;
			this.label5.Text = Local.LocalString("tool.with_selected_faces");
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// BevelValue
			// 
			this.BevelValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BevelValue.Location = new System.Drawing.Point(55, 50);
			this.BevelValue.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.BevelValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.BevelValue.Name = "BevelValue";
			this.BevelValue.Size = new System.Drawing.Size(58, 23);
			this.BevelValue.TabIndex = 11;
			this.BevelValue.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(119, 54);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(33, 15);
			this.label7.TabIndex = 6;
			this.label7.Text = Regex.Split(Local.LocalString("tool.bevel_by"), "{.}")[1].Trim();
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(-2, 52);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(51, 15);
			this.label6.TabIndex = 7;
			this.label6.Text = Regex.Split(Local.LocalString("tool.bevel_by"), "{.}")[0].Trim();
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// PokeFaceButton
			// 
			this.PokeFaceButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.PokeFaceButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PokeFaceButton.Location = new System.Drawing.Point(158, 21);
			this.PokeFaceButton.Name = "PokeFaceButton";
			this.PokeFaceButton.Size = new System.Drawing.Size(50, 23);
			this.PokeFaceButton.TabIndex = 9;
			this.PokeFaceButton.Text = Local.LocalString("poke");
			this.PokeFaceButton.UseVisualStyleBackColor = true;
			this.PokeFaceButton.Click += new System.EventHandler(this.PokeFaceButtonClicked);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(0, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(49, 15);
			this.label1.TabIndex = 7;
			this.label1.Text = Regex.Split(Local.LocalString("tool.poke_by"), "{.}")[0].Trim();
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(119, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(33, 15);
			this.label2.TabIndex = 6;
			this.label2.Text = Regex.Split(Local.LocalString("tool.bevel_by"), "{.}")[1].Trim();
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// PokeFaceCount
			// 
			this.PokeFaceCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PokeFaceCount.Location = new System.Drawing.Point(55, 23);
			this.PokeFaceCount.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.PokeFaceCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.PokeFaceCount.Name = "PokeFaceCount";
			this.PokeFaceCount.Size = new System.Drawing.Size(58, 23);
			this.PokeFaceCount.TabIndex = 11;
			this.PokeFaceCount.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
			// 
			// EditFaceControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.PokeFaceButton);
			this.Controls.Add(this.BevelButton);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.PokeFaceCount);
			this.Controls.Add(this.BevelValue);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.label6);
			this.Name = "EditFaceControl";
			this.Size = new System.Drawing.Size(211, 78);
			((System.ComponentModel.ISupportInitialize)(this.BevelValue)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PokeFaceCount)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BevelButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown BevelValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button PokeFaceButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown PokeFaceCount;
    }
}
