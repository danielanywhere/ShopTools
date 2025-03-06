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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geometry;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	OperationLayoutCollection																								*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of OperationLayoutItem Items.
	/// </summary>
	public class OperationLayoutCollection : List<OperationLayoutItem>
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
		//* AddArcCenterOffsetXY																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add an arc to the operation layout, given the center, the start point,
		/// and the approximate end point.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation for which the layout is being prepared.
		/// </param>
		/// <param name="center">
		/// Reference to the center offset.
		/// </param>
		/// <param name="startOffset">
		/// Reference to the starting offset at which the arc will be drawn.
		/// </param>
		/// <param name="endOffset">
		/// Reference to the approximate ending offset at which the arc will be
		/// drawn. The actual ending offset will be calculated by the angle of
		/// the provided ending offset, using the radius as a line distance.
		/// </param>
		/// <param name="winding">
		/// The winding direction to use for the arc sweep. Choices are clockwise and
		/// counterclockwise.
		/// </param>
		/// <param name="location">
		/// Reference to the last known traveling tool location.
		/// </param>
		/// <returns>
		/// Reference to the updated last known tool location, if found.
		/// Otherwise, an empty coordinate.
		/// </returns>
		public static FPoint AddArcCenterOffsetXY(
			PatternOperationItem operation,
			FPoint center, FPoint startOffset, FPoint endOffset,
			WindingOrientationEnum winding,
			FPoint location)
		{
			float angleEnd = 0f;
			float angleStart = 0f;
			FPoint boxBR = null;
			FPoint boxTL = null;
			float distance = 0f;
			OperationLayoutItem element = null;
			FPoint actualEndOffset = null;
			FPoint localLocation = null;
			float radius = 0f;
			FPoint result = null;
			float sweepAngle = 0f;

			if(operation != null && startOffset != null && endOffset != null &&
				location != null)
			{
				localLocation = new FPoint(location);
				distance = Trig.GetLineDistance(location, startOffset);
				if(Math.Abs(distance) > 0f)
				{
					//	Transit to the start point.
					element = new OperationLayoutItem()
					{
						ActionType = LayoutActionType.Move,
						Operation = operation,
						DisplayStartOffset = new FPoint(localLocation),
						DisplayEndOffset = new FPoint(startOffset),
						ToolStartOffset = new FPoint(localLocation),
						ToolEndOffset = new FPoint(startOffset)
					};
					operation.LayoutElements.Add(element);
					FPoint.TransferValues(element.ToolEndOffset, localLocation);
				}
				//	Plot the shape.
				radius = Math.Abs(Trig.GetLineDistance(center, startOffset));
				angleStart = Trig.GetLineAngle(center, startOffset);
				angleEnd = Trig.GetLineAngle(center, endOffset);
				if(winding == WindingOrientationEnum.Clockwise ||
					winding == WindingOrientationEnum.None)
				{
					//	Forward in drawing space.
					if(angleStart > angleEnd)
					{
						sweepAngle = (angleEnd + GeometryUtil.TwoPi) - angleStart;
					}
					else
					{
						sweepAngle = (angleEnd - angleStart);
					}
				}
				else
				{
					//	Reverse in drawing space.
					if(angleEnd > angleStart)
					{
						sweepAngle = angleEnd - (angleStart + GeometryUtil.TwoPi);
					}
					else
					{
						sweepAngle = angleEnd - angleStart;
					}
				}
				sweepAngle = Trig.GetLineAngle(center, endOffset) - angleStart;
				actualEndOffset =
					Trig.GetDestPoint(center, angleStart + sweepAngle, radius);
				boxTL = new FPoint(center.X - radius, center.Y - radius);
				boxBR = new FPoint(center.X + radius, center.Y + radius);
				element = new OperationLayoutItem()
				{
					ActionType = LayoutActionType.DrawArc,
					Operation = operation,
					DisplayStartOffset = new FPoint(boxTL),
					DisplayEndOffset = new FPoint(boxBR),
					StartAngle = angleStart,
					SweepAngle = sweepAngle,
					ToolStartOffset = new FPoint(startOffset),
					ToolEndOffset = new FPoint(actualEndOffset)
				};
				operation.LayoutElements.Add(element);
				result = new FPoint(element.ToolEndOffset);
			}
			if(result == null)
			{
				result = new FPoint();
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddArcCenterOffsetXYAngle																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add an arc to the operation layout, given the center, the start point,
		/// and the approximate end point.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation for which the layout is being prepared.
		/// </param>
		/// <param name="center">
		/// Reference to the center offset.
		/// </param>
		/// <param name="startOffset">
		/// Reference to the starting offset at which the arc will be drawn.
		/// </param>
		/// <param name="sweepAngle">
		/// The clockwise sweep angle to apply, in radians.
		/// </param>
		/// <param name="location">
		/// Reference to the last known traveling tool location.
		/// </param>
		/// <returns>
		/// Reference to the updated last known tool location, if found.
		/// Otherwise, an empty coordinate.
		/// </returns>
		public static FPoint AddArcCenterOffsetXYAngle(
			PatternOperationItem operation,
			FPoint center, FPoint startOffset, float sweepAngle, FPoint location)
		{
			float angleStart = 0f;
			FPoint boxBR = null;
			FPoint boxTL = null;
			float distance = 0f;
			OperationLayoutItem element = null;
			FPoint actualEndOffset = null;
			FPoint localLocation = null;
			float radius = 0f;
			FPoint result = null;

			if(operation != null && startOffset != null && sweepAngle != 0f &&
				location != null)
			{
				localLocation = new FPoint(location);
				distance = Trig.GetLineDistance(location, startOffset);
				if(Math.Abs(distance) > 0f)
				{
					//	Transit to the start point.
					element = new OperationLayoutItem()
					{
						ActionType = LayoutActionType.Move,
						Operation = operation,
						DisplayStartOffset = new FPoint(localLocation),
						DisplayEndOffset = new FPoint(startOffset),
						ToolStartOffset = new FPoint(localLocation),
						ToolEndOffset = new FPoint(startOffset)
					};
					operation.LayoutElements.Add(element);
					FPoint.TransferValues(element.ToolEndOffset, localLocation);
				}
				//	Plot the shape.
				radius = Math.Abs(Trig.GetLineDistance(center, startOffset));
				angleStart = Trig.GetLineAngle(center, startOffset);
				actualEndOffset =
					Trig.GetDestPoint(center, angleStart + sweepAngle, radius);
				boxTL = new FPoint(center.X - radius, center.Y - radius);
				boxBR = new FPoint(center.X + radius, center.Y + radius);
				element = new OperationLayoutItem()
				{
					ActionType = LayoutActionType.DrawArc,
					Operation = operation,
					DisplayStartOffset = new FPoint(boxTL),
					DisplayEndOffset = new FPoint(boxBR),
					StartAngle = angleStart,
					SweepAngle = sweepAngle,
					ToolStartOffset = new FPoint(startOffset),
					ToolEndOffset = new FPoint(actualEndOffset)
				};
				operation.LayoutElements.Add(element);
				result = new FPoint(element.ToolEndOffset);
			}
			if(result == null)
			{
				result = new FPoint();
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddArcCenterRadiusStartSweepAngle																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add an arc to the layout given its center coordinate, radius, start
		/// angle and sweep angle.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation for which the layout is being prepared.
		/// </param>
		/// <param name="center">
		/// Reference to the center offset.
		/// </param>
		/// <param name="radius">
		/// The radius of the circle upon which the arc is based.
		/// </param>
		/// <param name="startAngle">
		/// The clockwise start angle of the arc, in radians.
		/// </param>
		/// <param name="sweepAngle">
		/// The sweep angle of the arc, in radians.
		/// </param>
		/// <param name="location">
		/// Reference to the last known traveling tool location.
		/// </param>
		/// <returns>
		/// Reference to the updated last known tool location, if found.
		/// Otherwise, an empty coordinate.
		/// </returns>
		public static FPoint AddArcCenterRadiusStartSweepAngle(
			PatternOperationItem operation,
			FPoint center, float radius, float startAngle, float sweepAngle,
			FPoint location)
		{
			FPoint boxBR = null;
			FPoint boxTL = null;
			float distance = 0f;
			OperationLayoutItem element = null;
			FPoint actualEndOffset = null;
			FPoint localLocation = null;
			FPoint result = null;
			FPoint startOffset = null;

			if(operation != null && radius != 0f &&
				sweepAngle != 0f && location != null)
			{
				localLocation = new FPoint(location);
				startOffset = Trig.GetDestPoint(center, startAngle, radius);
				distance = Trig.GetLineDistance(location, startOffset);
				if(Math.Abs(distance) > 0f)
				{
					//	Transit to the start point.
					element = new OperationLayoutItem()
					{
						ActionType = LayoutActionType.Move,
						Operation = operation,
						DisplayStartOffset = new FPoint(localLocation),
						DisplayEndOffset = new FPoint(startOffset),
						ToolStartOffset = new FPoint(localLocation),
						ToolEndOffset = new FPoint(startOffset)
					};
					operation.LayoutElements.Add(element);
					FPoint.TransferValues(element.ToolEndOffset, localLocation);
				}
				//	Plot the shape.
				actualEndOffset =
					Trig.GetDestPoint(center, startAngle + sweepAngle, radius);
				boxTL = new FPoint(center.X - radius, center.Y - radius);
				boxBR = new FPoint(center.X + radius, center.Y + radius);
				element = new OperationLayoutItem()
				{
					ActionType = LayoutActionType.DrawArc,
					Operation = operation,
					DisplayStartOffset = new FPoint(boxTL),
					DisplayEndOffset = new FPoint(boxBR),
					StartAngle = startAngle,
					SweepAngle = sweepAngle,
					ToolStartOffset = new FPoint(startOffset),
					ToolEndOffset = new FPoint(actualEndOffset)
				};
				operation.LayoutElements.Add(element);
				result = new FPoint(element.ToolEndOffset);
			}
			if(result == null)
			{
				result = new FPoint();
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddEllipseCenterRadius																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add an ellipse shape to the operation layout, given the center and
		/// radius.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation for which the layout is being prepared.
		/// </param>
		/// <param name="center">
		/// Reference to the center offset of the ellipse.
		/// </param>
		/// <param name="radiusX">
		/// The X-axis radius of the ellipse.
		/// </param>
		/// <param name="radiusY">
		/// The Y-axis radius of the ellipse.
		/// </param>
		/// <param name="location">
		/// Reference to the last known traveling tool location.
		/// </param>
		/// <param name="filled">
		/// Value indicating whether the shape will be filled.
		/// </param>
		/// <returns>
		/// Reference to the updated last known tool location, if found.
		/// Otherwise, an empty coordinate.
		/// </returns>
		public static FPoint AddEllipseCenterRadius(
			PatternOperationItem operation,
			FPoint center, float radiusX, float radiusY, FPoint location,
			bool filled)
		{
			float angle = 0f;
			float distance = 0f;
			OperationLayoutItem element = null;
			FPoint intersection = null;
			FPoint[] intersections = null;
			FPoint localLocation = null;
			FPoint outside = null;
			FPoint result = null;

			if(operation != null && center != null &&
				radiusX != 0f && radiusY != 0f && location != null)
			{
				localLocation = new FPoint(location);
				//	Transit to the nearest side.
				angle = Trig.GetLineAngle(center, localLocation);
				distance = (float)Math.Max(radiusX, radiusY) * 2f;
				//	Point guaranteed to be outside the ellipse.
				outside = Trig.GetDestPoint(center, angle, distance);
				intersections = FEllipse.FindIntersections(center, radiusX, radiusY,
					new FLine(center, outside));
				if(intersections.Length > 0)
				{
					intersection = intersections[0];
				}
				else
				{
					intersection = FPoint.Clone(localLocation);
				}
				element = new OperationLayoutItem()
				{
					ActionType = LayoutActionType.Move,
					Operation = operation,
					DisplayStartOffset = new FPoint(localLocation),
					DisplayEndOffset = new FPoint(intersection),
					ToolStartOffset = new FPoint(localLocation),
					ToolEndOffset = new FPoint(intersection)
				};
				operation.LayoutElements.Add(element);
				FPoint.TransferValues(element.ToolEndOffset, localLocation);
				//	Plot the shape.
				element = new OperationLayoutItem()
				{
					ActionType = (filled ?
						LayoutActionType.FillEllipse : LayoutActionType.DrawEllipse),
					Operation = operation,
					DisplayStartOffset = new FPoint(
						center.X - radiusX, center.Y - radiusY),
					DisplayEndOffset = new FPoint(
						center.X + radiusX, center.Y + radiusY),
					ToolStartOffset = new FPoint(localLocation)
				};
				element.ToolEndOffset = new FPoint(element.ToolStartOffset);
				operation.LayoutElements.Add(element);
				result = new FPoint(element.ToolEndOffset);
			}
			if(result == null)
			{
				result = new FPoint(location);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddEllipseCornerRadius																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add an ellipse shape to the operation layout, given the corner
		/// coordinate and radius.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation for which the layout is being prepared.
		/// </param>
		/// <param name="corner">
		/// Reference to the corner offset of the ellipse.
		/// </param>
		/// <param name="radiusX">
		/// The X-axis radius of the ellipse.
		/// </param>
		/// <param name="radiusY">
		/// The Y-axis radius of the ellipse.
		/// </param>
		/// <param name="location">
		/// Reference to the last known traveling tool location.
		/// </param>
		/// <param name="filled">
		/// Value indicating whether the shape will be filled.
		/// </param>
		/// <returns>
		/// Reference to the updated last known tool location, if found.
		/// Otherwise, an empty coordinate.
		/// </returns>
		public static FPoint AddEllipseCornerRadius(
			PatternOperationItem operation,
			FPoint corner, float radiusX, float radiusY, FPoint location,
			bool filled)
		{
			FPoint center = null;
			FPoint result = null;

			if(operation != null && corner != null &&
				radiusX != 0f && radiusY != 0f && location != null)
			{
				center = new FPoint(corner.X + radiusX, corner.Y + radiusY);
				result = AddEllipseCenterRadius(operation, center, radiusX, radiusY,
					location, filled);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddLine																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a line shape to the operation layout, given the starting and
		/// ending points.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation for which the layout is being prepared.
		/// </param>
		/// <param name="startPoint">
		/// Reference to the start point of the line.
		/// </param>
		/// <param name="endPoint">
		/// Reference to the end point of the line.
		/// </param>
		/// <param name="location">
		/// Reference to the last known traveling tool location.
		/// </param>
		/// <returns>
		/// Reference to the current last known traveling tool location.
		/// </returns>
		public static FPoint AddLine(PatternOperationItem operation,
			FPoint startPoint, FPoint endPoint, FPoint location)
		{
			float dx = 0f;
			float dy = 0f;
			OperationLayoutItem element = null;
			FPoint result = null;

			if(operation != null && startPoint != null && endPoint != null &&
				((endPoint.X - startPoint.X) != 0f ||
				(endPoint.Y - startPoint.Y) != 0f) &&
				location != null)
			{
				dx = startPoint.X - location.X;
				dy = startPoint.Y - location.Y;
				if(dx != 0f || dy != 0f)
				{
					//	Transit to the starting location.
					element = new OperationLayoutItem()
					{
						ActionType = LayoutActionType.Move,
						Operation = operation,
						DisplayStartOffset = new FPoint(location),
						DisplayEndOffset = new FPoint(startPoint)
					};
					element.ToolStartOffset = new FPoint(element.DisplayStartOffset);
					element.ToolEndOffset = new FPoint(element.DisplayEndOffset);
					operation.LayoutElements.Add(element);
				}
				//	Draw the line.
				element = new OperationLayoutItem()
				{
					ActionType = LayoutActionType.DrawLine,
					Operation = operation,
					DisplayStartOffset = new FPoint(startPoint),
					DisplayEndOffset = new FPoint(endPoint)
				};
				element.ToolStartOffset = new FPoint(element.DisplayStartOffset);
				element.ToolEndOffset = new FPoint(element.DisplayEndOffset);
				operation.LayoutElements.Add(element);
				result = new FPoint(element.ToolEndOffset);
			}
			if(result == null)
			{
				result = new FPoint(location);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddMove																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a move action to the collection.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation for which the layout is being prepared.
		/// </param>
		/// <param name="endPoint">
		/// Reference to the end point of the line.
		/// </param>
		/// <param name="location">
		/// Reference to the last known traveling tool location.
		/// </param>
		/// <returns>
		/// Reference to the current last known traveling tool location.
		/// </returns>
		public static FPoint AddMove(PatternOperationItem operation,
			FPoint endPoint, FPoint location)
		{
			OperationLayoutItem element = null;
			FPoint result = null;

			if(operation != null && endPoint != null && location != null)
			{
				//if(startPoint.X - location.X != 0f || startPoint.Y - location.Y != 0f)
				//{
				//	//	Transit to the starting location.
				//	element = new OperationLayoutItem()
				//	{
				//		ActionType = LayoutActionType.Move,
				//		Operation = operation,
				//		DisplayStartOffset = new FPoint(location),
				//		DisplayEndOffset = new FPoint(startPoint),
				//		ToolStartOffset = new FPoint(location),
				//		ToolEndOffset = new FPoint(startPoint)
				//	};
				//	operation.LayoutElements.Add(element);
				//}
				//	Perform the expected move.
				element = new OperationLayoutItem()
				{
					ActionType = LayoutActionType.Move,
					Operation = operation,
					DisplayStartOffset = new FPoint(location),
					DisplayEndOffset = new FPoint(endPoint),
					ToolStartOffset = new FPoint(location),
					ToolEndOffset = new FPoint(endPoint)
				};
				operation.LayoutElements.Add(element);
				result = new FPoint(element.ToolEndOffset);
			}
			if(result == null)
			{
				result = new FPoint(location);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddPoint																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a point action to the collection.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation for which the layout is being prepared.
		/// </param>
		/// <param name="offset">
		/// Reference to the point offset.
		/// </param>
		/// <param name="location">
		/// Reference to the last known traveling tool location.
		/// </param>
		/// <returns>
		/// Reference to the current last known traveling tool location.
		/// </returns>
		public static FPoint AddPoint(PatternOperationItem operation,
			FPoint offset, FPoint location)
		{
			OperationLayoutItem element = null;
			FPoint result = null;

			if(operation != null && offset != null && location != null)
			{
				if(offset.X - location.X != 0f || offset.Y - location.Y != 0f)
				{
					//	Transit to the starting location.
					element = new OperationLayoutItem()
					{
						ActionType = LayoutActionType.Move,
						Operation = operation,
						DisplayStartOffset = new FPoint(location),
						DisplayEndOffset = new FPoint(offset),
						ToolStartOffset = new FPoint(location),
						ToolEndOffset = new FPoint(offset)
					};
					operation.LayoutElements.Add(element);
				}
				//	Perform the expected move.
				element = new OperationLayoutItem()
				{
					ActionType = LayoutActionType.Point,
					Operation = operation,
					DisplayStartOffset = new FPoint(offset),
					DisplayEndOffset = new FPoint(offset),
					ToolStartOffset = new FPoint(offset),
					ToolEndOffset = new FPoint(offset)
				};
				operation.LayoutElements.Add(element);
				result = new FPoint(element.ToolEndOffset);
			}
			if(result == null)
			{
				result = new FPoint(location);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddRectangleCenterXY																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a rectangle shape to the operation layout, given the center and
		/// X/Y lengths.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation for which the layout is being prepared.
		/// </param>
		/// <param name="center">
		/// Reference to the center offset of the ellipse.
		/// </param>
		/// <param name="lengthX">
		/// The X-axis length of the shape.
		/// </param>
		/// <param name="lengthY">
		/// The Y-axis length of the shape.
		/// </param>
		/// <param name="location">
		/// Reference to the last known traveling tool location.
		/// </param>
		/// <param name="filled">
		/// Value indicating whether the shape should be filled.
		/// </param>
		/// <returns>
		/// Reference to the updated last known tool location, if found.
		/// Otherwise, an empty coordinate.
		/// </returns>
		public static FPoint AddRectangleCenterXY(
			PatternOperationItem operation,
			FPoint center, float lengthX, float lengthY, FPoint location,
			bool filled)
		{
			float angle = 0f;
			float distance = 0f;
			OperationLayoutItem element = null;
			FPoint intersection = null;
			FLine line = null;
			FLine[] lines = null;
			FPoint localLocation = null;
			FPoint outside = null;
			FPoint result = null;
			float x1 = 0f;
			float x2 = 0f;
			float y1 = 0f;
			float y2 = 0f;

			if(operation != null && center != null &&
				lengthX != 0f && lengthY != 0f && location != null)
			{
				localLocation = new FPoint(location);
				x1 = center.X - (lengthX / 2f);
				y1 = center.Y - (lengthY / 2f);
				x2 = center.X + (lengthX / 2f);
				y2 = center.Y + (lengthY / 2f);
				lines = new FLine[]
				{
					new FLine(new FPoint(x1, y1), new FPoint(x2, y1)),
					new FLine(new FPoint(x1, y1), new FPoint(x1, y2)),
					new FLine(new FPoint(x2, y1), new FPoint(x2, y2)),
					new FLine(new FPoint(x1, y2), new FPoint(x2, y2))
				};
				//	Transit to the nearest side.
				angle = Trig.GetLineAngle(center, localLocation);
				distance = (float)Math.Max(lengthX, lengthY) * 2f;
				//	Point guaranteed to be outside the shape.
				outside = Trig.GetDestPoint(center, angle, distance);
				line = new FLine(center, outside);
				foreach(FLine lineItem in lines)
				{
					if(FLine.HasIntersection(line, lineItem, false))
					{
						//	Intersecting line found.
						intersection = FLine.Intersect(line, lineItem, false);
						line = lineItem;
						break;
					}
				}
				if(intersection.X != float.NegativeInfinity)
				{
					//	Intersecting line to shape center found.
					//	Get the nearest point on the line to the previous location.
					intersection = FLine.GetClosestPoint(line, location);
				}
				if(intersection != null && intersection.X != float.NegativeInfinity)
				{
					element = new OperationLayoutItem()
					{
						ActionType = LayoutActionType.Move,
						Operation = operation,
						DisplayStartOffset = new FPoint(localLocation),
						DisplayEndOffset = new FPoint(intersection),
						ToolStartOffset = new FPoint(localLocation),
						ToolEndOffset = new FPoint(intersection)
					};
					operation.LayoutElements.Add(element);
					FPoint.TransferValues(element.ToolEndOffset, localLocation);
				}
				//	Plot the shape.
				element = new OperationLayoutItem()
				{
					ActionType = (filled ?
						LayoutActionType.FillRectangle :
						LayoutActionType.DrawRectangle),
					Operation = operation,
					DisplayStartOffset = new FPoint(x1, y1),
					DisplayEndOffset = new FPoint(x2, y2),
					ToolStartOffset = new FPoint(localLocation)
				};
				element.ToolEndOffset = new FPoint(element.ToolStartOffset);
				operation.LayoutElements.Add(element);
				result = new FPoint(element.ToolEndOffset);
			}
			if(result == null)
			{
				result = new FPoint(location);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AddRectangleCornerXY																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a rectangle shape to the operation layout, given the corner
		/// coordinate and X/Y lengths.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation for which the layout is being prepared.
		/// </param>
		/// <param name="corner">
		/// Reference to the corner offset of the shape.
		/// </param>
		/// <param name="lengthX">
		/// The X-axis length of the shape.
		/// </param>
		/// <param name="lengthY">
		/// The Y-axis length of the shape.
		/// </param>
		/// <param name="location">
		/// Reference to the last known traveling tool location.
		/// </param>
		/// <param name="filled">
		/// Value indicating whether the shape should be filled.
		/// </param>
		/// <returns>
		/// Reference to the updated last known tool location, if found.
		/// Otherwise, an empty coordinate.
		/// </returns>
		public static FPoint AddRectangleCornerXY(
			PatternOperationItem operation,
			FPoint corner, float lengthX, float lengthY, FPoint location,
			bool filled)
		{
			FPoint center = null;
			FPoint result = null;

			if(operation != null && corner != null &&
				lengthX != 0f && lengthY != 0f && location != null)
			{
				center = new FPoint(corner.X + (lengthX / 2f),
					corner.Y + (lengthY / 2f));
				result = AddRectangleCenterXY(operation, center, lengthX, lengthY,
					location, filled);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clone																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a deep clone of the provided collection.
		/// </summary>
		/// <param name="items">
		/// Reference to the collection of items to clone.
		/// </param>
		/// <returns>
		/// Reference to the newly cloned collection.
		/// </returns>
		public static OperationLayoutCollection Clone(
			OperationLayoutCollection items)
		{
			OperationLayoutCollection result = new OperationLayoutCollection();

			if(items?.Count > 0)
			{
				foreach(OperationLayoutItem layoutItem in items)
				{
					result.Add(OperationLayoutItem.Clone(layoutItem));
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CloneLayout																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a deep clone of the entire layout of from the caller's
		/// host object.
		/// </summary>
		/// <param name="cuts">
		/// Reference to a cut profile collection whose layout will be cloned.
		/// </param>
		/// <returns>
		/// Reference to a newly created operation layout collection containing
		/// representation of all of the layout elements in the host context, if
		/// elements were found. Otherwise, an empty collection.
		/// </returns>
		public static OperationLayoutCollection CloneLayout(
			CutProfileCollection cuts)
		{
			OperationLayoutCollection result = new OperationLayoutCollection();

			if(cuts?.Count > 0)
			{
				foreach(CutProfileItem profileItem in cuts)
				{
					foreach(PatternOperationItem operationItem in profileItem.Operations)
					{
						foreach(OperationLayoutItem layoutItem in
							operationItem.LayoutElements)
						{
							result.Add(OperationLayoutItem.Clone(layoutItem));
						}
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	OperationLayoutItem																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Definition of display and tool tracking offsets.
	/// </summary>
	public class OperationLayoutItem
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
		//*	ActionType																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ActionType">ActionType</see>.
		/// </summary>
		private LayoutActionType mActionType = LayoutActionType.None;
		/// <summary>
		/// Get/Set the action type to plot for this element.
		/// </summary>
		public LayoutActionType ActionType
		{
			get { return mActionType; }
			set { mActionType = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clone																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a deep clone of the provided operation layout item.
		/// </summary>
		/// <param name="item">
		/// Reference to the item to clone.
		/// </param>
		/// <returns>
		/// Reference to the newly cloned operation layout item.
		/// </returns>
		public static OperationLayoutItem Clone(OperationLayoutItem item)
		{
			OperationLayoutItem result = null;

			if(item != null)
			{
				result = new OperationLayoutItem()
				{
					mActionType = item.mActionType,
					mDisplayEndOffset = new FPoint(item.mDisplayEndOffset),
					mDisplayStartOffset = new FPoint(item.mDisplayStartOffset),
					mOperation = item.mOperation,
					mStartAngle = item.mStartAngle,
					mSweepAngle = item.mSweepAngle,
					mToolEndOffset = new FPoint(item.mToolEndOffset),
					mToolStartOffset = new FPoint(item.mToolStartOffset)
				};
			}
			if(result == null)
			{
				result = new OperationLayoutItem();
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	DisplayEndOffset																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="DisplayEndOffset">DisplayEndOffset</see>.
		/// </summary>
		private FPoint mDisplayEndOffset = new FPoint();
		/// <summary>
		/// Get/Set a reference to the end offset of the display.
		/// </summary>
		public FPoint DisplayEndOffset
		{
			get { return mDisplayEndOffset; }
			set { mDisplayEndOffset = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	DisplayStartOffset																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="DisplayStartOffset">DisplayStartOffset</see>.
		/// </summary>
		private FPoint mDisplayStartOffset = new FPoint();
		/// <summary>
		/// Get/Set a reference to the start offset of the display.
		/// </summary>
		public FPoint DisplayStartOffset
		{
			get { return mDisplayStartOffset; }
			set { mDisplayStartOffset = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Operation																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Operation">Operation</see>.
		/// </summary>
		private PatternOperationItem mOperation = null;
		/// <summary>
		/// Get/Set a reference to the associated operation.
		/// </summary>
		public PatternOperationItem Operation
		{
			get { return mOperation; }
			set { mOperation = value; }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	PreviousToolEndOffset																									*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for
		///// <see cref="PreviousToolEndOffset">PreviousToolEndOffset</see>.
		///// </summary>
		//private FPoint mPreviousToolEndOffset = new FPoint();
		///// <summary>
		///// Get/Set a reference to the previous end offset of the tool.
		///// </summary>
		//public FPoint PreviousToolEndOffset
		//{
		//	get { return mPreviousToolEndOffset; }
		//	set { mPreviousToolEndOffset = value; }
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	PreviousToolStartOffset																								*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for
		///// <see cref="PreviousToolStartOffset">PreviousToolStartOffset</see>.
		///// </summary>
		//private FPoint mPreviousToolStartOffset = new FPoint();
		///// <summary>
		///// Get/Set a reference to the previous start offset of the tool.
		///// </summary>
		//public FPoint PreviousToolStartOffset
		//{
		//	get { return mPreviousToolStartOffset; }
		//	set { mPreviousToolStartOffset = value; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StartAngle																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="StartAngle">StartAngle</see>.
		/// </summary>
		private float mStartAngle = 0f;
		/// <summary>
		/// Get/Set the start angle of the operation.
		/// </summary>
		public float StartAngle
		{
			get { return mStartAngle; }
			set { mStartAngle = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SweepAngle																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="SweepAngle">SweepAngle</see>.
		/// </summary>
		private float mSweepAngle = 0f;
		/// <summary>
		/// Get/Set the sweep angle of the operation.
		/// </summary>
		public float SweepAngle
		{
			get { return mSweepAngle; }
			set { mSweepAngle = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToolEndOffset																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ToolEndOffset">ToolEndOffset</see>.
		/// </summary>
		private FPoint mToolEndOffset = new FPoint();
		/// <summary>
		/// Get/Set a reference to the end offset of the tool.
		/// </summary>
		public FPoint ToolEndOffset
		{
			get { return mToolEndOffset; }
			set { mToolEndOffset = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToolStartOffset																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ToolStartOffset">ToolStartOffset</see>.
		/// </summary>
		private FPoint mToolStartOffset = new FPoint();
		/// <summary>
		/// Get/Set a reference to the start offset of the tool.
		/// </summary>
		public FPoint ToolStartOffset
		{
			get { return mToolStartOffset; }
			set { mToolStartOffset = value; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
