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

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	TrackLayerCollection																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of TrackLayerItem Items.
	/// </summary>
	public class TrackLayerCollection : List<TrackLayerItem>
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
		//*	_Constructor																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a new instance of the TrackLayerCollection Item.
		/// </summary>
		public TrackLayerCollection()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the TrackLayerCollection Item.
		/// </summary>
		public TrackLayerCollection(CutProfileCollection cutList)
		{
			GenerateTracks(cutList);
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GenerateTracks																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Process all of the cuts in the caller's cut profile collection to
		/// generate the tracks for this collection.
		/// </summary>
		/// <param name="cutList">
		/// Reference to the collection of cuts to process.
		/// </param>
		public void GenerateTracks(CutProfileCollection cutList)
		{
			int count = 0;
			float cutDepth = 0f;
			float depth = 0f;
			FPoint endOffset = null;
			int index = 0;
			TrackLayerItem lastLayer = null;
			FPoint lastLocation = null;
			TrackLayerItem layer = null;
			FPoint location = null;
			float maxDepth = float.MinValue;
			float maxDepthPerPass = 0f;
			float minDepth = float.MaxValue;
			List<PatternOperationItem> operations = null;
			TrackSegmentItem prevSegment = null;
			TrackSegmentItem segment = null;
			TrackSegmentCollection segments = null;
			FPoint startOffset = null;
			float targetDepth = 0f;
			string text = "";
			UserToolItem tool = null;
			WorkpieceInfoItem workpiece = SessionWorkpieceInfo;

			if(cutList?.Count > 0 && workpiece != null)
			{
				operations = new List<PatternOperationItem>();
				//	Initialize the maximum depth per pass.
				tool = ConfigProfile.UserTools.FirstOrDefault(x =>
					x.ToolName == ConfigProfile.GeneralCuttingTool);
				if(tool != null)
				{
					text = tool.Properties["Diameter"].Value;
					if(text.Length > 0)
					{
						//	In general, the maximum depth per pass is 50% of the
						//	tool diameter, unless the implement is a facing tool, in
						//	which case, we need to adjust the feed rate.
						maxDepthPerPass = GetMillimeters(text) * 0.5f;
					}
				}
				if(maxDepthPerPass == 0f)
				{
					maxDepthPerPass = 1.5875f;
				}
				//	Get the minimum and maximum desired depths.
				foreach(CutProfileItem cutItem in cutList)
				{
					foreach(PatternOperationItem operationItem in cutItem.Operations)
					{
						if(operationItem.Action != OperationActionEnum.PointXY)
						{
							depth = ResolveDepth(operationItem);
							minDepth = Math.Min(minDepth, depth);
							maxDepth = Math.Max(maxDepth, depth);
						}
					}
				}
				if(minDepth == float.MaxValue)
				{
					minDepth = 0f;
				}
				if(maxDepth == float.MinValue)
				{
					maxDepth = 0f;
				}
				if(maxDepth > 0f)
				{
					//	A depth is known.
					depth = Math.Min(maxDepth, maxDepthPerPass);
					//	Create a default layer.
					layer = new TrackLayerItem();
					layer.CurrentDepth = maxDepth;
					layer.TargetDepth = maxDepth;
					segments = layer.Segments;
					//	Get all plunges first.
					//	All of the offsets need to be visited to assure correct
					//	positioning.
					location = lastLocation =
						TransformFromAbsolute(workpiece.RouterLocation);
					operations.Clear();
					foreach(CutProfileItem cutItem in cutList)
					{
						foreach(PatternOperationItem operationItem in cutItem.Operations)
						{
							startOffset = GetOperationStartLocation(operationItem,
								workpiece, location);
							endOffset = GetOperationEndLocation(operationItem,
								workpiece, startOffset);
							location = endOffset;
							if(operationItem.Action == OperationActionEnum.PointXY)
							{
								//	Plunge point found.
								if(!location.Equals(lastLocation))
								{
									segment = new TrackSegmentItem()
									{
										SegmentType = TrackSegmentType.Transit,
										StartOffset = TransformToAbsolute(startOffset),
										EndOffset = TransformToAbsolute(endOffset)
									};
									segments.Add(segment);
									segment = new TrackSegmentItem()
									{
										SegmentType = TrackSegmentType.Plunge,
										Depth = ResolveDepth(operationItem)
									};
									segments.Add(segment);
								}
								FPoint.TransferValues(location, lastLocation);
							}
						}
					}
					if(segments.Count > 0)
					{
						//	Add the plunges as a separate layer.
						this.Add(layer);
						layer = new TrackLayerItem();
						layer.CurrentDepth = depth;
						layer.TargetDepth = maxDepth;
						segments = layer.Segments;
					}
					//	Get all direct plots, skipping plunges.
					location = lastLocation =
						TransformFromAbsolute(workpiece.RouterLocation);
					foreach(CutProfileItem cutItem in cutList)
					{
						foreach(PatternOperationItem operationItem in cutItem.Operations)
						{
							startOffset = GetOperationStartLocation(operationItem,
								workpiece, location);
							endOffset = GetOperationEndLocation(operationItem,
									workpiece, startOffset);
							location = endOffset;
							switch(operationItem.Action)
							{
								case OperationActionEnum.DrawCircleCenterDiameter:
								case OperationActionEnum.DrawCircleCenterRadius:
								case OperationActionEnum.DrawCircleDiameter:
								case OperationActionEnum.DrawCircleRadius:
								case OperationActionEnum.DrawEllipseCenterDiameterXY:
								case OperationActionEnum.DrawEllipseCenterRadiusXY:
								case OperationActionEnum.DrawEllipseDiameterXY:
								case OperationActionEnum.DrawEllipseLengthWidth:
								case OperationActionEnum.DrawEllipseRadiusXY:
								case OperationActionEnum.DrawEllipseXY:
									break;
								case OperationActionEnum.DrawLineAngleLength:
								case OperationActionEnum.DrawLineLengthWidth:
								case OperationActionEnum.DrawLineXY:
									targetDepth = ResolveDepth(operationItem);
									cutDepth = Math.Min(depth, targetDepth);
									if(cutDepth > 0f)
									{
										if(!startOffset.Equals(lastLocation))
										{
											segment = new TrackSegmentItem()
											{
												StartOffset = TransformToAbsolute(lastLocation),
												EndOffset = TransformToAbsolute(startOffset),
												SegmentType = TrackSegmentType.Transit
											};
											segments.Add(segment);
										}
										segment = new TrackSegmentItem()
										{
											StartOffset = TransformToAbsolute(startOffset),
											EndOffset = TransformToAbsolute(endOffset),
											SegmentType = TrackSegmentType.Plot,
											Depth = cutDepth,
											TargetDepth = targetDepth
										};
										segments.Add(segment);
									}
									break;
								case OperationActionEnum.DrawPath:
								case OperationActionEnum.DrawRectangleLengthWidth:
								case OperationActionEnum.DrawRectangleXY:
								case OperationActionEnum.FillCircleCenterDiameter:
								case OperationActionEnum.FillCircleCenterRadius:
								case OperationActionEnum.FillCircleDiameter:
								case OperationActionEnum.FillCircleRadius:
								case OperationActionEnum.FillEllipseCenterDiameterXY:
								case OperationActionEnum.FillEllipseCenterRadiusXY:
								case OperationActionEnum.FillEllipseDiameterXY:
								case OperationActionEnum.FillEllipseLengthWidth:
								case OperationActionEnum.FillEllipseRadiusXY:
								case OperationActionEnum.FillEllipseXY:
								case OperationActionEnum.FillPath:
								case OperationActionEnum.FillRectangleLengthWidth:
								case OperationActionEnum.FillRectangleXY:
									break;
								case OperationActionEnum.MoveAngleLength:
								case OperationActionEnum.MoveXY:
									//	Transits are implicit when building a track
									//	in this version.
									break;
								case OperationActionEnum.None:
									break;
								case OperationActionEnum.PointXY:
									//	Plunges have already been handled.
									break;
							}
							lastLocation = location;
						}
					}
					depth += maxDepthPerPass;
					if(segments.Count > 0)
					{
						this.Add(layer);
						lastLayer = layer;
						layer = new TrackLayerItem();
						layer.CurrentDepth = depth;
						layer.TargetDepth = maxDepth;
						segments = layer.Segments;
					}
					startOffset = new FPoint();
					endOffset = new FPoint();
					lastLocation = TransformToAbsolute(lastLocation);
					while(depth <= maxDepth)
					{
						//	Repeat the layers in opposite directions until the maximum
						//	required depth has been reached.
						count = lastLayer.Segments.Count;
						for(index = count - 1; index > -1; index --)
						{
							prevSegment = lastLayer.Segments[index];
							if(prevSegment.SegmentType == TrackSegmentType.Plot &&
								prevSegment.TargetDepth >= depth)
							{
								//	The previous ending point will be the current starting
								//	point.
								FPoint.TransferValues(prevSegment.StartOffset, endOffset);
								FPoint.TransferValues(prevSegment.EndOffset, startOffset);
								if(!lastLocation.Equals(startOffset))
								{
									//	Create a transit.
									segment = new TrackSegmentItem()
									{
										SegmentType = TrackSegmentType.Transit
									};
									FPoint.TransferValues(lastLocation, segment.StartOffset);
									FPoint.TransferValues(startOffset, segment.EndOffset);
									segments.Add(segment);
								}
								segment = new TrackSegmentItem()
								{
									Depth = depth,
									SegmentType = prevSegment.SegmentType,
									TargetDepth = prevSegment.TargetDepth
								};
								FPoint.TransferValues(startOffset, segment.StartOffset);
								FPoint.TransferValues(endOffset, segment.EndOffset);
								segments.Add(segment);
								FPoint.TransferValues(endOffset, lastLocation);
							}
						}
						depth += maxDepthPerPass;
						if(segments.Count > 0)
						{
							this.Add(layer);
							lastLayer = layer;
							layer = new TrackLayerItem();
							layer.CurrentDepth = depth;
							layer.TargetDepth = maxDepth;
							segments = layer.Segments;
						}
						else
						{
							break;
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*



	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	TrackLayerItem																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Individual layer of track segments.
	/// </summary>
	public class TrackLayerItem
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
		private TrackSegmentCollection mSegments = new TrackSegmentCollection();
		/// <summary>
		/// Get a reference to the collection of segments on this layer.
		/// </summary>
		public TrackSegmentCollection Segments
		{
			get { return mSegments; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TargetDepth																														*
		//*-----------------------------------------------------------------------*
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

	}
	//*-------------------------------------------------------------------------*

}
