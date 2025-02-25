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

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	OperationActionEnum																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of recognized cut types.
	/// </summary>
	public enum OperationActionEnum
	{
		/// <summary>
		/// No cut type defined or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// Draw an arc from the specified start coordinate to the point nearest
		/// the specified end point, given the radius from the center to the
		/// starting offset.
		/// </summary>
		DrawArcCenterOffsetXY,
		/// <summary>
		/// Draw an arc using a center coordinate, an offset starting coordinate,
		/// and a sweep angle.
		/// </summary>
		DrawArcCenterOffsetXYAngle,
		/// <summary>
		/// Draw an arc using a center coordinate, a radius, a start angle, and a
		/// sweep angle.
		/// </summary>
		DrawArcCenterRadiusStartSweepAngle,
		/// <summary>
		/// Draw a circle using a center reference point and diameter.
		/// </summary>
		DrawCircleCenterDiameter,
		/// <summary>
		/// Draw a circle using a center reference point and radius.
		/// </summary>
		DrawCircleCenterRadius,
		/// <summary>
		/// Draw a circle using corner X, Y references and diameter.
		/// </summary>
		DrawCircleDiameter,
		/// <summary>
		/// Draw a circle using corner X, Y references and radius.
		/// </summary>
		DrawCircleRadius,
		/// <summary>
		/// Draw an ellipse using a center reference point and independent
		/// diameters.
		/// </summary>
		DrawEllipseCenterDiameterXY,
		/// <summary>
		/// Draw an ellipse using a center reference point and independent
		/// radii.
		/// </summary>
		DrawEllipseCenterRadiusXY,
		/// <summary>
		/// Draw an ellipse using corner X, Y references and independent
		/// diameters.
		/// </summary>
		DrawEllipseDiameterXY,
		/// <summary>
		/// Draw an ellipse using corner X, Y starting references, length, and
		/// width.
		/// </summary>
		DrawEllipseLengthWidth,
		/// <summary>
		/// Draw an ellipse using corner X, Y references and independent
		/// radii.
		/// </summary>
		DrawEllipseRadiusXY,
		/// <summary>
		/// Draw an ellipse using starting and ending X, Y coordinates.
		/// </summary>
		DrawEllipseXY,
		/// <summary>
		/// Draw a line using a point, an angle, and a length.
		/// </summary>
		DrawLineAngleLength,
		/// <summary>
		/// Draw a line using a point, a length, and a width.
		/// </summary>
		DrawLineLengthWidth,
		/// <summary>
		/// Draw a line using two points.
		/// </summary>
		DrawLineXY,
		/// <summary>
		/// Draw the path specified in the PathData property.
		/// </summary>
		DrawPath,
		/// <summary>
		/// Draw a rectangle using a center coordinate, length, and width.
		/// </summary>
		DrawRectangleCenterLengthWidth,
		/// <summary>
		/// Draw a rectangle using a corner, length, and width.
		/// </summary>
		DrawRectangleLengthWidth,
		/// <summary>
		/// Draw a rectangle using two corner points.
		/// </summary>
		DrawRectangleXY,
		/// <summary>
		/// Fill a circle using the center reference and a diameter.
		/// </summary>
		FillCircleCenterDiameter,
		/// <summary>
		/// Fill a circle using its center reference and a radius.
		/// </summary>
		FillCircleCenterRadius,
		/// <summary>
		/// Fill a circle using its corner reference and a diameter.
		/// </summary>
		FillCircleDiameter,
		/// <summary>
		/// Fill a circle using its corner reference and a radius.
		/// </summary>
		FillCircleRadius,
		/// <summary>
		/// Fill an ellipse using a center reference point and independent
		/// diameter values.
		/// </summary>
		FillEllipseCenterDiameterXY,
		/// <summary>
		/// Fill an ellipse using a center reference point and independent
		/// radii.
		/// </summary>
		FillEllipseCenterRadiusXY,
		/// <summary>
		/// Fill an ellipse using a corner point and independent diameter values.
		/// </summary>
		FillEllipseDiameterXY,
		/// <summary>
		/// Fill an ellipse using corner X, Y starting references, length, and
		/// width.
		/// </summary>
		FillEllipseLengthWidth,
		/// <summary>
		/// Fill an ellipse using a corner point and independent radii.
		/// </summary>
		FillEllipseRadiusXY,
		/// <summary>
		/// Fill an ellipse using starting and ending X, Y coordinates.
		/// </summary>
		FillEllipseXY,
		/// <summary>
		/// Fill the path specified in the PathData property.
		/// </summary>
		FillPath,
		/// <summary>
		/// Fill a rectangle using a center coordinate, length, and width.
		/// </summary>
		FillRectangleCenterLengthWidth,
		/// <summary>
		/// Fill a rectangle using one corner, width, and height.
		/// </summary>
		FillRectangleLengthWidth,
		/// <summary>
		/// Fill a rectangle using two corners.
		/// </summary>
		FillRectangleXY,
		/// <summary>
		///	Move the bit, without cutting, at an angle, by a specified length.
		/// </summary>
		MoveAngleLength,
		/// <summary>
		/// Move the tool, without cutting, to the specified coordinate.
		/// </summary>
		MoveXY,
		/// <summary>
		/// Drill at a point represented by the X and Y coordinates.
		/// </summary>
		PointXY
	}
	//*-------------------------------------------------------------------------*


}
