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

		////*-----------------------------------------------------------------------*
		////* CalculateResultingValues																							*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Calculate the resulting system values for the supplied user values on
		///// the provided operation.
		///// </summary>
		///// <param name="operation">
		///// The operation for which user values have been provided.
		///// </param>
		///// <param name="workpiece">
		///// Reference to information about the workpiece.
		///// </param>
		///// <param name="startingLocation">
		///// Reference to the starting location from which the operation will
		///// begin, in system units
		///// </param>
		///// <param name="previousToolName">
		///// Name of the previous tool.
		///// </param>
		///// <returns>
		///// Reference to the location at which the current operation will end,
		///// in system units.
		///// </returns>
		///// <remarks>
		///// <para>
		///// The starting offsets of all actions are updated in StartOffset{X|Y},
		///// since the abbreviated Offset{X|Y} is for user familiarity only.
		///// </para>
		///// <para>
		///// All of the measurements here are meant to be absolute in the local
		///// scope, relative to the workpiece.
		///// </para>
		///// </remarks>
		//public static FPoint CalculateResultingValues(
		//	PatternOperationItem operation,
		//	WorkpieceInfoItem workpiece, FPoint startingLocation,
		//	string previousToolName)
		//{
		//	float angle = 0f;
		//	float length = 0f;
		//	FPoint location = null;
		//	float position = 0f;
		//	FPoint result = new FPoint();
		//	FPoint transform = new FPoint();
		//	float width = 0f;

		//	if(operation != null && workpiece != null)
		//	{
		//		location = FPoint.Clone(startingLocation);
		//		//	First, convert every user value to system units.
		//		//operation.mAngle = GetAngle(operation.mAngle).ToString("0.######");
		//		if(operation.mDepth.Length == 0)
		//		{
		//			//	Depth wasn't specified. Get the workpiece depth.
		//			operation.mDepth = $"{workpiece.Thickness:0.###}mm";
		//		}
		//		//if(operation.mKerf == DirectionLeftRightEnum.None)
		//		//{
		//		//	//	Default kerf is right.
		//		//	operation.mKerf = DirectionLeftRightEnum.Right;
		//		//}
		//		if(operation.mTool?.Length == 0 ||
		//			operation.mTool.ToLower() == "{generaltoolname}")
		//		{
		//			//	General tool specification.
		//			operation.mTool = ConfigProfile.GeneralCuttingTool;
		//		}
		//		else if(operation.mTool.ToLower() == "{previoustoolname}")
		//		{
		//			//	Previously used tool.
		//			operation.mTool = previousToolName;
		//		}
		//		//	StartX,Y.
		//		switch(operation.mAction)
		//		{
		//			case OperationActionEnum.DrawCircleCenterDiameter:
		//			case OperationActionEnum.DrawCircleCenterRadius:
		//			case OperationActionEnum.DrawCircleDiameter:
		//			case OperationActionEnum.DrawCircleRadius:
		//			case OperationActionEnum.DrawEllipseCenterDiameterXY:
		//			case OperationActionEnum.DrawEllipseCenterRadiusXY:
		//			case OperationActionEnum.DrawEllipseDiameterXY:
		//			case OperationActionEnum.DrawEllipseLengthWidth:
		//			case OperationActionEnum.DrawEllipseRadiusXY:
		//			case OperationActionEnum.DrawLineAngleLength:
		//			case OperationActionEnum.DrawLineLengthWidth:
		//			case OperationActionEnum.DrawRectangleLengthWidth:
		//			case OperationActionEnum.FillCircleCenterDiameter:
		//			case OperationActionEnum.FillCircleCenterRadius:
		//			case OperationActionEnum.FillCircleDiameter:
		//			case OperationActionEnum.FillCircleRadius:
		//			case OperationActionEnum.FillEllipseCenterDiameterXY:
		//			case OperationActionEnum.FillEllipseCenterRadiusXY:
		//			case OperationActionEnum.FillEllipseDiameterXY:
		//			case OperationActionEnum.FillEllipseLengthWidth:
		//			case OperationActionEnum.FillEllipseRadiusXY:
		//			case OperationActionEnum.FillRectangleLengthWidth:
		//			case OperationActionEnum.PointXY:
		//				//	Starting OffsetX, OffsetY.
		//				//	X.
		//				position = 0f;
		//				if(operation.mOffsetX.Length > 0)
		//				{
		//					//	X was specified.
		//					position = GetMillimeters(operation.mOffsetX);
		//				}
		//				operation.mStartOffsetXOrigin = operation.mOffsetXOrigin;
		//				if(operation.mEndOffsetXOrigin != OffsetLeftRightEnum.Absolute)
		//				{
		//					position += TranslateOffset(workpiece.Area, 0f, location.X,
		//						operation.mEndOffsetXOrigin);
		//				}
		//				else
		//				{
		//					transform.X = position;
		//					transform.Y = 0f;
		//					position = TransformFromAbsolute(transform).X;
		//				}
		//				operation.mStartOffsetX = operation.mOffsetX =
		//					$"{position:0.###}mm";
		//				//	Y.
		//				position = 0f;
		//				if(operation.mOffsetY.Length > 0)
		//				{
		//					//	Y was specified.
		//					position = GetMillimeters(operation.mOffsetY);
		//				}
		//				operation.mStartOffsetYOrigin = operation.mOffsetYOrigin;
		//				if(operation.mStartOffsetYOrigin != OffsetTopBottomEnum.Absolute)
		//				{
		//					position += TranslateOffset(workpiece.Area, 0f, location.Y,
		//						operation.mOffsetYOrigin);
		//				}
		//				else
		//				{
		//					transform.X = 0f;
		//					transform.Y = position;
		//					position = TransformFromAbsolute(transform).Y;
		//				}
		//				operation.mStartOffsetY = operation.mOffsetY =
		//					$"{position:0.###}mm";
		//				break;
		//			case OperationActionEnum.DrawEllipseXY:
		//			case OperationActionEnum.DrawLineXY:
		//			case OperationActionEnum.DrawRectangleXY:
		//			case OperationActionEnum.FillEllipseXY:
		//			case OperationActionEnum.FillRectangleXY:
		//				//	StartOffsetX, StartOffsetY.
		//				//	X.
		//				position = 0f;
		//				if(operation.mStartOffsetX.Length > 0)
		//				{
		//					//	X was specified.
		//					position = GetMillimeters(operation.mStartOffsetX);
		//				}
		//				if(operation.mStartOffsetXOrigin != OffsetLeftRightEnum.Absolute)
		//				{
		//					position += TranslateOffset(workpiece.Area, 0f, location.X,
		//						operation.mStartOffsetXOrigin);
		//				}
		//				else
		//				{
		//					transform.X = position;
		//					transform.Y = 0f;
		//					position = TransformFromAbsolute(transform).X;
		//				}
		//				operation.mStartOffsetX = $"{position:0.###}mm";
		//				//	Y.
		//				position = 0f;
		//				if(operation.mStartOffsetY.Length > 0)
		//				{
		//					//	Y was specified.
		//					position = GetMillimeters(operation.mStartOffsetY);
		//				}
		//				if(operation.mStartOffsetYOrigin != OffsetTopBottomEnum.Absolute)
		//				{
		//					position += TranslateOffset(workpiece.Area, 0f, location.Y,
		//						operation.mStartOffsetYOrigin);
		//				}
		//				else
		//				{
		//					transform.X = 0f;
		//					transform.Y = position;
		//					position = TransformFromAbsolute(transform).Y;
		//				}
		//				operation.mStartOffsetY = $"{position:0.###}mm";
		//				break;
		//		}
		//		//	Update the location from the starting point.
		//		location.X = GetMillimeters(operation.mStartOffsetX);
		//		location.Y = GetMillimeters(operation.mStartOffsetY);
		//		//	Extent.
		//		switch(operation.mAction)
		//		{
		//			case OperationActionEnum.DrawCircleCenterDiameter:
		//			case OperationActionEnum.DrawCircleCenterRadius:
		//			case OperationActionEnum.DrawCircleDiameter:
		//			case OperationActionEnum.DrawCircleRadius:
		//			case OperationActionEnum.DrawEllipseCenterDiameterXY:
		//			case OperationActionEnum.DrawEllipseCenterRadiusXY:
		//			case OperationActionEnum.DrawEllipseDiameterXY:
		//			case OperationActionEnum.DrawEllipseLengthWidth:
		//			case OperationActionEnum.DrawEllipseRadiusXY:
		//			case OperationActionEnum.DrawEllipseXY:
		//			case OperationActionEnum.FillCircleCenterDiameter:
		//			case OperationActionEnum.FillCircleCenterRadius:
		//			case OperationActionEnum.FillCircleDiameter:
		//			case OperationActionEnum.FillCircleRadius:
		//			case OperationActionEnum.FillEllipseCenterDiameterXY:
		//			case OperationActionEnum.FillEllipseCenterRadiusXY:
		//			case OperationActionEnum.FillEllipseDiameterXY:
		//			case OperationActionEnum.FillEllipseLengthWidth:
		//			case OperationActionEnum.FillEllipseRadiusXY:
		//			case OperationActionEnum.FillEllipseXY:
		//			case OperationActionEnum.PointXY:
		//				//	End = Start.
		//				operation.mEndOffsetX = operation.mStartOffsetX;
		//				operation.mEndOffsetY = operation.mStartOffsetY;
		//				break;
		//			case OperationActionEnum.DrawLineAngleLength:
		//			case OperationActionEnum.MoveAngleLength:
		//				//	Angle, Length.
		//				angle = GetAngle(operation.mAngle);
		//				length = GetMillimeters(operation.mLength);
		//				location = Trig.GetDestPoint(location, angle, length);
		//				operation.mEndOffsetX = $"{location.X:0.###}mm";
		//				operation.mEndOffsetY = $"{location.Y:0.###}mm";
		//				break;
		//			case OperationActionEnum.DrawLineLengthWidth:
		//			case OperationActionEnum.DrawRectangleLengthWidth:
		//			case OperationActionEnum.FillRectangleLengthWidth:
		//				//	Length, Width.
		//				length = GetMillimeters(operation.mLength);
		//				width = GetMillimeters(operation.mWidth);
		//				if(ConfigProfile.AxisXIsOpenEnded)
		//				{
		//					location.X += length;
		//					location.Y += width;
		//				}
		//				else
		//				{
		//					location.X += width;
		//					location.Y += length;
		//				}
		//				operation.mEndOffsetX = $"{location.X:0.###}mm";
		//				operation.mEndOffsetY = $"{location.Y:0.###}mm";
		//				break;
		//			case OperationActionEnum.DrawLineXY:
		//			case OperationActionEnum.DrawRectangleXY:
		//			case OperationActionEnum.FillRectangleXY:
		//				//	EndOffsetX, EndOffsetY.
		//				//	X.
		//				position = 0f;
		//				if(operation.mEndOffsetX.Length > 0)
		//				{
		//					//	End offset was specified.
		//					position = GetMillimeters(operation.mEndOffsetX);
		//				}
		//				if(operation.mEndOffsetXOrigin != OffsetLeftRightEnum.Absolute)
		//				{
		//					position += TranslateOffset(workpiece.Area, 0f,
		//						GetMillimeters(operation.mStartOffsetX),
		//						operation.mEndOffsetXOrigin);
		//				}
		//				else
		//				{
		//					transform.X = position;
		//					transform.Y = 0f;
		//					position = TransformFromAbsolute(transform).X;
		//				}
		//				operation.mEndOffsetX = $"{position:0.###}mm";
		//				//	Y.
		//				position = 0f;
		//				if(operation.mEndOffsetY.Length > 0)
		//				{
		//					//	End offset was specified.
		//					position = GetMillimeters(operation.mEndOffsetY);
		//				}
		//				if(operation.mEndOffsetYOrigin != OffsetTopBottomEnum.Absolute)
		//				{
		//					position += TranslateOffset(workpiece.Area, 0f,
		//						GetMillimeters(operation.mStartOffsetY),
		//						operation.mEndOffsetYOrigin);
		//				}
		//				else
		//				{
		//					transform.X = 0f;
		//					transform.Y = position;
		//					position = TransformFromAbsolute(transform).Y;
		//				}
		//				operation.mEndOffsetY = $"{position:0.###}mm";
		//				break;
		//			case OperationActionEnum.MoveXY:
		//				//	Ending OffsetX, OffsetY.
		//				//	X.
		//				position = 0f;
		//				if(operation.mOffsetX.Length > 0)
		//				{
		//					//	End offset was specified.
		//					position = GetMillimeters(operation.mOffsetX);
		//				}
		//				if(operation.mOffsetXOrigin != OffsetLeftRightEnum.Absolute)
		//				{
		//					position += TranslateOffset(workpiece.Area, 0f,
		//						GetMillimeters(operation.mStartOffsetX),
		//						operation.mOffsetXOrigin);
		//				}
		//				else
		//				{
		//					transform.X = position;
		//					transform.Y = 0f;
		//					position = TransformFromAbsolute(transform).X;
		//				}
		//				operation.mEndOffsetX = operation.mOffsetX = $"{position:0.###}mm";
		//				//	Y.
		//				position = 0f;
		//				if(operation.mOffsetY.Length > 0)
		//				{
		//					//	End offset was specified.
		//					position = GetMillimeters(operation.mOffsetY);
		//				}
		//				if(operation.mOffsetYOrigin != OffsetTopBottomEnum.Absolute)
		//				{
		//					position += TranslateOffset(workpiece.Area, 0f,
		//						GetMillimeters(operation.mStartOffsetY),
		//						operation.mOffsetYOrigin);
		//				}
		//				else
		//				{
		//					transform.X = 0f;
		//					transform.Y = position;
		//					position = TransformFromAbsolute(transform).Y;
		//				}
		//				operation.mEndOffsetY = operation.mOffsetY = $"{position:0.###}mm";
		//				break;
		//		}
		//		//	Update the location from the ending location.
		//		location.X = GetMillimeters(operation.mEndOffsetX);
		//		location.Y = GetMillimeters(operation.mEndOffsetY);
		//		result = location;
		//	}
		//	return result;
		//}
		////*-----------------------------------------------------------------------*

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
		/// </param>
		/// <returns>
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
