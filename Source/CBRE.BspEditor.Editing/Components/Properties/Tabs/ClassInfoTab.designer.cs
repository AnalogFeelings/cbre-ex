using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBRE.BspEditor.Editing.Components.Properties.Tabs
{
    public partial class ClassInfoTab
    {
        private void InitializeComponent()
        {
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.pnlSmartEdit = new System.Windows.Forms.Panel();
            this.btnSmartEdit = new System.Windows.Forms.CheckBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.txtHelp = new System.Windows.Forms.TextBox();
            this.lstKeyValues = new System.Windows.Forms.ListView();
            this.colPropertyName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colPropertyValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cmbClass = new System.Windows.Forms.ComboBox();
            this.lblHelp = new System.Windows.Forms.Label();
            this.lblKeyValues = new System.Windows.Forms.Label();
            this.lblClass = new System.Windows.Forms.Label();
            this.angAngles = new CBRE.BspEditor.Editing.Controls.AngleControl();
            this.SuspendLayout();
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(74, 355);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(62, 23);
            this.btnDelete.TabIndex = 24;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.DeleteKeyClicked);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(6, 355);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(6, 3, 3, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(62, 23);
            this.btnAdd.TabIndex = 25;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.AddKeyClicked);
            // 
            // pnlSmartEdit
            // 
            this.pnlSmartEdit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSmartEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlSmartEdit.Location = new System.Drawing.Point(399, 75);
            this.pnlSmartEdit.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.pnlSmartEdit.Name = "pnlSmartEdit";
            this.pnlSmartEdit.Size = new System.Drawing.Size(274, 126);
            this.pnlSmartEdit.TabIndex = 23;
            // 
            // btnSmartEdit
            // 
            this.btnSmartEdit.AutoSize = true;
            this.btnSmartEdit.Checked = true;
            this.btnSmartEdit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.btnSmartEdit.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSmartEdit.Location = new System.Drawing.Point(210, 38);
            this.btnSmartEdit.Name = "btnSmartEdit";
            this.btnSmartEdit.Size = new System.Drawing.Size(80, 19);
            this.btnSmartEdit.TabIndex = 22;
            this.btnSmartEdit.Text = "Smart Edit";
            this.btnSmartEdit.UseVisualStyleBackColor = true;
            this.btnSmartEdit.CheckedChanged += new System.EventHandler(this.SmartEditToggled);
            // 
            // btnPaste
            // 
            this.btnPaste.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPaste.Location = new System.Drawing.Point(142, 35);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(62, 23);
            this.btnPaste.TabIndex = 20;
            this.btnPaste.Text = "Paste";
            this.btnPaste.UseVisualStyleBackColor = true;
            // 
            // btnCopy
            // 
            this.btnCopy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopy.Location = new System.Drawing.Point(74, 35);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(62, 23);
            this.btnCopy.TabIndex = 21;
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            // 
            // txtHelp
            // 
            this.txtHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHelp.BackColor = System.Drawing.SystemColors.Window;
            this.txtHelp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHelp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHelp.Location = new System.Drawing.Point(399, 228);
            this.txtHelp.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.txtHelp.Multiline = true;
            this.txtHelp.Name = "txtHelp";
            this.txtHelp.ReadOnly = true;
            this.txtHelp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHelp.Size = new System.Drawing.Size(274, 121);
            this.txtHelp.TabIndex = 19;
            // 
            // lstKeyValues
            // 
            this.lstKeyValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstKeyValues.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colPropertyName,
            this.colPropertyValue});
            this.lstKeyValues.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstKeyValues.FullRowSelect = true;
            this.lstKeyValues.GridLines = true;
            this.lstKeyValues.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstKeyValues.HideSelection = false;
            this.lstKeyValues.Location = new System.Drawing.Point(6, 64);
            this.lstKeyValues.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.lstKeyValues.MultiSelect = false;
            this.lstKeyValues.Name = "lstKeyValues";
            this.lstKeyValues.Size = new System.Drawing.Size(387, 285);
            this.lstKeyValues.TabIndex = 16;
            this.lstKeyValues.UseCompatibleStateImageBehavior = false;
            this.lstKeyValues.View = System.Windows.Forms.View.Details;
            this.lstKeyValues.SelectedIndexChanged += new System.EventHandler(this.SelectedPropertyChanged);
            // 
            // colPropertyName
            // 
            this.colPropertyName.Text = "Property Name";
            this.colPropertyName.Width = 162;
            // 
            // colPropertyValue
            // 
            this.colPropertyValue.Text = "Value";
            this.colPropertyValue.Width = 201;
            // 
            // cmbClass
            // 
            this.cmbClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbClass.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbClass.FormattingEnabled = true;
            this.cmbClass.Location = new System.Drawing.Point(74, 6);
            this.cmbClass.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            this.cmbClass.Name = "cmbClass";
            this.cmbClass.Size = new System.Drawing.Size(322, 23);
            this.cmbClass.TabIndex = 15;
            this.cmbClass.TextChanged += new System.EventHandler(this.ClassChanged);
            // 
            // lblHelp
            // 
            this.lblHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHelp.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHelp.Location = new System.Drawing.Point(399, 204);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Size = new System.Drawing.Size(45, 21);
            this.lblHelp.TabIndex = 12;
            this.lblHelp.Text = "Help:";
            this.lblHelp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblKeyValues
            // 
            this.lblKeyValues.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblKeyValues.Location = new System.Drawing.Point(6, 35);
            this.lblKeyValues.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.lblKeyValues.Name = "lblKeyValues";
            this.lblKeyValues.Size = new System.Drawing.Size(62, 23);
            this.lblKeyValues.TabIndex = 13;
            this.lblKeyValues.Text = "Keyvalues:";
            this.lblKeyValues.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblClass
            // 
            this.lblClass.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClass.Location = new System.Drawing.Point(6, 6);
            this.lblClass.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this.lblClass.Name = "lblClass";
            this.lblClass.Size = new System.Drawing.Size(45, 21);
            this.lblClass.TabIndex = 14;
            this.lblClass.Text = "Class:";
            this.lblClass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // angAngles
            // 
            this.angAngles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.angAngles.Angle = -1;
            this.angAngles.AnglePropertyString = "-90 0 0";
            this.angAngles.Down = false;
            this.angAngles.LabelText = "Angles:";
            this.angAngles.Location = new System.Drawing.Point(554, 6);
            this.angAngles.Margin = new System.Windows.Forms.Padding(3, 6, 6, 3);
            this.angAngles.Name = "angAngles";
            this.angAngles.ShowLabel = true;
            this.angAngles.ShowTextBox = true;
            this.angAngles.Size = new System.Drawing.Size(119, 63);
            this.angAngles.TabIndex = 26;
            this.angAngles.Up = true;
            this.angAngles.AngleChangedEvent += new System.EventHandler(this.SetAngleValue);
            // 
            // ClassInfoTab
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.angAngles);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.pnlSmartEdit);
            this.Controls.Add(this.btnSmartEdit);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.txtHelp);
            this.Controls.Add(this.lstKeyValues);
            this.Controls.Add(this.cmbClass);
            this.Controls.Add(this.lblHelp);
            this.Controls.Add(this.lblKeyValues);
            this.Controls.Add(this.lblClass);
            this.Name = "ClassInfoTab";
            this.Size = new System.Drawing.Size(679, 384);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel pnlSmartEdit;
        private System.Windows.Forms.CheckBox btnSmartEdit;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.TextBox txtHelp;
        private System.Windows.Forms.ListView lstKeyValues;
        private System.Windows.Forms.ColumnHeader colPropertyName;
        private System.Windows.Forms.ColumnHeader colPropertyValue;
        private System.Windows.Forms.ComboBox cmbClass;
        private System.Windows.Forms.Label lblHelp;
        private System.Windows.Forms.Label lblKeyValues;
        private System.Windows.Forms.Label lblClass;
        private Controls.AngleControl angAngles;
    }
}
