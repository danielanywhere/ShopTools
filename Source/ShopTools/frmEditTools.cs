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

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	frmEditTools																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// User tool editing dialog.
	/// </summary>
	public partial class frmEditTools : Form
	{
		//	DP20241229.0742 - In this version, the base value is that converted
		//	from the user input to the unit selected in the configuration profile.
		//	That value will be converted to mm during the generation of the g-code
		//	file.
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private bool mControlBusy = false;

		//*-----------------------------------------------------------------------*
		//* btnAdd_Click																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Add button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnAdd_Click(object sender, EventArgs e)
		{
			int index = 0;
			UserToolItem tool = new UserToolItem();

			while(true)
			{
				tool.ToolName = $"New Tool {++index}";
				if(!mWorkingUserTools.Exists(x => x.ToolName == tool.ToolName))
				{
					mWorkingUserTools.Add(tool);
					lstDefinedTools.Items.Add(tool);
					lstDefinedTools.SelectedItem = tool;
					cmboToolType.SelectedIndex = -1;
					if(cmboToolType.Items.Count > 0)
					{
						cmboToolType.SelectedIndex = 0;
					}
					break;
				}
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
			this.DialogResult = DialogResult.Cancel;
			this.Hide();
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
			if(mSelectedTool != null)
			{
				mWorkingUserTools.Remove(mSelectedTool);
				lstDefinedTools.Items.Remove(mSelectedTool);
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
			btnAdd.Enabled = false;
			btnCancel.Enabled = false;
			btnDelete.Enabled = false;
			btnOK.Enabled = false;
			lstDefinedTools.Enabled = false;
			grpProperties.Enabled = false;

			mConfiguration.UserTools.Clear();
			mConfiguration.UserTools.AddRange(mWorkingUserTools);

			this.DialogResult = DialogResult.OK;
			this.Hide();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* cmboToolType_SelectedIndexChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The underlying tool type has been changed for the selected tool.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void cmboToolType_SelectedIndexChanged(object sender, EventArgs e)
		{
			if(!mControlBusy)
			{
				mControlBusy = true;
				if(mSelectedTool != null)
				{
					if(cmboToolType.SelectedIndex > -1)
					{
						mSelectedTool.ToolType = cmboToolType.SelectedItem.ToString();
					}
					else
					{
						mSelectedTool.ToolType = "";
					}
				}
				UpdateAvailableProperties();
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* lstDefinedTools_SelectedIndexChanged																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selected index has changed on the defined tools list.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void lstDefinedTools_SelectedIndexChanged(object sender,
			EventArgs e)
		{
			if(lstDefinedTools.SelectedIndex > -1)
			{
				SelectedTool = (UserToolItem)lstDefinedTools.SelectedItem;
				btnDelete.Enabled = true;
			}
			else
			{
				SelectedTool = null;
				btnDelete.Enabled = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RefreshConfiguration																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Refresh the local representation of the currently assigned
		/// configuration.
		/// </summary>
		private void RefreshConfiguration()
		{
			UserToolItem tool = null;

			cmboToolType.Items.Clear();
			lstDefinedTools.Items.Clear();
			mWorkingUserTools.Clear();
			foreach(ToolTypeDefinitionItem defItem in
				mConfiguration.ToolTypeDefinitions)
			{
				if(defItem.Supported)
				{
					cmboToolType.Items.Add(defItem.ToolType);
				}
			}
			foreach(UserToolItem toolItem in mConfiguration.UserTools)
			{
				tool = UserToolItem.Clone(toolItem);
				mWorkingUserTools.Add(tool);
				lstDefinedTools.Items.Add(tool);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtAngle_TextChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text of the Angle textbox has been changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtAngle_TextChanged(object sender, EventArgs e)
		{
			if(!mControlBusy && mSelectedTool != null)
			{
				mControlBusy = true;
				mSelectedTool.Properties["Angle"].Value =
					GetFloatString(txtAngle.Text);
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtBottomGuideDiameter_TextChanged																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text of the Bottom Guide Diameter textbox has been changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtBottomGuideDiameter_TextChanged(object sender, EventArgs e)
		{
			string text = "";

			if(!mControlBusy && mSelectedTool != null)
			{
				mControlBusy = true;
				mSelectedTool.Properties["UserBottomGuideDiameter"].Value =
					txtBottomGuideDiameter.Text;
				text = GetMeasurementString(txtBottomGuideDiameter.Text,
					BaseUnit(mConfiguration.DisplayUnits));
				mSelectedTool.Properties["BottomGuideDiameter"].Value = text;
				lblBottomGuideDiameterUnit.Text =
					GetAltValue(text, txtBottomGuideDiameter.Text);
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtBottomGuideHeight_TextChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text of the Bottom Guide Height textbox has been changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtBottomGuideHeight_TextChanged(object sender, EventArgs e)
		{
			string text = "";

			if(!mControlBusy && mSelectedTool != null)
			{
				mControlBusy = true;
				mSelectedTool.Properties["UserBottomGuideHeight"].Value =
					txtBottomGuideHeight.Text;
				text = GetMeasurementString(txtBottomGuideHeight.Text,
					BaseUnit(mConfiguration.DisplayUnits));
				mSelectedTool.Properties["BottomGuideHeight"].Value = text;
				lblBottomGuideHeightUnit.Text =
					GetAltValue(text, txtBottomGuideHeight.Text);
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtDiameter_TextChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text of the Diameter textbox has been changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtDiameter_TextChanged(object sender, EventArgs e)
		{
			string text = "";

			if(!mControlBusy && mSelectedTool != null)
			{
				mControlBusy = true;
				mSelectedTool.Properties["UserDiameter"].Value =
					txtDiameter.Text;
				text = GetMeasurementString(txtDiameter.Text,
					BaseUnit(mConfiguration.DisplayUnits));
				mSelectedTool.Properties["Diameter"].Value = text;
				lblDiameterUnit.Text =
					GetAltValue(text, txtDiameter.Text);
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtFluteCount_TextChanged																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text of the Flute Count textbox has been changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtFluteCount_TextChanged(object sender, EventArgs e)
		{
			if(!mControlBusy && mSelectedTool != null)
			{
				mControlBusy = true;
				mSelectedTool.Properties["UserFluteCount"].Value =
					txtFluteCount.Text;
				mSelectedTool.Properties["FluteCount"].Value =
					GetIntegerString(txtFluteCount.Text);
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtFluteLength_TextChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text of the Flute Length textbox has been changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtFluteLength_TextChanged(object sender, EventArgs e)
		{
			string text = "";

			if(!mControlBusy && mSelectedTool != null)
			{
				mControlBusy = true;
				mSelectedTool.Properties["UserFluteLength"].Value =
					txtFluteLength.Text;
				text = GetMeasurementString(txtFluteLength.Text,
					BaseUnit(mConfiguration.DisplayUnits));
				mSelectedTool.Properties["FluteLength"].Value = text;
				lblFluteLengthUnit.Text =
					GetAltValue(text, txtFluteLength.Text);
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtShaftLength_TextChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text of the Shaft Length textbox has been changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtShaftLength_TextChanged(object sender, EventArgs e)
		{
			string text = "";

			if(!mControlBusy && mSelectedTool != null)
			{
				mControlBusy = true;
				mSelectedTool.Properties["UserShaftLength"].Value =
					txtShaftLength.Text;
				text = GetMeasurementString(txtShaftLength.Text,
					BaseUnit(mConfiguration.DisplayUnits));
				mSelectedTool.Properties["ShaftLength"].Value = text;
				lblShaftLengthUnit.Text =
					GetAltValue(text, txtShaftLength.Text);
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtToolName_LostFocus																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Tool Name textbox has lost focus.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtToolName_LostFocus(object sender, EventArgs e)
		{
			int selectedIndex = -1;

			//if(!mControlBusy)
			//{
			//	mControlBusy = true;
				if(mToolNameTextChanged)
				{
					if(lstDefinedTools.SelectedIndex > -1)
					{
						selectedIndex = lstDefinedTools.SelectedIndex;
						lstDefinedTools.SelectedIndex = -1;
						lstDefinedTools.Items.Clear();
						foreach(UserToolItem toolItem in mWorkingUserTools)
						{
							lstDefinedTools.Items.Add(toolItem);
						}
						if(selectedIndex > -1)
						{
							lstDefinedTools.SelectedIndex = selectedIndex;
							cmboToolType.Focus();
						}
					}
					mToolNameTextChanged = false;
				}
			//	mControlBusy = false;
			//}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtToolName_TextChanged																								*
		//*-----------------------------------------------------------------------*
		private bool mToolNameTextChanged = false;
		/// <summary>
		/// The text of the Tool Name textbox has been changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtToolName_TextChanged(object sender, EventArgs e)
		{
			if(!mControlBusy && mSelectedTool != null)
			{
				mControlBusy = true;
				if(mSelectedTool.ToolName != txtToolName.Text)
				{
					mToolNameTextChanged = true;
					mSelectedTool.ToolName = txtToolName.Text;
				}
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtTopGuideDiameter_TextChanged																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text of the Top Guide Diameter textbox has been changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtTopGuideDiameter_TextChanged(object sender, EventArgs e)
		{
			string text = "";

			if(!mControlBusy && mSelectedTool != null)
			{
				mControlBusy = true;
				mSelectedTool.Properties["UserTopGuideDiameter"].Value =
					txtTopGuideDiameter.Text;
				text = GetMeasurementString(txtTopGuideDiameter.Text,
					BaseUnit(mConfiguration.DisplayUnits));
				mSelectedTool.Properties["TopGuideDiameter"].Value = text;
				lblTopGuideDiameterUnit.Text =
					GetAltValue(text, txtTopGuideDiameter.Text);
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtTopGuideHeight_TextChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text of the Top Guide Height textbox has been changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtTopGuideHeight_TextChanged(object sender, EventArgs e)
		{
			string text = "";

			if(!mControlBusy && mSelectedTool != null)
			{
				mControlBusy = true;
				mSelectedTool.Properties["UserTopGuideHeight"].Value =
					txtTopGuideHeight.Text;
				text = GetMeasurementString(txtTopGuideHeight.Text,
					BaseUnit(mConfiguration.DisplayUnits));
				mSelectedTool.Properties["TopGuideHeight"].Value = text;
				lblTopGuideHeightUnit.Text =
					GetAltValue(text, txtTopGuideHeight.Text);
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdateAvailableProperties																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the control surface to reflect the properties available for this
		/// tool type.
		/// </summary>
		private void UpdateAvailableProperties()
		{
			ToolTypeDefinitionItem toolType = null;

			if(grpProperties.Enabled && cmboToolType.SelectedIndex > -1)
			{
				toolType = mConfiguration.ToolTypeDefinitions.FirstOrDefault(x =>
					x.Supported && x.ToolType == cmboToolType.SelectedItem.ToString());
				if(toolType != null)
				{
					//	A tool type was defined.
					lblAngle.Enabled =
						txtAngle.Enabled =
							lblAngleUnit.Enabled =
								toolType.PublishedProperties.Contains("Angle");
					lblBottomGuideDiameter.Enabled =
						txtBottomGuideDiameter.Enabled =
							toolType.PublishedProperties.Contains("BottomGuideDiameter");
					lblBottomGuideHeight.Enabled =
						txtBottomGuideHeight.Enabled =
							toolType.PublishedProperties.Contains("BottomGuideHeight");
					lblDiameter.Enabled =
						txtDiameter.Enabled =
							toolType.PublishedProperties.Contains("Diameter");
					lblFluteCount.Enabled =
						txtFluteCount.Enabled =
							toolType.PublishedProperties.Contains("FluteCount");
					lblFluteLength.Enabled =
						txtFluteLength.Enabled =
							toolType.PublishedProperties.Contains("FluteLength");
					lblShaftLength.Enabled =
						txtShaftLength.Enabled =
							toolType.PublishedProperties.Contains("ShaftLength");
					lblTopGuideDiameter.Enabled =
						txtTopGuideDiameter.Enabled =
							toolType.PublishedProperties.Contains("TopGuideDiameter");
					lblTopGuideHeight.Enabled =
						txtTopGuideHeight.Enabled =
							toolType.PublishedProperties.Contains("TopGuideHeight");
				}
			}
			else
			{
				//lblAngle.Visible = lblAngleUnit.Visible = txtAngle.Visible =
				//	lblBottomGuideDiameter.Visible = txtBottomGuideDiameter.Visible =
				//	lblBottomGuideHeight.Visible = txtBottomGuideHeight.Visible =
				//	lblDiameter.Visible = txtDiameter.Visible =
				//	lblFluteCount.Visible = txtFluteCount.Visible =
				//	lblFluteLength.Visible = txtFluteLength.Visible =
				//	lblShaftLength.Visible = txtShaftLength.Visible =
				//	lblTopGuideDiameter.Visible = txtTopGuideDiameter.Visible =
				//	lblTopGuideHeight.Visible = txtTopGuideHeight.Visible = false;
			}
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
		/// Create a new instance of the frmEditTools Item.
		/// </summary>
		public frmEditTools()
		{
			InitializeComponent();

			cmboToolType.Items.Clear();
			RefreshConfiguration();

			btnAdd.Click += btnAdd_Click;
			btnCancel.Click += btnCancel_Click;
			btnDelete.Click += btnDelete_Click;
			btnOK.Click += btnOK_Click;
			cmboToolType.SelectedIndexChanged += cmboToolType_SelectedIndexChanged;
			lstDefinedTools.SelectedIndexChanged +=
				lstDefinedTools_SelectedIndexChanged;
			txtAngle.TextChanged += txtAngle_TextChanged;
			txtBottomGuideDiameter.TextChanged += txtBottomGuideDiameter_TextChanged;
			txtBottomGuideHeight.TextChanged += txtBottomGuideHeight_TextChanged;
			txtDiameter.TextChanged += txtDiameter_TextChanged;
			txtFluteCount.TextChanged += txtFluteCount_TextChanged;
			txtFluteLength.TextChanged += txtFluteLength_TextChanged;
			txtShaftLength.TextChanged += txtShaftLength_TextChanged;
			txtToolName.TextChanged += txtToolName_TextChanged;
			txtToolName.LostFocus += txtToolName_LostFocus;
			txtTopGuideDiameter.TextChanged += txtTopGuideDiameter_TextChanged;
			txtTopGuideHeight.TextChanged += txtTopGuideHeight_TextChanged;

			this.CancelButton = btnCancel;
			this.AcceptButton = btnOK;



		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Configuration																													*
		//*-----------------------------------------------------------------------*
		private ShopToolsConfigItem mConfiguration = ConfigProfile;
		/// <summary>
		/// Get/Set a reference to the configuration profile to be used with this
		/// instance.
		/// </summary>
		public ShopToolsConfigItem Configuration
		{
			get { return mConfiguration; }
			set
			{
				mConfiguration = value;
				RefreshConfiguration();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SelectedTool																													*
		//*-----------------------------------------------------------------------*
		private UserToolItem mSelectedTool = null;
		/// <summary>
		/// Get/Set a reference to the currently selected user tool.
		/// </summary>
		public UserToolItem SelectedTool
		{
			get { return mSelectedTool; }
			set
			{
				mControlBusy = true;
				mSelectedTool = value;
				if(mSelectedTool != null)
				{
					grpProperties.Enabled = true;
					cmboToolType.SelectedItem = mSelectedTool.ToolType;
					txtAngle.Text =
						mSelectedTool.Properties.GetFirstValue(
							"UserAngle", "Angle");
					txtBottomGuideDiameter.Text =
						mSelectedTool.Properties.GetFirstValue(
							"UserBottomGuideDiameter", "BottomGuideDiameter");
					lblBottomGuideDiameterUnit.Text =
						GetAltValue(mSelectedTool.Properties.GetFirstValue(
							"BottomGuideDiameter"), txtBottomGuideDiameter.Text);
					txtBottomGuideHeight.Text =
						mSelectedTool.Properties.GetFirstValue(
							"UserBottomGuideHeight", "BottomGuideHeight");
					lblBottomGuideHeightUnit.Text =
						GetAltValue(mSelectedTool.Properties.GetFirstValue(
							"BottomGuideHeight"), txtBottomGuideHeight.Text);
					txtDiameter.Text =
						mSelectedTool.Properties.GetFirstValue(
							"UserDiameter", "Diameter");
					lblDiameterUnit.Text =
						GetAltValue(mSelectedTool.Properties.GetFirstValue(
							"Diameter"), txtDiameter.Text);
					txtFluteCount.Text =
						mSelectedTool.Properties.GetFirstValue(
							"UserFluteCount", "FluteCount");
					lblFluteCountUnit.Text = "";
					txtFluteLength.Text =
						mSelectedTool.Properties.GetFirstValue(
							"UserFluteLength", "FluteLength");
					lblFluteLengthUnit.Text =
						GetAltValue(mSelectedTool.Properties.GetFirstValue(
							"FluteLength"), txtFluteLength.Text);
					txtShaftLength.Text =
						mSelectedTool.Properties.GetFirstValue(
							"UserShaftLength", "ShaftLength");
					lblShaftLengthUnit.Text =
						GetAltValue(mSelectedTool.Properties.GetFirstValue(
							"ShaftLength"), txtShaftLength.Text);
					txtToolName.Text = mSelectedTool.ToolName;
					txtTopGuideDiameter.Text =
						mSelectedTool.Properties.GetFirstValue(
							"UserTopGuideDiameter", "TopGuideDiameter");
					lblTopGuideDiameterUnit.Text =
						GetAltValue(mSelectedTool.Properties.GetFirstValue(
							"TopGuideDiameter"), txtTopGuideDiameter.Text);
					txtTopGuideHeight.Text =
						mSelectedTool.Properties.GetFirstValue(
							"UserTopGuideHeight", "TopGuideHeight");
					lblTopGuideHeightUnit.Text =
						GetAltValue(mSelectedTool.Properties.GetFirstValue(
							"TopGuideHeight"), txtTopGuideHeight.Text);
				}
				else
				{
					grpProperties.Enabled = false;
				}
				UpdateAvailableProperties();
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SelectedToolName																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set the name of the selected user tool.
		/// </summary>
		public string SelectedToolName
		{
			get
			{
				string result = "";
				if(lstDefinedTools.SelectedIndex > -1)
				{
					result = ((UserToolItem)lstDefinedTools.SelectedItem).ToString();
				}
				return result;
			}
			set
			{
				int count = 0;
				int index = 0;

				if(value?.Length > 0)
				{
					count = lstDefinedTools.Items.Count;
					for(index = 0; index < count; index ++)
					{
						if(lstDefinedTools.Items[index].ToString() == value)
						{
							lstDefinedTools.SelectedIndex = index;
							break;
						}
					}
				}
				else
				{
					lstDefinedTools.SelectedIndex = -1;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WorkingUserTools																											*
		//*-----------------------------------------------------------------------*
		private List<UserToolItem> mWorkingUserTools = new List<UserToolItem>();
		/// <summary>
		/// Get a reference to the collection of user tools isolated for this
		/// instance.
		/// </summary>
		public List<UserToolItem> WorkingUserTools
		{
			get { return mWorkingUserTools; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
