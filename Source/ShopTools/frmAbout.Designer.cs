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
	partial class frmAbout
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
			picBanner = new System.Windows.Forms.PictureBox();
			lblVersion = new System.Windows.Forms.Label();
			lblVersionValue = new System.Windows.Forms.Label();
			lblDateCompiled = new System.Windows.Forms.Label();
			lblDateCompiledValue = new System.Windows.Forms.Label();
			lblCopyright = new System.Windows.Forms.Label();
			btnClose = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)picBanner).BeginInit();
			SuspendLayout();
			// 
			// picBanner
			// 
			picBanner.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			picBanner.Image = (System.Drawing.Image)resources.GetObject("picBanner.Image");
			picBanner.Location = new System.Drawing.Point(12, 12);
			picBanner.Name = "picBanner";
			picBanner.Size = new System.Drawing.Size(523, 191);
			picBanner.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			picBanner.TabIndex = 0;
			picBanner.TabStop = false;
			// 
			// lblVersion
			// 
			lblVersion.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblVersion.AutoSize = true;
			lblVersion.Location = new System.Drawing.Point(12, 256);
			lblVersion.Name = "lblVersion";
			lblVersion.Size = new System.Drawing.Size(60, 20);
			lblVersion.TabIndex = 1;
			lblVersion.Text = "Version:";
			// 
			// lblVersionValue
			// 
			lblVersionValue.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblVersionValue.AutoSize = true;
			lblVersionValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblVersionValue.Location = new System.Drawing.Point(128, 256);
			lblVersionValue.Name = "lblVersionValue";
			lblVersionValue.Size = new System.Drawing.Size(57, 20);
			lblVersionValue.TabIndex = 2;
			lblVersionValue.Text = "1.0.0.0";
			// 
			// lblDateCompiled
			// 
			lblDateCompiled.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblDateCompiled.AutoSize = true;
			lblDateCompiled.Location = new System.Drawing.Point(12, 280);
			lblDateCompiled.Name = "lblDateCompiled";
			lblDateCompiled.Size = new System.Drawing.Size(113, 20);
			lblDateCompiled.TabIndex = 3;
			lblDateCompiled.Text = "Date Compiled:";
			// 
			// lblDateCompiledValue
			// 
			lblDateCompiledValue.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblDateCompiledValue.AutoSize = true;
			lblDateCompiledValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblDateCompiledValue.Location = new System.Drawing.Point(128, 280);
			lblDateCompiledValue.Name = "lblDateCompiledValue";
			lblDateCompiledValue.Size = new System.Drawing.Size(186, 20);
			lblDateCompiledValue.TabIndex = 4;
			lblDateCompiledValue.Text = "Tuesday, January 1, 1980";
			// 
			// lblCopyright
			// 
			lblCopyright.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblCopyright.AutoSize = true;
			lblCopyright.Location = new System.Drawing.Point(12, 216);
			lblCopyright.Name = "lblCopyright";
			lblCopyright.Size = new System.Drawing.Size(12, 20);
			lblCopyright.TabIndex = 0;
			lblCopyright.Text = ".";
			// 
			// btnClose
			// 
			btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnClose.Location = new System.Drawing.Point(441, 313);
			btnClose.Name = "btnClose";
			btnClose.Size = new System.Drawing.Size(94, 32);
			btnClose.TabIndex = 5;
			btnClose.Text = "&Close";
			btnClose.UseVisualStyleBackColor = true;
			// 
			// frmAbout
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			BackColor = System.Drawing.Color.White;
			ClientSize = new System.Drawing.Size(547, 356);
			Controls.Add(btnClose);
			Controls.Add(lblCopyright);
			Controls.Add(lblDateCompiled);
			Controls.Add(lblDateCompiledValue);
			Controls.Add(lblVersionValue);
			Controls.Add(lblVersion);
			Controls.Add(picBanner);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "frmAbout";
			Text = "About ShopTools";
			((System.ComponentModel.ISupportInitialize)picBanner).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.PictureBox picBanner;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Label lblVersionValue;
		private System.Windows.Forms.Label lblDateCompiled;
		private System.Windows.Forms.Label lblDateCompiledValue;
		private System.Windows.Forms.Label lblCopyright;
		private System.Windows.Forms.Button btnClose;
	}
}