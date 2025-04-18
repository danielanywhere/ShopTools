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
	//*	DirectionLeftRightEnum																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of restricted left and right directions.
	/// </summary>
	public enum DirectionLeftRightEnum
	{
		/// <summary>
		/// No direction specified or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// Center.
		/// </summary>
		Center,
		/// <summary>
		/// Left.
		/// </summary>
		Left,
		/// <summary>
		/// Right.
		/// </summary>
		Right
	}
	//*-------------------------------------------------------------------------*

}
