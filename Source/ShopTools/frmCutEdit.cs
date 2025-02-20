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
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using Geometry;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	frmCutEdit																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Cut profile editing form.
	/// </summary>
	public partial class frmCutEdit : Form
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		/// <summary>
		/// Value indicating whether the form has already been activated for the
		/// first time.
		/// </summary>
		private bool mActivated = false;
		//private FPoint mEndLocation = new FPoint();
		/// <summary>
		/// Entry names for Direction Left Right Enumeration.
		/// </summary>
		private string[] mEntryNamesDLRE = Enum.GetNames<DirectionLeftRightEnum>();
		/// <summary>
		/// Entry names for Offset Left Right Enumeration.
		/// </summary>
		private string[] mEntryNamesOLRE = new string[] {
			"Left", "Center", "Right", "Relative", "Absolute", "None"
		};
		//private string[] mEntryNamesOLRE = Enum.GetNames<OffsetLeftRightEnum>();
		/// <summary>
		/// Entry names for Offset Top Bottom Enumeration.
		/// </summary>
		private string[] mEntryNamesOTBE = new string[] {
			"Top", "Center", "Bottom", "Relative", "Absolute", "None"
		};
		//private string[] mEntryNamesOTBE = Enum.GetNames<OffsetTopBottomEnum>();
		/// <summary>
		///	Currently defined tool names.
		/// </summary>
		private string[] mEntryNamesTool = GetToolNames();
		/// <summary>
		/// Reference to the list of operations that have been resolved while
		/// editing.
		/// </summary>
		/// <remarks>
		/// The contents of this list will be transferred to the caller's cut
		/// profile if the edit session is saved.
		/// </remarks>
		List<PatternOperationItem> mResolvedOperations =
			new List<PatternOperationItem>();
		/// <summary>
		/// Value indicating whether the timer until the next valid redraw has
		/// elapsed.
		/// </summary>
		private bool mRedrawElapsed = false;
		/// <summary>
		/// Value indicating whether a redraw of the preview panel is needed.
		/// </summary>
		private bool mRedrawNeeded = false;
		/// <summary>
		/// The timer used to keep track if a redraw can be performed. This
		/// control might no longer be needed since panel-level double-buffering
		/// has been activated.
		/// </summary>
		private Timer mRedrawTimer = new Timer()
		{
			Enabled = true,
			Interval = 250
		};
		//private FPoint mStartLocation = new FPoint();
		///// <summary>
		///// The list of realtime working values aligned with the property values in
		///// the data table.
		///// </summary>
		//List<string> mWorkingValues = new List<string>();

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
			mnuFormCloseWithoutSaving_Click(sender, e);
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
			mnuFormSaveChangesClose_Click(sender, e);
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* comboBox_LostFocus																										*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// The editing combo box has lost focus.
		///// </summary>
		///// <param name="sender">
		///// The object raising this event.
		///// </param>
		///// <param name="e">
		///// Standard event arguments.
		///// </param>
		//private void comboBox_LostFocus(object sender, EventArgs e)
		//{
		//	if(sender is ComboBox @comboBox)
		//	{
		//		comboBox.SelectedIndexChanged -= comboBox_SelectedIndexChanged;
		//		comboBox.LostFocus -= comboBox_LostFocus;
		//	}
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* comboBox_SelectedIndexChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selected index has changed on the editing combo box.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			StringBuilder builder = new StringBuilder();
			OperationVariableItem opVar = null;
			DataGridViewRow row = null;
			string text = "";

			if(sender is ComboBox @comboBox && comboBox.SelectedItem != null)
			{
				row = dgProperties.Rows[dgProperties.CurrentCell.RowIndex];
				text = comboBox.SelectedItem.ToString();
				//	Update the working cell value.
				opVar = (OperationVariableItem)row.DataBoundItem;
				opVar.WorkingValue = text;
				UpdateWorkpiece();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* dgProperties_CellBeginEdit																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The current data grid cell is going into edit mode.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// DataGridView cell cancel event arguments.
		/// </param>
		private void dgProperties_CellBeginEdit(object sender,
			DataGridViewCellCancelEventArgs e)
		{
			//if(e.ColumnIndex == dgProperties.Columns["Value"].Index)
			//{
			//	//	EditingControl is null during this event.
			//	if(dgProperties.EditingControl is TextBox @textBox)
			//	{
			//		Trace.WriteLine("Selecting text on BeginEdit...");
			//		textBox.SelectAll();
			//	}
			//}

			////DataGridViewCheckBoxCell checkBoxCell = null;
			//DataGridViewComboBoxCell comboBoxCell = null;
			//string controlName = "";
			//string name = "";
			//OperationActionPropertyItem propertyDef = null;
			//DataGridViewTextBoxCell textBoxCell = null;

			//if(e.ColumnIndex == dgProperties.Columns["Value"].Index)
			//{
			//	//	Value column.
			//	name = dgProperties.Rows[e.RowIndex].
			//		Cells["BaseName"].Value.ToString();
			//	propertyDef = ConfigProfile.OperationActionProperties.
			//		FirstOrDefault(x => x.PropertyName == name);
			//	if(propertyDef != null)
			//	{
			//		//	Property definition found.
			//		try
			//		{
			//			switch(propertyDef.DataType)
			//			{
			//				case "AngleString":
			//				case "MeasurementString":
			//				case "String":
			//					//	Textbox.
			//					controlName = "TextBox";
			//					textBoxCell = new DataGridViewTextBoxCell();
			//					dgProperties.Rows[e.RowIndex].Cells[e.ColumnIndex] =
			//						textBoxCell;
			//					break;
			//				case "DirectionLeftRightEnum":
			//					//	ComboBox.
			//					controlName = "Direction Left Right";
			//					comboBoxCell = new DataGridViewComboBoxCell();
			//					comboBoxCell.Items.AddRange(mEntryNamesDLRE);
			//					dgProperties.Rows[e.RowIndex].Cells[e.ColumnIndex] =
			//						comboBoxCell;
			//					break;
			//				case "OffsetLeftRightEnum":
			//					controlName = "Offset Left Right";
			//					comboBoxCell = new DataGridViewComboBoxCell();
			//					comboBoxCell.Items.AddRange(mEntryNamesOLRE);
			//					dgProperties.Rows[e.RowIndex].Cells[e.ColumnIndex] =
			//						comboBoxCell;
			//					break;
			//				case "OffsetTopBottomEnum":
			//					controlName = "Offset Up Down";
			//					comboBoxCell = new DataGridViewComboBoxCell();
			//					comboBoxCell.Items.AddRange(mEntryNamesOTBE);
			//					dgProperties.Rows[e.RowIndex].Cells[e.ColumnIndex] =
			//						comboBoxCell;
			//					break;
			//				case "PlotActionEnum":
			//				case "ToolName":
			//					break;
			//			}
			//		}
			//		//	Absorb any minor errors here...
			//		catch
			//		{
			//			statMessage.Text = $"Error initializing {controlName}";
			//		}
			//	}
			//}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* dgProperties_CellClick																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A cell has been clicked on the properties grid.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// DataGridView cell event arguments.
		/// </param>
		/// <remarks>
		/// This method is included to work around the issue where the user is
		/// normally required to click several times in the cell to activate the
		/// ComboBox drop down list. Upon clicking in a cell that uses a
		/// ComboBox control, that control opens immediately.
		/// </remarks>
		private void dgProperties_CellClick(object sender,
			DataGridViewCellEventArgs e)
		{
			if(e.ColumnIndex == dgProperties.Columns["Value"].Index)
			{
				//	Value column.
				dgProperties.BeginEdit(false);
				if(dgProperties.EditingControl is
					DataGridViewComboBoxEditingControl @comboBoxControl)
				{
					comboBoxControl.DroppedDown = true;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* dgProperties_CellEndEdit																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The cell has left the edit mode.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// DataGridView event arguments.
		/// </param>
		private void dgProperties_CellEndEdit(object sender,
			DataGridViewCellEventArgs e)
		{
			if(e.ColumnIndex == dgProperties.Columns["Value"].Index)
			{
				//	Value column.
				if(dgProperties.EditingControl is
					DataGridViewComboBoxEditingControl @comboBoxControl)
				{
					comboBoxControl.SelectedIndexChanged -=
						comboBox_SelectedIndexChanged;
				}
				else if(dgProperties.EditingControl is
					DataGridViewTextBoxEditingControl @textBoxControl)
				{
					textBoxControl.TextChanged -= editTextBox_TextChanged;
					textBoxControl.GotFocus -= editTextBox_GotFocus;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* dgProperties_CellEnter																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The cell has been entered on the grid.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// DataGridView cell event arguments.
		/// </param>
		private void dgProperties_CellEnter(object sender,
			DataGridViewCellEventArgs e)
		{
			if(e.ColumnIndex == dgProperties.Columns["Value"].Index)
			{
				//	Value column.
				dgProperties.BeginEdit(false);
				if(dgProperties.EditingControl is
					DataGridViewComboBoxEditingControl @comboBoxControl)
				{
					comboBoxControl.DroppedDown = true;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* dgProperties_EditingControlShowing																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The current data grid cell editing control is preparing to show.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// DataGridView editing control showing event arguments.
		/// </param>
		private void dgProperties_EditingControlShowing(object sender,
			DataGridViewEditingControlShowingEventArgs e)
		{
			//int count = 0;
			//int index = 0;
			//OperationVariableItem opVar = null;
			//OperationActionPropertyItem propertyDef = null;
			//DataGridViewRow row = null;
			//string name = "";

			if(dgProperties.CurrentCell.ColumnIndex ==
				dgProperties.Columns["Value"].Index)
			{
				if(e.Control is ComboBox @comboBox)
				{
					//row = dgProperties.Rows[dgProperties.CurrentCell.RowIndex];
					//opVar = (OperationVariableItem)row.DataBoundItem;
					//propertyDef = ConfigProfile.OperationActionProperties.
					//	FirstOrDefault(x => x.PropertyName == opVar.BaseName);

					//if(propertyDef != null)
					//{
					//	if(propertyDef.DataType == "ToolName" &&
					//		comboBox.SelectedIndex == -1)
					//	{
					//		//	Get the default tool name.
					//		name = ConfigProfile.GeneralCuttingTool;
					//		count = mEntryNamesTool.Length;
					//		for(index = 0; index < count; index ++)
					//		{
					//			if(mEntryNamesTool[index] == name)
					//			{
					//				//	Default tool found.
					//				opVar.Value = mEntryNamesTool[index];
					//				//comboBox.SelectedIndex = index;
					//				break;
					//			}
					//		}
					//	}
					//}
					comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
					//comboBox.LostFocus += comboBox_LostFocus;
					comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
				}
				else if(e.Control is TextBox @editTextBox)
				{
					editTextBox.TextChanged += editTextBox_TextChanged;
					editTextBox.GotFocus += editTextBox_GotFocus;
					//editTextBox.LostFocus += editTextBox_LostFocus;
					Trace.WriteLine("Selecting text content...");
					editTextBox.SelectAll();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* editTextBox_GotFocus																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The editing textbox has received focus.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void editTextBox_GotFocus(object sender, EventArgs e)
		{
			if(sender is TextBox @textBox)
			{
				//textBox.SelectionStart = 0;
				//textBox.SelectionLength = textBox.Text.Length;
				textBox.SelectAll();
			}
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* editTextBox_LostFocus																									*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// The grid cell editing textbox has lost focus.
		///// </summary>
		///// <param name="sender">
		///// The object raising this event.
		///// </param>
		///// <param name="e">
		///// Standard event arguments.
		///// </param>
		//private void editTextBox_LostFocus(object sender, EventArgs e)
		//{
		//	if(sender is TextBox @textBox)
		//	{
		//		textBox.TextChanged -= editTextBox_TextChanged;
		//		textBox.LostFocus -= editTextBox_LostFocus;
		//	}
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* editTextBox_TextChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The grid cell editing textbox text has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void editTextBox_TextChanged(object sender, EventArgs e)
		{
			StringBuilder builder = new StringBuilder();
			Match match = null;
			string measurement = "";
			OperationVariableItem opVar = null;
			OperationActionPropertyItem propertyDef = null;
			DataGridViewRow row = null;
			string text = "";

			if(sender is TextBox @textBox)
			{
				row = dgProperties.Rows[dgProperties.CurrentCell.RowIndex];
				builder.Append(row.Cells["DisplayName"].Value.ToString());
				builder.Append(": ");
				opVar = (OperationVariableItem)row.DataBoundItem;
				propertyDef = ConfigProfile.OperationActionProperties.
					FirstOrDefault(x => x.PropertyName == opVar.BaseName);
				if(propertyDef != null)
				{
					switch(propertyDef.DataType)
					{
						case "AngleString":
							match = Regex.Match(textBox.Text, ResourceMain.rxAngleUnit);
							if(match.Success)
							{
								text = GetValue(match, "angle");
								measurement = GetValue(match, "unit");
								if(measurement.Length == 0)
								{
									measurement = "degrees";
								}
								builder.Append($"{text} {measurement}");
							}
							break;
						case "MeasurementString":
							measurement = GetMeasurementString(textBox.Text);
							text = GetAltValue(measurement, textBox.Text, false);
							if(text.Length == 0)
							{
								text = textBox.Text;
							}
							if(text.Length == 0)
							{
								text = $"0 {measurement}";
							}
							builder.Append(text);
							break;
						default:
							builder.Append(textBox.Text);
							break;
					}
				}

				statValue.Text = builder.ToString();
				//	Update the working cell value.
				opVar.WorkingValue = textBox.Text;
				UpdateWorkpiece();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InitializeEditors																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Initialize the editors for each of the cells.
		/// </summary>
		private void InitializeEditors()
		{
			//DataGridViewCheckBoxCell checkBoxCell = null;
			//int columnIndexBase = dgProperties.Columns["BaseName"].Index;
			int columnIndexEdit = dgProperties.Columns["Value"].Index;
			DataGridViewComboBoxCell comboBoxCell = null;
			string controlName = "";
			string name = "";
			OperationVariableItem opVar = null;
			OperationActionPropertyItem propertyDef = null;
			//DataRow row = null;
			DataGridViewTextBoxCell textBoxCell = null;

			//	Value column.
			foreach(DataGridViewRow rowItem in dgProperties.Rows)
			{
				if(rowItem.DataBoundItem != null)
				{
					opVar = (OperationVariableItem)rowItem.DataBoundItem;
					name = opVar.BaseName;
					propertyDef = ConfigProfile.OperationActionProperties.
						FirstOrDefault(x => x.PropertyName == name);
					if(propertyDef != null)
					{
						//	Property definition found.
						try
						{
							switch(propertyDef.DataType)
							{
								case "AngleString":
								case "MeasurementString":
								case "String":
									//	Textbox.
									controlName = "TextBox";
									textBoxCell = new DataGridViewTextBoxCell();
									rowItem.Cells[columnIndexEdit] = textBoxCell;
									break;
								case "DirectionLeftRightEnum":
									//	ComboBox.
									controlName = "Direction Left Right";
									comboBoxCell = new DataGridViewComboBoxCell();
									comboBoxCell.FlatStyle = FlatStyle.Flat;
									comboBoxCell.Style.BackColor = Color.White;
									comboBoxCell.Items.AddRange(mEntryNamesDLRE);
									rowItem.Cells[columnIndexEdit] = comboBoxCell;
									break;
								case "OffsetLeftRightEnum":
									controlName = "Offset Left Right";
									comboBoxCell = new DataGridViewComboBoxCell();
									comboBoxCell.FlatStyle = FlatStyle.Flat;
									comboBoxCell.Style.BackColor = Color.White;
									comboBoxCell.Items.AddRange(mEntryNamesOLRE);
									rowItem.Cells[columnIndexEdit] = comboBoxCell;
									break;
								case "OffsetTopBottomEnum":
									controlName = "Offset Up Down";
									comboBoxCell = new DataGridViewComboBoxCell();
									comboBoxCell.FlatStyle = FlatStyle.Flat;
									comboBoxCell.Style.BackColor = Color.White;
									comboBoxCell.Items.AddRange(mEntryNamesOTBE);
									rowItem.Cells[columnIndexEdit] = comboBoxCell;
									break;
								case "PlotActionEnum":
									break;
								case "ToolName":
									controlName = "Toolname Drop-Down";
									comboBoxCell = new DataGridViewComboBoxCell();
									comboBoxCell.FlatStyle = FlatStyle.Flat;
									comboBoxCell.Style.BackColor = Color.White;
									comboBoxCell.Items.AddRange(mEntryNamesTool);
									rowItem.Cells[columnIndexEdit] = comboBoxCell;
									if(opVar.Value.Length == 0)
									{
										//	The tool hasn't been selected. Select the default.
										opVar.Value = ConfigProfile.GeneralCuttingTool;
									}
									break;
							}
						}
						//	Absorb any minor errors here...
						catch
						{
							statMessage.Text = $"Error initializing {controlName}";
						}
					}
				}
			}

		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFormCloseWithoutSaving_Click																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Form / Close Without Saving menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFormCloseWithoutSaving_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Hide();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mnuFormSaveChangesClose_Click																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Form / Save Changes and Close menu option has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mnuFormSaveChangesClose_Click(object sender, EventArgs e)
		{
			//if(mCutProfile != null)
			//{
			//	//	Transfer the updated operations.
			//	mCutProfile.Operations.Clear();
			//	foreach(PatternOperationItem operationItem in mResolvedOperations)
			//	{
			//		mCutProfile.Operations.Add(operationItem);
			//	}
			//}
			this.DialogResult = DialogResult.OK;
			this.Hide();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlPreview_Paint																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The preview panel is being painted.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Paint event arguments.
		/// </param>
		private void pnlPreview_Paint(object sender, PaintEventArgs e)
		{
			Graphics graphics = e.Graphics;
			FPoint location = FPoint.Clone(mCutProfile.StartLocation);
			float[] moveDashes = { 4, 4 };
			Pen movePen = new Pen(ColorTranslator.FromHtml("#f0007f00"), 2f)
			{
				DashPattern = moveDashes
			};
			FPoint offset = new FPoint();
			//FPoint offsetLocation = null;
			float scale = 1.0f;
			SizeF systemSize = GetSystemSize();
			Rectangle targetArea = new Rectangle(0, 0, 16, 16);
			Brush targetBackgroundBrush =
				new SolidBrush(ColorTranslator.FromHtml("#7fffffff"));
			Pen targetBorderPen =
				new Pen(ColorTranslator.FromHtml("#f0ff0000"), 2f);
			Rectangle workspaceArea = Rectangle.Empty;
			float workspaceRatio = GetWorkspaceRatio();
			Size workspaceSize = Size.Empty;
			//int x1 = 0;
			//int x2 = 0;
			//int y1 = 0;
			//int y2 = 0;

			if(mRedrawElapsed)
			{
				mRedrawElapsed = false;
				DrawTable(pnlPreview, mWorkpieceInfo, e.Graphics);

				//	Set the scale.
				workspaceSize = ResizeArea(
					pnlPreview.Width - 16, pnlPreview.Height - 16, workspaceRatio);
				scale = (float)workspaceSize.Width / systemSize.Width;
				workspaceArea = new Rectangle(
					CenteredLeft(pnlPreview.Width, workspaceSize.Width),
					CenteredTop(pnlPreview.Height, workspaceSize.Height),
					workspaceSize.Width, workspaceSize.Height);

				////	Router position.
				//offsetLocation = TransformFromAbsolute(mCutProfile.EndLocation);
				//targetArea.X =
				//	workspaceArea.Left +
				//		(int)(offsetLocation.X * scale) - (targetArea.Width / 2);
				//targetArea.Y =
				//	workspaceArea.Top +
				//		(int)(offsetLocation.Y * scale) - (targetArea.Height / 2);
				//graphics.FillEllipse(targetBackgroundBrush, targetArea);
				//graphics.DrawEllipse(targetBorderPen, targetArea);
				//x1 = x2 = targetArea.Left + (targetArea.Width / 2);
				//y1 = targetArea.Top;
				//y2 = targetArea.Bottom;
				//graphics.DrawLine(targetBorderPen,
				//	new Point(x1, y1), new Point(x2, y2));
				//x1 = targetArea.Left;
				//x2 = targetArea.Right;
				//y1 = y2 = targetArea.Top + (targetArea.Height / 2);
				//graphics.DrawLine(targetBorderPen,
				//	new Point(x1, y1), new Point(x2, y2));

				location = TransformFromAbsolute(location);
				DrawRouter(location, StartEndEnum.Start, graphics, workspaceArea, scale);
				//foreach(PatternOperationItem operationItem in mResolvedOperations)
				//{
				//	//Trace.WriteLine($"CutEdit Operation: Location: {location}; " +
				//	//	$"Operation: {operationItem.Action}");
				//	location = DrawOperation(operationItem,
				//		mWorkpieceInfo, location, "", graphics, workspaceArea, scale);
				//}
				foreach(PatternOperationItem operationItem in mCutProfile.Operations)
				{
					location = DrawOperation(operationItem, mWorkpieceInfo, location,
						"", graphics, workspaceArea, scale);
				}
				DrawRouter(location, StartEndEnum.End, graphics, workspaceArea, scale);
			}
			else
			{
				//	Queue a redraw for the next pass.
				mRedrawNeeded = true;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlPreview_Resize																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The preview panel is resizing.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void pnlPreview_Resize(object sender, EventArgs e)
		{
			pnlPreview.Invalidate();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mRedrawTimer_Tick																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The redraw timer has elapsed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void mRedrawTimer_Tick(object sender, EventArgs e)
		{
			if(mRedrawNeeded)
			{
				mRedrawElapsed = true;
				mRedrawNeeded = false;
				this.Refresh();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetEditMode																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the edit mode for the form.
		/// </summary>
		/// <param name="editMode">
		/// The edit mode to activate.
		/// </param>
		private void SetEditMode(OperationEditModeEnum editMode)
		{
			StringBuilder builder = new StringBuilder();

			mEditMode = editMode;
			switch(mEditMode)
			{
				case OperationEditModeEnum.CreateFromTemplate:
					builder.Append("Create New Cut");
					break;
				case OperationEditModeEnum.EditSelected:
					builder.Append("Edit Cut");
					break;
			}
			if(mCutProfile != null &&
				mCutProfile.TemplateName?.Length > 0)
			{
				builder.Append(" - ");
				builder.Append(mCutProfile.TemplateName);
			}
			this.Text = builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdateWorkpiece																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the workpiece from the current values.
		/// </summary>
		private void UpdateWorkpiece()
		{
			FPoint location = new FPoint();
			List<OperationVariableItem> variables =
				new List<OperationVariableItem>();

			mResolvedOperations.Clear();
			location = FPoint.Clone(mCutProfile.StartLocation);
			location = TransformFromAbsolute(location);

			//	Transfer user variables to operation.
			foreach(PatternOperationItem operationItem in mCutProfile.Operations)
			{
				//	Process each operation.
				if(operationItem.Action != OperationActionEnum.None)
				{
					variables.Clear();
					foreach(OperationVariableItem variableItem in mSettingsTable)
					{
						if(variableItem.PatternOperations.Contains(operationItem))
						{
							variables.Add(variableItem);
						}
					}
					if(variables.Count > 0)
					{
						//	There are variables defined for this operation.
						OperationVariableCollection.TransferWorkingValues(
							variables, operationItem);
					}
				}
			}
			location = CalculateLayout(mCutProfile, mWorkpieceInfo, location);

			location = TransformToAbsolute(location);
			mCutProfile.EndLocation = location;

			mRedrawNeeded = true;
			//pnlPreview.Refresh();

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
			int gridWidth = 0;

			base.OnActivated(e);
			if(!mActivated)
			{
				//	This is the first time activating the form.
				mActivated = true;
				dgProperties.AutoResizeColumn(
					dgProperties.Columns["DisplayName"].Index);
				foreach(DataGridViewColumn columnItem in dgProperties.Columns)
				{
					gridWidth += columnItem.Width;
				}
				if(dgProperties.Width < gridWidth + 88)
				{
					dgProperties.Width = gridWidth + 88;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* OnLoad																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the Load event when the form has been loaded and is ready to
		/// display.
		/// </summary>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		protected override void OnLoad(EventArgs e)
		{
			StringBuilder builder = new StringBuilder();
			List<string> propertyNames = new List<string>();

			base.OnLoad(e);
			if(mCutProfile != null)
			{
				mSettingsTable.Clear();
				mSettingsTable.SharedVariables.AddRange(mCutProfile.SharedVariables);
				foreach(PatternOperationItem operationItem in mCutProfile.Operations)
				{
					mSettingsTable.AddOperation(operationItem);
				}
				dgProperties.DataSource = mSettingsTable;
				InitializeEditors();
				UpdateWorkpiece();
			}
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the frmCutEdit Item.
		/// </summary>
		public frmCutEdit()
		{
			InitializeComponent();

			//	Double-buffering on Workspace panel.
			typeof(Panel).InvokeMember("DoubleBuffered",
				BindingFlags.SetProperty | BindingFlags.Instance |
				BindingFlags.NonPublic,
				null, pnlPreview, new object[] { true });

			this.DoubleBuffered = true;

			mRedrawTimer.Tick += mRedrawTimer_Tick;

			dgProperties.EditMode = DataGridViewEditMode.EditProgrammatically;
			dgProperties.MultiSelect = false;


			btnCancel.Click += btnCancel_Click;
			btnOK.Click += btnOK_Click;

			dgProperties.CellBeginEdit += dgProperties_CellBeginEdit;
			dgProperties.CellClick += dgProperties_CellClick;
			dgProperties.CellEndEdit += dgProperties_CellEndEdit;
			dgProperties.CellEnter += dgProperties_CellEnter;
			dgProperties.EditingControlShowing += dgProperties_EditingControlShowing;

			mnuFormCloseWithoutSaving.Click += mnuFormCloseWithoutSaving_Click;
			mnuFormSaveChangesClose.Click += mnuFormSaveChangesClose_Click;

			pnlPreview.Paint += pnlPreview_Paint;
			pnlPreview.Resize += pnlPreview_Resize;

			this.CancelButton = btnCancel;
			this.AcceptButton = btnOK;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CreateFromTemplate																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a cut profile from a blank template.
		/// </summary>
		/// <param name="template">
		/// Reference to the pattern template serving as the basis of the new cut.
		/// </param>
		/// <param name="startLocation">
		/// Reference to the starting router location for this cut, in absolute
		/// terms.
		/// </param>
		public void CreateFromTemplate(PatternTemplateItem template, FPoint
			startLocation)
		{
			FPoint start = startLocation;

			if(template != null)
			{
				if(start == null)
				{
					start = new FPoint();
				}
				mCutProfile = new CutProfileItem(template);
				mCutProfile.StartLocation = start;
				SetEditMode(OperationEditModeEnum.CreateFromTemplate);
				UpdateWorkpiece();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	CutProfile																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="CutProfile">CutProfile</see>.
		/// </summary>
		private CutProfileItem mCutProfile = null;
		/// <summary>
		/// Get/Set a reference to the cut profile to be edited in this session.
		/// </summary>
		public CutProfileItem CutProfile
		{
			get { return mCutProfile; }
			//set
			//{
			//	mCutProfile = value;
			//	EditMode = OperationEditModeEnum.EditSelected;
			//}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* EditCut																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Edit the provided cut.
		/// </summary>
		/// <param name="cutProfile">
		/// Reference to the cut to be edited.
		/// </param>
		public void EditCut(CutProfileItem cutProfile)
		{
			if(cutProfile != null)
			{
				mCutProfile = CutProfileItem.Clone(cutProfile);
				SetEditMode(OperationEditModeEnum.EditSelected);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	EditMode																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="EditMode">EditMode</see>.
		/// </summary>
		private OperationEditModeEnum mEditMode = OperationEditModeEnum.None;
		/// <summary>
		/// Get/Set the mode under which this cut will be edited.
		/// </summary>
		public OperationEditModeEnum EditMode
		{
			get { return mEditMode; }
			//set
			//{
			//	StringBuilder builder = new StringBuilder();

			//	mEditMode = value;
			//	switch(mEditMode)
			//	{
			//		case OperationEditModeEnum.CreateFromTemplate:
			//			builder.Append("Create New Cut");
			//			break;
			//		case OperationEditModeEnum.EditSelected:
			//			builder.Append("Edit Cut");
			//			break;
			//	}
			//	if(mCutProfile != null &&
			//		mCutProfile.TemplateName?.Length > 0)
			//	{
			//		builder.Append(" - ");
			//		builder.Append(mCutProfile.TemplateName);
			//	}
			//	this.Text = builder.ToString();
			//}
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	PatternTemplate																												*
		////*-----------------------------------------------------------------------*
		//private PatternTemplateItem mPatternTemplate = null;
		///// <summary>
		///// Get/Set a reference to the pattern template for the cut being edited.
		///// </summary>
		//public PatternTemplateItem PatternTemplate
		//{
		//	get { return mPatternTemplate; }
		//	set
		//	{
		//		mPatternTemplate = value;
		//		mCutProfile = new CutProfileItem(mPatternTemplate);
		//		EditMode = OperationEditModeEnum.CreateFromTemplate;
		//	}
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SettingsTable																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="SettingsTable">SettingsTable</see>.
		/// </summary>
		private OperationVariableCollection mSettingsTable =
			new OperationVariableCollection();
		/// <summary>
		/// Get a reference to the collection containing the runtime user settings
		/// for this edit.
		/// </summary>
		public OperationVariableCollection SettingsTable
		{
			get { return mSettingsTable; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WorkpieceInfo																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="WorkpieceInfo">WorkpieceInfo</see>.
		/// </summary>
		private WorkpieceInfoItem mWorkpieceInfo =
			WorkpieceInfoItem.Clone(SessionWorkpieceInfo);
		/// <summary>
		/// Get/Set a reference to the workpiece information for which the cut is
		/// being edited.
		/// </summary>
		public WorkpieceInfoItem WorkpieceInfo
		{
			get { return mWorkpieceInfo; }
			//set { mWorkpieceInfo = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
