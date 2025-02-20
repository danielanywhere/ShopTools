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
		//*-----------------------------------------------------------------------*
		//* IsPlot																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the specified action is of a plotting
		/// type, subject to kerf.
		/// </summary>
		/// <param name="action">
		/// The action in question.
		/// </param>
		/// <returns>
		/// True if the specified action plots a line and is subject to kerf
		/// offset. Otherwise, false.
		/// </returns>
		/// <remarks>
		/// Note that point operations are always center-oriented, while fill
		/// operations always distribute the kerf to the inside area. As a result,
		/// point and fill operations always return false from this method.
		/// </remarks>
		private static bool IsPlot(OperationActionEnum action)
		{
			bool result = false;

			switch(action)
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
				case OperationActionEnum.DrawLineAngleLength:
				case OperationActionEnum.DrawLineLengthWidth:
				case OperationActionEnum.DrawLineXY:
				case OperationActionEnum.DrawPath:
				case OperationActionEnum.DrawRectangleLengthWidth:
				case OperationActionEnum.DrawRectangleXY:
					result = true;
					break;
			}
			return result;
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
		/// Create a new instance of the TrackLayerCollection Item.
		/// </summary>
		public TrackLayerCollection()
		{
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Create a new instance of the TrackLayerCollection Item.
		/// </summary>
		/// <param name="cutList">
		/// Reference to to a collection of cuts from which to generate tracks.
		/// </param>
		public TrackLayerCollection(CutProfileCollection cutList)
		{
			GenerateTracks(cutList);
		}
		//*-----------------------------------------------------------------------*

		//	---
		//	TODO: Update GenerateTracks to support the new calculated patterns.
		private static FPoint GetOperationStartLocation(
			PatternOperationItem operationItem, WorkpieceInfoItem workpiece,
			FPoint location)
		{
			return location;
		}
		private static FPoint GetOperationEndLocation(
			PatternOperationItem operationItem, WorkpieceInfoItem workpiece,
			FPoint startOffset)
		{
			return startOffset;
		}
		//	---

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
			float kerfClearance = 0f;
			TrackSegmentCollection kerfSegments = null;
			TrackLayerItem lastLayer = null;
			FPoint lastLocation = null;
			TrackLayerItem layer = null;
			FLine line1 = new FLine();
			FLine line2 = new FLine();
			FPoint location = null;
			float maxDepth = float.MinValue;
			float maxDepthPerPass = 0f;
			float minDepth = float.MaxValue;
			TrackSegmentItem nextSegment = null;
			TrackSegmentItem newSegment = null;
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
						kerfClearance = maxDepthPerPass = GetMillimeters(text) * 0.5f;
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
					//	*** DRILLS ***
					//	Get all plunges first.
					//	All of the offsets need to be visited to assure correct
					//	positioning.
					location = lastLocation =
						TransformFromAbsolute(workpiece.RouterLocation);
					//	Plunges are always center kerf.
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
										Operation = operationItem,
										SegmentType = TrackSegmentType.Transit
									};
									FPoint.TransferValues(startOffset, segment.StartOffset);
									FPoint.TransferValues(endOffset, segment.EndOffset);
									segments.Add(segment);
									segment = new TrackSegmentItem()
									{
										Operation = operationItem,
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
					//	*** LINES AND SHAPES ***
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
									//	Drawing a single line.
									targetDepth = ResolveDepth(operationItem);
									cutDepth = Math.Min(depth, targetDepth);
									if(cutDepth > 0f)
									{
										if(!startOffset.Equals(lastLocation))
										{
											segment = new TrackSegmentItem()
											{
												Operation = operationItem,
												SegmentType = TrackSegmentType.Transit
											};
											FPoint.TransferValues(lastLocation, segment.StartOffset);
											FPoint.TransferValues(startOffset, segment.EndOffset);
											segments.Add(segment);
										}
										segment = new TrackSegmentItem()
										{
											Operation = operationItem,
											SegmentType = TrackSegmentType.Plot,
											Depth = cutDepth,
											TargetDepth = targetDepth
										};
										FPoint.TransferValues(startOffset, segment.StartOffset);
										FPoint.TransferValues(endOffset, segment.EndOffset);
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
					//	*** KERF ***
					//	TODO: Debug kerf offsets.
					//	Adjust initial segment set for kerf.
					if(segments.Count > 0)
					{
						kerfSegments = new TrackSegmentCollection();
						prevSegment = null;
						count = segments.Count;
						for(index = 0; index < count; index++)
						{
							segment = segments[index];
							newSegment = null;
							if(index + 1 < count)
							{
								nextSegment = segments[index + 1];
							}
							else
							{
								nextSegment = null;
							}
							FLine.TransferValues(line1,
								segment.StartOffset, segment.EndOffset);
							if(segment.SegmentType == TrackSegmentType.Plot)
							{
								//	Translate the current line for its assigned kerf.
								switch(segment.Operation.Kerf)
								{
									case DirectionLeftRightEnum.Left:
										FLine.TranslateVector(line1,
											kerfClearance, ArcDirectionEnum.Forward);
										break;
									case DirectionLeftRightEnum.Right:
										FLine.TranslateVector(line1,
											kerfClearance, ArcDirectionEnum.Reverse);
										break;
								}
							}
							//	Blend with previous.
							if(prevSegment != null &&
								prevSegment.SegmentType == TrackSegmentType.Plot &&
								prevSegment.EndOffset.Equals(segment.StartOffset))
							{
								//	The previous and current segments are joined.
								if((int)prevSegment.Operation?.Kerf > 1 ||
									(int)segment.Operation?.Kerf > 1)
								{
									//	Adjustment is only needed on this starting offset if
									//	the previous or current segments have kerf.
									FLine.TransferValues(line2,
										prevSegment.StartOffset, prevSegment.EndOffset);
									switch(prevSegment.Operation.Kerf)
									{
										case DirectionLeftRightEnum.Left:
											FLine.TranslateVector(line2,
												kerfClearance, ArcDirectionEnum.Forward);
											break;
										case DirectionLeftRightEnum.Right:
											FLine.TranslateVector(line2,
												kerfClearance, ArcDirectionEnum.Reverse);
											break;
									}
									location = FLine.Intersect(line1, line2, true);
									//	Update the starting location on the current segment.
									FPoint.TransferValues(location, line1.PointA);
								}
							}
							//	Blend with next.
							if(nextSegment != null &&
								nextSegment.SegmentType == TrackSegmentType.Plot &&
								nextSegment.StartOffset.Equals(segment.EndOffset))
							{
								//	The current and next segments are joined.
								if((int)nextSegment.Operation?.Kerf > 1 ||
									(int)segment.Operation?.Kerf > 1)
								{
									//	Adjustment is needed on this ending offset if
									//	the current or next segments have kerf.
									FLine.TransferValues(line2,
										nextSegment.StartOffset, nextSegment.EndOffset);
									switch(nextSegment.Operation.Kerf)
									{
										case DirectionLeftRightEnum.Left:
											FLine.TranslateVector(line2,
												kerfClearance, ArcDirectionEnum.Forward);
											break;
										case DirectionLeftRightEnum.Right:
											FLine.TranslateVector(line2,
												kerfClearance, ArcDirectionEnum.Reverse);
											break;
									}
									location = FLine.Intersect(line1, line2, true);
									//	Update the ending location on the current segment.
									FPoint.TransferValues(location, line1.PointB);
								}
							}
							newSegment = DeepClone(segment);
							FPoint.TransferValues(line1.PointA, newSegment.StartOffset);
							FPoint.TransferValues(line1.PointB, newSegment.EndOffset);
							kerfSegments.Add(newSegment);
							prevSegment = segment;
						}
						//	Heal the start/end locations on transits.
						lastLocation = new FPoint();
						prevSegment = null;
						for(index = 0; index < count; index ++)
						{
							segment = kerfSegments[index];
							if(index + 1 < count)
							{
								nextSegment = kerfSegments[index + 1];
							}
							if(segment.SegmentType == TrackSegmentType.Transit)
							{
								if(prevSegment != null)
								{
									//	Connect the current start to previous end.
									FPoint.TransferValues(
										prevSegment.EndOffset, segment.StartOffset);
								}
								if(nextSegment != null)
								{
									//	Connect the current end to the next start.
									FPoint.TransferValues(
										nextSegment.StartOffset, segment.EndOffset);
								}
							}
							else
							{
								FPoint.TransferValues(segment.EndOffset, lastLocation);
							}
							prevSegment = segment;
						}
						//	Transfer the adjusted segments to the general map.
						segments.Clear();
						segments.AddRange(kerfSegments);
					}
					//	*** RESOLVE TOOL PATH ***
					//	Reversing paths until all layers are completed.
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
					//	*** DRAWING SPACE TO PHYSICAL SPACE ***
					foreach(TrackLayerItem layerItem in this)
					{
						foreach(TrackSegmentItem segmentItem in layerItem.Segments)
						{
							segmentItem.EndOffset =
								TransformToAbsolute(segmentItem.EndOffset);
							segmentItem.StartOffset =
								TransformToAbsolute(segmentItem.StartOffset);
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

	}
	//*-------------------------------------------------------------------------*

}
