namespace CBRE.BspEditor.Environment.ContainmentBreach
{
    partial class BlitzEnvironmentEditor
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
            this.lblBaseGame = new System.Windows.Forms.Label();
            this.cmbBaseGame = new System.Windows.Forms.ComboBox();
            this.txtGameDir = new System.Windows.Forms.TextBox();
            this.lblGameDir = new System.Windows.Forms.Label();
            this.btnGameDirBrowse = new System.Windows.Forms.Button();
            this.lstFgds = new System.Windows.Forms.ListView();
            this.colFgdName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFgdPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnAddFgd = new System.Windows.Forms.Button();
            this.lblDefaultBrushEntity = new System.Windows.Forms.Label();
            this.btnRemoveFgd = new System.Windows.Forms.Button();
            this.lblDefaultPointEntity = new System.Windows.Forms.Label();
            this.cmbDefaultBrushEntity = new System.Windows.Forms.ComboBox();
            this.cmbDefaultPointEntity = new System.Windows.Forms.ComboBox();
            this.nudDefaultTextureScale = new System.Windows.Forms.NumericUpDown();
            this.lblDefaultTextureScale = new System.Windows.Forms.Label();
            this.cmbMapSizeOverrideHigh = new System.Windows.Forms.ComboBox();
            this.lblMapSizeOverrideHigh = new System.Windows.Forms.Label();
            this.cmbMapSizeOverrideLow = new System.Windows.Forms.ComboBox();
            this.chkOverrideMapSize = new System.Windows.Forms.CheckBox();
            this.lblMapSizeOverrideLow = new System.Windows.Forms.Label();
            this.grpDirectories = new System.Windows.Forms.GroupBox();
            this.grpFgds = new System.Windows.Forms.GroupBox();
            this.grpTextures = new System.Windows.Forms.GroupBox();
            this.lstAdditionalTextures = new System.Windows.Forms.ListView();
            this.colWadName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colWadPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnRemoveTextures = new System.Windows.Forms.Button();
            this.btnAddTextures = new System.Windows.Forms.Button();
            this.lblAdditionalTexturePackages = new System.Windows.Forms.Label();
            this.lblTexturePackageExclusions = new System.Windows.Forms.Label();
            this.cklTexturePackages = new System.Windows.Forms.CheckedListBox();
            this.chkToggleAllTextures = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudDefaultTextureScale)).BeginInit();
            this.grpDirectories.SuspendLayout();
            this.grpFgds.SuspendLayout();
            this.grpTextures.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblBaseGame
            // 
            this.lblBaseGame.Location = new System.Drawing.Point(2, 41);
            this.lblBaseGame.Name = "lblBaseGame";
            this.lblBaseGame.Size = new System.Drawing.Size(198, 20);
            this.lblBaseGame.TabIndex = 20;
            this.lblBaseGame.Text = "Base Game Directory";
            this.lblBaseGame.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbBaseGame
            // 
            this.cmbBaseGame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBaseGame.FormattingEnabled = true;
            this.cmbBaseGame.Items.AddRange(new object[] {
            "(Steam only) Half-Life",
            "Counter-Strike"});
            this.cmbBaseGame.Location = new System.Drawing.Point(210, 42);
            this.cmbBaseGame.Name = "cmbBaseGame";
            this.cmbBaseGame.Size = new System.Drawing.Size(153, 21);
            this.cmbBaseGame.TabIndex = 21;
            this.cmbBaseGame.SelectedIndexChanged += new System.EventHandler(this.BaseGameDirectoryChanged);
            // 
            // txtGameDir
            // 
            this.txtGameDir.Location = new System.Drawing.Point(107, 16);
            this.txtGameDir.Name = "txtGameDir";
            this.txtGameDir.Size = new System.Drawing.Size(256, 20);
            this.txtGameDir.TabIndex = 13;
            this.txtGameDir.Text = "example: C:\\Sierra\\Half-Life";
            this.txtGameDir.TextChanged += new System.EventHandler(this.GameDirectoryTextChanged);
            // 
            // lblGameDir
            // 
            this.lblGameDir.Location = new System.Drawing.Point(6, 16);
            this.lblGameDir.Name = "lblGameDir";
            this.lblGameDir.Size = new System.Drawing.Size(95, 20);
            this.lblGameDir.TabIndex = 14;
            this.lblGameDir.Text = "Game Dir";
            this.lblGameDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnGameDirBrowse
            // 
            this.btnGameDirBrowse.Location = new System.Drawing.Point(369, 16);
            this.btnGameDirBrowse.Name = "btnGameDirBrowse";
            this.btnGameDirBrowse.Size = new System.Drawing.Size(74, 20);
            this.btnGameDirBrowse.TabIndex = 15;
            this.btnGameDirBrowse.Text = "Browse...";
            this.btnGameDirBrowse.UseVisualStyleBackColor = true;
            this.btnGameDirBrowse.Click += new System.EventHandler(this.BrowseGameDirectory);
            // 
            // lstFgds
            // 
            this.lstFgds.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colFgdName,
            this.colFgdPath});
            this.lstFgds.FullRowSelect = true;
            this.lstFgds.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstFgds.HideSelection = false;
            this.lstFgds.Location = new System.Drawing.Point(6, 19);
            this.lstFgds.Name = "lstFgds";
            this.lstFgds.ShowItemToolTips = true;
            this.lstFgds.Size = new System.Drawing.Size(357, 115);
            this.lstFgds.TabIndex = 34;
            this.lstFgds.UseCompatibleStateImageBehavior = false;
            this.lstFgds.View = System.Windows.Forms.View.Details;
            // 
            // colFgdName
            // 
            this.colFgdName.Text = "Name";
            // 
            // colFgdPath
            // 
            this.colFgdPath.Text = "Path";
            // 
            // btnAddFgd
            // 
            this.btnAddFgd.Location = new System.Drawing.Point(369, 19);
            this.btnAddFgd.Name = "btnAddFgd";
            this.btnAddFgd.Size = new System.Drawing.Size(74, 23);
            this.btnAddFgd.TabIndex = 27;
            this.btnAddFgd.Text = "Add...";
            this.btnAddFgd.UseVisualStyleBackColor = true;
            this.btnAddFgd.Click += new System.EventHandler(this.BrowseFgd);
            // 
            // lblDefaultBrushEntity
            // 
            this.lblDefaultBrushEntity.Location = new System.Drawing.Point(7, 168);
            this.lblDefaultBrushEntity.Name = "lblDefaultBrushEntity";
            this.lblDefaultBrushEntity.Size = new System.Drawing.Size(151, 20);
            this.lblDefaultBrushEntity.TabIndex = 30;
            this.lblDefaultBrushEntity.Text = "Default Brush Entity";
            this.lblDefaultBrushEntity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnRemoveFgd
            // 
            this.btnRemoveFgd.Location = new System.Drawing.Point(369, 48);
            this.btnRemoveFgd.Name = "btnRemoveFgd";
            this.btnRemoveFgd.Size = new System.Drawing.Size(74, 23);
            this.btnRemoveFgd.TabIndex = 28;
            this.btnRemoveFgd.Text = "Remove";
            this.btnRemoveFgd.UseVisualStyleBackColor = true;
            this.btnRemoveFgd.Click += new System.EventHandler(this.RemoveFgd);
            // 
            // lblDefaultPointEntity
            // 
            this.lblDefaultPointEntity.Location = new System.Drawing.Point(7, 141);
            this.lblDefaultPointEntity.Name = "lblDefaultPointEntity";
            this.lblDefaultPointEntity.Size = new System.Drawing.Size(151, 20);
            this.lblDefaultPointEntity.TabIndex = 31;
            this.lblDefaultPointEntity.Text = "Default Point Entity";
            this.lblDefaultPointEntity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbDefaultBrushEntity
            // 
            this.cmbDefaultBrushEntity.DropDownHeight = 300;
            this.cmbDefaultBrushEntity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefaultBrushEntity.FormattingEnabled = true;
            this.cmbDefaultBrushEntity.IntegralHeight = false;
            this.cmbDefaultBrushEntity.Items.AddRange(new object[] {
            "Valve"});
            this.cmbDefaultBrushEntity.Location = new System.Drawing.Point(164, 167);
            this.cmbDefaultBrushEntity.Name = "cmbDefaultBrushEntity";
            this.cmbDefaultBrushEntity.Size = new System.Drawing.Size(199, 21);
            this.cmbDefaultBrushEntity.TabIndex = 32;
            // 
            // cmbDefaultPointEntity
            // 
            this.cmbDefaultPointEntity.DropDownHeight = 300;
            this.cmbDefaultPointEntity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDefaultPointEntity.FormattingEnabled = true;
            this.cmbDefaultPointEntity.IntegralHeight = false;
            this.cmbDefaultPointEntity.Items.AddRange(new object[] {
            "Valve"});
            this.cmbDefaultPointEntity.Location = new System.Drawing.Point(164, 140);
            this.cmbDefaultPointEntity.Name = "cmbDefaultPointEntity";
            this.cmbDefaultPointEntity.Size = new System.Drawing.Size(199, 21);
            this.cmbDefaultPointEntity.TabIndex = 33;
            // 
            // nudDefaultTextureScale
            // 
            this.nudDefaultTextureScale.DecimalPlaces = 2;
            this.nudDefaultTextureScale.Increment = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            this.nudDefaultTextureScale.Location = new System.Drawing.Point(198, 16);
            this.nudDefaultTextureScale.Name = "nudDefaultTextureScale";
            this.nudDefaultTextureScale.Size = new System.Drawing.Size(51, 20);
            this.nudDefaultTextureScale.TabIndex = 38;
            this.nudDefaultTextureScale.Value = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            // 
            // lblDefaultTextureScale
            // 
            this.lblDefaultTextureScale.Location = new System.Drawing.Point(6, 16);
            this.lblDefaultTextureScale.Name = "lblDefaultTextureScale";
            this.lblDefaultTextureScale.Size = new System.Drawing.Size(186, 20);
            this.lblDefaultTextureScale.TabIndex = 36;
            this.lblDefaultTextureScale.Text = "Default Texture Scale";
            this.lblDefaultTextureScale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbMapSizeOverrideHigh
            // 
            this.cmbMapSizeOverrideHigh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMapSizeOverrideHigh.FormattingEnabled = true;
            this.cmbMapSizeOverrideHigh.Items.AddRange(new object[] {
            "4096",
            "8192",
            "16384",
            "32768",
            "65536",
            "131072"});
            this.cmbMapSizeOverrideHigh.Location = new System.Drawing.Point(75, 275);
            this.cmbMapSizeOverrideHigh.Name = "cmbMapSizeOverrideHigh";
            this.cmbMapSizeOverrideHigh.Size = new System.Drawing.Size(57, 21);
            this.cmbMapSizeOverrideHigh.TabIndex = 44;
            // 
            // lblMapSizeOverrideHigh
            // 
            this.lblMapSizeOverrideHigh.Location = new System.Drawing.Point(5, 274);
            this.lblMapSizeOverrideHigh.Name = "lblMapSizeOverrideHigh";
            this.lblMapSizeOverrideHigh.Size = new System.Drawing.Size(64, 20);
            this.lblMapSizeOverrideHigh.TabIndex = 43;
            this.lblMapSizeOverrideHigh.Text = "High";
            this.lblMapSizeOverrideHigh.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmbMapSizeOverrideLow
            // 
            this.cmbMapSizeOverrideLow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMapSizeOverrideLow.FormattingEnabled = true;
            this.cmbMapSizeOverrideLow.Items.AddRange(new object[] {
            "-4096",
            "-8192",
            "-16384",
            "-32768",
            "-65536",
            "-131072"});
            this.cmbMapSizeOverrideLow.Location = new System.Drawing.Point(75, 248);
            this.cmbMapSizeOverrideLow.Name = "cmbMapSizeOverrideLow";
            this.cmbMapSizeOverrideLow.Size = new System.Drawing.Size(57, 21);
            this.cmbMapSizeOverrideLow.TabIndex = 42;
            // 
            // chkOverrideMapSize
            // 
            this.chkOverrideMapSize.Checked = true;
            this.chkOverrideMapSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOverrideMapSize.Location = new System.Drawing.Point(6, 221);
            this.chkOverrideMapSize.Name = "chkOverrideMapSize";
            this.chkOverrideMapSize.Size = new System.Drawing.Size(196, 24);
            this.chkOverrideMapSize.TabIndex = 41;
            this.chkOverrideMapSize.Text = "Override FGD map size";
            this.chkOverrideMapSize.UseVisualStyleBackColor = true;
            // 
            // lblMapSizeOverrideLow
            // 
            this.lblMapSizeOverrideLow.Location = new System.Drawing.Point(6, 249);
            this.lblMapSizeOverrideLow.Name = "lblMapSizeOverrideLow";
            this.lblMapSizeOverrideLow.Size = new System.Drawing.Size(63, 20);
            this.lblMapSizeOverrideLow.TabIndex = 40;
            this.lblMapSizeOverrideLow.Text = "Low";
            this.lblMapSizeOverrideLow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpDirectories
            // 
            this.grpDirectories.Controls.Add(this.lblGameDir);
            this.grpDirectories.Controls.Add(this.btnGameDirBrowse);
            this.grpDirectories.Controls.Add(this.txtGameDir);
            this.grpDirectories.Controls.Add(this.cmbBaseGame);
            this.grpDirectories.Controls.Add(this.lblBaseGame);
            this.grpDirectories.Location = new System.Drawing.Point(6, 3);
            this.grpDirectories.Name = "grpDirectories";
            this.grpDirectories.Size = new System.Drawing.Size(459, 75);
            this.grpDirectories.TabIndex = 46;
            this.grpDirectories.TabStop = false;
            this.grpDirectories.Text = "Directories";
            // 
            // grpFgds
            // 
            this.grpFgds.Controls.Add(this.lstFgds);
            this.grpFgds.Controls.Add(this.cmbDefaultPointEntity);
            this.grpFgds.Controls.Add(this.cmbDefaultBrushEntity);
            this.grpFgds.Controls.Add(this.cmbMapSizeOverrideHigh);
            this.grpFgds.Controls.Add(this.lblDefaultPointEntity);
            this.grpFgds.Controls.Add(this.lblMapSizeOverrideHigh);
            this.grpFgds.Controls.Add(this.btnRemoveFgd);
            this.grpFgds.Controls.Add(this.cmbMapSizeOverrideLow);
            this.grpFgds.Controls.Add(this.lblDefaultBrushEntity);
            this.grpFgds.Controls.Add(this.chkOverrideMapSize);
            this.grpFgds.Controls.Add(this.btnAddFgd);
            this.grpFgds.Controls.Add(this.lblMapSizeOverrideLow);
            this.grpFgds.Location = new System.Drawing.Point(6, 84);
            this.grpFgds.Name = "grpFgds";
            this.grpFgds.Size = new System.Drawing.Size(459, 304);
            this.grpFgds.TabIndex = 47;
            this.grpFgds.TabStop = false;
            this.grpFgds.Text = "Game Data Files";
            // 
            // grpTextures
            // 
            this.grpTextures.Controls.Add(this.lstAdditionalTextures);
            this.grpTextures.Controls.Add(this.btnRemoveTextures);
            this.grpTextures.Controls.Add(this.btnAddTextures);
            this.grpTextures.Controls.Add(this.lblAdditionalTexturePackages);
            this.grpTextures.Controls.Add(this.lblTexturePackageExclusions);
            this.grpTextures.Controls.Add(this.cklTexturePackages);
            this.grpTextures.Controls.Add(this.chkToggleAllTextures);
            this.grpTextures.Controls.Add(this.lblDefaultTextureScale);
            this.grpTextures.Controls.Add(this.nudDefaultTextureScale);
            this.grpTextures.Location = new System.Drawing.Point(6, 394);
            this.grpTextures.Name = "grpTextures";
            this.grpTextures.Size = new System.Drawing.Size(459, 407);
            this.grpTextures.TabIndex = 49;
            this.grpTextures.TabStop = false;
            this.grpTextures.Text = "Textures";
            // 
            // lstAdditionalTextures
            // 
            this.lstAdditionalTextures.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colWadName,
            this.colWadPath});
            this.lstAdditionalTextures.FullRowSelect = true;
            this.lstAdditionalTextures.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lstAdditionalTextures.HideSelection = false;
            this.lstAdditionalTextures.Location = new System.Drawing.Point(9, 254);
            this.lstAdditionalTextures.Name = "lstAdditionalTextures";
            this.lstAdditionalTextures.ShowItemToolTips = true;
            this.lstAdditionalTextures.Size = new System.Drawing.Size(364, 147);
            this.lstAdditionalTextures.TabIndex = 45;
            this.lstAdditionalTextures.UseCompatibleStateImageBehavior = false;
            this.lstAdditionalTextures.View = System.Windows.Forms.View.Details;
            // 
            // colWadName
            // 
            this.colWadName.Text = "Name";
            // 
            // colWadPath
            // 
            this.colWadPath.Text = "Path";
            // 
            // btnRemoveTextures
            // 
            this.btnRemoveTextures.Location = new System.Drawing.Point(379, 283);
            this.btnRemoveTextures.Name = "btnRemoveTextures";
            this.btnRemoveTextures.Size = new System.Drawing.Size(74, 23);
            this.btnRemoveTextures.TabIndex = 44;
            this.btnRemoveTextures.Text = "Remove";
            this.btnRemoveTextures.UseVisualStyleBackColor = true;
            this.btnRemoveTextures.Click += new System.EventHandler(this.RemoveWad);
            // 
            // btnAddTextures
            // 
            this.btnAddTextures.Location = new System.Drawing.Point(379, 254);
            this.btnAddTextures.Name = "btnAddTextures";
            this.btnAddTextures.Size = new System.Drawing.Size(74, 23);
            this.btnAddTextures.TabIndex = 43;
            this.btnAddTextures.Text = "Add...";
            this.btnAddTextures.UseVisualStyleBackColor = true;
            this.btnAddTextures.Click += new System.EventHandler(this.BrowseWad);
            // 
            // lblAdditionalTexturePackages
            // 
            this.lblAdditionalTexturePackages.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAdditionalTexturePackages.Location = new System.Drawing.Point(6, 231);
            this.lblAdditionalTexturePackages.Name = "lblAdditionalTexturePackages";
            this.lblAdditionalTexturePackages.Size = new System.Drawing.Size(357, 20);
            this.lblAdditionalTexturePackages.TabIndex = 34;
            this.lblAdditionalTexturePackages.Text = "Additional texture packages:";
            this.lblAdditionalTexturePackages.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTexturePackageExclusions
            // 
            this.lblTexturePackageExclusions.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTexturePackageExclusions.Location = new System.Drawing.Point(12, 36);
            this.lblTexturePackageExclusions.Name = "lblTexturePackageExclusions";
            this.lblTexturePackageExclusions.Size = new System.Drawing.Size(314, 20);
            this.lblTexturePackageExclusions.TabIndex = 34;
            this.lblTexturePackageExclusions.Text = "Texture packages to include:";
            this.lblTexturePackageExclusions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cklTexturePackages
            // 
            this.cklTexturePackages.CheckOnClick = true;
            this.cklTexturePackages.FormattingEnabled = true;
            this.cklTexturePackages.Location = new System.Drawing.Point(9, 59);
            this.cklTexturePackages.Name = "cklTexturePackages";
            this.cklTexturePackages.Size = new System.Drawing.Size(444, 169);
            this.cklTexturePackages.TabIndex = 39;
            // 
            // chkToggleAllTextures
            // 
            this.chkToggleAllTextures.Checked = true;
            this.chkToggleAllTextures.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkToggleAllTextures.Location = new System.Drawing.Point(332, 41);
            this.chkToggleAllTextures.Name = "chkToggleAllTextures";
            this.chkToggleAllTextures.Size = new System.Drawing.Size(121, 18);
            this.chkToggleAllTextures.TabIndex = 42;
            this.chkToggleAllTextures.Text = "Toggle all";
            this.chkToggleAllTextures.UseVisualStyleBackColor = true;
            this.chkToggleAllTextures.CheckedChanged += new System.EventHandler(this.ToggleAllTextures);
            // 
            // ContainmentBreachEnvironmentEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpTextures);
            this.Controls.Add(this.grpFgds);
            this.Controls.Add(this.grpDirectories);
            this.Name = "ContainmentBreachEnvironmentEditor";
            this.Size = new System.Drawing.Size(472, 1202);
            ((System.ComponentModel.ISupportInitialize)(this.nudDefaultTextureScale)).EndInit();
            this.grpDirectories.ResumeLayout(false);
            this.grpDirectories.PerformLayout();
            this.grpFgds.ResumeLayout(false);
            this.grpTextures.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblBaseGame;
        private System.Windows.Forms.ComboBox cmbBaseGame;
        private System.Windows.Forms.TextBox txtGameDir;
        private System.Windows.Forms.Label lblGameDir;
        private System.Windows.Forms.Button btnGameDirBrowse;
        private System.Windows.Forms.ListView lstFgds;
        private System.Windows.Forms.ColumnHeader colFgdName;
        private System.Windows.Forms.ColumnHeader colFgdPath;
        private System.Windows.Forms.Button btnAddFgd;
        private System.Windows.Forms.Label lblDefaultBrushEntity;
        private System.Windows.Forms.Button btnRemoveFgd;
        private System.Windows.Forms.Label lblDefaultPointEntity;
        private System.Windows.Forms.ComboBox cmbDefaultBrushEntity;
        private System.Windows.Forms.ComboBox cmbDefaultPointEntity;
        private System.Windows.Forms.NumericUpDown nudDefaultTextureScale;
        private System.Windows.Forms.Label lblDefaultTextureScale;
        private System.Windows.Forms.ComboBox cmbMapSizeOverrideHigh;
        private System.Windows.Forms.Label lblMapSizeOverrideHigh;
        private System.Windows.Forms.ComboBox cmbMapSizeOverrideLow;
        private System.Windows.Forms.CheckBox chkOverrideMapSize;
        private System.Windows.Forms.Label lblMapSizeOverrideLow;
        private System.Windows.Forms.GroupBox grpDirectories;
        private System.Windows.Forms.GroupBox grpFgds;
        private System.Windows.Forms.GroupBox grpTextures;
        private System.Windows.Forms.CheckedListBox cklTexturePackages;
        private System.Windows.Forms.Label lblTexturePackageExclusions;
        private System.Windows.Forms.CheckBox chkToggleAllTextures;
        private System.Windows.Forms.ListView lstAdditionalTextures;
        private System.Windows.Forms.ColumnHeader colWadName;
        private System.Windows.Forms.ColumnHeader colWadPath;
        private System.Windows.Forms.Button btnRemoveTextures;
        private System.Windows.Forms.Button btnAddTextures;
        private System.Windows.Forms.Label lblAdditionalTexturePackages;
    }
}
