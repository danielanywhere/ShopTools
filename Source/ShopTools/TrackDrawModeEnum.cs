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
	//*	TrackDrawModeEnum																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of available track drawing modes.
	/// </summary>
	public enum TrackDrawModeEnum
	{
		/// <summary>
		/// Don't draw tracks or mode undefined.
		/// </summary>
		None = 0,
		/// <summary>
		/// Draw all of the tracks. No animation active.
		/// </summary>
		DrawAllTracks,
		/// <summary>
		/// Begin with all tracks hiddent and draw the tracks as they become
		/// visited during the animation.
		/// </summary>
		DrawWhenVisited,
		/// <summary>
		/// Begin with all tracks drawn and hide each one when it has become
		/// visited.
		/// </summary>
		HideWhenVisited
	}
	//*-------------------------------------------------------------------------*

}
