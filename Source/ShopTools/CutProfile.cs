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
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geometry;
using Newtonsoft.Json;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	CutProfileCollection																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of CutProfileItem Items.
	/// </summary>
	public class CutProfileCollection : ChangeObjectCollection<CutProfileItem>
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
		/// Return a deep clone of the provided cut profile collection.
		/// </summary>
		/// <param name="items">
		/// Reference to the collection of cut profile items to clone.
		/// </param>
		/// <returns>
		/// Reference to the newly cloned collection of cut profile items.
		/// </returns>
		public static CutProfileCollection Clone(CutProfileCollection items)
		{
			CutProfileCollection result = new CutProfileCollection();

			if (items?.Count > 0)
			{
				foreach (CutProfileItem profileItem in items)
				{
					result.Add(CutProfileItem.Clone(profileItem));
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* MoveDown																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Move the specified item down in the collection by one space, if
		/// possible.
		/// </summary>
		/// <param name="cutProfile">
		/// Reference to the item to be moved downward in the collection.
		/// </param>
		public void MoveDown(CutProfileItem cutProfile)
		{
			int index = 0;

			if(cutProfile != null && this.Contains(cutProfile))
			{
				index = this.IndexOf(cutProfile);
				if(index + 1 < this.Count)
				{
					this.Remove(cutProfile);
					this.Insert(index + 1, cutProfile);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* MoveUp																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Move the specified item up in the collection by one space, if
		/// possible.
		/// </summary>
		/// <param name="cutProfile">
		/// Reference to the item to be moved upward in the collection.
		/// </param>
		public void MoveUp(CutProfileItem cutProfile)
		{
			int index = 0;

			if(cutProfile != null && this.Contains(cutProfile))
			{
				index = this.IndexOf(cutProfile);
				if(index > 0)
				{
					this.Remove(cutProfile);
					this.Insert(index - 1, cutProfile);
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	CutProfileItem																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual cut profile entry.
	/// </summary>
	public class CutProfileItem : PatternTemplateItem
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* mEndLocation_CoordinateChanged																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A coordinate has changed on the End Location.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Floating point event arguments.
		/// </param>
		private void mEndLocation_CoordinateChanged(object sender,
			FloatPointEventArgs e)
		{
			OnPropertyChanged("EndLocation");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mStartLocation_CoordinateChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A coordinate has changed on the Start Location.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Floating point event arguments.
		/// </param>
		private void mStartLocation_CoordinateChanged(object sender,
			FloatPointEventArgs e)
		{
			OnPropertyChanged("StartLocation");
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the CutProfileItem Item.
		/// </summary>
		public CutProfileItem() : base()
		{
			mEndLocation = new FVector2();
			mEndLocation.CoordinateChanged += mEndLocation_CoordinateChanged;
			mStartLocation = new FVector2();
			mStartLocation.CoordinateChanged += mStartLocation_CoordinateChanged;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the CutProfileItem Item.
		/// </summary>
		/// <param name="pattern">
		/// Reference to a pattern template item from which this item is being
		/// created.
		/// </param>
		public CutProfileItem(PatternTemplateItem pattern) : this()
		{
			if(pattern != null)
			{
				//foreach(string entryItem in pattern.AvailableProperties)
				//{
				//	this.AvailableProperties.Add(entryItem);
				//}
				this.DisplayFormat = pattern.DisplayFormat;
				this.IconFilename = pattern.IconFilename;
				this.Operations.AddRange(
					PatternOperationCollection.Clone(pattern.Operations));
				//this.Orientation = pattern.Orientation;
				//this.PatternLength = pattern.PatternLength;
				this.PatternTemplateId = pattern.PatternTemplateId;
				//this.PatternWidth = pattern.PatternWidth;
				foreach(string entryItem in pattern.Remarks)
				{
					this.Remarks.Add(entryItem);
				}
				foreach(string entryItem in pattern.SharedVariables)
				{
					this.SharedVariables.Add(entryItem);
				}
				this.TemplateName = pattern.TemplateName;
				//this.ToolSequenceStrict = pattern.ToolSequenceStrict;
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clone																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a deep clone of the provided cut profile item.
		/// </summary>
		/// <param name="item">
		/// Reference to the cut profile item to clone.
		/// </param>
		/// <returns>
		/// Reference to the newly cloned cut profile item.
		/// </returns>
		public static CutProfileItem Clone(CutProfileItem item)
		{
			CutProfileItem result = null;

			if(item != null)
			{
				result = new CutProfileItem()
				{
					DisplayFormat = item.DisplayFormat,
					IconFilename = item.IconFilename,
					mEndLocation = FVector2.Clone(item.mEndLocation),
					mStartLocation = FVector2.Clone(item.mStartLocation),
					//Orientation = item.Orientation,
					//PatternLength = item.PatternLength,
					PatternTemplateId = item.PatternTemplateId,
					//PatternWidth = item.PatternWidth,
					TemplateName = item.TemplateName,
					//ToolSequenceStrict = item.ToolSequenceStrict
				};
				//foreach(string entryItem in item.AvailableProperties)
				//{
				//	result.AvailableProperties.Add(entryItem);
				//}
				foreach(PatternOperationItem operationItem in item.Operations)
				{
					result.Operations.Add(PatternOperationItem.Clone(operationItem));
				}
				foreach(string entryItem in item.Remarks)
				{
					result.Remarks.Add(entryItem);
				}
				foreach(string entryItem in item.SharedVariables)
				{
					result.SharedVariables.Add(entryItem);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	EndLocation																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Internal EndLocation member <see cref="EndLocation"/>
		/// </summary>
		private FVector2 mEndLocation = null;
		/// <summary>
		/// Get/Set a reference to the end location of the router for this cut.
		/// </summary>
		/// <seealso href="https://danielanywhere.github.io/Geometry/html/T_Geometry_FVector2.htm">
		/// FVector2 Documentation</seealso>
		[JsonIgnore]
		public FVector2 EndLocation
		{
			get { return mEndLocation; }
			set
			{
				bool bChanged = (mEndLocation != value);

				//	Register events.
				if(bChanged)
				{
					if(mEndLocation != null)
					{
						mEndLocation.CoordinateChanged -= mEndLocation_CoordinateChanged;
					}
					if(value != null)
					{
						value.CoordinateChanged += mEndLocation_CoordinateChanged;
					}
				}
				mEndLocation = value;
				if (bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StartLocation																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Internal StartLocation member <see cref="StartLocation"/>.
		/// </summary>
		private FVector2 mStartLocation = new FVector2();
		/// <summary>
		/// Get/Set a reference to the start location of the router for this cut.
		/// </summary>
		/// <seealso href="https://danielanywhere.github.io/Geometry/html/T_Geometry_FVector2.htm">
		/// FVector2 Documentation</seealso>
		[JsonIgnore]
		public FVector2 StartLocation
		{
			get { return mStartLocation; }
			set
			{
				bool bChanged = (mStartLocation != value);

				//	Register events.
				if(bChanged)
				{
					if(mStartLocation != null)
					{
						mStartLocation.CoordinateChanged -=
							mStartLocation_CoordinateChanged;
					}
					if(value != null)
					{
						value.CoordinateChanged += mStartLocation_CoordinateChanged;
					}
				}
				mStartLocation = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransferValues																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Transfer the member values of the source item to the members of the
		/// target.
		/// </summary>
		/// <param name="source">
		/// Reference to the source cut profile item containing the values to
		/// transfer.
		/// </param>
		/// <param name="target">
		/// Reference to the target cut profile item to receive the update.
		/// </param>
		public static void TransferValues(CutProfileItem source,
			CutProfileItem target)
		{
			if(source != null && target != null)
			{
				target.DisplayFormat = source.DisplayFormat;
				target.IconFilename = source.IconFilename;
				FVector2.TransferValues(source.mEndLocation, target.mEndLocation);
				FVector2.TransferValues(source.mStartLocation, target.mStartLocation);
				//target.Orientation = source.Orientation;
				//target.PatternLength = source.PatternLength;
				target.PatternTemplateId = source.PatternTemplateId;
				//target.PatternWidth = source.PatternWidth;
				target.TemplateName = source.TemplateName;
				//target.ToolSequenceStrict = source.ToolSequenceStrict;

				target.Operations.Clear();
				foreach(PatternOperationItem operationItem in source.Operations)
				{
					target.Operations.Add(PatternOperationItem.Clone(operationItem));
				}
				target.Remarks.Clear();
				foreach(string entryItem in source.Remarks)
				{
					target.Remarks.Add(entryItem);
				}
				target.SharedVariables.Clear();
				foreach(string entryItem in source.SharedVariables)
				{
					target.SharedVariables.Add(entryItem);
				}
			}
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
