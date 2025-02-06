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
	//*	ToolTypeDefinitionCollection																						*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of ToolTypeDefinitionItem Items.
	/// </summary>
	public class ToolTypeDefinitionCollection : List<ToolTypeDefinitionItem>
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
		//* Clone																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a deep clone of the items in the caller's collection.
		/// </summary>
		/// <param name="items">
		/// Reference to the collection of items to be cloned.
		/// </param>
		/// <returns>
		/// Reference to a newly created collection containing deep clones
		/// of the members of the caller's original collection.
		/// </returns>
		public static ToolTypeDefinitionCollection Clone(
			ToolTypeDefinitionCollection items)
		{
			ToolTypeDefinitionItem item = null;
			ToolTypeDefinitionCollection result = new ToolTypeDefinitionCollection();

			if(items?.Count > 0)
			{
				foreach(ToolTypeDefinitionItem defItem in items)
				{
					item = new ToolTypeDefinitionItem()
					{
						Supported = defItem.Supported,
						ToolType = defItem.ToolType
					};
					foreach(string propertyNameItem in defItem.PublishedProperties)
					{
						item.PublishedProperties.Add(propertyNameItem);
					}
					result.Add(item);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	ToolTypeDefinitionItem																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// An individual tool type definition.
	/// </summary>
	public class ToolTypeDefinitionItem
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
		//*	Groups																																*
		//*-----------------------------------------------------------------------*
		private List<string> mGroups = new List<string>();
		/// <summary>
		/// Get a reference to a list of names of conceptual groups to which this
		/// tool belongs.
		/// </summary>
		public List<string> Groups
		{
			get { return mGroups; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PublishedProperties																										*
		//*-----------------------------------------------------------------------*
		private List<string> mPublishedProperties = new List<string>();
		/// <summary>
		/// Get a reference to the list of property names published for this type
		/// of tool.
		/// </summary>
		[JsonProperty(Order = 2)]
		public List<string> PublishedProperties
		{
			get { return mPublishedProperties; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Supported																															*
		//*-----------------------------------------------------------------------*
		private bool mSupported = true;
		/// <summary>
		/// Get/Set a value indicating whether this tool type is currently
		/// supported.
		/// </summary>
		[JsonProperty(Order = 1)]
		public bool Supported
		{
			get { return mSupported; }
			set { mSupported = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToolType																															*
		//*-----------------------------------------------------------------------*
		private string mToolType = "";
		/// <summary>
		/// Get/Set the name of the tool type.
		/// </summary>
		[JsonProperty(Order = 0)]
		public string ToolType
		{
			get { return mToolType; }
			set { mToolType = value; }
		}
		//*-----------------------------------------------------------------------*



	}
	//*-------------------------------------------------------------------------*

}
