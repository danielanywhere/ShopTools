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
	partial class frmEditTools
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
			lblDefinedTools = new System.Windows.Forms.Label();
			lstDefinedTools = new System.Windows.Forms.ListBox();
			grpProperties = new System.Windows.Forms.GroupBox();
			txtTopGuideHeight = new System.Windows.Forms.TextBox();
			lblTopGuideHeight = new System.Windows.Forms.Label();
			txtTopGuideDiameter = new System.Windows.Forms.TextBox();
			lblTopGuideDiameter = new System.Windows.Forms.Label();
			txtShaftLength = new System.Windows.Forms.TextBox();
			lblShaftLength = new System.Windows.Forms.Label();
			txtFluteLength = new System.Windows.Forms.TextBox();
			lblFluteLength = new System.Windows.Forms.Label();
			txtFluteCount = new System.Windows.Forms.TextBox();
			lblFluteCount = new System.Windows.Forms.Label();
			txtDiameter = new System.Windows.Forms.TextBox();
			lblDiameter = new System.Windows.Forms.Label();
			txtBottomGuideHeight = new System.Windows.Forms.TextBox();
			lblBottomGuideHeight = new System.Windows.Forms.Label();
			txtBottomGuideDiameter = new System.Windows.Forms.TextBox();
			lblBottomGuideDiameter = new System.Windows.Forms.Label();
			lblAngleUnit = new System.Windows.Forms.Label();
			txtAngle = new System.Windows.Forms.TextBox();
			lblAngle = new System.Windows.Forms.Label();
			cmboToolType = new System.Windows.Forms.ComboBox();
			lblToolType = new System.Windows.Forms.Label();
			txtToolName = new System.Windows.Forms.TextBox();
			lblToolName = new System.Windows.Forms.Label();
			btnOK = new System.Windows.Forms.Button();
			btnCancel = new System.Windows.Forms.Button();
			btnAdd = new System.Windows.Forms.Button();
			btnDelete = new System.Windows.Forms.Button();
			lblBottomGuideDiameterUnit = new System.Windows.Forms.Label();
			lblBottomGuideHeightUnit = new System.Windows.Forms.Label();
			lblDiameterUnit = new System.Windows.Forms.Label();
			lblFluteCountUnit = new System.Windows.Forms.Label();
			lblFluteLengthUnit = new System.Windows.Forms.Label();
			lblShaftLengthUnit = new System.Windows.Forms.Label();
			lblTopGuideDiameterUnit = new System.Windows.Forms.Label();
			lblTopGuideHeightUnit = new System.Windows.Forms.Label();
			grpProperties.SuspendLayout();
			SuspendLayout();
			// 
			// lblDefinedTools
			// 
			lblDefinedTools.AutoSize = true;
			lblDefinedTools.Location = new System.Drawing.Point(12, 9);
			lblDefinedTools.Name = "lblDefinedTools";
			lblDefinedTools.Size = new System.Drawing.Size(104, 20);
			lblDefinedTools.TabIndex = 0;
			lblDefinedTools.Text = "Defined Tools:";
			// 
			// lstDefinedTools
			// 
			lstDefinedTools.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lstDefinedTools.FormattingEnabled = true;
			lstDefinedTools.Location = new System.Drawing.Point(12, 32);
			lstDefinedTools.Name = "lstDefinedTools";
			lstDefinedTools.Size = new System.Drawing.Size(295, 404);
			lstDefinedTools.TabIndex = 1;
			// 
			// grpProperties
			// 
			grpProperties.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			grpProperties.Controls.Add(txtTopGuideHeight);
			grpProperties.Controls.Add(lblTopGuideHeight);
			grpProperties.Controls.Add(txtTopGuideDiameter);
			grpProperties.Controls.Add(lblTopGuideDiameter);
			grpProperties.Controls.Add(txtShaftLength);
			grpProperties.Controls.Add(lblShaftLength);
			grpProperties.Controls.Add(txtFluteLength);
			grpProperties.Controls.Add(lblFluteLength);
			grpProperties.Controls.Add(txtFluteCount);
			grpProperties.Controls.Add(lblFluteCount);
			grpProperties.Controls.Add(txtDiameter);
			grpProperties.Controls.Add(lblDiameter);
			grpProperties.Controls.Add(txtBottomGuideHeight);
			grpProperties.Controls.Add(lblBottomGuideHeight);
			grpProperties.Controls.Add(txtBottomGuideDiameter);
			grpProperties.Controls.Add(lblBottomGuideDiameter);
			grpProperties.Controls.Add(lblTopGuideHeightUnit);
			grpProperties.Controls.Add(lblTopGuideDiameterUnit);
			grpProperties.Controls.Add(lblShaftLengthUnit);
			grpProperties.Controls.Add(lblFluteLengthUnit);
			grpProperties.Controls.Add(lblFluteCountUnit);
			grpProperties.Controls.Add(lblDiameterUnit);
			grpProperties.Controls.Add(lblBottomGuideHeightUnit);
			grpProperties.Controls.Add(lblBottomGuideDiameterUnit);
			grpProperties.Controls.Add(lblAngleUnit);
			grpProperties.Controls.Add(txtAngle);
			grpProperties.Controls.Add(lblAngle);
			grpProperties.Controls.Add(cmboToolType);
			grpProperties.Controls.Add(lblToolType);
			grpProperties.Controls.Add(txtToolName);
			grpProperties.Controls.Add(lblToolName);
			grpProperties.Location = new System.Drawing.Point(313, 32);
			grpProperties.Name = "grpProperties";
			grpProperties.Size = new System.Drawing.Size(475, 412);
			grpProperties.TabIndex = 2;
			grpProperties.TabStop = false;
			grpProperties.Text = "Properties";
			// 
			// txtTopGuideHeight
			// 
			txtTopGuideHeight.Location = new System.Drawing.Point(188, 361);
			txtTopGuideHeight.Name = "txtTopGuideHeight";
			txtTopGuideHeight.Size = new System.Drawing.Size(130, 27);
			txtTopGuideHeight.TabIndex = 29;
			// 
			// lblTopGuideHeight
			// 
			lblTopGuideHeight.AutoSize = true;
			lblTopGuideHeight.Location = new System.Drawing.Point(6, 361);
			lblTopGuideHeight.Name = "lblTopGuideHeight";
			lblTopGuideHeight.Size = new System.Drawing.Size(129, 20);
			lblTopGuideHeight.TabIndex = 28;
			lblTopGuideHeight.Text = "Top Guide Height:";
			// 
			// txtTopGuideDiameter
			// 
			txtTopGuideDiameter.Location = new System.Drawing.Point(188, 328);
			txtTopGuideDiameter.Name = "txtTopGuideDiameter";
			txtTopGuideDiameter.Size = new System.Drawing.Size(130, 27);
			txtTopGuideDiameter.TabIndex = 26;
			// 
			// lblTopGuideDiameter
			// 
			lblTopGuideDiameter.AutoSize = true;
			lblTopGuideDiameter.Location = new System.Drawing.Point(6, 328);
			lblTopGuideDiameter.Name = "lblTopGuideDiameter";
			lblTopGuideDiameter.Size = new System.Drawing.Size(146, 20);
			lblTopGuideDiameter.TabIndex = 25;
			lblTopGuideDiameter.Text = "Top Guide Diameter:";
			// 
			// txtShaftLength
			// 
			txtShaftLength.Location = new System.Drawing.Point(188, 295);
			txtShaftLength.Name = "txtShaftLength";
			txtShaftLength.Size = new System.Drawing.Size(130, 27);
			txtShaftLength.TabIndex = 23;
			// 
			// lblShaftLength
			// 
			lblShaftLength.AutoSize = true;
			lblShaftLength.Location = new System.Drawing.Point(6, 295);
			lblShaftLength.Name = "lblShaftLength";
			lblShaftLength.Size = new System.Drawing.Size(95, 20);
			lblShaftLength.TabIndex = 22;
			lblShaftLength.Text = "Shaft Length:";
			// 
			// txtFluteLength
			// 
			txtFluteLength.Location = new System.Drawing.Point(188, 262);
			txtFluteLength.Name = "txtFluteLength";
			txtFluteLength.Size = new System.Drawing.Size(130, 27);
			txtFluteLength.TabIndex = 20;
			// 
			// lblFluteLength
			// 
			lblFluteLength.AutoSize = true;
			lblFluteLength.Location = new System.Drawing.Point(6, 262);
			lblFluteLength.Name = "lblFluteLength";
			lblFluteLength.Size = new System.Drawing.Size(93, 20);
			lblFluteLength.TabIndex = 19;
			lblFluteLength.Text = "Flute Length:";
			// 
			// txtFluteCount
			// 
			txtFluteCount.Location = new System.Drawing.Point(188, 229);
			txtFluteCount.Name = "txtFluteCount";
			txtFluteCount.Size = new System.Drawing.Size(130, 27);
			txtFluteCount.TabIndex = 17;
			// 
			// lblFluteCount
			// 
			lblFluteCount.AutoSize = true;
			lblFluteCount.Location = new System.Drawing.Point(6, 229);
			lblFluteCount.Name = "lblFluteCount";
			lblFluteCount.Size = new System.Drawing.Size(87, 20);
			lblFluteCount.TabIndex = 16;
			lblFluteCount.Text = "Flute Count:";
			// 
			// txtDiameter
			// 
			txtDiameter.Location = new System.Drawing.Point(188, 196);
			txtDiameter.Name = "txtDiameter";
			txtDiameter.Size = new System.Drawing.Size(130, 27);
			txtDiameter.TabIndex = 14;
			// 
			// lblDiameter
			// 
			lblDiameter.AutoSize = true;
			lblDiameter.Location = new System.Drawing.Point(6, 196);
			lblDiameter.Name = "lblDiameter";
			lblDiameter.Size = new System.Drawing.Size(74, 20);
			lblDiameter.TabIndex = 13;
			lblDiameter.Text = "Diameter:";
			// 
			// txtBottomGuideHeight
			// 
			txtBottomGuideHeight.Location = new System.Drawing.Point(188, 163);
			txtBottomGuideHeight.Name = "txtBottomGuideHeight";
			txtBottomGuideHeight.Size = new System.Drawing.Size(130, 27);
			txtBottomGuideHeight.TabIndex = 11;
			// 
			// lblBottomGuideHeight
			// 
			lblBottomGuideHeight.AutoSize = true;
			lblBottomGuideHeight.Location = new System.Drawing.Point(6, 163);
			lblBottomGuideHeight.Name = "lblBottomGuideHeight";
			lblBottomGuideHeight.Size = new System.Drawing.Size(154, 20);
			lblBottomGuideHeight.TabIndex = 10;
			lblBottomGuideHeight.Text = "Bottom Guide Height:";
			// 
			// txtBottomGuideDiameter
			// 
			txtBottomGuideDiameter.Location = new System.Drawing.Point(188, 130);
			txtBottomGuideDiameter.Name = "txtBottomGuideDiameter";
			txtBottomGuideDiameter.Size = new System.Drawing.Size(130, 27);
			txtBottomGuideDiameter.TabIndex = 8;
			// 
			// lblBottomGuideDiameter
			// 
			lblBottomGuideDiameter.AutoSize = true;
			lblBottomGuideDiameter.Location = new System.Drawing.Point(6, 130);
			lblBottomGuideDiameter.Name = "lblBottomGuideDiameter";
			lblBottomGuideDiameter.Size = new System.Drawing.Size(171, 20);
			lblBottomGuideDiameter.TabIndex = 7;
			lblBottomGuideDiameter.Text = "Bottom Guide Diameter:";
			// 
			// lblAngleUnit
			// 
			lblAngleUnit.AutoSize = true;
			lblAngleUnit.Location = new System.Drawing.Point(324, 100);
			lblAngleUnit.Name = "lblAngleUnit";
			lblAngleUnit.Size = new System.Drawing.Size(62, 20);
			lblAngleUnit.TabIndex = 6;
			lblAngleUnit.Text = "degrees";
			// 
			// txtAngle
			// 
			txtAngle.Location = new System.Drawing.Point(188, 97);
			txtAngle.Name = "txtAngle";
			txtAngle.Size = new System.Drawing.Size(130, 27);
			txtAngle.TabIndex = 5;
			// 
			// lblAngle
			// 
			lblAngle.AutoSize = true;
			lblAngle.Location = new System.Drawing.Point(6, 97);
			lblAngle.Name = "lblAngle";
			lblAngle.Size = new System.Drawing.Size(51, 20);
			lblAngle.TabIndex = 4;
			lblAngle.Text = "Angle:";
			// 
			// cmboToolType
			// 
			cmboToolType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			cmboToolType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
			cmboToolType.FormattingEnabled = true;
			cmboToolType.Items.AddRange(new object[] { "Spiral Downcut End Mill", "Spiral Upcut End Mill", "Spiral Compression End Mill", "Chamfer Bit", "Core Box Bit", "Cove Bit", "Drill Bit", "Flush Trim Bit", "Rabbeting Bit", "Round Over Bit", "Straight Plunge Rounter Bit", "V-Groove Bit", "" });
			cmboToolType.Location = new System.Drawing.Point(97, 63);
			cmboToolType.Name = "cmboToolType";
			cmboToolType.Size = new System.Drawing.Size(349, 28);
			cmboToolType.TabIndex = 3;
			// 
			// lblToolType
			// 
			lblToolType.AutoSize = true;
			lblToolType.Location = new System.Drawing.Point(6, 66);
			lblToolType.Name = "lblToolType";
			lblToolType.Size = new System.Drawing.Size(76, 20);
			lblToolType.TabIndex = 2;
			lblToolType.Text = "Tool Type:";
			// 
			// txtToolName
			// 
			txtToolName.Location = new System.Drawing.Point(97, 30);
			txtToolName.Name = "txtToolName";
			txtToolName.Size = new System.Drawing.Size(349, 27);
			txtToolName.TabIndex = 1;
			// 
			// lblToolName
			// 
			lblToolName.AutoSize = true;
			lblToolName.Location = new System.Drawing.Point(6, 33);
			lblToolName.Name = "lblToolName";
			lblToolName.Size = new System.Drawing.Size(85, 20);
			lblToolName.TabIndex = 0;
			lblToolName.Text = "Tool Name:";
			// 
			// btnOK
			// 
			btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnOK.Location = new System.Drawing.Point(594, 463);
			btnOK.Name = "btnOK";
			btnOK.Size = new System.Drawing.Size(94, 32);
			btnOK.TabIndex = 5;
			btnOK.Text = "&OK";
			btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnCancel.Location = new System.Drawing.Point(694, 463);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(94, 32);
			btnCancel.TabIndex = 6;
			btnCancel.Text = "&Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnAdd
			// 
			btnAdd.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btnAdd.Location = new System.Drawing.Point(12, 463);
			btnAdd.Name = "btnAdd";
			btnAdd.Size = new System.Drawing.Size(94, 32);
			btnAdd.TabIndex = 3;
			btnAdd.Text = "&Add Tool";
			btnAdd.UseVisualStyleBackColor = true;
			// 
			// btnDelete
			// 
			btnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btnDelete.Location = new System.Drawing.Point(112, 463);
			btnDelete.Name = "btnDelete";
			btnDelete.Size = new System.Drawing.Size(94, 32);
			btnDelete.TabIndex = 4;
			btnDelete.Text = "&Delete";
			btnDelete.UseVisualStyleBackColor = true;
			// 
			// lblBottomGuideDiameterUnit
			// 
			lblBottomGuideDiameterUnit.AutoSize = true;
			lblBottomGuideDiameterUnit.Location = new System.Drawing.Point(324, 133);
			lblBottomGuideDiameterUnit.Name = "lblBottomGuideDiameterUnit";
			lblBottomGuideDiameterUnit.Size = new System.Drawing.Size(18, 20);
			lblBottomGuideDiameterUnit.TabIndex = 6;
			lblBottomGuideDiameterUnit.Text = "...";
			// 
			// lblBottomGuideHeightUnit
			// 
			lblBottomGuideHeightUnit.AutoSize = true;
			lblBottomGuideHeightUnit.Location = new System.Drawing.Point(324, 166);
			lblBottomGuideHeightUnit.Name = "lblBottomGuideHeightUnit";
			lblBottomGuideHeightUnit.Size = new System.Drawing.Size(18, 20);
			lblBottomGuideHeightUnit.TabIndex = 6;
			lblBottomGuideHeightUnit.Text = "...";
			// 
			// lblDiameterUnit
			// 
			lblDiameterUnit.AutoSize = true;
			lblDiameterUnit.Location = new System.Drawing.Point(324, 199);
			lblDiameterUnit.Name = "lblDiameterUnit";
			lblDiameterUnit.Size = new System.Drawing.Size(18, 20);
			lblDiameterUnit.TabIndex = 6;
			lblDiameterUnit.Text = "...";
			// 
			// lblFluteCountUnit
			// 
			lblFluteCountUnit.AutoSize = true;
			lblFluteCountUnit.Location = new System.Drawing.Point(324, 232);
			lblFluteCountUnit.Name = "lblFluteCountUnit";
			lblFluteCountUnit.Size = new System.Drawing.Size(18, 20);
			lblFluteCountUnit.TabIndex = 6;
			lblFluteCountUnit.Text = "...";
			// 
			// lblFluteLengthUnit
			// 
			lblFluteLengthUnit.AutoSize = true;
			lblFluteLengthUnit.Location = new System.Drawing.Point(324, 265);
			lblFluteLengthUnit.Name = "lblFluteLengthUnit";
			lblFluteLengthUnit.Size = new System.Drawing.Size(18, 20);
			lblFluteLengthUnit.TabIndex = 6;
			lblFluteLengthUnit.Text = "...";
			// 
			// lblShaftLengthUnit
			// 
			lblShaftLengthUnit.AutoSize = true;
			lblShaftLengthUnit.Location = new System.Drawing.Point(324, 298);
			lblShaftLengthUnit.Name = "lblShaftLengthUnit";
			lblShaftLengthUnit.Size = new System.Drawing.Size(18, 20);
			lblShaftLengthUnit.TabIndex = 6;
			lblShaftLengthUnit.Text = "...";
			// 
			// lblTopGuideDiameterUnit
			// 
			lblTopGuideDiameterUnit.AutoSize = true;
			lblTopGuideDiameterUnit.Location = new System.Drawing.Point(324, 331);
			lblTopGuideDiameterUnit.Name = "lblTopGuideDiameterUnit";
			lblTopGuideDiameterUnit.Size = new System.Drawing.Size(18, 20);
			lblTopGuideDiameterUnit.TabIndex = 6;
			lblTopGuideDiameterUnit.Text = "...";
			// 
			// lblTopGuideHeightUnit
			// 
			lblTopGuideHeightUnit.AutoSize = true;
			lblTopGuideHeightUnit.Location = new System.Drawing.Point(324, 364);
			lblTopGuideHeightUnit.Name = "lblTopGuideHeightUnit";
			lblTopGuideHeightUnit.Size = new System.Drawing.Size(18, 20);
			lblTopGuideHeightUnit.TabIndex = 6;
			lblTopGuideHeightUnit.Text = "...";
			// 
			// frmEditTools
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(800, 512);
			Controls.Add(btnCancel);
			Controls.Add(btnDelete);
			Controls.Add(btnAdd);
			Controls.Add(btnOK);
			Controls.Add(grpProperties);
			Controls.Add(lstDefinedTools);
			Controls.Add(lblDefinedTools);
			Name = "frmEditTools";
			Text = "Edit Tools";
			grpProperties.ResumeLayout(false);
			grpProperties.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label lblDefinedTools;
		private System.Windows.Forms.ListBox lstDefinedTools;
		private System.Windows.Forms.GroupBox grpProperties;
		private System.Windows.Forms.Label lblToolType;
		private System.Windows.Forms.TextBox txtToolName;
		private System.Windows.Forms.Label lblToolName;
		private System.Windows.Forms.ComboBox cmboToolType;
		private System.Windows.Forms.Label lblDiameter;
		private System.Windows.Forms.TextBox txtDiameter;
		private System.Windows.Forms.Label lblFluteLength;
		private System.Windows.Forms.TextBox txtFluteLength;
		private System.Windows.Forms.Label lblAngleUnit;
		private System.Windows.Forms.Label lblAngle;
		private System.Windows.Forms.TextBox txtAngle;
		private System.Windows.Forms.Label lblFluteCount;
		private System.Windows.Forms.Label lblTopGuideHeight;
		private System.Windows.Forms.Label lblBottomGuideHeight;
		private System.Windows.Forms.Label lblTopGuideDiameter;
		private System.Windows.Forms.Label lblBottomGuideDiameter;
		private System.Windows.Forms.TextBox txtFluteCount;
		private System.Windows.Forms.TextBox txtTopGuideHeight;
		private System.Windows.Forms.TextBox txtBottomGuideHeight;
		private System.Windows.Forms.TextBox txtTopGuideDiameter;
		private System.Windows.Forms.TextBox txtBottomGuideDiameter;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.TextBox txtShaftLength;
		private System.Windows.Forms.Label lblShaftLength;
		private System.Windows.Forms.Label lblTopGuideHeightUnit;
		private System.Windows.Forms.Label lblTopGuideDiameterUnit;
		private System.Windows.Forms.Label lblShaftLengthUnit;
		private System.Windows.Forms.Label lblFluteLengthUnit;
		private System.Windows.Forms.Label lblFluteCountUnit;
		private System.Windows.Forms.Label lblDiameterUnit;
		private System.Windows.Forms.Label lblBottomGuideHeightUnit;
		private System.Windows.Forms.Label lblBottomGuideDiameterUnit;
	}
}