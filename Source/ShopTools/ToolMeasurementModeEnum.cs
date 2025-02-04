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
	//*	ToolMeasurementModeEnum																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of recognized tool measurement modes.
	/// </summary>
	public enum ToolMeasurementModeEnum
	{
		/// <summary>
		/// No measurement mode has been defined or is unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// Measurement is absolute from the corner of the material. This would
		/// be the same as material corner relative.
		/// </summary>
		MaterialAbsolute,
		/// <summary>
		/// Measurement is relative from the center of the template part.
		/// </summary>
		MaterialCenter,
		/// <summary>
		/// Measurement is absolute from the corner of the template part. This
		/// would be the same as part corner relative.
		/// </summary>
		PartAbsolute,
		/// <summary>
		/// Measurement is relative to the center of the template part.
		/// </summary>
		PartCenter,
		/// <summary>
		/// Measurement is purely relative from the current location of the
		/// tool.
		/// </summary>
		Relative,
		/// <summary>
		/// Measurement is absolute from the configured origin of the workspace
		/// canvas. In this mode, the absolute global positioning of the router is
		/// used directly.
		/// </summary>
		WorkspaceAbsolute,
		/// <summary>
		/// Measurement is relative to the center of the workspace, which might
		/// or might not also be the configured origin.
		/// </summary>
		WorkspaceCenter
	}
	//*-------------------------------------------------------------------------*


}
