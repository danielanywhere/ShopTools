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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	frmMessage																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// A replacement for MessageBox that centers on the calling form.
	/// </summary>
	public partial class frmMessage : Form
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* btn1_Click																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Button 1 has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btn1_Click(object sender, EventArgs e)
		{
			switch(mButtons)
			{
				case MessageBoxButtons.AbortRetryIgnore:
					this.DialogResult = DialogResult.Abort;
					break;
				case MessageBoxButtons.CancelTryContinue:
					this.DialogResult = DialogResult.Cancel;
					break;
				case MessageBoxButtons.OK:
					break;
				case MessageBoxButtons.OKCancel:
					this.DialogResult = DialogResult.OK;
					break;
				case MessageBoxButtons.RetryCancel:
					this.DialogResult = DialogResult.Retry;
					break;
				case MessageBoxButtons.YesNo:
					this.DialogResult = DialogResult.Yes;
					break;
				case MessageBoxButtons.YesNoCancel:
					this.DialogResult = DialogResult.Yes;
					break;
			}
			this.Hide();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btn2_Click																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Button 2 has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btn2_Click(object sender, EventArgs e)
		{
			switch(mButtons)
			{
				case MessageBoxButtons.AbortRetryIgnore:
					this.DialogResult = DialogResult.Retry;
					break;
				case MessageBoxButtons.CancelTryContinue:
					this.DialogResult = DialogResult.TryAgain;
					break;
				case MessageBoxButtons.OK:
					this.DialogResult = DialogResult.OK;
					break;
				case MessageBoxButtons.OKCancel:
					break;
				case MessageBoxButtons.RetryCancel:
					break;
				case MessageBoxButtons.YesNo:
					break;
				case MessageBoxButtons.YesNoCancel:
					this.DialogResult = DialogResult.No;
					break;
			}
			this.Hide();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btn3_Click																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Button 3 has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btn3_Click(object sender, EventArgs e)
		{
			switch(mButtons)
			{
				case MessageBoxButtons.AbortRetryIgnore:
					this.DialogResult = DialogResult.Ignore;
					break;
				case MessageBoxButtons.CancelTryContinue:
					this.DialogResult = DialogResult.Continue;
					break;
				case MessageBoxButtons.OK:
					break;
				case MessageBoxButtons.OKCancel:
					this.DialogResult = DialogResult.Cancel;
					break;
				case MessageBoxButtons.RetryCancel:
					this.DialogResult = DialogResult.Cancel;
					break;
				case MessageBoxButtons.YesNo:
					this.DialogResult = DialogResult.No;
					break;
				case MessageBoxButtons.YesNoCancel:
					this.DialogResult = DialogResult.Cancel;
					break;
			}
			this.Hide();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* frmMessage_Resize																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The form has resized.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void frmMessage_Resize(object sender, EventArgs e)
		{
			btn2.Left = (this.ClientSize.Width / 2) - (btn2.Width / 2);
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the frmMessage Form.
		/// </summary>
		public frmMessage()
		{
			InitializeComponent();

			this.StartPosition = FormStartPosition.CenterScreen;

			this.Resize += frmMessage_Resize;

			lblMessage.Text = "";

			//	Buttons.
			btn1.Click += btn1_Click;
			btn2.Click += btn2_Click;
			btn3.Click += btn3_Click;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the frmMessage Form.
		/// </summary>
		/// <param name="owner">
		/// Reference to the calling form.
		/// </param>
		public frmMessage(Form owner) : this()
		{
			if(owner != null)
			{
				this.StartPosition = FormStartPosition.CenterParent;
				this.Owner = owner;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Buttons																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Buttons">Buttons</see>.
		/// </summary>
		private MessageBoxButtons mButtons = MessageBoxButtons.YesNoCancel;
		/// <summary>
		/// Get/Set the button layout style for this instance.
		/// </summary>
		public MessageBoxButtons Buttons
		{
			get { return mButtons; }
			set
			{
				mButtons = value;
				switch(value)
				{
					case MessageBoxButtons.AbortRetryIgnore:
						btn1.Text = "&Abort";
						btn2.Text = "&Retry";
						btn3.Text = "&Ignore";
						this.CancelButton = btn1;
						this.AcceptButton = btn2;
						break;
					case MessageBoxButtons.CancelTryContinue:
						btn1.Text = "&Cancel";
						btn2.Text = "&Try";
						btn3.Text = "Con&tinue";
						this.CancelButton = btn1;
						this.AcceptButton = btn2;
						break;
					case MessageBoxButtons.OK:
						btn2.Text = "&OK";
						btn1.Visible = false;
						btn3.Visible = false;
						this.AcceptButton = btn2;
						break;
					case MessageBoxButtons.OKCancel:
						btn1.Text = "&OK";
						btn3.Text = "&Cancel";
						btn2.Visible = false;
						btn1.Left = (this.ClientSize.Width / 2) -
							(int)((float)btn1.Width * 1.5f);
						btn3.Left = (this.ClientSize.Width / 2) +
							(int)((float)btn1.Width * 0.5f);
						this.CancelButton = btn3;
						this.AcceptButton = btn1;
						break;
					case MessageBoxButtons.RetryCancel:
						btn1.Text = "&Retry";
						btn3.Text = "&Cancel";
						btn2.Visible = false;
						btn1.Left = (this.ClientSize.Width / 2) -
							(int)((float)btn1.Width * 1.5f);
						btn3.Left = (this.ClientSize.Width / 2) +
							(int)((float)btn1.Width * 0.5f);
						this.CancelButton = btn3;
						this.AcceptButton = btn1;
						break;
					case MessageBoxButtons.YesNo:
						btn1.Text = "&Yes";
						btn3.Text = "&No";
						btn2.Visible = false;
						btn1.Left = (this.ClientSize.Width / 2) -
							(int)((float)btn1.Width * 1.5f);
						btn3.Left = (this.ClientSize.Width / 2) +
							(int)((float)btn1.Width * 0.5f);
						this.CancelButton = btn3;
						this.AcceptButton = btn1;
						break;
					case MessageBoxButtons.YesNoCancel:
						btn1.Text = "&Yes";
						btn2.Text = "&No";
						btn3.Text = "&Cancel";
						this.CancelButton = btn3;
						this.AcceptButton = btn1;
						break;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Caption																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the message box title caption.
		/// </summary>
		public string Caption
		{
			get { return this.Text; }
			set
			{
				if(value?.Length > 0)
				{
					this.Text = value;
				}
				else if(this.Owner != null)
				{
					this.Text = this.Owner.Text;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MessageText																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the text of the message box message.
		/// </summary>
		public string MessageText
		{
			get { return lblMessage.Text; }
			set
			{
				if(value?.Length > 0)
				{
					lblMessage.Text = value;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Show																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Display the message box and return the result.
		/// </summary>
		/// <param name="owner">
		/// Reference to the calling form.
		/// </param>
		/// <param name="text">
		/// Text to display.
		/// </param>
		/// <param name="caption">
		/// Title caption.
		/// </param>
		/// <param name="buttons">
		/// Buttons allowed in this session.
		/// </param>
		/// <returns>
		/// The result corresponding to the clicked button.
		/// </returns>
		public static DialogResult Show(Form owner, string text, string caption,
			MessageBoxButtons buttons)
		{
			frmMessage dialog = new frmMessage(owner);
			DialogResult result = DialogResult.Cancel;

			dialog.MessageText = text;
			dialog.Caption = caption;
			dialog.Buttons = buttons;
			result = dialog.ShowDialog();
			return result;
		}

	}
	//*-------------------------------------------------------------------------*

}
