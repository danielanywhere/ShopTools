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
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	frmEditPatterns																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Pattern editing and management form.
	/// </summary>
	public partial class frmEditTemplates : Form
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		/// <summary>
		/// Reference to the currently selected template entry.
		/// </summary>
		private PatternTemplateItem mEntry = null;
		/// <summary>
		/// Value indicating whether the currently selected entry and its fields
		/// are currently being processed.
		/// </summary>
		private bool mEntryBusy = false;

		//*-----------------------------------------------------------------------*
		//* btnCancel_Click																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Cancel button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Hide();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnCreate_Click																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Create button has been clicked. Create a new pattern entry and
		/// select it.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnCreate_Click(object sender, EventArgs e)
		{
			ListViewItem lvItem = null;
			PatternTemplateItem templateItem = new PatternTemplateItem();

			templateItem.TemplateName = "(Please name your pattern)";
			mPatterns.Add(templateItem);

			lvItem = new ListViewItem(templateItem.PatternTemplateId,
				"NoImageIcon.png");
			lvItem.Tag = templateItem;
			lvTemplates.Items.Add(lvItem);
			lvTemplates.SelectedItems.Clear();
			lvItem.Selected = true;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnDelete_Click																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Delete button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnDelete_Click(object sender, EventArgs e)
		{
			List<ListViewItem> items = null;

			if(lvTemplates.SelectedItems.Count > 0)
			{
				items = new List<ListViewItem>();
				foreach(ListViewItem lvItem in lvTemplates.SelectedItems)
				{
					items.Add(lvItem);
				}
				foreach(ListViewItem lvItem in items)
				{
					lvTemplates.Items.Remove(lvItem);
					if(lvItem.Tag is PatternTemplateItem @templateItem)
					{
						mPatterns.Remove(templateItem);
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnIconFilename_Click																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Icon Filename button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnIconFilename_Click(object sender, EventArgs e)
		{
			Bitmap bitmap = null;
			byte[] bytes = null;
			OpenFileDialog dialog = new OpenFileDialog();
			ListViewItem lvItem = null;

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = true;
			dialog.DefaultExt = ".png";
			dialog.DereferenceLinks = true;
			dialog.Filter =
				"Image Files " +
				"(*.bmp;*.jpeg;*.jpg;*.png)|" +
				"*.bmp;*.jpeg;*.jpg;*.png|" +
				"Bitmap Files " +
				"(*.bmp)|" +
				"*.bmp|" +
				"JPEG Files " +
				"(*.jpg;*.jpeg)|" +
				"*.jpg;*.jpeg|" +
				"PNG Files " +
				"(*.png)|" +
				"*.png|" +
				"All Files (*.*)|*.*";
			dialog.FilterIndex = 0;
			dialog.Multiselect = false;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = "Assign an Icon Image";
			dialog.ValidateNames = true;
			if(mEntry != null && dialog.ShowDialog() == DialogResult.OK)
			{
				mEntryBusy = true;
				if(dialog.FileName.Length > 0)
				{
					//	Filename was specified.
					bitmap = (Bitmap)Bitmap.FromFile(dialog.FileName);
					bitmap = ResizeImage(bitmap, 128, 128);
					txtIconFilename.Text =
						mEntry.IconFilename =
							GetUniqueUserDataImageName(Path.GetFileName(dialog.FileName));
					ilTemplates.Images.Add(mEntry.IconFilename, bitmap);
					using(MemoryStream stream = new MemoryStream())
					{
						bitmap.Save(stream, ImageFormat.Png);
						stream.Flush();
						bytes = stream.ToArray();
					}
					mEntry.IconFileData =
						GetDataUri(GetFileExtension(mEntry.IconFilename), bytes);
					if(lvTemplates.SelectedItems.Count > 0)
					{
						lvItem = lvTemplates.SelectedItems[0];
						lvItem.ImageKey = mEntry.IconFilename;
					}
				}
				mEntryBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnMoveDown_Click																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Move Down button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnMoveDown_Click(object sender, EventArgs e)
		{
			int index = 0;
			ListViewItem lvItem = null;

			//	NOTE: LargeIconView will only sort properly according to the
			//	backing list when you render the new order in Details view first.
			if(lvTemplates.SelectedIndices.Count == 1 &&
				lvTemplates.SelectedIndices[0] + 1 < lvTemplates.Items.Count)
			{
				mEntryBusy = true;
				index = lvTemplates.SelectedIndices[0];
				lvItem = lvTemplates.SelectedItems[0];
				lvTemplates.View = View.Details;
				lvTemplates.Items.Remove(lvItem);
				lvTemplates.Items.Insert(index + 1, lvItem);
				lvTemplates.View = View.LargeIcon;
				lvItem.ImageKey = mEntry.IconFilename;
				mPatterns.MoveDown(mEntry);
				mEntryBusy = false;
				lvTemplates_ItemSelectionChanged(this, null);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnMoveUp_Click																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Move Up button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnMoveUp_Click(object sender, EventArgs e)
		{
			int index = 0;
			ListViewItem lvItem = null;

			//	NOTE: LargeIconView will only sort properly according to the
			//	backing list when you render the new order in Details view first.
			if(lvTemplates.SelectedIndices.Count == 1 &&
				lvTemplates.SelectedIndices[0] > 0)
			{
				mEntryBusy = true;
				index = lvTemplates.SelectedIndices[0];
				lvItem = lvTemplates.SelectedItems[0];
				lvTemplates.View = View.Details;
				lvTemplates.Items.Remove(lvItem);
				lvTemplates.Items.Insert(index - 1, lvItem);
				lvTemplates.View = View.LargeIcon;
				lvItem.ImageKey = mEntry.IconFilename;
				mPatterns.MoveUp(mEntry);
				mEntryBusy = false;
				lvTemplates_ItemSelectionChanged(this, null);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnOK_Click																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The OK button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnOK_Click(object sender, EventArgs e)
		{
			//	All of the data in the local collection is already stored and ready
			//	to transfer when this event occurs.
			//	TODO: When saving changes, copy any new images to data folder.
			this.DialogResult = DialogResult.OK;
			this.Hide();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnOperationAdd_Click																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Add Operation button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnOperationAdd_Click(object sender, EventArgs e)
		{
			frmEditOperation dialog = new frmEditOperation();
			ListViewItem lvItem = null;
			PatternOperationItem operation = null;

			dialog.CreateOperation();
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				operation = dialog.Operation;
				if(mEntry != null)
				{
					mEntry.Operations.Add(operation);
				}
				lvItem = new ListViewItem(
					ExpandCamelCase(operation.Action.ToString()), 0);
				lvItem.SubItems.Add(operation.OperationName);
				lvItem.Tag = operation;
				lvOperations.Items.Add(lvItem);
				lvOperations.AutoResizeColumns(
					ColumnHeaderAutoResizeStyle.HeaderSize);
				lvOperations.AutoResizeColumns(
					ColumnHeaderAutoResizeStyle.ColumnContent);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnOperationDelete_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Delete Operation button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnOperationDelete_Click(object sender, EventArgs e)
		{
			List<ListViewItem> items = null;

			if(lvOperations.SelectedItems.Count > 0)
			{
				items = new List<ListViewItem>();
				foreach(ListViewItem listItem in lvOperations.SelectedItems)
				{
					items.Add(listItem);
				}
				foreach(ListViewItem listItem in items)
				{
					lvOperations.Items.Remove(listItem);
					if(mEntry != null &&
						listItem.Tag is PatternOperationItem @operationItem)
					{
						mEntry.Operations.Remove(operationItem);
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnOperationDown_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The move operation down button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnOperationDown_Click(object sender, EventArgs e)
		{
			int index = 0;
			ListViewItem lvItem = null;

			if(lvOperations.SelectedIndices.Count > 0 &&
				lvOperations.SelectedIndices[0] < lvOperations.Items.Count - 1)
			{
				index = lvOperations.SelectedIndices[0];
				lvItem = lvOperations.SelectedItems[0];
				lvOperations.Items.Remove(lvItem);
				lvOperations.Items.Insert(index + 1, lvItem);
				if(mEntry != null &&
					lvItem.Tag is PatternOperationItem @operationItem)
				{
					mEntry.Operations.MoveDown(operationItem);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnOperationEdit_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Operations button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnOperationEdit_Click(object sender, EventArgs e)
		{
			EditSelectedOperation();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnOperationUp_Click																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The move operation up button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnOperationUp_Click(object sender, EventArgs e)
		{
			int index = 0;
			ListViewItem lvItem = null;

			if(lvOperations.SelectedIndices.Count > 0 &&
				lvOperations.SelectedIndices[0] > 0)
			{
				index = lvOperations.SelectedIndices[0];
				lvItem = lvOperations.SelectedItems[0];
				lvOperations.Items.Remove(lvItem);
				lvOperations.Items.Insert(index - 1, lvItem);
				if(mEntry != null &&
					lvItem.Tag is PatternOperationItem @operationItem)
				{
					mEntry.Operations.MoveUp(operationItem);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnTemplateID_Click																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Template ID button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnTemplateID_Click(object sender, EventArgs e)
		{
			if(mEntry != null &&
				MessageBox.Show("Generate a new unique ID?", "Generate Unique ID",
					MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				mEntry.PatternTemplateId =
					txtTemplateID.Text = Guid.NewGuid().ToString("D");
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CheckSharedVariable																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Check the shared variable having the same value as that specified by
		/// the caller.
		/// </summary>
		/// <param name="variableName">
		/// Name of the entry in the shared variables list to check.
		/// </param>
		private void CheckSharedVariable(string variableName)
		{
			int index = 0;
			string lVariableName = "";

			if(variableName?.Length > 0)
			{
				lVariableName = variableName.ToLower();
				foreach(string listItem in lstSharedVariables.Items)
				{
					if(listItem.ToLower() == lVariableName)
					{
						lstSharedVariables.SetItemChecked(index, true);
						break;
					}
					index++;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* EditSelectedOperation																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Edit the selected operation item.
		/// </summary>
		private void EditSelectedOperation()
		{
			frmEditOperation dialog = null;
			ListViewItem lvItem = null;

			if(lvOperations.SelectedIndices.Count == 1)
			{
				lvItem = lvOperations.SelectedItems[0];
				if(lvItem.Tag is PatternOperationItem @operationItem)
				{
					dialog = new frmEditOperation();
					dialog.EditOperation(operationItem);
					if(dialog.ShowDialog() == DialogResult.OK)
					{
						PatternOperationItem.TransferValues(
							dialog.Operation, operationItem);
						lvItem.Text = ExpandCamelCase(operationItem.Action.ToString());
						lvItem.SubItems[1].Text = operationItem.OperationName;
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InitializePatterns																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Initialize all of the template pattern tools.
		/// </summary>
		private void InitializePatterns()
		{
			string filenameOnly = "";
			Bitmap icon = null;
			List<string> loadedIconFilenames = new List<string>();
			ListViewItem lvItem = null;

			ilTemplates.Images.Clear();
			filenameOnly = "NoImageIcon.png";
			icon = (Bitmap)Bitmap.FromFile(
				Path.Combine(UserDataPath, filenameOnly));
			ilTemplates.Images.Add(filenameOnly, icon);
			lvTemplates.Items.Clear();
			if(mPatterns != null)
			{
				foreach(PatternTemplateItem templateItem in mPatterns)
				{
					lvItem = null;
					if(templateItem.IconFilename?.Length > 0 &&
						File.Exists(Path.Combine(UserDataPath, templateItem.IconFilename)))
					{
						filenameOnly = Path.GetFileName(templateItem.IconFilename);
						icon = (Bitmap)Bitmap.FromFile(
							Path.Combine(UserDataPath, templateItem.IconFilename));
						ilTemplates.Images.Add(filenameOnly, icon);
						lvItem = new ListViewItem(templateItem.TemplateName, filenameOnly);
						lvItem.Tag = templateItem;
					}
					else
					{
						lvItem = new ListViewItem(templateItem.TemplateName, 0);
					}
					if(lvItem != null)
					{
						lvTemplates.Items.Add(lvItem);
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* lstSharedVariables_ItemCheck																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The checked state of an item has changed in the Shared Variables list.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Item check event arguments.
		/// </param>
		private void lstSharedVariables_ItemCheck(object sender,
			ItemCheckEventArgs e)
		{
			int count = 0;
			int index = 0;

			if(mEntry != null && !mEntryBusy)
			{
				mEntryBusy = true;
				mEntry.SharedVariables.Clear();
				count = lstSharedVariables.Items.Count;
				for(index = 0; index < count; index ++)
				{
					if(lstSharedVariables.GetItemChecked(index) ||
						(e.Index == index && e.NewValue == CheckState.Checked))
					{
						mEntry.SharedVariables.Add(
							lstSharedVariables.Items[index].ToString());
					}
				}
				mEntryBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* lvOperations_DoubleClick																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Operations list view has been double-clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void lvOperations_DoubleClick(object sender, EventArgs e)
		{
			EditSelectedOperation();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* lvOperations_SelectedIndexChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The item selection state has changed on the operations list view.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// List view item selection changed event arguments.
		/// </param>
		private void lvOperations_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(lvOperations.SelectedItems.Count > 0)
			{
				btnOperationEdit.Enabled = true;
				btnOperationDelete.Enabled = true;
				btnOperationDown.Enabled =
					(lvOperations.SelectedIndices[0] < lvOperations.Items.Count - 1);
				btnOperationUp.Enabled =
					(lvOperations.SelectedIndices[0] > 0);
			}
			else
			{
				btnOperationEdit.Enabled =
					btnOperationDelete.Enabled =
					btnOperationDown.Enabled =
					btnOperationUp.Enabled = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* lvTemplates_ItemSelectionChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The item selection state has changed on the templates list view.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// List view item selection changed event arguments.
		/// </param>
		private void lvTemplates_ItemSelectionChanged(object sender,
			ListViewItemSelectionChangedEventArgs e)
		{
			ListViewItem lvItem = null;

			if(!mEntryBusy)
			{
				mEntryBusy = true;
				lvOperations.Items.Clear();
				UncheckSharedVariables();
				if(lvTemplates.SelectedItems.Count > 0)
				{
					mEntry = (PatternTemplateItem)lvTemplates.SelectedItems[0].Tag;
					txtDisplayFormat.Text = mEntry.DisplayFormat;
					txtIconFilename.Text = mEntry.IconFilename;
					txtRemarks.Text = ToMultiLineString(mEntry.Remarks);
					txtTemplateID.Text = mEntry.PatternTemplateId;
					txtTemplateName.Text = mEntry.TemplateName;
					foreach(string entryItem in mEntry.SharedVariables)
					{
						CheckSharedVariable(entryItem);
					}
					foreach(PatternOperationItem operationItem in mEntry.Operations)
					{
						lvItem = new ListViewItem(
							ExpandCamelCase(operationItem.Action.ToString()), 0);
						lvItem.SubItems.Add(operationItem.OperationName);
						lvItem.Tag = operationItem;
						lvOperations.Items.Add(lvItem);
					}
					lvOperations.AutoResizeColumns(
						ColumnHeaderAutoResizeStyle.HeaderSize);
					lvOperations.AutoResizeColumns(
						ColumnHeaderAutoResizeStyle.ColumnContent);
				}
				else
				{
					mEntry = null;
					txtDisplayFormat.Text = "";
					txtIconFilename.Text = "";
					txtRemarks.Text = "";
					txtTemplateID.Text = "";
					txtTemplateName.Text = "";
				}
				if(lvTemplates.SelectedItems.Count > 0)
				{
					//Trace.WriteLine(
					//	"lvTemplates_ItemSelectionChanged. Selected items count: " +
					//	$"{lvTemplates.SelectedItems.Count} " +
					//	$"#{lvTemplates.SelectedIndices[0]}");
					btnDelete.Enabled = true;
					btnMoveDown.Enabled =
						(lvTemplates.SelectedIndices[0] < lvTemplates.Items.Count - 1);
					btnMoveUp.Enabled =
						(lvTemplates.SelectedIndices[0] > 0);
					btnOperationAdd.Enabled = true;
					btnIconFilename.Enabled = true;
					btnTemplateID.Enabled = true;
					//Trace.WriteLine($" Move down enabled: {btnMoveDown.Enabled}");
					//Trace.WriteLine($" Move up enabled:   {btnMoveUp.Enabled}");
				}
				else
				{
					btnDelete.Enabled =
						btnMoveDown.Enabled =
						btnMoveUp.Enabled = false;
					btnOperationAdd.Enabled =
						btnOperationDelete.Enabled =
						btnOperationDown.Enabled =
						btnOperationEdit.Enabled =
						btnOperationUp.Enabled = false;
					btnIconFilename.Enabled = false;
					btnTemplateID.Enabled = false;
				}
				mEntryBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtDisplayFormat_TextChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Display Format text has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtDisplayFormat_TextChanged(object sender, EventArgs e)
		{
			if(mEntry != null && !mEntryBusy)
			{
				mEntry.DisplayFormat = txtDisplayFormat.Text;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtIconFilename_TextChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Icon Filename text has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtIconFilename_TextChanged(object sender, EventArgs e)
		{
			ListViewItem lvItem = null;

			if(mEntry != null && !mEntryBusy)
			{
				mEntry.IconFilename = txtIconFilename.Text;
				if(lvTemplates.SelectedItems.Count > 0)
				{
					lvItem = lvTemplates.SelectedItems[0];
					if(ilTemplates.Images.ContainsKey(mEntry.IconFilename))
					{
						lvItem.ImageKey = mEntry.IconFilename;
					}
					else
					{
						lvItem.ImageIndex = 0;
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtRemarks_TextChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Remarks text has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtRemarks_TextChanged(object sender, EventArgs e)
		{
			if(mEntry != null && !mEntryBusy)
			{
				mEntry.Remarks.Clear();
				mEntry.Remarks.AddRange(FromMultiLineString(txtRemarks.Text));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtTemplateName_TextChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Template Name text has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtTemplateName_TextChanged(object sender, EventArgs e)
		{
			ListViewItem lvItem = null;

			if(mEntry != null && !mEntryBusy)
			{
				mEntry.TemplateName = txtTemplateName.Text;
				if(lvTemplates.SelectedItems.Count > 0)
				{
					lvItem = lvTemplates.SelectedItems[0];
					lvItem.Text = mEntry.TemplateName;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UncheckSharedVariables																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Uncheck all of the shared variables.
		/// </summary>
		private void UncheckSharedVariables()
		{
			int count = 0;
			int index = 0;

			count = lstSharedVariables.Items.Count;
			for(index = 0; index < count; index++)
			{
				lstSharedVariables.SetItemChecked(index, false);
			}

		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* OnLoad																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Load event when the form has been loaded and is ready to
		/// display for the first time.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnLoad(EventArgs e)
		{
			InitializePatterns();
			base.OnLoad(e);
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the frmEditPatterns Item.
		/// </summary>
		public frmEditTemplates()
		{
			InitializeComponent();

			this.CancelButton = btnCancel;
			this.AcceptButton = btnOK;

			//	Operations List View.
			//	Width of -2 indicates auto-size.
			lvOperations.View = View.Details;
			lvOperations.FullRowSelect = true;
			lvOperations.Columns.Add("Action", -2, HorizontalAlignment.Left);
			lvOperations.Columns.Add("Name", -2, HorizontalAlignment.Left);
			lvOperations.DoubleClick += lvOperations_DoubleClick;
			lvOperations.SelectedIndexChanged += lvOperations_SelectedIndexChanged;

			//	Shared variables list.
			foreach(OperationActionPropertyItem propertyItem in
				ConfigProfile.OperationActionProperties)
			{
				lstSharedVariables.Items.Add(propertyItem.PropertyName, false);
			}
			lstSharedVariables.ItemCheck += lstSharedVariables_ItemCheck;

			//	Buttons.
			btnCancel.Click += btnCancel_Click;
			btnCreate.Click += btnCreate_Click;
			btnDelete.Click += btnDelete_Click;
			btnDelete.Enabled = false;
			btnIconFilename.Click += btnIconFilename_Click;
			btnIconFilename.Enabled = false;
			btnMoveDown.Click += btnMoveDown_Click;
			btnMoveDown.Enabled = false;
			btnMoveUp.Click += btnMoveUp_Click;
			btnMoveUp.Enabled = false;
			btnOK.Click += btnOK_Click;
			btnOperationAdd.Click += btnOperationAdd_Click;
			btnOperationAdd.Enabled = false;
			btnOperationDelete.Click += btnOperationDelete_Click;
			btnOperationDelete.Enabled = false;
			btnOperationDown.Click += btnOperationDown_Click;
			btnOperationDown.Enabled = false;
			btnOperationEdit.Click += btnOperationEdit_Click;
			btnOperationEdit.Enabled = false;
			btnOperationUp.Click += btnOperationUp_Click;
			btnOperationUp.Enabled = false;
			btnTemplateID.Click += btnTemplateID_Click;
			btnTemplateID.Enabled = false;

			//	List Views.
			lvTemplates.ItemSelectionChanged += lvTemplates_ItemSelectionChanged;

			//	Text.
			txtDisplayFormat.TextChanged += txtDisplayFormat_TextChanged;
			txtIconFilename.TextChanged += txtIconFilename_TextChanged;
			txtRemarks.TextChanged += txtRemarks_TextChanged;
			txtTemplateName.TextChanged += txtTemplateName_TextChanged;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Patterns																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Patterns">Patterns</see>.
		/// </summary>
		private PatternTemplateCollection mPatterns = null;
		/// <summary>
		/// Get/Set a reference to the collection of pattern templates to be edited.
		/// </summary>
		/// <remarks>
		/// When this value is set by the caller, a local clone is created so any
		/// future use will be safe.
		/// </remarks>
		public PatternTemplateCollection Patterns
		{
			get { return mPatterns; }
			set
			{
				if(value != null)
				{
					mPatterns = PatternTemplateCollection.Clone(value);
				}
				else
				{
					mPatterns = value;
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
