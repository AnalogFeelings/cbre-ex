namespace CBRE.BspEditor.Environment.Controls
{
    partial class EnvironmentCollectionEditor
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
            this.components = new System.ComponentModel.Container();
            this.treEnvironments = new System.Windows.Forms.TreeView();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new CBRE.Shell.Controls.DropdownButton();
            this.ctxEnvironmentMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.noEnvironmentsFoundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nameBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.pnlSettings = new System.Windows.Forms.Panel();
            this.ctxEnvironmentMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // treEnvironments
            // 
            this.treEnvironments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treEnvironments.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.treEnvironments.HideSelection = false;
            this.treEnvironments.Location = new System.Drawing.Point(3, 3);
            this.treEnvironments.Name = "treEnvironments";
            this.treEnvironments.Size = new System.Drawing.Size(213, 291);
            this.treEnvironments.TabIndex = 0;
            this.treEnvironments.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.EnvironmentSelected);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemove.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemove.Location = new System.Drawing.Point(141, 300);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 23);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.RemoveEnvironment);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.Location = new System.Drawing.Point(4, 300);
            this.btnAdd.Menu = this.ctxEnvironmentMenu;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            // 
            // ctxEnvironmentMenu
            // 
            this.ctxEnvironmentMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.noEnvironmentsFoundToolStripMenuItem});
            this.ctxEnvironmentMenu.Name = "ctxEnvironmentMenu";
            this.ctxEnvironmentMenu.Size = new System.Drawing.Size(205, 26);
            // 
            // noEnvironmentsFoundToolStripMenuItem
            // 
            this.noEnvironmentsFoundToolStripMenuItem.Enabled = false;
            this.noEnvironmentsFoundToolStripMenuItem.Name = "noEnvironmentsFoundToolStripMenuItem";
            this.noEnvironmentsFoundToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.noEnvironmentsFoundToolStripMenuItem.Text = "No environments found!";
            // 
            // nameBox
            // 
            this.nameBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameBox.Location = new System.Drawing.Point(297, 3);
            this.nameBox.Name = "nameBox";
            this.nameBox.Size = new System.Drawing.Size(375, 20);
            this.nameBox.TabIndex = 4;
            this.nameBox.TextChanged += new System.EventHandler(this.UpdateEnvironment);
            // 
            // nameLabel
            // 
            this.nameLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.Location = new System.Drawing.Point(222, 3);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(69, 20);
            this.nameLabel.TabIndex = 5;
            this.nameLabel.Text = "Name";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSettings
            // 
            this.pnlSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSettings.AutoScroll = true;
            this.pnlSettings.Location = new System.Drawing.Point(222, 29);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Size = new System.Drawing.Size(450, 294);
            this.pnlSettings.TabIndex = 6;
            // 
            // EnvironmentCollectionEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlSettings);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.treEnvironments);
            this.Name = "EnvironmentCollectionEditor";
            this.Size = new System.Drawing.Size(675, 326);
            this.ctxEnvironmentMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treEnvironments;
        private System.Windows.Forms.Button btnRemove;
        private Shell.Controls.DropdownButton btnAdd;
        private System.Windows.Forms.ContextMenuStrip ctxEnvironmentMenu;
        private System.Windows.Forms.ToolStripMenuItem noEnvironmentsFoundToolStripMenuItem;
        private System.Windows.Forms.TextBox nameBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Panel pnlSettings;
    }
}
