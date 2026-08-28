using CBRE.Localization;

namespace CBRE.Editor.UI
{
    partial class TextureReplaceDialog
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.Find = new System.Windows.Forms.TextBox();
			this.FindInfo = new System.Windows.Forms.Label();
			this.FindImage = new System.Windows.Forms.PictureBox();
			this.FindBrowse = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.Replace = new System.Windows.Forms.TextBox();
			this.ReplaceInfo = new System.Windows.Forms.Label();
			this.ReplaceImage = new System.Windows.Forms.PictureBox();
			this.ReplaceBrowse = new System.Windows.Forms.Button();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.ReplaceEverything = new System.Windows.Forms.RadioButton();
			this.ReplaceVisible = new System.Windows.Forms.RadioButton();
			this.ReplaceSelection = new System.Windows.Forms.RadioButton();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.ActionSelect = new System.Windows.Forms.RadioButton();
			this.ActionSubstitute = new System.Windows.Forms.RadioButton();
			this.ActionPartial = new System.Windows.Forms.RadioButton();
			this.ActionExact = new System.Windows.Forms.RadioButton();
			this.RescaleTextures = new System.Windows.Forms.CheckBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.OKButton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.FindImage)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ReplaceImage)).BeginInit();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.Find);
			this.groupBox1.Controls.Add(this.FindInfo);
			this.groupBox1.Controls.Add(this.FindImage);
			this.groupBox1.Controls.Add(this.FindBrowse);
			this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(193, 152);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = Local.LocalString("find");
			// 
			// Find
			// 
			this.Find.Location = new System.Drawing.Point(6, 17);
			this.Find.Name = "Find";
			this.Find.Size = new System.Drawing.Size(181, 23);
			this.Find.TabIndex = 4;
			// 
			// FindInfo
			// 
			this.FindInfo.AutoSize = true;
			this.FindInfo.Location = new System.Drawing.Point(112, 72);
			this.FindInfo.Name = "FindInfo";
			this.FindInfo.Size = new System.Drawing.Size(69, 15);
			this.FindInfo.TabIndex = 3;
			this.FindInfo.Text = Local.LocalString("texture.info");
			// 
			// FindImage
			// 
			this.FindImage.Location = new System.Drawing.Point(6, 46);
			this.FindImage.Name = "FindImage";
			this.FindImage.Size = new System.Drawing.Size(100, 100);
			this.FindImage.TabIndex = 2;
			this.FindImage.TabStop = false;
			// 
			// FindBrowse
			// 
			this.FindBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.FindBrowse.Location = new System.Drawing.Point(112, 46);
			this.FindBrowse.Name = "FindBrowse";
			this.FindBrowse.Size = new System.Drawing.Size(75, 23);
			this.FindBrowse.TabIndex = 1;
			this.FindBrowse.Text = Local.LocalString("browse");
			this.FindBrowse.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.Replace);
			this.groupBox2.Controls.Add(this.ReplaceInfo);
			this.groupBox2.Controls.Add(this.ReplaceImage);
			this.groupBox2.Controls.Add(this.ReplaceBrowse);
			this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(211, 12);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(193, 152);
			this.groupBox2.TabIndex = 0;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = Local.LocalString("replace");
			// 
			// Replace
			// 
			this.Replace.Location = new System.Drawing.Point(6, 17);
			this.Replace.Name = "Replace";
			this.Replace.Size = new System.Drawing.Size(181, 23);
			this.Replace.TabIndex = 4;
			// 
			// ReplaceInfo
			// 
			this.ReplaceInfo.AutoSize = true;
			this.ReplaceInfo.Location = new System.Drawing.Point(112, 72);
			this.ReplaceInfo.Name = "ReplaceInfo";
			this.ReplaceInfo.Size = new System.Drawing.Size(69, 15);
			this.ReplaceInfo.TabIndex = 3;
			this.ReplaceInfo.Text = Local.LocalString("texture.info");
			// 
			// ReplaceImage
			// 
			this.ReplaceImage.Location = new System.Drawing.Point(6, 46);
			this.ReplaceImage.Name = "ReplaceImage";
			this.ReplaceImage.Size = new System.Drawing.Size(100, 100);
			this.ReplaceImage.TabIndex = 2;
			this.ReplaceImage.TabStop = false;
			// 
			// ReplaceBrowse
			// 
			this.ReplaceBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.ReplaceBrowse.Location = new System.Drawing.Point(112, 46);
			this.ReplaceBrowse.Name = "ReplaceBrowse";
			this.ReplaceBrowse.Size = new System.Drawing.Size(75, 23);
			this.ReplaceBrowse.TabIndex = 1;
			this.ReplaceBrowse.Text = Local.LocalString("browse");
			this.ReplaceBrowse.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.ReplaceEverything);
			this.groupBox3.Controls.Add(this.ReplaceVisible);
			this.groupBox3.Controls.Add(this.ReplaceSelection);
			this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox3.Location = new System.Drawing.Point(12, 170);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(193, 126);
			this.groupBox3.TabIndex = 1;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = Local.LocalString("texture.replace");
			// 
			// ReplaceEverything
			// 
			this.ReplaceEverything.AutoSize = true;
			this.ReplaceEverything.Location = new System.Drawing.Point(6, 72);
			this.ReplaceEverything.Name = "ReplaceEverything";
			this.ReplaceEverything.Size = new System.Drawing.Size(81, 19);
			this.ReplaceEverything.TabIndex = 0;
			this.ReplaceEverything.TabStop = true;
			this.ReplaceEverything.Text = Local.LocalString("texture.replace.everything");
			this.ReplaceEverything.UseVisualStyleBackColor = true;
			// 
			// ReplaceVisible
			// 
			this.ReplaceVisible.AutoSize = true;
			this.ReplaceVisible.Location = new System.Drawing.Point(6, 47);
			this.ReplaceVisible.Name = "ReplaceVisible";
			this.ReplaceVisible.Size = new System.Drawing.Size(116, 19);
			this.ReplaceVisible.TabIndex = 0;
			this.ReplaceVisible.TabStop = true;
			this.ReplaceVisible.Text = Local.LocalString("texture.replace.visible");
			this.ReplaceVisible.UseVisualStyleBackColor = true;
			// 
			// ReplaceSelection
			// 
			this.ReplaceSelection.AutoSize = true;
			this.ReplaceSelection.Location = new System.Drawing.Point(6, 22);
			this.ReplaceSelection.Name = "ReplaceSelection";
			this.ReplaceSelection.Size = new System.Drawing.Size(73, 19);
			this.ReplaceSelection.TabIndex = 0;
			this.ReplaceSelection.TabStop = true;
			this.ReplaceSelection.Text = Local.LocalString("texture.replace.selection");
			this.ReplaceSelection.UseVisualStyleBackColor = true;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.ActionSelect);
			this.groupBox4.Controls.Add(this.ActionSubstitute);
			this.groupBox4.Controls.Add(this.ActionPartial);
			this.groupBox4.Controls.Add(this.ActionExact);
			this.groupBox4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox4.Location = new System.Drawing.Point(211, 170);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(193, 126);
			this.groupBox4.TabIndex = 1;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = Local.LocalString("texture.action");
			// 
			// ActionSelect
			// 
			this.ActionSelect.AutoSize = true;
			this.ActionSelect.Location = new System.Drawing.Point(6, 97);
			this.ActionSelect.Name = "ActionSelect";
			this.ActionSelect.Size = new System.Drawing.Size(184, 19);
			this.ActionSelect.TabIndex = 0;
			this.ActionSelect.TabStop = true;
			this.ActionSelect.Text = Local.LocalString("texture.action.matches");
			this.ActionSelect.UseVisualStyleBackColor = true;
			// 
			// ActionSubstitute
			// 
			this.ActionSubstitute.AutoSize = true;
			this.ActionSubstitute.Location = new System.Drawing.Point(6, 72);
			this.ActionSubstitute.Name = "ActionSubstitute";
			this.ActionSubstitute.Size = new System.Drawing.Size(162, 19);
			this.ActionSubstitute.TabIndex = 0;
			this.ActionSubstitute.TabStop = true;
			this.ActionSubstitute.Text = Local.LocalString("texture.action.substitute");
			this.ActionSubstitute.UseVisualStyleBackColor = true;
			// 
			// ActionPartial
			// 
			this.ActionPartial.AutoSize = true;
			this.ActionPartial.Location = new System.Drawing.Point(6, 47);
			this.ActionPartial.Name = "ActionPartial";
			this.ActionPartial.Size = new System.Drawing.Size(150, 19);
			this.ActionPartial.TabIndex = 0;
			this.ActionPartial.TabStop = true;
			this.ActionPartial.Text = Local.LocalString("texture.action.partial");
			this.ActionPartial.UseVisualStyleBackColor = true;
			// 
			// ActionExact
			// 
			this.ActionExact.AutoSize = true;
			this.ActionExact.Location = new System.Drawing.Point(6, 22);
			this.ActionExact.Name = "ActionExact";
			this.ActionExact.Size = new System.Drawing.Size(145, 19);
			this.ActionExact.TabIndex = 0;
			this.ActionExact.TabStop = true;
			this.ActionExact.Text = Local.LocalString("texture.action.exact");
			this.ActionExact.UseVisualStyleBackColor = true;
			// 
			// RescaleTextures
			// 
			this.RescaleTextures.AutoSize = true;
			this.RescaleTextures.Location = new System.Drawing.Point(12, 302);
			this.RescaleTextures.Name = "RescaleTextures";
			this.RescaleTextures.Size = new System.Drawing.Size(158, 17);
			this.RescaleTextures.TabIndex = 2;
			this.RescaleTextures.Text = Local.LocalString("texture.rescale");
			this.RescaleTextures.UseVisualStyleBackColor = true;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new System.Drawing.Point(331, 302);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = Local.LocalString("cancel");
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// OKButton
			// 
			this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OKButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.OKButton.Location = new System.Drawing.Point(250, 302);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(75, 23);
			this.OKButton.TabIndex = 3;
			this.OKButton.Text = Local.LocalString("ok");
			this.OKButton.UseVisualStyleBackColor = true;
			// 
			// TextureReplaceDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(418, 337);
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.RescaleTextures);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TextureReplaceDialog";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = Local.LocalString("texture");
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.FindImage)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ReplaceImage)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button FindBrowse;
        private System.Windows.Forms.PictureBox FindImage;
        private System.Windows.Forms.Label FindInfo;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label ReplaceInfo;
        private System.Windows.Forms.PictureBox ReplaceImage;
        private System.Windows.Forms.Button ReplaceBrowse;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton ReplaceEverything;
        private System.Windows.Forms.RadioButton ReplaceVisible;
        private System.Windows.Forms.RadioButton ReplaceSelection;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton ActionSubstitute;
        private System.Windows.Forms.RadioButton ActionPartial;
        private System.Windows.Forms.RadioButton ActionExact;
        private System.Windows.Forms.CheckBox RescaleTextures;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.TextBox Find;
        private System.Windows.Forms.TextBox Replace;
        private System.Windows.Forms.RadioButton ActionSelect;
    }
}