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

using Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	TrackViewSegmentCollection																							*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of TrackViewSegmentItem Items.
	/// </summary>
	public class TrackViewSegmentCollection : List<TrackViewSegmentItem>
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
	//*	TrackViewSegmentItem																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual viewable segment within a track view layer.
	/// </summary>
	public class TrackViewSegmentItem
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
		private FVector3 mEndOffset = new FVector3();
		/// <summary>
		/// Get/Set a reference to the ending offset.
		/// </summary>
		/// <seealso href="https://danielanywhere.github.io/Geometry/html/T_Geometry_FVector3.htm">
		/// FVector3 Documentation</seealso>
		public FVector3 EndOffset
		{
			get { return mEndOffset; }
			set { mEndOffset = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Line																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Line">Line</see>.
		/// </summary>
		private FLine3 mLine = new FLine3();
		/// <summary>
		/// Get/Set a reference to the line representing this segment, in world
		/// coordinates.
		/// </summary>
		public FLine3 Line
		{
			get { return mLine; }
			set { mLine = value; }
		}
		//*-----------------------------------------------------------------------*

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
		//*	ParentLayer																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ParentLayer">ParentLayer</see>.
		/// </summary>
		private TrackViewLayerItem mParentLayer = null;
		/// <summary>
		/// Get/Set a reference to the layer of which this item is a member.
		/// </summary>
		public TrackViewLayerItem ParentLayer
		{
			get { return mParentLayer; }
			set { mParentLayer = value; }
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
		private FVector3 mStartOffset = new FVector3();
		/// <summary>
		/// Get/Set a reference to the starting offset.
		/// </summary>
		/// <seealso href="https://danielanywhere.github.io/Geometry/html/T_Geometry_FVector3.htm">
		/// FVector3 Documentation</seealso>
		public FVector3 StartOffset
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

		//*-----------------------------------------------------------------------*
		//*	Visible																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Visible">Visible</see>.
		/// </summary>
		private bool mVisible = true;
		/// <summary>
		/// Get/Set the visibility state of this segment.
		/// </summary>
		public bool Visible
		{
			get { return mVisible; }
			set { mVisible = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
