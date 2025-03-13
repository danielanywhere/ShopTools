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
	//*	MaterialTypeCollection																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of MaterialTypeItem Items.
	/// </summary>
	public class MaterialTypeCollection : List<MaterialTypeItem>
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
		/// Return a deep clone of the provided collection of material type items.
		/// </summary>
		/// <param name="items">
		/// Reference to the collection of items to clone.
		/// </param>
		/// <returns>
		/// Reference to a newly cloned collection of material type items.
		/// </returns>
		public static MaterialTypeCollection Clone(MaterialTypeCollection items)
		{
			MaterialTypeCollection result = new MaterialTypeCollection();

			if(items?.Count > 0)
			{
				foreach(MaterialTypeItem typeItem in items)
				{
					result.Add(MaterialTypeItem.Clone(typeItem));
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	MaterialTypeItem																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about an individual material type.
	/// </summary>
	public class MaterialTypeItem
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
		/// Return a deep clone of the provided material type item.
		/// </summary>
		/// <param name="item">
		/// Reference to the item to clone.
		/// </param>
		/// <returns>
		/// Reference to a newly cloned material type item.
		/// </returns>
		public static MaterialTypeItem Clone(MaterialTypeItem item)
		{
			MaterialTypeItem result = null;

			if(item != null)
			{
				result = new MaterialTypeItem()
				{
					mFeedRate = item.mFeedRate,
					mMaterialTypeName = item.mMaterialTypeName,
					mUserFeedRate = item.mUserFeedRate
				};
			}
			if(result == null)
			{
				result = new MaterialTypeItem();
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	FeedRate																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="FeedRate">FeedRate</see>.
		/// </summary>
		private string mFeedRate = "";
		/// <summary>
		/// Get/Set the formal feed rate, in the currently selected display format.
		/// </summary>
		public string FeedRate
		{
			get { return mFeedRate; }
			set { mFeedRate = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MaterialTypeName																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="MaterialTypeName">MaterialTypeName</see>.
		/// </summary>
		private string mMaterialTypeName = "";
		/// <summary>
		/// Get/Set the name of the material type.
		/// </summary>
		public string MaterialTypeName
		{
			get { return mMaterialTypeName; }
			set { mMaterialTypeName = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserFeedRate																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="UserFeedRate">UserFeedRate</see>.
		/// </summary>
		private string mUserFeedRate = "";
		/// <summary>
		/// Get/Set the feed rate entered directly by the user.
		/// </summary>
		public string UserFeedRate
		{
			get { return mUserFeedRate; }
			set { mUserFeedRate = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
