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
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using Geometry;
using Newtonsoft.Json;
using SvgPlotting;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	ShopToolsUtil																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Utility features and functionality for the ShopTools application.
	/// </summary>
	public class ShopToolsUtil
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		/// <summary>
		/// Color used for the non-selected area fill.
		/// </summary>
		private const string mColorFill = "#3fff0000";
		/// <summary>
		/// Color used for the selected area fill.
		/// </summary>
		private const string mColorFillSelected = "#3f0900ff";
		/// <summary>
		/// Color used for the non-selected draw pen.
		/// </summary>
		private const string mColorDrawPen = "#ffff0000";
		/// <summary>
		/// Color used for the selected draw pen.
		/// </summary>
		private const string mColorDrawPenSelected = "#ff0000ff";
		/// <summary>
		/// Color used for the non-selected move pen.
		/// </summary>
		private const string mColorMovePen = "#ffffffff";
		/// <summary>
		/// Color used for the selected move pen.
		/// </summary>
		private const string mColorMovePenSelected = "#ff0000ff";
		/// <summary>
		/// Color used for router background brush.
		/// </summary>
		private const string mColorRouterBackgroundBrush = "#7fffffff";
		/// <summary>
		/// Color used for router end position pen.
		/// </summary>
		private const string mColorRouterEndPen = "#f0ff0000";
		/// <summary>
		/// Color used for selected router end position pen.
		/// </summary>
		private const string mColorRouterEndPenSelected = "";
		/// <summary>
		/// Color used for router start position pen.
		/// </summary>
		private const string mColorRouterStartPen = "#f0007f00";
		/// <summary>
		/// Color used for selected router start position pen.
		/// </summary>
		private const string mColorRouterStartPenSelected = "";
		/// <summary>
		/// Color used for the working screen background.
		/// </summary>
		private const string mColorScreenBackground = "#113366";
		/// <summary>
		/// Color used for the drill target area fill.
		/// </summary>
		private const string mColorTargetBackgroundBrush = "#3fffffff";
		/// <summary>
		/// Color used for the non-selected drill target pen.
		/// </summary>
		private const string mColorTargetPen = "#fffffff0";
		/// <summary>
		/// Color used for the selected drill target pen.
		/// </summary>
		private const string mColorTargetPenSelected = "#f00000ff";
		/// <summary>
		/// Color used for workpiece border pen.
		/// </summary>
		private const string mColorWorkpieceBorderPen = "#6e6d11";
		/// <summary>
		/// Color used for workpiece brush.
		/// </summary>
		private const string mColorWorkpieceBrush = "#333333";
		/// <summary>
		/// Color used for workspace background.
		/// </summary>
		private const string mColorWorkspaceBackground = "#603e1f";
		/// <summary>
		/// Color used for workspace border pen.
		/// </summary>
		private const string mColorWorkspaceBorderPen = "#000000";


		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	BaseUnit																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the base unit of measure for the specified display unit.
		/// </summary>
		/// <param name="displayUnit">
		/// The general display unit for which to find the default measurement
		/// unit.
		/// </param>
		/// <returns>
		/// The default measurement unit for the specified display unit.
		/// </returns>
		public static string BaseUnit(DisplayUnitEnum displayUnit)
		{
			string result = "mm";

			if(displayUnit == DisplayUnitEnum.UnitedStates)
			{
				result = "in";
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CalculateLayout																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Calculate the start and end locations of the tool and display for each
		/// of the operations in the caller's operations collection, returning the
		/// final location of the toolpath.
		/// </summary>
		/// <returns>
		/// Reference to the last known tool location.
		/// </returns>
		/// <remarks>
		/// <para>
		/// This overload updates the layout patterns for all of the operations of
		/// all of the patterns in the session workpiece.
		/// </para>
		/// <para>
		/// The SessionWorkpieceInfo / Cuts / Pattern / Operations / Operation /
		/// LayoutElements collection will contain additional transitions not
		/// specified in the source collection so the tool head is moved properly
		/// to an from each operational site.
		/// </para>
		/// </remarks>
		public static FVector2 CalculateLayout()
		{
			FVector2 location =
				TransformFromAbsolute(mSessionWorkpieceInfo.RouterLocation);
			foreach(CutProfileItem profileItem in mSessionWorkpieceInfo.Cuts)
			{
				profileItem.StartLocation = TransformToAbsolute(location);
				location =
					CalculateLayout(profileItem, mSessionWorkpieceInfo, location);
				profileItem.EndLocation = TransformToAbsolute(location);
			}
			return location;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
	/// <summary>
		/// Calculate the start and end locations of the tool and display for each
		/// of the operations in the caller's operations collection, returning a
		/// collection of locations, each referenced to its associated operation.
		/// </summary>
		/// <param name="pattern">
		/// Reference to the cut pattern to inspect.
		/// </param>
		/// <param name="workpiece">
		/// Reference to the active workpiece used for relative offsets.
		/// </param>
		/// <param name="startLocation">
		/// Reference to the starting location of the pattern, in drawing space.
		/// </param>
		/// <returns>
		/// Reference to the last known tool location.
		/// </returns>
		/// <remarks>
		/// The pattern / Operations / Operation / LayoutElements collection will
		/// contain additional transitions not specified in the source collection
		/// so the tool head is moved properly to an from each operational site.
		/// </remarks>
		public static FVector2 CalculateLayout(CutProfileItem pattern,
			WorkpieceInfoItem workpiece, FVector2 startLocation)
		{
			float angle = 0f;
			float diameter = 0f;
			FVector2 diameterXY = null;
			FVector2 endOffset = null;
			float length = 0f;
			FVector2 location = null;
			FVector2 offset = null;
			float radius = 0f;
			FVector2 radiusXY = null;
			float startAngle = 0f;
			FVector2 startOffset = null;
			float sweepAngle = 0f;
			float width = 0f;

			if(pattern != null)
			{
				if(startLocation != null)
				{
					location = new FVector2(startLocation);
				}
				//	Pattern start.
				foreach(PatternOperationItem operationItem in pattern.Operations)
				{
					operationItem.LayoutElements.Clear();
					//	Start of Operation.
					switch(operationItem.Action)
					{
						//	TODO: Add Winding parameter to configuration.
						//case OperationActionEnum.DrawArcCenterOffsetXY:
						//	//	Offset - Center coordinate.
						//	//	StartOffset - Starting Point.
						//	//	EndOffset - Ending Point.
						//	offset = PatternOperationItem.GetOffsetParameter(
						//		operationItem, workpiece, location);
						//	startOffset = PatternOperationItem.GetStartOffsetParameter(
						//		operationItem, workpiece, offset);
						//	endOffset = PatternOperationItem.GetEndOffsetParameter(
						//		operationItem, workpiece, startOffset);
						//	location = OperationLayoutCollection.AddArcCenterOffsetXY(
						//		operationItem, offset, startOffset, endOffset, location);
						//	break;
						case OperationActionEnum.DrawArcCenterOffsetXYAngle:
							//	Offset - Center coordinate.
							//	StartOffset - Starting Point.
							//	Angle - Sweep angle.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							startOffset = PatternOperationItem.GetStartOffsetParameter(
								operationItem, workpiece, offset);
							angle = GetAngle(operationItem.Angle);
							location = OperationLayoutCollection.AddArcCenterOffsetXYAngle(
								operationItem, offset, startOffset, angle, location);
							break;
						case OperationActionEnum.DrawArcCenterRadiusStartSweepAngle:
							//	Offset - Center coordinate.
							//	Radius - Circle radius from center.
							//	StartAngle - Starting angle.
							//	EndAngle - Ending angle.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							radius = GetMillimeters(operationItem.Radius);
							startAngle = GetAngle(operationItem.StartAngle);
							sweepAngle = GetAngle(operationItem.SweepAngle);
							location = OperationLayoutCollection.
								AddArcCenterRadiusStartSweepAngle(operationItem,
								offset, radius, startAngle, sweepAngle, location);
							break;
						case OperationActionEnum.DrawCircleCenterDiameter:
							//	Offset - Center coordinate.
							//	Diameter - Circle diameter.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							diameter = GetMillimeters(operationItem.Diameter);
							radius = diameter / 2f;
							location = OperationLayoutCollection.AddEllipseCenterRadius(
								operationItem, offset, radius, radius, location, false);
							break;
						case OperationActionEnum.DrawCircleCenterRadius:
							//	Offset - Center coordinate.
							//	Radius - Circle radius.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							radius = GetMillimeters(operationItem.Radius);
							location = OperationLayoutCollection.AddEllipseCenterRadius(
								operationItem, offset, radius, radius, location, false);
							break;
						case OperationActionEnum.DrawCircleDiameter:
							//	Offset - Top left XY coordinate.
							//	Diameter - Circle diameter.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							diameter = GetMillimeters(operationItem.Diameter);
							radius = diameter / 2f;
							location = OperationLayoutCollection.AddEllipseCornerRadius(
								operationItem, offset, radius, radius, location, false);
							break;
						case OperationActionEnum.DrawCircleRadius:
							//	Offset - Top left XY coordinate.
							//	Radius - Circle radius.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							radius = GetMillimeters(operationItem.Radius);
							location = OperationLayoutCollection.AddEllipseCornerRadius(
								operationItem, offset, radius, radius, location, false);
							break;
						case OperationActionEnum.DrawEllipseCenterDiameterXY:
							//	Offset - Center coordinate.
							//	DiameterX - Diameter on X-axis.
							//	DiameterY - Diameter on Y-axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							diameterXY = PatternOperationItem.GetDiameterXYParameter(
								operationItem, workpiece);
							location = OperationLayoutCollection.AddEllipseCenterRadius(
								operationItem, offset,
								diameterXY.X / 2f, diameterXY.Y / 2f, location, false);
							break;
						case OperationActionEnum.DrawEllipseCenterRadiusXY:
							//	Offset - Center coordinate.
							//	RadiusX - Radius on X-axis.
							//	RadiusY - Radius on Y-axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							radiusXY = PatternOperationItem.GetRadiusXYParameter(
								operationItem, workpiece);
							location = OperationLayoutCollection.AddEllipseCenterRadius(
								operationItem, offset, radiusXY.X, radiusXY.Y, location, false);
							break;
						case OperationActionEnum.DrawEllipseDiameterXY:
							//	Offset - Top left XY coordinate.
							//	DiameterX - Diameter on the X-axis.
							//	DiameterY - Diameter on the Y-axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							diameterXY = PatternOperationItem.GetDiameterXYParameter(
								operationItem, workpiece);
							location = OperationLayoutCollection.AddEllipseCornerRadius(
								operationItem, offset,
								diameterXY.X / 2f, diameterXY.Y / 2f, location, false);
							break;
						case OperationActionEnum.DrawEllipseLengthWidth:
							//	Offset - Top left XY coordinate.
							//	Length - Diameter on the length axis.
							//	Width - Diameter on the width axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							length = GetMillimeters(operationItem.Length);
							width = GetMillimeters(operationItem.Width);
							diameterXY = PatternOperationItem.GetLengthWidthXYParameter(
								length, width);
							location = OperationLayoutCollection.AddEllipseCornerRadius(
								operationItem, offset,
								diameterXY.X / 2f, diameterXY.Y / 2f, location, false);
							break;
						case OperationActionEnum.DrawEllipseRadiusXY:
							//	Offset - Top left XY coordinate.
							//	RadiusX - Radius on the X-axis.
							//	RadiusY - Radius on the Y-axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							radiusXY = PatternOperationItem.GetRadiusXYParameter(
								operationItem, workpiece);
							location = OperationLayoutCollection.AddEllipseCornerRadius(
								operationItem, offset, radiusXY.X, radiusXY.Y, location, false);
							break;
						case OperationActionEnum.DrawEllipseXY:
							//	StartOffset - Top left XY coordinate.
							//	EndOffset - Bottom right XY coordinate.
							startOffset = PatternOperationItem.GetStartOffsetParameter(
								operationItem, workpiece, location);
							endOffset = PatternOperationItem.GetEndOffsetParameter(
								operationItem, workpiece, startOffset);
							radiusXY = new FVector2()
							{
								X = (endOffset.X - startOffset.X) / 2f,
								Y = (endOffset.Y - startOffset.Y) / 2f
							};
							location = OperationLayoutCollection.AddEllipseCornerRadius(
								operationItem, offset, radiusXY.X, radiusXY.Y, location,
								false);
							break;
						case OperationActionEnum.DrawLineAngleLength:
							//	Offset - The starting offset of the line.
							//	Angle - The angle of the line.
							//	Length - The length of the line.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							angle = GetAngle(operationItem.Angle);
							length = GetMillimeters(operationItem.Length);
							endOffset = Trig.GetDestPoint(offset, angle, length);
							location = OperationLayoutCollection.AddLine(operationItem,
								offset, endOffset, location);
							break;
						case OperationActionEnum.DrawLineLengthWidth:
							//	Offset - The starting offset of the line.
							//	Length - The length of the line, in terms of the length
							//	axis base.
							//	Width - The width of the line, in terms of the width
							//	axis base.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							length = GetMillimeters(operationItem.Length);
							width = GetMillimeters(operationItem.Width);
							endOffset = PatternOperationItem.
								GetLengthWidthXYParameter(length, width);
							//	Convert distance to end coordinate.
							endOffset.X += offset.X;
							endOffset.Y += offset.Y;
							location = OperationLayoutCollection.AddLine(operationItem,
								offset, endOffset, location);
							break;
						case OperationActionEnum.DrawLineXY:
							//	StartOffset - The starting coordinate of the line.
							//	EndOffset - The ending coordinate of the line.
							startOffset = PatternOperationItem.GetStartOffsetParameter(
								operationItem, workpiece, location);
							endOffset = PatternOperationItem.GetEndOffsetParameter(
								operationItem, workpiece, startOffset);
							location = OperationLayoutCollection.AddLine(operationItem,
								startOffset, endOffset, location);
							break;
						case OperationActionEnum.DrawRectangleCenterLengthWidth:
							//	Offset - The center coordinate.
							//	Length - The distance along the length axis.
							//	Width - The distance along the width axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							length = GetMillimeters(operationItem.Length);
							width = GetMillimeters(operationItem.Width);
							diameterXY = PatternOperationItem.
								GetLengthWidthXYParameter(length, width);
							location = OperationLayoutCollection.AddRectangleCenterXY(
								operationItem, offset, diameterXY.X, diameterXY.Y, location,
								false);
							break;
						case OperationActionEnum.DrawRectangleLengthWidth:
							//	Offset - The coordinate of the upper left corner.
							//	Length - The distance along the length axis.
							//	Width - The distance along the width axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							length = GetMillimeters(operationItem.Length);
							width = GetMillimeters(operationItem.Width);
							diameterXY = PatternOperationItem.
								GetLengthWidthXYParameter(length, width);
							location = OperationLayoutCollection.AddRectangleCornerXY(
								operationItem, offset, diameterXY.X, diameterXY.Y, location,
								false);
							break;
						case OperationActionEnum.DrawRectangleXY:
							//	StartOffset = The starting coordinate.
							//	EndOffset - The ending coordinate.
							startOffset = PatternOperationItem.GetStartOffsetParameter(
								operationItem, workpiece, location);
							endOffset = PatternOperationItem.GetEndOffsetParameter(
								operationItem, workpiece, startOffset);
							diameterXY = new FVector2(endOffset.X - startOffset.X,
								endOffset.Y - startOffset.Y);
							location = OperationLayoutCollection.AddRectangleCornerXY(
								operationItem, offset, diameterXY.X, diameterXY.Y, location,
								false);
							break;
						case OperationActionEnum.FillCircleCenterDiameter:
							//	Offset - Center coordinate.
							//	Diameter - Circle diameter.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							diameter = GetMillimeters(operationItem.Diameter);
							radius = diameter / 2f;
							location = OperationLayoutCollection.AddEllipseCenterRadius(
								operationItem, offset, radius, radius, location, true);
							break;
						case OperationActionEnum.FillCircleCenterRadius:
							//	Offset - Center coordinate.
							//	Radius - Circle radius.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							radius = GetMillimeters(operationItem.Radius);
							location = OperationLayoutCollection.AddEllipseCenterRadius(
								operationItem, offset, radius, radius, location, true);
							break;
						case OperationActionEnum.FillCircleDiameter:
							//	Offset - Top left XY coordinate.
							//	Diameter - Circle diameter.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							diameter = GetMillimeters(operationItem.Diameter);
							radius = diameter / 2f;
							location = OperationLayoutCollection.AddEllipseCornerRadius(
								operationItem, offset, radius, radius, location, true);
							break;
						case OperationActionEnum.FillCircleRadius:
							//	Offset - Top left XY coordinate.
							//	Radius - Circle radius.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							radius = GetMillimeters(operationItem.Radius);
							location = OperationLayoutCollection.AddEllipseCornerRadius(
								operationItem, offset, radius, radius, location, true);
							break;
						case OperationActionEnum.FillEllipseCenterDiameterXY:
							//	Offset - Center coordinate.
							//	DiameterX - Diameter on X-axis.
							//	DiameterY - Diameter on Y-axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							diameterXY = PatternOperationItem.GetDiameterXYParameter(
								operationItem, workpiece);
							location = OperationLayoutCollection.AddEllipseCenterRadius(
								operationItem, offset,
								diameterXY.X / 2f, diameterXY.Y / 2f, location, true);
							break;
						case OperationActionEnum.FillEllipseCenterRadiusXY:
							//	Offset - Center coordinate.
							//	RadiusX - Radius on X-axis.
							//	RadiusY - Radius on Y-axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							radiusXY = PatternOperationItem.GetRadiusXYParameter(
								operationItem, workpiece);
							location = OperationLayoutCollection.AddEllipseCenterRadius(
								operationItem, offset, radiusXY.X, radiusXY.Y, location, true);
							break;
						case OperationActionEnum.FillEllipseDiameterXY:
							//	Offset - Top left XY coordinate.
							//	DiameterX - Diameter on the X-axis.
							//	DiameterY - Diameter on the Y-axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							diameterXY = PatternOperationItem.GetDiameterXYParameter(
								operationItem, workpiece);
							location = OperationLayoutCollection.AddEllipseCornerRadius(
								operationItem, offset,
								diameterXY.X / 2f, diameterXY.Y / 2f, location, true);
							break;
						case OperationActionEnum.FillEllipseLengthWidth:
							//	Offset - Top left XY coordinate.
							//	Length - Diameter on the length axis.
							//	Width - Diameter on the width axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							length = GetMillimeters(operationItem.Length);
							width = GetMillimeters(operationItem.Width);
							diameterXY = PatternOperationItem.GetLengthWidthXYParameter(
								length, width);
							location = OperationLayoutCollection.AddEllipseCornerRadius(
								operationItem, offset,
								diameterXY.X / 2f, diameterXY.Y / 2f, location, true);
							break;
						case OperationActionEnum.FillEllipseRadiusXY:
							//	Offset - Top left XY coordinate.
							//	RadiusX - Radius on the X-axis.
							//	RadiusY - Radius on the Y-axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							radiusXY = PatternOperationItem.GetRadiusXYParameter(
								operationItem, workpiece);
							location = OperationLayoutCollection.AddEllipseCornerRadius(
								operationItem, offset, radiusXY.X, radiusXY.Y, location, true);
							break;
						case OperationActionEnum.FillEllipseXY:
							//	StartOffset - Top left XY coordinate.
							//	EndOffset - Bottom right XY coordinate.
							startOffset = PatternOperationItem.GetStartOffsetParameter(
								operationItem, workpiece, location);
							endOffset = PatternOperationItem.GetEndOffsetParameter(
								operationItem, workpiece, startOffset);
							radiusXY = new FVector2()
							{
								X = (endOffset.X - startOffset.X) / 2f,
								Y = (endOffset.Y - startOffset.Y) / 2f
							};
							location = OperationLayoutCollection.AddEllipseCornerRadius(
								operationItem, offset, radiusXY.X, radiusXY.Y, location,
								true);
							break;
						case OperationActionEnum.FillRectangleCenterLengthWidth:
							//	Offset - The center coordinate.
							//	Length - The distance along the length axis.
							//	Width - The distance along the width axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							length = GetMillimeters(operationItem.Length);
							width = GetMillimeters(operationItem.Width);
							diameterXY = PatternOperationItem.
								GetLengthWidthXYParameter(length, width);
							location = OperationLayoutCollection.AddRectangleCenterXY(
								operationItem, offset, diameterXY.X, diameterXY.Y, location,
								false);
							break;
						case OperationActionEnum.FillRectangleLengthWidth:
							//	Offset - The coordinate of the upper left corner.
							//	Length - The distance along the length axis.
							//	Width - The distance along the width axis.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							length = GetMillimeters(operationItem.Length);
							width = GetMillimeters(operationItem.Width);
							diameterXY = PatternOperationItem.
								GetLengthWidthXYParameter(length, width);
							location = OperationLayoutCollection.AddRectangleCornerXY(
								operationItem, offset, diameterXY.X, diameterXY.Y, location,
								true);
							break;
						case OperationActionEnum.FillRectangleXY:
							//	StartOffset - The starting coordinate.
							//	EndOffset - The ending coordinate.
							startOffset = PatternOperationItem.GetStartOffsetParameter(
								operationItem, workpiece, location);
							endOffset = PatternOperationItem.GetEndOffsetParameter(
								operationItem, workpiece, startOffset);
							diameterXY = new FVector2(endOffset.X - startOffset.X,
								endOffset.Y - startOffset.Y);
							location = OperationLayoutCollection.AddRectangleCornerXY(
								operationItem, offset, diameterXY.X, diameterXY.Y, location,
								true);
							break;
						case OperationActionEnum.MoveAngleLength:
							//	Offset - Starting coordinate.
							//	Angle - The angle at which to move.
							//	Length - The distance to move.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							angle = GetAngle(operationItem.Angle);
							length = GetMillimeters(operationItem.Length);
							endOffset = Trig.GetDestPoint(offset, angle, length);
							//	This version doesn't include automatically articulated
							//	moves.
							//location = OperationLayoutCollection.AddMove(
							//	operationItem, offset, location);
							location = OperationLayoutCollection.AddMove(
								operationItem, endOffset, location);
							break;
						case OperationActionEnum.MoveXY:
							//	Offset - The coordinate to which a transit will occur.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							location = OperationLayoutCollection.AddMove(
								operationItem, offset, location);
							break;
						case OperationActionEnum.PointXY:
							//	Offset - The coordinate at which the drill will be made.
							offset = PatternOperationItem.GetOffsetParameter(
								operationItem, workpiece, location);
							location = OperationLayoutCollection.AddPoint(
								operationItem, offset, location);
							break;
					}
					//	End of Operation.
				}
			}
			if(location == null)
			{
				location = new FVector2();
			}
			return location;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CenteredLeft																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the left coordinate of an object centered within an area.
		/// </summary>
		/// <param name="areaWidth">
		/// Width of the area in which the object is centered.
		/// </param>
		/// <param name="objectWidth">
		/// Width of the object to be placed.
		/// </param>
		/// <returns>
		/// The left coordinate of the centered object.
		/// </returns>
		public static int CenteredLeft(int areaWidth, int objectWidth)
		{
			int result = (int)(((float)areaWidth / 2f) - ((float)objectWidth / 2f));
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CenteredTop																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the top coordinate of an object centered within an area.
		/// </summary>
		/// <param name="areaHeight">
		/// Height of the area in which the object is centered.
		/// </param>
		/// <param name="objectHeight">
		/// Height of the object to be placed.
		/// </param>
		/// <returns>
		/// The top coordinate of the centered object.
		/// </returns>
		public static int CenteredTop(int areaHeight, int objectHeight)
		{
			int result =
				(int)(((float)areaHeight / 2f) - ((float)objectHeight / 2f));
			return result;
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* CircleGetVertices																											*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return a list of vertices for a circle of the specified size centered
		///// at the provided location.
		///// </summary>
		///// <param name="center">
		///// Reference to the point at which the circle is centered.
		///// </param>
		///// <param name="radius">
		///// Radius of the circle.
		///// </param>
		///// <param name="vertexCount">
		///// Count of vertices to return.
		///// </param>
		///// <param name="thetaOffset">
		///// The optional rotation of the shape, in radians.
		///// </param>
		///// <returns>
		///// Reference to a list of evenly-spaced vertices around the edge of the
		///// indicated circle, if valid. Otherwise, an empty list.
		///// </returns>
		//public static List<FVector2> CircleGetVertices(FVector2 center, float radius,
		//	int vertexCount, float thetaOffset = 0f)
		//{
		//	float angle = thetaOffset;
		//	int index = 0;
		//	List<FVector2> result = new List<FVector2>();
		//	float space = 0f;
		//	//float spaceCount = 0f;

		//	if(center != null && radius > 0f && vertexCount > 0)
		//	{
		//		angle = thetaOffset;
		//		//spaceCount = (float)vertexCount - 1f;
		//		//space = GeometryUtil.TwoPi / spaceCount;
		//		space = GeometryUtil.TwoPi / (float)vertexCount;
		//		for(index = 0; index < vertexCount; index ++)
		//		{
		//			result.Add(Trig.GetDestPoint(center, angle, radius));
		//			angle += space;
		//		}
		//	}
		//	return result;
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clamp																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clamp the supplied value within the allowed minimum and maximum values.
		/// </summary>
		/// <param name="value">
		/// The value to clamp.
		/// </param>
		/// <param name="minimum">
		/// The minimum allowable value.
		/// </param>
		/// <param name="maximum">
		/// The maximum allowable value.
		/// </param>
		/// <returns>
		/// The caller's clamped value.
		/// </returns>
		public static float Clamp(float value,
			float minimum = 0f, float maximum = 1f)
		{
			return Math.Max(minimum, Math.Min(value, maximum));
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Clamp the supplied value within the allowed minimum and maximum values.
		/// </summary>
		/// <param name="value">
		/// The value to clamp.
		/// </param>
		/// <param name="minimum">
		/// The minimum allowable value.
		/// </param>
		/// <param name="maximum">
		/// The maximum allowable value.
		/// </param>
		/// <returns>
		/// The caller's clamped value.
		/// </returns>
		public static int Clamp(int value,
			int minimum = 0, int maximum = 1)
		{
			return Math.Max(minimum, Math.Min(value, maximum));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clear																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clear the contents of the specified string builder.
		/// </summary>
		/// <param name="builder">
		/// Reference to the string builder to clear.
		/// </param>
		public static void Clear(StringBuilder builder)
		{
			if(builder?.Length > 0)
			{
				builder.Remove(0, builder.Length);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ConfigProfile																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ConfigProfile">ConfigProfile</see>.
		/// </summary>
		private static ShopToolsConfigItem mConfigProfile = null;
		/// <summary>
		/// Get/Set a reference to the loaded configuration for this session.
		/// </summary>
		public static ShopToolsConfigItem ConfigProfile
		{
			get { return mConfigProfile; }
			set { mConfigProfile = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ConfigurationFilename																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="ConfigurationFilename">ConfigurationFilename</see>.
		/// </summary>
		private static string mConfigurationFilename = "";
		/// <summary>
		/// Get/Set the path and filename of the configuration file that was loaded
		/// for this session.
		/// </summary>
		public static string ConfigurationFilename
		{
			get { return mConfigurationFilename; }
			set { mConfigurationFilename = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ConvertRange																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Convert from one numeric range to another.
		/// </summary>
		/// <param name="value">
		/// The value to convert.
		/// </param>
		/// <param name="fromMin">
		/// Original range minimum limit.
		/// </param>
		/// <param name="fromMax">
		/// Original range maximum limit.
		/// </param>
		/// <param name="toMin">
		/// New range minimum limit.
		/// </param>
		/// <param name="toMax">
		/// New range maximum limit.
		/// </param>
		/// <returns>
		/// Specified value, converted to the new range.
		/// </returns>
		public static float ConvertRange(float value,
			float fromMin, float fromMax, float toMin, float toMax)
		{
			float fromRange = (fromMax - fromMin);
			float result = 0;
			float toRange = 0f;

			if(fromRange == 0f)
			{
				result = toMin;
			}
			else
			{
				toRange = (toMax - toMin);
				result = (((value - fromMin) * toRange) / fromRange) + toMin;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ConvertToRelative																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a collection of points that are relative to the supplied
		/// absolute points.
		/// </summary>
		/// <param name="points">
		/// Reference to the collection of absolute points to be converted to
		/// relative.
		/// </param>
		/// <param name="closeShape">
		/// Optional value indicating whether to close the shape. Default = true.
		/// </param>
		/// <returns>
		/// Reference to a newly created collection of points with meaurements
		/// relative to the caller's reference values.
		/// </returns>
		public static List<FVector2> ConvertToRelative(List<FVector2> points,
			bool closeShape = true)
		{
			FVector2 point = null;
			List<FVector2> result = new List<FVector2>();

			if(points?.Count > 0)
			{
				point = new FVector2();
				foreach(FVector2 pointItem in points)
				{
					result.Add(new FVector2()
					{
						X = pointItem.X - point.X,
						Y = pointItem.Y - point.Y
					});
					point = pointItem;
				}
				if(closeShape)
				{
					result.Add(new FVector2()
					{
						X = points[0].X - point.X,
						Y = points[0].Y - point.Y
					});
				}
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a collection of points that are relative to the supplied
		/// absolute points.
		/// </summary>
		/// <param name="points">
		/// Reference to the collection of absolute points to be converted to
		/// relative.
		/// </param>
		/// <param name="closeShape">
		/// Optional value indicating whether to close the shape. Default = true.
		/// </param>
		/// <returns>
		/// Reference to a newly created collection of points with meaurements
		/// relative to the caller's reference values.
		/// </returns>
		public static PlotPointCollection ConvertToRelative(
			PlotPointCollection points, bool closeShape = true)
		{
			int index = 0;
			PlotPointItem point = null;
			PlotPointCollection result = new PlotPointCollection();

			if(points?.Count > 0)
			{
				point = new PlotPointItem();
				foreach(PlotPointItem pointItem in points)
				{
					result.Add(new PlotPointItem()
					{
						PenStatus = (index == 0 ?
							PlotPointPenStatus.PenUp : pointItem.PenStatus),
						//PenStatus = pointItem.PenStatus,
						Point = new FVector2(
							pointItem.Point.X - point.Point.X,
							pointItem.Point.Y - point.Point.Y)
					});
					point = pointItem;
					index++;
				}
				if(closeShape)
				{
					result.Add(new PlotPointItem()
					{
						PenStatus = points[points.Count - 1].PenStatus,
						Point = new FVector2(
							points[0].Point.X - point.Point.X,
							points[0].Point.Y - point.Point.Y)
					});
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DeepClone																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a complete, recursive deep clone of the provided object.
		/// </summary>
		/// <typeparam name="T">
		/// The type to be cloned.
		/// </typeparam>
		/// <param name="source">
		/// Reference to the source object to be cloned.
		/// </param>
		/// <returns>
		/// Reference to the newly cloned object, where all values
		/// have been duplicated on a primitive level.
		/// </returns>
		public static T DeepClone<T>(T source)
		{
			string content = "";
			T result = default(T);

			if(source != null)
			{
				content = JsonConvert.SerializeObject(source);
				result = JsonConvert.DeserializeObject<T>(content);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DeepTransfer																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Conduct a deep transfer of all of the lowest level values from the
		/// source object to the target.
		/// </summary>
		/// <typeparam name="T">
		/// The type to be cloned.
		/// </typeparam>
		/// <param name="source">
		/// Reference to the source object whose values will be cloned.
		/// </param>
		/// <param name="target">
		/// Reference to the target object whose values will be updated at a base
		/// level.
		/// </param>
		public static void DeepTransfer<T>(T source, T target)
		{
			string content = "";

			if(source != null && target != null)
			{
				content = JsonConvert.SerializeObject(source);
				JsonConvert.PopulateObject(content, target);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawEllipseCenterDiameter																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw an ellipse using a center coordinate with X and Y diameters.
		/// </summary>
		/// <param name="centerOffset">
		/// Reference to the coordinate at the center of the circle.
		/// </param>
		/// <param name="diameterX">
		/// The X diameter of the ellipse.
		/// </param>
		/// <param name="diameterY">
		/// The Y diameter of the ellipse.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		/// <param name="pen">
		/// Reference to the active pen.
		/// </param>
		/// <param name="workspaceArea">
		/// The local drawing workspace in which the shape will be drawn.
		/// </param>
		/// <param name="scale">
		/// The scale to apply to the shape.
		/// </param>
		public static void DrawEllipseCenterDiameter(FVector2 centerOffset,
			float diameterX, float diameterY, Graphics graphics, Pen pen,
			Rectangle workspaceArea, float scale)
		{
			int iDiameterX = 0;
			int iDiameterY = 0;
			int x = 0;
			int y = 0;

			if(centerOffset != null && graphics != null && pen != null)
			{
				if(diameterX != 0f)
				{
					iDiameterX = (int)(diameterX * scale);
				}
				if(diameterY != 0f)
				{
					iDiameterY = (int)(diameterY * scale);
				}
				x =
					(int)((centerOffset.X - (diameterX / 2f)) * scale) + workspaceArea.X;
				y =
					(int)((centerOffset.Y - (diameterY / 2f)) * scale) + workspaceArea.Y;
				graphics.DrawEllipse(pen,
					new Rectangle(x, y, iDiameterX, iDiameterY));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawEllipseCenterRadius																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw an ellipse using a center coordinate with X and Y diameters.
		/// </summary>
		/// <param name="centerOffset">
		/// Reference to the coordinate at the center of the circle.
		/// </param>
		/// <param name="radiusX">
		/// The X radius of the circle.
		/// </param>
		/// <param name="radiusY">
		/// The Y radius of the circle.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		/// <param name="pen">
		/// Reference to the active pen.
		/// </param>
		/// <param name="workspaceArea">
		/// The local drawing workspace in which the shape will be drawn.
		/// </param>
		/// <param name="scale">
		/// The scale to apply to the shape.
		/// </param>
		public static void DrawEllipseCenterRadius(FVector2 centerOffset,
			float radiusX, float radiusY, Graphics graphics, Pen pen,
			Rectangle workspaceArea, float scale)
		{
			int iRadiusX2 = 0;
			int iRadiusY2 = 0;
			int x = 0;
			int y = 0;

			if(centerOffset != null && graphics != null && pen != null)
			{
				if(radiusX != 0f)
				{
					iRadiusX2 = (int)(radiusX * 2f * scale);
				}
				if(radiusY != 0f)
				{
					iRadiusY2 = (int)(radiusY * 2f * scale);
				}
				x = (int)((centerOffset.X - radiusX) * scale) + workspaceArea.X;
				y = (int)((centerOffset.Y - radiusY) * scale) + workspaceArea.Y;
				graphics.DrawEllipse(pen,
					new Rectangle(x, y, iRadiusX2, iRadiusY2));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawEllipseDiameter																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw an ellipse using the upper left coordinate with X and Y diameters.
		/// </summary>
		/// <param name="start">
		/// Reference to the coordinate at upper left corner.
		/// </param>
		/// <param name="diameterX">
		/// The X diameter of the circle.
		/// </param>
		/// <param name="diameterY">
		/// The Y diameter of the circle.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		/// <param name="pen">
		/// Reference to the active pen.
		/// </param>
		/// <param name="workspaceArea">
		/// The local drawing workspace in which the shape will be drawn.
		/// </param>
		/// <param name="scale">
		/// The scale to apply to the shape.
		/// </param>
		public static void DrawEllipseDiameter(FVector2 start,
			float diameterX, float diameterY, Graphics graphics, Pen pen,
			Rectangle workspaceArea, float scale)
		{
			int iDiameterX = 0;
			int iDiameterY = 0;
			int x = 0;
			int y = 0;

			if(start != null && graphics != null && pen != null)
			{
				if(diameterX != 0f)
				{
					iDiameterX = (int)(diameterX * scale);
				}
				if(diameterY != 0f)
				{
					iDiameterY = (int)(diameterY * scale);
				}
				x = (int)(start.X * scale) + workspaceArea.X;
				y = (int)(start.Y * scale) + workspaceArea.Y;
				graphics.DrawEllipse(pen,
					new Rectangle(x, y, iDiameterX, iDiameterY));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawEllipseRadius																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw an ellipse using the upper left coordinate with X and Y radii.
		/// </summary>
		/// <param name="start">
		/// Reference to the coordinate at the upper left corner of the circle.
		/// </param>
		/// <param name="radiusX">
		/// The X radius of the circle.
		/// </param>
		/// <param name="radiusY">
		/// The Y radius of the circle.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		/// <param name="pen">
		/// Reference to the active pen.
		/// </param>
		/// <param name="workspaceArea">
		/// The local drawing workspace in which the shape will be drawn.
		/// </param>
		/// <param name="scale">
		/// The scale to apply to the shape.
		/// </param>
		public static void DrawEllipseRadius(FVector2 start,
			float radiusX, float radiusY, Graphics graphics, Pen pen,
			Rectangle workspaceArea, float scale)
		{
			int iRadiusX2 = 0;
			int iRadiusY2 = 0;
			int x = 0;
			int y = 0;

			if(start != null && graphics != null && pen != null)
			{
				if(radiusX != 0f)
				{
					iRadiusX2 = (int)(radiusX * 2f * scale);
				}
				if(radiusY != 0f)
				{
					iRadiusY2 = (int)(radiusY * 2f * scale);
				}
				x = (int)(start.X * scale) + workspaceArea.X;
				y = (int)(start.Y * scale) + workspaceArea.Y;
				graphics.DrawEllipse(pen,
					new Rectangle(x, y, iRadiusX2, iRadiusY2));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawHole																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw the drill hole symbol.
		/// </summary>
		/// <param name="displayLocation">
		/// Reference to the raw display-space location at which the symbol will
		/// be centered.
		/// </param>
		/// <param name="graphics">
		/// Reference to the currently active graphics device.
		/// </param>
		/// <param name="workspaceArea">
		/// Workspace area representing the client offset from the control.
		/// </param>
		/// <param name="scale">
		/// Scale to apply to the positioning of the symbol on the drawing area.
		/// </param>
		/// <param name="selected">
		/// Value indicating whether the hole and its offset should be drawn as
		/// selected.
		/// </param>
		public static void DrawHole(
			FVector2 displayLocation, Graphics graphics,
			Rectangle workspaceArea, float scale, bool selected = false)
		{
			Rectangle targetArea = new Rectangle(0, 0, 12, 12);
			Brush targetBackgroundBrush =
				new SolidBrush(ColorTranslator.FromHtml(mColorTargetBackgroundBrush));
			Pen targetBorderPen = null;
			int x1 = 0;
			int x2 = 0;
			int y1 = 0;
			int y2 = 0;

			if(displayLocation != null && graphics != null)
			{
				if(selected)
				{
					targetBorderPen =
						new Pen(ColorTranslator.FromHtml(mColorTargetPenSelected), 1f);
				}
				else
				{
					targetBorderPen =
						new Pen(ColorTranslator.FromHtml(mColorTargetPen), 1f);
				}
				targetArea.X =
					workspaceArea.Left +
						(int)(displayLocation.X * scale) - (targetArea.Width / 2);
				targetArea.Y =
					workspaceArea.Top +
						(int)(displayLocation.Y * scale) - (targetArea.Height / 2);
				graphics.FillEllipse(targetBackgroundBrush, targetArea);
				graphics.DrawEllipse(targetBorderPen, targetArea);
				x1 = x2 = targetArea.Left + (targetArea.Width / 2);
				y1 = targetArea.Top;
				y2 = targetArea.Bottom;
				graphics.DrawLine(targetBorderPen,
					new Point(x1, y1), new Point(x2, y2));
				x1 = targetArea.Left;
				x2 = targetArea.Right;
				y1 = y2 = targetArea.Top + (targetArea.Height / 2);
				graphics.DrawLine(targetBorderPen,
					new Point(x1, y1), new Point(x2, y2));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawLine																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw a line using floating point coordinates, scaling, and translation.
		/// </summary>
		/// <param name="start">
		/// Raw starting coordinate.
		/// </param>
		/// <param name="end">
		/// Raw ending coordinate.
		/// </param>
		/// <param name="graphics">
		/// Reference to the graphics device to which the line will be painted.
		/// </param>
		/// <param name="pen">
		/// Reference to the pen used to draw the line.
		/// </param>
		/// <param name="workspaceArea">
		/// Target workspace on the current canvas.
		/// </param>
		/// <param name="scale">
		/// Scale to apply to the points.
		/// </param>
		public static void DrawLine(FVector2 start, FVector2 end,
			Graphics graphics, Pen pen, Rectangle workspaceArea, float scale)
		{
			int x1 = 0;
			int x2 = 0;
			int y1 = 0;
			int y2 = 0;

			if(start != null && end != null && graphics != null && pen != null)
			{
				x1 = (int)(start.X * scale) + workspaceArea.X;
				y1 = (int)(start.Y * scale) + workspaceArea.Y;
				x2 = (int)(end.X * scale) + workspaceArea.X;
				y2 = (int)(end.Y * scale) + workspaceArea.Y;
				graphics.DrawLine(pen,
					new Point(x1, y1), new Point(x2, y2));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawOperation																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw the specified operation to the provided graphics context.
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
		/// <param name="g">
		/// Reference to the current graphics device.
		/// </param>
		/// <param name="workspaceArea">
		/// Target workspace on the current canvas.
		/// </param>
		/// <param name="scale">
		/// Precalculated scale of drawing space to canvas space.
		/// </param>
		/// <param name="selected">
		/// Optional value indicating whether the operation is selected.
		/// </param>
		/// <returns>
		/// Reference to the location at which the current operation will end,
		/// in system units.
		/// </returns>
		public static FVector2 DrawOperation(
			PatternOperationItem operation,
			WorkpieceInfoItem workpiece, FVector2 startingLocation,
			string previousToolName, Graphics g, Rectangle workspaceArea,
			float scale, bool selected = false)
		{
			float angleStart = 0f;
			float angleSweep = 0f;
			Rectangle area = Rectangle.Empty;
			Brush brush = null;
			Pen drawPen = null;
			Point endCoordinate = Point.Empty;
			int height = 0;
			Pen movePen = null;
			Pen[] penSet = null;
			FVector2 result = new FVector2(startingLocation);
			Point startCoordinate = Point.Empty;
			int width = 0;

			if(operation != null && workpiece != null)
			{
				brush = GetFillBrush(selected);
				penSet = GetPens(selected);
				movePen = penSet[(int)PenSetEnum.Move];
				drawPen = penSet[(int)PenSetEnum.Draw];
				foreach(OperationLayoutItem layoutItem in operation.LayoutElements)
				{
					switch(layoutItem.ActionType)
					{
						case LayoutActionType.DrawArc:
							startCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayStartOffset, workspaceArea, scale);
							endCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayEndOffset, workspaceArea, scale);
							angleStart = Trig.RadToDeg(layoutItem.StartAngle);
							angleSweep = Trig.RadToDeg(
								layoutItem.SweepAngle - layoutItem.StartAngle);
							width = endCoordinate.X - startCoordinate.X;
							height = endCoordinate.Y - startCoordinate.Y;
							area = new Rectangle(
								startCoordinate.X, startCoordinate.Y, width, height);
							g.DrawArc(drawPen, area, angleStart, angleStart);
							FVector2.TransferValues(layoutItem.ToolEndOffset, result);
							break;
						case LayoutActionType.DrawEllipse:
							startCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayStartOffset, workspaceArea, scale);
							endCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayEndOffset, workspaceArea, scale);
							width = endCoordinate.X - startCoordinate.X;
							height = endCoordinate.Y - startCoordinate.Y;
							area = new Rectangle(
								startCoordinate.X, startCoordinate.Y, width, height);
							g.DrawEllipse(drawPen, area);
							FVector2.TransferValues(layoutItem.ToolEndOffset, result);
							break;
						case LayoutActionType.DrawLine:
							startCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayStartOffset, workspaceArea, scale);
							endCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayEndOffset, workspaceArea, scale);
							g.DrawLine(drawPen, startCoordinate, endCoordinate);
							FVector2.TransferValues(layoutItem.ToolEndOffset, result);
							break;
						case LayoutActionType.DrawRectangle:
							startCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayStartOffset, workspaceArea, scale);
							endCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayEndOffset, workspaceArea, scale);
							width = endCoordinate.X - startCoordinate.X;
							height = endCoordinate.Y - startCoordinate.Y;
							area = new Rectangle(
								startCoordinate.X, startCoordinate.Y, width, height);
							g.DrawRectangle(drawPen, area);
							FVector2.TransferValues(layoutItem.ToolEndOffset, result);
							break;
						case LayoutActionType.FillEllipse:
							startCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayStartOffset, workspaceArea, scale);
							endCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayEndOffset, workspaceArea, scale);
							width = endCoordinate.X - startCoordinate.X;
							height = endCoordinate.Y - startCoordinate.Y;
							area = new Rectangle(
								startCoordinate.X, startCoordinate.Y, width, height);
							g.FillEllipse(brush, area);
							g.DrawEllipse(drawPen, area);
							FVector2.TransferValues(layoutItem.ToolEndOffset, result);
							break;
						case LayoutActionType.FillRectangle:
							startCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayStartOffset, workspaceArea, scale);
							endCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayEndOffset, workspaceArea, scale);
							width = endCoordinate.X - startCoordinate.X;
							height = endCoordinate.Y - startCoordinate.Y;
							area = new Rectangle(
								startCoordinate.X, startCoordinate.Y, width, height);
							g.FillRectangle(brush, area);
							g.DrawRectangle(drawPen, area);
							FVector2.TransferValues(layoutItem.ToolEndOffset, result);
							break;
						case LayoutActionType.MoveExplicit:
						case LayoutActionType.MoveImplicit:
							startCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayStartOffset, workspaceArea, scale);
							endCoordinate = GetDisplayCoordinate(
								layoutItem.DisplayEndOffset, workspaceArea, scale);
							g.DrawLine(movePen, startCoordinate, endCoordinate);
							FVector2.TransferValues(layoutItem.ToolEndOffset, result);
							break;
						case LayoutActionType.Point:
							DrawHole(layoutItem.DisplayStartOffset,
								g, workspaceArea, scale, selected);
							FVector2.TransferValues(layoutItem.ToolEndOffset, result);
							break;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawRectangle																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw a rectangle using the upper left coordinate with length and width.
		/// </summary>
		/// <param name="start">
		/// Reference to the coordinate at upper left corner.
		/// </param>
		/// <param name="length">
		/// The length of the rectangle.
		/// </param>
		/// <param name="width">
		/// The width of the rectangle.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		/// <param name="pen">
		/// Reference to the active pen.
		/// </param>
		/// <param name="workspaceArea">
		/// The local drawing workspace in which the shape will be drawn.
		/// </param>
		/// <param name="scale">
		/// The scale to apply to the shape.
		/// </param>
		public static void DrawRectangle(FVector2 start,
			float length, float width, Graphics graphics, Pen pen,
			Rectangle workspaceArea, float scale)
		{
			int iHeight = 0;
			int iWidth = 0;
			int x = 0;
			int y = 0;

			if(start != null && graphics != null && pen != null)
			{
				if(LengthIsX())
				{
					if(length != 0f)
					{
						iWidth = (int)(length * scale);
					}
					if(width != 0f)
					{
						iHeight = (int)(width * scale);
					}
				}
				else
				{
					if(length != 0f)
					{
						iHeight = (int)(length * scale);
					}
					if(width != 0f)
					{
						iWidth = (int)(width * scale);
					}
				}
				x = (int)(start.X * scale) + workspaceArea.X;
				y = (int)(start.Y * scale) + workspaceArea.Y;
				graphics.DrawRectangle(pen,
					new Rectangle(x, y, iWidth, iHeight));
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Draw a rectangle using the upper left coordinate with length and width.
		/// </summary>
		/// <param name="start">
		/// Reference to the coordinate at upper left corner.
		/// </param>
		/// <param name="end">
		/// Reference to the end coordinate of the rectangle.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		/// <param name="pen">
		/// Reference to the active pen.
		/// </param>
		/// <param name="workspaceArea">
		/// The local drawing workspace in which the shape will be drawn.
		/// </param>
		/// <param name="scale">
		/// The scale to apply to the shape.
		/// </param>
		public static void DrawRectangle(FVector2 start,
			FVector2 end, Graphics graphics, Pen pen,
			Rectangle workspaceArea, float scale)
		{
			int x1 = 0;
			int x2 = 0;
			int y1 = 0;
			int y2 = 0;

			if(start != null && end != null && graphics != null && pen != null)
			{
				x1 = (int)(start.X * scale) + workspaceArea.X;
				y1 = (int)(start.Y * scale) + workspaceArea.Y;
				x2 = (int)(end.X * scale) + workspaceArea.X;
				y2 = (int)(end.Y * scale) + workspaceArea.Y;
				graphics.DrawRectangle(pen,
					new Rectangle(x1, y1, x2 - x1, y2 - y1));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawRouter																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw the starting or ending router symbol.
		/// </summary>
		/// <param name="displayLocation">
		/// Reference to the raw display-space location at which the symbol will
		/// be centered.
		/// </param>
		/// <param name="startEnd">
		/// The type of symbol to draw: Start or End.
		/// </param>
		/// <param name="graphics">
		/// Reference to the currently active graphics device.
		/// </param>
		/// <param name="workspaceArea">
		/// Workspace area representing the client offset from the control.
		/// </param>
		/// <param name="scale">
		/// Scale to apply to the positioning of the symbol on the drawing area.
		/// </param>
		public static void DrawRouter(
			FVector2 displayLocation, StartEndEnum startEnd, Graphics graphics,
			Rectangle workspaceArea, float scale)
		{
			Rectangle targetArea = new Rectangle(0, 0, 16, 16);
			Brush targetBackgroundBrush =
				new SolidBrush(ColorTranslator.FromHtml(mColorRouterBackgroundBrush));
			Pen targetBorderPen = null;
			int x1 = 0;
			int x2 = 0;
			int y1 = 0;
			int y2 = 0;

			if(displayLocation != null && graphics != null)
			{
				switch(startEnd)
				{
					case StartEndEnum.End:
						targetBorderPen =
							new Pen(ColorTranslator.FromHtml(mColorRouterEndPen), 2f);
						break;
					case StartEndEnum.Start:
					default:
						targetBorderPen =
							new Pen(ColorTranslator.FromHtml(mColorRouterStartPen), 2f);
						break;
				}
				targetArea.X =
					workspaceArea.Left +
						(int)(displayLocation.X * scale) - (targetArea.Width / 2);
				targetArea.Y =
					workspaceArea.Top +
						(int)(displayLocation.Y * scale) - (targetArea.Height / 2);
				graphics.FillEllipse(targetBackgroundBrush, targetArea);
				graphics.DrawEllipse(targetBorderPen, targetArea);
				x1 = x2 = targetArea.Left + (targetArea.Width / 2);
				y1 = targetArea.Top;
				y2 = targetArea.Bottom;
				graphics.DrawLine(targetBorderPen,
					new Point(x1, y1), new Point(x2, y2));
				x1 = targetArea.Left;
				x2 = targetArea.Right;
				y1 = y2 = targetArea.Top + (targetArea.Height / 2);
				graphics.DrawLine(targetBorderPen,
					new Point(x1, y1), new Point(x2, y2));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawTable																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw the work table and the workpiece.
		/// </summary>
		/// <param name="control">
		/// Reference to the panel control being painted.
		/// </param>
		/// <param name="workpieceInfo">
		/// Information about the workpiece and its cuts.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		public static void DrawTable(Panel control,
			WorkpieceInfoItem workpieceInfo,
			Graphics graphics)
		{
			Brush backgroundBrush =
				new SolidBrush(ColorTranslator.FromHtml(mColorScreenBackground));
			Pen borderPen =
				new Pen(ColorTranslator.FromHtml(mColorWorkspaceBorderPen), 2f);
			int index = 0;
			Rectangle offsetArea = Rectangle.Empty;
			FVector2 offsetLocation = new FVector2();
			float scale = 1f;
			string[] shadowLevels = new string[]
			{
				"af",
				"7f",
				"5f",
				"3f",
				"1f"
			};
			Pen shadowPen = new Pen(Color.Black);
			SizeF systemSize = GetSystemSize();
			Rectangle targetArea = new Rectangle(0, 0, 16, 16);
			//Brush targetBackgroundBrush =
			//	new SolidBrush(ColorTranslator.FromHtml("#7fffffff"));
			//Pen targetBorderPen =
			//	new Pen(ColorTranslator.FromHtml("#f0007f00"), 2f);
			Pen workpieceBorderPen =
				new Pen(
					new SolidBrush(
						ColorTranslator.FromHtml(mColorWorkpieceBorderPen)), 2f);
			Brush workpieceBrush =
				new SolidBrush(ColorTranslator.FromHtml(mColorWorkpieceBrush));
			Rectangle workspaceArea = Rectangle.Empty;
			Brush workspaceBrush =
				new SolidBrush(ColorTranslator.FromHtml(mColorWorkspaceBackground));
			Size workspaceSize = Size.Empty;
			float workspaceRatio = GetWorkspaceRatio();
			int x1 = 0;
			int x2 = 0;
			int y1 = 0;
			int y2 = 0;

			if(control != null && graphics != null)
			{
				//Debug.WriteLine("Paint Workspace...");
				graphics.CompositingMode = CompositingMode.SourceOver;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.Bicubic;
				graphics.SmoothingMode = SmoothingMode.AntiAlias;

				//	Clear the background.
				graphics.FillRectangle(backgroundBrush,
					new RectangleF(0, 0, control.Width, control.Height));
				//	Draw the shadow for the workspace.
				workspaceSize = ResizeArea(
					control.Width - 16, control.Height - 16, workspaceRatio);
				scale = (float)workspaceSize.Width / systemSize.Width;
				workspaceArea = new Rectangle(
					CenteredLeft(control.Width, workspaceSize.Width),
					CenteredTop(control.Height, workspaceSize.Height),
					workspaceSize.Width, workspaceSize.Height);
				//Debug.WriteLine($"Workspace: {workspaceArea}");
				//	Draw the shadow.
				for(index = 4; index > 0; index--)
				{
					offsetArea = OffsetArea(workspaceArea, index, index);
					shadowPen.Color =
						ColorTranslator.FromHtml($"#{shadowLevels[index]}000000");
					graphics.DrawLine(shadowPen,
						new Point(offsetArea.Left, offsetArea.Bottom),
						new Point(offsetArea.Right, offsetArea.Bottom));
					graphics.DrawLine(shadowPen,
						new Point(offsetArea.Right, offsetArea.Top),
						new Point(offsetArea.Right, offsetArea.Bottom));
				}
				//	Draw the workspace.
				graphics.FillRectangle(workspaceBrush, workspaceArea);
				graphics.DrawRectangle(borderPen, workspaceArea);
				//	Draw the workpiece.
				if(FArea.HasVolume(workpieceInfo.Area))
				{
					//offsetArea =
					//	TransformToDisplay(workspaceArea, mSessionWorkpieceInfo);
					offsetArea = new Rectangle(
						workspaceArea.Left + (int)(workpieceInfo.Area.Left * scale),
						workspaceArea.Top + (int)(workpieceInfo.Area.Top * scale),
						(int)(workpieceInfo.Area.Width * scale),
						(int)(workpieceInfo.Area.Height * scale));
					graphics.FillRectangle(workpieceBrush, offsetArea);
					graphics.DrawRectangle(workpieceBorderPen, offsetArea);
				}
				//	X pattern at center.
				shadowPen.Color =
					ColorTranslator.FromHtml($"#7f7f7f");
				x1 = x2 = workspaceArea.Left + (workspaceArea.Width / 2);
				y1 = workspaceArea.Top;
				y2 = workspaceArea.Bottom;
				graphics.DrawLine(shadowPen,
					new Point(x1, y1),
					new Point(x2, y2));
				x1 = workspaceArea.Left;
				x2 = workspaceArea.Right;
				y1 = y2 = workspaceArea.Top + (workspaceArea.Height / 2);
				graphics.DrawLine(shadowPen,
					new Point(x1, y1),
					new Point(x2, y2));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DumpPoints																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Dump the coordinates of the points to the local trace channel.
		/// </summary>
		/// <param name="points">
		/// Reference to the collection of points to dump.
		/// </param>
		public static void DumpPoints(PlotPointCollection points)
		{
			if(points?.Count > 0)
			{
				foreach(PlotPointItem plotPointItem in points)
				{
					Trace.Write(plotPointItem.PenStatus == PlotPointPenStatus.PenUp ?
						"U" : "D");
					Trace.Write(": ");
					Trace.WriteLine(plotPointItem.Point);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ExpandCamelCase																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Expand a camel-case value to alphanumeric with spaces.
		/// </summary>
		/// <param name="value">
		/// Value to expand.
		/// </param>
		/// <returns>
		/// Alphanumeric string with spaces.
		/// </returns>
		public static string ExpandCamelCase(string value)
		{
			StringBuilder builder = new StringBuilder();
			char charc = '\0';
			char[] chars = null;
			int count = 0;
			int index = 0;

			if(value?.Length > 0)
			{
				chars = value.ToCharArray();
				count = chars.Length;
				for(index = 0; index < count; index++)
				{
					charc = chars[index];
					if(index == 0)
					{
						//	Capitalize first item.
						builder.Append(char.ToUpper(charc));
					}
					else if(char.IsUpper(charc))
					{
						builder.Append(' ');
						builder.Append(charc);
					}
					else
					{
						builder.Append(charc);
					}
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FillEllipseCenterDiameter																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw a filled ellipse using a center coordinate with X and Y diameters.
		/// </summary>
		/// <param name="centerOffset">
		/// Reference to the coordinate at the center of the circle.
		/// </param>
		/// <param name="diameterX">
		/// The X diameter of the ellipse.
		/// </param>
		/// <param name="diameterY">
		/// The Y diameter of the ellipse.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		/// <param name="workspaceArea">
		/// The local drawing workspace in which the shape will be drawn.
		/// </param>
		/// <param name="scale">
		/// The scale to apply to the shape.
		/// </param>
		/// <param name="selected">
		/// Optional value indicating whether the shape is selected in the
		/// editor.
		/// </param>
		public static void FillEllipseCenterDiameter(FVector2 centerOffset,
			float diameterX, float diameterY, Graphics graphics,
			Rectangle workspaceArea, float scale, bool selected = false)
		{
			Brush brush = null;
			int iDiameterX = 0;
			int iDiameterY = 0;
			Pen pen = null;
			int x = 0;
			int y = 0;

			if(centerOffset != null && graphics != null)
			{
				if(selected)
				{
					brush = new SolidBrush(ColorTranslator.FromHtml(mColorFillSelected));
					pen = new Pen(ColorTranslator.FromHtml(mColorDrawPenSelected), 2f);
				}
				else
				{
					brush = new SolidBrush(ColorTranslator.FromHtml(mColorFill));
					pen = new Pen(ColorTranslator.FromHtml(mColorDrawPen), 2f);
				}
				if(diameterX != 0f)
				{
					iDiameterX = (int)(diameterX * scale);
				}
				if(diameterY != 0f)
				{
					iDiameterY = (int)(diameterY * scale);
				}
				x =
					(int)((centerOffset.X - (diameterX / 2f)) * scale) + workspaceArea.X;
				y =
					(int)((centerOffset.Y - (diameterY / 2f)) * scale) + workspaceArea.Y;
				graphics.FillEllipse(brush,
					new Rectangle(x, y, iDiameterX, iDiameterY));
				graphics.DrawEllipse(pen,
					new Rectangle(x, y, iDiameterX, iDiameterY));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FillEllipseCenterRadius																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw a filled ellipse using a center coordinate with X and Y diameters.
		/// </summary>
		/// <param name="centerOffset">
		/// Reference to the coordinate at the center of the circle.
		/// </param>
		/// <param name="radiusX">
		/// The X radius of the circle.
		/// </param>
		/// <param name="radiusY">
		/// The Y radius of the circle.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		/// <param name="workspaceArea">
		/// The local drawing workspace in which the shape will be drawn.
		/// </param>
		/// <param name="scale">
		/// The scale to apply to the shape.
		/// </param>
		/// <param name="selected">
		/// Optional value indicating whether the shape is selected in the
		/// editor.
		/// </param>
		public static void FillEllipseCenterRadius(FVector2 centerOffset,
			float radiusX, float radiusY, Graphics graphics,
			Rectangle workspaceArea, float scale, bool selected = false)
		{
			Brush brush = null;
			int iRadiusX2 = 0;
			int iRadiusY2 = 0;
			Pen pen = null;
			int x = 0;
			int y = 0;

			if(centerOffset != null && graphics != null)
			{
				if(selected)
				{
					brush = new SolidBrush(ColorTranslator.FromHtml(mColorFillSelected));
					pen = new Pen(ColorTranslator.FromHtml(mColorDrawPenSelected), 2f);
				}
				else
				{
					brush = new SolidBrush(ColorTranslator.FromHtml(mColorFill));
					pen = new Pen(ColorTranslator.FromHtml(mColorDrawPen), 2f);
				}
				if(radiusX != 0f)
				{
					iRadiusX2 = (int)(radiusX * 2f * scale);
				}
				if(radiusY != 0f)
				{
					iRadiusY2 = (int)(radiusY * 2f * scale);
				}
				x = (int)((centerOffset.X - radiusX) * scale) + workspaceArea.X;
				y = (int)((centerOffset.Y - radiusY) * scale) + workspaceArea.Y;
				graphics.FillEllipse(brush,
					new Rectangle(x, y, iRadiusX2, iRadiusY2));
				graphics.DrawEllipse(pen,
					new Rectangle(x, y, iRadiusX2, iRadiusY2));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FillEllipseDiameter																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw a filled ellipse using the upper left coordinate with X and Y
		/// diameters.
		/// </summary>
		/// <param name="start">
		/// Reference to the coordinate at upper left corner.
		/// </param>
		/// <param name="diameterX">
		/// The X diameter of the circle.
		/// </param>
		/// <param name="diameterY">
		/// The Y diameter of the circle.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		/// <param name="workspaceArea">
		/// The local drawing workspace in which the shape will be drawn.
		/// </param>
		/// <param name="scale">
		/// The scale to apply to the shape.
		/// </param>
		/// <param name="selected">
		/// Optional value indicating whether the shape is selected in the
		/// editor.
		/// </param>
		public static void FillEllipseDiameter(FVector2 start,
			float diameterX, float diameterY, Graphics graphics,
			Rectangle workspaceArea, float scale, bool selected = false)
		{
			Brush brush = null;
			int iDiameterX = 0;
			int iDiameterY = 0;
			Pen pen = null;
			int x = 0;
			int y = 0;

			if(start != null && graphics != null)
			{
				if(selected)
				{
					brush = new SolidBrush(ColorTranslator.FromHtml(mColorFillSelected));
					pen = new Pen(ColorTranslator.FromHtml(mColorDrawPenSelected), 2f);
				}
				else
				{
					brush = new SolidBrush(ColorTranslator.FromHtml(mColorFill));
					pen = new Pen(ColorTranslator.FromHtml(mColorDrawPen), 2f);
				}
				if(diameterX != 0f)
				{
					iDiameterX = (int)(diameterX * scale);
				}
				if(diameterY != 0f)
				{
					iDiameterY = (int)(diameterY * scale);
				}
				x = (int)(start.X * scale) + workspaceArea.X;
				y = (int)(start.Y * scale) + workspaceArea.Y;
				graphics.FillEllipse(brush,
					new Rectangle(x, y, iDiameterX, iDiameterY));
				graphics.DrawEllipse(pen,
					new Rectangle(x, y, iDiameterX, iDiameterY));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FillEllipseRadius																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw a filledellipse using the upper left coordinate with X and Y
		/// radii.
		/// </summary>
		/// <param name="start">
		/// Reference to the coordinate at the upper left corner of the circle.
		/// </param>
		/// <param name="radiusX">
		/// The X radius of the circle.
		/// </param>
		/// <param name="radiusY">
		/// The Y radius of the circle.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		/// <param name="workspaceArea">
		/// The local drawing workspace in which the shape will be drawn.
		/// </param>
		/// <param name="scale">
		/// The scale to apply to the shape.
		/// </param>
		/// <param name="selected">
		/// Optional value indicating whether the shape is selected in the
		/// editor.
		/// </param>
		public static void FillEllipseRadius(FVector2 start,
			float radiusX, float radiusY, Graphics graphics,
			Rectangle workspaceArea, float scale, bool selected = false)
		{
			Brush brush = null;
			int iRadiusX2 = 0;
			int iRadiusY2 = 0;
			Pen pen = null;
			int x = 0;
			int y = 0;

			if(start != null && graphics != null)
			{
				if(selected)
				{
					brush = new SolidBrush(ColorTranslator.FromHtml(mColorFillSelected));
					pen = new Pen(ColorTranslator.FromHtml(mColorDrawPenSelected), 2f);
				}
				else
				{
					brush = new SolidBrush(ColorTranslator.FromHtml(mColorFill));
					pen = new Pen(ColorTranslator.FromHtml(mColorDrawPen), 2f);
				}
				if(radiusX != 0f)
				{
					iRadiusX2 = (int)(radiusX * 2f * scale);
				}
				if(radiusY != 0f)
				{
					iRadiusY2 = (int)(radiusY * 2f * scale);
				}
				x = (int)(start.X * scale) + workspaceArea.X;
				y = (int)(start.Y * scale) + workspaceArea.Y;
				graphics.FillEllipse(brush,
					new Rectangle(x, y, iRadiusX2, iRadiusY2));
				graphics.DrawEllipse(pen,
					new Rectangle(x, y, iRadiusX2, iRadiusY2));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FillRectangle																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fill a rectangle using the upper left coordinate with length and width.
		/// </summary>
		/// <param name="start">
		/// Reference to the coordinate at upper left corner.
		/// </param>
		/// <param name="length">
		/// The length of the rectangle.
		/// </param>
		/// <param name="width">
		/// The width of the rectangle.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		/// <param name="workspaceArea">
		/// The local drawing workspace in which the shape will be drawn.
		/// </param>
		/// <param name="scale">
		/// The scale to apply to the shape.
		/// </param>
		/// <param name="selected">
		/// Optional value indicating whether the shape is selected in the
		/// editor.
		/// </param>
		public static void FillRectangle(FVector2 start,
			float length, float width, Graphics graphics,
			Rectangle workspaceArea, float scale, bool selected = false)
		{
			Brush brush = null;
			int iHeight = 0;
			int iWidth = 0;
			Pen pen = null;
			int x = 0;
			int y = 0;

			if(start != null && graphics != null)
			{
				if(selected)
				{
					brush = new SolidBrush(ColorTranslator.FromHtml(mColorFillSelected));
					pen = new Pen(ColorTranslator.FromHtml(mColorDrawPenSelected));
				}
				else
				{
					brush = new SolidBrush(ColorTranslator.FromHtml(mColorFill));
					pen = new Pen(ColorTranslator.FromHtml(mColorDrawPen));
				}
				if(LengthIsX())
				{
					if(length != 0f)
					{
						iWidth = (int)(length * scale);
					}
					if(width != 0f)
					{
						iHeight = (int)(width * scale);
					}
				}
				else
				{
					if(length != 0f)
					{
						iHeight = (int)(length * scale);
					}
					if(width != 0f)
					{
						iWidth = (int)(width * scale);
					}
				}
				x = (int)(start.X * scale) + workspaceArea.X;
				y = (int)(start.Y * scale) + workspaceArea.Y;
				graphics.FillRectangle(brush,
					new Rectangle(x, y, iWidth, iHeight));
				graphics.DrawRectangle(pen,
					new Rectangle(x, y, iWidth, iHeight));
			}
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Fill a rectangle using the upper left coordinate with length and width.
		/// </summary>
		/// <param name="start">
		/// Reference to the coordinate at upper left corner.
		/// </param>
		/// <param name="end">
		/// Reference to the end coordinate of the rectangle.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		/// <param name="workspaceArea">
		/// The local drawing workspace in which the shape will be drawn.
		/// </param>
		/// <param name="scale">
		/// The scale to apply to the shape.
		/// </param>
		/// <param name="selected">
		/// Optional value indicating whether the shape is selected in the
		/// editor.
		/// </param>
		public static void FillRectangle(FVector2 start,
			FVector2 end, Graphics graphics,
			Rectangle workspaceArea, float scale, bool selected = false)
		{
			Brush brush = null;
			Pen pen = null;
			int x1 = 0;
			int x2 = 0;
			int y1 = 0;
			int y2 = 0;

			if(start != null && end != null && graphics != null)
			{
				if(selected)
				{
					brush = new SolidBrush(ColorTranslator.FromHtml(mColorFillSelected));
					pen = new Pen(ColorTranslator.FromHtml(mColorDrawPenSelected));
				}
				else
				{
					brush = new SolidBrush(ColorTranslator.FromHtml(mColorFill));
					pen = new Pen(ColorTranslator.FromHtml(mColorDrawPen));
				}
				x1 = (int)(start.X * scale) + workspaceArea.X;
				y1 = (int)(start.Y * scale) + workspaceArea.Y;
				x2 = (int)(end.X * scale) + workspaceArea.X;
				y2 = (int)(end.Y * scale) + workspaceArea.Y;
				graphics.FillRectangle(brush,
					new Rectangle(x1, y1, x2 - x1, y2 - y1));
				graphics.DrawRectangle(pen,
					new Rectangle(x1, y1, x2 - x1, y2 - y1));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FromMultiLineString																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Convert a multiline string to a series of limited length lines.
		/// </summary>
		/// <param name="source">
		/// Reference to the source string that might have a single line of
		/// infinite length, or multiple lines.
		/// </param>
		/// <param name="maxLineLength">
		/// Maximum length of any individual line.
		/// </param>
		/// <returns>
		/// Collection of individual string segments, where one or more of those
		/// entries can contribute to a single line of text by including a space
		/// continuation at the end of the segment, or can be composed of multiple
		/// lines, by ending each segment with a non-space character.
		/// </returns>
		public static List<string> FromMultiLineString(string source,
			int maxLineLength = 60)
		{
			StringBuilder builder = new StringBuilder();
			string lineEnd = "";
			MatchCollection matches = null;
			List<string> result = new List<string>();
			string spaces = "";
			string text = "";

			if(source?.Length > 0)
			{
				matches = Regex.Matches(source, ResourceMain.rxWordSpace);
				foreach(Match matchItem in matches)
				{
					text = GetValue(matchItem, "word");
					spaces = GetValue(matchItem, "space");
					lineEnd = GetValue(matchItem, "lineend");
					if(text.Length > 0)
					{
						//	Text was found.
						if(builder.Length > 0 &&
							builder.Length + text.Length + spaces.Length > maxLineLength)
						{
							//	We can't add any more to this line. Create a new line.
							result.Add(builder.ToString());
							Clear(builder);
						}
						if(text.Length + spaces.Length > 0)
						{
							builder.Append(text + spaces);
						}
					}
					else if(lineEnd.Length > 0)
					{
						//	A line end was found.
						result.Add(builder.ToString());
						Clear(builder);
					}
				}
			}
			if(builder.Length > 0)
			{
				result.Add(builder.ToString());
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FromTitleCase																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Convert a string from its title case representation to one with
		/// string separators.
		/// </summary>
		/// <param name="titleCaseString">
		/// The title case string to separate.
		/// </param>
		/// <returns>
		/// A string with separators prior to each capital letter except the first.
		/// </returns>
		public static string FromTitleCase(string titleCaseString)
		{
			char cchar = '\0';
			byte cValue = 0;
			char[] chars = null;
			int count = 0;
			int index = 0;
			StringBuilder builder = new StringBuilder();

			if(titleCaseString?.Length > 0)
			{
				chars = titleCaseString.ToCharArray();
				count = chars.Length;
				for(index = 0; index < count; index ++)
				{
					if(index > 0)
					{
						cchar = chars[index];
						cValue = (byte)cchar;
						if(cValue >= 65 && cValue <= 90)
						{
							builder.Append(' ');
						}
						builder.Append(cchar);
					}
					else
					{
						builder.Append(chars[index]);
					}
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetAltValue																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the formatted alternate measurement value from the caller's
		/// value.
		/// </summary>
		/// <param name="value">
		/// The text value to inspect.
		/// </param>
		/// <param name="normalValue">
		/// The normal value already set for this object.
		/// </param>
		/// <param name="useParethesis">
		/// Optional value indicating whether to place the alternate value in
		/// parenthesis. Default = true.
		/// </param>
		/// <returns>
		/// The formatted alternate measurement value for the supplied text, where
		/// a blank string remains blank, a repeat of the normal value is blank,
		/// a space is replaced by elipsis, and any other value is placed in
		/// parenthesis.
		/// </returns>
		public static string GetAltValue(string value, string normalValue = "",
			bool useParethesis = true)
		{
			string result = "";

			if(value?.Length > 0)
			{
				switch(value)
				{
					case " ":
					case "...":
						result = "...";
						break;
					default:
						if(value.Replace(" ", "") != normalValue.Replace(" ", ""))
						{
							if(useParethesis &&
								!(value.StartsWith("(") && value.EndsWith(")")))
							{
								result = $"({value})";
							}
							else
							{
								result = value;
							}
						}
						break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetAngle																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the binary representation of the caller's angle, in degrees,
		/// after solving for variables and expressions.
		/// </summary>
		/// <param name="angleText">
		/// The freehand angle text to convert to a single binary value, in
		/// degrees.
		/// </param>
		/// <returns>
		/// The binary floating point representation of the caller's specified
		/// angle, after variables and expressions have been solved.
		/// </returns>
		/// <remarks>
		/// In this version, the text can be purely numeric, in which case, it will
		/// be interpreted as degrees, or can have the suffices 'deg', 'degrees',
		/// 'rad', or 'radians'.
		/// </remarks>
		public static float GetAngle(string angleText)
		{
			Match match = null;
			float result = 0f;
			string text = "";

			if(angleText?.Length > 0)
			{
				match = Regex.Match(angleText, ResourceMain.rxAngleUnit);
				if(match.Success)
				{
					result = ToFloat(GetValue(match, "angle"));
					text = GetValue(match, "unit").ToLower();
					switch(text)
					{
						case "rad":
						case "radians":
							//	The result was already expressed in radians.
							break;
						case "deg":
						case "degrees":
						default:
							result = Trig.DegToRad(result);
							break;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetBitmapFromDataUri																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a bitmap image from the specified Data URI.
		/// </summary>
		/// <param name="dataUri">
		/// Reference to the Data URI to parse.
		/// </param>
		/// <returns>
		/// Reference to a new Bitmap image representing the provided Data URI,
		/// if valid. Otherwise, null.
		/// </returns>
		public static Bitmap GetBitmapFromDataUri(string dataUri)
		{
			byte[] bytes = null;
			Match match = null;
			string mimeType = "";
			Bitmap result = null;

			if(IsDataUri(dataUri))
			{
				match = Regex.Match(dataUri, ResourceMain.rxDataUriHeader);
				if(match.Success)
				{
					mimeType = GetValue(match, "mimeType");
					if(mimeType.StartsWith("image/"))
					{
						//	This is image data.
						//	The data starts at the end of the header.
						bytes = Convert.FromBase64String(dataUri.Substring(match.Length));
						using(MemoryStream stream = new MemoryStream(bytes))
						{
							result = new Bitmap(stream);
						}
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetDataUri																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a reference to a Data URI created from the specified binary
		/// file.
		/// </summary>
		/// <param name="file">
		/// Reference to a file to load.
		/// </param>
		/// <returns>
		/// Fully prepared data URI.
		/// </returns>
		public static string GetDataUri(FileInfo file)
		{
			byte[] buffer = null;
			StringBuilder builder = new StringBuilder();

			if(file?.Exists == true)
			{
				buffer = File.ReadAllBytes(file.FullName);
				builder.Append("data:");
				builder.Append(MimeType(file.Extension));
				builder.Append(";base64,");
				builder.Append(Convert.ToBase64String(buffer));
			}
			return builder.ToString();
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return a reference to a Data URI created from the provided binary data
		/// loaded in association with a named file.
		/// </summary>
		/// <param name="extension">
		/// File extension used to establish the MIME type.
		/// </param>
		/// <param name="data">
		/// Binary data to convert.
		/// </param>
		/// <returns>
		/// Fully prepared data URI.
		/// </returns>
		public static string GetDataUri(string extension, byte[] data)
		{
			StringBuilder builder = new StringBuilder();

			if(data?.Length > 0)
			{
				builder.Append("data:");
				builder.Append(MimeType(extension));
				builder.Append(";base64,");
				builder.Append(Convert.ToBase64String(data));
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetDisplayCoordinate																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the scaled representation of the caller's drawing space
		/// coordinate for use on the current display canvas.
		/// </summary>
		/// <param name="offset">
		/// Reference to the drawing space offset to convert.
		/// </param>
		/// <param name="workspaceArea">
		/// The physical workspace onto which the drawing is being projected.
		/// </param>
		/// <param name="scale">
		/// The current scale.
		/// </param>
		/// <returns>
		/// The coordinate on the target canvas cooresponding to the caller's
		/// offset.
		/// </returns>
		public static Point GetDisplayCoordinate(FVector2 offset,
			Rectangle workspaceArea, float scale)
		{
			Point result = Point.Empty;

			if(offset != null && !workspaceArea.IsEmpty)
			{
				result = new Point(
					workspaceArea.X + (int)(offset.X * scale),
					workspaceArea.Y + (int)(offset.Y * scale));
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetFileExtension																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the file extension of the provided filename.
		/// </summary>
		/// <param name="filename">
		/// The filename for which the extension will be returned.
		/// </param>
		/// <returns>
		/// The extension portion of the provided filename, including the dot.
		/// </returns>
		/// <remarks>
		/// This version of the method interprets all dots in the name as being
		/// included in the file extension, providing for the multi-extension
		/// concept.
		/// </remarks>
		public static string GetFileExtension(string filename)
		{
			int index = 0;
			string result = "";

			if(filename?.Length > 0 && filename.IndexOf('.') > -1)
			{
				index = filename.IndexOf('.');
				result = filename.Substring(index);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetFilenameFriendly																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a filename friendly version of the caller's freeform text.
		/// </summary>
		/// <param name="text">
		/// Freeform text, as used to name a tool or write a remark.
		/// </param>
		/// <returns>
		/// The filename-friendly version of the caller's text.
		/// </returns>
		public static string GetFilenameFriendly(string text)
		{
			string[] charRemove = new string[] { " ", "\n", "\r", "\t" };
			string[] charUnderscore = new string[]
			{ "!", "\"", "#", "$", "%", "&", "'", "*", "+", "/", ":", ";",
				"<", "=", ">", "?", "@", "\\", "^", "`", "{", "|", "}", "~" };
			string result = "";

			if(text?.Length > 0)
			{
				result = text;
				foreach(string c in charRemove)
				{
					result = result.Replace(c, "");
				}
				foreach(string c in charUnderscore)
				{
					result = result.Replace(c, "_");
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetFillBrush																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a solid brush used to fill the active area.
		/// </summary>
		/// <param name="selected">
		/// Optional value indicating whether the area will be selected.
		/// Default = false.
		/// </param>
		/// <returns>
		/// Reference to the brush used to fill the active area.
		/// </returns>
		public static Brush GetFillBrush(bool selected = false)
		{
			Brush result = new SolidBrush(
				(selected ?
				ColorTranslator.FromHtml(mColorFillSelected) :
				ColorTranslator.FromHtml(mColorFill)
				));
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetFloatString																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the formally formatted version of the caller's floating point
		/// number.
		/// </summary>
		/// <param name="value">
		/// A value to convert to float then format.
		/// </param>
		/// <returns>
		/// String representation of a valid floating point number, if the caller's
		/// input was legitimate. Otherwise, "0".
		/// </returns>
		public static string GetFloatString(string value)
		{
			float result = ToFloat(value);

			return result.ToString("0.###");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetIntegerString																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the formally formatted version of the caller's integer number.
		/// </summary>
		/// <param name="value">
		/// A value to convert to integer then format.
		/// </param>
		/// <returns>
		/// String representation of a valid integer number, if the caller's
		/// input was legitimate. Otherwise, "0".
		/// </returns>
		public static string GetIntegerString(string value)
		{
			float result = ToInt(value);

			return result.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetMeasurementString																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the full measurement string corresponding to the caller's value
		/// and default unit.
		/// </summary>
		/// <param name="value">
		/// The value, optionally including a specific measurement unit.
		/// </param>
		/// <param name="defaultUnit">
		/// The optional default measurement unit to apply, if no unit was provided.
		/// If this value is blank, the default base unit is used from the
		/// currently loaded configuration profile.
		/// </param>
		/// <returns>
		/// The caller's measurement value and unit.
		/// </returns>
		public static string GetMeasurementString(string value,
			string defaultUnit = "")
		{
			StringBuilder builder = new StringBuilder();
			string unit = "";

			if(value?.Length > 0)
			{
				if(defaultUnit?.Length > 0)
				{
					unit = defaultUnit;
				}
				else
				{
					unit = BaseUnit(mConfigProfile.DisplayUnits);
				}
				switch(unit)
				{
					case "in":
						builder.Append(
							MeasurementProcessor.SumInches(value, unit).ToString("0.###"));
						builder.Append("in");
						break;
					case "mm":
						builder.Append(
							MeasurementProcessor.
								SumMillimeters(value, unit).ToString("0.###"));
						builder.Append("mm");
						break;
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetMillimeters																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the caller's value, represented in system millimeters.
		/// </summary>
		/// <param name="value">
		/// The freehand numerical value to convert.
		/// </param>
		/// <returns>
		/// The binary representation of the caller's value, in millimeters, using
		/// the default base unit of this session, where appropriate.
		/// </returns>
		public static float GetMillimeters(string value)
		{
			return MeasurementProcessor.SumMillimeters(value,
				BaseUnit(mConfigProfile.DisplayUnits));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetNonNumeric																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the first non-numeric portion of the caller's string, not
		/// including spaces.
		/// </summary>
		/// <param name="value">
		/// The user input value to parse.
		/// </param>
		/// <returns>
		/// The first non-numeric portion of the caller's string, not including
		/// spaces.
		/// </returns>
		/// <remarks>
		/// In this version, the non-numeric value trails the numeric, and does
		/// not include spaces.
		/// </remarks>
		public static string GetNonNumeric(string value)
		{
			string result =
				GetValue(value, ResourceMain.rxNumericNonNumeric, "nonnumeric");
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetNumeric																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the first numeric portion of the string.
		/// </summary>
		/// <param name="value">
		/// The user input value to parse.
		/// </param>
		/// <returns>
		/// The first numeric portion of the caller's string, not including
		/// spaces.
		/// </returns>
		public static string GetNumeric(string value)
		{
			string result =
				GetValue(value, ResourceMain.rxNumericNonNumeric, "numeric");

			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetPens																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the pen set for the operation paint appropriate for the
		/// specified selection state.
		/// </summary>
		/// <param name="selected">
		/// Optional value indicating whether the pens are for drawing in the
		/// selected state. Default = true.
		/// </param>
		/// <returns>
		/// Reference to an array of pens for use with the specified parameters.
		/// 0 - Move Pen; 1 - Draw Pen;
		/// </returns>
		public static Pen[] GetPens(bool selected = false)
		{
			Pen[] result = new Pen[2];
			float[] moveDashes = { 4, 4 };

			if(selected)
			{
				//	Move Pen.
				result[(int)PenSetEnum.Move] = new Pen(
					ColorTranslator.FromHtml(mColorMovePenSelected), 2f)
				{
					DashPattern = moveDashes
				};
				//	Draw Pen.
				result[(int)PenSetEnum.Draw] = new Pen(
					ColorTranslator.FromHtml(mColorDrawPenSelected), 2f);
			}
			else
			{
				//	Move Pen.
				result[(int)PenSetEnum.Move] =
					new Pen(ColorTranslator.FromHtml(mColorMovePen), 2f)
				{
					DashPattern = moveDashes
				};
				//	Draw Pen.
				result[(int)PenSetEnum.Draw] =
					new Pen(ColorTranslator.FromHtml(mColorDrawPen), 2f);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetPositionZAbs																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the absolute position of Z at the specified position type.
		/// </summary>
		/// <param name="zPositionType">
		/// The type of position to which to check the Z-axis value.
		/// </param>
		/// <param name="depthOffset">
		/// Optional depth offset to add to the found position.
		/// </param>
		/// <returns>
		/// The absolute position of the Z-axis at the specified position type,
		/// if known. Otherwise, 0.
		/// </returns>
		public static float GetPositionZAbs(TransitZEnum zPositionType,
			float depthOffset = 0f)
		{
			float depth = 0f;
			float extent = 0f;
			float reach = 0f;
			float result = 0f;

			if(ConfigProfile != null)
			{
				//	A configuration is loaded.
				reach = GetMillimeters(ConfigProfile.Depth);
				depth = SessionWorkpieceInfo.Thickness;
				if(reach != 0f)
				{
					switch(ConfigProfile.ZOrigin)
					{
						case OriginLocationEnum.Bottom:
							//	The head's maximum depth is 0.
							extent = reach;
							switch(ConfigProfile.TravelZ)
							{
								case DirectionUpDownEnum.Down:
									//	Fully extended, 0.
									//	Fully retracted, -reach.
									switch(zPositionType)
									{
										case TransitZEnum.FullyExtended:
											result = 0f;
											break;
										case TransitZEnum.FullyRetracted:
											result = 0f - extent;
											break;
										case TransitZEnum.TopOfMaterial:
											result = 0f - depth;
											break;
									}
									result += depthOffset;
									break;
								case DirectionUpDownEnum.None:
								case DirectionUpDownEnum.Up:
									//	Fully extended, 0.
									//	Fully retracted, +reach.
									switch(zPositionType)
									{
										case TransitZEnum.FullyExtended:
											result = 0f;
											break;
										case TransitZEnum.FullyRetracted:
											result = extent;
											break;
										case TransitZEnum.TopOfMaterial:
											result = depth;
											break;
									}
									result -= depthOffset;
									break;
							}
							break;
						case OriginLocationEnum.Center:
						case OriginLocationEnum.None:
							//	The head is homed in the center of its travel.
							extent = reach / 2f;
							switch(ConfigProfile.TravelZ)
							{
								case DirectionUpDownEnum.Down:
									//	Fully extended, +reach / 2.
									//	Fully retracted, -reach / 2.
									switch(zPositionType)
									{
										case TransitZEnum.FullyExtended:
											result = extent;
											break;
										case TransitZEnum.FullyRetracted:
											result = 0f - extent;
											break;
										case TransitZEnum.TopOfMaterial:
											result = extent - depth;
											break;
									}
									result += depthOffset;
									break;
								case DirectionUpDownEnum.None:
								case DirectionUpDownEnum.Up:
									//	Fully extended, -reach / 2.
									//	Fully retracted, +reach / 2.
									switch(zPositionType)
									{
										case TransitZEnum.FullyExtended:
											result = 0f - extent;
											break;
										case TransitZEnum.FullyRetracted:
											result = extent;
											break;
										case TransitZEnum.TopOfMaterial:
											result = (0f - extent) + depth;
											break;
									}
									result -= depthOffset;
									break;
							}
							break;
						case OriginLocationEnum.Top:
							//	The head is at 0 when fully retracted.
							extent = reach;
							switch(ConfigProfile.TravelZ)
							{
								case DirectionUpDownEnum.Down:
									//	Fully extended, +reach.
									//	Fully retracted, 0.
									switch(zPositionType)
									{
										case TransitZEnum.FullyExtended:
											result = extent;
											break;
										case TransitZEnum.FullyRetracted:
											result = 0f;
											break;
										case TransitZEnum.TopOfMaterial:
											result = extent - depth;
											break;
									}
									result += depthOffset;
									break;
								case DirectionUpDownEnum.None:
								case DirectionUpDownEnum.Up:
									//	Fully extended, -reach.
									//	Fully retracted, 0.
									switch(zPositionType)
									{
										case TransitZEnum.FullyExtended:
											result = 0f - extent;
											break;
										case TransitZEnum.FullyRetracted:
											result = 0f;
											break;
										case TransitZEnum.TopOfMaterial:
											result = (0f - extent) + depth;
											break;
									}
									result -= depthOffset;
									break;
							}
							break;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetRawEndOffset																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the calculated end offset from the caller's starting location.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation in focus.
		/// </param>
		/// <param name="workpiece">
		/// Reference to the workpiece information item.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location.
		/// </param>
		/// <returns>
		/// Reference to a coordinate representing the ending offset for the
		/// specified operation.
		/// </returns>
		public static FVector2 GetRawEndOffset(PatternOperationItem operation,
			WorkpieceInfoItem workpiece, FVector2 location)
		{
			float positionX = 0f;
			float positionY = 0f;
			FVector2 result = new FVector2();

			if(operation != null && workpiece != null && location != null)
			{
				//	EndOffsetX, EndOffsetY.
				//	X.
				if(operation.EndOffsetX.Length > 0 ||
					operation.EndOffsetXOrigin != OffsetLeftRightEnum.None)
				{
					//	End offset was specified.
					positionX = GetMillimeters(operation.EndOffsetX);
					positionX = TranslateOffset(workpiece.Area, positionX,
						operation.EndOffsetXOrigin, location.X);
				}
				else
				{
					positionX = location.X;
				}
				//	Y.
				if(operation.EndOffsetY.Length > 0 ||
					(operation.EndOffsetYOrigin != OffsetTopBottomEnum.None))
				{
					//	End offset was specified.
					positionY = GetMillimeters(operation.EndOffsetY);
					positionY = TranslateOffset(workpiece.Area, positionY,
						operation.EndOffsetYOrigin, location.Y);
				}
				else
				{
					positionY = location.Y;
				}
				result.X = positionX;
				result.Y = positionY;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetSystemSize																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the system size of the workspace.
		/// </summary>
		/// <returns>
		/// The system workspace size for the currently active profile.
		/// </returns>
		public static SizeF GetSystemSize()
		{
			string unit = BaseUnit(mConfigProfile.DisplayUnits);

			SizeF result = new SizeF(
				MeasurementProcessor.SumMillimeters(mConfigProfile.XDimension, unit),
				MeasurementProcessor.SumMillimeters(mConfigProfile.YDimension, unit));
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetToolNames																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return an array of tool names defined for this session.
		/// </summary>
		/// <returns>
		/// Reference to the string array containing the names of all of the
		/// currently defined tools, if found. Otherwise, an empty array.
		/// </returns>
		public static string[] GetToolNames()
		{
			List<string> names = null;
			string[] result = new string[0];


			if(mConfigProfile.UserTools.Count > 0)
			{
				names = UserToolCollection.Clone(mConfigProfile.UserTools).
					OrderBy(x => x.ToolName).Select(y => y.ToolName).ToList();
				result = names.ToArray();
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetUniqueUserDataImageName																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the first unique variation of the suggested image name within
		/// the user data folder.
		/// </summary>
		/// <param name="requestedName">
		/// The desired name for the file, if the name is available.
		/// </param>
		/// <returns>
		/// The first unique variation of the requested name requested.
		/// </returns>
		public static string GetUniqueUserDataImageName(string requestedName)
		{
			DirectoryInfo dir = null;
			string extension = "";
			List<FileInfo> files = null;
			int index = 0;
			string nameOnly = "";
			string result = "";

			if(requestedName?.Length > 0)
			{
				extension = GetFileExtension(requestedName);
				if(extension.Length > 0 && extension.Length < requestedName.Length)
				{
					nameOnly = requestedName.Substring(0,
						requestedName.Length - extension.Length);
				}
				else if(extension.Length == 0)
				{
					nameOnly = requestedName;
				}
				dir = new DirectoryInfo(UserDataPath);
				if(dir.Exists)
				{
					files = dir.GetFiles().ToList();
					if(files.Exists(x => x.Name == nameOnly + extension))
					{
						index = 1;
						while(files.Exists(x =>
							x.Name == nameOnly + $"-{index}" + extension))
						{
							index++;
						}
						nameOnly += $"-{index}";
					}
					result = nameOnly + extension;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetValue																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified group member in the provided match.
		/// </summary>
		/// <param name="match">
		/// Reference to the match to be inspected.
		/// </param>
		/// <param name="groupName">
		/// Name of the group for which the value will be found.
		/// </param>
		/// <returns>
		/// The value found in the specified group, if found. Otherwise, empty
		/// string.
		/// </returns>
		public static string GetValue(Match match, string groupName)
		{
			string result = "";

			if(match != null && match.Groups[groupName] != null &&
				match.Groups[groupName].Value != null)
			{
				result = match.Groups[groupName].Value;
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the value of the specified group member in a match found with
		/// the provided source and pattern.
		/// </summary>
		/// <param name="source">
		/// Source string to search.
		/// </param>
		/// <param name="pattern">
		/// Regular expression pattern to apply.
		/// </param>
		/// <param name="groupName">
		/// Name of the group for which the value will be found.
		/// </param>
		/// <returns>
		/// The value found in the specified group, if found. Otherwise, empty
		/// string.
		/// </returns>
		public static string GetValue(string source, string pattern,
			string groupName)
		{
			Match match = null;
			string result = "";

			if(source?.Length > 0 && pattern?.Length > 0 && groupName?.Length > 0)
			{
				match = Regex.Match(source, pattern);
				if(match.Success)
				{
					result = GetValue(match, groupName);
				}
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the value associated with the specified property name of the
		/// provided pattern operation.
		/// </summary>
		/// <param name="type">
		/// Reference to the type for which the property value will be retrieved.
		/// </param>
		/// <param name="instance">
		/// Reference to the instance of the object from which to retrieve the
		/// property.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to retrieve.
		/// </param>
		/// <returns>
		/// Value of the specified property on the provided pattern operation
		/// item, if found. Otherwise, an empty string.
		/// </returns>
		public static object GetValue(Type type, object instance,
			string propertyName)
		{
			PropertyInfo property = null;
			object result = null;
			object item = null;

			if(type != null && propertyName?.Length > 0)
			{
				try
				{
					property = type.GetProperty(propertyName,
						System.Reflection.BindingFlags.GetProperty |
						System.Reflection.BindingFlags.IgnoreCase |
						System.Reflection.BindingFlags.Instance |
						System.Reflection.BindingFlags.Public);
					if(property != null)
					{
						item = property.GetValue(instance);
					}
					if(item != null)
					{
						result = item;
					}
				}
				catch { }
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetWorkspaceArea																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a reference to the currently configured workspace area for this
		/// configuration.
		/// </summary>
		/// <returns>
		/// Reference to the established workspace area, where the top and left
		/// coordinates are aligned with the top left visual corner of the display.
		/// </returns>
		public static FArea GetWorkspaceArea()
		{
			float height = GetMillimeters(mConfigProfile.YDimension);
			//float offset = 0f;
			FArea result = null;
			float width = GetMillimeters(mConfigProfile.XDimension);

			result = new FArea(0f, 0f, width, height);

			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetWorkspaceRatio																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the ratio of the current workspace's width to its height.
		/// </summary>
		/// <returns>
		/// The ratio of the workspace width to its height.
		/// </returns>
		public static float GetWorkspaceRatio()
		{
			float height = 0f;
			//MeasurementProcessor measurements = null;
			float result = 0f;
			float width = 0f;

			width = MeasurementProcessor.SumMillimeters(mConfigProfile.XDimension,
				BaseUnit(mConfigProfile.DisplayUnits));
			height = MeasurementProcessor.SumMillimeters(mConfigProfile.YDimension,
				BaseUnit(mConfigProfile.DisplayUnits));

			if(height != 0f)
			{
				result = width / height;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* HasFraction																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the caller's specified value contains
		/// a fraction.
		/// </summary>
		/// <param name="value">
		/// The value to inspect for fractions.
		/// </param>
		/// <returns>
		/// True if the value contains a fraction. Otherwise, false.
		/// </returns>
		public static bool HasFraction(string value)
		{
			return GetValue(value, ResourceMain.rxFractional, "fraction").Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* HasProperty																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the provided type has a public
		/// property with the specified name.
		/// </summary>
		/// <param name="type">
		/// Reference to the object type to consider.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to search for.
		/// </param>
		/// <returns>
		/// True if a property exists in this class with the specified name.
		/// Otherwise, false.
		/// </returns>
		public static bool HasProperty(Type type, string propertyName)
		{
			bool result = false;

			if(type != null && propertyName?.Length > 0)
			{
				try
				{
					result = (type.GetProperty(propertyName,
						System.Reflection.BindingFlags.GetProperty |
						System.Reflection.BindingFlags.IgnoreCase |
						System.Reflection.BindingFlags.Instance |
						System.Reflection.BindingFlags.Public) != null);
				}
				catch { }
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* InitializeApplication																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Initialize all of the session-level functionality for the application.
		/// </summary>
		public static void InitializeApplication()
		{
			UserDataInitialize();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IsDataUri																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the caller's string is a DataURI.
		/// </summary>
		/// <param name="dataUri">
		/// String to test for DataURI.
		/// </param>
		/// <returns>
		/// True if the value is a DataURI. Otherwise, false.
		/// </returns>
		public static bool IsDataUri(string dataUri)
		{
			return (dataUri?.Length > 0 && dataUri.StartsWith("data:"));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IsXFeed																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the workspace is primarily viewed as
		/// X-feed, rather than Y-feed.
		/// </summary>
		/// <returns>
		/// Value indicating whether the router workspace is primarily considered
		/// to be an X-feed space.
		/// </returns>
		public static bool IsXFeed()
		{
			float height = 0f;
			bool result = false;
			float width = 0f;

			width = MeasurementProcessor.SumMillimeters(mConfigProfile.XDimension,
				BaseUnit(mConfigProfile.DisplayUnits));
			height = MeasurementProcessor.SumMillimeters(mConfigProfile.YDimension,
				BaseUnit(mConfigProfile.DisplayUnits));

			result = (width >= height &&
				(mConfigProfile.AxisXIsOpenEnded || !mConfigProfile.AxisYIsOpenEnded));
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* LengthIsX																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the Length variable runs on the X
		/// axis.
		/// </summary>
		/// <returns>
		/// True if the Length of a material is measured along the X axis.
		/// Otherwise, false.
		/// </returns>
		public static bool LengthIsX()
		{
			bool result = false;

			if(mConfigProfile.AxisXIsOpenEnded ||
				(!mConfigProfile.AxisYIsOpenEnded &&
				GetMillimeters(mConfigProfile.XDimension) >
				GetMillimeters(mConfigProfile.YDimension)))
			{
				result = true;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MimeType																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the MIME-type associated with the specified extension.
		/// </summary>
		/// <param name="extension">
		/// File extension to associate.
		/// </param>
		/// <returns>
		/// MIME-type associated with the specified file extension.
		/// </returns>
		public static string MimeType(string extension)
		{
			string result = "";

			if(extension?.Length > 0)
			{
				if(extension.StartsWith("."))
				{
					extension = extension.Substring(1);
				}
				extension = extension.ToLower();
				switch(extension)
				{
					case "3g2": //	3GPP2 audio/video container.
						result = "video/3gpp2";
						//	also, audio/3gpp2 for audio only.
						break;
					case "3gp": //	3GPP audio/video container.
						result = "video/3gpp";
						//	also, audio/3gpp for audio only.
						break;
					case "7z":  //	7-zip archive.
						result = "application/x-7z-compressed";
						break;
					case "aac": //	AAC audio.
						result = "audio/aac";
						break;
					case "abw": //	AbiWord document.
						result = "application/x-abiword";
						break;
					case "arc": //	Archive document (multiple files embedded).
						result = "application/x-freearc";
						break;
					case "avi": //	AVI: Audio Video Interleave.
						result = "video/x-msvideo";
						break;
					case "azw": //	Amazon Kindle eBook format.
						result = "application/vnd.amazon.ebook";
						break;
					case "bin": //	Any kind of binary data.
						result = "application/octet-stream";
						break;
					case "bmp": //	Windows OS/2 Bitmap Graphics.
						result = "image/bmp";
						break;
					case "bz":  //	BZip archive.
						result = "application/x-bzip";
						break;
					case "bz2": //	BZip2 archive.
						result = "application/x-bzip2";
						break;
					case "csh": //	C-Shell script.
						result = "application/x-csh";
						break;
					case "css": //	Cascading Style Sheets (CSS).
						result = "text/css";
						break;
					case "csv": //	Comma-separated values (CSV).
						result = "text/csv";
						break;
					case "doc": //	Microsoft Word (Legacy).
						result = "application/msword";
						break;
					case "docx":  //	Microsoft Word (OpenXML).
						result = "application/vnd.openxmlformats-officedocument." +
							"wordprocessingml.document";
						break;
					case "eot": //	MS Embedded OpenType fonts.
						result = "application/vnd.ms-fontobject";
						break;
					case "epub":  //	Electronic publication (EPUB).
						result = "application/epub+zip";
						break;
					case "gz":  //	GZip Compressed Archive.
						result = "application/gzip";
						break;
					case "gif": //	Graphics Interchange Format (GIF).
						result = "image/gif";
						break;
					case "htm": //	HyperText Markup Language (HTML).
					case "html":
						result = "text/html";
						break;
					case "ico": //	Icon format.
						result = "image/vnd.microsoft.icon";
						break;
					case "ics": //	iCalendar format.
						result = "text/calendar";
						break;
					case "jar": //	Java Archive (JAR).
						result = "application/java-archive";
						break;
					case "jpeg":  //	JPEG image.
					case "jpg":
						result = "image/jpeg";
						break;
					case "js":  //	JavaScript.
						result = "text/javascript";
						break;
					case "json":  //	JSON format.
						result = "application/json";
						break;
					case "jsonld":  //	JSON-LD format.
						result = "application/ld+json";
						break;
					case "mid": //	Musical Instrument Digital Interface (MIDI).
					case "midi":
						result = "audio/midi";
						//	also, audio/x-midi
						break;
					case "mjs": //	JavaScript module.
						result = "text/javascript";
						break;
					case "mp3": //	MP3 audio.
						result = "audio/mpeg";
						break;
					case "mp4": //	MP4 video.
						result = "video/mp4";
						break;
					case "mpeg":  //	MPEG Video.
						result = "video/mpeg";
						break;
					case "mpkg":  //	Apple Installer Package.
						result = "application/vnd.apple.installer+xml";
						break;
					case "odp": //	OpenDocument presentation document.
						result = "application/vnd.oasis.opendocument.presentation";
						break;
					case "ods": //	OpenDocument spreadsheet document.
						result = "application/vnd.oasis.opendocument.spreadsheet";
						break;
					case "odt": //	OpenDocument text document.
						result = "application/vnd.oasis.opendocument.text";
						break;
					case "oga": //	OGG audio.
						result = "audio/ogg";
						break;
					case "ogv": //	OGG video.
						result = "video/ogg";
						break;
					case "ogx": //	OGG.
						result = "application/ogg";
						break;
					case "opus":  //	Opus audio.
						result = "audio/opus";
						break;
					case "otf": //	OpenType font.
						result = "font/otf";
						break;
					case "png": //	Portable Network Graphics.
						result = "image/png";
						break;
					case "pdf": //	Adobe Portable Document Format (PDF).
						result = "application/pdf";
						break;
					case "php": //	Hypertext Preprocessor (Personal Home Page).
						result = "application/x-httpd-php";
						break;
					case "ppt": //	Microsoft PowerPoint (Legacy).
						result = "application/vnd.ms-powerpoint";
						break;
					case "pptx":  //	Microsoft PowerPoint (OpenXML).
						result = "application/vnd.openxmlformats-officedocument." +
							"presentationml.presentation";
						break;
					case "rar": //	RAR archive.
						result = "application/vnd.rar";
						break;
					case "rtf": //	Rich Text Format (RTF).
						result = "application/rtf";
						break;
					case "sh":  //	Bourne shell script.
						result = "application/x-sh";
						break;
					case "svg": //	Scalable Vector Graphics (SVG).
						result = "image/svg+xml";
						break;
					case "swf": //	Small web format (SWF) or Adobe Flash document.
						result = "application/x-shockwave-flash";
						break;
					case "tar": //	Tape Archive (TAR).
						result = "application/x-tar";
						break;
					case "tif": //	Tagged Image File Format (TIFF).
					case "tiff":
						result = "image/tiff";
						break;
					//case "ts":  //	MPEG transport stream.
					//	result = "video/mp2t";
					//	break;
					case "ts":  //	Typescript.
						result = "application/x-typescript";
						break;
					case "ttf": //	TrueType Font.
						result = "font/ttf";
						break;
					case "txt": //	Text, (generally ASCII or ISO 8859-n).
						result = "text/plain";
						break;
					case "vsd": //	Microsoft Visio.
						result = "application/vnd.visio";
						break;
					case "wav": //	Waveform Audio Format.
						result = "audio/wav";
						break;
					case "weba":  //	WEBM audio.
						result = "audio/webm";
						break;
					case "webm":  //	WEBM video.
						result = "video/webm";
						break;
					case "webp":  //	WEBP image.
						result = "image/webp";
						break;
					case "woff":  //	Web Open Font Format (WOFF).
						result = "font/woff";
						break;
					case "woff2": //	Web Open Font Format (WOFF).
						result = "font/woff2";
						break;
					case "xhtml": //	XHTML.
						result = "application/xhtml+xml";
						break;
					case "xls": //	Microsoft Excel (Legacy).
						result = "application/vnd.ms-excel";
						break;
					case "xlsx":  //	Microsoft Excel (OpenXML).
						result = "application/vnd.openxmlformats-officedocument." +
							"spreadsheetml.sheet";
						break;
					case "xml": //	XML.
						result = "application/xml";
						//	also, text/xml if readable by casual users
						break;
					case "xul": //	XUL.
						result = "application/vnd.mozilla.xul+xml";
						break;
					case "zip": //	ZIP archive.
						result = "application/zip";
						break;
					default:  //	Any unidentified format is binary data.
						result = "application/octet-stream";
						break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	OffsetArea																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a rectangle whose area has been offset by the specified amount
		/// from the source.
		/// </summary>
		/// <param name="source">
		/// The source rectangle to reference.
		/// </param>
		/// <param name="offsetX">
		/// The horizontal amount by which to offset the new area from the source.
		/// </param>
		/// <param name="offsetY">
		/// The vertical amount by which to offset the new area from the source.
		/// </param>
		/// <returns>
		/// New rectangle that has been offset by the specified horizontal and
		/// vertical amounts from the source.
		/// </returns>
		public static Rectangle OffsetArea(Rectangle source, int offsetX,
			int offsetY)
		{
			Rectangle result =
				new Rectangle(
					source.X + offsetX, source.Y + offsetY,
					source.Width, source.Height);
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ReadConfiguration																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Read the configuration file.
		/// </summary>
		public static void ReadConfiguration()
		{
			List<string> allOperationActions =
				Enum.GetNames(typeof(OperationActionEnum)).ToList();
			string content = "";
			string filename = "";
			List<OperationActionPropertyItem> operationActionProperties = null;
			ToolTypeDefinitionCollection toolTypeDefinitions = null;

			if(!(mConfigurationFilename?.Length > 0))
			{
				//	The configuration filename has not yet been set. Use the app
				//	directory.
				//mConfigurationFilename = Path.Combine(
				//	AppDomain.CurrentDomain.BaseDirectory, "ShopToolsConfig.json");
				mConfigurationFilename =
					Path.Combine(mUserDataPath, "ShopToolsConfig.json");
			}
			if(Path.Exists(mConfigurationFilename))
			{
				content = File.ReadAllText(mConfigurationFilename);
				try
				{
					mConfigProfile =
						JsonConvert.DeserializeObject<ShopToolsConfigItem>(content);
				}
				catch(Exception ex)
				{
					MessageBox.Show($"Error loading configuration file: {ex.Message}",
						"Read Configuration File");
					System.Environment.Exit(1);
				}
			}
			if(mConfigProfile == null)
			{
				mConfigProfile = new ShopToolsConfigItem();
			}
			//	Tool type definitions.
			filename =
				Path.Combine(mUserDataPath, "ToolTypes.json");
			if(Path.Exists(filename))
			{
				content = File.ReadAllText(filename);
				try
				{
					toolTypeDefinitions =
						JsonConvert.DeserializeObject<ToolTypeDefinitionCollection>(
							content);
					mConfigProfile.ToolTypeDefinitions.AddRange(toolTypeDefinitions);
				}
				catch(Exception ex)
				{
					MessageBox.Show($"Error loading tool definition file: {ex.Message}",
						"Read Tool Definition File");
					System.Environment.Exit(1);
				}
			}
			//	Operation action properties.
			filename =
				Path.Combine(mUserDataPath,
				"OperationActionProperties.json");
			if(Path.Exists(filename))
			{
				content = File.ReadAllText(filename);
				try
				{
					operationActionProperties =
						JsonConvert.DeserializeObject<OperationActionPropertyCollection>(
							content);
					//	Sort the properties by their user order.
					operationActionProperties =
						operationActionProperties.OrderBy(x => x.SortIndex).ToList();
					//	Process the include / exclude lists on each item.
					foreach(OperationActionPropertyItem propertyItem in
						operationActionProperties)
					{
						if(propertyItem.Internal)
						{
							//	An internal property doesn't autofill anywhere.
							propertyItem.IncludeOperationActions.Clear();
							propertyItem.ExcludeOperationActions.Clear();
						}
						else
						{
							//	If no included items have been specified, then include all.
							if(propertyItem.IncludeOperationActions.Count == 0)
							{
								propertyItem.IncludeOperationActions.
									AddRange(allOperationActions);
							}
							//	Remove any excluded operations.
							foreach(string entryItem in propertyItem.ExcludeOperationActions)
							{
								propertyItem.IncludeOperationActions.Remove(entryItem);
							}
						}
					}
					//	Add the prepared list to the session configuration.
					mConfigProfile.OperationActionProperties.AddRange(
						operationActionProperties);
				}
				catch(Exception ex)
				{
					MessageBox.Show(
						$"Error operation property definition file: {ex.Message}",
						"Read Operation Property Definition File");
					System.Environment.Exit(1);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RemoveDuplicates																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Remove all duplicate values from the caller's plot point collection.
		/// </summary>
		/// <param name="points">
		/// Reference to the collection of points in which to identify duplicate
		/// values.
		/// </param>
		/// <param name="epsilonDecimalPoints">
		/// Number of decimal points to consider as within the epsilon value.
		/// </param>
		public static void RemoveDuplicates(PlotPointCollection points,
			int epsilonDecimalPoints = 3)
		{
			int count = 0;
			float currentX = 0f;
			float currentY = 0f;
			string format = "0";
			int index = 0;
			float lastX = 0f;
			float lastY = 0f;
			FVector2 point = null;

			if(points?.Count > 0)
			{
				if(epsilonDecimalPoints > 0)
				{
					format += "." + new String('0', epsilonDecimalPoints);
				}
				count = points.Count;
				for(index = 0; index < count; index ++)
				{
					point = points[index].Point;
					currentX = ToFloat(point.X.ToString(format));
					currentY = ToFloat(point.Y.ToString(format));
					if(index > 0)
					{
						if(currentX == lastX && currentY == lastY)
						{
							//	This item matches the previous item.
							points.RemoveAt(index);
							index--;		//	Deindex.
							count--;		//	Discount.
						}
					}
					lastX = currentX;
					lastY = currentY;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ResizeArea																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a rectangle of the specified size ratio that perfectly fits
		/// into the caller's space.
		/// </summary>
		/// <param name="targetWidth">
		/// The width of the target area to cover.
		/// </param>
		/// <param name="targetHeight">
		/// The height of the target area to cover.
		/// </param>
		/// <param name="widthToHeightRatio">
		/// The Width to Height ratio of the new rectangle.
		/// </param>
		/// <returns>
		/// Size of the rectangle that will tightly fit into the specified
		/// area with the given width to height ratio.
		/// </returns>
		public static Size ResizeArea(int targetWidth, int targetHeight,
			float widthToHeightRatio)
		{
			double nHeight = 0d;
			double nWidth = 0d;
			Size result = new Size();
			double tHeight = (double)targetHeight;
			double tWidth = (double)targetWidth;
			double whRatio = (double)widthToHeightRatio;

			if(tHeight != 0d && tWidth / tHeight > widthToHeightRatio)
			{
				nHeight = tHeight;
				nWidth = nHeight * whRatio;
			}
			else if(whRatio != 0d)
			{
				nWidth = tWidth;
				nHeight = nWidth / whRatio;
			}
			result.Width = (int)nWidth;
			result.Height = (int)nHeight;
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ResizeImage																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Resize the image to the specified width and height.
		/// </summary>
		/// <param name="image">
		/// The image to resize.
		/// </param>
		/// <param name="width">
		/// The width to resize to.
		/// </param>
		/// <param name="height">
		/// The height to resize to.
		/// </param>
		/// <returns>
		/// The resized image.
		/// </returns>
		public static Bitmap ResizeImage(Image image, int width, int height)
		{
			Rectangle dRect = new Rectangle(0, 0, width, height);
			Bitmap dImage = new Bitmap(width, height);

			dImage.SetResolution(image.HorizontalResolution,
				image.VerticalResolution);

			using(Graphics graphics = Graphics.FromImage(dImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using(ImageAttributes wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, dRect,
						0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return dImage;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ResolveDepth																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the desired depth for the specified action.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation for which the depth will be checked.
		/// </param>
		/// <returns>
		/// The depth of the operation, if specified, or the thickness of the
		/// defined workpiece, if not specified.
		/// </returns>
		public static float ResolveDepth(PatternOperationItem operation)
		{
			float result = 0f;

			if(operation != null)
			{
				if(operation.Depth.Length == 0)
				{
					//	If no depth was specified, the full depth is assumed.
					result = mSessionWorkpieceInfo.Thickness;
				}
				else
				{
					result = GetMillimeters(operation.Depth);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SelectItem																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The select the first specified item in the provided list by item text.
		/// </summary>
		/// <param name="comboBox">
		/// Reference to the combobox to search.
		/// </param>
		/// <param name="text">
		/// The text to search for.
		/// </param>
		/// <param name="caseSensitive">
		/// Optional value indicating whether to perform a case-sensitive search.
		/// Default = false.
		/// </param>
		public static void SelectItem(ComboBox comboBox, string text,
			bool caseSensitive = false)
		{
			int count = 0;
			int index = 0;
			string ltext = "";
			object item = null;

			if(comboBox?.Items.Count > 0)
			{
				//	Deselect by default.
				comboBox.SelectedItem = null;
				if(!caseSensitive && text?.Length > 0)
				{
					ltext = text.ToLower();
				}
				count = comboBox.Items.Count;
				for(index = 0; index < count; index ++)
				{
					item = comboBox.Items[index];
					if(item != null)
					{
						if(caseSensitive)
						{
							if(text == item.ToString())
							{
								comboBox.SelectedItem = item;
								break;
							}
						}
						else
						{
							if(ltext == item.ToString().ToLower())
							{
								comboBox.SelectedItem = item;
								break;
							}
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SessionConverter																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="SessionConverter">SessionConverter</see>.
		/// </summary>
		private static ConversionCalc.Converter mSessionConverter =
			new ConversionCalc.Converter();
		/// <summary>
		/// Get a reference to the singleton converter for this session.
		/// </summary>
		public static ConversionCalc.Converter SessionConverter
		{
			get { return mSessionConverter; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SessionWorkpieceInfo																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="SessionWorkpieceInfo">SessionWorkpieceInfo</see>.
		/// </summary>
		private static WorkpieceInfoItem mSessionWorkpieceInfo =
			new WorkpieceInfoItem();
		/// <summary>
		/// Get/Set a reference to the session workpiece information.
		/// </summary>
		public static WorkpieceInfoItem SessionWorkpieceInfo
		{
			get { return mSessionWorkpieceInfo; }
			set { mSessionWorkpieceInfo = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToBool																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Provide fail-safe conversion of string to boolean value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Boolean value. False if not convertible.
		/// </returns>
		public static bool ToBool(object value)
		{
			bool result = false;
			if(value != null)
			{
				result = ToBool(value.ToString());
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Provide fail-safe conversion of string to boolean value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <param name="defaultValue">
		/// The default value to return if the value was not present.
		/// </param>
		/// <returns>
		/// Boolean value. False if not convertible.
		/// </returns>
		public static bool ToBool(string value, bool defaultValue = false)
		{
			//	A try .. catch block was originally implemented here, but the
			//	following text was being sent to output on each unsuccessful
			//	match.
			//	Exception thrown: 'System.FormatException' in mscorlib.dll
			bool result = false;

			if(value?.Length > 0)
			{
				if(!bool.TryParse(value, out result))
				{
					Debug.WriteLine($"Error on ToBool");
				}
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToFloat																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Floating point value. 0 if not convertible.
		/// </returns>
		public static float ToFloat(object value)
		{
			float result = 0f;
			if(value != null)
			{
				result = ToFloat(value.ToString());
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Floating point value. 0 if not convertible.
		/// </returns>
		/// <remarks>
		/// This version supports fractions.
		/// </remarks>
		public static float ToFloat(string value)
		{
			bool bIsNegative = false;
			int denominator = 0;
			Match match = null;
			int numerator = 0;
			float result = 0f;
			int whole = 0;

			try
			{
				if(HasFraction(value))
				{
					match = Regex.Match(value, ResourceMain.rxFractional);
					whole = ToInt(GetValue(match, "whole"));
					numerator = ToInt(GetValue(match, "numerator"));
					denominator = ToInt(GetValue(match, "denominator"));
					bIsNegative = GetValue(match, "fractional").StartsWith("-");
					result = (float)whole;
					if(denominator > 0)
					{
						result += (float)numerator / (float)denominator;
					}
					if(bIsNegative)
					{
						result = 0f - result;
					}
				}
				else
				{
					result = float.Parse(value);
				}
			}
			catch { }
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToInt																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Int32 value. 0 if not convertible.
		/// </returns>
		public static int ToInt(object value)
		{
			int result = 0;
			if(value != null)
			{
				result = ToInt(value.ToString());
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Int32 value. 0 if not convertible.
		/// </returns>
		public static int ToInt(string value)
		{
			int result = 0;
			try
			{
				result = int.Parse(value);
			}
			catch { }
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToMultiLineString																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the multi-line single string representation of the items found
		/// in the caller's list, where lines that end with a space are continued
		/// without a line-break and all other lines are ended with a line break.
		/// </summary>
		/// <param name="list">
		/// Reference to the list of string values to be converted to a single
		/// string.
		/// </param>
		/// <returns>
		/// A single multi-line representation of the caller's string list.
		/// </returns>
		public static string ToMultiLineString(List<string> list)
		{
			StringBuilder builder = new StringBuilder();

			if(list?.Count > 0)
			{
				foreach(string listItem in list)
				{
					if(listItem.EndsWith(' '))
					{
						builder.Append(listItem);
					}
					else
					{
						builder.AppendLine(listItem);
					}
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToPoint																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Convert a single precision floating point point to a
		/// System.Drawing.Point.
		/// </summary>
		/// <param name="point">
		/// Reference to the point to be converted.
		/// </param>
		/// <returns>
		/// A System.Drawing.Point.
		/// </returns>
		public static Point ToPoint(FVector2 point)
		{
			Point result = Point.Empty;

			if(point != null)
			{
				result = new Point((int)point.X, (int)point.Y);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransformFromAbsolute																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a point on the top, left working space that corresponds to an
		/// absolute coordinate on the defined physical table.
		/// </summary>
		/// <param name="absolutePoint">
		/// The absolute point to transform.
		/// </param>
		/// <returns>
		/// Reference to the point corresponding to the caller's absolute
		/// coordinate.
		/// </returns>
		public static FVector2 TransformFromAbsolute(FVector2 absolutePoint)
		{
			float height = GetMillimeters(mConfigProfile.YDimension);
			FVector2 result = new FVector2();
			FVector2 scale = new FVector2();
			FVector2 target = null;
			FVector2 translation = new FVector2();
			float width = GetMillimeters(mConfigProfile.XDimension);

			if(absolutePoint != null)
			{
				if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
				{
					//	Flip X axis.
					scale.X = -1f;
				}
				else
				{
					//	Normal X axis.
					scale.X = 1f;
				}
				if(mConfigProfile.TravelY == DirectionUpDownEnum.Up)
				{
					//	Flip Y axis.
					scale.Y = -1f;
				}
				else
				{
					//	Normal Y axis.
					scale.Y = 1f;
				}
				target = FMatrix3.Scale((FVector2)absolutePoint, scale);
				switch(mConfigProfile.XYOrigin)
				{
					case OriginLocationEnum.Bottom:
						//	Bottom center to top left.
						translation.X = width / 2f;
						translation.Y = height;
						break;
					case OriginLocationEnum.BottomLeft:
						//	Bottom left to top left.
						translation.X = 0f;
						translation.Y = height;
						break;
					case OriginLocationEnum.BottomRight:
						//	Bottom right to top left.
						translation.X = width;
						translation.Y = height;
						break;
					case OriginLocationEnum.Center:
						//	Center center to top left.
						translation.X = width / 2f;
						translation.Y = height / 2f;
						break;
					case OriginLocationEnum.Left:
						//	Center left to top left.
						translation.X = 0f;
						translation.Y = height / 2f;
						break;
					case OriginLocationEnum.Right:
						//	Center right to top left.
						translation.X = width;
						translation.Y = height / 2f;
						break;
					case OriginLocationEnum.Top:
						//	Top center to top left.
						translation.X = width / 2f;
						translation.Y = 0f;
						break;
					case OriginLocationEnum.TopLeft:
					case OriginLocationEnum.None:
						break;
					case OriginLocationEnum.TopRight:
						//	Top right to top left.
						translation.X = width;
						translation.Y = 0f;
						break;
				}
				if(translation.X != 0f ||
					translation.Y != 0f)
				{
					target = FMatrix3.Translate(target, translation);
				}
				result = new FVector2(target);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransformToAbsolute																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a point on the physical working space that corresponds to a
		/// display-based coordinate.
		/// </summary>
		/// <param name="displayPoint">
		/// The display point to transform, based on top, left anchor.
		/// </param>
		/// <returns>
		/// Reference to the absolute point corresponding to the caller's display
		/// coordinate.
		/// </returns>
		public static FVector2 TransformToAbsolute(FVector2 displayPoint)
		{
			bool bFlipX = false;
			bool bFlipY = false;
			float height = GetMillimeters(mConfigProfile.YDimension);
			FVector2 result = new FVector2();
			FVector2 scale = new FVector2();
			FVector2 target = null;
			FVector2 translation = new FVector2();
			float width = GetMillimeters(mConfigProfile.XDimension);

			if(displayPoint != null)
			{
				if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
				{
					//	Flip X axis.
					bFlipX = true;
					scale.X = -1f;
				}
				else
				{
					//	Normal X axis.
					scale.X = 1f;
				}
				if(mConfigProfile.TravelY == DirectionUpDownEnum.Up)
				{
					//	Flip Y axis.
					bFlipY = true;
					scale.Y = -1f;
				}
				else
				{
					//	Normal Y axis.
					scale.Y = 1f;
				}
				target = FMatrix3.Scale((FVector2)displayPoint, scale);
				switch(mConfigProfile.XYOrigin)
				{
					case OriginLocationEnum.Bottom:
						//	Top left to Bottom center.
						//	TODO: Check top left to bottom center.
						translation.X = 0f - (width / 2f);
						translation.Y =
							(bFlipY ? height : 0f - height);
						break;
					case OriginLocationEnum.BottomLeft:
						//	Bottom left to top left.
						translation.X = 0f;
						translation.Y =
							(bFlipY ? height : 0f - height);
						break;
					case OriginLocationEnum.BottomRight:
						//	Top left to bottom right.
						translation.X =
							(bFlipX ? width : 0f - width);
						translation.Y =
							(bFlipY ? height : 0f - height);
						break;
					case OriginLocationEnum.Center:
						//	Top left to center center.
						//	TODO: Check top left to center center.
						translation.X =
							(bFlipX ? width / 2f : 0f - (width / 2f));
						translation.Y =
							(bFlipY ? height / 2f : 0f - (height / 2f));
						break;
					case OriginLocationEnum.Left:
						//	Center left to top left.
						//	TODO: Check top left to center left.
						translation.X = 0f;
						translation.Y = 0f - (height / 2f);
						break;
					case OriginLocationEnum.Right:
						//	Center right to top left.
						//	TODO: Check top left to center right.
						translation.X = 0f - width;
						translation.Y = 0f - (height / 2f);
						break;
					case OriginLocationEnum.Top:
						//	Top center to top left.
						//	TODO: Check top left to top center.
						translation.X = 0f - (width / 2f);
						translation.Y = 0f;
						break;
					case OriginLocationEnum.TopLeft:
					case OriginLocationEnum.None:
						break;
					case OriginLocationEnum.TopRight:
						//	Top right to top left.
						//	TODO: Check top left to top right.
						translation.X = 0f - width;
						translation.Y = 0f;
						break;
				}
				if(translation.X != 0f ||
					translation.Y != 0f)
				{
					target = FMatrix3.Translate(target, translation);
				}
				result = new FVector2(target);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TranslateDirection																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Translate the relative directional offset from the visual perspective
		/// to the defined direction on the table.
		/// </summary>
		/// <param name="offset">
		/// The offset to polarize.
		/// </param>
		/// <param name="directionType">
		/// The visual relative direction with which the offset is associated.
		/// </param>
		/// <returns>
		/// The physical offset, relative to the established direction of travel
		/// on the table.
		/// </returns>
		public static float TranslateDirection(float offset,
			DirectionLeftRightEnum directionType)
		{
			float result = offset;

			if(directionType == DirectionLeftRightEnum.Left)
			{
				//	Opposite direction.
				result *= -1f;
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Translate the relative directional offset from the visual perspective
		/// to the defined direction on the table.
		/// </summary>
		/// <param name="offset">
		/// The offset to polarize.
		/// </param>
		/// <param name="offsetType">
		/// The visual relative direction with which the offset is associated.
		/// </param>
		/// <returns>
		/// The physical offset, relative to the established direction of travel
		/// on the table.
		/// </returns>
		public static float TranslateDirection(float offset,
			DirectionUpDownEnum offsetType)
		{
			float result = offset;
			if(offsetType == DirectionUpDownEnum.Up)
			{
				//	Opposite direction.
				result *= -1f;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TranslateOffset																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Translate the relative positional offset from the visual perspective
		/// to absolute positions based upon the physical anchors on the table or
		/// material.
		/// </summary>
		/// <param name="area">
		/// Reference to the area from which the offset is relative. This
		/// value represents physical coordinates.
		/// </param>
		/// <param name="offset">
		/// The offset to polarize.
		/// </param>
		/// <param name="offsetType">
		/// The visual relative direction with which the offset is associated.
		/// </param>
		/// <param name="relativeOffset">
		/// A base offset for the entire space.
		/// </param>
		/// <returns>
		/// The physical offset, relative to the established direction of travel
		/// on the table.
		/// </returns>
		/// <remarks>
		/// This method assumes that the left and top positions constitute the
		/// local anchor of the area.
		/// </remarks>
		public static float TranslateOffset(FArea area, float offset,
			OffsetLeftRightEnum offsetType, float relativeOffset = float.MinValue)
		{
			return TranslateOffset(area, 0f, offset, offsetType, relativeOffset);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Translate the relative positional offset from the visual perspective
		/// to absolute positions based upon the physical anchors on the table or
		/// material.
		/// </summary>
		/// <param name="area">
		/// Reference to the area from which the offset is relative. This
		/// value represents physical coordinates.
		/// </param>
		/// <param name="offset">
		/// The offset to polarize.
		/// </param>
		/// <param name="offsetType">
		/// The visual relative direction with which the offset is associated.
		/// </param>
		/// <param name="relativeOffset">
		/// A base offset for the entire space.
		/// </param>
		/// <returns>
		/// The physical offset, relative to the established direction of travel
		/// on the table.
		/// </returns>
		/// <remarks>
		/// This method assumes that the left and top positions constitute the
		/// local anchor of the area.
		/// </remarks>
		public static float TranslateOffset(FArea area, float offset,
			OffsetTopBottomEnum offsetType, float relativeOffset = float.MinValue)
		{
			return TranslateOffset(area, 0f, offset, offsetType, relativeOffset);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Translate the relative positional offset from the visual perspective
		/// to absolute positions based upon the physical anchors on the table or
		/// material.
		/// </summary>
		/// <param name="area">
		/// Reference to the area from which the offset is relative. This
		/// value represents physical coordinates.
		/// </param>
		/// <param name="subjectHeight">
		/// Height of the subject being positioned.
		/// </param>
		/// <param name="offset">
		/// The offset to polarize.
		/// </param>
		/// <param name="offsetType">
		/// The visual relative direction with which the offset is associated.
		/// </param>
		/// <param name="relativeOffset">
		/// A base offset for the entire space.
		/// </param>
		/// <returns>
		/// The physical offset, relative to the established direction of travel
		/// on the table.
		/// </returns>
		/// <remarks>
		/// This method assumes that the left and top positions constitute the
		/// local anchor of the area.
		/// </remarks>
		public static float TranslateOffset(FArea area, float subjectHeight,
			float offset, OffsetTopBottomEnum offsetType,
			float relativeOffset = float.MinValue)
		{
			float result = offset;

			if(area != null)
			{
				switch(offsetType)
				{
					case OffsetTopBottomEnum.Absolute:
						result = TransformFromAbsolute(new FVector2(0f, offset)).Y;
						break;
					case OffsetTopBottomEnum.Bottom:
						result = area.Bottom - subjectHeight +
							TranslateDirection(offset, mConfigProfile.TravelY);
						break;
					case OffsetTopBottomEnum.Center:
						result = (area.Top + (area.Height / 2f) +
							TranslateDirection(offset, mConfigProfile.TravelY)) -
							(subjectHeight / 2f);
						break;
					case OffsetTopBottomEnum.BottomEdgeToCenter:
						result = (area.Top + (area.Height / 2f) +
							TranslateDirection(offset, mConfigProfile.TravelY)) -
							subjectHeight;
						break;
					case OffsetTopBottomEnum.TopEdgeToCenter:
						result = area.Top + (area.Height / 2f) +
							TranslateDirection(offset, mConfigProfile.TravelY);
						break;
					case OffsetTopBottomEnum.None:
					case OffsetTopBottomEnum.Relative:
						//	Relative to the area.
						if(relativeOffset == float.MinValue)
						{
							result = area.Top +
								TranslateDirection(offset, mConfigProfile.TravelY);
						}
						else
						{
							//	Relative offset specified.
							result = relativeOffset +
								TranslateDirection(offset, mConfigProfile.TravelY);
						}
						break;
					case OffsetTopBottomEnum.Top:
						result = area.Top +
							TranslateDirection(offset, mConfigProfile.TravelY);
						break;
				}
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Translate the relative positional offset from the visual perspective
		/// to absolute positions based upon the physical anchors on the table or
		/// material.
		/// </summary>
		/// <param name="area">
		/// Reference to the area from which the offset is relative. This
		/// value represents physical coordinates.
		/// </param>
		/// <param name="subjectWidth">
		/// Width of the subject being positioned.
		/// </param>
		/// <param name="offset">
		/// The offset to polarize.
		/// </param>
		/// <param name="offsetType">
		/// The visual relative direction with which the offset is associated.
		/// </param>
		/// <param name="relativeOffset">
		/// A base offset for the entire space.
		/// </param>
		/// <returns>
		/// The physical offset, relative to the established direction of travel
		/// on the table.
		/// </returns>
		/// <remarks>
		/// This method assumes that the left and top positions constitute the
		/// local anchor of the area.
		/// </remarks>
		public static float TranslateOffset(FArea area, float subjectWidth,
			float offset, OffsetLeftRightEnum offsetType,
			float relativeOffset = float.MinValue)
		{
			float result = offset;

			if(area != null)
			{
				switch(offsetType)
				{
					case OffsetLeftRightEnum.Absolute:
						result = TransformFromAbsolute(new FVector2(offset, 0f)).X;
						break;
					case OffsetLeftRightEnum.Center:
						result = (area.Left + (area.Width / 2f) +
							TranslateDirection(offset, mConfigProfile.TravelX)) -
							(subjectWidth / 2f);
						break;
					case OffsetLeftRightEnum.LeftEdgeToCenter:
						result = area.Left + (area.Width / 2f) +
							TranslateDirection(offset, mConfigProfile.TravelX);
						break;
					case OffsetLeftRightEnum.RightEdgeToCenter:
						result = (area.Left + (area.Width / 2f) +
							TranslateDirection(offset, mConfigProfile.TravelX)) -
							subjectWidth;
						break;
					case OffsetLeftRightEnum.Left:
						result = area.Left +
							TranslateDirection(offset, mConfigProfile.TravelX);
						break;
					case OffsetLeftRightEnum.None:
					case OffsetLeftRightEnum.Relative:
						//	Relative to the area.
						if(relativeOffset == float.MinValue)
						{
							result = area.Left +
								TranslateDirection(offset, mConfigProfile.TravelX);
						}
						else
						{
							//	Relative offset specified.
							result = relativeOffset +
								TranslateDirection(offset, mConfigProfile.TravelX);
						}
						break;
					case OffsetLeftRightEnum.Right:
						result = area.Right - subjectWidth +
							TranslateDirection(offset, mConfigProfile.TravelX);
						break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UserDataInitialize																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Initialize data in the user data folder.
		/// </summary>
		public static void UserDataInitialize()
		{
			string appDataFolder = Path.GetDirectoryName(
				Assembly.GetExecutingAssembly().Location);
			bool bCopyAll = false;
			bool bDestExists = false;
			bool bSourceNewer = false;
			string content = "";
			FileInfo destFile = null;
			DirectoryInfo directory = null;
			FileInfo[] files = null;
			ShopToolsConfigItem shopToolsConfigSource = null;
			ShopToolsConfigItem shopToolsConfigTarget = null;

			mUserDataPath =
				Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
					"ShopTools");
			if(!Path.Exists(mUserDataPath))
			{
				bCopyAll = true;
				Directory.CreateDirectory(mUserDataPath);
			}
			if(Path.Exists(mUserDataPath))
			{
				directory =
					new DirectoryInfo(Path.Combine(appDataFolder, "Configuration"));
				files = directory.GetFiles();
				foreach(FileInfo fileItem in files)
				{
					bSourceNewer = false;
					destFile = new FileInfo(Path.Combine(mUserDataPath, fileItem.Name));
					bDestExists = destFile.Exists;
					if(!bDestExists)
					{
						bSourceNewer = true;
					}
					else
					{
						//	Destination file exists.
						if(fileItem.LastWriteTimeUtc.CompareTo(
							destFile.LastWriteTimeUtc) > 0)
						{
							//	Source is newer.
							if(fileItem.Name == "ShopToolsConfig.json")
							{
								//	When updating ShopTools configuration, make sure to
								//	only update the target properties that have not yet been
								//	set.
								content = File.ReadAllText(fileItem.FullName);
								shopToolsConfigSource =
									JsonConvert.DeserializeObject<ShopToolsConfigItem>(content);
								content = File.ReadAllText(destFile.FullName);
								shopToolsConfigTarget =
									JsonConvert.DeserializeObject<ShopToolsConfigItem>(content);
								//	Skip pattern templates.
								//	Process properties.
								foreach(PropertyItem propertyItem in
									shopToolsConfigSource.Properties)
								{
									if(!shopToolsConfigSource.Properties.Exists(x =>
										x.Name == propertyItem.Name))
									{
										shopToolsConfigTarget.Properties.Add(propertyItem);
										bSourceNewer = true;
									}
								}
								//	Tool type definitions are sourced elsewhere.
								//	Skip user tools.
								if(bSourceNewer)
								{
									//	Save the changes to the target file.
									content = JsonConvert.SerializeObject(shopToolsConfigTarget);
									File.WriteAllText(destFile.FullName, content);
									bSourceNewer = false;
								}
							}
							else
							{
								bSourceNewer = true;
							}
						}
					}
					if(bCopyAll || bSourceNewer)
					{
						fileItem.CopyTo(Path.Combine(mUserDataPath, fileItem.Name), true);
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserDataPath																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="UserDataPath">UserDataPath</see>.
		/// </summary>
		private static string mUserDataPath = "";
		/// <summary>
		/// Get/Set the user's configuration data path.
		/// </summary>
		public static string UserDataPath
		{
			get { return mUserDataPath; }
			set { mUserDataPath = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* WriteConfiguration																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Write the contents of the configuration file.
		/// </summary>
		public static void WriteConfiguration()
		{
			string content = "";

			if(mConfigurationFilename?.Length > 0)
			{
				content = JsonConvert.SerializeObject(mConfigProfile);
				File.WriteAllText(mConfigurationFilename, content);
			}
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
