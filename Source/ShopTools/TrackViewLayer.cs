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
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	TrackViewLayerCollection																								*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of TrackViewLayerItem Items.
	/// </summary>
	public class TrackViewLayerCollection : List<TrackViewLayerItem>
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		/// <summary>
		/// The exaggeration factor to apply to the Z axis.
		/// </summary>
		private float mZMagnification = 1f;

		//*-----------------------------------------------------------------------*
		//* PlotToPositionXYAbs																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Connect the XY path of the segment travel to the current depth of the
		/// provided location, returning the 3D end point of the plotted travel.
		/// </summary>
		/// <param name="layer">
		/// Reference to the layer to which view segments are being posted.
		/// </param>
		/// <param name="sourceSegment">
		/// The source segment containing the beginning and end locations.
		/// </param>
		/// <param name="location">
		/// The current starting location.
		/// </param>
		/// <returns>
		/// Reference to the absolute end coordinate of the current path.
		/// </returns>
		private FVector3 PlotToPositionXYAbs(TrackViewLayerItem layer,
			TrackSegmentItem sourceSegment, FVector3 location)
		{
			FVector3 localLocation = FVector3.Clone(location);
			TrackViewSegmentItem segment = null;
			float vertical = SessionWorkpieceInfo.Thickness * mZMagnification;

			if(layer != null && sourceSegment != null && location != null)
			{
				segment = new TrackViewSegmentItem()
				{
					Depth = vertical - sourceSegment.Depth,
					Operation = sourceSegment.Operation,
					ParentLayer = layer,
					SegmentType = TrackSegmentType.Plot,
					TargetDepth = sourceSegment.TargetDepth
				};
				segment.StartOffset.X = sourceSegment.StartOffset.X;
				segment.StartOffset.Y = sourceSegment.StartOffset.Y;
				segment.StartOffset.Z =
					vertical - (sourceSegment.Depth * mZMagnification);
				segment.EndOffset.X = sourceSegment.EndOffset.X;
				segment.EndOffset.Y = sourceSegment.EndOffset.Y;
				segment.EndOffset.Z = segment.StartOffset.Z;
				FVector3.TransferValues(segment.StartOffset, segment.Line.PointA);
				FVector3.TransferValues(segment.EndOffset, segment.Line.PointB);
				localLocation = FVector3.Clone(segment.EndOffset);
				layer.Segments.Add(segment);
			}
			if(localLocation == null)
			{
				localLocation = new FVector3();
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PlungeZAbs																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Plunge the Z-axis into the material.
		/// </summary>
		/// <param name="layer">
		/// Reference to the layer upon which the plunge is being created.
		/// </param>
		/// <param name="location">
		/// Reference to the location from which the plunge is starting.
		/// </param>
		/// <param name="sourceSegment">
		/// The source segment containing the depth position to plunge to from the
		/// registered height.
		/// </param>
		/// <returns>
		/// Reference to a string containing the command for the Z-axis plunge.
		/// </returns>
		/// <remarks>
		/// <para>
		/// The top of the material represents plunge depth 0.
		/// </para>
		/// <para>
		/// The wasteboard of the canvas represents the fully extended position
		/// of the Z-axis.
		/// </para>
		/// </remarks>
		private FVector3 PlungeZAbs(TrackViewLayerItem layer,
			FVector3 location, TrackSegmentItem sourceSegment)
		{
			FVector3 localLocation = FVector3.Clone(location);
			TrackViewSegmentItem segment = null;

			if(layer != null && location != null)
			{
				segment = new TrackViewSegmentItem();
				FVector3.TransferValues(localLocation, segment.StartOffset);
				FVector3.TransferValues(localLocation, segment.Line.PointA);
				localLocation.Z = (SessionWorkpieceInfo.Thickness * mZMagnification) -
					(sourceSegment.Depth * mZMagnification);
				FVector3.TransferValues(localLocation, segment.EndOffset);
				FVector3.TransferValues(localLocation, segment.Line.PointB);
				segment.Depth = localLocation.Z;
				segment.SegmentType = TrackSegmentType.Plunge;
				segment.ParentLayer = layer;
				layer.Segments.Add(segment);
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RetractTool																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Retract the tool to the maximum Z distance.
		/// </summary>
		/// <param name="layer">
		/// Reference to the layer for which this line is being created.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location from which to retract.
		/// </param>
		/// <returns>
		/// Reference to the updated tool position.
		/// </returns>
		private FVector3 RetractTool(TrackViewLayerItem layer,
			FVector3 location)
		{
			FVector3 localLocation = FVector3.Clone(location);
			TrackViewSegmentItem segment = null;
			float zDimension = GetMillimeters(ConfigProfile.Depth) * mZMagnification;

			if(layer != null && location != null)
			{
				segment = new TrackViewSegmentItem();
				FVector3.TransferValues(localLocation, segment.StartOffset);
				FVector3.TransferValues(localLocation,  segment.Line.PointA);
				localLocation.Z = zDimension;
				FVector3.TransferValues(localLocation, segment.EndOffset);
				FVector3.TransferValues(localLocation, segment.Line.PointB);
				segment.Depth = localLocation.Z;
				segment.SegmentType = TrackSegmentType.Transit;
				segment.ParentLayer = layer;
				layer.Segments.Add(segment);
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransitToPositionXYAbs																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Transit to the specified XY location.
		/// </summary>
		/// <param name="layer">
		/// Reference the layer being constructed.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location of the transit.
		/// </param>
		/// <param name="xyLocation">
		/// Reference to the target transit destination.
		/// </param>
		/// <returns>
		/// Reference to the new 3D location, in world coordinates at the end
		/// of the transit.
		/// </returns>
		private FVector3 TransitToPositionXYAbs(TrackViewLayerItem layer,
			FVector3 location, FVector2 xyLocation)
		{
			FVector3 localLocation = FVector3.Clone(location);
			TrackViewSegmentItem segment = null;

			if(layer != null && location != null && xyLocation != null)
			{
				segment = new TrackViewSegmentItem();
				FVector3.TransferValues(localLocation, segment.StartOffset);
				FVector3.TransferValues(localLocation, segment.Line.PointA);
				localLocation.X = xyLocation.X;
				localLocation.Y = xyLocation.Y;
				FVector3.TransferValues(localLocation, segment.EndOffset);
				FVector3.TransferValues(localLocation, segment.Line.PointB);
				segment.Depth = localLocation.Z;
				segment.SegmentType = TrackSegmentType.Transit;
				segment.ParentLayer = layer;
				layer.Segments.Add(segment);
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransitToPositionZAbs																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Transit to an absolute Z position.
		/// </summary>
		/// <param name="layer">
		/// Reference the layer being constructed.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location of the transit.
		/// </param>
		/// <param name="transitZEnum">
		/// The vertical location to which the tool will be transited.
		/// </param>
		/// <returns>
		/// Reference to the new absolute world coordinate of the tool after the
		/// transit.
		/// </returns>
		private FVector3 TransitToPositionZAbs(TrackViewLayerItem layer,
			FVector3 location, TransitZEnum transitZEnum)
		{
			FVector3 localLocation = FVector3.Clone(location);
			TrackViewSegmentItem segment = null;
			float zDimension = GetMillimeters(ConfigProfile.Depth) * mZMagnification;

			if(layer != null && location != null)
			{
				segment = new TrackViewSegmentItem();
				FVector3.TransferValues(localLocation, segment.StartOffset);
				FVector3.TransferValues(localLocation, segment.Line.PointA);
				switch(transitZEnum)
				{
					case TransitZEnum.FullyExtended:
						localLocation.Z = 0f;
						break;
					case TransitZEnum.TopOfMaterial:
						localLocation.Z =
							SessionWorkpieceInfo.Thickness * mZMagnification;
						break;
					case TransitZEnum.FullyRetracted:
					default:
						localLocation.Z = zDimension;
						break;
				}
				FVector3.TransferValues(localLocation, segment.EndOffset);
				FVector3.TransferValues(localLocation, segment.Line.PointB);
				segment.Depth = localLocation.Z;
				segment.SegmentType = TrackSegmentType.Transit;
				segment.ParentLayer = layer;
				layer.Segments.Add(segment);
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* Convert																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a new track view layer collection from the caller's basic track
		/// layer collection.
		/// </summary>
		/// <param name="tracks">
		/// Reference to a basic track layer collection more suitable for plotting.
		/// </param>
		/// <param name="zMagnification">
		/// The Z magnification at which to display the tracks from the surface.
		/// </param>
		/// <returns>
		/// Reference to a track view layer collection more suitable for 3D
		/// display. This type of object contains all of the point-to-point
		/// movements as individual lines of a certain type.
		/// </returns>
		public static TrackViewLayerCollection Convert(TrackLayerCollection tracks,
			float zMagnification)
		{
			bool bRetracted = true;
			TrackSegmentItem lastSegment = null;
			int layerIndex = 0;
			FVector3 location = new FVector3();
			//TrackSegmentItem plotSegment = null;
			TrackViewLayerCollection result = new TrackViewLayerCollection();
			TrackViewLayerItem viewLayer = null;
			string toolName = "";
			string toolNameLast = "";
			float zDimension = GetMillimeters(ConfigProfile.Depth) * zMagnification;

			if(tracks?.Count > 0)
			{
				result.mZMagnification = zMagnification;
				location = new FVector3(0f, 0f, zDimension);
				foreach(TrackLayerItem layerItem in tracks)
				{
					//	Each layer.
					viewLayer = new TrackViewLayerItem()
					{
						CurrentDepth = layerItem.CurrentDepth,
						TargetDepth = layerItem.TargetDepth
					};
					toolName =
						(layerItem.Tool?.ToolName.Length > 0 ?
						layerItem.Tool.ToolName : "");
					viewLayer.ToolName = toolName;
					if(toolName != toolNameLast)
					{
						if(!bRetracted)
						{
							location = result.RetractTool(viewLayer, location);
							bRetracted = true;
						}
					}
					//if(layerIndex > 0 && layerItem.Segments.Count > 0)
					//{
					//	//	On subsequent layers, plunge directly to the current relative
					//	//	depth.
					//	plotSegment = layerItem.Segments[0];
					//	if(plotSegment.SegmentType == TrackSegmentType.Plot)
					//	{
					//		location = result.PlungeZAbs(viewLayer, location, plotSegment);
					//	}
					//}
					foreach(TrackSegmentItem segmentItem in layerItem.Segments)
					{
						//	Each segment on plane.
						switch(segmentItem.SegmentType)
						{
							case TrackSegmentType.Plot:
								if(bRetracted)
								{
									//	Lower the tool to the material.
									location = result.TransitToPositionZAbs(viewLayer, location,
										TransitZEnum.TopOfMaterial);
								}
								if(bRetracted ||
									(lastSegment != null &&
										lastSegment.Depth != segmentItem.Depth))
								{
									//	Dig in.
									location =
										result.PlungeZAbs(viewLayer, location, segmentItem);
								}
								location =
									result.PlotToPositionXYAbs(viewLayer, segmentItem, location);
								bRetracted = false;
								break;
							case TrackSegmentType.Plunge:
								if(!FVector2.Equals(segmentItem.StartOffset, location))
								{
									location = result.TransitToPositionXYAbs(viewLayer, location,
										segmentItem.StartOffset);
								}
								location = result.TransitToPositionZAbs(viewLayer, location,
									TransitZEnum.TopOfMaterial);
								location =
									result.PlungeZAbs(viewLayer, location, segmentItem);
								location =
									result.TransitToPositionZAbs(viewLayer, location,
									TransitZEnum.FullyRetracted);
								bRetracted = true;
								break;
							case TrackSegmentType.Transit:
								if(!bRetracted)
								{
									location = result.TransitToPositionZAbs(viewLayer, location,
										TransitZEnum.FullyRetracted);
									bRetracted = true;
								}
								location = result.TransitToPositionXYAbs(viewLayer, location,
									segmentItem.EndOffset);
								break;
						}
						lastSegment = segmentItem;
					}
					result.Add(viewLayer);
					toolNameLast = toolName;
					layerIndex++;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetMultipleToolsDeclared																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether multiple tools have been declared in
		/// this layer collection.
		/// </summary>
		/// <returns>
		/// Value indicating whether multiple tools are declared in this
		/// collection.
		/// </returns>
		public bool GetMultipleToolsDeclared()
		{
			bool result = false;
			string toolName = "";

			foreach(TrackViewLayerItem layerItem in this)
			{
				if(layerItem.ToolName?.Length > 0)
				{
					if(toolName.Length == 0)
					{
						toolName = layerItem.ToolName;
					}
					else if(layerItem.ToolName != toolName)
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetSegment																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a segment from the collection by its ordinal index.
		/// </summary>
		/// <param name="index">
		/// Absolute ordinal index of the segment within the layers.
		/// </param>
		/// <returns>
		/// Reference to the specified segment, if found. Otherwise, null.
		/// </returns>
		public TrackViewSegmentItem GetSegment(int index)
		{
			int maxOffset = 0;
			int minOffset = 0;
			TrackViewSegmentItem result = null;

			if(index > 0)
			{
				foreach(TrackViewLayerItem layerItem in this)
				{
					maxOffset = minOffset + layerItem.Segments.Count - 1;
					if(index >= minOffset && index <= maxOffset)
					{
						result = layerItem.Segments[index - minOffset];
					}
					minOffset += layerItem.Segments.Count;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* HideAllSegments																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Hide all segments in the collection.
		/// </summary>
		public void HideAllSegments()
		{
			foreach(TrackViewLayerItem layerItem in this)
			{
				foreach(TrackViewSegmentItem segmentItem in layerItem.Segments)
				{
					segmentItem.Visible = false;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* HideSegmentsAfter																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Hide all the segments after the specified item.
		/// </summary>
		/// <param name="segment">
		/// Reference to the segment after which all others will be hidden.
		/// </param>
		public void HideSegmentsAfter(TrackViewSegmentItem segment)
		{
			bool bActive = false;
			foreach(TrackViewLayerItem layerItem in this)
			{
				foreach(TrackViewSegmentItem segmentItem in layerItem.Segments)
				{
					if(segmentItem == segment)
					{
						bActive = true;
					}
					else if(bActive)
					{
						segmentItem.Visible = false;
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShowAllSegments																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Show all segments in the collection.
		/// </summary>
		public void ShowAllSegments()
		{
			foreach(TrackViewLayerItem layerItem in this)
			{
				foreach(TrackViewSegmentItem segmentItem in layerItem.Segments)
				{
					segmentItem.Visible = true;
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShowSegmentsBefore																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Show all the segments before the specified item.
		/// </summary>
		/// <param name="segment">
		/// Reference to the segment before which all others will be shown.
		/// </param>
		public void ShowSegmentsBefore(TrackViewSegmentItem segment)
		{
			foreach(TrackViewLayerItem layerItem in this)
			{
				foreach(TrackViewSegmentItem segmentItem in layerItem.Segments)
				{
					if(segmentItem == segment)
					{
						break;
					}
					segmentItem.Visible = true;
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	TrackViewLayerItem																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about an individual track view layer.
	/// </summary>
	public class TrackViewLayerItem
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
		//*	CurrentDepth																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="CurrentDepth">CurrentDepth</see>.
		/// </summary>
		private float mCurrentDepth = 0f;
		/// <summary>
		/// Get/Set the current depth assigned to this segment.
		/// </summary>
		public float CurrentDepth
		{
			get { return mCurrentDepth; }
			set { mCurrentDepth = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Segments																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Segments">Segments</see>.
		/// </summary>
		private TrackViewSegmentCollection mSegments =
			new TrackViewSegmentCollection();
		/// <summary>
		/// Get a reference to the collection of segments in this layer.
		/// </summary>
		public TrackViewSegmentCollection Segments
		{
			get { return mSegments; }
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
		/// Get/Set the target depth for this cut.
		/// </summary>
		public float TargetDepth
		{
			get { return mTargetDepth; }
			set { mTargetDepth = value; }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	Tool																																	*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for <see cref="Tool">Tool</see>.
		///// </summary>
		//private TrackToolItem mTool = null;
		///// <summary>
		///// Get/Set a reference to the tool selected for this layer.
		///// </summary>
		//public TrackToolItem Tool
		//{
		//	get { return mTool; }
		//	set { mTool = value; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToolName																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ToolName">ToolName</see>.
		/// </summary>
		private string mToolName = "";
		/// <summary>
		/// Get/Set the name of the selected tool for this layer.
		/// </summary>
		public string ToolName
		{
			get { return mToolName; }
			set { mToolName = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
