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
	//*	UserToolCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of UserToolItem Items.
	/// </summary>
	public class UserToolCollection : List<UserToolItem>
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
		/// Return a memberwise clone of all of the items in the caller's
		/// collection.
		/// </summary>
		/// <param name="items">
		/// Reference to the collection of items to be cloned.
		/// </param>
		/// <returns>
		/// Reference to a new collection containing memberwise clones of the
		/// members of the original collection.
		/// </returns>
		public static UserToolCollection Clone(UserToolCollection items)
		{
			UserToolCollection result = new UserToolCollection();
			UserToolItem tool = null;

			if(items?.Count > 0)
			{
				foreach(UserToolItem toolItem in items)
				{
					tool = new UserToolItem()
					{
						ToolId = toolItem.ToolId,
						ToolName = toolItem.ToolName,
						ToolType = toolItem.ToolType
					};
					tool.Properties.AddRange(
						PropertyCollection.Clone(toolItem.Properties));
					result.Add(tool);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	UserToolItem																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Definition of an individual user tool.
	/// </summary>
	public class UserToolItem
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
		/// Create a member-level clone of the caller's item and return it.
		/// </summary>
		/// <param name="tool">
		/// Reference to the user tool item to be cloned.
		/// </param>
		/// <returns>
		/// Reference to the newly created user tool item that has been duplicated
		/// at the member level.
		/// </returns>
		public static UserToolItem Clone(UserToolItem tool)
		{
			UserToolItem result = new UserToolItem();

			if(tool != null)
			{
				result.mToolName = tool.mToolName;
				result.mToolType = tool.mToolType;
				foreach(PropertyItem propertyItem in tool.mProperties)
				{
					result.mProperties.Add(new PropertyItem()
					{
						Name = propertyItem.Name,
						Value = propertyItem.Value
					});
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Properties																														*
		//*-----------------------------------------------------------------------*
		private PropertyCollection mProperties = new PropertyCollection();
		/// <summary>
		/// Get a reference to the collection of properties defining this tool.
		/// </summary>
		[JsonProperty(Order = 3)]
		public PropertyCollection Properties
		{
			get { return mProperties; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToolId																																*
		//*-----------------------------------------------------------------------*
		private string mToolId = Guid.NewGuid().ToString("D").ToLower();
		/// <summary>
		/// Get/Set the globally unique tool ID.
		/// </summary>
		[JsonProperty(Order = 0)]
		public string ToolId
		{
			get { return mToolId; }
			set { mToolId = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToolName																															*
		//*-----------------------------------------------------------------------*
		private string mToolName = "";
		/// <summary>
		/// Get/Set the user's name for the tool.
		/// </summary>
		[JsonProperty(Order = 1)]		
		public string ToolName
		{
			get { return mToolName; }
			set { mToolName = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToolType																															*
		//*-----------------------------------------------------------------------*
		private string mToolType = "";
		/// <summary>
		/// Get/Set the name of the base tool definition upon which this tool is
		/// based.
		/// </summary>
		[JsonProperty(Order = 2)]
		public string ToolType
		{
			get { return mToolType; }
			set { mToolType = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of this item.
		/// </summary>
		/// <returns>
		/// The string representation of this item.
		/// </returns>
		public override string ToString()
		{
			return (mToolName?.Length > 0 ? mToolName : "Unnamed Tool");
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
