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

#define NoInternalTest

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Nodes;
using System.Windows.Forms;

using Geometry;
using Newtonsoft.Json;
using SvgPlotting;
using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	frmMain																																	*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Main form of the application.
	/// </summary>
	public partial class frmMain : Form
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//private bool mControlBusy = false;
		//private bool mPaintEnabled = true;
		//private Timer mPaintTimer = null;
		/// <summary>
		/// The default title bar text on the form.
		/// </summary>
		private string mBaseFormText = "";
		/// <summary>
		/// The first button row Y offset beneath the cut-list, which will be
		/// adjustable in height.
		/// </summary>
		private int mCutButtonRowY1Offset = 0;
		/// <summary>
		/// The second button row Y offset beneath the cut-list, which will be
		/// adjustable in height.
		/// </summary>
		private int mCutButtonRowY2Offset = 0;
		/// <summary>
		/// The third button row Y offset beneath the cut-list, which will be
		/// adjustable in height.
		/// </summary>
		private int mCutButtonRowY3Offset = 0;
		/// <summary>
		/// A value indicating whether values in the current cut-list have changed.
		/// </summary>
		private bool mCutListChanged = false;
		/// <summary>
		/// Filename of the currently loaded cut-list file.
		/// </summary>
		private string mCutListFilename = "";
		/// <summary>
		/// Minimum height of the cut-list control.
		/// </summary>
		private int mCutListMinHeight = 0;
		/// <summary>
		/// Value indicating whether the control panel is toggled.
		/// </summary>
		private bool mPanelControlToggle = false;
		/// <summary>
		/// Current control panel width, when visible.
		/// </summary>
		private int mPanelControlWidth = 0;
		/// <summary>
		/// Value indicating whether the workpiece panel is toggled.
		/// </summary>
		private bool mPanelWorkpieceToggle = false;
		/// <summary>
		/// Current workpiece panel width, when visible.
		/// </summary>
		private int mPanelWorkpieceWidth = 0;
		//private PointF mRouterLocation = new PointF();
		/// <summary>
		/// Timer for addressing a request to toggle the width of a panel.
		/// </summary>
		/// <remarks>
		/// This timer is necessary for resetting the widths of the requested
		/// panels outside of the event chain fired upon the double-click of the
		/// associated splitter.
		/// </remarks>
		private Timer mToggleTimer = null;
		//private RectangleF mWorkpieceArea = new RectangleF();
		/// <summary>
		/// Value indicating whether the workpiece is currently busy and should
		/// not respond to related events, which are probably its own.
		/// </summary>
		private bool mWorkpieceBusy = false;
		//private WorkpieceInfoItem mWorkpieceInfo = new WorkpieceInfoItem();

		//*-----------------------------------------------------------------------*
		//* btnDeleteCut_Click																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Delete Cut button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnDeleteCut_Click(object sender, EventArgs e)
		{
			List<ListViewItem> items = new List<ListViewItem>();

			foreach(ListViewItem listItem in lvCutList.SelectedItems)
			{
				items.Add(listItem);
			}
			foreach(ListViewItem listItem in items)
			{
				lvCutList.Items.Remove(listItem);
				if(listItem.Tag is CutProfileItem @cutItem)
				{
					SessionWorkpieceInfo.Cuts.Remove(cutItem);
				}
			}
			this.Refresh();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnDuplicateCut_Click																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Duplicate Cut button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnDuplicateCut_Click(object sender, EventArgs e)
		{
			DuplicateCut();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnEditCut_Click																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit Cut button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnEditCut_Click(object sender, EventArgs e)
		{
			EditCut();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnGO_Click																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The GO button has been clicked. Run the procedure.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnGO_Click(object sender, EventArgs e)
		{
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnMoveCutDown_Click																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Move the selected cut down in the cut-list one space.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnMoveCutDown_Click(object sender, EventArgs e)
		{
			int index = 0;
			ListViewItem lvItem = null;

			if(lvCutList.SelectedIndices.Count == 1 &&
				lvCutList.SelectedIndices[0] + 1 < lvCutList.Items.Count)
			{
				index = lvCutList.SelectedIndices[0] + 1;
				lvItem = lvCutList.SelectedItems[0];
				lvCutList.Items.Remove(lvItem);
				lvCutList.Items.Insert(index, lvItem);
				if(lvItem.Tag is CutProfileItem @cutProfileItem)
				{
					SessionWorkpieceInfo.Cuts.MoveDown(cutProfileItem);
				}
				mCutListChanged = true;
				CalculateLayout();
				UpdateForm();
				this.Refresh();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnMoveCutUp_Click																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Move the selected cut up in the cut-list one space.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnMoveCutUp_Click(object sender, EventArgs e)
		{
			int index = 0;
			ListViewItem lvItem = null;

			if(lvCutList.SelectedIndices.Count == 1 &&
				lvCutList.SelectedIndices[0] - 1 > -1)
			{
				index = lvCutList.SelectedIndices[0] - 1;
				lvItem = lvCutList.SelectedItems[0];
				lvCutList.Items.Remove(lvItem);
				lvCutList.Items.Insert(index, lvItem);
				if(lvItem.Tag is CutProfileItem @cutProfileItem)
				{
					SessionWorkpieceInfo.Cuts.MoveUp(cutProfileItem);
				}
				mCutListChanged = true;
				CalculateLayout();
				UpdateForm();
				this.Refresh();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* btnStop_Click																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Stop button has been clicked. Stop all motors.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnStop_Click(object sender, EventArgs e)
		{
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* cmboMaterialType_SelectedIndexChanged																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selected index has changed on the Material Type combo box.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void cmboMaterialType_SelectedIndexChanged(object sender,
			EventArgs e)
		{
			UpdateWorkpiece();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* cmboWorkpieceLeft_SelectedIndexChanged																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selected index has changed on the Workpiece Left combo box.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void cmboWorkpieceLeft_SelectedIndexChanged(object sender,
			EventArgs e)
		{
			//if(!mControlBusy)
			//{
			//	mControlBusy = true;
			//	txtWorkpieceX_TextChanged(sender, e);
			//	mControlBusy = false;
			//}
			UpdateWorkpiece();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* cmboWorkpieceTop_SelectedIndexChanged																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selected index has changed on the Workpiece Top combo box.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void cmboWorkpieceTop_SelectedIndexChanged(object sender,
			EventArgs e)
		{
			//if(!mControlBusy)
			//{
			//	mControlBusy = true;
			//	txtWorkpieceY_TextChanged(sender, e);
			//	mControlBusy = false;
			//}
			UpdateWorkpiece();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CreateCut																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a cut entry for specified pattern template item.
		/// </summary>
		/// <param name="pattern">
		/// Reference to the pattern template item for which the cut will be
		/// defined.
		/// </param>
		private void CreateCut(PatternTemplateItem pattern)
		{
			frmCutEdit dialog = null;

			if(pattern != null)
			{
				dialog = new frmCutEdit();
				//dialog.WorkpieceInfo = WorkpieceInfoItem.Clone(SessionWorkpieceInfo);
				if(lvCutList.Items.Count == 0)
				{
					//	No cuts are defined. Use the global router starting position.
					dialog.CreateFromTemplate(pattern,
						SessionWorkpieceInfo.RouterLocation);
				}
				else
				{
					//	Cuts have been defined. Use the end location of the last cut.
					dialog.CreateFromTemplate(pattern,
						((CutProfileItem)lvCutList.Items[^1].Tag).EndLocation);
				}
				if(dialog.ShowDialog() == DialogResult.OK)
				{
					//	Cut was stored.
					SessionWorkpieceInfo.Cuts.Add(dialog.CutProfile);
					UpdateCutList();
					mCutListChanged = true;
					UpdateForm();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DuplicateCut																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Duplicate the selected cuts.
		/// </summary>
		private void DuplicateCut()
		{
			bool bChanged = false;
			List<ListViewItem> items = new List<ListViewItem>();

			if(lvCutList.SelectedItems.Count > 0)
			{
				foreach(ListViewItem listItem in lvCutList.SelectedItems)
				{
					if(listItem.Tag is CutProfileItem @cut)
					{
						SessionWorkpieceInfo.Cuts.Add(CutProfileItem.Clone(cut));
						bChanged = true;
					}
				}
				if(bChanged)
				{
					UpdateCutList();
					mCutListChanged = true;
					UpdateForm();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* EditCut																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Edit the first selected cut.
		/// </summary>
		private void EditCut()
		{
			frmCutEdit dialog = null;

			if(lvCutList.SelectedItems.Count > 0)
			{
				if(lvCutList.SelectedItems[0].Tag is CutProfileItem @cutItem)
				{
					dialog = new frmCutEdit();
					dialog.EditCut(cutItem);
					if(dialog.ShowDialog() == DialogResult.OK)
					{
						//	During editing, the original item's values were cloned.
						//	Transfer those values back to the original.
						CutProfileItem.TransferValues(dialog.CutProfile, cutItem);
						mCutListChanged = true;
						CalculateLayout();
						UpdateForm();
						this.Refresh();
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ExportGCode																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Export the contents of the current cut-list to g-code.
		/// </summary>
		/// <param name="filename">
		/// Path and filename of the G-code file to create.
		/// </param>
		private void ExportGCode(string filename)
		{
			List<string> contents = null;
			int count = 0;
			string extension = "";
			FileInfo fileInfo = null;
			string filenameOnly = "";
			string localFilename = "";
			string path = "";
			string s = "";

			if(SessionWorkpieceInfo != null)
			{
				fileInfo = new FileInfo(filename);
				filenameOnly = fileInfo.Name;
				path = fileInfo.DirectoryName;
				extension = fileInfo.Extension;
				if(extension.Length > 0)
				{
					filenameOnly =
						filenameOnly.Substring(0, filenameOnly.Length - extension.Length);
				}
				contents = GCode.RenderGCode(filenameOnly, extension);
				count = contents.Count;
				if(count == 1)
				{
					localFilename = filename;
					File.WriteAllText(localFilename, contents[0]);
				}
				else
				{
					foreach(string contentItem in contents)
					{
						localFilename =
							GetValue(contentItem,
							ResourceMain.rxGCodeRemarkEmbeddedFilename,
							"filename");
						File.WriteAllText(
							Path.Combine(path, localFilename) + extension, contentItem);
					}
				}
				s = (count == 1 ? "" : "s");
				statMessage.Text =
					$"G-code exported to {Path.GetFileName(filename)} as " +
					$"{count} file{s}...";
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

			ilPatterns.Images.Clear();
			ilPatternsSmall.Images.Clear();
			filenameOnly = "NoImageIcon.png";
			icon = (Bitmap)Bitmap.FromFile(
				Path.Combine(UserDataPath, filenameOnly));
			ilPatterns.Images.Add(filenameOnly, icon);
			lvPatterns.Items.Clear();
			foreach(PatternTemplateItem templateItem in
				ConfigProfile.PatternTemplates)
			{
				lvItem = null;
				if(templateItem.IconFilename?.Length > 0 &&
					File.Exists(Path.Combine(UserDataPath, templateItem.IconFilename)))
				{
					filenameOnly = Path.GetFileName(templateItem.IconFilename);
					icon = (Bitmap)Bitmap.FromFile(
						Path.Combine(UserDataPath, templateItem.IconFilename));
					ilPatterns.Images.Add(filenameOnly, icon);
					//	Draw the small version of the image.
					icon = ResizeImage(icon, 24, 24);
					ilPatternsSmall.Images.Add(filenameOnly, icon);
					lvItem = new ListViewItem(templateItem.TemplateName, filenameOnly);
				}
				else if(templateItem.IconFileData?.Length > 0)
				{
					//	Template has a DataURI image.
					icon = GetBitmapFromDataUri(templateItem.IconFileData);
					if(icon != null)
					{
						if(templateItem.IconFilename.Length == 0)
						{
							templateItem.IconFilename = templateItem.PatternTemplateId;
						}
						ilPatterns.Images.Add(templateItem.IconFilename, icon);
						//	Draw the small version of the image.
						icon = ResizeImage(icon, 24, 24);
						ilPatternsSmall.Images.Add(templateItem.IconFilename,
							icon);
						lvItem = new ListViewItem(
							templateItem.TemplateName, templateItem.IconFilename);
					}
				}
				else
				{
					lvItem = new ListViewItem(templateItem.TemplateName, 0);
				}
				if(lvItem != null)
				{
					lvItem.Tag = templateItem;
					lvPatterns.Items.Add(lvItem);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* lvCutList_DoubleClick																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The cut list has received a double-click. Edit the selected item.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void lvCutList_DoubleClick(object sender, EventArgs e)
		{
			EditCut();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* lvCutList_SelectedIndexChanged																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selected index has changed on the cut list.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void lvCutList_SelectedIndexChanged(object sender, EventArgs e)
		{
			//btnGO.Enabled = (lvCutList.Items.Count != 0);
			if(lvCutList.SelectedItems.Count > 0)
			{
				btnEditCut.Enabled =
					btnMoveCutDown.Enabled =
					btnMoveCutUp.Enabled =
					(lvCutList.SelectedItems.Count == 1);
				btnDeleteCut.Enabled = btnDuplicateCut.Enabled = true;
			}
			else
			{
				btnEditCut.Enabled =
					btnDeleteCut.Enabled =
					btnDuplicateCut.Enabled =
					btnMoveCutDown.Enabled =
					btnMoveCutUp.Enabled = false;
			}
			this.Refresh();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* lvPatterns_DoubleClick																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The patterns list has received a double-click.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void lvPatterns_DoubleClick(object sender, EventArgs e)
		{
			ListViewItem selectedItem = null;

			if(lvPatterns.SelectedItems.Count > 0)
			{
				selectedItem = lvPatterns.SelectedItems[0];
				if(selectedItem.Tag != null && selectedItem.Tag is
					PatternTemplateItem @patternTemplate)
				{
					CreateCut(patternTemplate);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* lvPatterns_ItemDrag																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Drag the selected pattern item.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Item drag event arguments.
		/// </param>
		private void lvPatterns_ItemDrag(object sender, ItemDragEventArgs e)
		{
			List<ListViewItem> items = new List<ListViewItem>();

			items.Add((ListViewItem)e.Item);
			lvPatterns.DoDragDrop(items, DragDropEffects.Copy);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditSettings_Click																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Settings menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditSettings_Click(object sender, EventArgs e)
		{
			frmSettings dialog = new frmSettings();

			if(dialog.ShowDialog() == DialogResult.OK)
			{
				//	Save the settings.
				WriteConfiguration();
				UpdateWorkpiece();
				RefreshControls();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuEditTemplates_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit / Templates menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuEditTemplates_Click(object sender, EventArgs e)
		{
			frmEditTemplates dialog = new frmEditTemplates();

			dialog.Patterns = ConfigProfile.PatternTemplates;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				PatternTemplateCollection.TransferValues(
					dialog.Patterns, ConfigProfile.PatternTemplates);
				WriteConfiguration();
				InitializePatterns();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileExit_Click																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Exit menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileExportConfiguration_Click																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Export / Configuration menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileExportConfiguration_Click(object sender, EventArgs e)
		{
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileExportGCode_Click																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Export / G-Code menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileExportGCode_Click(object sender, EventArgs e)
		{
			SaveFileDialog dialog = null;

			if(SessionWorkpieceInfo != null)
			{
				dialog = new SaveFileDialog();
				dialog.AddExtension = true;
				dialog.AutoUpgradeEnabled = true;
				dialog.CheckFileExists = false;
				dialog.CheckPathExists = true;
				dialog.CreatePrompt = false;
				dialog.DefaultExt = ".gcode";
				dialog.DereferenceLinks = true;
				dialog.Filter =
					"GCode Files " +
					"(*.gcode)|" +
					"*.gcode;|" +
					"(*.nc)|" +
					"*.nc;|" +
					"Text Files " +
					"(*.txt)|" +
					"*.txt;|" +
					"All Files (*.*)|*.*";
				dialog.FilterIndex = 0;
				dialog.OverwritePrompt = true;
				dialog.SupportMultiDottedExtensions = true;
				dialog.Title = "Save G-code File";
				dialog.ValidateNames = true;
				if(dialog.ShowDialog() == DialogResult.OK)
				{
					ExportGCode(dialog.FileName);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileImportPatterns_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Import / Patterns menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileImportPatterns_Click(object sender, EventArgs e)
		{
			string content = "";
			int count = 0;
			OpenFileDialog dialog = new OpenFileDialog();
			List<PatternTemplateItem> templates = null;

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = true;
			dialog.DefaultExt = ".patterns.json";
			dialog.DereferenceLinks = true;
			dialog.Filter =
				"ShopTools Pattern Files " +
				"(*.patterns.json)|" +
				"*.patterns.json;|" +
				"Text Files " +
				"(*.txt)|" +
				"*.txt;|" +
				"All Files (*.*)|*.*";
			dialog.FilterIndex = 0;
			dialog.Multiselect = true;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = "Open Pattern Files";
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				if(dialog.FileNames.Length > 0)
				{
					//	Filenames were specified.

					foreach(string filenameItem in dialog.FileNames)
					{
						content = File.ReadAllText(filenameItem);
						try
						{
							templates =
								JsonConvert.
									DeserializeObject<List<PatternTemplateItem>>(content);
							foreach(PatternTemplateItem templateItem in templates)
							{
								if(!ConfigProfile.PatternTemplates.Exists(x =>
									x.PatternTemplateId.ToLower() ==
										templateItem.PatternTemplateId.ToLower()))
								{
									//	A unique template was found.
									ConfigProfile.PatternTemplates.Add(templateItem);
									count++;
								}
							}
						}
						catch(Exception ex)
						{
							frmMessage.Show(this,
								$"Error opening pattern file: {ex.Message}",
								$"Open Pattern Files - {Path.GetFileName(filenameItem)}",
								MessageBoxButtons.OK);
						}
					}
					if(count > 0)
					{
						WriteConfiguration();
						InitializePatterns();
					}
					statMessage.Text =
						$"{count} patterns were added to the toolbar...";
				}

				this.Refresh();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileImportSvgDrawing_Click																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Import / SVG Drawing menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileImportSvgDrawing_Click(object sender, EventArgs e)
		{
			string content = "";
			FVector2 currentPoint = null;
			OpenFileDialog dialog = new OpenFileDialog();
			Html.HtmlDocument doc = null;
			PatternOperationItem operation = null;
			//PlotPointItem plotPoint = null;
			FVector2 point = SessionWorkpieceInfo.RouterLocation;
			PlotPointCollection points = null;
			CutProfileItem profile = null;
			SvgImageItem svg = null;
			PatternTemplateItem templateLine = null;
			PatternTemplateItem templateMove = null;
			//DateTime timeEnd = DateTime.Now;
			//TimeSpan timer = TimeSpan.Zero;
			//DateTime timeStart = DateTime.Now;

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = true;
			dialog.DefaultExt = ".svg";
			dialog.DereferenceLinks = true;
			dialog.Filter =
				"SVG Files " +
				"(*.svg)|" +
				"*.svg;|" +
				"HTML Files " +
				"(*.html)|" +
				"*.html;|" +
				"Text Files " +
				"(*.txt)|" +
				"*.txt;|" +
				"All Files (*.*)|*.*";
			dialog.FilterIndex = 0;
			dialog.Multiselect = false;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = "Open SVG File";
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				this.Cursor = Cursors.WaitCursor;

				content = File.ReadAllText(dialog.FileName);

				//timeStart = DateTime.Now;
				//timeEnd = DateTime.Now;
				//timer += timeEnd.Subtract(timeStart);
				//timeStart = timeEnd;
				//Trace.WriteLine($"Read HTML Start: {timer:ss\\.fffffff}");
				doc = new Html.HtmlDocument(content);

				//timeEnd = DateTime.Now;
				//timer += timeEnd.Subtract(timeStart);
				//timeStart = timeEnd;
				//Trace.WriteLine($"Read SVG Start: {timer:ss\\.fffffff}");
				svg = new SvgImageItem(doc, 10);

				//	Remove duplicates.
				RemoveDuplicates(svg.PlotPoints);

				//	Remove the initial non-drawing movements.
				while(svg.PlotPoints.Count > 0 &&
					svg.PlotPoints[0].PenStatus == PlotPointPenStatus.PenUp)
				{
					svg.PlotPoints.RemoveAt(0);
				}

				//	Move the center of the object to the current router position.
				point = PlotPointCollection.GetCenter(svg.PlotPoints);
				point = FVector2.Negate(point);
				point += SessionWorkpieceInfo.RouterLocation;
				foreach(PlotPointItem plotPointItem in svg.PlotPoints)
				{
					plotPointItem.Point += point;
				}

				//	The so-called polarization of the SVG file is bottom+, right+,
				//	whereas the polarization of the table is probably different.
				//	We want the image to appear on the table with the same orientation
				//	that it appears on the drawing editor.
				//	The easiest way to make this conversion is to convert the entire
				//	collection to relative measurements, then to set polarization
				//	flags, if necessary, for any axes that need to be flipped before
				//	display.
				//DumpPoints(svg.PlotPoints);
				points = ConvertToRelative(svg.PlotPoints);
				if(ConfigProfile.TravelX == DirectionLeftRightEnum.Left)
				{
					foreach(PlotPointItem plotPointItem in points)
					{
						plotPointItem.Point.X *= -1f;
					}
				}
				if(ConfigProfile.TravelY == DirectionUpDownEnum.Up)
				{
					foreach(PlotPointItem plotPointItem in points)
					{
						plotPointItem.Point.Y *= -1f;
					}
				}
				//DumpPoints(points);

				//timeEnd = DateTime.Now;
				//timer += timeEnd.Subtract(timeStart);
				//timeStart = timeEnd;
				//Trace.WriteLine($"Import Start: {timer:ss\\.fffffff}");
				templateLine = ConfigProfile.PatternTemplates.FirstOrDefault(x =>
					x.PatternTemplateId == "26342ee6-786a-4c47-bdbf-01f9afeb290f");
				templateMove = ConfigProfile.PatternTemplates.FirstOrDefault(x =>
					x.PatternTemplateId == "ed4b0b5e-3b3e-4a28-97a9-b34b7f026f09");
				if(templateLine != null && templateMove != null)
				{
					mWorkpieceBusy = true;
					point = SessionWorkpieceInfo.RouterLocation;
					foreach(PlotPointItem plotPointItem in points)
					{
						//timeEnd = DateTime.Now;
						//timer += timeEnd.Subtract(timeStart);
						//timeStart = timeEnd;
						//Trace.WriteLine($"Item Start: {timer:ss\\.fffffff}");
						currentPoint = plotPointItem.Point;
						if(plotPointItem.PenStatus == PlotPointPenStatus.PenDown)
						{
							profile = new CutProfileItem(templateLine);
							profile.StartLocation = point;
							profile.EndLocation = currentPoint;
							operation = profile.Operations[0];
							operation.EndOffsetX = $"{currentPoint.X:0.###}mm";
							operation.EndOffsetY = $"{currentPoint.Y:0.###}mm";
							operation.EndOffsetXOrigin = OffsetLeftRightEnum.Relative;
							operation.EndOffsetYOrigin = OffsetTopBottomEnum.Relative;
						}
						else
						{
							profile = new CutProfileItem(templateMove);
							profile.StartLocation = point;
							profile.EndLocation = currentPoint;
							operation = profile.Operations[0];
							operation.OffsetX = $"{currentPoint.X:0.###}mm";
							operation.OffsetY = $"{currentPoint.Y:0.###}mm";
							operation.OffsetXOrigin = OffsetLeftRightEnum.Relative;
							operation.OffsetYOrigin = OffsetTopBottomEnum.Relative;
						}
						SessionWorkpieceInfo.Cuts.Add(profile);
						FVector2.TransferValues(currentPoint, point);
					}
					////	Join the last point to the first.
					//if(points.Count > 0)
					//{
					//	plotPoint = points[0];
					//	currentPoint = plotPoint.Point;
					//	if(plotPoint.PenStatus == PlotPointPenStatus.PenDown)
					//	{
					//		profile = new CutProfileItem(templateLine);
					//		profile.StartLocation = point;
					//		profile.EndLocation = currentPoint;
					//		operation = profile.Operations[0];
					//		operation.EndOffsetX = $"{currentPoint.X:0.###}mm";
					//		operation.EndOffsetY = $"{currentPoint.Y:0.###}mm";
					//		operation.EndOffsetXOrigin = OffsetLeftRightEnum.Relative;
					//		operation.EndOffsetYOrigin = OffsetTopBottomEnum.Relative;
					//	}
					//	else
					//	{
					//		profile = new CutProfileItem(templateMove);
					//		profile.StartLocation = point;
					//		profile.EndLocation = currentPoint;
					//		operation = profile.Operations[0];
					//		operation.OffsetX = $"{currentPoint.X:0.###}mm";
					//		operation.OffsetY = $"{currentPoint.Y:0.###}mm";
					//		operation.OffsetXOrigin = OffsetLeftRightEnum.Relative;
					//		operation.OffsetYOrigin = OffsetTopBottomEnum.Relative;
					//	}
					//	SessionWorkpieceInfo.Cuts.Add(profile);
					//	FVector2.TransferValues(currentPoint, point);
					//}
					//timeEnd = DateTime.Now;
					//timer += timeEnd.Subtract(timeStart);
					//timeStart = timeEnd;
					//Trace.WriteLine($"Items completed: {timer:ss\\.fffffff}");
					mWorkpieceBusy = false;
					UpdateCutList();
					//timeEnd = DateTime.Now;
					//timer += timeEnd.Subtract(timeStart);
					//timeStart = timeEnd;
					//Trace.WriteLine($"Cut-list updated: {timer:ss\\.fffffff}");
					mCutListChanged = true;
					UpdateForm();
					//timeEnd = DateTime.Now;
					//timer += timeEnd.Subtract(timeStart);
					//timeStart = timeEnd;
					//Trace.WriteLine($"Form updated: {timer:ss\\.fffffff}");
					UpdateWorkpieceUI();
					statMessage.Text = "File opened...";
					UpdateForm();
				}
				this.Refresh();
				this.Cursor = Cursors.Default;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileExportPatterns_Click																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Export / Patterns menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileExportPatterns_Click(object sender, EventArgs e)
		{
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileImportConfiguration_Click																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Import / Configuration menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileImportConfiguration_Click(object sender, EventArgs e)
		{
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileNewCutList_Click																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / New Cut-List menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileNewCutList_Click(object sender, EventArgs e)
		{
			if(PromptSave() != DialogResult.Cancel)
			{
				mCutListFilename = "";
				SessionWorkpieceInfo = new WorkpieceInfoItem();
				SessionWorkpieceInfo.PropertyChanged +=
					sessionWorkpieceInfo_PropertyChanged;
				WorkpieceInfoItem.ConfigureFromUserValues(SessionWorkpieceInfo);
				UpdateWorkpieceUI();
				mCutListChanged = false;
				statMessage.Text = "File opened...";
				UpdateForm();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileOpen_Click																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Open menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileOpen_Click(object sender, EventArgs e)
		{
			string content = "";
			OpenFileDialog dialog = new OpenFileDialog();

			dialog.AddExtension = true;
			dialog.AutoUpgradeEnabled = true;
			dialog.CheckFileExists = true;
			dialog.DefaultExt = ".cutlist.json";
			dialog.DereferenceLinks = true;
			dialog.Filter =
				"ShopTools CutList Files " +
				"(*.cutlist.json)|" +
				"*.cutlist.json;|" +
				"Text Files " +
				"(*.txt)|" +
				"*.txt;|" +
				"All Files (*.*)|*.*";
			dialog.FilterIndex = 0;
			dialog.Multiselect = false;
			dialog.SupportMultiDottedExtensions = true;
			dialog.Title = "Open Cut-List File";
			dialog.ValidateNames = true;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				mCutListFilename = dialog.FileName;
				content = File.ReadAllText(dialog.FileName);
				try
				{
					SessionWorkpieceInfo =
						JsonConvert.DeserializeObject<WorkpieceInfoItem>(content);
				}
				catch
				{
					SessionWorkpieceInfo = new WorkpieceInfoItem();
				}
				SessionWorkpieceInfo.PropertyChanged +=
					sessionWorkpieceInfo_PropertyChanged;
				WorkpieceInfoItem.ConfigureFromUserValues(SessionWorkpieceInfo);
				UpdateWorkpieceUI();
				mCutListChanged = false;
				statMessage.Text = "File opened...";
				UpdateForm();

				this.Refresh();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileSave_Click																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Save menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileSave_Click(object sender, EventArgs e)
		{
			if(mCutListFilename?.Length > 0)
			{
				SaveCutList(mCutListFilename);
			}
			else
			{
				SaveCutListAs();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFileSaveAs_Click																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The File / Save As menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFileSaveAs_Click(object sender, EventArgs e)
		{
			SaveCutListAs();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuHelpAbout_Click																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Help / About menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuHelpAbout_Click(object sender, EventArgs e)
		{
			frmAbout dialog = new frmAbout(this);

			dialog.ShowDialog();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuView3D_Click																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The View / 3D Preview menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuView3D_Click(object sender, EventArgs e)
		{
			frmView3D dialog = new frmView3D();

			dialog.ShowDialog();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuViewGCode_Click																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The View / G-code menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuViewGCode_Click(object sender, EventArgs e)
		{
			string content = String.Join("\r\n", GCode.RenderGCode());
			frmViewText dialog = new frmViewText();

			dialog.Text = content;
			dialog.Title = "Preview G-Code";

			dialog.ShowDialog();

		}
		//*-----------------------------------------------------------------------*


		////*-----------------------------------------------------------------------*
		////* mPaintTimer_Tick																											*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// The duration has elapsed on the paint timer.
		///// </summary>
		///// <param name="sender">
		///// The object raising this event.
		///// </param>
		///// <param name="e">
		///// Standard event arguments.
		///// </param>
		//private void mPaintTimer_Tick(object sender, EventArgs e)
		//{
		//	mPaintEnabled = true;
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mToggleTimer_Tick																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The toggle timer has elapsed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mToggleTimer_Tick(object sender, EventArgs e)
		{
			bool bChange = (mPanelControlToggle || mPanelWorkpieceToggle);

			if(bChange)
			{
				this.SuspendLayout();
				if(mPanelControlToggle)
				{
					if(pnlControl.Width != mPanelControlWidth)
					{
						pnlControl.Width = mPanelControlWidth;
					}
					else
					{
						pnlControl.Width = 24;
					}
					mPanelControlToggle = false;
				}
				if(mPanelWorkpieceToggle)
				{
					if(pnlWorkpiece.Width != mPanelWorkpieceWidth)
					{
						pnlWorkpiece.Width = mPanelWorkpieceWidth;
					}
					else
					{
						pnlWorkpiece.Width = 24;
					}
					mPanelWorkpieceToggle = false;
				}
				this.ResumeLayout();
			}
			mToggleTimer.Stop();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlWorkspace_DragDrop																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The dragged item has been dropped over the workspace panel.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Drag event arguments.
		/// </param>
		private void pnlWorkspace_DragDrop(object sender, DragEventArgs e)
		{
			List<ListViewItem> items = null;

			if(e.Data.GetDataPresent(typeof(List<ListViewItem>)))
			{
				items = (List<ListViewItem>)e.Data.GetData(typeof(List<ListViewItem>));
				if(items.Count > 0)
				{
					//	Items were present.
					if(items[0].Tag != null &&
						items[0].Tag is PatternTemplateItem @patternItem)
					{
						//	Create a cut from a template.
						CreateCut((PatternTemplateItem)items[0].Tag);
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlWorkspace_DragEnter																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A drag operation has entered the workspace panel.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Drag event arguments.
		/// </param>
		private void pnlWorkspace_DragEnter(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(typeof(List<ListViewItem>)))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlWorkspace_Paint																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Workspace panel is being painted.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Paint event arguments.
		/// </param>
		/// <remarks>
		/// The start and end locations of chained operations are recalculated
		/// while drawing.
		/// </remarks>
		private void pnlWorkspace_Paint(object sender, PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			FVector2 location = null;
			CutProfileItem profile = null;
			float scale = 1f;
			SizeF systemSize = GetSystemSize();
			Rectangle workspaceArea = Rectangle.Empty;
			Size workspaceSize = Size.Empty;
			float workspaceRatio = GetWorkspaceRatio();

			//if(mCutListFilename.Length > 0)
			//{
			//	//	If a file is open, check the layout.
			//	Trace.WriteLine("pnlWorkspace_Paint - Break here...");
			//}
			workspaceSize = ResizeArea(
				pnlWorkspace.Width - 16, pnlWorkspace.Height - 16, workspaceRatio);
			scale = (float)workspaceSize.Width / systemSize.Width;
			workspaceArea = new Rectangle(
				CenteredLeft(pnlWorkspace.Width, workspaceSize.Width),
				CenteredTop(pnlWorkspace.Height, workspaceSize.Height),
				workspaceSize.Width, workspaceSize.Height);

			//	Draw the main table.
			DrawTable(pnlWorkspace, SessionWorkpieceInfo, graphics);
			//	Draw the starting router position.
			location = FVector2.Clone(SessionWorkpieceInfo.RouterLocation);
			location = TransformFromAbsolute(location);
			DrawRouter(location, StartEndEnum.Start, graphics, workspaceArea, scale);

			if(lvCutList.Items.Count > 0)
			{
				//	Draw all operations from the cut list.
				//Trace.WriteLine("pnlWorkspace_Paint");
				foreach(ListViewItem lvItem in lvCutList.Items)
				{
					profile = (CutProfileItem)lvItem.Tag;
					//Trace.WriteLine($" Drawing {profile.TemplateName}");
					profile.StartLocation = TransformToAbsolute(location);
					foreach(PatternOperationItem operationItem in profile.Operations)
					{
						location = DrawOperation(operationItem,
							SessionWorkpieceInfo, location, "", graphics,
							workspaceArea, scale, lvItem.Selected);
					}
					//if(lvItem.Selected)
					//{
					//	foreach(PatternOperationItem operationItem in profile.Operations)
					//	{
					//		location = DrawOperation(operationItem,
					//			SessionWorkpieceInfo, location, "", graphics,
					//			workspaceArea, scale, true);
					//	}
					//}
					//else
					//{
					//	foreach(PatternOperationItem operationItem in profile.Operations)
					//	{
					//		location = DrawOperation(operationItem,
					//			SessionWorkpieceInfo, location, "", graphics,
					//			workspaceArea, scale, false);
					//	}
					//}
					profile.EndLocation = TransformToAbsolute(location);
				}
				//foreach(CutProfileItem profileItem in SessionWorkpieceInfo.Cuts)
				//{
				//	foreach(PatternOperationItem operationItem in profileItem.Operations)
				//	{
				//		location = DrawOperation(operationItem,
				//			SessionWorkpieceInfo, location, "", graphics,
				//			workspaceArea, scale);
				//	}
				//}
				DrawRouter(location, StartEndEnum.End, graphics, workspaceArea, scale);
				//if(!btnGO.Enabled)
				//{
				//	btnGO.Enabled = true;
				//}
			}
			else
			{
				if(btnDeleteCut.Enabled)
				{
					btnDeleteCut.Enabled = false;
				}
				if(btnDuplicateCut.Enabled)
				{
					btnDuplicateCut.Enabled = false;
				}
				if(btnEditCut.Enabled)
				{
					btnEditCut.Enabled = false;
				}
				if(btnMoveCutDown.Enabled)
				{
					btnMoveCutDown.Enabled = false;
				}
				if(btnMoveCutUp.Enabled)
				{
					btnMoveCutUp.Enabled = false;
				}
				//if(btnGO.Enabled)
				//{
				//	btnGO.Enabled = false;
				//}
				//if(btnStop.Enabled)
				//{
				//	btnStop.Enabled = false;
				//}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlWorkspace_Resize																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The workspace panel has been resized.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void pnlWorkspace_Resize(object sender, EventArgs e)
		{
			pnlWorkspace.Refresh();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PromptSave																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// If a file is open, prompt the user as to whether it should be saved.
		/// </summary>
		/// <returns>
		/// Continue if the file was either unchanged or successfully saved.
		/// Otherwise, Cancel.
		/// </returns>
		private DialogResult PromptSave()
		{
			DialogResult result = DialogResult.Continue;

			if(mCutListChanged)
			{
				result = frmMessage.Show(this,
					"Do you wish to save your changes?", "Exit",
					MessageBoxButtons.YesNoCancel);
				switch(result)
				{
					case DialogResult.Yes:
						if(mCutListFilename?.Length > 0)
						{
							SaveCutList(mCutListFilename);
						}
						else
						{
							SaveCutListAs();
						}
						result = DialogResult.Continue;
						break;
					case DialogResult.Cancel:
						result = DialogResult.Cancel;
						break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RefreshControls																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the visible controls from the current configuration.
		/// </summary>
		private void RefreshControls()
		{
			statWorkspace.Text =
				$"{ConfigProfile.XDimension} x {ConfigProfile.YDimension}";
			if(IsXFeed())
			{
				lblWorkpieceWidth.Text = "Width:";
				lblWorkpieceLength.Text = "Length:";
			}
			else
			{
				lblWorkpieceWidth.Text = "Length:";
				lblWorkpieceLength.Text = "Width:";
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SaveCutList																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Save the contents of the workpiece and operations to the specified
		/// file.
		/// </summary>
		/// <param name="filename">
		/// Path and filename of the file to store.
		/// </param>
		private void SaveCutList(string filename)
		{
			string content = "";

			if(filename?.Length > 0)
			{
				content = JsonConvert.SerializeObject(SessionWorkpieceInfo,
					Formatting.Indented);
				File.WriteAllText(filename, content);
				statMessage.Text = "File saved...";
			}
			else
			{
				statMessage.Text = "Ready...";
			}
			mCutListChanged = false;
			UpdateForm();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SaveCutListAs																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Give the cut-list a filename and save that file.
		/// </summary>
		private void SaveCutListAs()
		{
			SaveFileDialog dialog = null;

			if(SessionWorkpieceInfo != null)
			{
				dialog = new SaveFileDialog();
				dialog.AddExtension = true;
				dialog.AutoUpgradeEnabled = true;
				dialog.CheckFileExists = false;
				dialog.CheckPathExists = true;
				dialog.CreatePrompt = false;
				dialog.DefaultExt = ".cutlist.json";
				dialog.DereferenceLinks = true;
				dialog.Filter =
					"ShopTools CutList Files " +
					"(*.cutlist.json)|" +
					"*.cutlist.json;|" +
					"Text Files " +
					"(*.txt)|" +
					"*.txt;|" +
					"All Files (*.*)|*.*";
				dialog.FilterIndex = 0;
				dialog.OverwritePrompt = true;
				dialog.SupportMultiDottedExtensions = true;
				dialog.Title = "Save Cut-List File";
				dialog.ValidateNames = true;
				if(dialog.ShowDialog() == DialogResult.OK)
				{
					mCutListFilename = dialog.FileName;
					SaveCutList(mCutListFilename);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* sessionWorkpieceInfo_PropertyChanged																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The value of property has changed on the current session workpiece
		/// information block or its members.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Property change event arguments.
		/// </param>
		private void sessionWorkpieceInfo_PropertyChanged(object sender,
			PropertyChangeEventArgs e)
		{
			mCutListChanged = true;
			statMessage.Text = "File Changed...";
			UpdateForm();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* splitControl_DoubleClick																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The control panel splitter has been double-clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void splitControl_DoubleClick(object sender, EventArgs e)
		{
			//this.SuspendLayout();
			//if(pnlControl.Width > 32)
			//{
			//	pnlControl.Width = 16;
			//}
			//else
			//{
			//	pnlControl.Width = mPanelControlWidth;
			//}
			//this.ResumeLayout();
			//this.PerformLayout();
			mPanelControlToggle = true;
			mToggleTimer.Start();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* splitWorkpiece_DoubleClick																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The workpiece information splitter has been double-clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void splitWorkpiece_DoubleClick(object sender, EventArgs e)
		{
			//if(pnlWorkpiece.Width > 32)
			//{
			//	pnlWorkpiece.Width = 16;
			//}
			//else
			//{
			//	pnlWorkpiece.Width = mPanelWorkpieceWidth;
			//}
			mPanelWorkpieceToggle = true;
			mToggleTimer.Start();
		}
		//*-----------------------------------------------------------------------*

#if InternalTest
		//*-----------------------------------------------------------------------*
		//* TestAbsoluteTransformation																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Test the transformation methods to and from an absolute value, with
		/// regard to the display drawing space.
		/// </summary>
		private static void TestAbsoluteTransformation()
		{
			ShopToolsUtil.ConfigProfile = new ShopToolsConfigItem()
			{
				AxisXIsOpenEnded = true,
				AxisYIsOpenEnded = false,
				Depth = "30mm",
				DisplayUnits = DisplayUnitEnum.UnitedStates,
				GeneralCuttingTool = "1/8in",
				TravelX = DirectionLeftRightEnum.Left,
				TravelY = DirectionUpDownEnum.Up,
				TravelZ = DirectionUpDownEnum.Up,
				XDimension = "2438.4mm",
				YDimension = "1219.2mm",
				XYOrigin = OriginLocationEnum.BottomRight
			};
			FVector2 absPoint = new FVector2(0f, 0f);
			FVector2 displayPoint = TransformFromAbsolute(absPoint);
			Trace.WriteLine(
				$"Transfer from {ConfigProfile.XYOrigin} origin - " +
				$"{absPoint} -> {displayPoint}");
			absPoint.X = 100f;
			absPoint.Y = 100f;
			displayPoint = TransformFromAbsolute(absPoint);
			Trace.WriteLine(
				$"Transfer from {ConfigProfile.XYOrigin} origin - " +
				$"{absPoint} -> {displayPoint}");
			absPoint.X = -100f;
			absPoint.Y = -100f;
			displayPoint = TransformFromAbsolute(absPoint);
			Trace.WriteLine(
				$"Transfer from {ConfigProfile.XYOrigin} origin - " +
				$"{absPoint} -> {displayPoint}");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TestMeasurementUnits																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Test the current method for handling measurement conversion.
		/// </summary>
		private static void TestMeasurementUnits()
		{
			string expression = "";
			float number = 0f;

			expression = "((1in + 12mm + 1 5/16) / 52) * 0.01mm";
			Trace.WriteLine($"Solve: {expression}");
			number = MeasurementProcessor.SumInches(expression, "in");
			Trace.WriteLine($" Answer: {number:0.###}in");
			expression = "14in - 12mm + 1 5/16\" * 0.01mm";
			Trace.WriteLine($"Solve: {expression}");
			number = MeasurementProcessor.SumMillimeters(
				expression, "mm");
			Trace.WriteLine($" Answer: {number:0.###}mm");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TestTransferValues																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Test the deep transfer of values using the reflection-based utility.
		/// </summary>
		private static void TestTransferValues()
		{
			string content = "";
			CutProfileItem profile2 = new CutProfileItem()
			{
				EndLocation = new FVector2(10f, 20f),
				StartLocation = new FVector2(50f, 60f)
			};
			CutProfileItem profile1 = new CutProfileItem()
			{
				DisplayFormat = "a display format",
				EndLocation = new FVector2(10f, 20f),
				IconFilename = "long/icon/filename.img",
				Orientation = TemplateOrientationEnum.Workpiece,
				PatternLength = "30in",
				PatternWidth = "40mm",
				StartLocation = new FVector2(50f, 60f),
				TemplateName = "random name",
				ToolSequenceStrict = false
			};
			profile1.Operations.Add(new PatternOperationItem()
			{
				Action = OperationActionEnum.DrawCircleCenterDiameter,
				Angle = "70deg",
				Depth = "80nm",
				EndOffsetX = "90mm",
				EndOffsetXOrigin = OffsetLeftRightEnum.Left,
				EndOffsetY = "100mm",
				EndOffsetYOrigin = OffsetTopBottomEnum.Top,
				Kerf = DirectionLeftRightEnum.Right,
				Length = "110mm",
				OffsetX = "120mm",
				OffsetXOrigin = OffsetLeftRightEnum.Right,
				OffsetY = "130mm",
				OffsetYOrigin = OffsetTopBottomEnum.Relative,
				OperationName = "random operation",
				StartOffsetX = "140mm",
				StartOffsetXOrigin = OffsetLeftRightEnum.Right,
				StartOffsetY = "150mm",
				StartOffsetYOrigin = OffsetTopBottomEnum.Bottom,
				Tool = "a random tool name",
				Width = "160mm"
			});
			profile1.Remarks.Add("This is a comment for the profile.");
			profile1.SharedVariables.AddRange(
				new string[] { "Variable1", "Variable2" });
			content = JsonConvert.SerializeObject(profile1, Formatting.Indented);
			Trace.WriteLine($"TestTransferValues. Original: {content}");
			DeepTransfer(profile1, profile2);
			content = JsonConvert.SerializeObject(profile2, Formatting.Indented);
			Trace.WriteLine($"TestTransferValues. Transfer: {content}");
			profile2 = DeepClone(profile1);
			content = JsonConvert.SerializeObject(profile2, Formatting.Indented);
			Trace.WriteLine($"TestTransferValues. Clone:    {content}");
			//	When using this form of cloning, make sure to recalculate any
			//	members on the target that are ignored during JSON serialization.
			//	In the above case, EndLocation and StartLocation are not serialized,
			//	and as a result, are blank after cloning.
		}
		//*-----------------------------------------------------------------------*
#endif

		//*-----------------------------------------------------------------------*
		//* txtRouterPositionX_TextChanged																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text in the Router Position X textbox has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtRouterPositionX_TextChanged(object sender, EventArgs e)
		{
			UpdateWorkpiece();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtRouterPositionY_TextChanged																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text in the Router Position Y textbox has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtRouterPositionY_TextChanged(object sender, EventArgs e)
		{
			UpdateWorkpiece();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtWorkpieceDepth_TextChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text has changed in the Workpiece Depth textbox.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtWorkpieceDepth_TextChanged(object sender, EventArgs e)
		{
			UpdateWorkpiece();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtWorkpieceX_TextChanged																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text has changed in the Workpiece Left textbox.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtWorkpieceX_TextChanged(object sender, EventArgs e)
		{
			UpdateWorkpiece();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtWorkpieceLength_TextChanged																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text has changed in the Workpiece Length textbox.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtWorkpieceLength_TextChanged(object sender, EventArgs e)
		{
			UpdateWorkpiece();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtWorkpieceY_TextChanged																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text has changed in the Workpiece Top textbox.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtWorkpieceY_TextChanged(object sender, EventArgs e)
		{
			UpdateWorkpiece();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtWorkpieceWidth_TextChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text has changed in the Workpiece Width textbox.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtWorkpieceWidth_TextChanged(object sender, EventArgs e)
		{
			UpdateWorkpiece();
		}
		//*-----------------------------------------------------------------------*

		//	TODO: Improve the speed of frmMain.UpdateCutList.
		//*-----------------------------------------------------------------------*
		//* UpdateCutList																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the contents of the cut list.
		/// </summary>
		private void UpdateCutList()
		{
			string imageName = "";
			Image image = null;
			List<string> loadedIconFilenames = new List<string>();
			ListViewItem lvItem = null;

			btnDeleteCut.Enabled =
				btnDuplicateCut.Enabled =
				btnEditCut.Enabled =
				btnMoveCutDown.Enabled =
				btnMoveCutUp.Enabled = false;
			lvCutList.Items.Clear();
			foreach(CutProfileItem cutItem in SessionWorkpieceInfo.Cuts)
			{
				lvItem = null;
				if(cutItem.IconFilename.Length > 0)
				{
					imageName = Path.GetFileName(cutItem.IconFilename);
					image = ilPatterns.Images[imageName];
					if(image == null)
					{
						imageName = "";
					}
				}
				if(imageName.Length == 0 && ilPatterns.Images.Count > 0)
				{
					//	Image has not yet been found.
					imageName = ilPatterns.Images.Keys[0];
				}
				if(imageName.Length > 0)
				{
					//lvItem = new ListViewItem(cutItem.TemplateName, imageName);
					lvItem = new ListViewItem(
						PatternTemplateItem.GetDisplayString(cutItem), imageName);
					lvItem.Tag = cutItem;
				}
				if(lvItem != null)
				{
					lvCutList.Items.Add(lvItem);
				}
			}
			lvCutList.Columns[0].Width = -1;
			//btnGO.Enabled = (lvCutList.Items.Count > 0);
			this.Refresh();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdateForm																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the general parts of the form affected by status.
		/// </summary>
		private void UpdateForm()
		{
			StringBuilder builder = new StringBuilder();

			//	Title bar.
			builder.Append(mBaseFormText);
			if(mCutListFilename?.Length > 0)
			{
				builder.Append(" - ");
				builder.Append(Path.GetFileName(mCutListFilename));
			}
			if(mCutListChanged)
			{
				builder.Append(" *");
			}
			this.Text = builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdateWorkpiece																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the dimensions and positioning of the workpiece.
		/// </summary>
		private void UpdateWorkpiece()
		{
			OffsetLeftRightEnum baseHorz = OffsetLeftRightEnum.None;
			OffsetTopBottomEnum baseVert = OffsetTopBottomEnum.None;

			if(!mWorkpieceBusy)
			{
				mWorkpieceBusy = true;
				//	Material Type.
				if(cmboMaterialType.SelectedIndex > -1)
				{
					SessionWorkpieceInfo.MaterialTypeName =
						(string)cmboMaterialType.SelectedItem;
				}
				else
				{
					SessionWorkpieceInfo.MaterialTypeName = "";
				}
				//	Length.
				SessionWorkpieceInfo.UserLength = txtWorkpieceLength.Text;
				//	Width.
				SessionWorkpieceInfo.UserWidth = txtWorkpieceWidth.Text;
				//	Depth.
				SessionWorkpieceInfo.UserDepth =
					(txtWorkpieceDepth.Text.Length > 0 ?
						txtWorkpieceDepth.Text : "3.175mm");
				//	X.
				SessionWorkpieceInfo.UserOffsetX = txtWorkpieceX.Text;
				if(cmboWorkpieceX.SelectedIndex > -1)
				{
					switch(cmboWorkpieceX.SelectedItem.ToString().ToLower())
					{
						case "center to center":
							baseHorz = OffsetLeftRightEnum.Center;
							break;
						case "left edge to center":
							baseHorz = OffsetLeftRightEnum.LeftEdgeToCenter;
							break;
						case "right edge to center":
							baseHorz = OffsetLeftRightEnum.RightEdgeToCenter;
							break;
						case "from left":
							baseHorz = OffsetLeftRightEnum.Left;
							break;
						case "from right":
							baseHorz = OffsetLeftRightEnum.Right;
							break;
					}
				}
				SessionWorkpieceInfo.UserOffsetXOrigin = baseHorz;
				//	Y.
				SessionWorkpieceInfo.UserOffsetY = txtWorkpieceY.Text;
				if(cmboWorkpieceY.SelectedIndex > -1)
				{
					switch(cmboWorkpieceY.SelectedItem.ToString().ToLower())
					{
						case "bottom edge to center":
							baseVert = OffsetTopBottomEnum.BottomEdgeToCenter;
							break;
						case "center to center":
							baseVert = OffsetTopBottomEnum.Center;
							break;
						case "top edge to center":
							baseVert = OffsetTopBottomEnum.TopEdgeToCenter;
							break;
						case "from top":
							baseVert = OffsetTopBottomEnum.Top;
							break;
						case "from bottom":
							baseVert = OffsetTopBottomEnum.Bottom;
							break;
					}
				}
				SessionWorkpieceInfo.UserOffsetYOrigin = baseVert;
				//	Router location X.
				SessionWorkpieceInfo.UserRouterLocationX = txtRouterPositionX.Text;
				//	Router location Y.
				SessionWorkpieceInfo.UserRouterLocationY = txtRouterPositionY.Text;
				//	Update the binary working values.
				WorkpieceInfoItem.ConfigureFromUserValues(SessionWorkpieceInfo);

				//	Length.
				lblWorkpieceLengthUnit.Text = SessionWorkpieceInfo.AltLength;
				//	Width.
				lblWorkpieceWidthUnit.Text = SessionWorkpieceInfo.AltWidth;
				//	Depth.
				lblWorkpieceDepthUnit.Text = SessionWorkpieceInfo.AltDepth;
				//	X.
				lblWorkpieceXUnit.Text = SessionWorkpieceInfo.AltOffsetX;
				//	Y.
				lblWorkpieceYUnit.Text = SessionWorkpieceInfo.AltOffsetY;
				//	Router Location X.
				lblRouterPositionXUnit.Text = SessionWorkpieceInfo.AltRouterLocationX;
				//	Router Location Y.
				lblRouterPositionYUnit.Text = SessionWorkpieceInfo.AltRouterLocationY;

				CalculateLayout();

				pnlWorkspace.Refresh();
				mWorkpieceBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdateWorkpieceUI																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the contents of the user interface from the user values in the
		/// session workpiece.
		/// </summary>
		private void UpdateWorkpieceUI()
		{
			if(!mWorkpieceBusy)
			{
				mWorkpieceBusy = true;
				cmboMaterialType.SelectedItem =
					SessionWorkpieceInfo.MaterialTypeName;
				//	Length.
				txtWorkpieceLength.Text = SessionWorkpieceInfo.UserLength;
				lblWorkpieceLengthUnit.Text = SessionWorkpieceInfo.AltLength;
				//	Width.
				txtWorkpieceWidth.Text = SessionWorkpieceInfo.UserWidth;
				lblWorkpieceWidthUnit.Text = SessionWorkpieceInfo.AltWidth;
				//	Depth.
				txtWorkpieceDepth.Text = SessionWorkpieceInfo.UserDepth;
				lblWorkpieceDepthUnit.Text = SessionWorkpieceInfo.AltDepth;
				//	X.
				txtWorkpieceX.Text = SessionWorkpieceInfo.UserOffsetX;
				lblWorkpieceXUnit.Text = SessionWorkpieceInfo.AltOffsetX;
				switch(SessionWorkpieceInfo.UserOffsetXOrigin)
				{
					case OffsetLeftRightEnum.Center:
						SelectItem(cmboWorkpieceX, "Center To Center");
						break;
					case OffsetLeftRightEnum.Left:
						SelectItem(cmboWorkpieceX, "From Left");
						break;
					case OffsetLeftRightEnum.LeftEdgeToCenter:
						SelectItem(cmboWorkpieceX, "Left Edge To Center");
						break;
					case OffsetLeftRightEnum.None:
						SelectItem(cmboWorkpieceX, "None");
						break;
					case OffsetLeftRightEnum.Right:
						SelectItem(cmboWorkpieceX, "From Right");
						break;
					case OffsetLeftRightEnum.RightEdgeToCenter:
						SelectItem(cmboWorkpieceX, "Right Edge To Center");
						break;
				}
				//	Y.
				txtWorkpieceY.Text = SessionWorkpieceInfo.UserOffsetY;
				lblWorkpieceYUnit.Text = SessionWorkpieceInfo.AltOffsetY;
				switch(SessionWorkpieceInfo.UserOffsetYOrigin)
				{
					case OffsetTopBottomEnum.Bottom:
						SelectItem(cmboWorkpieceY, "From Bottom");
						break;
					case OffsetTopBottomEnum.BottomEdgeToCenter:
						SelectItem(cmboWorkpieceY, "Bottom Edge To Center");
						break;
					case OffsetTopBottomEnum.Center:
						SelectItem(cmboWorkpieceY, "Center To Center");
						break;
					case OffsetTopBottomEnum.None:
						SelectItem(cmboWorkpieceY, "None");
						break;
					case OffsetTopBottomEnum.Top:
						SelectItem(cmboWorkpieceY, "From Top");
						break;
					case OffsetTopBottomEnum.TopEdgeToCenter:
						SelectItem(cmboWorkpieceY, "Top Edge To Center");
						break;
				}
				//	Router location X.
				txtRouterPositionX.Text = SessionWorkpieceInfo.UserRouterLocationX;
				lblRouterPositionXUnit.Text = SessionWorkpieceInfo.AltRouterLocationX;
				//	Router location Y.
				txtRouterPositionY.Text = SessionWorkpieceInfo.UserRouterLocationY;
				lblRouterPositionYUnit.Text = SessionWorkpieceInfo.AltRouterLocationY;

				CalculateLayout();
				UpdateCutList();

				pnlWorkspace.Refresh();
				mWorkpieceBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* OnActivated																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Activated event when the form has been activated.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnActivated(EventArgs e)
		{
			//Debug.WriteLine("Form activated...");
			//mPaintEnabled = true;
			this.Refresh();
			base.OnActivated(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnClosing																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Closing event when the form is closing.
		/// </summary>
		/// <param name="e">
		/// Cancel event arguments.
		/// </param>
		protected override void OnClosing(CancelEventArgs e)
		{
			DialogResult result = DialogResult.Continue;

			result = PromptSave();
			if(result == DialogResult.Cancel)
			{
				e.Cancel = true;
				statMessage.Text = "Exit cancelled...";
			}
			base.OnClosing(e);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnResize																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Resize event when the form has been resized.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnResize(EventArgs e)
		{
			//mPaintEnabled = true;
			base.OnResize(e);
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the frmMain Item.
		/// </summary>
		public frmMain()
		{
			InitializeComponent();

			mBaseFormText = this.Text;

			InitializeApplication();

#if InternalTest
			TestMeasurementUnits();
			TestAbsoluteTransformation();
			TestTransferValues();
#endif

			pnlWorkspace.AllowDrop = true;
			btnDeleteCut.Enabled = false;
			btnEditCut.Enabled = false;
			btnMoveCutDown.Enabled = false;
			btnMoveCutUp.Enabled = false;

			btnGO.Enabled = false;
			btnStop.Enabled = false;

			//	The design-time version of cut list is its defined minimum height.
			mCutListMinHeight = lvCutList.Height;
			//	The Cut list height is going to be adjustable, so content below it
			//	needs to be moveable.
			mCutButtonRowY1Offset = btnGO.Top - lvCutList.Bottom;
			mCutButtonRowY2Offset = btnDeleteCut.Top - lvCutList.Bottom;
			mCutButtonRowY3Offset = btnStop.Top - lblCutList.Bottom;

			mToggleTimer = new Timer()
			{
				Enabled = false,
				Interval = 250
			};
			mToggleTimer.Tick += mToggleTimer_Tick;

			//	Double-buffering on Workspace panel.
			typeof(Panel).InvokeMember("DoubleBuffered",
				BindingFlags.SetProperty | BindingFlags.Instance |
				BindingFlags.NonPublic,
				null, pnlWorkspace, new object[] { true });

			cmboWorkpieceX.SelectedItem = "Center To Center";
			cmboWorkpieceY.SelectedItem = "Center To Center";
			lblRouterPositionXUnit.Text = "";
			lblRouterPositionYUnit.Text = "";
			lblWorkpieceDepthUnit.Text = "";
			lblWorkpieceXUnit.Text = "";
			lblWorkpieceLengthUnit.Text = "";
			lblWorkpieceYUnit.Text = "";
			lblWorkpieceWidthUnit.Text = "";

			this.DoubleBuffered = true;

			cmboWorkpieceX.SelectedIndexChanged +=
				cmboWorkpieceLeft_SelectedIndexChanged;
			cmboWorkpieceY.SelectedIndexChanged +=
				cmboWorkpieceTop_SelectedIndexChanged;
			cmboMaterialType.SelectedIndexChanged +=
				cmboMaterialType_SelectedIndexChanged;

			//	Menu options.
			mnuEditSettings.Click += mnuEditSettings_Click;
			mnuEditTemplates.Click += mnuEditTemplates_Click;
			mnuFileExit.Click += mnuFileExit_Click;
			mnuFileExportConfiguration.Click += mnuFileExportConfiguration_Click;
			mnuFileExportGCode.Click += mnuFileExportGCode_Click;
			mnuFileExportPatterns.Click += mnuFileExportPatterns_Click;
			mnuFileImportConfiguration.Click += mnuFileImportConfiguration_Click;
			mnuFileImportPatterns.Click += mnuFileImportPatterns_Click;
			mnuFileImportSvgDrawing.Click += mnuFileImportSvgDrawing_Click;
			mnuFileNewCutList.Click += mnuFileNewCutList_Click;
			mnuFileOpen.Click += mnuFileOpen_Click;
			mnuFileSave.Click += mnuFileSave_Click;
			mnuFileSaveAs.Click += mnuFileSaveAs_Click;
			mnuHelpAbout.Click += mnuHelpAbout_Click;
			mnuView3D.Click += mnuView3D_Click;
			mnuViewGCode.Click += mnuViewGCode_Click;

			mnuFileImportConfiguration.Enabled = false;
			mnuFileExportConfiguration.Enabled = false;
			mnuFileExportGCode.Enabled = true;
			mnuFileExportPatterns.Enabled = false;

			//	Form events.
			btnDeleteCut.Click += btnDeleteCut_Click;
			btnDuplicateCut.Click += btnDuplicateCut_Click;
			btnEditCut.Click += btnEditCut_Click;
			btnGO.Click += btnGO_Click;
			btnMoveCutDown.Click += btnMoveCutDown_Click;
			btnMoveCutUp.Click += btnMoveCutUp_Click;
			btnStop.Click += btnStop_Click;

			lvCutList.DoubleClick += lvCutList_DoubleClick;
			lvCutList.SelectedIndexChanged += lvCutList_SelectedIndexChanged;
			lvPatterns.DoubleClick += lvPatterns_DoubleClick;
			lvPatterns.ItemDrag += lvPatterns_ItemDrag;

			pnlWorkspace.DragDrop += pnlWorkspace_DragDrop;
			pnlWorkspace.DragEnter += pnlWorkspace_DragEnter;
			pnlWorkspace.Paint += pnlWorkspace_Paint;
			pnlWorkspace.Resize += pnlWorkspace_Resize;

			splitControl.DoubleClick += splitControl_DoubleClick;
			splitWorkpiece.DoubleClick += splitWorkpiece_DoubleClick;

			txtRouterPositionX.TextChanged += txtRouterPositionX_TextChanged;
			txtRouterPositionY.TextChanged += txtRouterPositionY_TextChanged;
			txtWorkpieceDepth.TextChanged += txtWorkpieceDepth_TextChanged;
			txtWorkpieceX.TextChanged += txtWorkpieceX_TextChanged;
			txtWorkpieceLength.TextChanged += txtWorkpieceLength_TextChanged;
			txtWorkpieceY.TextChanged += txtWorkpieceY_TextChanged;
			txtWorkpieceWidth.TextChanged += txtWorkpieceWidth_TextChanged;

			mPanelControlWidth = pnlControl.Width;
			mPanelWorkpieceWidth = pnlWorkpiece.Width;

			//InitializeDefaultProfile();
			ReadConfiguration();
			foreach(MaterialTypeItem typeItem in ConfigProfile.MaterialTypes)
			{
				cmboMaterialType.Items.Add(typeItem.MaterialTypeName);
			}
			InitializePatterns();
			UpdateWorkpiece();
			RefreshControls();

			SessionWorkpieceInfo.PropertyChanged +=
				sessionWorkpieceInfo_PropertyChanged;
			//mPaintTimer.Enabled = true;

		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
