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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	frmEditOperation																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Operation editing dialog.
	/// </summary>
	public partial class frmEditOperation : Form
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		/// <summary>
		/// Value indicating whether the form has already been activated for the
		/// first time.
		/// </summary>
		private bool mActivated = false;
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
		/// <summary>
		///	Currently defined tool names.
		/// </summary>
		private string[] mEntryNamesTool = GetToolNames();
		/// <summary>
		/// Value indicating whether the fields have been made ready.
		/// </summary>
		private bool mReady = false;

		//*-----------------------------------------------------------------------*
		//* AutoSizeGrid																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Auto-size the grid.
		/// </summary>
		private void AutoSizeGrid()
		{
			if(dgVariables.Columns.Count > 0)
			{
				dgVariables.AutoResizeColumn(
					dgVariables.Columns["DisplayName"].Index);
				dgVariables.AutoResizeColumn(
					dgVariables.Columns["Value"].Index);
			}
		}
		//*-----------------------------------------------------------------------*

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
			UpdateEntry();
			this.DialogResult = DialogResult.Cancel;
			this.Hide();
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
			UpdateEntry();
			this.DialogResult = DialogResult.OK;
			this.Hide();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* cmboAction_SelectedIndexChanged																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selected index has changed on the Action combo box.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void cmboAction_SelectedIndexChanged(object sender, EventArgs e)
		{
			OperationActionEnum action = OperationActionEnum.None;

			if(cmboAction.SelectedIndex > -1)
			{
				if(Enum.TryParse<OperationActionEnum>(
					cmboAction.SelectedItem.ToString().Replace(" ", ""),
						true, out action))
				{
					mOperation.Action = action;
				}
				else
				{
					mOperation.Action = OperationActionEnum.None;
				}
			}
			else
			{
				mOperation.Action = OperationActionEnum.None;
			}
			if(mActivated)
			{
				UpdateGrid();
			}
		}
		//*-----------------------------------------------------------------------*

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
			OperationActionPropertyEditItem opVar = null;
			DataGridViewRow row = null;
			string text = "";

			if(sender is ComboBox @comboBox && comboBox.SelectedItem != null)
			{
				row = dgVariables.Rows[dgVariables.CurrentCell.RowIndex];
				text = comboBox.SelectedItem.ToString();
				//	Update the working cell value.
				opVar = (OperationActionPropertyEditItem)row.DataBoundItem;
				opVar.Value = text;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* dgVariables_CellBeginEdit																							*
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
		private void dgVariables_CellBeginEdit(object sender,
			DataGridViewCellCancelEventArgs e)
		{
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* dgVariables_CellClick																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A cell has been clicked on the variables grid.
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
		private void dgVariables_CellClick(object sender,
			DataGridViewCellEventArgs e)
		{
			if(e.ColumnIndex == dgVariables.Columns["Value"].Index)
			{
				//	Value column.
				dgVariables.BeginEdit(false);
				if(dgVariables.EditingControl is
					DataGridViewComboBoxEditingControl @comboBoxControl)
				{
					comboBoxControl.DroppedDown = true;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* dgVariables_CellEndEdit																								*
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
		private void dgVariables_CellEndEdit(object sender,
			DataGridViewCellEventArgs e)
		{
			if(e.ColumnIndex == dgVariables.Columns["Value"].Index)
			{
				//	Value column.
				if(dgVariables.EditingControl is
					DataGridViewComboBoxEditingControl @comboBoxControl)
				{
					comboBoxControl.SelectedIndexChanged -=
						comboBox_SelectedIndexChanged;
				}
				else if(dgVariables.EditingControl is
					DataGridViewTextBoxEditingControl @textBoxControl)
				{
					textBoxControl.TextChanged -= editTextBox_TextChanged;
					textBoxControl.GotFocus -= editTextBox_GotFocus;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* dgVariables_CellEnter																									*
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
		private void dgVariables_CellEnter(object sender,
			DataGridViewCellEventArgs e)
		{
			if(e.ColumnIndex == dgVariables.Columns["Value"].Index)
			{
				//	Value column.
				dgVariables.BeginEdit(false);
				if(dgVariables.EditingControl is
					DataGridViewComboBoxEditingControl @comboBoxControl)
				{
					comboBoxControl.DroppedDown = true;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* dgVariables_EditingControlShowing																			*
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
		private void dgVariables_EditingControlShowing(object sender,
			DataGridViewEditingControlShowingEventArgs e)
		{
			if(dgVariables.CurrentCell.ColumnIndex ==
				dgVariables.Columns["Value"].Index)
			{
				if(e.Control is ComboBox @comboBox)
				{
					comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
					comboBox.SelectedIndexChanged += comboBox_SelectedIndexChanged;
				}
				else if(e.Control is TextBox @editTextBox)
				{
					editTextBox.TextChanged += editTextBox_TextChanged;
					editTextBox.GotFocus += editTextBox_GotFocus;
					//editTextBox.LostFocus += editTextBox_LostFocus;
					//Trace.WriteLine("Selecting text content...");
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
				textBox.SelectAll();
			}
		}
		//*-----------------------------------------------------------------------*

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
			OperationActionPropertyEditItem opVar = null;
			OperationActionPropertyItem propertyDef = null;
			DataGridViewRow row = null;
			string text = "";

			if(sender is TextBox @textBox)
			{
				row = dgVariables.Rows[dgVariables.CurrentCell.RowIndex];
				builder.Append(row.Cells["DisplayName"].Value.ToString());
				builder.Append(": ");
				opVar = (OperationActionPropertyEditItem)row.DataBoundItem;
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

				lblStatValue.Text = builder.ToString();
				//	Update the working cell value.
				//opVar.WorkingValue = textBox.Text;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetDisplayName																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the display name for the specified property name.
		/// </summary>
		/// <param name="propertyName">
		/// Name of the property to format.
		/// </param>
		/// <returns>
		/// The display name of the provided property.
		/// </returns>
		private string GetDisplayName(string propertyName)
		{
			StringBuilder builder = new StringBuilder();

			if(propertyName?.Length > 0)
			{
				if(txtOperationName.Text.Length > 0)
				{
					builder.Append(txtOperationName.Text);
					builder.Append(' ');
				}
				builder.Append(ExpandCamelCase(propertyName));
			}
			return builder.ToString();
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
			int columnIndexEdit = dgVariables.Columns["Value"].Index;
			DataGridViewComboBoxCell comboBoxCell = null;
			string controlName = "";
			string name = "";
			OperationActionPropertyEditItem opVar = null;
			OperationActionPropertyItem propertyDef = null;
			DataGridViewTextBoxCell textBoxCell = null;

			Trace.WriteLine("frmEditOperation.InitializeEditors()");
			//	Value column.
			foreach(DataGridViewRow rowItem in dgVariables.Rows)
			{
				if(rowItem.DataBoundItem != null)
				{
					opVar = (OperationActionPropertyEditItem)rowItem.DataBoundItem;
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
							lblStatValue.Text = $"Error initializing {controlName}";
						}
					}
				}
			}
			AutoSizeGrid();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtOperationName_TextChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text in the operation name has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtOperationName_TextChanged(object sender, EventArgs e)
		{
			foreach(OperationActionPropertyEditItem editItem in mSettingsTable)
			{
				editItem.DisplayName = GetDisplayName(editItem.BaseName);
			}
			AutoSizeGrid();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdateEntry																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the record entry.
		/// </summary>
		private void UpdateEntry()
		{
			int count = 0;
			DirectionLeftRightEnum direction = DirectionLeftRightEnum.None;
			int index = 0;
			OffsetLeftRightEnum offsetlr = OffsetLeftRightEnum.None;
			OffsetTopBottomEnum offsettb = OffsetTopBottomEnum.None;
			List<string> result = new List<string>();

			if(mOperation != null)
			{
				//	Transfer operation name.
				mOperation.OperationName = txtOperationName.Text;
				//	Transfer hidden variables.
				mOperation.HiddenVariables.Clear();
				count = lstHiddenVariables.CheckedItems.Count;
				for(index = 0; index < count; index++)
				{
					mOperation.HiddenVariables.Add(
						lstHiddenVariables.CheckedItems[index].ToString());
				}
				//	Transfer field values.
				foreach(OperationActionPropertyEditItem editItem in
					mSettingsTable)
				{
					switch(editItem.DataType)
					{
						case "AngleString":
						case "MeasurementString":
						case "String":
							//	Textbox.
							if(editItem.Value.Length > 0)
							{
								//	Only store non-blank text values.
								PatternOperationItem.SetValue(mOperation,
									editItem.BaseName, editItem.Value);
							}
							break;
						case "DirectionLeftRightEnum":
							if(editItem.Value != "None" &&
								Enum.TryParse<DirectionLeftRightEnum>(
									editItem.Value, true, out direction))
							{
								PatternOperationItem.SetValue(mOperation,
									editItem.BaseName, direction);
							}
							break;
						case "OffsetLeftRightEnum":
							if(editItem.Value != "None" &&
								Enum.TryParse<OffsetLeftRightEnum>(
									editItem.Value, true, out offsetlr))
							{
								PatternOperationItem.SetValue(mOperation,
									editItem.BaseName, offsetlr);
							}
							break;
						case "OffsetTopBottomEnum":
							if(editItem.Value != "None" &&
								Enum.TryParse<OffsetTopBottomEnum>(
									editItem.Value, true, out offsettb))
							{
								PatternOperationItem.SetValue(mOperation,
									editItem.BaseName, offsettb);
							}
							break;
						case "ToolName":
							if(editItem.Value.Length > 0 &&
								editItem.Value != ConfigProfile.GeneralCuttingTool)
							{
								//	Only set the tool name if not using the default.
								mOperation.Tool = editItem.Value;
							}
							break;
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdateGrid																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the grid to reflect the properties present for the selected
		/// action.
		/// </summary>
		private void UpdateGrid()
		{
			List<OperationActionPropertyItem> propertyDefs = null;

			Trace.WriteLine("frmEditOperation.UpdateGrid()");
			mSettingsTable.Clear();
			lstHiddenVariables.Items.Clear();
			if(mOperation.Action != OperationActionEnum.None)
			{
				propertyDefs = ConfigProfile.OperationActionProperties.
					FindAll(x => x.IncludeOperationActions.Contains(
						mOperation.Action.ToString()));
				foreach(OperationActionPropertyItem propertyItem in propertyDefs)
				{
					mSettingsTable.Add(new OperationActionPropertyEditItem()
					{
						DataType = propertyItem.DataType,
						BaseName = propertyItem.PropertyName,
						DisplayName = GetDisplayName(propertyItem.PropertyName),
						Value = ""
					});
					lstHiddenVariables.Items.Add(propertyItem.PropertyName);
				}
				InitializeEditors();
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

			base.OnActivated(e);
			if(!mActivated)
			{
				//	This is the first time activating the form.
				mActivated = true;
				AutoSizeGrid();
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

			if(!mReady)
			{
				cmboAction.SelectedItem = mOperation.Action.ToString();
				UpdateGrid();
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
		/// Create a new instance of the frmEditOperation Item.
		/// </summary>
		public frmEditOperation()
		{
			int count = 0;
			int index = 0;
			List<string> names = null;

			InitializeComponent();

			this.CancelButton = btnCancel;
			this.AcceptButton = btnOK;

			//	Buttons.
			btnCancel.Click += btnCancel_Click;
			btnOK.Click += btnOK_Click;

			//	Action combo.
			names = Enum.GetNames<OperationActionEnum>().ToList();
			count = names.Count;
			for(index = 0; index < count; index ++)
			{
				names[index] = ExpandCamelCase(names[index]);
			}
			cmboAction.Items.AddRange(names.ToArray());
			cmboAction.SelectedIndexChanged += cmboAction_SelectedIndexChanged;

			//	Data grid.
			dgVariables.EditMode = DataGridViewEditMode.EditProgrammatically;
			dgVariables.MultiSelect = false;

			dgVariables.CellBeginEdit += dgVariables_CellBeginEdit;
			dgVariables.CellClick += dgVariables_CellClick;
			dgVariables.CellEndEdit += dgVariables_CellEndEdit;
			dgVariables.CellEnter += dgVariables_CellEnter;
			dgVariables.EditingControlShowing += dgVariables_EditingControlShowing;

			dgVariables.DataSource = mSettingsTable;

			//	Text.
			txtOperationName.TextChanged += txtOperationName_TextChanged;
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	Action																																*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for <see cref="Action">Action</see>.
		///// </summary>
		//private OperationActionEnum mAction = OperationActionEnum.None;
		///// <summary>
		///// Get/Set the action associated with this operation.
		///// </summary>
		//public OperationActionEnum Action
		//{
		//	get { return mAction; }
		//	set
		//	{
		//		mAction = value;
		//		if(mActivated)
		//		{
		//			cmboAction.SelectedItem = ExpandCamelCase(mAction.ToString());
		//		}
		//	}
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CreateOperation																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create an operation record and edit it.
		/// </summary>
		public void CreateOperation()
		{
			this.Text = "New Operation";
			cmboAction.SelectedItem = "None";
			UpdateGrid();
			mReady = true;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* EditOperation																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Edit the provided operation.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation to be edited.
		/// </param>
		/// <remarks>
		/// A clone of the provided operation is created for the editing session.
		/// </remarks>
		public void EditOperation(PatternOperationItem operation)
		{
			int count = 0;
			int index = 0;
			string text = "";

			if(operation != null)
			{
				mOperation = PatternOperationItem.Clone(operation);
				cmboAction.SelectedItem =
					ExpandCamelCase(Operation.Action.ToString());
				txtOperationName.Text = mOperation.OperationName;
				UpdateGrid();
				foreach(OperationActionPropertyEditItem editItem in mSettingsTable)
				{
					text = PatternOperationItem.GetValue(mOperation,
						editItem.BaseName);
					switch(editItem.DataType)
					{
						case "AngleString":
						case "MeasurementString":
						case "String":
							//	Textbox.
							if(text.Length > 0)
							{
								//	Only store non-blank text values.
								editItem.Value = text;
							}
							break;
						case "DirectionLeftRightEnum":
						case "OffsetLeftRightEnum":
						case "OffsetTopBottomEnum":
							if(text != "None")
							{
								editItem.Value = text;
							}
							break;
						case "ToolName":
							if(text.Length == 0)
							{
								editItem.Value = ConfigProfile.GeneralCuttingTool;
							}
							break;
					}
				}
				count = lstHiddenVariables.Items.Count;
				foreach(string entryItem in mOperation.HiddenVariables)
				{
					for(index = 0; index < count; index++)
					{
						text = lstHiddenVariables.Items[index].ToString();
						if(text == entryItem)
						{
							lstHiddenVariables.SetItemChecked(index, true);
							break;
						}
					}
				}
			}
			else
			{
				mOperation = null;
			}
			mReady = true;
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	HiddenVariables																												*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Get a reference to the list of hidden variables for this operation.
		///// </summary>
		//public List<string> HiddenVariables
		//{
		//	get
		//	{
		//		int count = 0;
		//		int index = 0;
		//		List<string> result = new List<string>();

		//		count = lstHiddenVariables.CheckedItems.Count;
		//		for(index = 0; index < count; index ++)
		//		{
		//			result.Add(lstHiddenVariables.CheckedItems[index].ToString());
		//		}
		//		return result;
		//	}
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Operation																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Operation">Operation</see>.
		/// </summary>
		private PatternOperationItem mOperation = new PatternOperationItem();
		/// <summary>
		/// Get a reference to the operation being edited.
		/// </summary>
		public PatternOperationItem Operation
		{
			get { return mOperation; }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	OperationName																													*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Get/Set the operation name used for grouping and arranging the
		///// properties of multiple actions.
		///// </summary>
		//public string OperationName
		//{
		//	get { return txtOperationName.Text; }
		//	set { txtOperationName.Text = value; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SettingsTable																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="SettingsTable">SettingsTable</see>.
		/// </summary>
		private OperationActionPropertyEditCollection mSettingsTable =
			new OperationActionPropertyEditCollection();
		/// <summary>
		/// Get a reference to the collection containing the runtime user settings
		/// for this edit.
		/// </summary>
		public OperationActionPropertyEditCollection SettingsTable
		{
			get { return mSettingsTable; }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	Title																																	*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Get/Set the title of the form.
		///// </summary>
		//public string Title
		//{
		//	get { return this.Text; }
		//	set { this.Text = value; }
		//}
		////*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
