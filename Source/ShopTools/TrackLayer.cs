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
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Geometry;
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
		/// <summary>
		/// The default maximum depth per pass, in system units, if no tool
		/// information could be found.
		/// </summary>
		private const float mDefaultMaxDepthPerPass = 1.5875f;

		//*-----------------------------------------------------------------------*
		//* AddPrecedingExplicitMoves																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add explicit moves that directly precede the current layout item.
		/// </summary>
		/// <param name="layouts">
		/// Reference to the collection of layouts being processed.
		/// </param>
		/// <param name="layoutIndex">
		/// Index of the current layout within the collection.
		/// </param>
		/// <param name="moveIndices">
		/// Collection of explicit move element ranges within the layout
		/// collection.
		/// </param>
		/// <param name="tool">
		/// Reference to the currently selected tool for the operation.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location.
		/// </param>
		/// <returns>
		/// Reference to the last-known current location.
		/// </returns>
		private FPoint AddPrecedingExplicitMoves(OperationLayoutCollection layouts,
			int layoutIndex, IntRangeCollection moveIndices, TrackToolItem tool,
			FPoint location)
		{
			int index = 0;
			OperationLayoutItem layout = null;
			FPoint localLocation = FPoint.Clone(location);
			IntRangeItem moveIndex = null;
			TrackLayerItem trackLayer = null;

			if(layouts?.Count > 0 && layoutIndex > -1 &&
				layoutIndex < layouts.Count && moveIndices?.Count > 0 &&
				tool != null)
			{
				//	Another item precedes this set.
				moveIndex = moveIndices.FirstOrDefault(x => x.End == layoutIndex - 1);
				if(moveIndex != null)
				{
					//	One or more moves directly preceded this set.
					trackLayer = new TrackLayerItem()
					{
						BaseLayer = true,
						Tool = tool
					};
					for(index = moveIndex.Start; index <= moveIndex.End; index++)
					{
						layout = layouts[index];
						localLocation = MoveExplicit(layout, trackLayer, localLocation);
						//	When the move action has been allocated, make sure not to
						//	reprocess it.
						layout.ActionType = LayoutActionType.None;
					}
					this.Add(trackLayer);
				}
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* AdjustKerf																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Adjust the tool position to place the kerf on the desired side of the
		/// line.
		/// </summary>
		/// <returns>
		/// Reference to the updated last-known location.
		/// </returns>
		private FPoint AdjustKerf()
		{
			int count = 0;
			FLine line1 = new FLine();
			FLine line2 = new FLine();
			int index = 0;
			float kerfClearance = 0f;
			FPoint lastLocation = null;
			FPoint location = null;
			TrackSegmentCollection kerfSegments = null;
			TrackSegmentItem newSegment = null;
			TrackSegmentItem nextSegment = null;
			TrackSegmentItem prevSegment = null;
			TrackSegmentItem segment = null;
			TrackSegmentCollection segments = null;

			foreach(TrackLayerItem trackLayerItem in this)
			{
				if(!trackLayerItem.FinalLayer)
				{
					//	If this layer has not been finalized, then kerf is adjustable.
					kerfClearance = trackLayerItem.Tool.Diameter / 2f;
					segments = trackLayerItem.Segments;
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
								prevSegment.EndOffset == segment.StartOffset)
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
								nextSegment.StartOffset == segment.EndOffset)
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
						for(index = 0; index < count; index++)
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
				}
			}
			return lastLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawArc																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw the arc described by the supplied layout item.
		/// </summary>
		/// <param name="layoutItem">
		/// Reference to the layout item to be drawn.
		/// </param>
		/// <param name="track">
		/// Reference to the track layer to which new segments will be stored.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location of the tool prior to drilling
		/// the first hole.
		/// </param>
		/// <returns>
		/// Reference to the updated last known location.
		/// </returns>
		private FPoint DrawArc(OperationLayoutItem layoutItem,
			TrackLayerItem track, FPoint location)
		{
			float angle = 0f;
			float angleIncrement = 0f;
			float cutDepth = 0f;
			FEllipse ellipse = null;
			float endAngle = 0f;
			FPoint endOffset = null;
			FPoint localLocation = new FPoint(location);
			float radiusX = 0f;
			float radiusY = 0f;
			TrackSegmentItem segment = null;
			FPoint startOffset = null;
			float toolOffset = 0f;
			float targetDepth = 0f;

			if(layoutItem != null && track != null)
			{
				toolOffset = track.Tool.Diameter;
				targetDepth = ResolveDepth(layoutItem.Operation);
				cutDepth = Math.Min(track.CurrentDepth, targetDepth);
				//	The display coordinates contain the entire virtual ellipse.
				startOffset = layoutItem.DisplayStartOffset;
				endOffset = layoutItem.DisplayEndOffset;
				radiusX = (endOffset.X - startOffset.X) / 2f;
				radiusY = (endOffset.Y - startOffset.Y) / 2f;
				ellipse = new FEllipse(
					startOffset.X + radiusX, startOffset.Y + radiusY,
					radiusX,
					radiusY);
				//	Tune the number of facets to be the smaller of 6mm travel and 6deg.
				angleIncrement = Trig.GetLineAngle(ellipse.Center.X, ellipse.Center.Y,
					ellipse.Center.X + 6f,
					ellipse.Center.Y - Math.Min(ellipse.RadiusX, ellipse.RadiusY));
				if(Trig.RadToDeg(angleIncrement) > 6f)
				{
					angleIncrement = Trig.DegToRad(6f);
				}
				//	Reassign Start/End offsets to per-facet usage.
				startOffset = layoutItem.ToolStartOffset;
				angle = Trig.GetLineAngle(ellipse.Center, startOffset);

				//	Transit to site, if applicable.
				if(startOffset != localLocation)
				{
					segment = new TrackSegmentItem()
					{
						Operation = layoutItem.Operation,
						SegmentType = TrackSegmentType.Transit
					};
					FPoint.TransferValues(localLocation, segment.StartOffset);
					FPoint.TransferValues(startOffset, segment.EndOffset);
					track.Segments.Add(segment);
					FPoint.TransferValues(segment.EndOffset, localLocation);
				}
				if(angleIncrement != 0f)
				{
					//	Run the loop.
					if(GetAngle(layoutItem.Operation.SweepAngle) >= 0f)
					{
						//	Clockwise sweep.
						endAngle = angle + GetAngle(layoutItem.Operation.SweepAngle);
						for(; angle <= endAngle; angle += angleIncrement)
						{
							endOffset = FEllipse.GetCoordinateAtAngle(ellipse, angle);
							segment = new TrackSegmentItem()
							{
								Operation = layoutItem.Operation,
								SegmentType = TrackSegmentType.Plot,
								StartOffset = new FPoint(startOffset),
								EndOffset = new FPoint(endOffset),
								Depth = cutDepth,
								TargetDepth = targetDepth
							};
							track.Segments.Add(segment);
							FPoint.TransferValues(segment.EndOffset, startOffset);
							FPoint.TransferValues(segment.EndOffset, localLocation);
						}
					}
					else
					{
						//	Counterclockwise sweep.
						endAngle = angle + GetAngle(layoutItem.Operation.SweepAngle);
						for(; angle >= endAngle; angle -= angleIncrement)
						{
							endOffset = FEllipse.GetCoordinateAtAngle(ellipse, angle);
							segment = new TrackSegmentItem()
							{
								Operation = layoutItem.Operation,
								SegmentType = TrackSegmentType.Plot,
								StartOffset = new FPoint(startOffset),
								EndOffset = new FPoint(endOffset),
								Depth = cutDepth,
								TargetDepth = targetDepth
							};
							track.Segments.Add(segment);
							FPoint.TransferValues(segment.EndOffset, startOffset);
							FPoint.TransferValues(segment.EndOffset, localLocation);
						}
					}
					//	Finalize the path.
					endOffset = layoutItem.ToolEndOffset;
					if(endOffset != localLocation)
					{
						segment = new TrackSegmentItem()
						{
							Operation = layoutItem.Operation,
							SegmentType = TrackSegmentType.Plot,
							StartOffset = new FPoint(startOffset),
							EndOffset = new FPoint(endOffset),
							Depth = cutDepth,
							TargetDepth = targetDepth
						};
						track.Segments.Add(segment);
						FPoint.TransferValues(segment.EndOffset, localLocation);
					}
				}
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawEllipse																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw the ellipse described by the supplied layout item.
		/// </summary>
		/// <param name="layoutItem">
		/// Reference to the layout item to be drawn.
		/// </param>
		/// <param name="track">
		/// Reference to the track layer to which new segments will be stored.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location of the tool prior to drilling
		/// the first hole.
		/// </param>
		/// <returns>
		/// Reference to the updated last known location.
		/// </returns>
		private FPoint DrawEllipse(OperationLayoutItem layoutItem,
			TrackLayerItem track, FPoint location)
		{
			float angle = 0f;
			float angleIncrement = 0f;
			float cutDepth = 0f;
			FEllipse ellipse = null;
			FPoint endOffset = null;
			float facetIndex = 0f;
			float facetCount = 50f;
			FPoint localLocation = new FPoint(location);
			float radiusX = 0f;
			float radiusY = 0f;
			TrackSegmentItem segment = null;
			FPoint startOffset = null;
			float toolOffset = 0f;
			float targetDepth = 0f;

			if(layoutItem != null && track != null)
			{
				toolOffset = track.Tool.Diameter;
				targetDepth = ResolveDepth(layoutItem.Operation);
				cutDepth = Math.Min(track.CurrentDepth, targetDepth);
				startOffset = layoutItem.DisplayStartOffset;
				endOffset = layoutItem.DisplayEndOffset;
				radiusX = (endOffset.X - startOffset.X) / 2f;
				radiusY = (endOffset.Y - startOffset.Y) / 2f;
				ellipse = new FEllipse(
					startOffset.X + radiusX, startOffset.Y + radiusY,
					radiusX,
					radiusY);
				//	Tune the number of facets to be the smaller of 6mm travel and 6deg.
				angleIncrement = Trig.GetLineAngle(ellipse.Center.X, ellipse.Center.Y,
					ellipse.Center.X + 6f,
					ellipse.Center.Y - Math.Min(ellipse.RadiusX, ellipse.RadiusY));
				if(Trig.RadToDeg(angleIncrement) > 6f)
				{
					angleIncrement = Trig.DegToRad(6f);
				}
				//	Reassign Start/End offsets to per-facet usage.
				startOffset = layoutItem.ToolStartOffset;
				endOffset = layoutItem.ToolEndOffset;
				angle = Trig.GetLineAngle(ellipse.Center, startOffset);
				//	Transit to site, if applicable.
				if(startOffset != localLocation)
				{
					segment = new TrackSegmentItem()
					{
						Operation = layoutItem.Operation,
						SegmentType = TrackSegmentType.Transit
					};
					FPoint.TransferValues(localLocation, segment.StartOffset);
					FPoint.TransferValues(startOffset, segment.EndOffset);
					track.Segments.Add(segment);
					FPoint.TransferValues(segment.EndOffset, localLocation);
				}
				if(angleIncrement != 0f)
				{
					//	Run the loop.
					facetCount = GeometryUtil.TwoPi / angleIncrement;
					for(facetIndex = 0f; facetIndex < facetCount; facetIndex++,
						angle -= angleIncrement)
					{
						endOffset = FEllipse.GetCoordinateAtAngle(ellipse, angle);
						segment = new TrackSegmentItem()
						{
							Operation = layoutItem.Operation,
							SegmentType = TrackSegmentType.Plot,
							StartOffset = new FPoint(startOffset),
							EndOffset = new FPoint(endOffset),
							Depth = cutDepth,
							TargetDepth = targetDepth
						};
						track.Segments.Add(segment);
						FPoint.TransferValues(segment.EndOffset, startOffset);
						FPoint.TransferValues(segment.EndOffset, localLocation);
					}
					//	Finalize the path.
					endOffset = layoutItem.ToolEndOffset;
					if(endOffset != localLocation)
					{
						segment = new TrackSegmentItem()
						{
							Operation = layoutItem.Operation,
							SegmentType = TrackSegmentType.Plot,
							StartOffset = new FPoint(startOffset),
							EndOffset = new FPoint(endOffset),
							Depth = cutDepth,
							TargetDepth = targetDepth
						};
						track.Segments.Add(segment);
						FPoint.TransferValues(segment.EndOffset, localLocation);
					}
				}
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawLine																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw the line indicated by the layout item.
		/// </summary>
		/// <param name="layoutItem">
		/// Reference to the layout item describing the line to plot.
		/// </param>
		/// <param name="track">
		/// Reference to the track where new segments will be added.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location of the tool prior to drilling
		/// the first hole.
		/// </param>
		/// <returns>
		/// Reference to the updated last known location.
		/// </returns>
		private FPoint DrawLine(OperationLayoutItem layoutItem,
			TrackLayerItem track, FPoint location)
		{
			float cutDepth = 0f;
			FPoint endOffset = null;
			FPoint localLocation = new FPoint(location);
			TrackSegmentItem segment = null;
			FPoint startOffset = null;
			float targetDepth = 0f;

			if(layoutItem != null && track != null)
			{
				targetDepth = ResolveDepth(layoutItem.Operation);
				cutDepth = Math.Min(track.CurrentDepth, targetDepth);
				startOffset = layoutItem.ToolStartOffset;
				endOffset = layoutItem.ToolEndOffset;
				if(startOffset != localLocation)
				{
					//	Transit to the line.
					segment = new TrackSegmentItem()
					{
						Operation = layoutItem.Operation,
						SegmentType = TrackSegmentType.Transit
					};
					FPoint.TransferValues(localLocation, segment.StartOffset);
					FPoint.TransferValues(startOffset, segment.EndOffset);
					track.Segments.Add(segment);
				}
				segment = new TrackSegmentItem()
				{
					Operation = layoutItem.Operation,
					SegmentType = TrackSegmentType.Plot,
					Depth = cutDepth,
					TargetDepth = targetDepth
				};
				FPoint.TransferValues(startOffset, segment.StartOffset);
				FPoint.TransferValues(endOffset, segment.EndOffset);
				track.Segments.Add(segment);
				FPoint.TransferValues(endOffset, localLocation);
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawRectangle																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw the rectangle described by the provided layout item.
		/// </summary>
		/// <param name="layoutItem">
		/// Reference to the layout item to draw.
		/// </param>
		/// <param name="track">
		/// Reference to the track layer to which new segments will be stored.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location of the tool prior to drilling
		/// the first hole.
		/// </param>
		/// <returns>
		/// Reference to the updated last known location.
		/// </returns>
		private FPoint DrawRectangle(OperationLayoutItem layoutItem,
			TrackLayerItem track, FPoint location)
		{
			FArea boundingBox = null;
			int count = 0;
			float cutDepth = 0f;
			FPoint endOffset = null;
			int index = 0;
			FLine line = null;
			int lineIndex = 0;
			List<FLine> lineList = new List<FLine>();
			FLine lineRemainder = null;
			List<FLine> lines = null;
			List<FLine> linesMatching = null;
			FPoint localLocation = new FPoint(location);
			TrackSegmentItem segment = null;
			FLine startLine = null;
			FPoint startOffset = null;
			float targetDepth = 0f;

			if(layoutItem != null && track != null)
			{
				targetDepth = ResolveDepth(layoutItem.Operation);
				cutDepth = Math.Min(track.CurrentDepth, targetDepth);
				startOffset = layoutItem.ToolStartOffset;
				endOffset = layoutItem.ToolEndOffset;
				//	In the case of a rectangle, the tool offset represents
				//	a point on one of the lines, which might or might not
				//	be located at a corner.
				boundingBox = new FArea(
					layoutItem.DisplayStartOffset,
					layoutItem.DisplayEndOffset);
				lines = FArea.GetLines(boundingBox);
				//	Transit to site, if applicable.
				if(startOffset != localLocation)
				{
					segment = new TrackSegmentItem()
					{
						Operation = layoutItem.Operation,
						SegmentType = TrackSegmentType.Transit
					};
					FPoint.TransferValues(localLocation, segment.StartOffset);
					FPoint.TransferValues(startOffset, segment.EndOffset);
					track.Segments.Add(segment);
				}
				if(FArea.IsPointAtCorner(boundingBox, layoutItem.ToolStartOffset))
				{
					//	If the tool is pre-positioned at a corner, then
					//	lay all lines beginning with the line whose start
					//	end matches that corner.
					lineList.Clear();
					linesMatching = FLine.GetIntersectingLines(lines,
						layoutItem.ToolStartOffset);
					startLine = linesMatching.FirstOrDefault(x =>
						x.PointA == layoutItem.ToolStartOffset);
					if(startLine != null)
					{
						//	This line's starting point is the correct starting
						//	location.
						//	Add the bounding box lines to the queue in the proper
						//	order.
						count = 4;
						for(index = 0, lineIndex = lines.IndexOf(startLine);
							index < count; index++, lineIndex++)
						{
							lineList.Add(lines[lineIndex % 4]);
						}
						//	Plot the lines.
						for(index = 0; index < count; index++)
						{
							line = lineList[index];
							segment = new TrackSegmentItem()
							{
								Operation = layoutItem.Operation,
								SegmentType = TrackSegmentType.Plot,
								Depth = cutDepth,
								TargetDepth = targetDepth
							};
							FPoint.TransferValues(line.PointA,
								segment.StartOffset);
							FPoint.TransferValues(line.PointB,
								segment.EndOffset);
							FPoint.TransferValues(segment.EndOffset, localLocation);
						}
					}
				}
				else
				{
					//	The point is not at the corner. Find the intersecting
					//	line and split it.
					startLine = FLine.GetIntersectingLine(lines,
						layoutItem.ToolStartOffset);
					if(startLine != null)
					{
						lineRemainder = null;
						count = 4;
						for(index = 0, lineIndex = lines.IndexOf(startLine);
							index < count; index++, lineIndex++)
						{
							line = lines[lineIndex % 4];
							if(index == 0)
							{
								//	The first line needs to be split at the
								//	intersection.
								lineRemainder = new FLine(line.PointA,
									layoutItem.ToolStartOffset);
								line.PointA = new FPoint(layoutItem.ToolStartOffset);
								lineList.Add(line);
							}
							else
							{
								lineList.Add(line);
							}
						}
						lineList.Add(lineRemainder);
						//	Plot the lines.
						count = 5;
						for(index = 0; index < count; index++)
						{
							line = lineList[index];
							segment = new TrackSegmentItem()
							{
								Operation = layoutItem.Operation,
								SegmentType = TrackSegmentType.Plot,
								Depth = cutDepth,
								TargetDepth = targetDepth
							};
							FPoint.TransferValues(line.PointA,
								segment.StartOffset);
							FPoint.TransferValues(line.PointB,
								segment.EndOffset);
							FPoint.TransferValues(segment.EndOffset, localLocation);
						}
					}
				}
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrillHoles																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Drill the holes found in the layout list, separating each set of holes
		/// by the tool used to make that hole.
		/// </summary>
		/// <param name="layouts">
		/// Reference to the collection of layout elements for which to drill the
		/// holes.
		/// </param>
		/// <param name="tools">
		/// Reference to the collection of tools available for the holes.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location of the tool prior to drilling
		/// the first hole.
		/// </param>
		/// <returns>
		/// Reference to the updated last known location.
		/// </returns>
		private FPoint DrillHoles(OperationLayoutCollection layouts,
			TrackToolCollection tools, FPoint location)
		{
			string defaultToolName = "";
			FPoint endOffset = null;
			int layoutIndex = 0;
			List<List<OperationLayoutItem>> layoutSetsMatching = null;
			//List<OperationLayoutItem> layoutsMatching = null;
			FPoint localLocation = new FPoint(location);
			MinMaxItem minMax = null;
			IntRangeCollection moveIndices = null;
			List<List<OperationLayoutItem>> moveSets = null;
			FPoint newLocation = null;
			FPoint startOffset = null;
			TrackToolItem tool = null;
			List<string> toolNames = null;
			TrackLayerItem trackLayer = null;
			TrackSegmentItem segment = null;
			TrackSegmentCollection segments = null;

			if(layouts?.Count > 0 && tools?.Count > 0)
			{
				tool = tools.FirstOrDefault(x => x.IsDefault);
				if(tool != null)
				{
					defaultToolName = tool.ToolName;
				}
				//	One separate layer per tool.
				toolNames = layouts.Select(x => x.Operation?.Tool).Distinct().ToList();
				if(toolNames.Contains(null))
				{
					//	Non-operational layout elements are not supported in this
					//	version.
					toolNames.Remove(null);
				}
				if(toolNames.Count > 0)
				{
					moveSets = layouts.FindAllContiguous(x =>
						x.ActionType == LayoutActionType.MoveExplicit);
					moveIndices = layouts.GetIndexRanges(moveSets);

					foreach(string toolNameItem in toolNames)
					{
						tool = tools.SelectTool(toolNameItem);

						layoutSetsMatching = layouts.FindAllContiguous(x =>
							(x.ActionType == LayoutActionType.Point ||
							x.ActionType == LayoutActionType.MoveImplicit) &&
							x.Operation?.Tool == toolNameItem);

						foreach(List<OperationLayoutItem> layoutsMatching in
							layoutSetsMatching)
						{
							layoutsMatching.RemoveAll(x =>
								x.ActionType == LayoutActionType.MoveImplicit);
							if(layoutsMatching.Count > 0)
							{
								//	Transits before the current set.
								layoutIndex = layouts.IndexOf(layoutsMatching[0]);
								localLocation = AddPrecedingExplicitMoves(layouts,
									layoutIndex, moveIndices, tool, localLocation);

								//	Process the current set.
								minMax = GetMinMaxDepth(layoutsMatching);
								trackLayer = new TrackLayerItem()
								{
									BaseLayer = true,
									FinalLayer = true,
									Tool = tool
								};
								trackLayer.CurrentDepth = minMax.Maximum;
								trackLayer.TargetDepth = minMax.Maximum;
								segments = trackLayer.Segments;
								foreach(OperationLayoutItem layoutItem in layoutsMatching)
								{
									startOffset = layoutItem.ToolStartOffset;
									endOffset = layoutItem.ToolEndOffset;
									newLocation = endOffset;
									//	Plunge point found.
									if(localLocation != newLocation)
									{
										//	Transit to the site.
										segment = new TrackSegmentItem()
										{
											Operation = layoutItem.Operation,
											SegmentType = TrackSegmentType.Transit
										};
										FPoint.TransferValues(localLocation, segment.StartOffset);
										FPoint.TransferValues(startOffset, segment.EndOffset);
										segments.Add(segment);
										//	Drill the site.
										segment = new TrackSegmentItem()
										{
											Operation = layoutItem.Operation,
											SegmentType = TrackSegmentType.Plunge,
											Depth = ResolveDepth(layoutItem.Operation)
										};
										FPoint.TransferValues(startOffset, segment.StartOffset);
										FPoint.TransferValues(endOffset, segment.EndOffset);
										segments.Add(segment);
									}
									FPoint.TransferValues(newLocation, localLocation);
								}
								if(segments.Count > 0)
								{
									//	Every drill tool is on its own layer.
									//	Add the plunges as a separate layer.
									this.Add(trackLayer);
									trackLayer = null;
								}
							}
						}
					}
				}
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FillEllipse																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fill the ellipse described by the supplied layout item.
		/// </summary>
		/// <param name="layoutItem">
		/// Reference to the layout item to be drawn.
		/// </param>
		/// <param name="track">
		/// Reference to the track layer to which new segments will be stored.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location of the tool prior to drilling
		/// the first hole.
		/// </param>
		/// <returns>
		/// Reference to the updated last known location.
		/// </returns>
		private FPoint FillEllipse(OperationLayoutItem layoutItem,
			TrackLayerItem track, FPoint location)
		{
			FArea boundingBox = null;
			FPoint boxEndOffset = null;
			FPoint boxStartOffset = null;
			float cutDepth = 0f;
			FEllipse ellipse = null;
			FPoint endOffset = null;
			FPoint[] ends = null;
			int index = 0;
			FLine line = null;
			float lineY = 0f;
			FPoint localLocation = new FPoint(location);
			float radiusX = 0f;
			float radiusY = 0f;
			TrackSegmentItem segment = null;
			FPoint startOffset = null;
			float targetDepth = 0f;
			float toolOffset = 0f;

			if(layoutItem != null && track != null)
			{
				toolOffset = track.Tool.Diameter;
				targetDepth = ResolveDepth(layoutItem.Operation);
				cutDepth = Math.Min(track.CurrentDepth, targetDepth);
				startOffset = layoutItem.DisplayStartOffset;
				endOffset = layoutItem.DisplayEndOffset;
				//	There is material to cut.
				//	Transit to the upper left center.
				radiusX = (endOffset.X - startOffset.X) / 2f;
				radiusY = (endOffset.Y - startOffset.Y) / 2f;
				ellipse = new FEllipse(
					startOffset.X + radiusX,
					startOffset.Y + radiusY,
					radiusX - toolOffset,
					radiusY - toolOffset
					);
				boundingBox = FEllipse.BoundingBox(ellipse);
				boxStartOffset = new FPoint(boundingBox.Left, boundingBox.Top);
				boxEndOffset = new FPoint(boundingBox.Right, boundingBox.Bottom);
				//	Further increments will be by radius.
				toolOffset /= 2f;
				line = new FLine(
					new FPoint(startOffset.X, boundingBox.Top + toolOffset),
					new FPoint(endOffset.X, boundingBox.Top + toolOffset));
				ends = FEllipse.FindIntersections(ellipse, line, false);
				//	Reassign Start/End offsets to per-line usage.
				startOffset = null;
				endOffset = null;
				if(ends.Length > 0)
				{
					//	At least one end was found.
					startOffset = GetLeftPoint(ends);
					endOffset = GetRightPoint(ends);
				}
				//	Transit to site, if applicable.
				if(startOffset != localLocation)
				{
					segment = new TrackSegmentItem()
					{
						Operation = layoutItem.Operation,
						SegmentType = TrackSegmentType.Transit
					};
					FPoint.TransferValues(localLocation, segment.StartOffset);
					FPoint.TransferValues(startOffset, segment.EndOffset);
					track.Segments.Add(segment);
					FPoint.TransferValues(segment.EndOffset, localLocation);
				}
				//	Draw a radiator pattern of lines from the top to the bottom
				//	coordinates.
				for(index = 0, lineY = boxStartOffset.Y; lineY <= boxEndOffset.Y;
					index++, lineY += Math.Min(toolOffset, boxEndOffset.Y - lineY))
				{
					line.PointA.Y = line.PointB.Y = lineY;
					ends = FEllipse.FindIntersections(ellipse, line, false);
					if(index % 2 == 0)
					{
						//	Even pass. Left -> Right.
						startOffset = GetLeftPoint(ends);
						endOffset = GetRightPoint(ends);
					}
					else
					{
						//	Odd pass. Right -> Left.
						startOffset = GetRightPoint(ends);
						endOffset = GetLeftPoint(ends);
					}
					//	Plot downward to the next row, if appropriate.
					if(localLocation.Y != lineY)
					{
						segment = new TrackSegmentItem()
						{
							Operation = layoutItem.Operation,
							SegmentType = TrackSegmentType.Plot,
							Depth = cutDepth,
							TargetDepth = targetDepth,
							StartOffset = new FPoint(localLocation.X, localLocation.Y),
							EndOffset = new FPoint(startOffset.X, startOffset.Y)
						};
						track.Segments.Add(segment);
						FPoint.TransferValues(segment.EndOffset, localLocation);
					}
					//	Plot across the row.
					segment = new TrackSegmentItem()
					{
						Operation = layoutItem.Operation,
						SegmentType = TrackSegmentType.Plot,
						Depth = cutDepth,
						TargetDepth = targetDepth,
						StartOffset = new FPoint(startOffset.X, startOffset.Y),
						EndOffset = new FPoint(endOffset.X, endOffset.Y)
					};
					track.Segments.Add(segment);
					FPoint.TransferValues(segment.EndOffset, localLocation);
				}
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FillRectangle																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fill the rectangle described by the supplied layout item.
		/// </summary>
		/// <param name="layoutItem">
		/// Reference to the layout item to be drawn.
		/// </param>
		/// <param name="track">
		/// Reference to the track layer to which new segments will be stored.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location of the tool prior to drilling
		/// the first hole.
		/// </param>
		/// <returns>
		/// Reference to the updated last known location.
		/// </returns>
		private FPoint FillRectangle(OperationLayoutItem layoutItem,
			TrackLayerItem track, FPoint location)
		{
			FArea boundingBox = null;
			FPoint boxEndOffset = null;
			float boxLeft = 0f;
			float boxRight = 0f;
			FPoint boxStartOffset = null;
			float cutDepth = 0f;
			FPoint endOffset = null;
			int index = 0;
			List<FLine> lines = null;
			float lineY = 0f;
			FPoint localLocation = new FPoint(location);
			TrackSegmentItem segment = null;
			FPoint startOffset = null;
			float targetDepth = 0f;
			float toolOffset = 0f;

			if(layoutItem != null && track != null)
			{
				toolOffset = track.Tool.Diameter;
				targetDepth = ResolveDepth(layoutItem.Operation);
				cutDepth = Math.Min(track.CurrentDepth, targetDepth);
				startOffset = layoutItem.DisplayStartOffset;
				endOffset = layoutItem.DisplayEndOffset;
				//	There is material to cut.
				//	Transit to the upper left corner.
				boundingBox = new FArea(
					layoutItem.DisplayStartOffset,
					layoutItem.DisplayEndOffset);
				//	Further increments will be by radius.
				toolOffset /= 2f;
				//	Shrink the area uniformly by 1 tool radius.
				boundingBox.Left += toolOffset;
				boundingBox.Top += toolOffset;
				boundingBox.Right -= toolOffset;
				boundingBox.Bottom -= toolOffset;
				boxLeft = boundingBox.Left;
				boxRight = boundingBox.Right;
				boxStartOffset = new FPoint(boundingBox.Left, boundingBox.Top);
				boxEndOffset = new FPoint(boundingBox.Right, boundingBox.Bottom);
				//	Reassign Start/End offsets to per-line usage.
				startOffset = null;
				endOffset = null;
				lines = FArea.GetLines(boundingBox);
				startOffset = new FPoint(boxStartOffset);
				endOffset = new FPoint(boundingBox.Right, boundingBox.Top);
				//	Transit to site, if applicable.
				if(startOffset != localLocation)
				{
					segment = new TrackSegmentItem()
					{
						Operation = layoutItem.Operation,
						SegmentType = TrackSegmentType.Transit
					};
					FPoint.TransferValues(localLocation, segment.StartOffset);
					FPoint.TransferValues(startOffset, segment.EndOffset);
					track.Segments.Add(segment);
					FPoint.TransferValues(segment.EndOffset, localLocation);
				}
				//	Draw a radiator pattern of lines from the top left to the bottom
				//	right coordinates.
				for(index = 0, lineY = boxStartOffset.Y; lineY <= boxEndOffset.Y;
					index ++, lineY += Math.Min(toolOffset, boxEndOffset.Y - lineY))
				{
					startOffset.Y = endOffset.Y = lineY;
					if(index % 2 == 0)
					{
						//	Even pass. Left -> Right.
						startOffset.X = boxLeft;
						endOffset.X = boxRight;
					}
					else
					{
						//	Odd pass. Right -> Left.
						startOffset.X = boxRight;
						endOffset.X = boxLeft;
					}
					//	Plot downward to the next row, if appropriate.
					if(localLocation.Y != lineY)
					{
						segment = new TrackSegmentItem()
						{
							Operation = layoutItem.Operation,
							SegmentType = TrackSegmentType.Plot,
							Depth = cutDepth,
							TargetDepth = targetDepth,
							StartOffset = new FPoint(localLocation.X, localLocation.Y),
							EndOffset = new FPoint(startOffset.X, startOffset.Y)
						};
						track.Segments.Add(segment);
						FPoint.TransferValues(segment.EndOffset, localLocation);
					}
					//	Plot across the row.
					segment = new TrackSegmentItem()
					{
						Operation = layoutItem.Operation,
						SegmentType = TrackSegmentType.Plot,
						Depth = cutDepth,
						TargetDepth = targetDepth,
						StartOffset = new FPoint(startOffset.X, startOffset.Y),
						EndOffset = new FPoint(endOffset.X, endOffset.Y)
					};
					track.Segments.Add(segment);
					FPoint.TransferValues(segment.EndOffset, localLocation);
				}
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetLeftPoint																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the leftmost point from the caller's array.
		/// </summary>
		/// <param name="points">
		/// Reference to an array of points.
		/// </param>
		/// <returns>
		/// Reference to the piont with the lowest x-axis value in the set, if
		/// found. Otherwise, null.
		/// </returns>
		private static FPoint GetLeftPoint(FPoint[] points)
		{
			FPoint match = null;
			FPoint result = null;

			if(points?.Length > 0)
			{
				foreach(FPoint pointItem in points)
				{
					if(match == null || pointItem.X < match.X)
					{
						match = pointItem;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetMinMaxDepth																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the minimum and maximum required depths for the provided set of
		/// layouts.
		/// </summary>
		/// <param name="layouts">
		/// Reference to the set of layouts to test for minimum and maximum.
		/// </param>
		/// <returns>
		/// Reference to a minimum/maximum value representing the minimum and
		/// maximum depths of the set of layout actions.
		/// </returns>
		/// <remarks>
		/// This method doesn't consider whether the specified depths for the
		/// layer are within range for the current pass.
		/// </remarks>
		private MinMaxItem GetMinMaxDepth(List<OperationLayoutItem> layouts)
		{
			float depth = 0f;
			float maxDepth = float.MinValue;
			float minDepth = float.MaxValue;
			MinMaxItem result = new MinMaxItem();

			if(layouts?.Count > 0)
			{
				foreach(OperationLayoutItem layoutItem in layouts)
				{
					depth = ResolveDepth(layoutItem.Operation);
					minDepth = Math.Min(minDepth, depth);
					maxDepth = Math.Max(maxDepth, depth);
				}
				if(minDepth == float.MaxValue)
				{
					minDepth = 0f;
				}
				if(maxDepth == float.MinValue)
				{
					maxDepth = 0f;
				}
				result.Minimum = minDepth;
				result.Maximum = maxDepth;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetRightPoint																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the rightmost point from the caller's array.
		/// </summary>
		/// <param name="points">
		/// Reference to an array of points.
		/// </param>
		/// <returns>
		/// Reference to the piont with the highest x-axis value in the set, if
		/// found. Otherwise, null.
		/// </returns>
		private static FPoint GetRightPoint(FPoint[] points)
		{
			FPoint match = null;
			FPoint result = null;

			if(points?.Length > 0)
			{
				foreach(FPoint pointItem in points)
				{
					if(match == null || pointItem.X > match.X)
					{
						match = pointItem;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* MoveExplicit																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Move the tool to another location on the board.
		/// </summary>
		/// <param name="layoutItem">
		/// Reference to the layout item being processed.
		/// </param>
		/// <param name="trackLayer">
		/// Reference to the track layer into which the actions will be placed.
		/// </param>
		/// <param name="location">
		/// Reference to the active location from which the move is starting.
		/// </param>
		/// <returns>
		/// Reference to the updated current location.
		/// </returns>
		private FPoint MoveExplicit(OperationLayoutItem layoutItem,
			TrackLayerItem trackLayer, FPoint location)
		{
			FPoint endOffset = null;
			FPoint localLocation = FPoint.Clone(location);
			TrackSegmentItem segment = null;
			FPoint startOffset = null;

			if(layoutItem != null && trackLayer != null)
			{
				startOffset = layoutItem.ToolStartOffset;
				endOffset = layoutItem.ToolEndOffset;
				//	Transit to the line.
				segment = new TrackSegmentItem()
				{
					Operation = layoutItem.Operation,
					SegmentType = TrackSegmentType.Transit
				};
				FPoint.TransferValues(startOffset, segment.StartOffset);
				FPoint.TransferValues(endOffset, segment.EndOffset);
				trackLayer.Segments.Add(segment);
				FPoint.TransferValues(endOffset, localLocation);
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PlotEndingMovements																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Plot the set of transit that occur after the end of all of the cutting
		/// actions.
		/// </summary>
		/// <param name="layouts">
		/// Reference to the full collection of layouts beeing processed in this
		/// session.
		/// </param>
		/// <param name="trackTools">
		/// Reference to the collection of tools found in this session.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location.
		/// </param>
		/// <returns>
		/// Reference to the last-known current location after the movements have
		/// been applied.
		/// </returns>
		private FPoint PlotEndingMovements(OperationLayoutCollection layouts,
			TrackToolCollection trackTools, FPoint location)
		{
			bool bToolSet = false;
			List<OperationLayoutItem> layoutsMatching = null;
			FPoint localLocation = FPoint.Clone(location);
			TrackLayerItem trackLayer = null;

			if(layouts?.Count > 0 && trackTools != null)
			{
				layoutsMatching = layouts.FindAllEnding(x =>
					x.ActionType == LayoutActionType.MoveExplicit);
				if(layoutsMatching.Count > 0)
				{
					//	One or more moves directly preceded this set.
					trackLayer = new TrackLayerItem()
					{
						BaseLayer = true
					};
					foreach(OperationLayoutItem layoutItem in layoutsMatching)
					{
						if(!bToolSet &&
							layoutItem.Operation != null)
						{
							trackLayer.Tool = trackTools.FirstOrDefault(x =>
								x.ToolName == layoutItem.Operation.Tool);
							bToolSet = true;
						}
						localLocation = MoveExplicit(layoutItem, trackLayer,
							localLocation);
						//	When the move action has been allocated, make sure not to
						//	reprocess it.
						layoutItem.ActionType = LayoutActionType.None;
					}
					this.Add(trackLayer);

				}
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PlotLinesAndShapes																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Plot all basic lines and shapes.
		/// </summary>
		/// <param name="layouts">
		/// Reference to the collection of layout elements for which to drill the
		/// holes.
		/// </param>
		/// <param name="tools">
		/// Reference to the collection of tools available for the holes.
		/// </param>
		/// <param name="location">
		/// Reference to the starting location of the tool prior to drilling
		/// the first hole.
		/// </param>
		/// <returns>
		/// Reference to the updated last known location.
		/// </returns>
		private FPoint PlotLinesAndShapes(OperationLayoutCollection layouts,
			TrackToolCollection tools, FPoint location)
		{
			string defaultToolName = "";
			//OperationLayoutItem last = null;
			int layoutIndex = 0;
			List<List<OperationLayoutItem>> layoutSetsMatching = null;
			//List<OperationLayoutItem> layoutsMatching = null;
			FPoint localLocation = new FPoint(location);
			MinMaxItem minMax = null;
			IntRangeCollection moveIndices = null;
			//List<OperationLayoutItem> moveSet = null;
			List<List<OperationLayoutItem>> moveSets = null;
			TrackSegmentCollection segments = null;
			TrackToolItem tool = null;
			List<string> toolNames = null;
			TrackLayerItem trackLayer = null;

			if(layouts?.Count > 0 && tools?.Count > 0)
			{
				moveSets = layouts.FindAllContiguous(x =>
					x.ActionType == LayoutActionType.MoveExplicit);
				moveIndices = layouts.GetIndexRanges(moveSets);

				tool = tools.FirstOrDefault(x => x.IsDefault);
				if(tool != null)
				{
					defaultToolName = tool.ToolName;
				}
				//	One separate layer per tool.
				toolNames = layouts.Select(x => x.Operation?.Tool).Distinct().ToList();
				if(toolNames.Contains(null))
				{
					//	Non-operational layout elements are not supported in this
					//	version.
					toolNames.Remove(null);
				}
				foreach(string toolNameItem in toolNames)
				{
					tool = tools.SelectTool(toolNameItem);

					layoutSetsMatching = layouts.FindAllContiguous(x =>
						x.ActionType != LayoutActionType.Point &&
						x.ActionType != LayoutActionType.MoveExplicit &&
						x.ActionType != LayoutActionType.MoveImplicit &&
						x.Operation?.Tool == toolNameItem);
					foreach(List<OperationLayoutItem> layoutsMatching in
						layoutSetsMatching)
					{
						//layoutsMatching.RemoveAll(x =>
						//	x.ActionType == LayoutActionType.MoveImplicit);
						if(layoutsMatching.Count > 0)
						{
							//	Transits before the current set.
							layoutIndex = layouts.IndexOf(layoutsMatching[0]);
							localLocation = AddPrecedingExplicitMoves(layouts,
								layoutIndex, moveIndices, tool, localLocation);

							//	Process the current set.
							minMax = GetMinMaxDepth(layoutsMatching);
							trackLayer = new TrackLayerItem()
							{
								BaseLayer = true,
								Tool = tool
							};
							SetCurrentTargetDepths(trackLayer, minMax, tool);
							segments = trackLayer.Segments;
							foreach(OperationLayoutItem layoutItem in layoutsMatching)
							{
								//startOffset = layoutItem.ToolStartOffset;
								//endOffset = layoutItem.ToolEndOffset;
								switch(layoutItem.ActionType)
								{
									case LayoutActionType.DrawArc:
										localLocation =
											DrawArc(layoutItem, trackLayer, localLocation);
										break;
									case LayoutActionType.DrawEllipse:
										localLocation =
											DrawEllipse(layoutItem, trackLayer, localLocation);
										break;
									case LayoutActionType.DrawLine:
										localLocation =
											DrawLine(layoutItem, trackLayer, localLocation);
										break;
									case LayoutActionType.DrawRectangle:
										localLocation =
											DrawRectangle(layoutItem, trackLayer, localLocation);
										break;
									case LayoutActionType.FillEllipse:
										//	This shape is a non-kerf area preceded by a DrawEllipse
										//	outline having a left-hand kerf as an outline, which
										//	means that the outer ring of the shape has already been
										//	drawn when this call is made.
										localLocation =
											FillEllipse(layoutItem, trackLayer, localLocation);
										break;
									case LayoutActionType.FillRectangle:
										//	This shape is a non-kerf area preceded by a
										//	DrawRectangle outline with a left-kerf as an outline,
										//	which means that the outer ring of the shape has
										//	already been drawn.
										localLocation =
											FillRectangle(layoutItem, trackLayer, localLocation);
										break;
									case LayoutActionType.MoveExplicit:
									case LayoutActionType.MoveImplicit:
									case LayoutActionType.None:
									case LayoutActionType.Point:
										//	These cases can be removed in final production.
										//	Movements are processed separately in this version.
										//	Actions of type None are ignored.
										//	Points (drills) are handled separately first.
										break;
								}
								//last = layoutItem;
							}
							if(segments.Count > 0)
							{
								this.Add(trackLayer);
							}
						}
					}
				}
			}
			return localLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PrepareFills																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Prepare the filled shapes by creating a preceding outline for each one.
		/// </summary>
		/// <param name="layouts">
		/// Reference to the layouts that might contain filled shapes.
		/// </param>
		/// <remarks>
		/// The collection is updated inline.
		/// </remarks>
		private void PrepareFills(OperationLayoutCollection layouts)
		{
			int count = 0;
			int index = 0;
			OperationLayoutItem layout = null;
			PatternOperationItem operation = null;

			//	This item visits each entry in the list once so multiple passes
			//	aren't necessary to repeatedly look up the index.
			if(layouts?.Count > 0)
			{
				count = layouts.Count;
				for(index = 0; index < count; index++)
				{
					layout = layouts[index];
					if(layout.ActionType == LayoutActionType.FillEllipse ||
						layout.ActionType == LayoutActionType.FillRectangle)
					{
						//	This item will receive an outline.
						operation = PatternOperationItem.Clone(layout.Operation);
						operation.Kerf = DirectionLeftRightEnum.Left;
						layout = OperationLayoutItem.Clone(layout);
						if(layout.ActionType == LayoutActionType.FillEllipse)
						{
							layout.ActionType = LayoutActionType.DrawEllipse;
							layout.Operation = operation;
							layouts.Insert(index, layout);
							//	Skip past this item next time.
							index++;
							count++;
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RelateExplicitMovements																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Relate the explicitly defined movements in the list to the actions they
		/// directly precede or follow.
		/// </summary>
		/// <param name="layouts">
		/// Reference to the operational layouts currently being processed.
		/// </param>
		/// <remarks>
		/// High-speed movement commands are not naturally associated with a tool,
		/// but giving them the same tool as their closest tool-bearing neighbor
		/// has the effect of grouping them with those tool-based actions.
		/// </remarks>
		private static void RelateExplicitMovements(
			OperationLayoutCollection layouts)
		{
			int count = 0;
			int index = 0;
			OperationLayoutItem last = null;
			OperationLayoutItem layout = null;
			OperationLayoutItem next = null;

			if(layouts?.Count > 0)
			{
				count = layouts.Count;
				for(index = 0; index < count; index ++)
				{
					layout = layouts[index];
					if(layout.ActionType == LayoutActionType.MoveExplicit)
					{
						next = layouts.NextMatchAfter(layout,
							x => x.ActionType != LayoutActionType.MoveExplicit &&
								x.ActionType != LayoutActionType.MoveImplicit &&
								x.Operation != null);
						if(next != null)
						{
							//	Next match was found.
							if(layout.Operation == null)
							{
								layout.Operation = next.Operation;
							}
							else
							{
								layout.Operation.Tool = next.Operation.Tool;
							}
						}
						else if(last != null)
						{
							//	No next match was found, but a previous operation was
							//	visited.
							if(layout.Operation == null)
							{
								layout.Operation = last.Operation;
							}
							else
							{
								layout.Operation.Tool = last.Operation.Tool;
							}
						}
					}
					else if(layout.Operation != null)
					{
						last = layout;
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ResolveToolPath																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Resolve the tool paths of each track.
		/// </summary>
		/// <param name="location">
		/// The last known location of the tool.
		/// </param>
		/// <returns>
		/// Reference to the last known location.
		/// </returns>
		private FPoint ResolveToolPath(FPoint location)
		{
			int count = 0;
			float depth = 0f;
			FPoint endOffset = null;
			int index = 0;
			TrackLayerItem lastLayer = null;
			FPoint lastLocation = new FPoint(location);
			TrackLayerItem layer = null;
			float maxDepth = 0f;
			float maxDepthPerPass = 0f;
			TrackSegmentItem prevSegment = null;
			TrackSegmentItem segment = null;
			TrackSegmentCollection segments = null;
			FPoint startOffset = null;
			TrackLayerItem track = null;
			int trackCount = 0;
			int trackIndex = 0;

			//	NOTE: At this point, there will only be different layers if
			//	different tools are in use. If everything can be performed
			//	with paths and a single tool, a single base layer will be
			//	present.
			//	If drills and plots of a single tool are present, the drills from
			//	that action will be present on one layer and the plots will be
			//	present on another layer.
			//	All drills are skipped during this process because they are already
			//	completed.
			//	Because each layer represents a different tool at this stage,
			//	each new layer for an incremental track will be inserted directly
			//	behind the base.

			//	Reversing paths until all layers are completed.
			trackCount = this.Count;
			for(trackIndex = 0; trackIndex < trackCount; trackIndex ++)
			{
				track = this[trackIndex];
				if(track.BaseLayer &&
					track.Segments.Count(x =>
						x.SegmentType == TrackSegmentType.Plot) > 0)
				{
					//	This track can be resolved.
					maxDepth = track.TargetDepth;
					maxDepthPerPass = track.Tool.Diameter / 2f;
					depth = Math.Min(maxDepth,
						track.CurrentDepth + maxDepthPerPass);
					startOffset = new FPoint();
					endOffset = new FPoint();
					lastLayer = track;
					layer = new TrackLayerItem()
					{
						CurrentDepth = depth,
						TargetDepth = maxDepth,
						Tool = lastLayer.Tool
					};
					segments = layer.Segments;
					while(depth <= maxDepth)
					{
						//	Repeat the layers in opposite directions until the maximum
						//	required depth has been reached.
						count = lastLayer.Segments.Count;
						for(index = count - 1; index > -1; index--)
						{
							prevSegment = lastLayer.Segments[index];
							if(prevSegment.SegmentType == TrackSegmentType.Plot &&
								prevSegment.TargetDepth >= depth)
							{
								//	The previous ending point will be the current starting
								//	point.
								FPoint.TransferValues(prevSegment.StartOffset, endOffset);
								FPoint.TransferValues(prevSegment.EndOffset, startOffset);
								//	In this version, lines must be connected to be contiguous.
								//if(lastLocation != startOffset)
								//{
								//	//	Create a transit.
								//	segment = new TrackSegmentItem()
								//	{
								//		SegmentType = TrackSegmentType.Transit
								//	};
								//	FPoint.TransferValues(lastLocation, segment.StartOffset);
								//	FPoint.TransferValues(startOffset, segment.EndOffset);
								//	segments.Add(segment);
								//}
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
							this.Insert(trackIndex + 1, layer);
							trackCount++;
							trackIndex++;
							lastLayer = layer;
							layer = new TrackLayerItem()
							{
								CurrentDepth = depth,
								TargetDepth = maxDepth,
								Tool = lastLayer.Tool
							};
							segments = layer.Segments;
						}
						else
						{
							break;
						}
					}
				}
			}
			return lastLocation;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SetCurrentTargetDepths																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Set the current and target depths for the layer using requirements from
		/// the selected tool and minimum / maximum required depths.
		/// </summary>
		/// <param name="layer">
		/// Reference to the layer upon which the values will be set.
		/// </param>
		/// <param name="minMax">
		/// Minimum and maximum depths requested on the layer.
		/// </param>
		/// <param name="tool">
		/// The active tool assigned to the layer.
		/// </param>
		/// <param name="previousDepth">
		/// The previous depth already reached.
		/// </param>
		/// <returns>
		/// True if some goal has been set. Otherwise, false.
		/// </returns>
		/// <remarks>
		/// This value is set inline on the caller's layer.
		/// </remarks>
		private bool SetCurrentTargetDepths(TrackLayerItem layer,
			MinMaxItem minMax, TrackToolItem tool, float previousDepth = 0f)
		{
			float difference = 0f;
			float increment = 0f;
			bool result = false;

			if(layer != null && tool != null && minMax != null)
			{
				if(minMax.Maximum > previousDepth)
				{
					if(tool != null)
					{
						increment = tool.MaxDepthPerPass;
					}
					else
					{
						increment = mDefaultMaxDepthPerPass;
					}
					difference = minMax.Maximum - previousDepth;
					layer.CurrentDepth = previousDepth + Math.Min(difference, increment);
					layer.TargetDepth = minMax.Maximum;
					result = true;
				}
				else
				{
					//	Target depth already achieved.
					layer.CurrentDepth = layer.TargetDepth = previousDepth;
				}
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
			FPoint lastLocation = null;
			OperationLayoutCollection layouts = null;
			FLine line1 = new FLine();
			FLine line2 = new FLine();
			List<FLine> lineList = null;
			FPoint location = null;
			TrackToolCollection toolSet = new TrackToolCollection();
			WorkpieceInfoItem workpiece = SessionWorkpieceInfo;

			//	There may be explicit MoveTo actions dispersed throughout the
			//	cut list. Each MoveTo, including those defined as first and last
			//	moves, should be observed as being a specific break between tracks.
			//	In the meantime, any implicit move-to actions that were inherited
			//	with the starts or continuations of normal actions are absorbed
			//	during the rendering process.
			if(cutList?.Count > 0 && workpiece != null)
			{
				layouts = OperationLayoutCollection.CloneLayout(cutList);
				lineList = new List<FLine>();
				toolSet = new TrackToolCollection();
				toolSet.Initialize(layouts);

				//	TODO: !1 - Stopped here...
				//	TODO: Fix the depth problem that occurs after a hard transit.
				//	That must be found in GCode and TrackViewLayer, beause the 2D info + depth is clean here...

				RelateExplicitMovements(layouts);

				location = lastLocation =
					TransformFromAbsolute(workpiece.RouterLocation);
				//	*** FILL PREPARATION ***
				PrepareFills(layouts);
				//	*** DRILL HOLES ***
				location = DrillHoles(layouts, toolSet, location);
				//	*** LINES AND SHAPES ***
				//	Get all direct plots, skipping plunges.
				location = PlotLinesAndShapes(layouts, toolSet, location);
				//	*** ADJUST KERF ***
				location = AdjustKerf();
				//	*** RESOLVE TOOL PATH ***
				location = ResolveToolPath(location);
				//	*** APPEND ENDING TRANSITS ***
				location = PlotEndingMovements(layouts, toolSet, location);
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
		//*	BaseLayer																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="BaseLayer">BaseLayer</see>.
		/// </summary>
		private bool mBaseLayer = false;
		/// <summary>
		/// Get/Set a value indicating whether this is a base layer, upon which
		/// can be built multiple passes.
		/// </summary>
		/// <remarks>
		/// A read access on this property is blended with the FinalLayer property.
		/// If FinalLayer is true, BaseLayer will always return false.
		/// </remarks>
		public bool BaseLayer
		{
			get { return mBaseLayer && !mFinalLayer; }
			set { mBaseLayer = value; }
		}
		//*-----------------------------------------------------------------------*

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
		//*	FinalLayer																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="FinalLayer">FinalLayer</see>.
		/// </summary>
		private bool mFinalLayer = false;
		/// <summary>
		/// Get/Set a value indicating whether this is a final layer onto which no
		/// further layers should be stacked.
		/// </summary>
		public bool FinalLayer
		{
			get { return mFinalLayer; }
			set { mFinalLayer = value; }
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

		//*-----------------------------------------------------------------------*
		//*	Tool																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Tool">Tool</see>.
		/// </summary>
		private TrackToolItem mTool = null;
		/// <summary>
		/// Get/Set a reference to the tool selected for this layer.
		/// </summary>
		public TrackToolItem Tool
		{
			get { return mTool; }
			set { mTool = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
