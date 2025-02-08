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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Geometry;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	PatternOperationCollection																							*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of PatternOperationItem Items.
	/// </summary>
	public class PatternOperationCollection : List<PatternOperationItem>
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************

		//*-----------------------------------------------------------------------*
		//* Clone																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a deep clone of the provided pattern operation collection.
		/// </summary>
		/// <param name="items">
		/// Reference to the collection of items to clone.
		/// </param>
		/// <returns>
		/// Reference to the newly cloned collection of pattern operation items.
		/// </returns>
		public static PatternOperationCollection Clone(
			PatternOperationCollection items)
		{
			PatternOperationCollection result = new PatternOperationCollection();

			if(items?.Count > 0)
			{
				foreach(PatternOperationItem operationItem in items)
				{
					result.Add(PatternOperationItem.Clone(operationItem));
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	PatternOperationItem																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about an individual operation definition on a pattern.
	/// </summary>
	public class PatternOperationItem
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		////*-----------------------------------------------------------------------*
		////* GetHorizontalPosition																									*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return the effective horizontal offset from the left side of the
		///// workpiece, given the offset and offset type.
		///// </summary>
		///// <param name="offsetType">
		///// The offset type by which the relative position will be calculated.
		///// </param>
		///// <param name="offsetX">
		///// The raw offset value to apply to the given offset type.
		///// </param>
		///// <param name="materialWidth">
		///// The width of the material.
		///// </param>
		///// <param name="previousX">
		///// The previously known X position.
		///// </param>
		///// <returns>
		///// The total X position, relative to the material.
		///// </returns>
		//private static float GetHorizontalOffset(OffsetLeftRightEnum offsetType,
		//	float offsetX, float materialWidth, float previousX)
		//{
		//	float position = offsetX;

		//	switch(offsetType)
		//	{
		//		case OffsetLeftRightEnum.Center:
		//			position += (materialWidth / 2f);
		//			break;
		//		case OffsetLeftRightEnum.Left:
		//			//	Left or right sides of material.
		//			if(ConfigProfile.TravelX == DirectionLeftRightEnum.Left)
		//			{
		//				//	RTL.
		//				position = materialWidth - position;
		//			}
		//			break;
		//		case OffsetLeftRightEnum.None:
		//		case OffsetLeftRightEnum.Relative:
		//			position += previousX;
		//			break;
		//		case OffsetLeftRightEnum.Right:
		//			//	Right or left sides of material.
		//			if(ConfigProfile.TravelX == DirectionLeftRightEnum.Right)
		//			{
		//				//	LTR.
		//				position = materialWidth - position;
		//			}
		//			break;
		//	}
		//	return position;
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* GetVerticalPosition																										*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return the effective vertical offset from the top side of the
		///// workpiece, given the offset and offset type.
		///// </summary>
		///// <param name="offsetType">
		///// The offset type by which the relative position will be calculated.
		///// </param>
		///// <param name="offsetY">
		///// The raw offset value to apply to the given offset type.
		///// </param>
		///// <param name="materialHeight">
		///// The height of the material.
		///// </param>
		///// <param name="previousY">
		///// The previously known Y position.
		///// </param>
		///// <returns>
		///// The total Y position, relative to the material.
		///// </returns>
		//private static float GetVerticalOffset(OffsetTopBottomEnum offsetType,
		//	float offsetY, float materialHeight, float previousY)
		//{
		//	float position = offsetY;

		//	switch(offsetType)
		//	{
		//		case OffsetTopBottomEnum.Bottom:
		//			//	Bottom or top sides of material.
		//			if(ConfigProfile.TravelY == DirectionUpDownEnum.Down)
		//			{
		//				//	BTT.
		//				position = materialHeight - position;
		//			}
		//			break;
		//		case OffsetTopBottomEnum.Center:
		//			position += (materialHeight / 2f);
		//			break;
		//		case OffsetTopBottomEnum.None:
		//		case OffsetTopBottomEnum.Relative:
		//			position += previousY;
		//			break;
		//		case OffsetTopBottomEnum.Top:
		//			//	Top or bottom sides of material.
		//			if(ConfigProfile.TravelY == DirectionUpDownEnum.Up)
		//			{
		//				//	TTB.
		//				position = materialHeight - position;
		//			}
		//			break;
		//	}
		//	return position;
		//}
		////*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	Action																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Action">Action</see>.
		/// </summary>
		private OperationActionEnum mAction = OperationActionEnum.None;
		/// <summary>
		/// Get/Set the action to execute on this operation.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(Order = 2)]
		public OperationActionEnum Action
		{
			get { return mAction; }
			set { mAction = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Angle																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Angle">Angle</see>.
		/// </summary>
		private string mAngle = "";
		/// <summary>
		/// Get/Set the angle at which the operation will take place.
		/// </summary>
		[JsonProperty(Order = 15)]
		public string Angle
		{
			get { return mAngle; }
			set { mAngle = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clone																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a deep clone of the provided pattern operation item.
		/// </summary>
		/// <param name="item">
		/// Reference to the item to clone.
		/// </param>
		/// <returns>
		/// Reference to the newly cloned pattern operation item.
		/// </returns>
		public static PatternOperationItem Clone(
			PatternOperationItem item)
		{
			PatternOperationItem result = null;

			if(item != null)
			{
				result = new PatternOperationItem()
				{
					mAction = item.mAction,
					mAngle = item.mAngle,
					mDepth = item.mDepth,
					mEndOffsetX = item.mEndOffsetX,
					mEndOffsetXOrigin = item.mEndOffsetXOrigin,
					mEndOffsetY = item.mEndOffsetY,
					mEndOffsetYOrigin = item.mEndOffsetYOrigin,
					mKerf = item.mKerf,
					mLength = item.mLength,
					mOffsetX = item.mOffsetX,
					mOffsetXOrigin = item.mOffsetXOrigin,
					mOffsetY = item.mOffsetY,
					mOffsetYOrigin = item.mOffsetYOrigin,
					mOperationName = item.mOperationName,
					mStartOffsetX = item.mStartOffsetX,
					mStartOffsetXOrigin = item.mStartOffsetXOrigin,
					mStartOffsetY = item.mStartOffsetY,
					mStartOffsetYOrigin = item.mStartOffsetYOrigin,
					mTool = item.mTool,
					mWidth = item.mWidth
				};
				foreach(string entryItem in item.mHiddenVariables)
				{
					result.mHiddenVariables.Add(entryItem);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Depth																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Depth">Depth</see>.
		/// </summary>
		private string mDepth = "";
		/// <summary>
		/// Get/Set the depth of the stroke to make.
		/// </summary>
		/// <remarks>
		/// If depth is defined in this operation and the property is blank, then
		/// the depth of the material is assumed.
		/// </remarks>
		[JsonProperty(Order = 18)]
		public string Depth
		{
			get { return mDepth; }
			set { mDepth = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	EndOffsetX																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="EndOffsetX">EndOffsetX</see>.
		/// </summary>
		private string mEndOffsetX = "";
		/// <summary>
		/// Get/Set the distance along the X axis into the material body at which
		/// to stop.
		/// </summary>
		/// <remarks>
		/// If neither this value nor EndOffsetXOrigin are supplied, no movement
		/// will take place. If EndOffsetX is supplied and EndOffsetXOrigin is
		/// Relative, the ending X offset will be relative to the starting X
		/// position.
		/// </remarks>
		[JsonProperty(Order = 11)]
		public string EndOffsetX
		{
			get { return mEndOffsetX; }
			set { mEndOffsetX = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	EndOffsetXOrigin																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="EndOffsetXOrigin">EndOffsetXOrigin</see>.
		/// </summary>
		private OffsetLeftRightEnum mEndOffsetXOrigin =
			OffsetLeftRightEnum.None;
		/// <summary>
		/// Get/Set the origin of the offset for the ending X position.
		/// </summary>
		/// <remarks>
		/// If EndOffsetX is 0 and EndOffsetXOrigin is Left or Right, then the
		/// stroke ends at the left or right sides, respectively.
		/// </remarks>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(Order = 12)]
		public OffsetLeftRightEnum EndOffsetXOrigin
		{
			get { return mEndOffsetXOrigin; }
			set { mEndOffsetXOrigin = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	EndOffsetY																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="EndOffsetY">EndOffsetY</see>.
		/// </summary>
		private string mEndOffsetY = "";
		/// <summary>
		/// Get/Set the distance along the Y axis into the material body at which
		/// to stop.
		/// </summary>
		/// <remarks>
		/// If neither this value nor EndOffsetYOrigin are supplied, no movement
		/// will take place. If EndOffsetY is supplied and EndOffsetYOrigin is
		/// Relative, the ending Y offset will be relative to the starting Y
		/// position.
		/// </remarks>
		[JsonProperty(Order = 13)]
		public string EndOffsetY
		{
			get { return mEndOffsetY; }
			set { mEndOffsetY = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	EndOffsetYOrigin																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="EndOffsetYOrigin">EndOffsetYOrigin</see>.
		/// </summary>
		private OffsetTopBottomEnum mEndOffsetYOrigin = OffsetTopBottomEnum.None;
		/// <summary>
		/// Get/Set the origin of the offset for the ending Y position.
		/// </summary>
		/// <remarks>
		/// If EndOffsetY is 0 and EndOffsetYOrigin is Top or Bottom, then the
		/// stroke ends at the top or bottom sides, respectively.
		/// </remarks>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(Order = 14)]
		public OffsetTopBottomEnum EndOffsetYOrigin
		{
			get { return mEndOffsetYOrigin; }
			set { mEndOffsetYOrigin = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetResultingLocation																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Calculate and return the resulting system location for the supplied
		/// user values on the provided operation.
		/// </summary>
		/// <param name="operation">
		/// The operation for which user values have been provided.
		/// </param>
		/// <param name="workpiece">
		/// Reference to information about the workpiece.
		/// </param>
		/// <param name="startingLocation">
		/// Reference to the starting location from which the operation will
		/// begin, in system units
		/// </param>
		/// <param name="previousToolName">
		/// Name of the previous tool.
		/// </param>
		/// <returns>
		/// Reference to the location at which the current operation will end,
		/// in system units.
		/// </returns>
		public static FPoint GetResultingLocation(
			PatternOperationItem operation,
			WorkpieceInfoItem workpiece, FPoint startingLocation,
			string previousToolName)
		{
			FPoint location = null;

			if(operation != null && workpiece != null)
			{
				location = FPoint.Clone(startingLocation);
				location = GetOperationStartLocation(operation, workpiece, location);
				location = GetOperationEndLocation(operation, workpiece, location);
			}
			if(location == null)
			{
				location = new FPoint();
			}
			return location;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetRows																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return all settings rows in a table that have a matching pattern
		/// operation ID.
		/// </summary>
		/// <param name="operation">
		/// The operation whose OperationId property will be used to locate the
		/// results.
		/// </param>
		/// <param name="table">
		/// Reference to the data table containing the rows to retrieve.
		/// </param>
		/// <param name="keyColumn">
		/// Name of the key column in which to find the ID for the row.
		/// </param>
		/// <returns>
		/// Array of data rows matching the specified pattern operation ID.
		/// </returns>
		public static DataRow[] GetRows(PatternOperationItem operation,
			DataTable table, string keyColumn)
		{
			DataRow[] result = new DataRow[0];
			EnumerableRowCollection<DataRow> rows = null;

			if(operation != null && table?.Rows.Count > 0 && keyColumn?.Length > 0)
			{
				rows = from row in table.AsEnumerable()
								where row.Field<string>(keyColumn) == operation.mOperationId
									select row;
				if(rows.Count() > 0)
				{
					result = rows.ToArray();
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetValue																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value associated with the specified property name of the
		/// provided pattern operation.
		/// </summary>
		/// <param name="operation">
		/// Reference to the pattern operation item for which the property value
		/// will be retrieved.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to retrieve.
		/// </param>
		/// <returns>
		/// Value of the specified property on the provided pattern operation
		/// item, if found. Otherwise, an empty string.
		/// </returns>
		public static string GetValue(PatternOperationItem operation,
			string propertyName)
		{
			string result = "";
			object item = null;

			if(operation != null && propertyName?.Length > 0)
			{
				try
				{
					item = operation.GetType().GetProperty(propertyName,
						System.Reflection.BindingFlags.GetProperty |
						System.Reflection.BindingFlags.IgnoreCase |
						System.Reflection.BindingFlags.Instance |
						System.Reflection.BindingFlags.Public).
							GetValue(operation);
					if(item != null)
					{
						result = item.ToString();
					}
				}
				catch { }
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* HasStartOffset																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the provided operation has an
		/// explicitly stated starting offset.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation to inspect.
		/// </param>
		/// <returns>
		/// True if the provided operation has an explicitly specified starting
		/// offset for the type of action present. Otherwise, false, and the
		/// start offset is implicit.
		/// </returns>
		public static bool HasStartOffset(PatternOperationItem operation)
		{
			bool result = false;

			if(operation != null)
			{
				switch(operation.mAction)
				{
					case OperationActionEnum.DrawCircleCenterDiameter:
					case OperationActionEnum.DrawCircleCenterRadius:
					case OperationActionEnum.DrawCircleDiameter:
					case OperationActionEnum.DrawCircleRadius:
					case OperationActionEnum.DrawEllipseCenterDiameterXY:
					case OperationActionEnum.DrawEllipseCenterRadiusXY:
					case OperationActionEnum.DrawEllipseDiameterXY:
					case OperationActionEnum.DrawEllipseLengthWidth:
					case OperationActionEnum.DrawEllipseRadiusXY:
					case OperationActionEnum.DrawLineAngleLength:
					case OperationActionEnum.DrawLineLengthWidth:
					case OperationActionEnum.DrawRectangleLengthWidth:
					case OperationActionEnum.FillCircleCenterDiameter:
					case OperationActionEnum.FillCircleCenterRadius:
					case OperationActionEnum.FillCircleDiameter:
					case OperationActionEnum.FillCircleRadius:
					case OperationActionEnum.FillEllipseCenterDiameterXY:
					case OperationActionEnum.FillEllipseCenterRadiusXY:
					case OperationActionEnum.FillEllipseDiameterXY:
					case OperationActionEnum.FillEllipseLengthWidth:
					case OperationActionEnum.FillEllipseRadiusXY:
					case OperationActionEnum.FillRectangleLengthWidth:
					case OperationActionEnum.PointXY:
						//	Starting OffsetX, OffsetY.
						if(operation.OffsetX.Length > 0 ||
							operation.OffsetXOrigin != OffsetLeftRightEnum.None ||
							operation.OffsetY.Length > 0 ||
							operation.OffsetYOrigin != OffsetTopBottomEnum.None)
						{
							result = true;
						}
						break;
					case OperationActionEnum.DrawEllipseXY:
					case OperationActionEnum.DrawLineXY:
					case OperationActionEnum.DrawRectangleXY:
					case OperationActionEnum.FillEllipseXY:
					case OperationActionEnum.FillRectangleXY:
						//	StartOffsetX, StartOffsetY.
						if(operation.StartOffsetX.Length > 0 ||
							operation.StartOffsetXOrigin != OffsetLeftRightEnum.None ||
							operation.StartOffsetY.Length > 0 ||
							operation.StartOffsetYOrigin != OffsetTopBottomEnum.None)
						{
							result = true;
						}
						break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	HiddenVariables																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="HiddenVariables">HiddenVariables</see>.
		/// </summary>
		private List<string> mHiddenVariables = new List<string>();
		/// <summary>
		/// Get a reference to a list of variable names hidden from user input.
		/// </summary>
		[JsonProperty(Order = 21)]
		public List<string> HiddenVariables
		{
			get { return mHiddenVariables; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Kerf																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Kerf">Kerf</see>.
		/// </summary>
		private DirectionLeftRightEnum mKerf = DirectionLeftRightEnum.None;
		/// <summary>
		/// Get/Set the side to which the kerf will accumulate along the path of
		/// travel. If None or Center, half of the Kerf will accumulate on each
		/// side.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(Order = 19)]
		public DirectionLeftRightEnum Kerf
		{
			get { return mKerf; }
			set { mKerf = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Length																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Length">Length</see>.
		/// </summary>
		private string mLength = "";
		/// <summary>
		/// Get/Set the length of the operation.
		/// </summary>
		[JsonProperty(Order = 16)]
		public string Length
		{
			get { return mLength; }
			set { mLength = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	OffsetX																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="OffsetX">OffsetX</see>.
		/// </summary>
		private string mOffsetX = "";
		/// <summary>
		/// Get/Set the distance along the X axis into the material body.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only used in cases where a single starting point is needed.
		/// </para>
		/// <para>
		/// If neither this value nor OffsetXOrigin are supplied, no movement
		/// will take place. If OffsetX is supplied and OffsetXOrigin is
		/// Relative, the X offset will be relative to the previous router
		/// position.
		/// </para>
		/// </remarks>
		[JsonProperty(Order = 7)]
		public string OffsetX
		{
			get { return mOffsetX; }
			set { mOffsetX = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	OffsetXOrigin																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="OffsetXOrigin">OffsetXOrigin</see>.
		/// </summary>
		private OffsetLeftRightEnum mOffsetXOrigin =
			OffsetLeftRightEnum.None;
		/// <summary>
		/// Get/Set the reference edge or corner from which to measure the X
		/// starting point.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(Order = 8)]
		public OffsetLeftRightEnum OffsetXOrigin
		{
			get { return mOffsetXOrigin; }
			set { mOffsetXOrigin = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	OffsetY																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="OffsetY">OffsetY</see>.
		/// </summary>
		private string mOffsetY = "";
		/// <summary>
		/// Get/Set the distance along the Y axis into the material body.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Only used in cases where a single starting point is needed.
		/// </para>
		/// <para>
		/// If neither this value nor OffsetYOrigin are supplied, no movement
		/// will take place. If OffsetY is supplied and OffsetYOrigin is
		/// Relative, the Y offset will be relative to the previous router
		/// position.
		/// </para>
		/// </remarks>
		[JsonProperty(Order = 9)]
		public string OffsetY
		{
			get { return mOffsetY; }
			set { mOffsetY = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	OffsetYOrigin																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="OffsetYOrigin">OffsetYOrigin</see>.
		/// </summary>
		private OffsetTopBottomEnum mOffsetYOrigin = OffsetTopBottomEnum.None;
		/// <summary>
		/// Get/Set the reference edge or corner from which to measure the Y
		/// starting point.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(Order = 10)]
		public OffsetTopBottomEnum OffsetYOrigin
		{
			get { return mOffsetYOrigin; }
			set { mOffsetYOrigin = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	OperationId																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="OperationId">OperationId</see>.
		/// </summary>
		private string mOperationId = Guid.NewGuid().ToString("D").ToLower();
		/// <summary>
		/// Get/Set the globally unique ID of this pattern operation.
		/// </summary>
		[JsonProperty(Order = 0)]
		public string OperationId
		{
			get { return mOperationId; }
			set { mOperationId = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	OperationName																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="OperationName">OperationName</see>.
		/// </summary>
		private string mOperationName = "";
		/// <summary>
		/// Get/Set the optional name to assign to the current operation in order
		/// to separate variable values among multiple operations.
		/// </summary>
		/// <remarks>
		/// This name can be repeated on multiple operations in the same pattern
		/// to establish repeated values.
		/// </remarks>
		[JsonProperty(Order = 1)]
		public string OperationName
		{
			get { return mOperationName; }
			set { mOperationName = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetValue																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the value associated with the specified property name of the
		/// provided pattern operation.
		/// </summary>
		/// <param name="operation">
		/// Reference to the pattern operation item for which the property value
		/// will be set.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to update.
		/// </param>
		/// <param name="value">
		/// Value to place in the specified property.
		/// </param>
		public static void SetValue(PatternOperationItem operation,
			string propertyName, DirectionLeftRightEnum value)
		{
			PropertyInfo property = null;

			if(operation != null && propertyName?.Length > 0)
			{
				property = operation.GetType().GetProperty(propertyName,
					System.Reflection.BindingFlags.SetProperty |
					System.Reflection.BindingFlags.IgnoreCase |
					System.Reflection.BindingFlags.Instance |
					System.Reflection.BindingFlags.Public);
				if(property != null &&
					property.PropertyType == typeof(DirectionLeftRightEnum))
				{
					try
					{
						property.SetValue(operation, value);
					}
					catch { }
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Set the value associated with the specified property name of the
		/// provided pattern operation.
		/// </summary>
		/// <param name="operation">
		/// Reference to the pattern operation item for which the property value
		/// will be set.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to update.
		/// </param>
		/// <param name="value">
		/// Value to place in the specified property.
		/// </param>
		public static void SetValue(PatternOperationItem operation,
			string propertyName, OffsetLeftRightEnum value)
		{
			PropertyInfo property = null;

			if(operation != null && propertyName?.Length > 0)
			{
				property = operation.GetType().GetProperty(propertyName,
					System.Reflection.BindingFlags.SetProperty |
					System.Reflection.BindingFlags.IgnoreCase |
					System.Reflection.BindingFlags.Instance |
					System.Reflection.BindingFlags.Public);
				if(property != null &&
					property.PropertyType == typeof(OffsetLeftRightEnum))
				{
					try
					{
						property.SetValue(operation, value);
					}
					catch { }
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Set the value associated with the specified property name of the
		/// provided pattern operation.
		/// </summary>
		/// <param name="operation">
		/// Reference to the pattern operation item for which the property value
		/// will be set.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to update.
		/// </param>
		/// <param name="value">
		/// Value to place in the specified property.
		/// </param>
		public static void SetValue(PatternOperationItem operation,
			string propertyName, OffsetTopBottomEnum value)
		{
			PropertyInfo property = null;

			if(operation != null && propertyName?.Length > 0)
			{
				property = operation.GetType().GetProperty(propertyName,
					System.Reflection.BindingFlags.SetProperty |
					System.Reflection.BindingFlags.IgnoreCase |
					System.Reflection.BindingFlags.Instance |
					System.Reflection.BindingFlags.Public);
				if(property != null &&
					property.PropertyType == typeof(OffsetTopBottomEnum))
				{
					try
					{
						property.SetValue(operation, value);
					}
					catch { }
				}
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Set the value associated with the specified property name of the
		/// provided pattern operation.
		/// </summary>
		/// <param name="operation">
		/// Reference to the pattern operation item for which the property value
		/// will be set.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to update.
		/// </param>
		/// <param name="value">
		/// Value to place in the specified property.
		/// </param>
		public static void SetValue(PatternOperationItem operation,
			string propertyName, string value)
		{
			string text = "";

			if(operation != null && propertyName?.Length > 0)
			{
				if(value?.Length > 0)
				{
					text = value;
				}
				try
				{
					operation.GetType().GetProperty(propertyName,
						System.Reflection.BindingFlags.SetProperty |
						System.Reflection.BindingFlags.IgnoreCase |
						System.Reflection.BindingFlags.Instance |
						System.Reflection.BindingFlags.Public).
						SetValue(operation, text);
				}
				catch { }
			}
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	SharedVariables																												*
		////*-----------------------------------------------------------------------*
		//private List<string> mSharedVariables = new List<string>();
		///// <summary>
		///// Get a reference to a list of variable names in this operation that are
		///// shared for the entire pattern.
		///// </summary>
		//public List<string> SharedVariables
		//{
		//	get { return mSharedVariables; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeAction																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the Action property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeAction()
		{
			return true;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeAngle																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the Angle property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeAngle()
		{
			return this.mAngle?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeDepth																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the Depth property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeDepth()
		{
			return this.mDepth?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeEndOffsetX																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the EndOffsetX property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeEndOffsetX()
		{
			return this.mEndOffsetX?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeEndOffsetXOrigin																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the EndOffsetXOrigin
		/// property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeEndOffsetXOrigin()
		{
			return this.mEndOffsetXOrigin != OffsetLeftRightEnum.None;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeEndOffsetY																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the EndOffsetY property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeEndOffsetY()
		{
			return this.mEndOffsetY?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeEndOffsetYOrigin																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the EndOffsetYOrigin
		/// property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeEndOffsetYOrigin()
		{
			return this.mEndOffsetYOrigin != OffsetTopBottomEnum.None;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeHiddenVariables																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the HiddenVariables
		/// property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeHiddenVariables()
		{
			return this.mHiddenVariables.Count > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeKerf																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the Kerf property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeKerf()
		{
			return this.mKerf != DirectionLeftRightEnum.None;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeLength																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the Length property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeLength()
		{
			return this.mLength?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeOffsetX																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the OffsetX property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeOffsetX()
		{
			return this.mOffsetX?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeOffsetXOrigin																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the OffsetXOrigin
		/// property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeOffsetXOrigin()
		{
			return this.mOffsetXOrigin != OffsetLeftRightEnum.None;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeOffsetY																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the OffsetY property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeOffsetY()
		{
			return this.mOffsetY?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeOffsetYOrigin																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the OffsetYOrigin
		/// property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeOffsetYOrigin()
		{
			return this.mOffsetYOrigin != OffsetTopBottomEnum.None;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeOperationId																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the OperationId
		/// property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeOperationId()
		{
			return this.mOperationId?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeOperationName																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the OperationName
		/// property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeOperationName()
		{
			return this.mOperationName?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeStartOffsetX																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the StartOffsetX
		/// property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeStartOffsetX()
		{
			return this.mStartOffsetX?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeStartOffsetXOrigin																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the StartOffsetXOrigin
		/// property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeStartOffsetXOrigin()
		{
			return this.mStartOffsetXOrigin != OffsetLeftRightEnum.None;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeStartOffsetY																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the StartOffsetY
		/// property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeStartOffsetY()
		{
			return this.mStartOffsetY?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeStartOffsetYOrigin																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the StartOffsetYOrigin
		/// property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeStartOffsetYOrigin()
		{
			return this.mStartOffsetYOrigin != OffsetTopBottomEnum.None;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeTool																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the Tool property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeTool()
		{
			return this.mTool?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeWidth																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether to serialize the Width property.
		/// </summary>
		/// <returns>
		/// True if the property will be serialized. Otherwise, false.
		/// </returns>
		public bool ShouldSerializeWidth()
		{
			return this.mWidth?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StartOffsetX																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="StartOffsetX">StartOffsetX</see>.
		/// </summary>
		private string mStartOffsetX = "";
		/// <summary>
		/// Get/Set the distance along the X axis into the material body at which
		/// to start.
		/// </summary>
		/// <remarks>
		/// If neither this value nor StartOffsetXOrigin are supplied, no movement
		/// will take place. If StartOffsetX is supplied and StartOffsetXOrigin is
		/// Relative, the X offset will be relative to the previous router
		/// position.
		/// </remarks>
		[JsonProperty(Order = 3)]
		public string StartOffsetX
		{
			get { return mStartOffsetX; }
			set { mStartOffsetX = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StartOffsetXOrigin																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="StartOffsetXOrigin">StartOffsetXOrigin</see>.
		/// </summary>
		private OffsetLeftRightEnum mStartOffsetXOrigin =
			OffsetLeftRightEnum.None;
		/// <summary>
		/// Get/Set the origin of the offset for the starting X position.
		/// </summary>
		/// <remarks>
		/// If StartOffsetX is 0 and StartOffsetXOrigin is Left or Right, then the
		/// stroke begins at the left or right sides, respectively.
		/// </remarks>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(Order = 4)]
		public OffsetLeftRightEnum StartOffsetXOrigin
		{
			get { return mStartOffsetXOrigin; }
			set { mStartOffsetXOrigin = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StartOffsetY																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="StartOffsetY">StartOffsetY</see>.
		/// </summary>
		private string mStartOffsetY = "";
		/// <summary>
		/// Get/Set the distance along the Y axis into the material body.
		/// </summary>
		/// <remarks>
		/// If neither this value nor StartOffsetYOrigin are supplied, no movement
		/// will take place. If StartOffsetY is supplied and StartOffsetYOrigin is
		/// Relative, the Y offset will be relative to the previous router
		/// position.
		/// </remarks>
		[JsonProperty(Order = 5)]
		public string StartOffsetY
		{
			get { return mStartOffsetY; }
			set { mStartOffsetY = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StartOffsetYOrigin																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="StartOffsetYOrigin">StartOffsetYOrigin</see>.
		/// </summary>
		private OffsetTopBottomEnum mStartOffsetYOrigin = OffsetTopBottomEnum.None;
		/// <summary>
		/// Get/Set the origin of the offset for the starting Y position.
		/// </summary>
		/// <remarks>
		/// If StartOffsetY is 0 and StartOffsetYOrigin is Top or Bottom, then the
		/// stroke begins at the top or bottom sides, respectively.
		/// </remarks>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(Order = 6)]
		public OffsetTopBottomEnum StartOffsetYOrigin
		{
			get { return mStartOffsetYOrigin; }
			set { mStartOffsetYOrigin = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Tool																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Tool">Tool</see>.
		/// </summary>
		private string mTool = "";
		/// <summary>
		/// Get/Set the name of the selected tool. If no tool was specified, then
		/// the general cutting tool is used.
		/// </summary>
		[JsonProperty(Order = 20)]
		public string Tool
		{
			get { return mTool; }
			set { mTool = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Width																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Width">Width</see>.
		/// </summary>
		private string mWidth = "";
		/// <summary>
		/// Get/Set the width of the operation.
		/// </summary>
		[JsonProperty(Order = 17)]
		public string Width
		{
			get { return mWidth; }
			set { mWidth = value; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*


}
