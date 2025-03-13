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
	partial class frmMessage
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
			lblMessage = new System.Windows.Forms.Label();
			btn1 = new System.Windows.Forms.Button();
			btn2 = new System.Windows.Forms.Button();
			btn3 = new System.Windows.Forms.Button();
			SuspendLayout();
			// 
			// lblMessage
			// 
			lblMessage.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			lblMessage.Location = new System.Drawing.Point(12, 9);
			lblMessage.Name = "lblMessage";
			lblMessage.Size = new System.Drawing.Size(395, 111);
			lblMessage.TabIndex = 0;
			lblMessage.Text = "Prompt message.";
			// 
			// btn1
			// 
			btn1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btn1.Location = new System.Drawing.Point(12, 133);
			btn1.Name = "btn1";
			btn1.Size = new System.Drawing.Size(80, 32);
			btn1.TabIndex = 1;
			btn1.Text = "&Yes";
			btn1.UseVisualStyleBackColor = true;
			// 
			// btn2
			// 
			btn2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btn2.Location = new System.Drawing.Point(169, 133);
			btn2.Name = "btn2";
			btn2.Size = new System.Drawing.Size(80, 32);
			btn2.TabIndex = 2;
			btn2.Text = "&No";
			btn2.UseVisualStyleBackColor = true;
			// 
			// btn3
			// 
			btn3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btn3.Location = new System.Drawing.Point(327, 133);
			btn3.Name = "btn3";
			btn3.Size = new System.Drawing.Size(80, 32);
			btn3.TabIndex = 3;
			btn3.Text = "&Cancel";
			btn3.UseVisualStyleBackColor = true;
			// 
			// frmMessage
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(419, 177);
			ControlBox = false;
			Controls.Add(btn3);
			Controls.Add(btn2);
			Controls.Add(btn1);
			Controls.Add(lblMessage);
			MaximizeBox = false;
			MinimizeBox = false;
			Name = "frmMessage";
			Text = "Message";
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.Button btn1;
		private System.Windows.Forms.Button btn2;
		private System.Windows.Forms.Button btn3;
	}
}