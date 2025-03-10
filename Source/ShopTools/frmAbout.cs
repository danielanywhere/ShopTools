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
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	frmAbout																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Dialog that displays information about the application.
	/// </summary>
	public partial class frmAbout : Form
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* btnClose_Click																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Close button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnClose_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Hide();
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
		/// Create a new instance of the frmAbout Item.
		/// </summary>
		public frmAbout()
		{
			DateTime date = DateTime.MinValue;
			string[] tuples = null;
			string version = Application.ProductVersion;

			InitializeComponent();

			this.StartPosition = FormStartPosition.CenterScreen;

			this.AcceptButton = btnClose;
			this.CancelButton = btnClose;

			btnClose.Click += btnClose_Click;

			lblCopyright.Text = ResourceMain.CopyrightMessage;
			lblVersionValue.Text = version;
			tuples = version.Split('.');
			lblDateCompiledValue.Text = new DateTime(
				2000 + ToInt(tuples[0]),
				ToInt(tuples[1].Substring(0, 2)) - 20,
				ToInt(tuples[1].Substring(2, 2))).ToString("dddd, MMMM d, yyyy");
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the frmAbout Item.
		/// </summary>
		/// <param name="callingForm">
		/// The calling form.
		/// </param>
		public frmAbout(Form callingForm) : this()
		{
			this.StartPosition = FormStartPosition.CenterParent;
			this.Owner = callingForm;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
