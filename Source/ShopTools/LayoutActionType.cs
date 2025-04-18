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
	//*	LayoutActionType																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of recognized layout actions.
	/// </summary>
	public enum LayoutActionType
	{
		/// <summary>
		/// No layout action defined or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// Plot an arc.
		/// </summary>
		DrawArc,
		/// <summary>
		/// Plot an ellipse.
		/// </summary>
		DrawEllipse,
		/// <summary>
		/// Plot a line.
		/// </summary>
		DrawLine,
		/// <summary>
		/// Plot a rectangle.
		/// </summary>
		DrawRectangle,
		/// <summary>
		/// Plot a filled ellipse.
		/// </summary>
		FillEllipse,
		/// <summary>
		/// Plot a filled rectangle.
		/// </summary>
		FillRectangle,
		/// <summary>
		/// Move the tool explicitly as a part of the project.
		/// </summary>
		MoveExplicit,
		/// <summary>
		/// Move the tool implicitly as a part of the present action.
		/// </summary>
		MoveImplicit,
		/// <summary>
		/// Drill a hole.
		/// </summary>
		Point
	}
	//*-------------------------------------------------------------------------*

}
