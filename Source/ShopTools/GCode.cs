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
	//*	GCode																																		*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Features and functionality for processing G-code.
	/// </summary>
	public class GCode
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
		//* GetPositionZAbs																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the absolute position of Z at the specified position type.
		/// </summary>
		/// <param name="zPositionType">
		/// The type of position to which to check the Z-axis value.
		/// </param>
		/// <param name="depthOffset">
		/// Optional depth offset to add to the found position.
		/// </param>
		/// <returns>
		/// The absolute position of the Z-axis at the specified position type,
		/// if known. Otherwise, 0.
		/// </returns>
		public static float GetPositionZAbs(TransitZEnum zPositionType,
			float depthOffset = 0f)
		{
			float depth = 0f;
			float extent = 0f;
			float reach = 0f;
			float result = 0f;

			if(ConfigProfile != null)
			{
				//	A configuration is loaded.
				reach = GetMillimeters(ConfigProfile.Depth);
				depth = SessionWorkpieceInfo.Thickness;
				if(reach != 0f)
				{
					switch(ConfigProfile.ZOrigin)
					{
						case OriginLocationEnum.Bottom:
							//	The head's maximum depth is 0.
							extent = reach;
							switch(ConfigProfile.TravelZ)
							{
								case DirectionUpDownEnum.Down:
									//	Fully extended, 0.
									//	Fully retracted, -reach.
									switch(zPositionType)
									{
										case TransitZEnum.FullyExtended:
											result = 0f;
											break;
										case TransitZEnum.FullyRetracted:
											result = 0f - extent;
											break;
										case TransitZEnum.TopOfMaterial:
											result = 0f - depth;
											break;
									}
									result += depthOffset;
									break;
								case DirectionUpDownEnum.None:
								case DirectionUpDownEnum.Up:
									//	Fully extended, 0.
									//	Fully retracted, +reach.
									switch(zPositionType)
									{
										case TransitZEnum.FullyExtended:
											result = 0f;
											break;
										case TransitZEnum.FullyRetracted:
											result = extent;
											break;
										case TransitZEnum.TopOfMaterial:
											result = depth;
											break;
									}
									result -= depthOffset;
									break;
							}
							break;
						case OriginLocationEnum.Center:
						case OriginLocationEnum.None:
							//	The head is homed in the center of its travel.
							extent = reach / 2f;
							switch(ConfigProfile.TravelZ)
							{
								case DirectionUpDownEnum.Down:
									//	Fully extended, +reach / 2.
									//	Fully retracted, -reach / 2.
									switch(zPositionType)
									{
										case TransitZEnum.FullyExtended:
											result = extent;
											break;
										case TransitZEnum.FullyRetracted:
											result = 0f - extent;
											break;
										case TransitZEnum.TopOfMaterial:
											result = extent - depth;
											break;
									}
									result += depthOffset;
									break;
								case DirectionUpDownEnum.None:
								case DirectionUpDownEnum.Up:
									//	Fully extended, -reach / 2.
									//	Fully retracted, +reach / 2.
									switch(zPositionType)
									{
										case TransitZEnum.FullyExtended:
											result = 0f - extent;
											break;
										case TransitZEnum.FullyRetracted:
											result = extent;
											break;
										case TransitZEnum.TopOfMaterial:
											result = (0f - extent) + depth;
											break;
									}
									result -= depthOffset;
									break;
							}
							break;
						case OriginLocationEnum.Top:
							//	The head is at 0 when fully retracted.
							extent = reach;
							switch(ConfigProfile.TravelZ)
							{
								case DirectionUpDownEnum.Down:
									//	Fully extended, +reach.
									//	Fully retracted, 0.
									switch(zPositionType)
									{
										case TransitZEnum.FullyExtended:
											result = extent;
											break;
										case TransitZEnum.FullyRetracted:
											result = 0f;
											break;
										case TransitZEnum.TopOfMaterial:
											result = extent - depth;
											break;
									}
									result += depthOffset;
									break;
								case DirectionUpDownEnum.None:
								case DirectionUpDownEnum.Up:
									//	Fully extended, -reach.
									//	Fully retracted, 0.
									switch(zPositionType)
									{
										case TransitZEnum.FullyExtended:
											result = 0f - extent;
											break;
										case TransitZEnum.FullyRetracted:
											result = 0f;
											break;
										case TransitZEnum.TopOfMaterial:
											result = (0f - extent) + depth;
											break;
									}
									result -= depthOffset;
									break;
							}
							break;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PlotToPositionXYAbs																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Plot to an absolute location on the canvas.
		/// </summary>
		/// <param name="offset">
		/// Reference to the absolute offset from the physical origin.
		/// </param>
		/// <param name="feedRate">
		/// The feed rate at which to plot to the destination.
		/// </param>
		/// <returns>
		/// Reference to a string representing the command to plot to the
		/// specified XY coordinates at the specified feedrate, if
		/// valid. Otherwise, an empty string.
		/// </returns>
		public static string PlotToPositionXYAbs(FPoint offset, float feedRate)
		{
			StringBuilder builder = new StringBuilder();

			if(offset != null)
			{
				builder.Append($"G01 X{offset.X} Y{offset.Y} F{feedRate};");
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PlungeZAbs																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Plunge the Z-axis into the material.
		/// </summary>
		/// <param name="plungeDepth">
		/// The depth to plunge from the registered height.
		/// </param>
		/// <param name="feedRate">
		/// The feed rate at which to make the plunge.
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
		public static string PlungeZAbs(float plungeDepth, float feedRate)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append($"G01 Z{plungeDepth} F{feedRate};");

			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PlungeZRel																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Plunge the Z-axis into the material.
		/// </summary>
		/// <param name="plungeDepth">
		/// The depth to plunge from the registered height.
		/// </param>
		/// <param name="feedRate">
		/// The feed rate at which to make the plunge.
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
		public static string PlungeZRel(float plungeDepth, float feedRate)
		{
			StringBuilder builder = new StringBuilder();
			float depth = 0f;

			//	Transit Z to the material top.
			builder.AppendLine(TransitToPositionZAbs(TransitZEnum.TopOfMaterial));
			//	Use relative movement for this section.
			builder.AppendLine("G91;");
			if(ConfigProfile.TravelZ == DirectionUpDownEnum.Down)
			{
				depth = plungeDepth;
			}
			else
			{
				depth = 0f - plungeDepth;
			}
			builder.AppendLine($"G01 Z{depth} F{feedRate};");
			//	Return to absolute.
			builder.Append("G90;");

			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* RenderGCode																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the G-code corresponding to the current configuration and
		/// operations in the provided workpiece.
		/// </summary>
		/// <param name="workpiece">
		/// Reference to the workpiece for which the G-code will be rendered.
		/// </param>
		/// <returns>
		/// A string containing G-code instructions for the workpiece and its
		/// operations, if present. Otherwise, an empty string.
		/// </returns>
		public static string RenderGCode()
		{
			bool bRetracted = false;
			StringBuilder builder = new StringBuilder();
			TrackSegmentItem lastSegment = null;
			int layerIndex = 0;
			FPoint location = null;
			TrackSegmentItem segment = null;
			TrackLayerCollection trackLayers = null;
			WorkpieceInfoItem workpiece = SessionWorkpieceInfo;

			if(workpiece?.Cuts.Count > 0)
			{
				//	There are cuts to convert.
				builder.AppendLine("(Generated by Dan's ShopTools)");
				builder.AppendLine("(All units mm.)");
				builder.AppendLine("(All measurements absolute by default.)");
				builder.AppendLine("G21;");
				builder.AppendLine("G90;");
				builder.AppendLine(TransitToPositionZAbs(TransitZEnum.FullyRetracted));
				bRetracted = true;

				location = new FPoint();
				trackLayers = new TrackLayerCollection(workpiece.Cuts);
				foreach(TrackLayerItem layerItem in trackLayers)
				{
					if(layerIndex > 0 && layerItem.Segments.Count > 0)
					{
						segment = layerItem.Segments[0];
						if(segment.SegmentType == TrackSegmentType.Plot)
						{
							builder.AppendLine(
								PlungeZAbs(
									GetPositionZAbs(
										TransitZEnum.TopOfMaterial,
										segment.Depth),
									segment.FeedRate));
						}
					}
					foreach(TrackSegmentItem segmentItem in layerItem.Segments)
					{
						switch(segmentItem.SegmentType)
						{
							case TrackSegmentType.Plot:
								if(bRetracted)
								{
									//	Lower the boom.
									builder.AppendLine(
										TransitToPositionZAbs(TransitZEnum.TopOfMaterial));
								}
								if(lastSegment?.SegmentType != TrackSegmentType.Plot)
								{
									//	Dig in.
									builder.AppendLine(
										PlungeZAbs(
											GetPositionZAbs(
												TransitZEnum.TopOfMaterial,
												segmentItem.Depth),
											segmentItem.FeedRate));
								}
								builder.AppendLine(
									PlotToPositionXYAbs(segmentItem.EndOffset,
										segmentItem.FeedRate));
								FPoint.TransferValues(segmentItem.EndOffset, location);
								bRetracted = false;
								break;
							case TrackSegmentType.Plunge:
								if(!FPoint.Equals(segmentItem.StartOffset, location))
								{
									builder.AppendLine(
										TransitToPositionXYAbs(segmentItem.StartOffset));
									FPoint.TransferValues(segmentItem.StartOffset, location);
								}
								builder.AppendLine(
									PlungeZAbs(
										GetPositionZAbs(
											TransitZEnum.TopOfMaterial,
											segmentItem.Depth),
										segmentItem.FeedRate));
								builder.AppendLine(
									TransitToPositionZAbs(TransitZEnum.FullyRetracted));
								bRetracted = true;
								break;
							case TrackSegmentType.Transit:
								if(!bRetracted)
								{
									builder.AppendLine(
										TransitToPositionZAbs(TransitZEnum.FullyRetracted));
									bRetracted = true;
								}
								builder.AppendLine(
									TransitToPositionXYAbs(segmentItem.EndOffset));
								FPoint.TransferValues(segmentItem.EndOffset, location);
								break;
						}
						lastSegment = segmentItem;
					}
					layerIndex++;
				}
				if(!bRetracted)
				{
					builder.AppendLine(
						TransitToPositionZAbs(TransitZEnum.FullyRetracted));
					bRetracted = true;
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransitToPositionXYAbs																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Transit to an absolute location on the canvas.
		/// </summary>
		/// <param name="offset">
		/// Reference to the absolute offset from the physical origin.
		/// </param>
		/// <returns>
		/// Reference to a string representing the command to transit to the
		/// specified XY coordinates at the maximum possible feedrate, if
		/// valid. Otherwise, an empty string.
		/// </returns>
		public static string TransitToPositionXYAbs(FPoint offset)
		{
			StringBuilder builder = new StringBuilder();

			if(offset != null)
			{
				builder.Append($"G00 X{offset.X} Y{offset.Y};");
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransitToPositionZAbs																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the command to move the Z axis to transit position.
		/// </summary>
		/// <param name="zPositionType">
		/// The type of position to which the Z-axis will be transited.
		/// </param>
		/// <returns>
		/// Reference to a string containing the command to move the Z axis to
		/// its topmost position for transit.
		/// </returns>
		public static string TransitToPositionZAbs(TransitZEnum zPositionType)
		{
			return $"G00 Z{GetPositionZAbs(zPositionType)}";
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*


}
