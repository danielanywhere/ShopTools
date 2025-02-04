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
	partial class frmCutEdit
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
			pnlPreview = new System.Windows.Forms.Panel();
			dgProperties = new System.Windows.Forms.DataGridView();
			menuCut = new System.Windows.Forms.MenuStrip();
			mnuForm = new System.Windows.Forms.ToolStripMenuItem();
			mnuFormCloseWithoutSaving = new System.Windows.Forms.ToolStripMenuItem();
			mnuFormSaveChangesClose = new System.Windows.Forms.ToolStripMenuItem();
			statusCut = new System.Windows.Forms.StatusStrip();
			statMessage = new System.Windows.Forms.ToolStripStatusLabel();
			pnlWorkspace = new System.Windows.Forms.Panel();
			splitter1 = new System.Windows.Forms.Splitter();
			pnlControl = new System.Windows.Forms.Panel();
			btnCancel = new System.Windows.Forms.Button();
			btnOK = new System.Windows.Forms.Button();
			statValue = new System.Windows.Forms.ToolStripStatusLabel();
			((System.ComponentModel.ISupportInitialize)dgProperties).BeginInit();
			menuCut.SuspendLayout();
			statusCut.SuspendLayout();
			pnlWorkspace.SuspendLayout();
			pnlControl.SuspendLayout();
			SuspendLayout();
			// 
			// pnlPreview
			// 
			pnlPreview.BackColor = System.Drawing.Color.FromArgb(17, 51, 102);
			pnlPreview.Dock = System.Windows.Forms.DockStyle.Fill;
			pnlPreview.Location = new System.Drawing.Point(0, 0);
			pnlPreview.Name = "pnlPreview";
			pnlPreview.Size = new System.Drawing.Size(461, 350);
			pnlPreview.TabIndex = 0;
			// 
			// dgProperties
			// 
			dgProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgProperties.Dock = System.Windows.Forms.DockStyle.Right;
			dgProperties.Location = new System.Drawing.Point(469, 0);
			dgProperties.Name = "dgProperties";
			dgProperties.RowHeadersWidth = 51;
			dgProperties.Size = new System.Drawing.Size(331, 350);
			dgProperties.TabIndex = 2;
			// 
			// menuCut
			// 
			menuCut.ImageScalingSize = new System.Drawing.Size(20, 20);
			menuCut.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuForm });
			menuCut.Location = new System.Drawing.Point(0, 0);
			menuCut.Name = "menuCut";
			menuCut.Size = new System.Drawing.Size(800, 28);
			menuCut.TabIndex = 3;
			menuCut.Text = "menuStrip1";
			// 
			// mnuForm
			// 
			mnuForm.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { mnuFormCloseWithoutSaving, mnuFormSaveChangesClose });
			mnuForm.Name = "mnuForm";
			mnuForm.Size = new System.Drawing.Size(57, 24);
			mnuForm.Text = "&Form";
			// 
			// mnuFormCloseWithoutSaving
			// 
			mnuFormCloseWithoutSaving.Name = "mnuFormCloseWithoutSaving";
			mnuFormCloseWithoutSaving.Size = new System.Drawing.Size(252, 26);
			mnuFormCloseWithoutSaving.Text = "&Close Without Saving";
			// 
			// mnuFormSaveChangesClose
			// 
			mnuFormSaveChangesClose.Name = "mnuFormSaveChangesClose";
			mnuFormSaveChangesClose.Size = new System.Drawing.Size(252, 26);
			mnuFormSaveChangesClose.Text = "Save Changes and Cl&ose";
			// 
			// statusCut
			// 
			statusCut.ImageScalingSize = new System.Drawing.Size(20, 20);
			statusCut.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { statMessage, statValue });
			statusCut.Location = new System.Drawing.Point(0, 424);
			statusCut.Name = "statusCut";
			statusCut.Size = new System.Drawing.Size(800, 26);
			statusCut.TabIndex = 4;
			statusCut.Text = "statusStrip1";
			// 
			// statMessage
			// 
			statMessage.Name = "statMessage";
			statMessage.Size = new System.Drawing.Size(115, 20);
			statMessage.Text = "Edit Properties...";
			// 
			// pnlWorkspace
			// 
			pnlWorkspace.Controls.Add(pnlPreview);
			pnlWorkspace.Controls.Add(splitter1);
			pnlWorkspace.Controls.Add(dgProperties);
			pnlWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
			pnlWorkspace.Location = new System.Drawing.Point(0, 28);
			pnlWorkspace.Name = "pnlWorkspace";
			pnlWorkspace.Size = new System.Drawing.Size(800, 350);
			pnlWorkspace.TabIndex = 5;
			// 
			// splitter1
			// 
			splitter1.BackColor = System.Drawing.Color.White;
			splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			splitter1.Dock = System.Windows.Forms.DockStyle.Right;
			splitter1.Location = new System.Drawing.Point(461, 0);
			splitter1.Name = "splitter1";
			splitter1.Size = new System.Drawing.Size(8, 350);
			splitter1.TabIndex = 3;
			splitter1.TabStop = false;
			// 
			// pnlControl
			// 
			pnlControl.Controls.Add(btnCancel);
			pnlControl.Controls.Add(btnOK);
			pnlControl.Dock = System.Windows.Forms.DockStyle.Bottom;
			pnlControl.Location = new System.Drawing.Point(0, 378);
			pnlControl.Name = "pnlControl";
			pnlControl.Size = new System.Drawing.Size(800, 46);
			pnlControl.TabIndex = 6;
			// 
			// btnCancel
			// 
			btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnCancel.Location = new System.Drawing.Point(697, 6);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(91, 32);
			btnCancel.TabIndex = 0;
			btnCancel.Text = "&Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			btnOK.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			btnOK.Location = new System.Drawing.Point(600, 6);
			btnOK.Name = "btnOK";
			btnOK.Size = new System.Drawing.Size(91, 32);
			btnOK.TabIndex = 0;
			btnOK.Text = "&OK";
			btnOK.UseVisualStyleBackColor = true;
			// 
			// statValue
			// 
			statValue.Name = "statValue";
			statValue.Size = new System.Drawing.Size(670, 20);
			statValue.Spring = true;
			statValue.Text = "...";
			statValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// frmCutEdit
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(800, 450);
			Controls.Add(pnlWorkspace);
			Controls.Add(pnlControl);
			Controls.Add(statusCut);
			Controls.Add(menuCut);
			MainMenuStrip = menuCut;
			Name = "frmCutEdit";
			Text = "Edit Cut";
			((System.ComponentModel.ISupportInitialize)dgProperties).EndInit();
			menuCut.ResumeLayout(false);
			menuCut.PerformLayout();
			statusCut.ResumeLayout(false);
			statusCut.PerformLayout();
			pnlWorkspace.ResumeLayout(false);
			pnlControl.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Panel pnlPreview;
		private System.Windows.Forms.DataGridView dgProperties;
		private System.Windows.Forms.MenuStrip menuCut;
		private System.Windows.Forms.StatusStrip statusCut;
		private System.Windows.Forms.Panel pnlWorkspace;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ToolStripMenuItem mnuForm;
		private System.Windows.Forms.ToolStripMenuItem mnuFormCloseWithoutSaving;
		private System.Windows.Forms.Panel pnlControl;
		private System.Windows.Forms.ToolStripMenuItem mnuFormSaveChangesClose;
		private System.Windows.Forms.ToolStripStatusLabel statMessage;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ToolStripStatusLabel statValue;
	}
}