/*
 * Copyright (c). 2024 - 2025 Daniel Patterson, MCSD (danielanywhere).
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 * 
 */

namespace ShopTools
{
	partial class frmMain
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			lblSuggestion = new System.Windows.Forms.Label();
			mnuMain = new System.Windows.Forms.MenuStrip();
			mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileOpen = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileSave = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileSaveAs = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileNewCutList = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileSep1 = new System.Windows.Forms.ToolStripSeparator();
			mnuFileImport = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileImportConfiguration = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileImportPatterns = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileExport = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileExportGCode = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileExportSep1 = new System.Windows.Forms.ToolStripSeparator();
			mnuFileExportConfiguration = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileExportPatterns = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileSep2 = new System.Windows.Forms.ToolStripSeparator();
			mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
			mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
			mnuEditTemplates = new System.Windows.Forms.ToolStripMenuItem();
			mnuEditSettings = new System.Windows.Forms.ToolStripMenuItem();
			mnuView = new System.Windows.Forms.ToolStripMenuItem();
			mnuView3D = new System.Windows.Forms.ToolStripMenuItem();
			mnuViewGCode = new System.Windows.Forms.ToolStripMenuItem();
			mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
			mnuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
			statusMain = new System.Windows.Forms.StatusStrip();
			statMessage = new System.Windows.Forms.ToolStripStatusLabel();
			statFill = new System.Windows.Forms.ToolStripStatusLabel();
			statWorkspace = new System.Windows.Forms.ToolStripStatusLabel();
			pnlControl = new System.Windows.Forms.Panel();
			lvPatterns = new System.Windows.Forms.ListView();
			ilPatterns = new System.Windows.Forms.ImageList(components);
			lblPatterns = new System.Windows.Forms.Label();
			splitControl = new System.Windows.Forms.Splitter();
			pnlWorkspace = new System.Windows.Forms.Panel();
			pnlWorkpiece = new System.Windows.Forms.Panel();
			cmboMaterialType = new System.Windows.Forms.ComboBox();
			lblMaterialType = new System.Windows.Forms.Label();
			lvCutList = new System.Windows.Forms.ListView();
			lvCutListUnusedHeader = new System.Windows.Forms.ColumnHeader();
			ilPatternsSmall = new System.Windows.Forms.ImageList(components);
			btnDeleteCut = new System.Windows.Forms.Button();
			btnDuplicateCut = new System.Windows.Forms.Button();
			btnMoveCutDown = new System.Windows.Forms.Button();
			ilIcons = new System.Windows.Forms.ImageList(components);
			btnMoveCutUp = new System.Windows.Forms.Button();
			btnEditCut = new System.Windows.Forms.Button();
			btnStop = new System.Windows.Forms.Button();
			btnGO = new System.Windows.Forms.Button();
			cmboWorkpieceY = new System.Windows.Forms.ComboBox();
			cmboWorkpieceX = new System.Windows.Forms.ComboBox();
			txtWorkpieceDepth = new System.Windows.Forms.TextBox();
			lblWorkpieceDepthUnit = new System.Windows.Forms.Label();
			txtWorkpieceWidth = new System.Windows.Forms.TextBox();
			lblWorkpieceWidthUnit = new System.Windows.Forms.Label();
			txtRouterPositionY = new System.Windows.Forms.TextBox();
			txtWorkpieceY = new System.Windows.Forms.TextBox();
			txtRouterPositionX = new System.Windows.Forms.TextBox();
			txtWorkpieceX = new System.Windows.Forms.TextBox();
			lblRouterPositionYUnit = new System.Windows.Forms.Label();
			txtWorkpieceLength = new System.Windows.Forms.TextBox();
			lblWorkpieceYUnit = new System.Windows.Forms.Label();
			lblWorkpieceDepth = new System.Windows.Forms.Label();
			lblRouterPositionXUnit = new System.Windows.Forms.Label();
			lblWorkpieceWidth = new System.Windows.Forms.Label();
			lblRouterPositionY = new System.Windows.Forms.Label();
			lblWorkpieceXUnit = new System.Windows.Forms.Label();
			lblWorkpieceY = new System.Windows.Forms.Label();
			lblRouterPositionX = new System.Windows.Forms.Label();
			lblWorkpieceLengthUnit = new System.Windows.Forms.Label();
			lblWorkpieceX = new System.Windows.Forms.Label();
			lblWorkpieceLength = new System.Windows.Forms.Label();
			lblCutList = new System.Windows.Forms.Label();
			lblRouterPosition = new System.Windows.Forms.Label();
			lblWorkpiece = new System.Windows.Forms.Label();
			splitWorkpiece = new System.Windows.Forms.Splitter();
			mnuFileImportSvgDrawing = new System.Windows.Forms.ToolStripMenuItem();
			mnuMain.SuspendLayout();
			statusMain.SuspendLayout();
			pnlControl.SuspendLayout();
			pnlWorkpiece.SuspendLayout();
			SuspendLayout();
			// 
			// lblSuggestion
			// 
			lblSuggestion.ForeColor = System.Drawing.SystemColors.ButtonShadow;
			lblSuggestion.Location = new System.Drawing.Point(0, 0);
			lblSuggestion.Name = "lblSuggestion";
			lblSuggestion.Size = new System.Drawing.Size(267, 58);
			lblSuggestion.TabIndex = 0;
			lblSuggestion.Text = "Use your CNC router to perform REGULAR everyday power tool tasks.";
			// 
			// mnuMain
			// 
			mnuMain.ImageScalingSize = new System.Drawing.Size(20, 20);
			mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuFile, mnuEdit, mnuView, mnuHelp });
			mnuMain.Location = new System.Drawing.Point(0, 0);
			mnuMain.Name = "mnuMain";
			mnuMain.Size = new System.Drawing.Size(925, 28);
			mnuMain.TabIndex = 1;
			mnuMain.Text = "menuStrip1";
			// 
			// mnuFile
			// 
			mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuFileOpen, mnuFileSave, mnuFileSaveAs, mnuFileNewCutList, mnuFileSep1, mnuFileImport, mnuFileExport, mnuFileSep2, mnuFileExit });
			mnuFile.Name = "mnuFile";
			mnuFile.Size = new System.Drawing.Size(46, 24);
			mnuFile.Text = "&File";
			// 
			// mnuFileOpen
			// 
			mnuFileOpen.Name = "mnuFileOpen";
			mnuFileOpen.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O;
			mnuFileOpen.Size = new System.Drawing.Size(235, 26);
			mnuFileOpen.Text = "&Open Cut-List";
			// 
			// mnuFileSave
			// 
			mnuFileSave.Name = "mnuFileSave";
			mnuFileSave.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;
			mnuFileSave.Size = new System.Drawing.Size(235, 26);
			mnuFileSave.Text = "&Save Cut-List";
			// 
			// mnuFileSaveAs
			// 
			mnuFileSaveAs.Name = "mnuFileSaveAs";
			mnuFileSaveAs.Size = new System.Drawing.Size(235, 26);
			mnuFileSaveAs.Text = "Save Cut-List &As";
			// 
			// mnuFileNewCutList
			// 
			mnuFileNewCutList.Name = "mnuFileNewCutList";
			mnuFileNewCutList.ShortcutKeys = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N;
			mnuFileNewCutList.Size = new System.Drawing.Size(235, 26);
			mnuFileNewCutList.Text = "&New Cut-List";
			// 
			// mnuFileSep1
			// 
			mnuFileSep1.Name = "mnuFileSep1";
			mnuFileSep1.Size = new System.Drawing.Size(232, 6);
			// 
			// mnuFileImport
			// 
			mnuFileImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuFileImportConfiguration, mnuFileImportPatterns, mnuFileImportSvgDrawing });
			mnuFileImport.Name = "mnuFileImport";
			mnuFileImport.Size = new System.Drawing.Size(235, 26);
			mnuFileImport.Text = "&Import";
			// 
			// mnuFileImportConfiguration
			// 
			mnuFileImportConfiguration.Name = "mnuFileImportConfiguration";
			mnuFileImportConfiguration.Size = new System.Drawing.Size(224, 26);
			mnuFileImportConfiguration.Text = "&Configuration";
			// 
			// mnuFileImportPatterns
			// 
			mnuFileImportPatterns.Name = "mnuFileImportPatterns";
			mnuFileImportPatterns.Size = new System.Drawing.Size(224, 26);
			mnuFileImportPatterns.Text = "&Patterns";
			// 
			// mnuFileExport
			// 
			mnuFileExport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuFileExportGCode, mnuFileExportSep1, mnuFileExportConfiguration, mnuFileExportPatterns });
			mnuFileExport.Name = "mnuFileExport";
			mnuFileExport.Size = new System.Drawing.Size(235, 26);
			mnuFileExport.Text = "&Export";
			// 
			// mnuFileExportGCode
			// 
			mnuFileExportGCode.Name = "mnuFileExportGCode";
			mnuFileExportGCode.Size = new System.Drawing.Size(183, 26);
			mnuFileExportGCode.Text = "&G-code";
			// 
			// mnuFileExportSep1
			// 
			mnuFileExportSep1.Name = "mnuFileExportSep1";
			mnuFileExportSep1.Size = new System.Drawing.Size(180, 6);
			// 
			// mnuFileExportConfiguration
			// 
			mnuFileExportConfiguration.Name = "mnuFileExportConfiguration";
			mnuFileExportConfiguration.Size = new System.Drawing.Size(183, 26);
			mnuFileExportConfiguration.Text = "&Configuration";
			// 
			// mnuFileExportPatterns
			// 
			mnuFileExportPatterns.Name = "mnuFileExportPatterns";
			mnuFileExportPatterns.Size = new System.Drawing.Size(183, 26);
			mnuFileExportPatterns.Text = "&Patterns";
			// 
			// mnuFileSep2
			// 
			mnuFileSep2.Name = "mnuFileSep2";
			mnuFileSep2.Size = new System.Drawing.Size(232, 6);
			// 
			// mnuFileExit
			// 
			mnuFileExit.Name = "mnuFileExit";
			mnuFileExit.Size = new System.Drawing.Size(235, 26);
			mnuFileExit.Text = "E&xit";
			// 
			// mnuEdit
			// 
			mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuEditTemplates, mnuEditSettings });
			mnuEdit.Name = "mnuEdit";
			mnuEdit.Size = new System.Drawing.Size(49, 24);
			mnuEdit.Text = "&Edit";
			// 
			// mnuEditTemplates
			// 
			mnuEditTemplates.Name = "mnuEditTemplates";
			mnuEditTemplates.Size = new System.Drawing.Size(160, 26);
			mnuEditTemplates.Text = "&Templates";
			// 
			// mnuEditSettings
			// 
			mnuEditSettings.Name = "mnuEditSettings";
			mnuEditSettings.Size = new System.Drawing.Size(160, 26);
			mnuEditSettings.Text = "&Settings";
			// 
			// mnuView
			// 
			mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuView3D, mnuViewGCode });
			mnuView.Name = "mnuView";
			mnuView.Size = new System.Drawing.Size(55, 24);
			mnuView.Text = "&View";
			// 
			// mnuView3D
			// 
			mnuView3D.Name = "mnuView3D";
			mnuView3D.Size = new System.Drawing.Size(166, 26);
			mnuView3D.Text = "&3D Preview";
			// 
			// mnuViewGCode
			// 
			mnuViewGCode.Name = "mnuViewGCode";
			mnuViewGCode.Size = new System.Drawing.Size(166, 26);
			mnuViewGCode.Text = "&G-code";
			// 
			// mnuHelp
			// 
			mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuHelpAbout });
			mnuHelp.Name = "mnuHelp";
			mnuHelp.Size = new System.Drawing.Size(55, 24);
			mnuHelp.Text = "&Help";
			// 
			// mnuHelpAbout
			// 
			mnuHelpAbout.Name = "mnuHelpAbout";
			mnuHelpAbout.Size = new System.Drawing.Size(133, 26);
			mnuHelpAbout.Text = "&About";
			// 
			// statusMain
			// 
			statusMain.ImageScalingSize = new System.Drawing.Size(20, 20);
			statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { statMessage, statFill, statWorkspace });
			statusMain.Location = new System.Drawing.Point(0, 424);
			statusMain.Name = "statusMain";
			statusMain.Size = new System.Drawing.Size(925, 26);
			statusMain.TabIndex = 2;
			statusMain.Text = "statusStrip1";
			// 
			// statMessage
			// 
			statMessage.Name = "statMessage";
			statMessage.Size = new System.Drawing.Size(59, 20);
			statMessage.Text = "Ready...";
			// 
			// statFill
			// 
			statFill.Name = "statFill";
			statFill.Size = new System.Drawing.Size(734, 20);
			statFill.Spring = true;
			// 
			// statWorkspace
			// 
			statWorkspace.Name = "statWorkspace";
			statWorkspace.Size = new System.Drawing.Size(117, 20);
			statWorkspace.Text = "Workspace: 2'x2'";
			// 
			// pnlControl
			// 
			pnlControl.BackColor = System.Drawing.Color.White;
			pnlControl.Controls.Add(lvPatterns);
			pnlControl.Controls.Add(lblPatterns);
			pnlControl.Dock = System.Windows.Forms.DockStyle.Left;
			pnlControl.Location = new System.Drawing.Point(0, 28);
			pnlControl.Name = "pnlControl";
			pnlControl.Size = new System.Drawing.Size(216, 396);
			pnlControl.TabIndex = 3;
			// 
			// lvPatterns
			// 
			lvPatterns.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lvPatterns.LargeImageList = ilPatterns;
			lvPatterns.Location = new System.Drawing.Point(3, 36);
			lvPatterns.Name = "lvPatterns";
			lvPatterns.Size = new System.Drawing.Size(207, 344);
			lvPatterns.TabIndex = 1;
			lvPatterns.UseCompatibleStateImageBehavior = false;
			// 
			// ilPatterns
			// 
			ilPatterns.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			ilPatterns.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ilPatterns.ImageStream");
			ilPatterns.TransparentColor = System.Drawing.Color.Transparent;
			ilPatterns.Images.SetKeyName(0, "NoImageIcon.png");
			// 
			// lblPatterns
			// 
			lblPatterns.AutoSize = true;
			lblPatterns.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblPatterns.Location = new System.Drawing.Point(3, 10);
			lblPatterns.Name = "lblPatterns";
			lblPatterns.Size = new System.Drawing.Size(76, 23);
			lblPatterns.TabIndex = 0;
			lblPatterns.Text = "Patterns";
			// 
			// splitControl
			// 
			splitControl.BackColor = System.Drawing.Color.White;
			splitControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			splitControl.Location = new System.Drawing.Point(216, 28);
			splitControl.Name = "splitControl";
			splitControl.Size = new System.Drawing.Size(8, 396);
			splitControl.TabIndex = 4;
			splitControl.TabStop = false;
			// 
			// pnlWorkspace
			// 
			pnlWorkspace.BackColor = System.Drawing.Color.FromArgb(17, 51, 102);
			pnlWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
			pnlWorkspace.Location = new System.Drawing.Point(224, 28);
			pnlWorkspace.Name = "pnlWorkspace";
			pnlWorkspace.Size = new System.Drawing.Size(403, 396);
			pnlWorkspace.TabIndex = 5;
			// 
			// pnlWorkpiece
			// 
			pnlWorkpiece.AutoScroll = true;
			pnlWorkpiece.AutoScrollMargin = new System.Drawing.Size(16, 16);
			pnlWorkpiece.BackColor = System.Drawing.Color.White;
			pnlWorkpiece.Controls.Add(cmboMaterialType);
			pnlWorkpiece.Controls.Add(lblMaterialType);
			pnlWorkpiece.Controls.Add(lvCutList);
			pnlWorkpiece.Controls.Add(btnDeleteCut);
			pnlWorkpiece.Controls.Add(btnDuplicateCut);
			pnlWorkpiece.Controls.Add(btnMoveCutDown);
			pnlWorkpiece.Controls.Add(btnMoveCutUp);
			pnlWorkpiece.Controls.Add(btnEditCut);
			pnlWorkpiece.Controls.Add(btnStop);
			pnlWorkpiece.Controls.Add(btnGO);
			pnlWorkpiece.Controls.Add(lblSuggestion);
			pnlWorkpiece.Controls.Add(cmboWorkpieceY);
			pnlWorkpiece.Controls.Add(cmboWorkpieceX);
			pnlWorkpiece.Controls.Add(txtWorkpieceDepth);
			pnlWorkpiece.Controls.Add(lblWorkpieceDepthUnit);
			pnlWorkpiece.Controls.Add(txtWorkpieceWidth);
			pnlWorkpiece.Controls.Add(lblWorkpieceWidthUnit);
			pnlWorkpiece.Controls.Add(txtRouterPositionY);
			pnlWorkpiece.Controls.Add(txtWorkpieceY);
			pnlWorkpiece.Controls.Add(txtRouterPositionX);
			pnlWorkpiece.Controls.Add(txtWorkpieceX);
			pnlWorkpiece.Controls.Add(lblRouterPositionYUnit);
			pnlWorkpiece.Controls.Add(txtWorkpieceLength);
			pnlWorkpiece.Controls.Add(lblWorkpieceYUnit);
			pnlWorkpiece.Controls.Add(lblWorkpieceDepth);
			pnlWorkpiece.Controls.Add(lblRouterPositionXUnit);
			pnlWorkpiece.Controls.Add(lblWorkpieceWidth);
			pnlWorkpiece.Controls.Add(lblRouterPositionY);
			pnlWorkpiece.Controls.Add(lblWorkpieceXUnit);
			pnlWorkpiece.Controls.Add(lblWorkpieceY);
			pnlWorkpiece.Controls.Add(lblRouterPositionX);
			pnlWorkpiece.Controls.Add(lblWorkpieceLengthUnit);
			pnlWorkpiece.Controls.Add(lblWorkpieceX);
			pnlWorkpiece.Controls.Add(lblWorkpieceLength);
			pnlWorkpiece.Controls.Add(lblCutList);
			pnlWorkpiece.Controls.Add(lblRouterPosition);
			pnlWorkpiece.Controls.Add(lblWorkpiece);
			pnlWorkpiece.Dock = System.Windows.Forms.DockStyle.Right;
			pnlWorkpiece.Location = new System.Drawing.Point(635, 28);
			pnlWorkpiece.Name = "pnlWorkpiece";
			pnlWorkpiece.Size = new System.Drawing.Size(290, 396);
			pnlWorkpiece.TabIndex = 6;
			// 
			// cmboMaterialType
			// 
			cmboMaterialType.FormattingEnabled = true;
			cmboMaterialType.Location = new System.Drawing.Point(83, 95);
			cmboMaterialType.Name = "cmboMaterialType";
			cmboMaterialType.Size = new System.Drawing.Size(151, 28);
			cmboMaterialType.TabIndex = 3;
			// 
			// lblMaterialType
			// 
			lblMaterialType.AutoSize = true;
			lblMaterialType.Location = new System.Drawing.Point(14, 100);
			lblMaterialType.Name = "lblMaterialType";
			lblMaterialType.Size = new System.Drawing.Size(67, 20);
			lblMaterialType.TabIndex = 2;
			lblMaterialType.Text = "Material:";
			// 
			// lvCutList
			// 
			lvCutList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { lvCutListUnusedHeader });
			lvCutList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			lvCutList.LargeImageList = ilPatterns;
			lvCutList.Location = new System.Drawing.Point(6, 557);
			lvCutList.Name = "lvCutList";
			lvCutList.Size = new System.Drawing.Size(248, 186);
			lvCutList.SmallImageList = ilPatternsSmall;
			lvCutList.TabIndex = 29;
			lvCutList.UseCompatibleStateImageBehavior = false;
			lvCutList.View = System.Windows.Forms.View.Details;
			// 
			// lvCutListUnusedHeader
			// 
			lvCutListUnusedHeader.Text = "";
			// 
			// ilPatternsSmall
			// 
			ilPatternsSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			ilPatternsSmall.ImageSize = new System.Drawing.Size(24, 24);
			ilPatternsSmall.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// btnDeleteCut
			// 
			btnDeleteCut.Location = new System.Drawing.Point(6, 784);
			btnDeleteCut.Name = "btnDeleteCut";
			btnDeleteCut.Size = new System.Drawing.Size(94, 29);
			btnDeleteCut.TabIndex = 31;
			btnDeleteCut.Text = "De&lete Cut";
			btnDeleteCut.UseVisualStyleBackColor = true;
			// 
			// btnDuplicateCut
			// 
			btnDuplicateCut.Location = new System.Drawing.Point(160, 749);
			btnDuplicateCut.Name = "btnDuplicateCut";
			btnDuplicateCut.Size = new System.Drawing.Size(94, 29);
			btnDuplicateCut.TabIndex = 34;
			btnDuplicateCut.Text = "&Duplicate";
			btnDuplicateCut.UseVisualStyleBackColor = true;
			// 
			// btnMoveCutDown
			// 
			btnMoveCutDown.ImageIndex = 1;
			btnMoveCutDown.ImageList = ilIcons;
			btnMoveCutDown.Location = new System.Drawing.Point(110, 795);
			btnMoveCutDown.Name = "btnMoveCutDown";
			btnMoveCutDown.Size = new System.Drawing.Size(40, 40);
			btnMoveCutDown.TabIndex = 33;
			btnMoveCutDown.UseVisualStyleBackColor = true;
			// 
			// ilIcons
			// 
			ilIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			ilIcons.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ilIcons.ImageStream");
			ilIcons.TransparentColor = System.Drawing.Color.Transparent;
			ilIcons.Images.SetKeyName(0, "MoveUp.png");
			ilIcons.Images.SetKeyName(1, "MoveDown.png");
			// 
			// btnMoveCutUp
			// 
			btnMoveCutUp.ImageIndex = 0;
			btnMoveCutUp.ImageList = ilIcons;
			btnMoveCutUp.Location = new System.Drawing.Point(110, 749);
			btnMoveCutUp.Name = "btnMoveCutUp";
			btnMoveCutUp.Size = new System.Drawing.Size(40, 40);
			btnMoveCutUp.TabIndex = 32;
			btnMoveCutUp.UseVisualStyleBackColor = true;
			// 
			// btnEditCut
			// 
			btnEditCut.Location = new System.Drawing.Point(6, 749);
			btnEditCut.Name = "btnEditCut";
			btnEditCut.Size = new System.Drawing.Size(94, 29);
			btnEditCut.TabIndex = 30;
			btnEditCut.Text = "Edit &Cut";
			btnEditCut.UseVisualStyleBackColor = true;
			// 
			// btnStop
			// 
			btnStop.Location = new System.Drawing.Point(160, 818);
			btnStop.Name = "btnStop";
			btnStop.Size = new System.Drawing.Size(94, 29);
			btnStop.TabIndex = 36;
			btnStop.Text = "&Stop";
			btnStop.UseVisualStyleBackColor = true;
			// 
			// btnGO
			// 
			btnGO.Location = new System.Drawing.Point(160, 784);
			btnGO.Name = "btnGO";
			btnGO.Size = new System.Drawing.Size(94, 29);
			btnGO.TabIndex = 35;
			btnGO.Text = "&GO";
			btnGO.UseVisualStyleBackColor = true;
			// 
			// cmboWorkpieceY
			// 
			cmboWorkpieceY.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cmboWorkpieceY.FormattingEnabled = true;
			cmboWorkpieceY.Items.AddRange(new object[] { "From Top", "Top Edge To Center", "Center To Center", "Bottom Edge To Center", "From Bottom" });
			cmboWorkpieceY.Location = new System.Drawing.Point(83, 347);
			cmboWorkpieceY.Name = "cmboWorkpieceY";
			cmboWorkpieceY.Size = new System.Drawing.Size(151, 28);
			cmboWorkpieceY.TabIndex = 20;
			// 
			// cmboWorkpieceX
			// 
			cmboWorkpieceX.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			cmboWorkpieceX.FormattingEnabled = true;
			cmboWorkpieceX.Items.AddRange(new object[] { "From Left", "Left Edge To Center", "Center To Center", "Right Edge To Center", "From Right" });
			cmboWorkpieceX.Location = new System.Drawing.Point(83, 280);
			cmboWorkpieceX.Name = "cmboWorkpieceX";
			cmboWorkpieceX.Size = new System.Drawing.Size(151, 28);
			cmboWorkpieceX.TabIndex = 16;
			// 
			// txtWorkpieceDepth
			// 
			txtWorkpieceDepth.Location = new System.Drawing.Point(83, 196);
			txtWorkpieceDepth.Name = "txtWorkpieceDepth";
			txtWorkpieceDepth.Size = new System.Drawing.Size(113, 27);
			txtWorkpieceDepth.TabIndex = 11;
			// 
			// lblWorkpieceDepthUnit
			// 
			lblWorkpieceDepthUnit.AutoSize = true;
			lblWorkpieceDepthUnit.Location = new System.Drawing.Point(202, 199);
			lblWorkpieceDepthUnit.Name = "lblWorkpieceDepthUnit";
			lblWorkpieceDepthUnit.Size = new System.Drawing.Size(18, 20);
			lblWorkpieceDepthUnit.TabIndex = 12;
			lblWorkpieceDepthUnit.Text = "...";
			// 
			// txtWorkpieceWidth
			// 
			txtWorkpieceWidth.Location = new System.Drawing.Point(83, 163);
			txtWorkpieceWidth.Name = "txtWorkpieceWidth";
			txtWorkpieceWidth.Size = new System.Drawing.Size(113, 27);
			txtWorkpieceWidth.TabIndex = 8;
			// 
			// lblWorkpieceWidthUnit
			// 
			lblWorkpieceWidthUnit.AutoSize = true;
			lblWorkpieceWidthUnit.Location = new System.Drawing.Point(202, 166);
			lblWorkpieceWidthUnit.Name = "lblWorkpieceWidthUnit";
			lblWorkpieceWidthUnit.Size = new System.Drawing.Size(18, 20);
			lblWorkpieceWidthUnit.TabIndex = 9;
			lblWorkpieceWidthUnit.Text = "...";
			// 
			// txtRouterPositionY
			// 
			txtRouterPositionY.Location = new System.Drawing.Point(83, 463);
			txtRouterPositionY.Name = "txtRouterPositionY";
			txtRouterPositionY.Size = new System.Drawing.Size(113, 27);
			txtRouterPositionY.TabIndex = 26;
			// 
			// txtWorkpieceY
			// 
			txtWorkpieceY.Location = new System.Drawing.Point(83, 314);
			txtWorkpieceY.Name = "txtWorkpieceY";
			txtWorkpieceY.Size = new System.Drawing.Size(113, 27);
			txtWorkpieceY.TabIndex = 18;
			// 
			// txtRouterPositionX
			// 
			txtRouterPositionX.Location = new System.Drawing.Point(83, 430);
			txtRouterPositionX.Name = "txtRouterPositionX";
			txtRouterPositionX.Size = new System.Drawing.Size(113, 27);
			txtRouterPositionX.TabIndex = 23;
			// 
			// txtWorkpieceX
			// 
			txtWorkpieceX.Location = new System.Drawing.Point(83, 247);
			txtWorkpieceX.Name = "txtWorkpieceX";
			txtWorkpieceX.Size = new System.Drawing.Size(113, 27);
			txtWorkpieceX.TabIndex = 14;
			// 
			// lblRouterPositionYUnit
			// 
			lblRouterPositionYUnit.AutoSize = true;
			lblRouterPositionYUnit.Location = new System.Drawing.Point(202, 466);
			lblRouterPositionYUnit.Name = "lblRouterPositionYUnit";
			lblRouterPositionYUnit.Size = new System.Drawing.Size(18, 20);
			lblRouterPositionYUnit.TabIndex = 27;
			lblRouterPositionYUnit.Text = "...";
			// 
			// txtWorkpieceLength
			// 
			txtWorkpieceLength.Location = new System.Drawing.Point(83, 130);
			txtWorkpieceLength.Name = "txtWorkpieceLength";
			txtWorkpieceLength.Size = new System.Drawing.Size(113, 27);
			txtWorkpieceLength.TabIndex = 5;
			// 
			// lblWorkpieceYUnit
			// 
			lblWorkpieceYUnit.AutoSize = true;
			lblWorkpieceYUnit.Location = new System.Drawing.Point(202, 317);
			lblWorkpieceYUnit.Name = "lblWorkpieceYUnit";
			lblWorkpieceYUnit.Size = new System.Drawing.Size(18, 20);
			lblWorkpieceYUnit.TabIndex = 19;
			lblWorkpieceYUnit.Text = "...";
			// 
			// lblWorkpieceDepth
			// 
			lblWorkpieceDepth.AutoSize = true;
			lblWorkpieceDepth.Location = new System.Drawing.Point(14, 199);
			lblWorkpieceDepth.Name = "lblWorkpieceDepth";
			lblWorkpieceDepth.Size = new System.Drawing.Size(53, 20);
			lblWorkpieceDepth.TabIndex = 10;
			lblWorkpieceDepth.Text = "Depth:";
			// 
			// lblRouterPositionXUnit
			// 
			lblRouterPositionXUnit.AutoSize = true;
			lblRouterPositionXUnit.Location = new System.Drawing.Point(202, 433);
			lblRouterPositionXUnit.Name = "lblRouterPositionXUnit";
			lblRouterPositionXUnit.Size = new System.Drawing.Size(18, 20);
			lblRouterPositionXUnit.TabIndex = 24;
			lblRouterPositionXUnit.Text = "...";
			// 
			// lblWorkpieceWidth
			// 
			lblWorkpieceWidth.AutoSize = true;
			lblWorkpieceWidth.Location = new System.Drawing.Point(14, 166);
			lblWorkpieceWidth.Name = "lblWorkpieceWidth";
			lblWorkpieceWidth.Size = new System.Drawing.Size(52, 20);
			lblWorkpieceWidth.TabIndex = 7;
			lblWorkpieceWidth.Text = "Width:";
			// 
			// lblRouterPositionY
			// 
			lblRouterPositionY.AutoSize = true;
			lblRouterPositionY.Location = new System.Drawing.Point(51, 466);
			lblRouterPositionY.Name = "lblRouterPositionY";
			lblRouterPositionY.Size = new System.Drawing.Size(20, 20);
			lblRouterPositionY.TabIndex = 25;
			lblRouterPositionY.Text = "Y:";
			// 
			// lblWorkpieceXUnit
			// 
			lblWorkpieceXUnit.AutoSize = true;
			lblWorkpieceXUnit.Location = new System.Drawing.Point(202, 250);
			lblWorkpieceXUnit.Name = "lblWorkpieceXUnit";
			lblWorkpieceXUnit.Size = new System.Drawing.Size(18, 20);
			lblWorkpieceXUnit.TabIndex = 15;
			lblWorkpieceXUnit.Text = "...";
			// 
			// lblWorkpieceY
			// 
			lblWorkpieceY.AutoSize = true;
			lblWorkpieceY.Location = new System.Drawing.Point(14, 317);
			lblWorkpieceY.Name = "lblWorkpieceY";
			lblWorkpieceY.Size = new System.Drawing.Size(64, 20);
			lblWorkpieceY.TabIndex = 17;
			lblWorkpieceY.Text = "Y Offset:";
			// 
			// lblRouterPositionX
			// 
			lblRouterPositionX.AutoSize = true;
			lblRouterPositionX.Location = new System.Drawing.Point(51, 433);
			lblRouterPositionX.Name = "lblRouterPositionX";
			lblRouterPositionX.Size = new System.Drawing.Size(21, 20);
			lblRouterPositionX.TabIndex = 22;
			lblRouterPositionX.Text = "X:";
			// 
			// lblWorkpieceLengthUnit
			// 
			lblWorkpieceLengthUnit.AutoSize = true;
			lblWorkpieceLengthUnit.Location = new System.Drawing.Point(202, 133);
			lblWorkpieceLengthUnit.Name = "lblWorkpieceLengthUnit";
			lblWorkpieceLengthUnit.Size = new System.Drawing.Size(18, 20);
			lblWorkpieceLengthUnit.TabIndex = 6;
			lblWorkpieceLengthUnit.Text = "...";
			// 
			// lblWorkpieceX
			// 
			lblWorkpieceX.AutoSize = true;
			lblWorkpieceX.Location = new System.Drawing.Point(14, 250);
			lblWorkpieceX.Name = "lblWorkpieceX";
			lblWorkpieceX.Size = new System.Drawing.Size(65, 20);
			lblWorkpieceX.TabIndex = 13;
			lblWorkpieceX.Text = "X Offset:";
			// 
			// lblWorkpieceLength
			// 
			lblWorkpieceLength.AutoSize = true;
			lblWorkpieceLength.Location = new System.Drawing.Point(14, 133);
			lblWorkpieceLength.Name = "lblWorkpieceLength";
			lblWorkpieceLength.Size = new System.Drawing.Size(57, 20);
			lblWorkpieceLength.TabIndex = 4;
			lblWorkpieceLength.Text = "Length:";
			// 
			// lblCutList
			// 
			lblCutList.AutoSize = true;
			lblCutList.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblCutList.Location = new System.Drawing.Point(6, 531);
			lblCutList.Name = "lblCutList";
			lblCutList.Size = new System.Drawing.Size(71, 23);
			lblCutList.TabIndex = 28;
			lblCutList.Text = "Cut List";
			// 
			// lblRouterPosition
			// 
			lblRouterPosition.AutoSize = true;
			lblRouterPosition.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblRouterPosition.Location = new System.Drawing.Point(6, 404);
			lblRouterPosition.Name = "lblRouterPosition";
			lblRouterPosition.Size = new System.Drawing.Size(203, 23);
			lblRouterPosition.TabIndex = 21;
			lblRouterPosition.Text = "Starting Router Position";
			// 
			// lblWorkpiece
			// 
			lblWorkpiece.AutoSize = true;
			lblWorkpiece.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblWorkpiece.Location = new System.Drawing.Point(6, 70);
			lblWorkpiece.Name = "lblWorkpiece";
			lblWorkpiece.Size = new System.Drawing.Size(96, 23);
			lblWorkpiece.TabIndex = 1;
			lblWorkpiece.Text = "Workpiece";
			// 
			// splitWorkpiece
			// 
			splitWorkpiece.BackColor = System.Drawing.Color.White;
			splitWorkpiece.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			splitWorkpiece.Dock = System.Windows.Forms.DockStyle.Right;
			splitWorkpiece.Location = new System.Drawing.Point(627, 28);
			splitWorkpiece.Name = "splitWorkpiece";
			splitWorkpiece.Size = new System.Drawing.Size(8, 396);
			splitWorkpiece.TabIndex = 7;
			splitWorkpiece.TabStop = false;
			// 
			// mnuFileImportSvgDrawing
			// 
			mnuFileImportSvgDrawing.Name = "mnuFileImportSvgDrawing";
			mnuFileImportSvgDrawing.Size = new System.Drawing.Size(224, 26);
			mnuFileImportSvgDrawing.Text = "&SVG Drawing";
			// 
			// frmMain
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(925, 450);
			Controls.Add(pnlWorkspace);
			Controls.Add(splitWorkpiece);
			Controls.Add(pnlWorkpiece);
			Controls.Add(splitControl);
			Controls.Add(pnlControl);
			Controls.Add(statusMain);
			Controls.Add(mnuMain);
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			MainMenuStrip = mnuMain;
			Name = "frmMain";
			Text = "Dan's ShopTools";
			mnuMain.ResumeLayout(false);
			mnuMain.PerformLayout();
			statusMain.ResumeLayout(false);
			statusMain.PerformLayout();
			pnlControl.ResumeLayout(false);
			pnlControl.PerformLayout();
			pnlWorkpiece.ResumeLayout(false);
			pnlWorkpiece.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label lblSuggestion;
		private System.Windows.Forms.MenuStrip mnuMain;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.StatusStrip statusMain;
		private System.Windows.Forms.ToolStripStatusLabel statMessage;
		private System.Windows.Forms.Panel pnlControl;
		private System.Windows.Forms.Splitter splitControl;
		private System.Windows.Forms.ToolStripStatusLabel statFill;
		private System.Windows.Forms.ToolStripStatusLabel statWorkspace;
		private System.Windows.Forms.Panel pnlWorkspace;
		private System.Windows.Forms.ToolStripMenuItem mnuEdit;
		private System.Windows.Forms.ToolStripMenuItem mnuEditSettings;
		private System.Windows.Forms.Panel pnlWorkpiece;
		private System.Windows.Forms.Label lblWorkpiece;
		private System.Windows.Forms.Label lblWorkpieceLength;
		private System.Windows.Forms.TextBox txtWorkpieceWidth;
		private System.Windows.Forms.Label lblWorkpieceWidthUnit;
		private System.Windows.Forms.TextBox txtWorkpieceLength;
		private System.Windows.Forms.Label lblWorkpieceWidth;
		private System.Windows.Forms.Label lblWorkpieceLengthUnit;
		private System.Windows.Forms.TextBox txtWorkpieceY;
		private System.Windows.Forms.TextBox txtWorkpieceX;
		private System.Windows.Forms.Label lblWorkpieceYUnit;
		private System.Windows.Forms.Label lblWorkpieceXUnit;
		private System.Windows.Forms.Label lblWorkpieceY;
		private System.Windows.Forms.Label lblWorkpieceX;
		private System.Windows.Forms.ComboBox cmboWorkpieceY;
		private System.Windows.Forms.ComboBox cmboWorkpieceX;
		private System.Windows.Forms.TextBox txtRouterPositionY;
		private System.Windows.Forms.TextBox txtRouterPositionX;
		private System.Windows.Forms.Label lblRouterPositionYUnit;
		private System.Windows.Forms.Label lblRouterPositionXUnit;
		private System.Windows.Forms.Label lblRouterPositionY;
		private System.Windows.Forms.Label lblRouterPositionX;
		private System.Windows.Forms.Label lblRouterPosition;
		private System.Windows.Forms.Splitter splitWorkpiece;
		private System.Windows.Forms.TextBox txtWorkpieceDepth;
		private System.Windows.Forms.Label lblWorkpieceDepthUnit;
		private System.Windows.Forms.Label lblWorkpieceDepth;
		private System.Windows.Forms.ListView lvPatterns;
		private System.Windows.Forms.Label lblPatterns;
		private System.Windows.Forms.ImageList ilPatterns;
		private System.Windows.Forms.ListView lvCutList;
		private System.Windows.Forms.Button btnGO;
		private System.Windows.Forms.Label lblCutList;
		private System.Windows.Forms.Button btnDeleteCut;
		private System.Windows.Forms.Button btnEditCut;
		private System.Windows.Forms.ColumnHeader lvCutListUnusedHeader;
		private System.Windows.Forms.ImageList ilPatternsSmall;
		private System.Windows.Forms.ToolStripMenuItem mnuFileImport;
		private System.Windows.Forms.ToolStripMenuItem mnuFileImportConfiguration;
		private System.Windows.Forms.ToolStripMenuItem mnuFileExport;
		private System.Windows.Forms.ToolStripMenuItem mnuFileExportGCode;
		private System.Windows.Forms.ToolStripSeparator mnuFileExportSep1;
		private System.Windows.Forms.ToolStripMenuItem mnuFileExportConfiguration;
		private System.Windows.Forms.ToolStripSeparator mnuFileSep1;
		private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
		private System.Windows.Forms.Button btnDuplicateCut;
		private System.Windows.Forms.ToolStripMenuItem mnuFileOpen;
		private System.Windows.Forms.ToolStripMenuItem mnuFileSave;
		private System.Windows.Forms.ToolStripMenuItem mnuFileSaveAs;
		private System.Windows.Forms.ToolStripSeparator mnuFileSep2;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.ToolStripMenuItem mnuFileImportPatterns;
		private System.Windows.Forms.ToolStripMenuItem mnuFileExportPatterns;
		private System.Windows.Forms.ToolStripMenuItem mnuView;
		private System.Windows.Forms.ToolStripMenuItem mnuViewGCode;
		private System.Windows.Forms.ToolStripMenuItem mnuEditTemplates;
		private System.Windows.Forms.ToolStripMenuItem mnuHelp;
		private System.Windows.Forms.ToolStripMenuItem mnuHelpAbout;
		private System.Windows.Forms.ToolStripMenuItem mnuFileNewCutList;
		private System.Windows.Forms.Button btnMoveCutUp;
		private System.Windows.Forms.Button btnMoveCutDown;
		private System.Windows.Forms.ImageList ilIcons;
		private System.Windows.Forms.ComboBox cmboMaterialType;
		private System.Windows.Forms.Label lblMaterialType;
		private System.Windows.Forms.ToolStripMenuItem mnuView3D;
		private System.Windows.Forms.ToolStripMenuItem mnuFileImportSvgDrawing;
	}
}
