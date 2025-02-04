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
	//*	OriginLocationEnum																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of general locations for an origin.
	/// </summary>
	public enum OriginLocationEnum
	{
		/// <summary>
		/// No origin defined or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// Bottom location.
		/// </summary>
		Bottom,
		/// <summary>
		/// Bottom left location.
		/// </summary>
		BottomLeft,
		/// <summary>
		/// Bottom right location.
		/// </summary>
		BottomRight,
		/// <summary>
		/// Center location.
		/// </summary>
		Center,
		/// <summary>
		/// Left location.
		/// </summary>
		Left,
		/// <summary>
		/// Right location.
		/// </summary>
		Right,
		/// <summary>
		/// Top location.
		/// </summary>
		Top,
		/// <summary>
		/// Top left location.
		/// </summary>
		TopLeft,
		/// <summary>
		/// Top right location.
		/// </summary>
		TopRight
	}
	//*-------------------------------------------------------------------------*

}
