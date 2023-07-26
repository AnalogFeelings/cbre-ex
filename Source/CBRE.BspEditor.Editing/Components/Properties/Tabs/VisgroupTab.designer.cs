using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBRE.BspEditor.Editing.Components.Properties.Tabs
{
    public sealed partial class VisgroupTab
    {
        private void InitializeComponent()
        {
            this.btnEditVisgroups = new System.Windows.Forms.Button();
            this.lblMemberOfGroup = new System.Windows.Forms.Label();
            this.visgroupPanel = new CBRE.BspEditor.Editing.Components.Visgroup.VisgroupPanel();
            this.SuspendLayout();
            // 
            // btnEditVisgroups
            // 
            this.btnEditVisgroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditVisgroups.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditVisgroups.Location = new System.Drawing.Point(476, 439);
            this.btnEditVisgroups.Margin = new System.Windows.Forms.Padding(3, 3, 6, 6);
            this.btnEditVisgroups.Name = "btnEditVisgroups";
            this.btnEditVisgroups.Size = new System.Drawing.Size(105, 23);
            this.btnEditVisgroups.TabIndex = 5;
            this.btnEditVisgroups.Text = "Edit Visgroups";
            this.btnEditVisgroups.UseVisualStyleBackColor = true;
            this.btnEditVisgroups.Click += new System.EventHandler(this.EditVisgroupsClicked);
            // 
            // lblMemberOfGroup
            // 
            this.lblMemberOfGroup.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMemberOfGroup.Location = new System.Drawing.Point(6, 6);
            this.lblMemberOfGroup.Margin = new System.Windows.Forms.Padding(6, 6, 3, 0);
            this.lblMemberOfGroup.Name = "lblMemberOfGroup";
            this.lblMemberOfGroup.Size = new System.Drawing.Size(170, 20);
            this.lblMemberOfGroup.TabIndex = 4;
            this.lblMemberOfGroup.Text = "Member of group:";
            this.lblMemberOfGroup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // visgroupPanel
            // 
            this.visgroupPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.visgroupPanel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.visgroupPanel.Location = new System.Drawing.Point(6, 29);
            this.visgroupPanel.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            this.visgroupPanel.Name = "visgroupPanel";
            this.visgroupPanel.SelectedVisgroup = null;
            this.visgroupPanel.ShowCheckboxes = true;
            this.visgroupPanel.Size = new System.Drawing.Size(575, 404);
            this.visgroupPanel.TabIndex = 6;
            this.visgroupPanel.VisgroupToggled += new CBRE.BspEditor.Editing.Components.Visgroup.VisgroupPanel.VisgroupToggledEventHandler(this.VisgroupToggled);
            // 
            // VisgroupTab
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.visgroupPanel);
            this.Controls.Add(this.btnEditVisgroups);
            this.Controls.Add(this.lblMemberOfGroup);
            this.Name = "VisgroupTab";
            this.Size = new System.Drawing.Size(587, 468);
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Button btnEditVisgroups;
        private System.Windows.Forms.Label lblMemberOfGroup;
        private Visgroup.VisgroupPanel visgroupPanel;
    }
}
