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
	partial class frmSettings
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
			pnlCanvasArea = new System.Windows.Forms.Panel();
			lblWorkingArea = new System.Windows.Forms.Label();
			lblXDimension = new System.Windows.Forms.Label();
			txtXDimension = new System.Windows.Forms.TextBox();
			lblUnits = new System.Windows.Forms.Label();
			optUS = new System.Windows.Forms.RadioButton();
			optMetric = new System.Windows.Forms.RadioButton();
			lblYDimension = new System.Windows.Forms.Label();
			txtYDimension = new System.Windows.Forms.TextBox();
			lblXYOrigin = new System.Windows.Forms.Label();
			cmboXYOrigin = new System.Windows.Forms.ComboBox();
			txtZDimension = new System.Windows.Forms.TextBox();
			lblZDimension = new System.Windows.Forms.Label();
			lblTravel = new System.Windows.Forms.Label();
			optXPRight = new System.Windows.Forms.RadioButton();
			optXPLeft = new System.Windows.Forms.RadioButton();
			optYPUp = new System.Windows.Forms.RadioButton();
			optYPDown = new System.Windows.Forms.RadioButton();
			optZPUp = new System.Windows.Forms.RadioButton();
			optZPDown = new System.Windows.Forms.RadioButton();
			lblStockAccess = new System.Windows.Forms.Label();
			lblStockAccessTip = new System.Windows.Forms.Label();
			chkXOpenEnded = new System.Windows.Forms.CheckBox();
			chkYOpenEnded = new System.Windows.Forms.CheckBox();
			pnlTravelX = new System.Windows.Forms.Panel();
			pnlTravelY = new System.Windows.Forms.Panel();
			pnlTravelZ = new System.Windows.Forms.Panel();
			pnlUnits = new System.Windows.Forms.Panel();
			lblTools = new System.Windows.Forms.Label();
			lblGeneralCuttingTool = new System.Windows.Forms.Label();
			cmboGeneralCuttingTool = new System.Windows.Forms.ComboBox();
			btnOK = new System.Windows.Forms.Button();
			btnCancel = new System.Windows.Forms.Button();
			lblSystemSummary = new System.Windows.Forms.Label();
			txtSystemSummary = new System.Windows.Forms.TextBox();
			lblZOrigin = new System.Windows.Forms.Label();
			cmboZOrigin = new System.Windows.Forms.ComboBox();
			btnEditTools = new System.Windows.Forms.Button();
			lblXDimensionUnit = new System.Windows.Forms.Label();
			lblYDimensionUnit = new System.Windows.Forms.Label();
			lblZDimensionUnit = new System.Windows.Forms.Label();
			pnlTravelX.SuspendLayout();
			pnlTravelY.SuspendLayout();
			pnlTravelZ.SuspendLayout();
			pnlUnits.SuspendLayout();
			SuspendLayout();
			// 
			// pnlCanvasArea
			// 
			pnlCanvasArea.BackColor = System.Drawing.Color.FromArgb(191, 165, 142);
			pnlCanvasArea.Location = new System.Drawing.Point(140, 154);
			pnlCanvasArea.Name = "pnlCanvasArea";
			pnlCanvasArea.Size = new System.Drawing.Size(194, 194);
			pnlCanvasArea.TabIndex = 12;
			// 
			// lblWorkingArea
			// 
			lblWorkingArea.AutoSize = true;
			lblWorkingArea.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblWorkingArea.Location = new System.Drawing.Point(12, 84);
			lblWorkingArea.Name = "lblWorkingArea";
			lblWorkingArea.Size = new System.Drawing.Size(228, 28);
			lblWorkingArea.TabIndex = 2;
			lblWorkingArea.Text = "Working Area (Canvas)";
			// 
			// lblXDimension
			// 
			lblXDimension.AutoSize = true;
			lblXDimension.Location = new System.Drawing.Point(93, 124);
			lblXDimension.Name = "lblXDimension";
			lblXDimension.Size = new System.Drawing.Size(96, 20);
			lblXDimension.TabIndex = 3;
			lblXDimension.Text = "&X Dimension:";
			// 
			// txtXDimension
			// 
			txtXDimension.Location = new System.Drawing.Point(195, 121);
			txtXDimension.Name = "txtXDimension";
			txtXDimension.Size = new System.Drawing.Size(105, 27);
			txtXDimension.TabIndex = 4;
			// 
			// lblUnits
			// 
			lblUnits.AutoSize = true;
			lblUnits.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblUnits.Location = new System.Drawing.Point(12, 9);
			lblUnits.Name = "lblUnits";
			lblUnits.Size = new System.Drawing.Size(137, 28);
			lblUnits.TabIndex = 0;
			lblUnits.Text = "Display Units";
			// 
			// optUS
			// 
			optUS.AutoSize = true;
			optUS.Checked = true;
			optUS.Location = new System.Drawing.Point(5, 3);
			optUS.Name = "optUS";
			optUS.Size = new System.Drawing.Size(120, 24);
			optUS.TabIndex = 0;
			optUS.TabStop = true;
			optUS.Text = "&U.S. (in, ft, ', \")";
			optUS.UseVisualStyleBackColor = true;
			// 
			// optMetric
			// 
			optMetric.AutoSize = true;
			optMetric.Location = new System.Drawing.Point(166, 3);
			optMetric.Name = "optMetric";
			optMetric.Size = new System.Drawing.Size(159, 24);
			optMetric.TabIndex = 1;
			optMetric.Text = "&Metric (mm, cm, m)";
			optMetric.UseVisualStyleBackColor = true;
			// 
			// lblYDimension
			// 
			lblYDimension.AutoSize = true;
			lblYDimension.Location = new System.Drawing.Point(28, 154);
			lblYDimension.Name = "lblYDimension";
			lblYDimension.Size = new System.Drawing.Size(95, 20);
			lblYDimension.TabIndex = 6;
			lblYDimension.Text = "&Y Dimension:";
			// 
			// txtYDimension
			// 
			txtYDimension.Location = new System.Drawing.Point(28, 177);
			txtYDimension.Name = "txtYDimension";
			txtYDimension.Size = new System.Drawing.Size(105, 27);
			txtYDimension.TabIndex = 7;
			// 
			// lblXYOrigin
			// 
			lblXYOrigin.AutoSize = true;
			lblXYOrigin.Location = new System.Drawing.Point(29, 361);
			lblXYOrigin.Name = "lblXYOrigin";
			lblXYOrigin.Size = new System.Drawing.Size(80, 20);
			lblXYOrigin.TabIndex = 13;
			lblXYOrigin.Text = "X/Y O&rigin:";
			// 
			// cmboXYOrigin
			// 
			cmboXYOrigin.FormattingEnabled = true;
			cmboXYOrigin.Items.AddRange(new object[] { "Center", "Top", "Top Left", "Left", "Bottom Left", "Bottom", "Bottom Right", "Right", "Top Right" });
			cmboXYOrigin.Location = new System.Drawing.Point(115, 358);
			cmboXYOrigin.Name = "cmboXYOrigin";
			cmboXYOrigin.Size = new System.Drawing.Size(137, 28);
			cmboXYOrigin.TabIndex = 14;
			// 
			// txtZDimension
			// 
			txtZDimension.Location = new System.Drawing.Point(28, 281);
			txtZDimension.Name = "txtZDimension";
			txtZDimension.Size = new System.Drawing.Size(105, 27);
			txtZDimension.TabIndex = 10;
			// 
			// lblZDimension
			// 
			lblZDimension.Location = new System.Drawing.Point(28, 240);
			lblZDimension.Name = "lblZDimension";
			lblZDimension.Size = new System.Drawing.Size(104, 52);
			lblZDimension.TabIndex = 9;
			lblZDimension.Text = "&Z Dimension (plunge):";
			// 
			// lblTravel
			// 
			lblTravel.AutoSize = true;
			lblTravel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblTravel.Location = new System.Drawing.Point(505, 9);
			lblTravel.Name = "lblTravel";
			lblTravel.Size = new System.Drawing.Size(69, 28);
			lblTravel.TabIndex = 20;
			lblTravel.Text = "Travel";
			// 
			// optXPRight
			// 
			optXPRight.AutoSize = true;
			optXPRight.Checked = true;
			optXPRight.Location = new System.Drawing.Point(3, 3);
			optXPRight.Name = "optXPRight";
			optXPRight.Size = new System.Drawing.Size(88, 24);
			optXPRight.TabIndex = 0;
			optXPRight.TabStop = true;
			optXPRight.Text = "X+ Right";
			optXPRight.UseVisualStyleBackColor = true;
			// 
			// optXPLeft
			// 
			optXPLeft.AutoSize = true;
			optXPLeft.Location = new System.Drawing.Point(112, 3);
			optXPLeft.Name = "optXPLeft";
			optXPLeft.Size = new System.Drawing.Size(78, 24);
			optXPLeft.TabIndex = 1;
			optXPLeft.Text = "X+ Left";
			optXPLeft.UseVisualStyleBackColor = true;
			// 
			// optYPUp
			// 
			optYPUp.AutoSize = true;
			optYPUp.Checked = true;
			optYPUp.Location = new System.Drawing.Point(3, 3);
			optYPUp.Name = "optYPUp";
			optYPUp.Size = new System.Drawing.Size(71, 24);
			optYPUp.TabIndex = 0;
			optYPUp.TabStop = true;
			optYPUp.Text = "Y+ Up";
			optYPUp.UseVisualStyleBackColor = true;
			// 
			// optYPDown
			// 
			optYPDown.AutoSize = true;
			optYPDown.Location = new System.Drawing.Point(112, 3);
			optYPDown.Name = "optYPDown";
			optYPDown.Size = new System.Drawing.Size(91, 24);
			optYPDown.TabIndex = 1;
			optYPDown.Text = "Y+ Down";
			optYPDown.UseVisualStyleBackColor = true;
			// 
			// optZPUp
			// 
			optZPUp.AutoSize = true;
			optZPUp.Checked = true;
			optZPUp.Location = new System.Drawing.Point(3, 3);
			optZPUp.Name = "optZPUp";
			optZPUp.Size = new System.Drawing.Size(72, 24);
			optZPUp.TabIndex = 0;
			optZPUp.TabStop = true;
			optZPUp.Text = "Z+ Up";
			optZPUp.UseVisualStyleBackColor = true;
			// 
			// optZPDown
			// 
			optZPDown.AutoSize = true;
			optZPDown.Location = new System.Drawing.Point(112, 3);
			optZPDown.Name = "optZPDown";
			optZPDown.Size = new System.Drawing.Size(92, 24);
			optZPDown.TabIndex = 1;
			optZPDown.Text = "Z+ Down";
			optZPDown.UseVisualStyleBackColor = true;
			// 
			// lblStockAccess
			// 
			lblStockAccess.AutoSize = true;
			lblStockAccess.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblStockAccess.Location = new System.Drawing.Point(505, 145);
			lblStockAccess.Name = "lblStockAccess";
			lblStockAccess.Size = new System.Drawing.Size(133, 28);
			lblStockAccess.TabIndex = 24;
			lblStockAccess.Text = "Stock Access";
			// 
			// lblStockAccessTip
			// 
			lblStockAccessTip.Location = new System.Drawing.Point(522, 176);
			lblStockAccessTip.Name = "lblStockAccessTip";
			lblStockAccessTip.Size = new System.Drawing.Size(200, 89);
			lblStockAccessTip.TabIndex = 25;
			lblStockAccessTip.Text = "On which axis are you able to feed stock larger than your bed for indexing, cut-in, or drilling operations?";
			// 
			// chkXOpenEnded
			// 
			chkXOpenEnded.AutoSize = true;
			chkXOpenEnded.Location = new System.Drawing.Point(522, 268);
			chkXOpenEnded.Name = "chkXOpenEnded";
			chkXOpenEnded.Size = new System.Drawing.Size(140, 24);
			chkXOpenEnded.TabIndex = 26;
			chkXOpenEnded.Text = "X is open-ended";
			chkXOpenEnded.UseVisualStyleBackColor = true;
			// 
			// chkYOpenEnded
			// 
			chkYOpenEnded.AutoSize = true;
			chkYOpenEnded.Location = new System.Drawing.Point(522, 298);
			chkYOpenEnded.Name = "chkYOpenEnded";
			chkYOpenEnded.Size = new System.Drawing.Size(139, 24);
			chkYOpenEnded.TabIndex = 27;
			chkYOpenEnded.Text = "Y is open-ended";
			chkYOpenEnded.UseVisualStyleBackColor = true;
			// 
			// pnlTravelX
			// 
			pnlTravelX.Controls.Add(optXPRight);
			pnlTravelX.Controls.Add(optXPLeft);
			pnlTravelX.Location = new System.Drawing.Point(522, 40);
			pnlTravelX.Name = "pnlTravelX";
			pnlTravelX.Size = new System.Drawing.Size(205, 32);
			pnlTravelX.TabIndex = 21;
			// 
			// pnlTravelY
			// 
			pnlTravelY.Controls.Add(optYPUp);
			pnlTravelY.Controls.Add(optYPDown);
			pnlTravelY.Location = new System.Drawing.Point(522, 73);
			pnlTravelY.Name = "pnlTravelY";
			pnlTravelY.Size = new System.Drawing.Size(205, 32);
			pnlTravelY.TabIndex = 22;
			// 
			// pnlTravelZ
			// 
			pnlTravelZ.Controls.Add(optZPUp);
			pnlTravelZ.Controls.Add(optZPDown);
			pnlTravelZ.Location = new System.Drawing.Point(522, 106);
			pnlTravelZ.Name = "pnlTravelZ";
			pnlTravelZ.Size = new System.Drawing.Size(205, 32);
			pnlTravelZ.TabIndex = 23;
			// 
			// pnlUnits
			// 
			pnlUnits.Controls.Add(optMetric);
			pnlUnits.Controls.Add(optUS);
			pnlUnits.Location = new System.Drawing.Point(29, 40);
			pnlUnits.Name = "pnlUnits";
			pnlUnits.Size = new System.Drawing.Size(327, 32);
			pnlUnits.TabIndex = 1;
			// 
			// lblTools
			// 
			lblTools.AutoSize = true;
			lblTools.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblTools.Location = new System.Drawing.Point(12, 386);
			lblTools.Name = "lblTools";
			lblTools.Size = new System.Drawing.Size(61, 28);
			lblTools.TabIndex = 17;
			lblTools.Text = "Tools";
			// 
			// lblGeneralCuttingTool
			// 
			lblGeneralCuttingTool.AutoSize = true;
			lblGeneralCuttingTool.Location = new System.Drawing.Point(28, 426);
			lblGeneralCuttingTool.Name = "lblGeneralCuttingTool";
			lblGeneralCuttingTool.Size = new System.Drawing.Size(115, 20);
			lblGeneralCuttingTool.TabIndex = 18;
			lblGeneralCuttingTool.Text = "&General Cutting:";
			// 
			// cmboGeneralCuttingTool
			// 
			cmboGeneralCuttingTool.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			cmboGeneralCuttingTool.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			cmboGeneralCuttingTool.FormattingEnabled = true;
			cmboGeneralCuttingTool.Items.AddRange(new object[] { "1/8in end mill", "1/4in end mill" });
			cmboGeneralCuttingTool.Location = new System.Drawing.Point(149, 423);
			cmboGeneralCuttingTool.Name = "cmboGeneralCuttingTool";
			cmboGeneralCuttingTool.Size = new System.Drawing.Size(199, 28);
			cmboGeneralCuttingTool.TabIndex = 19;
			// 
			// btnOK
			// 
			btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnOK.Location = new System.Drawing.Point(533, 470);
			btnOK.Name = "btnOK";
			btnOK.Size = new System.Drawing.Size(94, 32);
			btnOK.TabIndex = 31;
			btnOK.Text = "&OK";
			btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnCancel.Location = new System.Drawing.Point(633, 470);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(94, 32);
			btnCancel.TabIndex = 32;
			btnCancel.Text = "&Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// lblSystemSummary
			// 
			lblSystemSummary.AutoSize = true;
			lblSystemSummary.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblSystemSummary.Location = new System.Drawing.Point(505, 329);
			lblSystemSummary.Name = "lblSystemSummary";
			lblSystemSummary.Size = new System.Drawing.Size(176, 28);
			lblSystemSummary.TabIndex = 28;
			lblSystemSummary.Text = "System Summary";
			// 
			// txtSystemSummary
			// 
			txtSystemSummary.Location = new System.Drawing.Point(433, 360);
			txtSystemSummary.Multiline = true;
			txtSystemSummary.Name = "txtSystemSummary";
			txtSystemSummary.ReadOnly = true;
			txtSystemSummary.Size = new System.Drawing.Size(294, 91);
			txtSystemSummary.TabIndex = 29;
			txtSystemSummary.TabStop = false;
			txtSystemSummary.Text = "Display: U.S. units;\r\nWork area: 30.375 in x 30.375 in x 3 in;\r\nOrigin: X-Middle, Y-Middle, Z-Middle\r\n";
			// 
			// lblZOrigin
			// 
			lblZOrigin.AutoSize = true;
			lblZOrigin.Location = new System.Drawing.Point(258, 361);
			lblZOrigin.Name = "lblZOrigin";
			lblZOrigin.Size = new System.Drawing.Size(66, 20);
			lblZOrigin.TabIndex = 15;
			lblZOrigin.Text = "Z Or&igin:";
			// 
			// cmboZOrigin
			// 
			cmboZOrigin.FormattingEnabled = true;
			cmboZOrigin.Items.AddRange(new object[] { "Top", "Center", "Bottom" });
			cmboZOrigin.Location = new System.Drawing.Point(330, 358);
			cmboZOrigin.Name = "cmboZOrigin";
			cmboZOrigin.Size = new System.Drawing.Size(97, 28);
			cmboZOrigin.TabIndex = 16;
			// 
			// btnEditTools
			// 
			btnEditTools.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnEditTools.Location = new System.Drawing.Point(34, 470);
			btnEditTools.Name = "btnEditTools";
			btnEditTools.Size = new System.Drawing.Size(94, 32);
			btnEditTools.TabIndex = 30;
			btnEditTools.Text = "&Edit Tools";
			btnEditTools.UseVisualStyleBackColor = true;
			// 
			// lblXDimensionUnit
			// 
			lblXDimensionUnit.AutoSize = true;
			lblXDimensionUnit.Location = new System.Drawing.Point(306, 124);
			lblXDimensionUnit.Name = "lblXDimensionUnit";
			lblXDimensionUnit.Size = new System.Drawing.Size(18, 20);
			lblXDimensionUnit.TabIndex = 5;
			lblXDimensionUnit.Text = "...";
			// 
			// lblYDimensionUnit
			// 
			lblYDimensionUnit.AutoSize = true;
			lblYDimensionUnit.Location = new System.Drawing.Point(29, 207);
			lblYDimensionUnit.Name = "lblYDimensionUnit";
			lblYDimensionUnit.Size = new System.Drawing.Size(18, 20);
			lblYDimensionUnit.TabIndex = 8;
			lblYDimensionUnit.Text = "...";
			// 
			// lblZDimensionUnit
			// 
			lblZDimensionUnit.AutoSize = true;
			lblZDimensionUnit.Location = new System.Drawing.Point(34, 311);
			lblZDimensionUnit.Name = "lblZDimensionUnit";
			lblZDimensionUnit.Size = new System.Drawing.Size(18, 20);
			lblZDimensionUnit.TabIndex = 11;
			lblZDimensionUnit.Text = "...";
			// 
			// frmSettings
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(747, 523);
			Controls.Add(txtZDimension);
			Controls.Add(txtSystemSummary);
			Controls.Add(btnCancel);
			Controls.Add(btnEditTools);
			Controls.Add(btnOK);
			Controls.Add(pnlUnits);
			Controls.Add(pnlTravelZ);
			Controls.Add(pnlTravelY);
			Controls.Add(pnlTravelX);
			Controls.Add(chkYOpenEnded);
			Controls.Add(chkXOpenEnded);
			Controls.Add(lblStockAccessTip);
			Controls.Add(cmboGeneralCuttingTool);
			Controls.Add(lblGeneralCuttingTool);
			Controls.Add(cmboZOrigin);
			Controls.Add(lblZOrigin);
			Controls.Add(cmboXYOrigin);
			Controls.Add(lblXYOrigin);
			Controls.Add(lblZDimension);
			Controls.Add(lblYDimension);
			Controls.Add(txtYDimension);
			Controls.Add(txtXDimension);
			Controls.Add(lblZDimensionUnit);
			Controls.Add(lblYDimensionUnit);
			Controls.Add(lblXDimensionUnit);
			Controls.Add(lblXDimension);
			Controls.Add(lblUnits);
			Controls.Add(lblTools);
			Controls.Add(lblSystemSummary);
			Controls.Add(lblStockAccess);
			Controls.Add(lblTravel);
			Controls.Add(lblWorkingArea);
			Controls.Add(pnlCanvasArea);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "frmSettings";
			Text = "Settings";
			pnlTravelX.ResumeLayout(false);
			pnlTravelX.PerformLayout();
			pnlTravelY.ResumeLayout(false);
			pnlTravelY.PerformLayout();
			pnlTravelZ.ResumeLayout(false);
			pnlTravelZ.PerformLayout();
			pnlUnits.ResumeLayout(false);
			pnlUnits.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Panel pnlCanvasArea;
		private System.Windows.Forms.Label lblWorkingArea;
		private System.Windows.Forms.Label lblXDimension;
		private System.Windows.Forms.TextBox txtXDimension;
		private System.Windows.Forms.Label lblUnits;
		private System.Windows.Forms.RadioButton optUS;
		private System.Windows.Forms.RadioButton optMetric;
		private System.Windows.Forms.Label lblYDimension;
		private System.Windows.Forms.TextBox txtYDimension;
		private System.Windows.Forms.Label lblXYOrigin;
		private System.Windows.Forms.ComboBox cmboXYOrigin;
		private System.Windows.Forms.TextBox txtZDimension;
		private System.Windows.Forms.Label lblZDimension;
		private System.Windows.Forms.Label lblTravel;
		private System.Windows.Forms.RadioButton optXPRight;
		private System.Windows.Forms.RadioButton optXPLeft;
		private System.Windows.Forms.RadioButton optYPUp;
		private System.Windows.Forms.RadioButton optYPDown;
		private System.Windows.Forms.RadioButton optZPUp;
		private System.Windows.Forms.RadioButton optZPDown;
		private System.Windows.Forms.Label lblStockAccess;
		private System.Windows.Forms.Label lblStockAccessTip;
		private System.Windows.Forms.CheckBox chkXOpenEnded;
		private System.Windows.Forms.CheckBox chkYOpenEnded;
		private System.Windows.Forms.Panel pnlTravelX;
		private System.Windows.Forms.Panel pnlTravelY;
		private System.Windows.Forms.Panel pnlTravelZ;
		private System.Windows.Forms.Panel pnlUnits;
		private System.Windows.Forms.Label lblTools;
		private System.Windows.Forms.Label lblGeneralCuttingTool;
		private System.Windows.Forms.ComboBox cmboGeneralCuttingTool;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblSystemSummary;
		private System.Windows.Forms.TextBox txtSystemSummary;
		private System.Windows.Forms.Label lblZOrigin;
		private System.Windows.Forms.ComboBox cmboZOrigin;
		private System.Windows.Forms.Button btnEditTools;
		private System.Windows.Forms.Label lblXDimensionUnit;
		private System.Windows.Forms.Label lblYDimensionUnit;
		private System.Windows.Forms.Label lblZDimensionUnit;
	}
}