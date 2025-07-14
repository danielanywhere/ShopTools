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

using Geometry;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	TrackSegmentCollection																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of TrackSegmentItem Items.
	/// </summary>
	public class TrackSegmentCollection : List<TrackSegmentItem>
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
	//*	TrackSegmentItem																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual segment within a tool track.
	/// </summary>
	public class TrackSegmentItem
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
		//*	Depth																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Depth">Depth</see>.
		/// </summary>
		private float mDepth = 0f;
		/// <summary>
		/// Get/Set the depth of this segment.
		/// </summary>
		public float Depth
		{
			get { return mDepth; }
			set { mDepth = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	EndOffset																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="EndOffset">EndOffset</see>.
		/// </summary>
		private FVector2 mEndOffset = new FVector2();
		/// <summary>
		/// Get/Set a reference to the ending offset.
		/// </summary>
		/// <seealso href="https://danielanywhere.github.io/Geometry/html/T_Geometry_FVector2.htm">
		/// FVector2 Documentation</seealso>
		public FVector2 EndOffset
		{
			get { return mEndOffset; }
			set { mEndOffset = value; }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	FeedRate																															*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for <see cref="FeedRate">FeedRate</see>.
		///// </summary>
		//private float mFeedRate = 500f;
		///// <summary>
		///// Get/Set the feed rate to be applied to the segment, if applicable.
		///// </summary>
		//public float FeedRate
		//{
		//	get { return mFeedRate; }
		//	set { mFeedRate = value; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Operation																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Operation">Operation</see>.
		/// </summary>
		private PatternOperationItem mOperation = null;
		/// <summary>
		/// Get/Set a reference to the pattern operation from which this segment
		/// was created.
		/// </summary>
		public PatternOperationItem Operation
		{
			get { return mOperation; }
			set { mOperation = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SegmentType																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="SegmentType">SegmentType</see>.
		/// </summary>
		private TrackSegmentType mSegmentType = TrackSegmentType.None;
		/// <summary>
		/// Get/Set the type of path assigned to this segment.
		/// </summary>
		public TrackSegmentType SegmentType
		{
			get { return mSegmentType; }
			set { mSegmentType = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	StartOffset																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="StartOffset">StartOffset</see>.
		/// </summary>
		private FVector2 mStartOffset = new FVector2();
		/// <summary>
		/// Get/Set a reference to the starting offset.
		/// </summary>
		/// <seealso href="https://danielanywhere.github.io/Geometry/html/T_Geometry_FVector2.htm">
		/// FVector2 Documentation</seealso>
		public FVector2 StartOffset
		{
			get { return mStartOffset; }
			set { mStartOffset = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TargetDepth																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="TargetDepth">TargetDepth</see>.
		/// </summary>
		private float mTargetDepth = 0f;
		/// <summary>
		/// Get/Set the target depth of this segment.
		/// </summary>
		public float TargetDepth
		{
			get { return mTargetDepth; }
			set { mTargetDepth = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
