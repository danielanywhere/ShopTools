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
	partial class frmEditOperation
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
			lblAction = new System.Windows.Forms.Label();
			cmboAction = new System.Windows.Forms.ComboBox();
			lblOperationName = new System.Windows.Forms.Label();
			txtOperationName = new System.Windows.Forms.TextBox();
			lblOperationNameHint = new System.Windows.Forms.Label();
			dgVariables = new System.Windows.Forms.DataGridView();
			lblVariables = new System.Windows.Forms.Label();
			lblHiddenVariables = new System.Windows.Forms.Label();
			lstHiddenVariables = new System.Windows.Forms.CheckedListBox();
			btnCancel = new System.Windows.Forms.Button();
			btnOK = new System.Windows.Forms.Button();
			lblStatValue = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)dgVariables).BeginInit();
			SuspendLayout();
			// 
			// lblAction
			// 
			lblAction.AutoSize = true;
			lblAction.Location = new System.Drawing.Point(17, 15);
			lblAction.Name = "lblAction";
			lblAction.Size = new System.Drawing.Size(55, 20);
			lblAction.TabIndex = 0;
			lblAction.Text = "Action:";
			// 
			// cmboAction
			// 
			cmboAction.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			cmboAction.FormattingEnabled = true;
			cmboAction.Location = new System.Drawing.Point(146, 12);
			cmboAction.Name = "cmboAction";
			cmboAction.Size = new System.Drawing.Size(541, 28);
			cmboAction.TabIndex = 1;
			// 
			// lblOperationName
			// 
			lblOperationName.AutoSize = true;
			lblOperationName.Location = new System.Drawing.Point(17, 54);
			lblOperationName.Name = "lblOperationName";
			lblOperationName.Size = new System.Drawing.Size(123, 20);
			lblOperationName.TabIndex = 2;
			lblOperationName.Text = "Operation Name:";
			// 
			// txtOperationName
			// 
			txtOperationName.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			txtOperationName.Location = new System.Drawing.Point(146, 51);
			txtOperationName.Name = "txtOperationName";
			txtOperationName.Size = new System.Drawing.Size(541, 27);
			txtOperationName.TabIndex = 3;
			// 
			// lblOperationNameHint
			// 
			lblOperationNameHint.ForeColor = System.Drawing.SystemColors.ControlDark;
			lblOperationNameHint.Location = new System.Drawing.Point(146, 81);
			lblOperationNameHint.Name = "lblOperationNameHint";
			lblOperationNameHint.Size = new System.Drawing.Size(393, 49);
			lblOperationNameHint.TabIndex = 4;
			lblOperationNameHint.Text = "Optional. Use this name to group properties between multiple actions.";
			// 
			// dgVariables
			// 
			dgVariables.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			dgVariables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgVariables.Location = new System.Drawing.Point(23, 170);
			dgVariables.Name = "dgVariables";
			dgVariables.RowHeadersWidth = 51;
			dgVariables.Size = new System.Drawing.Size(426, 188);
			dgVariables.TabIndex = 6;
			// 
			// lblVariables
			// 
			lblVariables.AutoSize = true;
			lblVariables.Location = new System.Drawing.Point(18, 147);
			lblVariables.Name = "lblVariables";
			lblVariables.Size = new System.Drawing.Size(72, 20);
			lblVariables.TabIndex = 5;
			lblVariables.Text = "Variables:";
			// 
			// lblHiddenVariables
			// 
			lblHiddenVariables.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
			lblHiddenVariables.AutoSize = true;
			lblHiddenVariables.Location = new System.Drawing.Point(481, 147);
			lblHiddenVariables.Name = "lblHiddenVariables";
			lblHiddenVariables.Size = new System.Drawing.Size(125, 20);
			lblHiddenVariables.TabIndex = 7;
			lblHiddenVariables.Text = "Hidden Variables:";
			// 
			// lstHiddenVariables
			// 
			lstHiddenVariables.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			lstHiddenVariables.FormattingEnabled = true;
			lstHiddenVariables.Location = new System.Drawing.Point(481, 170);
			lstHiddenVariables.Name = "lstHiddenVariables";
			lstHiddenVariables.Size = new System.Drawing.Size(206, 180);
			lstHiddenVariables.TabIndex = 8;
			// 
			// btnCancel
			// 
			btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnCancel.Location = new System.Drawing.Point(593, 373);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(94, 32);
			btnCancel.TabIndex = 10;
			btnCancel.Text = "&Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnOK
			// 
			btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnOK.Location = new System.Drawing.Point(493, 373);
			btnOK.Name = "btnOK";
			btnOK.Size = new System.Drawing.Size(94, 32);
			btnOK.TabIndex = 9;
			btnOK.Text = "&OK";
			btnOK.UseVisualStyleBackColor = true;
			// 
			// lblStatValue
			// 
			lblStatValue.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblStatValue.AutoSize = true;
			lblStatValue.Location = new System.Drawing.Point(23, 385);
			lblStatValue.Name = "lblStatValue";
			lblStatValue.Size = new System.Drawing.Size(18, 20);
			lblStatValue.TabIndex = 11;
			lblStatValue.Text = "...";
			// 
			// frmEditOperation
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(699, 418);
			Controls.Add(lblStatValue);
			Controls.Add(btnOK);
			Controls.Add(btnCancel);
			Controls.Add(lstHiddenVariables);
			Controls.Add(lblHiddenVariables);
			Controls.Add(lblVariables);
			Controls.Add(dgVariables);
			Controls.Add(txtOperationName);
			Controls.Add(lblOperationNameHint);
			Controls.Add(lblOperationName);
			Controls.Add(cmboAction);
			Controls.Add(lblAction);
			Name = "frmEditOperation";
			Text = "Edit Operation";
			((System.ComponentModel.ISupportInitialize)dgVariables).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label lblAction;
		private System.Windows.Forms.ComboBox cmboAction;
		private System.Windows.Forms.Label lblOperationName;
		private System.Windows.Forms.TextBox txtOperationName;
		private System.Windows.Forms.Label lblOperationNameHint;
		private System.Windows.Forms.DataGridView dgVariables;
		private System.Windows.Forms.Label lblVariables;
		private System.Windows.Forms.Label lblHiddenVariables;
		private System.Windows.Forms.CheckedListBox lstHiddenVariables;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Label lblStatValue;
	}
}