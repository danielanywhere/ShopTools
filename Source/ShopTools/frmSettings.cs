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
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ConversionCalc;

using static ShopTools.ShopToolsUtil;

//	Panel maximum size: 345, 194
namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	frmSettings																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Settings dialog.
	/// </summary>
	public partial class frmSettings : Form
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		private bool mControlBusy = false;
		private const int mOriginDotSize = 16;

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
		//* btnEditTools_Click																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The Edit Tools button has been clicked.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void btnEditTools_Click(object sender, EventArgs e)
		{
			int count = 0;
			frmEditTools dialog = new frmEditTools();
			int index = 0;
			int selectedIndex = -1;
			string selectedText = "";
			string text = "";

			dialog.Configuration = mConfiguration;
			if(cmboGeneralCuttingTool.SelectedIndex > -1)
			{
				selectedText = cmboGeneralCuttingTool.SelectedItem.ToString();
			}
			dialog.SelectedToolName = selectedText;
			if(dialog.ShowDialog() == DialogResult.OK)
			{
				//	Rebuild the tool list.
				cmboGeneralCuttingTool.SelectedIndex = -1;
				cmboGeneralCuttingTool.Items.Clear();
				count = mConfiguration.UserTools.Count;
				for(index = 0; index < count; index ++)
				{
					text = mConfiguration.UserTools[index].ToolName;
					if(text.Length > 0)
					{
						cmboGeneralCuttingTool.Items.Add(text);
						if(text == selectedText)
						{
							selectedIndex = index;
						}
					}
				}
				if(selectedIndex > -1)
				{
					cmboGeneralCuttingTool.SelectedIndex = selectedIndex;
				}
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

			foreach(Control control in this.Controls)
			{
				control.Enabled = false;
			}
			//	Overwrite the current configuration.
			ConfigProfile = mConfiguration;
			this.DialogResult = DialogResult.OK;
			this.Hide();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* chkXOpenEnded_CheckedChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The checked state of the 'X is open ended' checkbox has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void chkXOpenEnded_CheckedChanged(object sender, EventArgs e)
		{
			if(!mControlBusy)
			{
				mControlBusy = true;
				mConfiguration.AxisXIsOpenEnded = chkXOpenEnded.Checked;
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* chkYOpenEnded_CheckedChanged																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The checked state of the 'Y is open ended' checkbox has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void chkYOpenEnded_CheckedChanged(object sender, EventArgs e)
		{
			if(!mControlBusy)
			{
				mControlBusy = true;
				mConfiguration.AxisYIsOpenEnded = chkYOpenEnded.Checked;
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* cmboGeneralCuttingTool_SelectedIndexChanged														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selected index has changed on the General Cutting Tool dropdown
		/// list.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void cmboGeneralCuttingTool_SelectedIndexChanged(object sender,
			EventArgs e)
		{
			string text = "";

			if(!mControlBusy)
			{
				text = (string)cmboGeneralCuttingTool.SelectedItem;
				if(text?.Length > 0)
				{
					mConfiguration.GeneralCuttingTool = text;
				}
				else
				{
					mConfiguration.GeneralCuttingTool = "";
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* cmboXYOrigin_SelectedIndexChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selected index has changed on the Workspace XY Origin dropdown
		/// list.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void cmboXYOrigin_SelectedIndexChanged(object sender, EventArgs e)
		{
			string text = "";

			if(!mControlBusy)
			{
				mControlBusy = true;
				text = (string)cmboXYOrigin.SelectedItem;
				if(text?.Length > 0)
				{
					mConfiguration.XYOrigin =
						Enum.Parse<OriginLocationEnum>(text.Replace(" ", ""), true);
				}
				else
				{
					mConfiguration.XYOrigin = OriginLocationEnum.None;
				}
				UpdateSummary();
				mControlBusy = false;
			}
			pnlCanvasArea.Refresh();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* cmboZOrigin_SelectedIndexChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The selected index has changed on the Workspace Z Origin dropdown
		/// list.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void cmboZOrigin_SelectedIndexChanged(object sender, EventArgs e)
		{
			string text = "";

			if(!mControlBusy)
			{
				mControlBusy = true;
				text = (string)cmboZOrigin.SelectedItem;
				if(text?.Length > 0)
				{
					mConfiguration.ZOrigin = Enum.Parse<OriginLocationEnum>(text, true);
				}
				else
				{
					mConfiguration.ZOrigin = OriginLocationEnum.None;
				}
				UpdateSummary();
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* optUS_CheckedChanged																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The checked state of the U.S. Units option button has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void optUS_CheckedChanged(object sender, EventArgs e)
		{
			if(!mControlBusy)
			{
				mControlBusy = true;
				mConfiguration.DisplayUnits = (optUS.Checked ?
					DisplayUnitEnum.UnitedStates : DisplayUnitEnum.Metric);
				mControlBusy = false;
				txtZDimension_TextChanged(this, e);
				txtYDimension_TextChanged(this, e);
				txtXDimension_TextChanged(this, e);
				mControlBusy = true;
				UpdateSummary();
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* optXPRight_CheckedChanged																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The checked state of the X+ Right option button has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void optXPRight_CheckedChanged(object sender, EventArgs e)
		{
			if(!mControlBusy)
			{
				mControlBusy = true;
				mConfiguration.TravelX =
					(optXPRight.Checked ?
					DirectionLeftRightEnum.Right : DirectionLeftRightEnum.Left);
				UpdateSummary();
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* optYPUp_CheckedChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The checked state of the Y+ Up option button has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void optYPUp_CheckedChanged(object sender, EventArgs e)
		{
			if(!mControlBusy)
			{
				mControlBusy = true;
				mConfiguration.TravelY =
					(optYPUp.Checked ?
					DirectionUpDownEnum.Up : DirectionUpDownEnum.Down);
				UpdateSummary();
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* optZPUp_CheckedChanged																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The checked state of the Z+ Up option button has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void optZPUp_CheckedChanged(object sender, EventArgs e)
		{
			if(!mControlBusy)
			{
				mControlBusy = true;
				mConfiguration.TravelZ =
					(optZPUp.Checked ?
					DirectionUpDownEnum.Up : DirectionUpDownEnum.Down);
				UpdateSummary();
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* pnlCanvasArea_Paint																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The canvas panel is being re-painted.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Paint event arguments.
		/// </param>
		private void pnlCanvasArea_Paint(object sender, PaintEventArgs e)
		{
			Brush background = new SolidBrush(ColorTranslator.FromHtml("#BFA58E"));
			Graphics graphics = e.Graphics;
			int cmboSelectedIndex =
				(cmboXYOrigin.SelectedIndex > -1 ? cmboXYOrigin.SelectedIndex : 0);

			string origin = (string)cmboXYOrigin.Items[cmboSelectedIndex];
			Rectangle originArea = new Rectangle()
			{
				Width = mOriginDotSize,
				Height = mOriginDotSize
			};
			Brush originBackColor = new SolidBrush(Color.White);
			Color originStrokeColor = Color.Black;
			Pen penOrigin = new Pen(originStrokeColor)
			{
				Width = 2
			};

			Debug.WriteLine("CanvasPanel.Paint");

			graphics.FillRectangle(background,
				new Rectangle(0, 0, pnlCanvasArea.Width, pnlCanvasArea.Height));
			graphics.DrawRectangle(penOrigin,
				new Rectangle(0, 0, pnlCanvasArea.Width, pnlCanvasArea.Height));

			switch(origin)
			{
				case "Bottom":
					originArea.X = (pnlCanvasArea.Width / 2) - (mOriginDotSize / 2);
					originArea.Y = pnlCanvasArea.Height - mOriginDotSize - 4;
					break;
				case "Bottom Left":
					originArea.X = 4;
					originArea.Y = pnlCanvasArea.Height - mOriginDotSize - 4;
					break;
				case "Bottom Right":
					originArea.X = pnlCanvasArea.Width - mOriginDotSize - 4;
					originArea.Y = pnlCanvasArea.Height - mOriginDotSize - 4;
					break;
				case "Center":
					originArea.X = (pnlCanvasArea.Width / 2) - (mOriginDotSize / 2);
					originArea.Y = (pnlCanvasArea.Height / 2) - (mOriginDotSize / 2);
					break;
				case "Left":
					originArea.X = 4;
					originArea.Y = (pnlCanvasArea.Height / 2) - (mOriginDotSize / 2);
					break;
				case "Right":
					originArea.X = pnlCanvasArea.Width - mOriginDotSize - 4;
					originArea.Y = (pnlCanvasArea.Height / 2) - (mOriginDotSize / 2);
					break;
				case "Top":
					originArea.X = (pnlCanvasArea.Width / 2) - (mOriginDotSize / 2);
					originArea.Y = 4;
					break;
				case "Top Left":
					originArea.X = 4;
					originArea.Y = 4;
					break;
				case "Top Right":
					originArea.X = pnlCanvasArea.Width - mOriginDotSize - 4;
					originArea.Y = 4;
					break;
			}

			graphics.SmoothingMode = SmoothingMode.AntiAlias;
			graphics.FillEllipse(originBackColor, originArea);
			graphics.DrawEllipse(penOrigin, originArea);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RefreshControls																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Referesh the settings of the controls based upon the current
		/// configuration.
		/// </summary>
		private void RefreshControls()
		{
			int count = 0;
			int index = 0;
			string text = "";

			if(mConfiguration != null && !mControlBusy)
			{
				mControlBusy = true;
				switch(mConfiguration.DisplayUnits)
				{
					case DisplayUnitEnum.UnitedStates:
						optUS.Checked = true;
						break;
					default:
						optMetric.Checked = true;
						break;
				}
				txtXDimension.Text = mConfiguration.UserXDimension;
				lblXDimensionUnit.Text =
					GetAltValue(mConfiguration.XDimension, txtXDimension.Text);
				txtYDimension.Text = mConfiguration.UserYDimension;
				lblYDimensionUnit.Text =
					GetAltValue(mConfiguration.YDimension, txtYDimension.Text);
				txtZDimension.Text = mConfiguration.UserDepth;
				lblZDimensionUnit.Text = GetAltValue(mConfiguration.Depth, txtZDimension.Text);
				cmboXYOrigin.SelectedIndex = -1;
				text = FromTitleCase(mConfiguration.XYOrigin.ToString());
				count = cmboXYOrigin.Items.Count;
				for(index = 0; index < count; index ++)
				{
					if((string)cmboXYOrigin.Items[index] == text)
					{
						cmboXYOrigin.SelectedIndex = index;
						break;
					}
				}
				cmboZOrigin.SelectedIndex = -1;
				text = FromTitleCase(mConfiguration.ZOrigin.ToString());
				count = cmboZOrigin.Items.Count;
				for(index = 0; index < count; index ++)
				{
					if((string)cmboZOrigin.Items[index] == text)
					{
						cmboZOrigin.SelectedIndex = index;
						break;
					}
				}
				cmboGeneralCuttingTool.SelectedIndex = -1;
				text = mConfiguration.GeneralCuttingTool;
				count = cmboGeneralCuttingTool.Items.Count;
				for(index = 0; index < count; index ++)
				{
					if((string)cmboGeneralCuttingTool.Items[index] == text)
					{
						cmboGeneralCuttingTool.SelectedIndex = index;
						break;
					}
				}
				switch(mConfiguration.TravelX)
				{
					case DirectionLeftRightEnum.Left:
						optXPLeft.Checked = true;
						break;
					case DirectionLeftRightEnum.Right:
						optXPRight.Checked = true;
						break;
				}
				switch(mConfiguration.TravelY)
				{
					case DirectionUpDownEnum.Down:
						optYPDown.Checked = true;
						break;
					case DirectionUpDownEnum.Up:
						optYPUp.Checked = true;
						break;
				}
				switch(mConfiguration.TravelZ)
				{
					case DirectionUpDownEnum.Down:
						optZPDown.Checked = true;
						break;
					case DirectionUpDownEnum.Up:
						optZPUp.Checked = true;
						break;
				}
				chkXOpenEnded.Checked = mConfiguration.AxisXIsOpenEnded;
				chkYOpenEnded.Checked = mConfiguration.AxisYIsOpenEnded;

				UpdateSummary();
				UpdatePanelLayout();
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtZDimension_TextChanged																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text in the Z dimension textbox has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtZDimension_TextChanged(object sender, EventArgs e)
		{
			//MeasurementCollection measurements = null;
			string text = "";

			if(!mControlBusy)
			{
				mControlBusy = true;
				//measurements = GetMeasurements(txtDepth.Text,
				//	(IsMetric ? "mm" : "in"));
				//if(measurements.Count > 0)
				//{
				//	mSystemDepth = measurements.SumMillimeters();
				//	mUserDepth = txtDepth.Text;
				//}
				mConfiguration.UserDepth = txtZDimension.Text;
				text = GetMeasurementString(txtZDimension.Text,
					BaseUnit(mConfiguration.DisplayUnits));
				mConfiguration.Depth = text;
				lblZDimensionUnit.Text = GetAltValue(text, txtZDimension.Text);
				UpdateSummary();
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtYDimension_TextChanged																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text in the Y dimension textbox has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtYDimension_TextChanged(object sender, EventArgs e)
		{
			//MeasurementCollection measurements = null;
			string text = "";

			if(!mControlBusy)
			{
				mControlBusy = true;
				//measurements = GetMeasurements(txtHeight.Text,
				//	(IsMetric ? "mm" : "in"));
				//if(measurements.Count > 0)
				//{
				//	mSystemHeight = measurements.SumMillimeters();
				//	mUserYDimension = txtHeight.Text;
				//}
				mConfiguration.UserYDimension = txtYDimension.Text;
				text = GetMeasurementString(txtYDimension.Text,
					BaseUnit(mConfiguration.DisplayUnits));
				mConfiguration.YDimension = text;
				lblYDimensionUnit.Text = GetAltValue(text, txtYDimension.Text);
				UpdateSummary();
				UpdatePanelLayout();
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* txtXDimension_TextChanged																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The text in the X dimension textbox has changed.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Standard event arguments.
		/// </param>
		private void txtXDimension_TextChanged(object sender, EventArgs e)
		{
			//MeasurementCollection measurements = null;
			string text = "";

			if(!mControlBusy)
			{
				mControlBusy = true;
				//measurements = GetMeasurements(txtWidth.Text,
				//	(IsMetric ? "mm" : "in"));
				//if(measurements.Count > 0)
				//{
				//	mSystemWidth = measurements.SumMillimeters();
				//	mUserWidth = txtWidth.Text;
				//}
				mConfiguration.UserXDimension = txtXDimension.Text;
				text = GetMeasurementString(txtXDimension.Text,
					BaseUnit(mConfiguration.DisplayUnits));
				mConfiguration.XDimension = text;
				lblXDimensionUnit.Text = GetAltValue(text, txtXDimension.Text);
				UpdateSummary();
				UpdatePanelLayout();
				mControlBusy = false;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdatePanelLayout																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the layout of the visual canvas panel, given the current width
		/// and height settings.
		/// </summary>
		private void UpdatePanelLayout()
		{
			//	Maximum width: 345
			//	Maximum height: 194
			float adjust = 0f;
			float height = (float)pnlCanvasArea.Height;
			float maxHeight = 194f;
			float maxWidth = 345f;
			MeasurementCollection measurements = null;
			float ratio = 0f;
			float systemHeight = 0f;
			float systemWidth = 0f;
			float width = (float)pnlCanvasArea.Width;

			measurements =
				MeasurementCollection.GetMeasurements(mConfiguration.XDimension,
					BaseUnit(mConfiguration.DisplayUnits));
			systemWidth = measurements.SumMillimeters();
			measurements =
				MeasurementCollection.GetMeasurements(mConfiguration.YDimension,
					BaseUnit(mConfiguration.DisplayUnits));
			systemHeight = measurements.SumMillimeters();

			if(systemHeight != 0f)
			{
				ratio = systemWidth / systemHeight;
				if(ratio >= 1f)
				{
					//	Width is greater than or equal to height (landscape).
					//	Start with maximum width.
					width = maxWidth;
					//	Natural height.
					height = width / ratio;
					//	Adjust to fit within boundaries.
					if(height > maxHeight)
					{
						adjust = maxHeight / height;
						width *= adjust;
						height *= adjust;
					}
				}
				else
				{
					//	Width is less than height (portrait).
					//	Start with maximum height.
					height = maxHeight;
					//	Natural width.
					width = height * ratio;
				}
				pnlCanvasArea.Size = new Size((int)width, (int)height);
				pnlCanvasArea.Refresh();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdateSummary																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the contents of the summary box.
		/// </summary>
		private void UpdateSummary()
		{
			StringBuilder builder = new StringBuilder();
			MeasurementCollection measurements = null;
			float systemDepth = 0f;
			float systemHeight = 0f;
			float systemWidth = 0f;
			string text = "";

			measurements =
				MeasurementCollection.GetMeasurements(mConfiguration.XDimension,
					BaseUnit(mConfiguration.DisplayUnits));
			systemWidth = measurements.SumMillimeters();
			measurements =
				MeasurementCollection.GetMeasurements(mConfiguration.YDimension,
					BaseUnit(mConfiguration.DisplayUnits));
			systemHeight = measurements.SumMillimeters();
			measurements =
				MeasurementCollection.GetMeasurements(mConfiguration.Depth,
					BaseUnit(mConfiguration.DisplayUnits));
			systemDepth = measurements.SumMillimeters();

			//	Display units.
			builder.Append("Display: ");
			switch(mConfiguration.DisplayUnits)
			{
				case DisplayUnitEnum.Metric:
					builder.Append("metric");
					break;
				case DisplayUnitEnum.UnitedStates:
					builder.Append("U.S.");
					break;
				default:
					builder.Append("unknown");
					break;
			}
			builder.AppendLine(" units;");
			//	Work area.
			builder.Append("Work area: ");
			switch(mConfiguration.DisplayUnits)
			{
				case DisplayUnitEnum.Metric:
					builder.AppendLine(
						$"{systemWidth} mm x {systemHeight} mm x {systemDepth} mm;");
					break;
				case DisplayUnitEnum.UnitedStates:
					//	X
					text = SessionConverter.Convert(systemWidth,
						"mm", "in").ToString("##0.###");
					builder.Append($"{text} in x ");
					//	Y
					text = SessionConverter.Convert(systemHeight,
						"mm", "in").ToString("##0.###");
					builder.Append($"{text} in x ");
					//	Z
					text = SessionConverter.Convert(systemDepth,
						"mm", "in").ToString("##0.###");
					builder.AppendLine($"{text} in;");
					break;
				default:
					builder.AppendLine("unknown;");
					break;
			}
			//	Origin.
			builder.Append("Origin: ");
			switch(mConfiguration.XYOrigin)
			{
				case OriginLocationEnum.Bottom:
					builder.Append("X-Middle, ");
					switch(mConfiguration.TravelY)
					{
						case DirectionUpDownEnum.Down:
							builder.Append("Y-Max, ");
							break;
						case DirectionUpDownEnum.Up:
						default:
							builder.Append("Y-Min, ");
							break;
					}
					break;
				case OriginLocationEnum.BottomLeft:
					switch(mConfiguration.TravelX)
					{
						case DirectionLeftRightEnum.Left:
							builder.Append("X-Max, ");
							break;
						case DirectionLeftRightEnum.Right:
						default:
							builder.Append("X-Min, ");
							break;
					}
					switch(mConfiguration.TravelY)
					{
						case DirectionUpDownEnum.Down:
							builder.Append("Y-Max, ");
							break;
						case DirectionUpDownEnum.Up:
						default:
							builder.Append("Y-Min, ");
							break;
					}
					break;
				case OriginLocationEnum.BottomRight:
					switch(mConfiguration.TravelX)
					{
						case DirectionLeftRightEnum.Left:
							builder.Append("X-Min, ");
							break;
						case DirectionLeftRightEnum.Right:
						default:
							builder.Append("X-Max, ");
							break;
					}
					switch(mConfiguration.TravelY)
					{
						case DirectionUpDownEnum.Down:
							builder.Append("Y-Max, ");
							break;
						case DirectionUpDownEnum.Up:
						default:
							builder.Append("Y-Min, ");
							break;
					}
					break;
				case OriginLocationEnum.Center:
					builder.Append("X-Middle, ");
					builder.Append("Y-Middle, ");
					break;
				case OriginLocationEnum.Left:
					switch(mConfiguration.TravelX)
					{
						case DirectionLeftRightEnum.Left:
							builder.Append("X-Max, ");
							break;
						case DirectionLeftRightEnum.Right:
						default:
							builder.Append("X-Min, ");
							break;
					}
					builder.Append("Y-Middle, ");
					break;
				case OriginLocationEnum.Right:
					switch(mConfiguration.TravelX)
					{
						case DirectionLeftRightEnum.Left:
							builder.Append("X-Min, ");
							break;
						case DirectionLeftRightEnum.Right:
						default:
							builder.Append("X-Max, ");
							break;
					}
					builder.Append("Y-Middle, ");
					break;
				case OriginLocationEnum.Top:
					builder.Append("X-Middle, ");
					switch(mConfiguration.TravelY)
					{
						case DirectionUpDownEnum.Down:
							builder.Append("Y-Min, ");
							break;
						case DirectionUpDownEnum.Up:
						default:
							builder.Append("Y-Max, ");
							break;
					}
					break;
				case OriginLocationEnum.TopLeft:
					switch(mConfiguration.TravelX)
					{
						case DirectionLeftRightEnum.Left:
							builder.Append("X-Max, ");
							break;
						case DirectionLeftRightEnum.Right:
						default:
							builder.Append("X-Min, ");
							break;
					}
					switch(mConfiguration.TravelY)
					{
						case DirectionUpDownEnum.Down:
							builder.Append("Y-Min, ");
							break;
						case DirectionUpDownEnum.Up:
						default:
							builder.Append("Y-Max, ");
							break;
					}
					break;
				case OriginLocationEnum.TopRight:
					switch(mConfiguration.TravelX)
					{
						case DirectionLeftRightEnum.Left:
							builder.Append("X-Min, ");
							break;
						case DirectionLeftRightEnum.Right:
						default:
							builder.Append("X-Max, ");
							break;
					}
					switch(mConfiguration.TravelY)
					{
						case DirectionUpDownEnum.Down:
							builder.Append("Y-Min, ");
							break;
						case DirectionUpDownEnum.Up:
						default:
							builder.Append("Y-Max, ");
							break;
					}
					break;
			}
			switch(mConfiguration.ZOrigin)
			{
				case OriginLocationEnum.Bottom:
					switch(mConfiguration.TravelZ)
					{
						case DirectionUpDownEnum.Down:
							builder.Append("Z-Max");
							break;
						case DirectionUpDownEnum.Up:
						default:
							builder.Append("Z-Min");
							break;
					}
					break;
				case OriginLocationEnum.Center:
					builder.Append("Z-Middle");
					break;
				case OriginLocationEnum.Top:
					switch(mConfiguration.TravelZ)
					{
						case DirectionUpDownEnum.Down:
							builder.Append("Z-Min");
							break;
						case DirectionUpDownEnum.Up:
						default:
							builder.Append("Z-Max");
							break;
					}
					break;
				default:
					builder.Append("Z-?");
					break;
			}
			txtSystemSummary.Text = builder.ToString();
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
		/// Create a new instance of the frmSettings Item.
		/// </summary>
		public frmSettings()
		{
			EventArgs e = new EventArgs();

			InitializeComponent();

			cmboGeneralCuttingTool.Items.Clear();
			foreach(UserToolItem toolItem in ConfigProfile.UserTools)
			{
				cmboGeneralCuttingTool.Items.Add(toolItem.ToolName);
			}
			cmboGeneralCuttingTool.SelectedIndex = 0;
			cmboXYOrigin.SelectedIndex = 0;
			cmboZOrigin.SelectedIndex = 1;

			this.CancelButton = btnCancel;
			this.AcceptButton = btnOK;

			optUS.CheckedChanged += optUS_CheckedChanged;
			txtXDimension.TextChanged += txtXDimension_TextChanged;
			txtYDimension.TextChanged += txtYDimension_TextChanged;
			txtZDimension.TextChanged += txtZDimension_TextChanged;
			cmboXYOrigin.SelectedIndexChanged += cmboXYOrigin_SelectedIndexChanged;
			cmboZOrigin.SelectedIndexChanged += cmboZOrigin_SelectedIndexChanged;
			cmboGeneralCuttingTool.SelectedIndexChanged +=
				cmboGeneralCuttingTool_SelectedIndexChanged;
			optXPRight.CheckedChanged += optXPRight_CheckedChanged;
			optYPUp.CheckedChanged += optYPUp_CheckedChanged;
			optZPUp.CheckedChanged += optZPUp_CheckedChanged;
			chkXOpenEnded.CheckedChanged += chkXOpenEnded_CheckedChanged;
			chkYOpenEnded.CheckedChanged += chkYOpenEnded_CheckedChanged;
			btnOK.Click += btnOK_Click;
			btnCancel.Click += btnCancel_Click;
			btnEditTools.Click += btnEditTools_Click;

			pnlCanvasArea.Paint += pnlCanvasArea_Paint;

			//txtZDimension_TextChanged(this, e);
			//txtYDimension_TextChanged(this, e);
			//txtXDimension_TextChanged(this, e);
			RefreshControls();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Configuration																													*
		//*-----------------------------------------------------------------------*
		private ShopToolsConfigItem mConfiguration =
			ShopToolsConfigItem.Clone(ConfigProfile);
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
				RefreshControls();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IsMetric																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set a value indicating whether the current unit mode is metric.
		/// </summary>
		public bool IsMetric
		{
			get { return optMetric.Checked; }
			set
			{
				if(value)
				{
					optMetric.Checked = true;
				}
				else
				{
					optUS.Checked = true;
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
