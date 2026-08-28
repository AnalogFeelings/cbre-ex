/*
 * Created by SharpDevelop.
 * User: Dan
 * Date: 10/09/2008
 * Time: 7:25 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using CBRE.Localization;
using System.Text.RegularExpressions;

namespace CBRE.Editor.Settings
{
	partial class SettingsForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.groupBox15 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.lblConfigSteamDir = new System.Windows.Forms.Label();
            this.btnConfigListSteamGames = new System.Windows.Forms.Button();
            this.btnConfigSteamDirBrowse = new System.Windows.Forms.Button();
            this.cmbConfigSteamUser = new System.Windows.Forms.ComboBox();
            this.lblConfigSteamUser = new System.Windows.Forms.Label();
            this.btnCancelSettings = new System.Windows.Forms.Button();
            this.btnApplyAndCloseSettings = new System.Windows.Forms.Button();
            this.btnApplySettings = new System.Windows.Forms.Button();
            this.HelpTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.tabHotkeys = new System.Windows.Forms.TabPage();
            this.HotkeyResetButton = new System.Windows.Forms.Button();
            this.groupBox22 = new System.Windows.Forms.GroupBox();
            this.HotkeyActionList = new System.Windows.Forms.ComboBox();
            this.HotkeyAddButton = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.HotkeyCombination = new System.Windows.Forms.TextBox();
            this.HotkeyReassignButton = new System.Windows.Forms.Button();
            this.HotkeyRemoveButton = new System.Windows.Forms.Button();
            this.HotkeyList = new System.Windows.Forms.ListView();
            this.chAction = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDescription = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ckKeyCombo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tab3DViews = new System.Windows.Forms.TabPage();
            this.groupBox12 = new System.Windows.Forms.GroupBox();
            this.label39 = new System.Windows.Forms.Label();
            this.ViewportBackgroundColour = new System.Windows.Forms.Panel();
            this.CameraFOV = new System.Windows.Forms.NumericUpDown();
            this.label29 = new System.Windows.Forms.Label();
            this.groupBox13 = new System.Windows.Forms.GroupBox();
            this.label32 = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.TimeToTopSpeedUpDown = new System.Windows.Forms.NumericUpDown();
            this.ForwardSpeedUpDown = new System.Windows.Forms.NumericUpDown();
            this.MouseWheelMoveDistance = new System.Windows.Forms.NumericUpDown();
            this.InvertMouseX = new System.Windows.Forms.CheckBox();
            this.InvertMouseY = new System.Windows.Forms.CheckBox();
            this.TimeToTopSpeed = new System.Windows.Forms.TrackBar();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.ForwardSpeed = new System.Windows.Forms.TrackBar();
            this.groupBox14 = new System.Windows.Forms.GroupBox();
            this.label26 = new System.Windows.Forms.Label();
            this.DetailRenderDistanceUpDown = new System.Windows.Forms.NumericUpDown();
            this.ModelRenderDistanceUpDown = new System.Windows.Forms.NumericUpDown();
            this.BackClippingPlaneUpDown = new System.Windows.Forms.NumericUpDown();
            this.label24 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.DetailRenderDistance = new System.Windows.Forms.TrackBar();
            this.ModelRenderDistance = new System.Windows.Forms.TrackBar();
            this.BackClippingPane = new System.Windows.Forms.TrackBar();
            this.tab2DViews = new System.Windows.Forms.TabPage();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.RotationStyle_SnapNever = new System.Windows.Forms.RadioButton();
            this.RotationStyle_SnapOnShift = new System.Windows.Forms.RadioButton();
            this.RotationStyle_SnapOffShift = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ColourPresetPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.GridHighlight2Colour = new System.Windows.Forms.Panel();
            this.label30 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.GridBackgroundColour = new System.Windows.Forms.Panel();
            this.GridHighlight2UnitNum = new System.Windows.Forms.DomainUpDown();
            this.GridBoundaryColour = new System.Windows.Forms.Panel();
            this.GridZeroAxisColour = new System.Windows.Forms.Panel();
            this.label19 = new System.Windows.Forms.Label();
            this.GridHighlight1On = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.GridHighlight2On = new System.Windows.Forms.CheckBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.GridHighlight1Colour = new System.Windows.Forms.Panel();
            this.GridColour = new System.Windows.Forms.Panel();
            this.GridHighlight1Distance = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox17 = new System.Windows.Forms.GroupBox();
            this.DrawEntityAngles = new System.Windows.Forms.CheckBox();
            this.DrawEntityNames = new System.Windows.Forms.CheckBox();
            this.CrosshairCursorIn2DViews = new System.Windows.Forms.CheckBox();
            this.groupBox16 = new System.Windows.Forms.GroupBox();
            this.ArrowKeysNudgeSelection = new System.Windows.Forms.CheckBox();
            this.NudgeStyle_GridOnCtrl = new System.Windows.Forms.RadioButton();
            this.NudgeStyle_GridOffCtrl = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.NudgeUnits = new System.Windows.Forms.NumericUpDown();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.SnapStyle_SnapOffAlt = new System.Windows.Forms.RadioButton();
            this.SnapStyle_SnapOnAlt = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.HideGridOn = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.HideGridLimit = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.HideGridFactor = new System.Windows.Forms.DomainUpDown();
            this.DefaultGridSize = new System.Windows.Forms.DomainUpDown();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tbcSettings = new System.Windows.Forms.TabControl();
            this.tabDirectories = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textureDirsDataGrid = new System.Windows.Forms.DataGridView();
            this.deleteButtons = new System.Windows.Forms.DataGridViewButtonColumn();
            this.textureDirs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.browseButtons = new System.Windows.Forms.DataGridViewButtonColumn();
            this.modelDirsDataGrid = new System.Windows.Forms.DataGridView();
            this.dataGridViewButtonColumn1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewButtonColumn2 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox10.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.tabHotkeys.SuspendLayout();
            this.groupBox22.SuspendLayout();
            this.tab3DViews.SuspendLayout();
            this.groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CameraFOV)).BeginInit();
            this.groupBox13.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeToTopSpeedUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ForwardSpeedUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouseWheelMoveDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeToTopSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ForwardSpeed)).BeginInit();
            this.groupBox14.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetailRenderDistanceUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelRenderDistanceUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackClippingPlaneUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetailRenderDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelRenderDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackClippingPane)).BeginInit();
            this.tab2DViews.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.ColourPresetPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridHighlight1Distance)).BeginInit();
            this.groupBox17.SuspendLayout();
            this.groupBox16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NudgeUnits)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HideGridLimit)).BeginInit();
            this.tabGeneral.SuspendLayout();
            this.tbcSettings.SuspendLayout();
            this.tabDirectories.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textureDirsDataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelDirsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox15
            // 
            this.groupBox15.Location = new System.Drawing.Point(0, 0);
            this.groupBox15.Name = "groupBox15";
            this.groupBox15.Size = new System.Drawing.Size(200, 100);
            this.groupBox15.TabIndex = 0;
            this.groupBox15.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 100);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.checkBox6);
            this.groupBox10.Controls.Add(this.checkBox5);
            this.groupBox10.Location = new System.Drawing.Point(6, 19);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(426, 83);
            this.groupBox10.TabIndex = 3;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Mouselook";
            // 
            // checkBox6
            // 
            this.checkBox6.Location = new System.Drawing.Point(27, 49);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(104, 24);
            this.checkBox6.TabIndex = 0;
            this.checkBox6.Text = Local.LocalString("setting.invert_x_axis");
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.Location = new System.Drawing.Point(27, 19);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(104, 24);
            this.checkBox5.TabIndex = 0;
            this.checkBox5.Text = Local.LocalString("setting.invert_y_axis");
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.trackBar3);
            this.groupBox9.Controls.Add(this.label5);
            this.groupBox9.Location = new System.Drawing.Point(6, 319);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(426, 98);
            this.groupBox9.TabIndex = 2;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = Local.LocalString("setting.time_to_top");
            // 
            // trackBar3
            // 
            this.trackBar3.AutoSize = false;
            this.trackBar3.BackColor = System.Drawing.SystemColors.Window;
            this.trackBar3.Location = new System.Drawing.Point(6, 20);
            this.trackBar3.Maximum = 50;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new System.Drawing.Size(414, 42);
            this.trackBar3.TabIndex = 0;
            this.trackBar3.TickFrequency = 10000;
            this.trackBar3.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar3.Value = 5;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(414, 23);
            this.label5.TabIndex = 1;
            this.label5.Text = "0.5 sec";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.trackBar2);
            this.groupBox8.Controls.Add(this.label1);
            this.groupBox8.Location = new System.Drawing.Point(6, 215);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(426, 98);
            this.groupBox8.TabIndex = 2;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = Local.LocalString("setting.forward_speed");
            // 
            // trackBar2
            // 
            this.trackBar2.AutoSize = false;
            this.trackBar2.BackColor = System.Drawing.SystemColors.Window;
            this.trackBar2.Location = new System.Drawing.Point(6, 20);
            this.trackBar2.Maximum = 5000;
            this.trackBar2.Minimum = 100;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(414, 42);
            this.trackBar2.TabIndex = 0;
            this.trackBar2.TickFrequency = 10000;
            this.trackBar2.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar2.Value = 1000;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(414, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "1000 units/sec";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.trackBar1);
            this.groupBox7.Controls.Add(this.label4);
            this.groupBox7.Location = new System.Drawing.Point(6, 108);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(426, 98);
            this.groupBox7.TabIndex = 2;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Back Clipping Pane";
            // 
            // trackBar1
            // 
            this.trackBar1.AutoSize = false;
            this.trackBar1.BackColor = System.Drawing.SystemColors.Window;
            this.trackBar1.Location = new System.Drawing.Point(6, 20);
            this.trackBar1.Maximum = 10000;
            this.trackBar1.Minimum = 2000;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(414, 42);
            this.trackBar1.TabIndex = 0;
            this.trackBar1.TickFrequency = 10000;
            this.trackBar1.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar1.Value = 5000;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(414, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "4000";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblConfigSteamDir
            // 
            this.lblConfigSteamDir.Location = new System.Drawing.Point(12, 9);
            this.lblConfigSteamDir.Name = "lblConfigSteamDir";
            this.lblConfigSteamDir.Size = new System.Drawing.Size(97, 20);
            this.lblConfigSteamDir.TabIndex = 6;
            this.lblConfigSteamDir.Text = "Steam Directory";
            this.lblConfigSteamDir.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnConfigListSteamGames
            // 
            this.btnConfigListSteamGames.Location = new System.Drawing.Point(243, 33);
            this.btnConfigListSteamGames.Name = "btnConfigListSteamGames";
            this.btnConfigListSteamGames.Size = new System.Drawing.Size(115, 25);
            this.btnConfigListSteamGames.TabIndex = 8;
            this.btnConfigListSteamGames.Text = "List Available Games";
            this.btnConfigListSteamGames.UseVisualStyleBackColor = true;
            // 
            // btnConfigSteamDirBrowse
            // 
            this.btnConfigSteamDirBrowse.Location = new System.Drawing.Point(346, 7);
            this.btnConfigSteamDirBrowse.Name = "btnConfigSteamDirBrowse";
            this.btnConfigSteamDirBrowse.Size = new System.Drawing.Size(67, 23);
            this.btnConfigSteamDirBrowse.TabIndex = 8;
            this.btnConfigSteamDirBrowse.Text = "Browse...";
            this.btnConfigSteamDirBrowse.UseVisualStyleBackColor = true;
            // 
            // cmbConfigSteamUser
            // 
            this.cmbConfigSteamUser.FormattingEnabled = true;
            this.cmbConfigSteamUser.Location = new System.Drawing.Point(115, 35);
            this.cmbConfigSteamUser.Name = "cmbConfigSteamUser";
            this.cmbConfigSteamUser.Size = new System.Drawing.Size(121, 20);
            this.cmbConfigSteamUser.TabIndex = 7;
            this.cmbConfigSteamUser.Text = "Penguinboy77";
            // 
            // lblConfigSteamUser
            // 
            this.lblConfigSteamUser.Location = new System.Drawing.Point(12, 36);
            this.lblConfigSteamUser.Name = "lblConfigSteamUser";
            this.lblConfigSteamUser.Size = new System.Drawing.Size(97, 20);
            this.lblConfigSteamUser.TabIndex = 6;
            this.lblConfigSteamUser.Text = "Steam Username";
            this.lblConfigSteamUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancelSettings
            // 
            this.btnCancelSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancelSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelSettings.Location = new System.Drawing.Point(792, 412);
            this.btnCancelSettings.Name = "btnCancelSettings";
            this.btnCancelSettings.Size = new System.Drawing.Size(75, 21);
            this.btnCancelSettings.TabIndex = 1;
            this.btnCancelSettings.Text = Local.LocalString("cancel");
            this.btnCancelSettings.UseVisualStyleBackColor = true;
            this.btnCancelSettings.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Close);
            // 
            // btnApplyAndCloseSettings
            // 
            this.btnApplyAndCloseSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplyAndCloseSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnApplyAndCloseSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplyAndCloseSettings.Location = new System.Drawing.Point(682, 412);
            this.btnApplyAndCloseSettings.Name = "btnApplyAndCloseSettings";
            this.btnApplyAndCloseSettings.Size = new System.Drawing.Size(104, 21);
            this.btnApplyAndCloseSettings.TabIndex = 1;
            this.btnApplyAndCloseSettings.Text = Local.LocalString("apply_close");
            this.btnApplyAndCloseSettings.UseVisualStyleBackColor = true;
            this.btnApplyAndCloseSettings.MouseClick += new System.Windows.Forms.MouseEventHandler(this.ApplyAndClose);
            // 
            // btnApplySettings
            // 
            this.btnApplySettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApplySettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnApplySettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnApplySettings.Location = new System.Drawing.Point(601, 412);
            this.btnApplySettings.Name = "btnApplySettings";
            this.btnApplySettings.Size = new System.Drawing.Size(75, 21);
            this.btnApplySettings.TabIndex = 1;
            this.btnApplySettings.Text = Local.LocalString("apply");
            this.btnApplySettings.UseVisualStyleBackColor = true;
            this.btnApplySettings.Click += new System.EventHandler(this.Apply);
            // 
            // HelpTooltip
            // 
            this.HelpTooltip.AutomaticDelay = 0;
            this.HelpTooltip.AutoPopDelay = 32000;
            this.HelpTooltip.InitialDelay = 500;
            this.HelpTooltip.IsBalloon = true;
            this.HelpTooltip.ReshowDelay = 100;
            this.HelpTooltip.UseAnimation = false;
            this.HelpTooltip.UseFading = false;
            // 
            // tabHotkeys
            // 
            this.tabHotkeys.BackColor = System.Drawing.Color.Transparent;
            this.tabHotkeys.Controls.Add(this.HotkeyResetButton);
            this.tabHotkeys.Controls.Add(this.groupBox22);
            this.tabHotkeys.Controls.Add(this.HotkeyReassignButton);
            this.tabHotkeys.Controls.Add(this.HotkeyRemoveButton);
            this.tabHotkeys.Controls.Add(this.HotkeyList);
            this.tabHotkeys.Location = new System.Drawing.Point(4, 24);
            this.tabHotkeys.Name = "tabHotkeys";
            this.tabHotkeys.Padding = new System.Windows.Forms.Padding(3);
            this.tabHotkeys.Size = new System.Drawing.Size(847, 367);
            this.tabHotkeys.TabIndex = 6;
            this.tabHotkeys.Text = Local.LocalString("setting.hotkey");
            // 
            // HotkeyResetButton
            // 
            this.HotkeyResetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.HotkeyResetButton.Location = new System.Drawing.Point(648, 474);
            this.HotkeyResetButton.Name = "HotkeyResetButton";
            this.HotkeyResetButton.Size = new System.Drawing.Size(118, 21);
            this.HotkeyResetButton.TabIndex = 5;
            this.HotkeyResetButton.Text = "Reset to Defaults";
            this.HotkeyResetButton.UseVisualStyleBackColor = true;
            this.HotkeyResetButton.Click += new System.EventHandler(this.HotkeyResetButtonClicked);
            // 
            // groupBox22
            // 
            this.groupBox22.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox22.Controls.Add(this.HotkeyActionList);
            this.groupBox22.Controls.Add(this.HotkeyAddButton);
            this.groupBox22.Controls.Add(this.label25);
            this.groupBox22.Controls.Add(this.label23);
            this.groupBox22.Controls.Add(this.HotkeyCombination);
            this.groupBox22.Location = new System.Drawing.Point(6, 450);
            this.groupBox22.Name = "groupBox22";
            this.groupBox22.Size = new System.Drawing.Size(437, 46);
            this.groupBox22.TabIndex = 4;
            this.groupBox22.TabStop = false;
            this.groupBox22.Text = "New Hotkey";
            // 
            // HotkeyActionList
            // 
            this.HotkeyActionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HotkeyActionList.FormattingEnabled = true;
            this.HotkeyActionList.Location = new System.Drawing.Point(49, 17);
            this.HotkeyActionList.Name = "HotkeyActionList";
            this.HotkeyActionList.Size = new System.Drawing.Size(150, 23);
            this.HotkeyActionList.TabIndex = 3;
            // 
            // HotkeyAddButton
            // 
            this.HotkeyAddButton.Location = new System.Drawing.Point(358, 15);
            this.HotkeyAddButton.Name = "HotkeyAddButton";
            this.HotkeyAddButton.Size = new System.Drawing.Size(69, 21);
            this.HotkeyAddButton.TabIndex = 3;
            this.HotkeyAddButton.Text = "Add";
            this.HotkeyAddButton.UseVisualStyleBackColor = true;
            this.HotkeyAddButton.Click += new System.EventHandler(this.HotkeyAddButtonClicked);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(6, 20);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(42, 15);
            this.label25.TabIndex = 2;
            this.label25.Text = Local.LocalString("setting.hotkey.action");
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(205, 20);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(45, 15);
            this.label23.TabIndex = 2;
            this.label23.Text = Local.LocalString("setting.hotkey.hotkey");
            // 
            // HotkeyCombination
            // 
            this.HotkeyCombination.Location = new System.Drawing.Point(252, 18);
            this.HotkeyCombination.Name = "HotkeyCombination";
            this.HotkeyCombination.Size = new System.Drawing.Size(100, 23);
            this.HotkeyCombination.TabIndex = 1;
            this.HotkeyCombination.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyCombinationKeyDown);
            // 
            // HotkeyReassignButton
            // 
            this.HotkeyReassignButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.HotkeyReassignButton.Location = new System.Drawing.Point(772, 450);
            this.HotkeyReassignButton.Name = "HotkeyReassignButton";
            this.HotkeyReassignButton.Size = new System.Drawing.Size(69, 21);
            this.HotkeyReassignButton.TabIndex = 3;
            this.HotkeyReassignButton.Text = "Reassign";
            this.HotkeyReassignButton.UseVisualStyleBackColor = true;
            this.HotkeyReassignButton.Click += new System.EventHandler(this.HotkeyReassignButtonClicked);
            // 
            // HotkeyRemoveButton
            // 
            this.HotkeyRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.HotkeyRemoveButton.Location = new System.Drawing.Point(772, 474);
            this.HotkeyRemoveButton.Name = "HotkeyRemoveButton";
            this.HotkeyRemoveButton.Size = new System.Drawing.Size(69, 21);
            this.HotkeyRemoveButton.TabIndex = 3;
            this.HotkeyRemoveButton.Text = "Remove";
            this.HotkeyRemoveButton.UseVisualStyleBackColor = true;
            this.HotkeyRemoveButton.Click += new System.EventHandler(this.HotkeyRemoveButtonClicked);
            // 
            // HotkeyList
            // 
            this.HotkeyList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HotkeyList.BackColor = System.Drawing.SystemColors.Control;
            this.HotkeyList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chAction,
            this.chDescription,
            this.ckKeyCombo});
            this.HotkeyList.FullRowSelect = true;
            this.HotkeyList.HideSelection = false;
            this.HotkeyList.Location = new System.Drawing.Point(6, 6);
            this.HotkeyList.MultiSelect = false;
            this.HotkeyList.Name = "HotkeyList";
            this.HotkeyList.Size = new System.Drawing.Size(835, 358);
            this.HotkeyList.TabIndex = 0;
            this.HotkeyList.UseCompatibleStateImageBehavior = false;
            this.HotkeyList.View = System.Windows.Forms.View.Details;
            this.HotkeyList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HotkeyListKeyDown);
            this.HotkeyList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.HotkeyListDoubleClicked);
            // 
            // chAction
            // 
            this.chAction.Text = Local.LocalString("setting.hotkey.action");
            // 
            // chDescription
            // 
            this.chDescription.Text = Local.LocalString("setting.hotkey.description");
            // 
            // ckKeyCombo
            // 
            this.ckKeyCombo.Text = Local.LocalString("setting.hotkey.hotkey");
            // 
            // tab3DViews
            // 
            this.tab3DViews.AutoScroll = true;
            this.tab3DViews.BackColor = System.Drawing.Color.Transparent;
            this.tab3DViews.Controls.Add(this.groupBox12);
            this.tab3DViews.Controls.Add(this.groupBox13);
            this.tab3DViews.Controls.Add(this.groupBox14);
            this.tab3DViews.Location = new System.Drawing.Point(4, 24);
            this.tab3DViews.Name = "tab3DViews";
            this.tab3DViews.Padding = new System.Windows.Forms.Padding(3);
            this.tab3DViews.Size = new System.Drawing.Size(847, 367);
            this.tab3DViews.TabIndex = 4;
            this.tab3DViews.Text = Local.LocalString("setting.3d_views");
            // 
            // groupBox12
            // 
            this.groupBox12.Controls.Add(this.label39);
            this.groupBox12.Controls.Add(this.ViewportBackgroundColour);
            this.groupBox12.Controls.Add(this.CameraFOV);
            this.groupBox12.Controls.Add(this.label29);
            this.groupBox12.Location = new System.Drawing.Point(475, 6);
            this.groupBox12.Name = "groupBox12";
            this.groupBox12.Size = new System.Drawing.Size(255, 84);
            this.groupBox12.TabIndex = 4;
            this.groupBox12.TabStop = false;
            this.groupBox12.Text = Local.LocalString("setting.general");
            // 
            // label39
            // 
            this.label39.Location = new System.Drawing.Point(12, 45);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(74, 21);
            this.label39.TabIndex = 3;
            this.label39.Text = Local.LocalString("setting.grid_color.background");
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ViewportBackgroundColour
            // 
            this.ViewportBackgroundColour.BackColor = System.Drawing.Color.Black;
            this.ViewportBackgroundColour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ViewportBackgroundColour.Location = new System.Drawing.Point(92, 45);
            this.ViewportBackgroundColour.Name = "ViewportBackgroundColour";
            this.ViewportBackgroundColour.Size = new System.Drawing.Size(51, 21);
            this.ViewportBackgroundColour.TabIndex = 4;
            // 
            // CameraFOV
            // 
            this.CameraFOV.Location = new System.Drawing.Point(92, 18);
            this.CameraFOV.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.CameraFOV.Minimum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.CameraFOV.Name = "CameraFOV";
            this.CameraFOV.Size = new System.Drawing.Size(50, 23);
            this.CameraFOV.TabIndex = 1;
            this.CameraFOV.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // label29
            // 
            this.label29.Location = new System.Drawing.Point(12, 18);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(74, 21);
            this.label29.TabIndex = 0;
            this.label29.Text = Local.LocalString("setting.3d_views.fov");
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox13
            // 
            this.groupBox13.Controls.Add(this.label32);
            this.groupBox13.Controls.Add(this.label43);
            this.groupBox13.Controls.Add(this.label31);
            this.groupBox13.Controls.Add(this.TimeToTopSpeedUpDown);
            this.groupBox13.Controls.Add(this.ForwardSpeedUpDown);
            this.groupBox13.Controls.Add(this.MouseWheelMoveDistance);
            this.groupBox13.Controls.Add(this.InvertMouseX);
            this.groupBox13.Controls.Add(this.InvertMouseY);
            this.groupBox13.Controls.Add(this.TimeToTopSpeed);
            this.groupBox13.Controls.Add(this.label28);
            this.groupBox13.Controls.Add(this.label27);
            this.groupBox13.Controls.Add(this.ForwardSpeed);
            this.groupBox13.Location = new System.Drawing.Point(6, 173);
            this.groupBox13.Name = "groupBox13";
            this.groupBox13.Size = new System.Drawing.Size(463, 166);
            this.groupBox13.TabIndex = 2;
            this.groupBox13.TabStop = false;
            this.groupBox13.Text = Local.LocalString("setting.navigation");
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(400, 96);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(50, 15);
            this.label32.TabIndex = 7;
            this.label32.Text = Local.LocalString("setting.3d_views.sec");
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label43
            // 
            this.label43.Location = new System.Drawing.Point(240, 138);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(161, 21);
            this.label43.TabIndex = 6;
            this.label43.Text = Local.LocalString("setting.3d_views.wheel_move_distance");
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(395, 52);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(55, 15);
            this.label31.TabIndex = 7;
            this.label31.Text = Local.LocalString("setting.3d_views.units_per_sec");
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TimeToTopSpeedUpDown
            // 
            this.TimeToTopSpeedUpDown.DecimalPlaces = 1;
            this.TimeToTopSpeedUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.TimeToTopSpeedUpDown.Location = new System.Drawing.Point(392, 72);
            this.TimeToTopSpeedUpDown.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TimeToTopSpeedUpDown.Name = "TimeToTopSpeedUpDown";
            this.TimeToTopSpeedUpDown.Size = new System.Drawing.Size(65, 23);
            this.TimeToTopSpeedUpDown.TabIndex = 5;
            this.TimeToTopSpeedUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.TimeToTopSpeedUpDown.ValueChanged += new System.EventHandler(this.TimeToTopSpeedUpDownValueChanged);
            // 
            // ForwardSpeedUpDown
            // 
            this.ForwardSpeedUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ForwardSpeedUpDown.Location = new System.Drawing.Point(392, 28);
            this.ForwardSpeedUpDown.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.ForwardSpeedUpDown.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ForwardSpeedUpDown.Name = "ForwardSpeedUpDown";
            this.ForwardSpeedUpDown.Size = new System.Drawing.Size(65, 23);
            this.ForwardSpeedUpDown.TabIndex = 5;
            this.ForwardSpeedUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ForwardSpeedUpDown.ValueChanged += new System.EventHandler(this.ForwardSpeedUpDownValueChanged);
            // 
            // MouseWheelMoveDistance
            // 
            this.MouseWheelMoveDistance.Location = new System.Drawing.Point(407, 138);
            this.MouseWheelMoveDistance.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.MouseWheelMoveDistance.Name = "MouseWheelMoveDistance";
            this.MouseWheelMoveDistance.Size = new System.Drawing.Size(50, 23);
            this.MouseWheelMoveDistance.TabIndex = 5;
            this.MouseWheelMoveDistance.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // InvertMouseX
            // 
            this.InvertMouseX.Location = new System.Drawing.Point(12, 137);
            this.InvertMouseX.Name = "InvertMouseX";
            this.InvertMouseX.Size = new System.Drawing.Size(170, 22);
            this.InvertMouseX.TabIndex = 0;
            this.InvertMouseX.Text = Local.LocalString("setting.3d_views.invert_x_axis");
            this.InvertMouseX.UseVisualStyleBackColor = true;
            // 
            // InvertMouseY
            // 
            this.InvertMouseY.Location = new System.Drawing.Point(12, 109);
            this.InvertMouseY.Name = "InvertMouseY";
            this.InvertMouseY.Size = new System.Drawing.Size(170, 22);
            this.InvertMouseY.TabIndex = 0;
            this.InvertMouseY.Text = Local.LocalString("setting.3d_views.invert_y_axis");
            this.InvertMouseY.UseVisualStyleBackColor = true;
            // 
            // TimeToTopSpeed
            // 
            this.TimeToTopSpeed.AutoSize = false;
            this.TimeToTopSpeed.BackColor = System.Drawing.SystemColors.Control;
            this.TimeToTopSpeed.Location = new System.Drawing.Point(125, 63);
            this.TimeToTopSpeed.Maximum = 50;
            this.TimeToTopSpeed.Name = "TimeToTopSpeed";
            this.TimeToTopSpeed.Size = new System.Drawing.Size(261, 39);
            this.TimeToTopSpeed.TabIndex = 0;
            this.TimeToTopSpeed.TickFrequency = 10;
            this.TimeToTopSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.TimeToTopSpeed.Value = 5;
            this.TimeToTopSpeed.Scroll += new System.EventHandler(this.TimeToTopSpeedChanged);
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(9, 77);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(102, 15);
            this.label28.TabIndex = 4;
            this.label28.Text = Local.LocalString("setting.time_to_top");
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(9, 32);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(84, 15);
            this.label27.TabIndex = 4;
            this.label27.Text = Local.LocalString("setting.forward_speed");
            // 
            // ForwardSpeed
            // 
            this.ForwardSpeed.AutoSize = false;
            this.ForwardSpeed.BackColor = System.Drawing.SystemColors.Control;
            this.ForwardSpeed.Location = new System.Drawing.Point(125, 18);
            this.ForwardSpeed.Maximum = 5000;
            this.ForwardSpeed.Minimum = 100;
            this.ForwardSpeed.Name = "ForwardSpeed";
            this.ForwardSpeed.Size = new System.Drawing.Size(261, 39);
            this.ForwardSpeed.TabIndex = 0;
            this.ForwardSpeed.TickFrequency = 1000;
            this.ForwardSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.ForwardSpeed.Value = 1000;
            this.ForwardSpeed.Scroll += new System.EventHandler(this.ForwardSpeedChanged);
            // 
            // groupBox14
            // 
            this.groupBox14.Controls.Add(this.label26);
            this.groupBox14.Controls.Add(this.DetailRenderDistanceUpDown);
            this.groupBox14.Controls.Add(this.ModelRenderDistanceUpDown);
            this.groupBox14.Controls.Add(this.BackClippingPlaneUpDown);
            this.groupBox14.Controls.Add(this.label24);
            this.groupBox14.Controls.Add(this.label22);
            this.groupBox14.Controls.Add(this.DetailRenderDistance);
            this.groupBox14.Controls.Add(this.ModelRenderDistance);
            this.groupBox14.Controls.Add(this.BackClippingPane);
            this.groupBox14.Location = new System.Drawing.Point(6, 6);
            this.groupBox14.Name = "groupBox14";
            this.groupBox14.Size = new System.Drawing.Size(463, 162);
            this.groupBox14.TabIndex = 2;
            this.groupBox14.TabStop = false;
            this.groupBox14.Text = Local.LocalString("setting.performance");
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(7, 126);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(121, 15);
            this.label26.TabIndex = 4;
            this.label26.Text = Local.LocalString("setting.detail_render_distance");
            // 
            // DetailRenderDistanceUpDown
            // 
            this.DetailRenderDistanceUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.DetailRenderDistanceUpDown.Location = new System.Drawing.Point(392, 121);
            this.DetailRenderDistanceUpDown.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.DetailRenderDistanceUpDown.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.DetailRenderDistanceUpDown.Name = "DetailRenderDistanceUpDown";
            this.DetailRenderDistanceUpDown.Size = new System.Drawing.Size(65, 23);
            this.DetailRenderDistanceUpDown.TabIndex = 5;
            this.DetailRenderDistanceUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.DetailRenderDistanceUpDown.ValueChanged += new System.EventHandler(this.DetailRenderDistanceUpDownValueChanged);
            // 
            // ModelRenderDistanceUpDown
            // 
            this.ModelRenderDistanceUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ModelRenderDistanceUpDown.Location = new System.Drawing.Point(392, 73);
            this.ModelRenderDistanceUpDown.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.ModelRenderDistanceUpDown.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.ModelRenderDistanceUpDown.Name = "ModelRenderDistanceUpDown";
            this.ModelRenderDistanceUpDown.Size = new System.Drawing.Size(65, 23);
            this.ModelRenderDistanceUpDown.TabIndex = 5;
            this.ModelRenderDistanceUpDown.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.ModelRenderDistanceUpDown.ValueChanged += new System.EventHandler(this.ModelRenderDistanceUpDownValueChanged);
            // 
            // BackClippingPlaneUpDown
            // 
            this.BackClippingPlaneUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.BackClippingPlaneUpDown.Location = new System.Drawing.Point(392, 30);
            this.BackClippingPlaneUpDown.Maximum = new decimal(new int[] {
            30000,
            0,
            0,
            0});
            this.BackClippingPlaneUpDown.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.BackClippingPlaneUpDown.Name = "BackClippingPlaneUpDown";
            this.BackClippingPlaneUpDown.Size = new System.Drawing.Size(65, 23);
            this.BackClippingPlaneUpDown.TabIndex = 5;
            this.BackClippingPlaneUpDown.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.BackClippingPlaneUpDown.ValueChanged += new System.EventHandler(this.BackClippingPlaneUpDownValueChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(7, 75);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(125, 15);
            this.label24.TabIndex = 4;
            this.label24.Text = Local.LocalString("setting.model_render_distance");
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(6, 31);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(110, 15);
            this.label22.TabIndex = 4;
            this.label22.Text = Local.LocalString("setting.back_clipping_plane");
            // 
            // DetailRenderDistance
            // 
            this.DetailRenderDistance.AutoSize = false;
            this.DetailRenderDistance.BackColor = System.Drawing.SystemColors.Control;
            this.DetailRenderDistance.Location = new System.Drawing.Point(138, 113);
            this.DetailRenderDistance.Maximum = 30000;
            this.DetailRenderDistance.Minimum = 200;
            this.DetailRenderDistance.Name = "DetailRenderDistance";
            this.DetailRenderDistance.Size = new System.Drawing.Size(249, 38);
            this.DetailRenderDistance.TabIndex = 0;
            this.DetailRenderDistance.TickFrequency = 10000;
            this.DetailRenderDistance.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.DetailRenderDistance.Value = 5000;
            this.DetailRenderDistance.Scroll += new System.EventHandler(this.DetailRenderDistanceChanged);
            // 
            // ModelRenderDistance
            // 
            this.ModelRenderDistance.AutoSize = false;
            this.ModelRenderDistance.BackColor = System.Drawing.SystemColors.Control;
            this.ModelRenderDistance.Location = new System.Drawing.Point(138, 62);
            this.ModelRenderDistance.Maximum = 30000;
            this.ModelRenderDistance.Minimum = 200;
            this.ModelRenderDistance.Name = "ModelRenderDistance";
            this.ModelRenderDistance.Size = new System.Drawing.Size(249, 38);
            this.ModelRenderDistance.TabIndex = 0;
            this.ModelRenderDistance.TickFrequency = 10000;
            this.ModelRenderDistance.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.ModelRenderDistance.Value = 5000;
            this.ModelRenderDistance.Scroll += new System.EventHandler(this.ModelRenderDistanceChanged);
            // 
            // BackClippingPane
            // 
            this.BackClippingPane.AutoSize = false;
            this.BackClippingPane.BackColor = System.Drawing.SystemColors.Control;
            this.BackClippingPane.Location = new System.Drawing.Point(138, 18);
            this.BackClippingPane.Maximum = 30000;
            this.BackClippingPane.Minimum = 2000;
            this.BackClippingPane.Name = "BackClippingPane";
            this.BackClippingPane.Size = new System.Drawing.Size(248, 38);
            this.BackClippingPane.TabIndex = 0;
            this.BackClippingPane.TickFrequency = 10000;
            this.BackClippingPane.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.BackClippingPane.Value = 5000;
            this.BackClippingPane.Scroll += new System.EventHandler(this.BackClippingPaneChanged);
            // 
            // tab2DViews
            // 
            this.tab2DViews.AutoScroll = true;
            this.tab2DViews.BackColor = System.Drawing.Color.Transparent;
            this.tab2DViews.Controls.Add(this.groupBox6);
            this.tab2DViews.Controls.Add(this.groupBox3);
            this.tab2DViews.Controls.Add(this.groupBox17);
            this.tab2DViews.Controls.Add(this.groupBox16);
            this.tab2DViews.Controls.Add(this.groupBox4);
            this.tab2DViews.Controls.Add(this.groupBox5);
            this.tab2DViews.Location = new System.Drawing.Point(4, 24);
            this.tab2DViews.Name = "tab2DViews";
            this.tab2DViews.Padding = new System.Windows.Forms.Padding(3);
            this.tab2DViews.Size = new System.Drawing.Size(847, 367);
            this.tab2DViews.TabIndex = 1;
            this.tab2DViews.Text = Local.LocalString("setting.2d_views");
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.RotationStyle_SnapNever);
            this.groupBox6.Controls.Add(this.RotationStyle_SnapOnShift);
            this.groupBox6.Controls.Add(this.RotationStyle_SnapOffShift);
            this.groupBox6.Location = new System.Drawing.Point(6, 6);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(265, 105);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = Local.LocalString("setting.rotation");
            // 
            // RotationStyle_SnapNever
            // 
            this.RotationStyle_SnapNever.Location = new System.Drawing.Point(6, 76);
            this.RotationStyle_SnapNever.Name = "RotationStyle_SnapNever";
            this.RotationStyle_SnapNever.Size = new System.Drawing.Size(241, 22);
            this.RotationStyle_SnapNever.TabIndex = 2;
            this.RotationStyle_SnapNever.Text = Local.LocalString("setting.rotation.no_snap");
            this.RotationStyle_SnapNever.UseVisualStyleBackColor = true;
            // 
            // RotationStyle_SnapOnShift
            // 
            this.RotationStyle_SnapOnShift.Checked = true;
            this.RotationStyle_SnapOnShift.Location = new System.Drawing.Point(6, 20);
            this.RotationStyle_SnapOnShift.Name = "RotationStyle_SnapOnShift";
            this.RotationStyle_SnapOnShift.Size = new System.Drawing.Size(241, 22);
            this.RotationStyle_SnapOnShift.TabIndex = 2;
            this.RotationStyle_SnapOnShift.TabStop = true;
            this.RotationStyle_SnapOnShift.Text = Local.LocalString("setting.rotation.shift_snap");
            this.RotationStyle_SnapOnShift.UseVisualStyleBackColor = true;
            // 
            // RotationStyle_SnapOffShift
            // 
            this.RotationStyle_SnapOffShift.Location = new System.Drawing.Point(6, 48);
            this.RotationStyle_SnapOffShift.Name = "RotationStyle_SnapOffShift";
            this.RotationStyle_SnapOffShift.Size = new System.Drawing.Size(246, 22);
            this.RotationStyle_SnapOffShift.TabIndex = 2;
            this.RotationStyle_SnapOffShift.Text = Local.LocalString("setting.rotation.snap_unless_shift");
            this.RotationStyle_SnapOffShift.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ColourPresetPanel);
            this.groupBox3.Controls.Add(this.GridHighlight2Colour);
            this.groupBox3.Controls.Add(this.label30);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.GridBackgroundColour);
            this.groupBox3.Controls.Add(this.GridHighlight2UnitNum);
            this.groupBox3.Controls.Add(this.GridBoundaryColour);
            this.groupBox3.Controls.Add(this.GridZeroAxisColour);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Controls.Add(this.GridHighlight1On);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.GridHighlight2On);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.GridHighlight1Colour);
            this.groupBox3.Controls.Add(this.GridColour);
            this.groupBox3.Controls.Add(this.GridHighlight1Distance);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(6, 226);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(478, 240);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = Local.LocalString("setting.grid_color");
            // 
            // ColourPresetPanel
            // 
            this.ColourPresetPanel.Controls.Add(this.button1);
            this.ColourPresetPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.ColourPresetPanel.Location = new System.Drawing.Point(366, 33);
            this.ColourPresetPanel.Margin = new System.Windows.Forms.Padding(1);
            this.ColourPresetPanel.Name = "ColourPresetPanel";
            this.ColourPresetPanel.Size = new System.Drawing.Size(102, 198);
            this.ColourPresetPanel.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 20);
            this.button1.TabIndex = 0;
            this.button1.Text = Local.LocalString("preset.color");
            this.button1.UseVisualStyleBackColor = true;
            // 
            // GridHighlight2Colour
            // 
            this.GridHighlight2Colour.BackColor = System.Drawing.Color.DarkRed;
            this.GridHighlight2Colour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridHighlight2Colour.Location = new System.Drawing.Point(94, 155);
            this.GridHighlight2Colour.Name = "GridHighlight2Colour";
            this.GridHighlight2Colour.Size = new System.Drawing.Size(51, 21);
            this.GridHighlight2Colour.TabIndex = 2;
            // 
            // label30
            // 
            this.label30.Location = new System.Drawing.Point(363, 15);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(94, 16);
            this.label30.TabIndex = 1;
            this.label30.Text = Local.LocalString("preset.color");
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = Local.LocalString("setting.grid_color.background");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 158);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 15);
            this.label9.TabIndex = 1;
            this.label9.Text = Local.LocalString("setting.grid_color.highlight_2");
            // 
            // GridBackgroundColour
            // 
            this.GridBackgroundColour.BackColor = System.Drawing.Color.Black;
            this.GridBackgroundColour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridBackgroundColour.Location = new System.Drawing.Point(94, 21);
            this.GridBackgroundColour.Name = "GridBackgroundColour";
            this.GridBackgroundColour.Size = new System.Drawing.Size(51, 21);
            this.GridBackgroundColour.TabIndex = 2;
            // 
            // GridHighlight2UnitNum
            // 
            this.GridHighlight2UnitNum.Items.Add("32768");
            this.GridHighlight2UnitNum.Items.Add("16384");
            this.GridHighlight2UnitNum.Items.Add("8192");
            this.GridHighlight2UnitNum.Items.Add("4096");
            this.GridHighlight2UnitNum.Items.Add("2048");
            this.GridHighlight2UnitNum.Items.Add("1024");
            this.GridHighlight2UnitNum.Items.Add("512");
            this.GridHighlight2UnitNum.Items.Add("256");
            this.GridHighlight2UnitNum.Items.Add("128");
            this.GridHighlight2UnitNum.Items.Add("64");
            this.GridHighlight2UnitNum.Items.Add("32");
            this.GridHighlight2UnitNum.Location = new System.Drawing.Point(265, 156);
            this.GridHighlight2UnitNum.Name = "GridHighlight2UnitNum";
            this.GridHighlight2UnitNum.Size = new System.Drawing.Size(50, 23);
            this.GridHighlight2UnitNum.TabIndex = 0;
            this.GridHighlight2UnitNum.Text = "1024";
            // 
            // GridBoundaryColour
            // 
            this.GridBoundaryColour.BackColor = System.Drawing.Color.Red;
            this.GridBoundaryColour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridBoundaryColour.Location = new System.Drawing.Point(94, 102);
            this.GridBoundaryColour.Name = "GridBoundaryColour";
            this.GridBoundaryColour.Size = new System.Drawing.Size(51, 21);
            this.GridBoundaryColour.TabIndex = 2;
            // 
            // GridZeroAxisColour
            // 
            this.GridZeroAxisColour.BackColor = System.Drawing.Color.Aqua;
            this.GridZeroAxisColour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridZeroAxisColour.Location = new System.Drawing.Point(94, 75);
            this.GridZeroAxisColour.Name = "GridZeroAxisColour";
            this.GridZeroAxisColour.Size = new System.Drawing.Size(51, 21);
            this.GridZeroAxisColour.TabIndex = 2;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(22, 104);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(66, 15);
            this.label19.TabIndex = 1;
            this.label19.Text = Local.LocalString("setting.grid_color.boundaries");
            // 
            // GridHighlight1On
            // 
            this.GridHighlight1On.Checked = true;
            this.GridHighlight1On.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GridHighlight1On.Location = new System.Drawing.Point(151, 128);
            this.GridHighlight1On.Name = "GridHighlight1On";
            this.GridHighlight1On.Size = new System.Drawing.Size(108, 21);
            this.GridHighlight1On.TabIndex = 0;
            this.GridHighlight1On.Text = Regex.Split(Local.LocalString("setting.grid_color.highlight_grid"), "{.}")[0].Trim();
            this.GridHighlight1On.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(29, 78);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 15);
            this.label10.TabIndex = 1;
            this.label10.Text = Local.LocalString("setting.grid_color.zero_axes");
            // 
            // GridHighlight2On
            // 
            this.GridHighlight2On.Checked = true;
            this.GridHighlight2On.CheckState = System.Windows.Forms.CheckState.Checked;
            this.GridHighlight2On.Location = new System.Drawing.Point(151, 155);
            this.GridHighlight2On.Name = "GridHighlight2On";
            this.GridHighlight2On.Size = new System.Drawing.Size(108, 21);
            this.GridHighlight2On.TabIndex = 0;
            this.GridHighlight2On.Text = Regex.Split(Local.LocalString("setting.grid_color.highlight_units"), "{.}")[0].Trim();
            this.GridHighlight2On.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.Location = new System.Drawing.Point(321, 158);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(41, 16);
            this.label21.TabIndex = 3;
            this.label21.Text = Regex.Split(Local.LocalString("setting.grid_color.highlight_units"), "{.}")[1].Trim();
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(321, 128);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(41, 16);
            this.label11.TabIndex = 3;
            this.label11.Text = Regex.Split(Local.LocalString("setting.grid_color.highlight_grid"), "{.}")[1].Trim();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(59, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 15);
            this.label7.TabIndex = 1;
            this.label7.Text = Local.LocalString("setting.grid_color.grid");
            // 
            // GridHighlight1Colour
            // 
            this.GridHighlight1Colour.BackColor = System.Drawing.Color.White;
            this.GridHighlight1Colour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridHighlight1Colour.Location = new System.Drawing.Point(94, 128);
            this.GridHighlight1Colour.Name = "GridHighlight1Colour";
            this.GridHighlight1Colour.Size = new System.Drawing.Size(51, 21);
            this.GridHighlight1Colour.TabIndex = 2;
            // 
            // GridColour
            // 
            this.GridColour.BackColor = System.Drawing.Color.Gainsboro;
            this.GridColour.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.GridColour.Location = new System.Drawing.Point(94, 48);
            this.GridColour.Name = "GridColour";
            this.GridColour.Size = new System.Drawing.Size(51, 21);
            this.GridColour.TabIndex = 2;
            // 
            // GridHighlight1Distance
            // 
            this.GridHighlight1Distance.Location = new System.Drawing.Point(265, 126);
            this.GridHighlight1Distance.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.GridHighlight1Distance.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.GridHighlight1Distance.Name = "GridHighlight1Distance";
            this.GridHighlight1Distance.Size = new System.Drawing.Size(50, 23);
            this.GridHighlight1Distance.TabIndex = 2;
            this.GridHighlight1Distance.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 15);
            this.label8.TabIndex = 1;
            this.label8.Text = Local.LocalString("setting.grid_color.highlight_1");
            // 
            // groupBox17
            // 
            this.groupBox17.Controls.Add(this.DrawEntityAngles);
            this.groupBox17.Controls.Add(this.DrawEntityNames);
            this.groupBox17.Controls.Add(this.CrosshairCursorIn2DViews);
            this.groupBox17.Location = new System.Drawing.Point(421, 116);
            this.groupBox17.Name = "groupBox17";
            this.groupBox17.Size = new System.Drawing.Size(403, 104);
            this.groupBox17.TabIndex = 0;
            this.groupBox17.TabStop = false;
            this.groupBox17.Text = Local.LocalString("setting.options");
            // 
            // DrawEntityAngles
            // 
            this.DrawEntityAngles.Location = new System.Drawing.Point(10, 73);
            this.DrawEntityAngles.Name = "DrawEntityAngles";
            this.DrawEntityAngles.Size = new System.Drawing.Size(225, 22);
            this.DrawEntityAngles.TabIndex = 5;
            this.DrawEntityAngles.Tag = "";
            this.DrawEntityAngles.Text = Local.LocalString("setting.options.draw_entity_angles");
            this.DrawEntityAngles.UseVisualStyleBackColor = true;
            // 
            // DrawEntityNames
            // 
            this.DrawEntityNames.Location = new System.Drawing.Point(10, 45);
            this.DrawEntityNames.Name = "DrawEntityNames";
            this.DrawEntityNames.Size = new System.Drawing.Size(225, 22);
            this.DrawEntityNames.TabIndex = 4;
            this.DrawEntityNames.Tag = "";
            this.DrawEntityNames.Text = Local.LocalString("setting.options.draw_entity_names");
            this.DrawEntityNames.UseVisualStyleBackColor = true;
            // 
            // CrosshairCursorIn2DViews
            // 
            this.CrosshairCursorIn2DViews.Location = new System.Drawing.Point(10, 18);
            this.CrosshairCursorIn2DViews.Name = "CrosshairCursorIn2DViews";
            this.CrosshairCursorIn2DViews.Size = new System.Drawing.Size(225, 22);
            this.CrosshairCursorIn2DViews.TabIndex = 0;
            this.CrosshairCursorIn2DViews.Tag = "";
            this.CrosshairCursorIn2DViews.Text = Local.LocalString("setting.options.crosshair_cursor");
            this.CrosshairCursorIn2DViews.UseVisualStyleBackColor = true;
            // 
            // groupBox16
            // 
            this.groupBox16.Controls.Add(this.ArrowKeysNudgeSelection);
            this.groupBox16.Controls.Add(this.NudgeStyle_GridOnCtrl);
            this.groupBox16.Controls.Add(this.NudgeStyle_GridOffCtrl);
            this.groupBox16.Controls.Add(this.label2);
            this.groupBox16.Controls.Add(this.NudgeUnits);
            this.groupBox16.Location = new System.Drawing.Point(526, 6);
            this.groupBox16.Name = "groupBox16";
            this.groupBox16.Size = new System.Drawing.Size(298, 105);
            this.groupBox16.TabIndex = 0;
            this.groupBox16.TabStop = false;
            this.groupBox16.Text = Local.LocalString("setting.nudge_grid");
            // 
            // ArrowKeysNudgeSelection
            // 
            this.ArrowKeysNudgeSelection.Checked = true;
            this.ArrowKeysNudgeSelection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ArrowKeysNudgeSelection.Location = new System.Drawing.Point(10, 18);
            this.ArrowKeysNudgeSelection.Name = "ArrowKeysNudgeSelection";
            this.ArrowKeysNudgeSelection.Size = new System.Drawing.Size(144, 21);
            this.ArrowKeysNudgeSelection.TabIndex = 0;
            this.ArrowKeysNudgeSelection.Text = Regex.Split(Local.LocalString("setting.nudge_grid.arrow_keys"), "{.}")[0].Trim();
            this.ArrowKeysNudgeSelection.UseVisualStyleBackColor = true;
            // 
            // NudgeStyle_GridOnCtrl
            // 
            this.NudgeStyle_GridOnCtrl.Checked = true;
            this.NudgeStyle_GridOnCtrl.Location = new System.Drawing.Point(10, 48);
            this.NudgeStyle_GridOnCtrl.Name = "NudgeStyle_GridOnCtrl";
            this.NudgeStyle_GridOnCtrl.Size = new System.Drawing.Size(264, 22);
            this.NudgeStyle_GridOnCtrl.TabIndex = 2;
            this.NudgeStyle_GridOnCtrl.TabStop = true;
            this.NudgeStyle_GridOnCtrl.Text = Local.LocalString("setting.nudge_grid.press_control");
            this.NudgeStyle_GridOnCtrl.UseVisualStyleBackColor = true;
            // 
            // NudgeStyle_GridOffCtrl
            // 
            this.NudgeStyle_GridOffCtrl.Location = new System.Drawing.Point(10, 76);
            this.NudgeStyle_GridOffCtrl.Name = "NudgeStyle_GridOffCtrl";
            this.NudgeStyle_GridOffCtrl.Size = new System.Drawing.Size(270, 22);
            this.NudgeStyle_GridOffCtrl.TabIndex = 2;
            this.NudgeStyle_GridOffCtrl.Text = Local.LocalString("setting.nudge_grid.nudge_unless_control_pressed");
            this.NudgeStyle_GridOffCtrl.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(217, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 22);
            this.label2.TabIndex = 3;
            this.label2.Text = Regex.Split(Local.LocalString("setting.nudge_grid.arrow_keys"), "{.}")[1].Trim();
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // NudgeUnits
            // 
            this.NudgeUnits.DecimalPlaces = 2;
            this.NudgeUnits.Location = new System.Drawing.Point(160, 18);
            this.NudgeUnits.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NudgeUnits.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.NudgeUnits.Name = "NudgeUnits";
            this.NudgeUnits.Size = new System.Drawing.Size(51, 23);
            this.NudgeUnits.TabIndex = 2;
            this.NudgeUnits.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.SnapStyle_SnapOffAlt);
            this.groupBox4.Controls.Add(this.SnapStyle_SnapOnAlt);
            this.groupBox4.Location = new System.Drawing.Point(277, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(243, 105);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = Local.LocalString("setting.snap_to_grid");
            // 
            // SnapStyle_SnapOffAlt
            // 
            this.SnapStyle_SnapOffAlt.Checked = true;
            this.SnapStyle_SnapOffAlt.Location = new System.Drawing.Point(10, 18);
            this.SnapStyle_SnapOffAlt.Name = "SnapStyle_SnapOffAlt";
            this.SnapStyle_SnapOffAlt.Size = new System.Drawing.Size(243, 22);
            this.SnapStyle_SnapOffAlt.TabIndex = 2;
            this.SnapStyle_SnapOffAlt.TabStop = true;
            this.SnapStyle_SnapOffAlt.Text = Local.LocalString("setting.snap_to_grid.hold_alt_to_ignore");
            this.SnapStyle_SnapOffAlt.UseVisualStyleBackColor = true;
            // 
            // SnapStyle_SnapOnAlt
            // 
            this.SnapStyle_SnapOnAlt.Location = new System.Drawing.Point(10, 45);
            this.SnapStyle_SnapOnAlt.Name = "SnapStyle_SnapOnAlt";
            this.SnapStyle_SnapOnAlt.Size = new System.Drawing.Size(243, 22);
            this.SnapStyle_SnapOnAlt.TabIndex = 2;
            this.SnapStyle_SnapOnAlt.Text = Local.LocalString("setting.snap_to_grid.ignore_unless_alt_pressed");
            this.SnapStyle_SnapOnAlt.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.HideGridOn);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.HideGridLimit);
            this.groupBox5.Controls.Add(this.label13);
            this.groupBox5.Controls.Add(this.HideGridFactor);
            this.groupBox5.Controls.Add(this.DefaultGridSize);
            this.groupBox5.Location = new System.Drawing.Point(6, 116);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(409, 104);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = Local.LocalString("setting.grid_setting");
            // 
            // HideGridOn
            // 
            this.HideGridOn.Checked = true;
            this.HideGridOn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HideGridOn.Location = new System.Drawing.Point(6, 46);
            this.HideGridOn.Name = "HideGridOn";
            this.HideGridOn.Size = new System.Drawing.Size(145, 21);
            this.HideGridOn.TabIndex = 5;
            this.HideGridOn.Text = Regex.Split(Local.LocalString("setting.grid_setting.hide_grid"), "{.}")[0].Trim();
            this.HideGridOn.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = Local.LocalString("setting.grid_setting.default_size");
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // HideGridLimit
            // 
            this.HideGridLimit.Location = new System.Drawing.Point(157, 45);
            this.HideGridLimit.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.HideGridLimit.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.HideGridLimit.Name = "HideGridLimit";
            this.HideGridLimit.Size = new System.Drawing.Size(34, 23);
            this.HideGridLimit.TabIndex = 2;
            this.HideGridLimit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(197, 46);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(122, 21);
            this.label13.TabIndex = 3;
            this.label13.Text = Regex.Split(Local.LocalString("setting.grid_setting.hide_grid"), "{.}")[1].Trim();
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // HideGridFactor
            // 
            this.HideGridFactor.Items.Add("64");
            this.HideGridFactor.Items.Add("32");
            this.HideGridFactor.Items.Add("16");
            this.HideGridFactor.Items.Add("8");
            this.HideGridFactor.Items.Add("4");
            this.HideGridFactor.Items.Add("2");
            this.HideGridFactor.Location = new System.Drawing.Point(325, 45);
            this.HideGridFactor.Name = "HideGridFactor";
            this.HideGridFactor.Size = new System.Drawing.Size(38, 23);
            this.HideGridFactor.TabIndex = 0;
            this.HideGridFactor.Text = "8";
            // 
            // DefaultGridSize
            // 
            this.DefaultGridSize.Items.Add("1024");
            this.DefaultGridSize.Items.Add("512");
            this.DefaultGridSize.Items.Add("256");
            this.DefaultGridSize.Items.Add("128");
            this.DefaultGridSize.Items.Add("64");
            this.DefaultGridSize.Items.Add("32");
            this.DefaultGridSize.Items.Add("16");
            this.DefaultGridSize.Items.Add("8");
            this.DefaultGridSize.Items.Add("4");
            this.DefaultGridSize.Items.Add("2");
            this.DefaultGridSize.Items.Add("1");
            this.DefaultGridSize.Location = new System.Drawing.Point(106, 18);
            this.DefaultGridSize.Name = "DefaultGridSize";
            this.DefaultGridSize.SelectedIndex = 4;
            this.DefaultGridSize.Size = new System.Drawing.Size(49, 23);
            this.DefaultGridSize.TabIndex = 0;
            this.DefaultGridSize.Text = "64";
            // 
            // tabGeneral
            // 
            this.tabGeneral.BackColor = System.Drawing.Color.Transparent;
            this.tabGeneral.Controls.Add(this.flowLayoutPanel1);
            this.tabGeneral.Location = new System.Drawing.Point(4, 24);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(847, 367);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = Local.LocalString("setting.general");
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(841, 361);
            this.flowLayoutPanel1.TabIndex = 6;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // tbcSettings
            // 
            this.tbcSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbcSettings.Controls.Add(this.tabGeneral);
            this.tbcSettings.Controls.Add(this.tab2DViews);
            this.tbcSettings.Controls.Add(this.tab3DViews);
            this.tbcSettings.Controls.Add(this.tabHotkeys);
            this.tbcSettings.Controls.Add(this.tabDirectories);
            this.tbcSettings.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbcSettings.Location = new System.Drawing.Point(12, 11);
            this.tbcSettings.Name = "tbcSettings";
            this.tbcSettings.SelectedIndex = 0;
            this.tbcSettings.Size = new System.Drawing.Size(855, 395);
            this.tbcSettings.TabIndex = 0;
            this.tbcSettings.SelectedIndexChanged += new System.EventHandler(this.TabChanged);
            // 
            // tabDirectories
            // 
            this.tabDirectories.BackColor = System.Drawing.Color.Transparent;
            this.tabDirectories.Controls.Add(this.tableLayoutPanel1);
            this.tabDirectories.Location = new System.Drawing.Point(4, 24);
            this.tabDirectories.Name = "tabDirectories";
            this.tabDirectories.Size = new System.Drawing.Size(847, 367);
            this.tabDirectories.TabIndex = 7;
            this.tabDirectories.Text = Local.LocalString("setting.directory");
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.textureDirsDataGrid, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.modelDirsDataGrid, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(840, 363);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // textureDirsDataGrid
            // 
            this.textureDirsDataGrid.AllowUserToAddRows = false;
            this.textureDirsDataGrid.AllowUserToResizeColumns = false;
            this.textureDirsDataGrid.AllowUserToResizeRows = false;
            this.textureDirsDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textureDirsDataGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.textureDirsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.textureDirsDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.deleteButtons,
            this.textureDirs,
            this.browseButtons});
            this.textureDirsDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.textureDirsDataGrid.Location = new System.Drawing.Point(3, 3);
            this.textureDirsDataGrid.Name = "textureDirsDataGrid";
            this.textureDirsDataGrid.RowHeadersVisible = false;
            this.textureDirsDataGrid.Size = new System.Drawing.Size(834, 175);
            this.textureDirsDataGrid.TabIndex = 0;
            // 
            // deleteButtons
            // 
            this.deleteButtons.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.deleteButtons.HeaderText = "";
            this.deleteButtons.MinimumWidth = 24;
            this.deleteButtons.Name = "deleteButtons";
            this.deleteButtons.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.deleteButtons.Width = 24;
            // 
            // textureDirs
            // 
            this.textureDirs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.textureDirs.HeaderText = Local.LocalString("setting.directory.texture");
            this.textureDirs.Name = "textureDirs";
            this.textureDirs.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.textureDirs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // browseButtons
            // 
            this.browseButtons.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.browseButtons.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.browseButtons.HeaderText = "";
            this.browseButtons.MinimumWidth = 50;
            this.browseButtons.Name = "browseButtons";
            this.browseButtons.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.browseButtons.Width = 50;
            // 
            // modelDirsDataGrid
            // 
            this.modelDirsDataGrid.AllowUserToAddRows = false;
            this.modelDirsDataGrid.AllowUserToResizeColumns = false;
            this.modelDirsDataGrid.AllowUserToResizeRows = false;
            this.modelDirsDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.modelDirsDataGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.modelDirsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.modelDirsDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewButtonColumn1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewButtonColumn2});
            this.modelDirsDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.modelDirsDataGrid.Location = new System.Drawing.Point(3, 184);
            this.modelDirsDataGrid.Name = "modelDirsDataGrid";
            this.modelDirsDataGrid.RowHeadersVisible = false;
            this.modelDirsDataGrid.Size = new System.Drawing.Size(834, 176);
            this.modelDirsDataGrid.TabIndex = 1;
            // 
            // dataGridViewButtonColumn1
            // 
            this.dataGridViewButtonColumn1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataGridViewButtonColumn1.HeaderText = "";
            this.dataGridViewButtonColumn1.MinimumWidth = 24;
            this.dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            this.dataGridViewButtonColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewButtonColumn1.Width = 24;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = Local.LocalString("setting.directory.model");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewButtonColumn2
            // 
            this.dataGridViewButtonColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.dataGridViewButtonColumn2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dataGridViewButtonColumn2.HeaderText = "";
            this.dataGridViewButtonColumn2.MinimumWidth = 50;
            this.dataGridViewButtonColumn2.Name = "dataGridViewButtonColumn2";
            this.dataGridViewButtonColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewButtonColumn2.Width = 50;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(879, 444);
            this.Controls.Add(this.btnApplySettings);
            this.Controls.Add(this.btnApplyAndCloseSettings);
            this.Controls.Add(this.btnCancelSettings);
            this.Controls.Add(this.tbcSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(780, 446);
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = Local.LocalString("setting");
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SettingsFormClosed);
            this.Load += new System.EventHandler(this.SettingsFormLoad);
            this.groupBox10.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            this.groupBox8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.tabHotkeys.ResumeLayout(false);
            this.groupBox22.ResumeLayout(false);
            this.groupBox22.PerformLayout();
            this.tab3DViews.ResumeLayout(false);
            this.groupBox12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CameraFOV)).EndInit();
            this.groupBox13.ResumeLayout(false);
            this.groupBox13.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TimeToTopSpeedUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ForwardSpeedUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MouseWheelMoveDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TimeToTopSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ForwardSpeed)).EndInit();
            this.groupBox14.ResumeLayout(false);
            this.groupBox14.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DetailRenderDistanceUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelRenderDistanceUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackClippingPlaneUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DetailRenderDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ModelRenderDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BackClippingPane)).EndInit();
            this.tab2DViews.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ColourPresetPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridHighlight1Distance)).EndInit();
            this.groupBox17.ResumeLayout(false);
            this.groupBox16.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.NudgeUnits)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.HideGridLimit)).EndInit();
            this.tabGeneral.ResumeLayout(false);
            this.tbcSettings.ResumeLayout(false);
            this.tabDirectories.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.textureDirsDataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.modelDirsDataGrid)).EndInit();
            this.ResumeLayout(false);

		}

        private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox15;
		private System.Windows.Forms.Button btnCancelSettings;
		private System.Windows.Forms.Button btnApplyAndCloseSettings;
		private System.Windows.Forms.Button btnApplySettings;
		private System.Windows.Forms.CheckBox checkBox5;
		private System.Windows.Forms.CheckBox checkBox6;
		private System.Windows.Forms.GroupBox groupBox10;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TrackBar trackBar1;
		private System.Windows.Forms.GroupBox groupBox7;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TrackBar trackBar2;
		private System.Windows.Forms.GroupBox groupBox8;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TrackBar trackBar3;
		private System.Windows.Forms.GroupBox groupBox9;
		private System.Windows.Forms.Label lblConfigSteamDir;
		private System.Windows.Forms.Button btnConfigListSteamGames;
		private System.Windows.Forms.Button btnConfigSteamDirBrowse;
		private System.Windows.Forms.ComboBox cmbConfigSteamUser;
		private System.Windows.Forms.Label lblConfigSteamUser;
        private System.Windows.Forms.ToolTip HelpTooltip;
        private System.Windows.Forms.TabPage tabHotkeys;
        private System.Windows.Forms.Button HotkeyResetButton;
        private System.Windows.Forms.GroupBox groupBox22;
        private System.Windows.Forms.ComboBox HotkeyActionList;
        private System.Windows.Forms.Button HotkeyAddButton;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox HotkeyCombination;
        private System.Windows.Forms.Button HotkeyReassignButton;
        private System.Windows.Forms.Button HotkeyRemoveButton;
        private System.Windows.Forms.ListView HotkeyList;
        private System.Windows.Forms.ColumnHeader chAction;
        private System.Windows.Forms.ColumnHeader chDescription;
        private System.Windows.Forms.ColumnHeader ckKeyCombo;
        private System.Windows.Forms.TabPage tab3DViews;
        private System.Windows.Forms.GroupBox groupBox12;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Panel ViewportBackgroundColour;
        private System.Windows.Forms.NumericUpDown CameraFOV;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.GroupBox groupBox13;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.NumericUpDown TimeToTopSpeedUpDown;
        private System.Windows.Forms.NumericUpDown ForwardSpeedUpDown;
        private System.Windows.Forms.NumericUpDown MouseWheelMoveDistance;
        private System.Windows.Forms.CheckBox InvertMouseX;
        private System.Windows.Forms.CheckBox InvertMouseY;
        private System.Windows.Forms.TrackBar TimeToTopSpeed;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TrackBar ForwardSpeed;
        private System.Windows.Forms.GroupBox groupBox14;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.NumericUpDown DetailRenderDistanceUpDown;
        private System.Windows.Forms.NumericUpDown ModelRenderDistanceUpDown;
        private System.Windows.Forms.NumericUpDown BackClippingPlaneUpDown;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TrackBar DetailRenderDistance;
        private System.Windows.Forms.TrackBar ModelRenderDistance;
        private System.Windows.Forms.TrackBar BackClippingPane;
        private System.Windows.Forms.TabPage tab2DViews;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.RadioButton RotationStyle_SnapNever;
        private System.Windows.Forms.RadioButton RotationStyle_SnapOnShift;
        private System.Windows.Forms.RadioButton RotationStyle_SnapOffShift;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.FlowLayoutPanel ColourPresetPanel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel GridHighlight2Colour;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel GridBackgroundColour;
        private System.Windows.Forms.DomainUpDown GridHighlight2UnitNum;
        private System.Windows.Forms.Panel GridBoundaryColour;
        private System.Windows.Forms.Panel GridZeroAxisColour;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.CheckBox GridHighlight1On;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox GridHighlight2On;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel GridHighlight1Colour;
        private System.Windows.Forms.Panel GridColour;
        private System.Windows.Forms.NumericUpDown GridHighlight1Distance;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox17;
        private System.Windows.Forms.CheckBox DrawEntityAngles;
        private System.Windows.Forms.CheckBox DrawEntityNames;
        private System.Windows.Forms.CheckBox CrosshairCursorIn2DViews;
        private System.Windows.Forms.GroupBox groupBox16;
        private System.Windows.Forms.CheckBox ArrowKeysNudgeSelection;
        private System.Windows.Forms.RadioButton NudgeStyle_GridOnCtrl;
        private System.Windows.Forms.RadioButton NudgeStyle_GridOffCtrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NudgeUnits;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton SnapStyle_SnapOffAlt;
        private System.Windows.Forms.RadioButton SnapStyle_SnapOnAlt;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox HideGridOn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown HideGridLimit;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DomainUpDown HideGridFactor;
        private System.Windows.Forms.DomainUpDown DefaultGridSize;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TabControl tbcSettings;
        private System.Windows.Forms.TabPage tabDirectories;
        private System.Windows.Forms.DataGridView modelDirsDataGrid;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView textureDirsDataGrid;
        private System.Windows.Forms.DataGridViewButtonColumn deleteButtons;
        private System.Windows.Forms.DataGridViewTextBoxColumn textureDirs;
        private System.Windows.Forms.DataGridViewButtonColumn browseButtons;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn2;
    }
}
