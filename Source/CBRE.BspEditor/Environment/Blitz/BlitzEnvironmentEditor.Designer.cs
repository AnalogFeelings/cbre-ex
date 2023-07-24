namespace CBRE.BspEditor.Environment.Blitz
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
            this.lblDefaultBrushEntity = new System.Windows.Forms.Label();
            this.lblDefaultPointEntity = new System.Windows.Forms.Label();
            this.cmbDefaultBrushEntity = new System.Windows.Forms.ComboBox();
            this.cmbDefaultPointEntity = new System.Windows.Forms.ComboBox();
            this.nudDefaultTextureScale = new System.Windows.Forms.NumericUpDown();
            this.lblDefaultTextureScale = new System.Windows.Forms.Label();
            this.grpFgds = new System.Windows.Forms.GroupBox();
            this.grpDirectories = new System.Windows.Forms.GroupBox();
            this.texturesGrid = new System.Windows.Forms.DataGridView();
            this.deleteButtons = new System.Windows.Forms.DataGridViewButtonColumn();
            this.textureDirs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.browseButtons = new System.Windows.Forms.DataGridViewButtonColumn();
            this.modelsGrid = new System.Windows.Forms.DataGridView();
            this.deleteButtonsModel = new System.Windows.Forms.DataGridViewButtonColumn();
            this.modelDirs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.browseButtonsModel = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.nudDefaultTextureScale)).BeginInit();
            this.grpFgds.SuspendLayout();
            this.grpDirectories.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.texturesGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDefaultBrushEntity
            // 
            this.lblDefaultBrushEntity.Location = new System.Drawing.Point(5, 47);
            this.lblDefaultBrushEntity.Name = "lblDefaultBrushEntity";
            this.lblDefaultBrushEntity.Size = new System.Drawing.Size(151, 20);
            this.lblDefaultBrushEntity.TabIndex = 30;
            this.lblDefaultBrushEntity.Text = "Default Brush Entity";
            this.lblDefaultBrushEntity.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblDefaultPointEntity
            // 
            this.lblDefaultPointEntity.Location = new System.Drawing.Point(5, 19);
            this.lblDefaultPointEntity.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
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
            this.cmbDefaultBrushEntity.Location = new System.Drawing.Point(162, 47);
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
            this.cmbDefaultPointEntity.Location = new System.Drawing.Point(162, 20);
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
            this.nudDefaultTextureScale.Location = new System.Drawing.Point(162, 74);
            this.nudDefaultTextureScale.Name = "nudDefaultTextureScale";
            this.nudDefaultTextureScale.Size = new System.Drawing.Size(199, 20);
            this.nudDefaultTextureScale.TabIndex = 38;
            this.nudDefaultTextureScale.Value = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            // 
            // lblDefaultTextureScale
            // 
            this.lblDefaultTextureScale.Location = new System.Drawing.Point(10, 74);
            this.lblDefaultTextureScale.Name = "lblDefaultTextureScale";
            this.lblDefaultTextureScale.Size = new System.Drawing.Size(146, 20);
            this.lblDefaultTextureScale.TabIndex = 36;
            this.lblDefaultTextureScale.Text = "Default Texture Scale";
            this.lblDefaultTextureScale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // grpFgds
            // 
            this.grpFgds.Controls.Add(this.cmbDefaultPointEntity);
            this.grpFgds.Controls.Add(this.cmbDefaultBrushEntity);
            this.grpFgds.Controls.Add(this.lblDefaultPointEntity);
            this.grpFgds.Controls.Add(this.lblDefaultBrushEntity);
            this.grpFgds.Controls.Add(this.lblDefaultTextureScale);
            this.grpFgds.Controls.Add(this.nudDefaultTextureScale);
            this.grpFgds.Location = new System.Drawing.Point(8, 425);
            this.grpFgds.Name = "grpFgds";
            this.grpFgds.Size = new System.Drawing.Size(456, 107);
            this.grpFgds.TabIndex = 47;
            this.grpFgds.TabStop = false;
            this.grpFgds.Text = "Environment Parameters";
            // 
            // grpDirectories
            // 
            this.grpDirectories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpDirectories.Controls.Add(this.modelsGrid);
            this.grpDirectories.Controls.Add(this.texturesGrid);
            this.grpDirectories.Location = new System.Drawing.Point(8, 8);
            this.grpDirectories.Name = "grpDirectories";
            this.grpDirectories.Size = new System.Drawing.Size(456, 411);
            this.grpDirectories.TabIndex = 50;
            this.grpDirectories.TabStop = false;
            this.grpDirectories.Text = "Resource Directories";
            // 
            // texturesGrid
            // 
            this.texturesGrid.AllowUserToAddRows = false;
            this.texturesGrid.AllowUserToResizeColumns = false;
            this.texturesGrid.AllowUserToResizeRows = false;
            this.texturesGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.texturesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.texturesGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.deleteButtons,
            this.textureDirs,
            this.browseButtons});
            this.texturesGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.texturesGrid.Location = new System.Drawing.Point(13, 19);
            this.texturesGrid.Name = "texturesGrid";
            this.texturesGrid.RowHeadersVisible = false;
            this.texturesGrid.Size = new System.Drawing.Size(437, 187);
            this.texturesGrid.TabIndex = 0;
            // 
            // deleteButtons
            // 
            this.deleteButtons.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteButtons.HeaderText = "";
            this.deleteButtons.MinimumWidth = 24;
            this.deleteButtons.Name = "deleteButtons";
            this.deleteButtons.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.deleteButtons.Width = 24;
            // 
            // textureDirs
            // 
            this.textureDirs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.textureDirs.HeaderText = "Texture Directories";
            this.textureDirs.Name = "textureDirs";
            this.textureDirs.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.textureDirs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // browseButtons
            // 
            this.browseButtons.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.browseButtons.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseButtons.HeaderText = "";
            this.browseButtons.MinimumWidth = 50;
            this.browseButtons.Name = "browseButtons";
            this.browseButtons.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.browseButtons.Width = 50;
            // 
            // modelsGrid
            // 
            this.modelsGrid.AllowUserToAddRows = false;
            this.modelsGrid.AllowUserToResizeColumns = false;
            this.modelsGrid.AllowUserToResizeRows = false;
            this.modelsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.modelsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.modelsGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.deleteButtonsModel,
            this.modelDirs,
            this.browseButtonsModel});
            this.modelsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.modelsGrid.Location = new System.Drawing.Point(13, 212);
            this.modelsGrid.Name = "modelsGrid";
            this.modelsGrid.RowHeadersVisible = false;
            this.modelsGrid.Size = new System.Drawing.Size(437, 187);
            this.modelsGrid.TabIndex = 1;
            // 
            // deleteButtonsModel
            // 
            this.deleteButtonsModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteButtonsModel.HeaderText = "";
            this.deleteButtonsModel.MinimumWidth = 24;
            this.deleteButtonsModel.Name = "deleteButtonsModel";
            this.deleteButtonsModel.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.deleteButtonsModel.Width = 24;
            // 
            // modelDirs
            // 
            this.modelDirs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.modelDirs.HeaderText = "Model Directories";
            this.modelDirs.Name = "modelDirs";
            this.modelDirs.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.modelDirs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // browseButtonsModel
            // 
            this.browseButtonsModel.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.browseButtonsModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.browseButtonsModel.HeaderText = "";
            this.browseButtonsModel.MinimumWidth = 50;
            this.browseButtonsModel.Name = "browseButtonsModel";
            this.browseButtonsModel.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.browseButtonsModel.Width = 50;
            // 
            // BlitzEnvironmentEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpDirectories);
            this.Controls.Add(this.grpFgds);
            this.Name = "BlitzEnvironmentEditor";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(472, 539);
            ((System.ComponentModel.ISupportInitialize)(this.nudDefaultTextureScale)).EndInit();
            this.grpFgds.ResumeLayout(false);
            this.grpDirectories.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.texturesGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelsGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblDefaultBrushEntity;
        private System.Windows.Forms.Label lblDefaultPointEntity;
        private System.Windows.Forms.ComboBox cmbDefaultBrushEntity;
        private System.Windows.Forms.ComboBox cmbDefaultPointEntity;
        private System.Windows.Forms.NumericUpDown nudDefaultTextureScale;
        private System.Windows.Forms.Label lblDefaultTextureScale;
        private System.Windows.Forms.GroupBox grpFgds;
        private System.Windows.Forms.GroupBox grpDirectories;
        private System.Windows.Forms.DataGridView texturesGrid;
        private System.Windows.Forms.DataGridViewButtonColumn deleteButtons;
        private System.Windows.Forms.DataGridViewTextBoxColumn textureDirs;
        private System.Windows.Forms.DataGridViewButtonColumn browseButtons;
        private System.Windows.Forms.DataGridView modelsGrid;
        private System.Windows.Forms.DataGridViewButtonColumn deleteButtonsModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn modelDirs;
        private System.Windows.Forms.DataGridViewButtonColumn browseButtonsModel;
    }
}
