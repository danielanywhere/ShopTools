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
	partial class frmEditTemplates
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
			if(disposing && (components != null))
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
			components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEditTemplates));
			menuEditTemplates = new System.Windows.Forms.MenuStrip();
			mnuFile = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileImportPatterns = new System.Windows.Forms.ToolStripMenuItem();
			mnuFileExportSelectedPatterns = new System.Windows.Forms.ToolStripMenuItem();
			mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
			mnuEditRestoreFactoryTemplates = new System.Windows.Forms.ToolStripMenuItem();
			mnuView = new System.Windows.Forms.ToolStripMenuItem();
			mnuViewTab = new System.Windows.Forms.ToolStripMenuItem();
			mnuViewTabGeneral = new System.Windows.Forms.ToolStripMenuItem();
			mnuViewTabDisplay = new System.Windows.Forms.ToolStripMenuItem();
			mnuViewTabRemarks = new System.Windows.Forms.ToolStripMenuItem();
			statusEditTemplates = new System.Windows.Forms.StatusStrip();
			statMessage = new System.Windows.Forms.ToolStripStatusLabel();
			pnlTemplates = new System.Windows.Forms.Panel();
			lvTemplates = new System.Windows.Forms.ListView();
			ilTemplates = new System.Windows.Forms.ImageList(components);
			splitterTemplates = new System.Windows.Forms.Splitter();
			pnlControls = new System.Windows.Forms.Panel();
			btnCancel = new System.Windows.Forms.Button();
			btnOK = new System.Windows.Forms.Button();
			tctlEditTemplates = new System.Windows.Forms.TabControl();
			tpgGeneral = new System.Windows.Forms.TabPage();
			lstSharedVariables = new System.Windows.Forms.CheckedListBox();
			lblSharedVariables = new System.Windows.Forms.Label();
			btnTemplateID = new System.Windows.Forms.Button();
			txtTemplateName = new System.Windows.Forms.TextBox();
			txtTemplateID = new System.Windows.Forms.TextBox();
			lblTemplateName = new System.Windows.Forms.Label();
			lblTemplateID = new System.Windows.Forms.Label();
			lblOperations = new System.Windows.Forms.Label();
			lvOperations = new System.Windows.Forms.ListView();
			ilTemplateIcons = new System.Windows.Forms.ImageList(components);
			btnOperationDown = new System.Windows.Forms.Button();
			btnOperationUp = new System.Windows.Forms.Button();
			btnOperationAdd = new System.Windows.Forms.Button();
			btnOperationDelete = new System.Windows.Forms.Button();
			btnOperationEdit = new System.Windows.Forms.Button();
			tpgDisplay = new System.Windows.Forms.TabPage();
			btnIconFilename = new System.Windows.Forms.Button();
			txtIconFilename = new System.Windows.Forms.TextBox();
			lblIconFilename = new System.Windows.Forms.Label();
			txtDisplayFormat = new System.Windows.Forms.TextBox();
			lblDisplayFormat = new System.Windows.Forms.Label();
			tpgRemarks = new System.Windows.Forms.TabPage();
			txtRemarks = new System.Windows.Forms.TextBox();
			btnDelete = new System.Windows.Forms.Button();
			ilTemplateControls = new System.Windows.Forms.ImageList(components);
			btnMoveDown = new System.Windows.Forms.Button();
			btnCreate = new System.Windows.Forms.Button();
			btnMoveUp = new System.Windows.Forms.Button();
			menuEditTemplates.SuspendLayout();
			statusEditTemplates.SuspendLayout();
			pnlTemplates.SuspendLayout();
			pnlControls.SuspendLayout();
			tctlEditTemplates.SuspendLayout();
			tpgGeneral.SuspendLayout();
			tpgDisplay.SuspendLayout();
			tpgRemarks.SuspendLayout();
			SuspendLayout();
			// 
			// menuEditTemplates
			// 
			menuEditTemplates.ImageScalingSize = new System.Drawing.Size(20, 20);
			menuEditTemplates.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuFile, mnuEdit, mnuView });
			menuEditTemplates.Location = new System.Drawing.Point(0, 0);
			menuEditTemplates.Name = "menuEditTemplates";
			menuEditTemplates.Size = new System.Drawing.Size(975, 28);
			menuEditTemplates.TabIndex = 0;
			menuEditTemplates.Text = "menuStrip1";
			// 
			// mnuFile
			// 
			mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuFileImportPatterns, mnuFileExportSelectedPatterns });
			mnuFile.Name = "mnuFile";
			mnuFile.Size = new System.Drawing.Size(46, 24);
			mnuFile.Text = "&File";
			// 
			// mnuFileImportPatterns
			// 
			mnuFileImportPatterns.Name = "mnuFileImportPatterns";
			mnuFileImportPatterns.Size = new System.Drawing.Size(252, 26);
			mnuFileImportPatterns.Text = "&Import Patterns";
			// 
			// mnuFileExportSelectedPatterns
			// 
			mnuFileExportSelectedPatterns.Name = "mnuFileExportSelectedPatterns";
			mnuFileExportSelectedPatterns.Size = new System.Drawing.Size(252, 26);
			mnuFileExportSelectedPatterns.Text = "&Export Selected Patterns";
			// 
			// mnuEdit
			// 
			mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuEditRestoreFactoryTemplates });
			mnuEdit.Name = "mnuEdit";
			mnuEdit.Size = new System.Drawing.Size(49, 24);
			mnuEdit.Text = "&Edit";
			// 
			// mnuEditRestoreFactoryTemplates
			// 
			mnuEditRestoreFactoryTemplates.Name = "mnuEditRestoreFactoryTemplates";
			mnuEditRestoreFactoryTemplates.Size = new System.Drawing.Size(265, 26);
			mnuEditRestoreFactoryTemplates.Text = "&Restore Factory Templates";
			// 
			// mnuView
			// 
			mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuViewTab });
			mnuView.Name = "mnuView";
			mnuView.Size = new System.Drawing.Size(55, 24);
			mnuView.Text = "&View";
			// 
			// mnuViewTab
			// 
			mnuViewTab.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuViewTabGeneral, mnuViewTabDisplay, mnuViewTabRemarks });
			mnuViewTab.Name = "mnuViewTab";
			mnuViewTab.Size = new System.Drawing.Size(115, 26);
			mnuViewTab.Text = "&Tab";
			// 
			// mnuViewTabGeneral
			// 
			mnuViewTabGeneral.Name = "mnuViewTabGeneral";
			mnuViewTabGeneral.Size = new System.Drawing.Size(148, 26);
			mnuViewTabGeneral.Text = "&General";
			// 
			// mnuViewTabDisplay
			// 
			mnuViewTabDisplay.Name = "mnuViewTabDisplay";
			mnuViewTabDisplay.Size = new System.Drawing.Size(148, 26);
			mnuViewTabDisplay.Text = "&Display";
			// 
			// mnuViewTabRemarks
			// 
			mnuViewTabRemarks.Name = "mnuViewTabRemarks";
			mnuViewTabRemarks.Size = new System.Drawing.Size(148, 26);
			mnuViewTabRemarks.Text = "&Remarks";
			// 
			// statusEditTemplates
			// 
			statusEditTemplates.ImageScalingSize = new System.Drawing.Size(20, 20);
			statusEditTemplates.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { statMessage });
			statusEditTemplates.Location = new System.Drawing.Point(0, 482);
			statusEditTemplates.Name = "statusEditTemplates";
			statusEditTemplates.Size = new System.Drawing.Size(975, 26);
			statusEditTemplates.TabIndex = 1;
			statusEditTemplates.Text = "statusStrip1";
			// 
			// statMessage
			// 
			statMessage.Name = "statMessage";
			statMessage.Size = new System.Drawing.Size(59, 20);
			statMessage.Text = "Ready...";
			// 
			// pnlTemplates
			// 
			pnlTemplates.Controls.Add(lvTemplates);
			pnlTemplates.Dock = System.Windows.Forms.DockStyle.Left;
			pnlTemplates.Location = new System.Drawing.Point(0, 28);
			pnlTemplates.Name = "pnlTemplates";
			pnlTemplates.Size = new System.Drawing.Size(273, 454);
			pnlTemplates.TabIndex = 2;
			// 
			// lvTemplates
			// 
			lvTemplates.Dock = System.Windows.Forms.DockStyle.Fill;
			lvTemplates.LargeImageList = ilTemplates;
			lvTemplates.Location = new System.Drawing.Point(0, 0);
			lvTemplates.Name = "lvTemplates";
			lvTemplates.Size = new System.Drawing.Size(273, 454);
			lvTemplates.TabIndex = 0;
			lvTemplates.UseCompatibleStateImageBehavior = false;
			// 
			// ilTemplates
			// 
			ilTemplates.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			ilTemplates.ImageSize = new System.Drawing.Size(128, 128);
			ilTemplates.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// splitterTemplates
			// 
			splitterTemplates.Location = new System.Drawing.Point(273, 28);
			splitterTemplates.Name = "splitterTemplates";
			splitterTemplates.Size = new System.Drawing.Size(8, 454);
			splitterTemplates.TabIndex = 3;
			splitterTemplates.TabStop = false;
			// 
			// pnlControls
			// 
			pnlControls.Controls.Add(btnCancel);
			pnlControls.Controls.Add(btnOK);
			pnlControls.Controls.Add(tctlEditTemplates);
			pnlControls.Controls.Add(btnDelete);
			pnlControls.Controls.Add(btnMoveDown);
			pnlControls.Controls.Add(btnCreate);
			pnlControls.Controls.Add(btnMoveUp);
			pnlControls.Dock = System.Windows.Forms.DockStyle.Fill;
			pnlControls.Location = new System.Drawing.Point(281, 28);
			pnlControls.Name = "pnlControls";
			pnlControls.Size = new System.Drawing.Size(694, 454);
			pnlControls.TabIndex = 4;
			// 
			// btnCancel
			// 
			btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnCancel.Location = new System.Drawing.Point(593, 411);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(94, 32);
			btnCancel.TabIndex = 6;
			btnCancel.Text = "&Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnOK.Location = new System.Drawing.Point(493, 411);
			btnOK.Name = "btnOK";
			btnOK.Size = new System.Drawing.Size(94, 32);
			btnOK.TabIndex = 5;
			btnOK.Text = "&OK";
			btnOK.UseVisualStyleBackColor = true;
			// 
			// tctlEditTemplates
			// 
			tctlEditTemplates.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			tctlEditTemplates.Controls.Add(tpgGeneral);
			tctlEditTemplates.Controls.Add(tpgDisplay);
			tctlEditTemplates.Controls.Add(tpgRemarks);
			tctlEditTemplates.Location = new System.Drawing.Point(110, 5);
			tctlEditTemplates.Name = "tctlEditTemplates";
			tctlEditTemplates.SelectedIndex = 0;
			tctlEditTemplates.Size = new System.Drawing.Size(581, 404);
			tctlEditTemplates.TabIndex = 4;
			// 
			// tpgGeneral
			// 
			tpgGeneral.Controls.Add(lstSharedVariables);
			tpgGeneral.Controls.Add(lblSharedVariables);
			tpgGeneral.Controls.Add(btnTemplateID);
			tpgGeneral.Controls.Add(txtTemplateName);
			tpgGeneral.Controls.Add(txtTemplateID);
			tpgGeneral.Controls.Add(lblTemplateName);
			tpgGeneral.Controls.Add(lblTemplateID);
			tpgGeneral.Controls.Add(lblOperations);
			tpgGeneral.Controls.Add(lvOperations);
			tpgGeneral.Controls.Add(btnOperationDown);
			tpgGeneral.Controls.Add(btnOperationUp);
			tpgGeneral.Controls.Add(btnOperationAdd);
			tpgGeneral.Controls.Add(btnOperationDelete);
			tpgGeneral.Controls.Add(btnOperationEdit);
			tpgGeneral.Location = new System.Drawing.Point(4, 29);
			tpgGeneral.Name = "tpgGeneral";
			tpgGeneral.Padding = new System.Windows.Forms.Padding(3);
			tpgGeneral.Size = new System.Drawing.Size(573, 371);
			tpgGeneral.TabIndex = 0;
			tpgGeneral.Text = "General";
			tpgGeneral.UseVisualStyleBackColor = true;
			// 
			// lstSharedVariables
			// 
			lstSharedVariables.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			lstSharedVariables.FormattingEnabled = true;
			lstSharedVariables.Location = new System.Drawing.Point(408, 125);
			lstSharedVariables.Name = "lstSharedVariables";
			lstSharedVariables.Size = new System.Drawing.Size(150, 180);
			lstSharedVariables.TabIndex = 13;
			// 
			// lblSharedVariables
			// 
			lblSharedVariables.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			lblSharedVariables.AutoSize = true;
			lblSharedVariables.Location = new System.Drawing.Point(408, 102);
			lblSharedVariables.Name = "lblSharedVariables";
			lblSharedVariables.Size = new System.Drawing.Size(122, 20);
			lblSharedVariables.TabIndex = 12;
			lblSharedVariables.Text = "Shared Variables:";
			// 
			// btnTemplateID
			// 
			btnTemplateID.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnTemplateID.Location = new System.Drawing.Point(525, 60);
			btnTemplateID.Name = "btnTemplateID";
			btnTemplateID.Size = new System.Drawing.Size(33, 29);
			btnTemplateID.TabIndex = 4;
			btnTemplateID.Text = "...";
			btnTemplateID.UseVisualStyleBackColor = true;
			// 
			// txtTemplateName
			// 
			txtTemplateName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtTemplateName.Location = new System.Drawing.Point(99, 25);
			txtTemplateName.Name = "txtTemplateName";
			txtTemplateName.Size = new System.Drawing.Size(459, 27);
			txtTemplateName.TabIndex = 1;
			// 
			// txtTemplateID
			// 
			txtTemplateID.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtTemplateID.BackColor = System.Drawing.Color.Gainsboro;
			txtTemplateID.Location = new System.Drawing.Point(99, 61);
			txtTemplateID.Name = "txtTemplateID";
			txtTemplateID.ReadOnly = true;
			txtTemplateID.Size = new System.Drawing.Size(420, 27);
			txtTemplateID.TabIndex = 3;
			txtTemplateID.TabStop = false;
			// 
			// lblTemplateName
			// 
			lblTemplateName.AutoSize = true;
			lblTemplateName.Location = new System.Drawing.Point(6, 28);
			lblTemplateName.Name = "lblTemplateName";
			lblTemplateName.Size = new System.Drawing.Size(52, 20);
			lblTemplateName.TabIndex = 0;
			lblTemplateName.Text = "Name:";
			// 
			// lblTemplateID
			// 
			lblTemplateID.AutoSize = true;
			lblTemplateID.Location = new System.Drawing.Point(6, 64);
			lblTemplateID.Name = "lblTemplateID";
			lblTemplateID.Size = new System.Drawing.Size(93, 20);
			lblTemplateID.TabIndex = 2;
			lblTemplateID.Text = "Template ID:";
			// 
			// lblOperations
			// 
			lblOperations.AutoSize = true;
			lblOperations.Location = new System.Drawing.Point(6, 102);
			lblOperations.Name = "lblOperations";
			lblOperations.Size = new System.Drawing.Size(85, 20);
			lblOperations.TabIndex = 5;
			lblOperations.Text = "Operations:";
			// 
			// lvOperations
			// 
			lvOperations.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lvOperations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			lvOperations.LargeImageList = ilTemplateIcons;
			lvOperations.Location = new System.Drawing.Point(6, 125);
			lvOperations.Name = "lvOperations";
			lvOperations.Size = new System.Drawing.Size(265, 166);
			lvOperations.SmallImageList = ilTemplateIcons;
			lvOperations.TabIndex = 6;
			lvOperations.UseCompatibleStateImageBehavior = false;
			lvOperations.View = System.Windows.Forms.View.Details;
			// 
			// ilTemplateIcons
			// 
			ilTemplateIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			ilTemplateIcons.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ilTemplateIcons.ImageStream");
			ilTemplateIcons.TransparentColor = System.Drawing.Color.Transparent;
			ilTemplateIcons.Images.SetKeyName(0, "Task_ig.png");
			ilTemplateIcons.Images.SetKeyName(1, "Task_igAdd.png");
			ilTemplateIcons.Images.SetKeyName(2, "Task_igEdit.png");
			ilTemplateIcons.Images.SetKeyName(3, "Task_igDelete.png");
			ilTemplateIcons.Images.SetKeyName(4, "MoveUp.png");
			ilTemplateIcons.Images.SetKeyName(5, "MoveDown.png");
			// 
			// btnOperationDown
			// 
			btnOperationDown.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnOperationDown.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnOperationDown.ImageIndex = 5;
			btnOperationDown.ImageList = ilTemplateIcons;
			btnOperationDown.Location = new System.Drawing.Point(276, 192);
			btnOperationDown.Name = "btnOperationDown";
			btnOperationDown.Size = new System.Drawing.Size(94, 29);
			btnOperationDown.TabIndex = 9;
			btnOperationDown.Text = "Do&wn";
			btnOperationDown.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			btnOperationDown.UseVisualStyleBackColor = true;
			// 
			// btnOperationUp
			// 
			btnOperationUp.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnOperationUp.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnOperationUp.ImageIndex = 4;
			btnOperationUp.ImageList = ilTemplateIcons;
			btnOperationUp.Location = new System.Drawing.Point(276, 160);
			btnOperationUp.Name = "btnOperationUp";
			btnOperationUp.Size = new System.Drawing.Size(94, 29);
			btnOperationUp.TabIndex = 8;
			btnOperationUp.Text = "U&p";
			btnOperationUp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			btnOperationUp.UseVisualStyleBackColor = true;
			// 
			// btnOperationAdd
			// 
			btnOperationAdd.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnOperationAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnOperationAdd.ImageIndex = 1;
			btnOperationAdd.ImageList = ilTemplateIcons;
			btnOperationAdd.Location = new System.Drawing.Point(276, 125);
			btnOperationAdd.Name = "btnOperationAdd";
			btnOperationAdd.Size = new System.Drawing.Size(94, 29);
			btnOperationAdd.TabIndex = 7;
			btnOperationAdd.Text = "&Add";
			btnOperationAdd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			btnOperationAdd.UseVisualStyleBackColor = true;
			// 
			// btnOperationDelete
			// 
			btnOperationDelete.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnOperationDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnOperationDelete.ImageIndex = 3;
			btnOperationDelete.ImageList = ilTemplateIcons;
			btnOperationDelete.Location = new System.Drawing.Point(276, 262);
			btnOperationDelete.Name = "btnOperationDelete";
			btnOperationDelete.Size = new System.Drawing.Size(94, 29);
			btnOperationDelete.TabIndex = 11;
			btnOperationDelete.Text = "Dele&te";
			btnOperationDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			btnOperationDelete.UseVisualStyleBackColor = true;
			// 
			// btnOperationEdit
			// 
			btnOperationEdit.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnOperationEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnOperationEdit.ImageIndex = 2;
			btnOperationEdit.ImageList = ilTemplateIcons;
			btnOperationEdit.Location = new System.Drawing.Point(276, 227);
			btnOperationEdit.Name = "btnOperationEdit";
			btnOperationEdit.Size = new System.Drawing.Size(94, 29);
			btnOperationEdit.TabIndex = 10;
			btnOperationEdit.Text = "Ed&it";
			btnOperationEdit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			btnOperationEdit.UseVisualStyleBackColor = true;
			// 
			// tpgDisplay
			// 
			tpgDisplay.Controls.Add(btnIconFilename);
			tpgDisplay.Controls.Add(txtIconFilename);
			tpgDisplay.Controls.Add(lblIconFilename);
			tpgDisplay.Controls.Add(txtDisplayFormat);
			tpgDisplay.Controls.Add(lblDisplayFormat);
			tpgDisplay.Location = new System.Drawing.Point(4, 29);
			tpgDisplay.Name = "tpgDisplay";
			tpgDisplay.Padding = new System.Windows.Forms.Padding(3);
			tpgDisplay.Size = new System.Drawing.Size(573, 371);
			tpgDisplay.TabIndex = 1;
			tpgDisplay.Text = "Display";
			tpgDisplay.UseVisualStyleBackColor = true;
			// 
			// btnIconFilename
			// 
			btnIconFilename.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnIconFilename.Location = new System.Drawing.Point(384, 24);
			btnIconFilename.Name = "btnIconFilename";
			btnIconFilename.Size = new System.Drawing.Size(33, 29);
			btnIconFilename.TabIndex = 2;
			btnIconFilename.Text = "...";
			btnIconFilename.UseVisualStyleBackColor = true;
			// 
			// txtIconFilename
			// 
			txtIconFilename.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtIconFilename.Location = new System.Drawing.Point(126, 25);
			txtIconFilename.Name = "txtIconFilename";
			txtIconFilename.Size = new System.Drawing.Size(252, 27);
			txtIconFilename.TabIndex = 1;
			// 
			// lblIconFilename
			// 
			lblIconFilename.AutoSize = true;
			lblIconFilename.Location = new System.Drawing.Point(8, 28);
			lblIconFilename.Name = "lblIconFilename";
			lblIconFilename.Size = new System.Drawing.Size(104, 20);
			lblIconFilename.TabIndex = 0;
			lblIconFilename.Text = "Icon Filename:";
			// 
			// txtDisplayFormat
			// 
			txtDisplayFormat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtDisplayFormat.Location = new System.Drawing.Point(126, 61);
			txtDisplayFormat.Name = "txtDisplayFormat";
			txtDisplayFormat.Size = new System.Drawing.Size(291, 27);
			txtDisplayFormat.TabIndex = 4;
			// 
			// lblDisplayFormat
			// 
			lblDisplayFormat.AutoSize = true;
			lblDisplayFormat.Location = new System.Drawing.Point(8, 64);
			lblDisplayFormat.Name = "lblDisplayFormat";
			lblDisplayFormat.Size = new System.Drawing.Size(112, 20);
			lblDisplayFormat.TabIndex = 3;
			lblDisplayFormat.Text = "Display Format:";
			// 
			// tpgRemarks
			// 
			tpgRemarks.Controls.Add(txtRemarks);
			tpgRemarks.Location = new System.Drawing.Point(4, 29);
			tpgRemarks.Name = "tpgRemarks";
			tpgRemarks.Size = new System.Drawing.Size(573, 371);
			tpgRemarks.TabIndex = 2;
			tpgRemarks.Text = "Remarks";
			tpgRemarks.UseVisualStyleBackColor = true;
			// 
			// txtRemarks
			// 
			txtRemarks.AcceptsReturn = true;
			txtRemarks.Dock = System.Windows.Forms.DockStyle.Fill;
			txtRemarks.Font = new System.Drawing.Font("Cascadia Mono SemiBold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			txtRemarks.Location = new System.Drawing.Point(0, 0);
			txtRemarks.Multiline = true;
			txtRemarks.Name = "txtRemarks";
			txtRemarks.Size = new System.Drawing.Size(573, 371);
			txtRemarks.TabIndex = 0;
			// 
			// btnDelete
			// 
			btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btnDelete.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			btnDelete.ImageIndex = 3;
			btnDelete.ImageList = ilTemplateControls;
			btnDelete.Location = new System.Drawing.Point(6, 370);
			btnDelete.Name = "btnDelete";
			btnDelete.Size = new System.Drawing.Size(98, 81);
			btnDelete.TabIndex = 3;
			btnDelete.Text = "De&lete";
			btnDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			btnDelete.UseVisualStyleBackColor = true;
			// 
			// ilTemplateControls
			// 
			ilTemplateControls.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			ilTemplateControls.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ilTemplateControls.ImageStream");
			ilTemplateControls.TransparentColor = System.Drawing.Color.Transparent;
			ilTemplateControls.Images.SetKeyName(0, "ButtonUpIcon.png");
			ilTemplateControls.Images.SetKeyName(1, "ButtonDownIcon.png");
			ilTemplateControls.Images.SetKeyName(2, "ButtonCreateIcon.png");
			ilTemplateControls.Images.SetKeyName(3, "ButtonDeleteIcon.png");
			// 
			// btnMoveDown
			// 
			btnMoveDown.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			btnMoveDown.ImageIndex = 1;
			btnMoveDown.ImageList = ilTemplateControls;
			btnMoveDown.Location = new System.Drawing.Point(6, 226);
			btnMoveDown.Name = "btnMoveDown";
			btnMoveDown.Size = new System.Drawing.Size(98, 81);
			btnMoveDown.TabIndex = 2;
			btnMoveDown.Text = "Move &Down";
			btnMoveDown.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			btnMoveDown.UseVisualStyleBackColor = true;
			// 
			// btnCreate
			// 
			btnCreate.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			btnCreate.ImageIndex = 2;
			btnCreate.ImageList = ilTemplateControls;
			btnCreate.Location = new System.Drawing.Point(6, 5);
			btnCreate.Name = "btnCreate";
			btnCreate.Size = new System.Drawing.Size(98, 81);
			btnCreate.TabIndex = 0;
			btnCreate.Text = "C&reate";
			btnCreate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			btnCreate.UseVisualStyleBackColor = true;
			// 
			// btnMoveUp
			// 
			btnMoveUp.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			btnMoveUp.ImageIndex = 0;
			btnMoveUp.ImageList = ilTemplateControls;
			btnMoveUp.Location = new System.Drawing.Point(6, 139);
			btnMoveUp.Name = "btnMoveUp";
			btnMoveUp.Size = new System.Drawing.Size(98, 81);
			btnMoveUp.TabIndex = 1;
			btnMoveUp.Text = "Move &Up";
			btnMoveUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			btnMoveUp.UseVisualStyleBackColor = true;
			// 
			// frmEditTemplates
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(975, 508);
			Controls.Add(pnlControls);
			Controls.Add(splitterTemplates);
			Controls.Add(pnlTemplates);
			Controls.Add(statusEditTemplates);
			Controls.Add(menuEditTemplates);
			MainMenuStrip = menuEditTemplates;
			Name = "frmEditTemplates";
			Text = "Edit Templates";
			menuEditTemplates.ResumeLayout(false);
			menuEditTemplates.PerformLayout();
			statusEditTemplates.ResumeLayout(false);
			statusEditTemplates.PerformLayout();
			pnlTemplates.ResumeLayout(false);
			pnlControls.ResumeLayout(false);
			tctlEditTemplates.ResumeLayout(false);
			tpgGeneral.ResumeLayout(false);
			tpgGeneral.PerformLayout();
			tpgDisplay.ResumeLayout(false);
			tpgDisplay.PerformLayout();
			tpgRemarks.ResumeLayout(false);
			tpgRemarks.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.MenuStrip menuEditTemplates;
		private System.Windows.Forms.StatusStrip statusEditTemplates;
		private System.Windows.Forms.ToolStripStatusLabel statMessage;
		private System.Windows.Forms.Panel pnlTemplates;
		private System.Windows.Forms.ListView lvTemplates;
		private System.Windows.Forms.Splitter splitterTemplates;
		private System.Windows.Forms.Panel pnlControls;
		private System.Windows.Forms.Button btnMoveUp;
		private System.Windows.Forms.ImageList ilTemplateControls;
		private System.Windows.Forms.Button btnMoveDown;
		private System.Windows.Forms.Button btnCreate;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.TabControl tctlEditTemplates;
		private System.Windows.Forms.TabPage tpgGeneral;
		private System.Windows.Forms.TabPage tpgDisplay;
		private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStripMenuItem mnuView;
		private System.Windows.Forms.ToolStripMenuItem mnuViewTab;
		private System.Windows.Forms.ToolStripMenuItem mnuViewTabGeneral;
		private System.Windows.Forms.ToolStripMenuItem mnuViewTabDisplay;
		private System.Windows.Forms.TextBox txtDisplayFormat;
		private System.Windows.Forms.Label lblDisplayFormat;
		private System.Windows.Forms.Button btnIconFilename;
		private System.Windows.Forms.TextBox txtIconFilename;
		private System.Windows.Forms.Label lblIconFilename;
		private System.Windows.Forms.ListView lvOperations;
		private System.Windows.Forms.Button btnOperationEdit;
		private System.Windows.Forms.Label lblOperations;
		private System.Windows.Forms.Button btnTemplateID;
		private System.Windows.Forms.TextBox txtTemplateID;
		private System.Windows.Forms.Label lblTemplateID;
		private System.Windows.Forms.ToolStripMenuItem mnuViewTabRemarks;
		private System.Windows.Forms.TabPage tpgRemarks;
		private System.Windows.Forms.TextBox txtRemarks;
		private System.Windows.Forms.CheckedListBox lstSharedVariables;
		private System.Windows.Forms.Label lblSharedVariables;
		private System.Windows.Forms.TextBox txtTemplateName;
		private System.Windows.Forms.Label lblTemplateName;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ImageList ilTemplates;
		private System.Windows.Forms.ImageList ilTemplateIcons;
		private System.Windows.Forms.Button btnOperationDown;
		private System.Windows.Forms.Button btnOperationUp;
		private System.Windows.Forms.Button btnOperationAdd;
		private System.Windows.Forms.Button btnOperationDelete;
		private System.Windows.Forms.ToolStripMenuItem mnuEdit;
		private System.Windows.Forms.ToolStripMenuItem mnuEditRestoreFactoryTemplates;
		private System.Windows.Forms.ToolStripMenuItem mnuFileImportPatterns;
		private System.Windows.Forms.ToolStripMenuItem mnuFileExportSelectedPatterns;
	}
}