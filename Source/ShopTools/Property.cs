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
	//*	PropertyCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of PropertyItem Items.
	/// </summary>
	public class PropertyCollection : List<PropertyItem>
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
		//*	_Indexer																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get a property from the collection by name.
		/// </summary>
		public PropertyItem this[string name]
		{
			get
			{
				PropertyItem result = null;

				if(name?.Length > 0)
				{
					result = this.FirstOrDefault(x => x.Name == name);
				}
				if(result == null && mAutoCreate)
				{
					result = new PropertyItem()
					{
						Name = name
					};
					this.Add(result);
				}
				return result;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AutoCreate																														*
		//*-----------------------------------------------------------------------*
		private bool mAutoCreate = true;
		/// <summary>
		/// Get/Set a value indicating whether a property will automatically be
		/// created and added to the list if not found by name.
		/// </summary>
		[JsonIgnore]
		public bool AutoCreate
		{
			get { return mAutoCreate; }
			set { mAutoCreate = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clone																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a memberwise clone of the caller's property collection.
		/// </summary>
		/// <param name="items">
		/// Reference to the collection of properties to clone.
		/// </param>
		/// <returns>
		/// A memberwise clone of the caller's original collection.
		/// </returns>
		public static PropertyCollection Clone(PropertyCollection items)
		{
			PropertyCollection result = new PropertyCollection();

			if(items?.Count > 0)
			{
				foreach(PropertyItem propertyItem in items)
				{
					result.Add(new PropertyItem()
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
		//* GetFirstValue																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the first property name found within the
		/// collection.
		/// </summary>
		/// <param name="propertyNames">
		/// Reference to the array of property names to allow. A series of
		/// individual string parameter names is supported.
		/// </param>
		/// <returns>
		/// The value of the first named property found, if located. Otherwise,
		/// an empty string.
		/// </returns>
		public string GetFirstValue(params string[] propertyNames)
		{
			PropertyItem property = null;
			string result = "";

			foreach(string propertyNameItem in propertyNames)
			{
				property = this.FirstOrDefault(x => x.Name == propertyNameItem);
				if(property?.Value != null)
				{
					result = property.Value;
					break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	PropertyItem																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual name and value property combination.
	/// </summary>
	public class PropertyItem
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
		//*	Name																																	*
		//*-----------------------------------------------------------------------*
		private string mName = "";
		/// <summary>
		/// Get/Set the name of this item.
		/// </summary>
		[JsonProperty(Order = 0)]
		public string Name
		{
			get { return mName; }
			set { mName = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Value																																	*
		//*-----------------------------------------------------------------------*
		private string mValue = "";
		/// <summary>
		/// Get/Set the value of this item.
		/// </summary>
		[JsonProperty(Order = 1)]
		public string Value
		{
			get { return mValue; }
			set { mValue = value; }
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
