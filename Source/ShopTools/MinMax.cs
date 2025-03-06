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
	//*	MinMaxCollection																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of MinMaxItem Items.
	/// </summary>
	public class MinMaxCollection : List<MinMaxItem>
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


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	MinMaxItem																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Minimum and maximum values.
	/// </summary>
	public class MinMaxItem
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
		//*	Maximum																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The private member for <see cref="Maximum">Maximum</see>.
		/// </summary>
		private float mMaximum = 0f;
		/// <summary>
		/// Get/Set the maximum value.
		/// </summary>
		public float Maximum
		{
			get { return mMaximum; }
			set { mMaximum = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Minimum																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The private member for <see cref="Minimum">Minimum</see>.
		/// </summary>
		private float mMinimum = 0f;
		/// <summary>
		/// Get/Set the minimum value.
		/// </summary>
		public float Minimum
		{
			get { return mMinimum; }
			set { mMinimum = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
