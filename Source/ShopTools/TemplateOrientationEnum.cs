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
	//*	TemplateOrientationEnum																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of space orientations available when defining a cutting
	/// template.
	/// </summary>
	public enum TemplateOrientationEnum
	{
		/// <summary>
		/// No template orientation defined or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// Measurements are oriented relative to the nearest edge of the
		/// workpiece.
		/// </summary>
		Edge,
		/// <summary>
		/// Measurements are oriented relative to the last-known position of the
		/// tool.
		/// </summary>
		Relative,
		/// <summary>
		/// Measurements are oriented relative to the corner of the workpiece.
		/// </summary>
		Workpiece,
		/// <summary>
		/// Measurements are oriented global to the entire workspace.
		/// </summary>
		Workspace
	}
	//*-------------------------------------------------------------------------*

}
