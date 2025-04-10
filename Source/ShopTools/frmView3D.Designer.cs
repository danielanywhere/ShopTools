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
	partial class frmView3D
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmView3D));
			menuView3D = new System.Windows.Forms.MenuStrip();
			lblToolStat = new System.Windows.Forms.Label();
			btnOK = new System.Windows.Forms.Button();
			btnCancel = new System.Windows.Forms.Button();
			pnlPreview = new System.Windows.Forms.Panel();
			lblStat = new System.Windows.Forms.Label();
			btnFirst = new System.Windows.Forms.Button();
			ilControls = new System.Windows.Forms.ImageList(components);
			btnPrevious = new System.Windows.Forms.Button();
			btnStop = new System.Windows.Forms.Button();
			btnPlay = new System.Windows.Forms.Button();
			btnPause = new System.Windows.Forms.Button();
			btnNext = new System.Windows.Forms.Button();
			btnLast = new System.Windows.Forms.Button();
			chkHideTracksUntilVisited = new System.Windows.Forms.CheckBox();
			chkTurntable = new System.Windows.Forms.CheckBox();
			chkLoopAnimation = new System.Windows.Forms.CheckBox();
			lblZMag = new System.Windows.Forms.Label();
			udZMag = new System.Windows.Forms.NumericUpDown();
			((System.ComponentModel.ISupportInitialize)udZMag).BeginInit();
			SuspendLayout();
			// 
			// menuView3D
			// 
			menuView3D.ImageScalingSize = new System.Drawing.Size(20, 20);
			menuView3D.Location = new System.Drawing.Point(0, 0);
			menuView3D.Name = "menuView3D";
			menuView3D.Size = new System.Drawing.Size(937, 24);
			menuView3D.TabIndex = 0;
			menuView3D.Text = "menuStrip1";
			// 
			// lblToolStat
			// 
			lblToolStat.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblToolStat.AutoSize = true;
			lblToolStat.Location = new System.Drawing.Point(15, 338);
			lblToolStat.Name = "lblToolStat";
			lblToolStat.Size = new System.Drawing.Size(142, 20);
			lblToolStat.TabIndex = 1;
			lblToolStat.Text = "Tool: None Selected";
			// 
			// btnOK
			// 
			btnOK.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnOK.Location = new System.Drawing.Point(727, 434);
			btnOK.Name = "btnOK";
			btnOK.Size = new System.Drawing.Size(96, 32);
			btnOK.TabIndex = 3;
			btnOK.Text = "&OK";
			btnOK.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
			btnCancel.Location = new System.Drawing.Point(829, 434);
			btnCancel.Name = "btnCancel";
			btnCancel.Size = new System.Drawing.Size(96, 32);
			btnCancel.TabIndex = 4;
			btnCancel.Text = "&Cancel";
			btnCancel.UseVisualStyleBackColor = true;
			// 
			// pnlPreview
			// 
			pnlPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			pnlPreview.BackColor = System.Drawing.Color.White;
			pnlPreview.Location = new System.Drawing.Point(12, 27);
			pnlPreview.Name = "pnlPreview";
			pnlPreview.Size = new System.Drawing.Size(913, 308);
			pnlPreview.TabIndex = 0;
			// 
			// lblStat
			// 
			lblStat.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblStat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lblStat.Location = new System.Drawing.Point(12, 362);
			lblStat.Name = "lblStat";
			lblStat.Size = new System.Drawing.Size(690, 24);
			lblStat.TabIndex = 2;
			lblStat.Text = "Generating Preview...";
			// 
			// btnFirst
			// 
			btnFirst.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btnFirst.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnFirst.ImageIndex = 0;
			btnFirst.ImageList = ilControls;
			btnFirst.Location = new System.Drawing.Point(17, 389);
			btnFirst.Name = "btnFirst";
			btnFirst.Size = new System.Drawing.Size(96, 32);
			btnFirst.TabIndex = 5;
			btnFirst.Text = "&First";
			btnFirst.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			btnFirst.UseVisualStyleBackColor = true;
			// 
			// ilControls
			// 
			ilControls.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			ilControls.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("ilControls.ImageStream");
			ilControls.TransparentColor = System.Drawing.Color.Transparent;
			ilControls.Images.SetKeyName(0, "TransportFirstButtonIcon.png");
			ilControls.Images.SetKeyName(1, "TransportLastButtonIcon.png");
			ilControls.Images.SetKeyName(2, "TransportNextButtonIcon.png");
			ilControls.Images.SetKeyName(3, "TransportPlayButtonIcon.png");
			ilControls.Images.SetKeyName(4, "TransportPrevButtonIcon.png");
			ilControls.Images.SetKeyName(5, "TransportStopButtonIcon.png");
			ilControls.Images.SetKeyName(6, "TransportPauseButtonIcon.png");
			// 
			// btnPrevious
			// 
			btnPrevious.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btnPrevious.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnPrevious.ImageIndex = 4;
			btnPrevious.ImageList = ilControls;
			btnPrevious.Location = new System.Drawing.Point(119, 389);
			btnPrevious.Name = "btnPrevious";
			btnPrevious.Size = new System.Drawing.Size(96, 32);
			btnPrevious.TabIndex = 6;
			btnPrevious.Text = "P&rev";
			btnPrevious.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			btnPrevious.UseVisualStyleBackColor = true;
			// 
			// btnStop
			// 
			btnStop.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btnStop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnStop.ImageIndex = 5;
			btnStop.ImageList = ilControls;
			btnStop.Location = new System.Drawing.Point(221, 389);
			btnStop.Name = "btnStop";
			btnStop.Size = new System.Drawing.Size(96, 32);
			btnStop.TabIndex = 7;
			btnStop.Text = "&Stop";
			btnStop.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			btnStop.UseVisualStyleBackColor = true;
			// 
			// btnPlay
			// 
			btnPlay.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btnPlay.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			btnPlay.ImageIndex = 3;
			btnPlay.ImageList = ilControls;
			btnPlay.Location = new System.Drawing.Point(323, 389);
			btnPlay.Name = "btnPlay";
			btnPlay.Size = new System.Drawing.Size(96, 32);
			btnPlay.TabIndex = 8;
			btnPlay.Text = "&Play";
			btnPlay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnPlay.UseVisualStyleBackColor = true;
			// 
			// btnPause
			// 
			btnPause.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btnPause.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			btnPause.ImageIndex = 6;
			btnPause.ImageList = ilControls;
			btnPause.Location = new System.Drawing.Point(425, 389);
			btnPause.Name = "btnPause";
			btnPause.Size = new System.Drawing.Size(96, 32);
			btnPause.TabIndex = 9;
			btnPause.Text = "Pa&use";
			btnPause.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnPause.UseVisualStyleBackColor = true;
			// 
			// btnNext
			// 
			btnNext.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btnNext.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			btnNext.ImageIndex = 2;
			btnNext.ImageList = ilControls;
			btnNext.Location = new System.Drawing.Point(527, 389);
			btnNext.Name = "btnNext";
			btnNext.Size = new System.Drawing.Size(96, 32);
			btnNext.TabIndex = 10;
			btnNext.Text = "&Next";
			btnNext.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnNext.UseVisualStyleBackColor = true;
			// 
			// btnLast
			// 
			btnLast.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			btnLast.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			btnLast.ImageIndex = 1;
			btnLast.ImageList = ilControls;
			btnLast.Location = new System.Drawing.Point(629, 389);
			btnLast.Name = "btnLast";
			btnLast.Size = new System.Drawing.Size(96, 32);
			btnLast.TabIndex = 11;
			btnLast.Text = "&Last";
			btnLast.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnLast.UseVisualStyleBackColor = true;
			// 
			// chkHideTracksUntilVisited
			// 
			chkHideTracksUntilVisited.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			chkHideTracksUntilVisited.AutoSize = true;
			chkHideTracksUntilVisited.Location = new System.Drawing.Point(17, 439);
			chkHideTracksUntilVisited.Name = "chkHideTracksUntilVisited";
			chkHideTracksUntilVisited.Size = new System.Drawing.Size(185, 24);
			chkHideTracksUntilVisited.TabIndex = 12;
			chkHideTracksUntilVisited.Text = "&Hide tracks until visited";
			chkHideTracksUntilVisited.UseVisualStyleBackColor = true;
			// 
			// chkTurntable
			// 
			chkTurntable.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			chkTurntable.AutoSize = true;
			chkTurntable.Checked = true;
			chkTurntable.CheckState = System.Windows.Forms.CheckState.Checked;
			chkTurntable.Location = new System.Drawing.Point(221, 439);
			chkTurntable.Name = "chkTurntable";
			chkTurntable.Size = new System.Drawing.Size(94, 24);
			chkTurntable.TabIndex = 13;
			chkTurntable.Text = "Turnt&able";
			chkTurntable.UseVisualStyleBackColor = true;
			// 
			// chkLoopAnimation
			// 
			chkLoopAnimation.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			chkLoopAnimation.AutoSize = true;
			chkLoopAnimation.Checked = true;
			chkLoopAnimation.CheckState = System.Windows.Forms.CheckState.Checked;
			chkLoopAnimation.Location = new System.Drawing.Point(323, 439);
			chkLoopAnimation.Name = "chkLoopAnimation";
			chkLoopAnimation.Size = new System.Drawing.Size(138, 24);
			chkLoopAnimation.TabIndex = 14;
			chkLoopAnimation.Text = "Loop Animation";
			chkLoopAnimation.UseVisualStyleBackColor = true;
			// 
			// lblZMag
			// 
			lblZMag.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			lblZMag.AutoSize = true;
			lblZMag.Location = new System.Drawing.Point(494, 440);
			lblZMag.Name = "lblZMag";
			lblZMag.Size = new System.Drawing.Size(55, 20);
			lblZMag.TabIndex = 15;
			lblZMag.Text = "&Z Mag:";
			// 
			// udZMag
			// 
			udZMag.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
			udZMag.Location = new System.Drawing.Point(555, 438);
			udZMag.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
			udZMag.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
			udZMag.Name = "udZMag";
			udZMag.Size = new System.Drawing.Size(68, 27);
			udZMag.TabIndex = 16;
			udZMag.Value = new decimal(new int[] { 10, 0, 0, 0 });
			// 
			// frmView3D
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(937, 478);
			Controls.Add(udZMag);
			Controls.Add(lblZMag);
			Controls.Add(chkTurntable);
			Controls.Add(chkLoopAnimation);
			Controls.Add(chkHideTracksUntilVisited);
			Controls.Add(pnlPreview);
			Controls.Add(btnCancel);
			Controls.Add(btnLast);
			Controls.Add(btnNext);
			Controls.Add(btnPause);
			Controls.Add(btnPlay);
			Controls.Add(btnStop);
			Controls.Add(btnPrevious);
			Controls.Add(btnFirst);
			Controls.Add(btnOK);
			Controls.Add(lblStat);
			Controls.Add(lblToolStat);
			Controls.Add(menuView3D);
			DoubleBuffered = true;
			MainMenuStrip = menuView3D;
			Name = "frmView3D";
			Text = "Preview 3D";
			((System.ComponentModel.ISupportInitialize)udZMag).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private System.Windows.Forms.MenuStrip menuView3D;
		private System.Windows.Forms.Label lblToolStat;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Panel pnlPreview;
		private System.Windows.Forms.Label lblStat;
		private System.Windows.Forms.Button btnFirst;
		private System.Windows.Forms.Button btnPrevious;
		private System.Windows.Forms.Button btnStop;
		private System.Windows.Forms.Button btnPlay;
		private System.Windows.Forms.Button btnPause;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnLast;
		private System.Windows.Forms.ImageList ilControls;
		private System.Windows.Forms.CheckBox chkHideTracksUntilVisited;
		private System.Windows.Forms.CheckBox chkTurntable;
		private System.Windows.Forms.CheckBox chkLoopAnimation;
		private System.Windows.Forms.Label lblZMag;
		private System.Windows.Forms.NumericUpDown udZMag;
	}
}