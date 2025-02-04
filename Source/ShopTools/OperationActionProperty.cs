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

using Newtonsoft.Json;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	OperationActionPropertyCollection																				*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of OperationActionPropertyItem Items.
	/// </summary>
	public class OperationActionPropertyCollection :
		List<OperationActionPropertyItem>
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
	//*	OperationActionPropertyItem																							*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information that helps to define the use and behavior of properties
	/// found in pattern operation.
	/// </summary>
	public class OperationActionPropertyItem
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
		//*	DataType																															*
		//*-----------------------------------------------------------------------*
		private string mDataType = "";
		/// <summary>
		/// Get/Set the data type associated with this property.
		/// </summary>
		[JsonProperty(Order = 1)]		
		public string DataType
		{
			get { return mDataType; }
			set { mDataType = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ExcludeOperationActions																								*
		//*-----------------------------------------------------------------------*
		private List<string> mExcludeOperationActions = new List<string>();
		/// <summary>
		/// Get a reference to the list of operation actions from which this
		/// property name will be excluded.
		/// </summary>
		/// <remarks>
		/// The items in this list are removed from the list of all or included
		/// actions. If only an excluded actions list is provided, it is assumed
		/// that all of the actions except those listed will be associated with
		/// this property.
		/// </remarks>
		[JsonProperty(Order = 5)]
		public List<string> ExcludeOperationActions
		{
			get { return mExcludeOperationActions; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IncludeOperationActions																								*
		//*-----------------------------------------------------------------------*
		private List<string> mIncludeOperationActions = new List<string>();
		/// <summary>
		/// Get a reference to the list of operation actions into which this
		/// property name will be included.
		/// </summary>
		/// <remarks>
		/// If no entries are made here, then it is assumed the property will
		/// be associated with all actions.
		/// </remarks>
		[JsonProperty(Order = 4)]
		public List<string> IncludeOperationActions
		{
			get { return mIncludeOperationActions; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Internal																															*
		//*-----------------------------------------------------------------------*
		private bool mInternal = false;
		/// <summary>
		/// Get/Set a value indicating whether this property is only used for
		/// internal processing.
		/// </summary>
		[JsonProperty(Order = 2)]		
		public bool Internal
		{
			get { return mInternal; }
			set { mInternal = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PropertyName																													*
		//*-----------------------------------------------------------------------*
		private string mPropertyName = "";
		/// <summary>
		/// Get/Set the name of the operation property.
		/// </summary>
		[JsonProperty(Order = 0)]
		public string PropertyName
		{
			get { return mPropertyName; }
			set { mPropertyName = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SortIndex																															*
		//*-----------------------------------------------------------------------*
		private int mSortIndex = 0;
		/// <summary>
		/// Get/Set the sorting index of this property within user interactions.
		/// </summary>
		[JsonProperty(Order = 3)]		
		public int SortIndex
		{
			get { return mSortIndex; }
			set { mSortIndex = value; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
