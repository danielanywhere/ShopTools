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
		public static string PlotToPositionXYAbs(FVector2 offset, float feedRate)
		{
			StringBuilder builder = new StringBuilder();

			if(offset != null)
			{
				builder.Append(
					$"G01 X{offset.X:0.###} Y{offset.Y:0.###} F{feedRate};");
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

			builder.Append($"G01 Z{plungeDepth:0.###} F{feedRate};");

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
			builder.AppendLine($"G01 Z{depth:0.###} F{feedRate};");
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
		/// <param name="filenameBase">
		/// The pattern to use for the output filename.
		/// </param>
		/// <param name="extension">
		/// The pattern to use for the output filename extension.
		/// </param>
		/// <returns>
		/// A string containing G-code instructions for the workpiece and its
		/// operations, if present. Otherwise, an empty string.
		/// </returns>
		/// <remarks>
		/// <para>
		/// The names of each file are available in the text's regex field
		/// \(Filename:(?&lt;filename&gt;[^\)]+)\) and will have the pattern
		/// {filenameBase}-{Index}of{Count}-{ToolName}{Extension}
		/// where Index is the index of the file within the set, Count is the
		/// count of files in the set, ToolName is the name of the selected
		/// tool for the series of actions, and Extension is .gcode if not
		/// supplied by the caller.
		/// </para>
		/// </remarks>
		public static List<string> RenderGCode(string filenameBase = "",
			string extension = "")
		{
			bool bFeed = false;
			bool bRetracted = false;
			StringBuilder builder = new StringBuilder();
			int count = 0;
			float feedRate = 100f;
			int index = 0;
			TrackSegmentItem lastSegment = null;
			int layerIndex = 0;
			string localExtension = "";
			string localFilenameBase = "";
			FVector2 location = null;
			float min = 0f;
			MaterialTypeItem material = null;
			string materialName = "Unknown";
			float number = 0f;
			List<string> results = new List<string>();
			//TrackSegmentItem segment = null;
			int selectedIndex = 0;
			string text = "";
			string toolName = "";
			string toolNameLast = "";
			TrackLayerCollection trackLayers = null;
			WorkpieceInfoItem workpiece = SessionWorkpieceInfo;

			if(workpiece?.Cuts.Count > 0)
			{
				//	There are cuts to convert.
				//	Configure the feed rate.
				if(workpiece.MaterialTypeName?.Length > 0)
				{
					//	A material type has been defined. Set the feed rate.
					material = ConfigProfile.MaterialTypes.FirstOrDefault(x =>
						x.MaterialTypeName.ToLower() ==
							workpiece.MaterialTypeName.ToLower());
					if(material != null)
					{
						materialName = (material.MaterialTypeName.Length > 0 ?
							material.MaterialTypeName : "(blank)");
						if(material.FeedRate?.Length > 0)
						{
							number = GetMillimeters(material.FeedRate);
							if(number > 0f)
							{
								feedRate = number;
								bFeed = true;
							}
						}
					}
				}
				if(!bFeed && ConfigProfile.MaterialTypes.Count > 0)
				{
					//	If material types are defined, but none have been selected,
					//	we want the slowest one for safety.
					min = float.MaxValue;
					index = 0;
					foreach(MaterialTypeItem typeItem in ConfigProfile.MaterialTypes)
					{
						if(typeItem.FeedRate?.Length > 0)
						{
							number = GetMillimeters(typeItem.FeedRate);
							if(number > 0f && number < min)
							{
								min = number;
								selectedIndex = index;
							}
						}
						index++;
					}
					if(min > 0f && min < float.MaxValue)
					{
						feedRate = min;
						bFeed = true;
					}
				}
				//	... otherwise, use the accepted minimum feed rate.

				Clear(builder);
				location = new FVector2();
				trackLayers = new TrackLayerCollection(workpiece.Cuts);
				foreach(TrackLayerItem layerItem in trackLayers)
				{
					//	Each Layer.
					toolName =
						(layerItem.Tool?.ToolName.Length > 0 ?
						layerItem.Tool.ToolName : "");
					if(toolName != toolNameLast)
					{
						//	Each tool.
						//	End the previous tool.
						if(builder.Length > 0)
						{
							if(!bRetracted)
							{
								builder.AppendLine(
									TransitToPositionZAbs(TransitZEnum.FullyRetracted));
								bRetracted = true;
							}
							results.Add(builder.ToString());
							Clear(builder);
						}
						//	Initialize the next tool.
						builder.Append("(Filename:{FilenameBase}-");
						builder.Append("{FileIndex}of{FileCount}-");
						builder.Append(GetFilenameFriendly(toolName));
						builder.AppendLine("{Extension})");
						builder.AppendLine("(Generated by Dan's ShopTools Desktop)");
						builder.AppendLine("(All units mm.)");
						builder.AppendLine("(All measurements absolute by default.)");
						builder.AppendLine("G21;");
						builder.AppendLine("G90;");
						builder.AppendLine(
							TransitToPositionZAbs(TransitZEnum.FullyRetracted));
						bRetracted = true;
						builder.AppendLine($"(Material:{materialName})");
						builder.Append("(Surface location:");
						builder.AppendLine(
							$"{GetPositionZAbs(TransitZEnum.TopOfMaterial)})");
						//builder.Append("M0 ");
						builder.AppendLine($"(Please attach tool: {toolName})");
					}
					//if(layerIndex > 0 && layerItem.Segments.Count > 0)
					//{
					//	//	On subsequent layers, plunge directly to the current relative
					//	//	depth.
					//	segment = layerItem.Segments[0];
					//	if(segment.SegmentType == TrackSegmentType.Plot)
					//	{
					//		if(bRetracted)
					//		{
					//			builder.AppendLine(
					//				TransitToPositionZAbs(TransitZEnum.TopOfMaterial));
					//			bRetracted = false;
					//		}
					//		builder.AppendLine(
					//			PlungeZAbs(
					//				GetPositionZAbs(
					//					TransitZEnum.TopOfMaterial,
					//					segment.Depth),
					//				feedRate));
					//	}
					//}
					foreach(TrackSegmentItem segmentItem in layerItem.Segments)
					{
						//	Process each segment.
						switch(segmentItem.SegmentType)
						{
							case TrackSegmentType.Plot:
								if(bRetracted)
								{
									//	Lower the boom.
									builder.AppendLine(
										TransitToPositionZAbs(TransitZEnum.TopOfMaterial));
								}
								if(bRetracted ||
									(lastSegment != null &&
										lastSegment.Depth != segmentItem.Depth))
								{
									//	Dig in.
									builder.AppendLine(
									PlungeZAbs(
										GetPositionZAbs(
											TransitZEnum.TopOfMaterial,
											segmentItem.Depth),
										feedRate));
								}
								builder.AppendLine(
									PlotToPositionXYAbs(segmentItem.EndOffset,
										feedRate));
								FVector2.TransferValues(segmentItem.EndOffset, location);
								bRetracted = false;
								break;
							case TrackSegmentType.Plunge:
								if(!FVector2.Equals(segmentItem.StartOffset, location))
								{
									builder.AppendLine(
										TransitToPositionXYAbs(segmentItem.StartOffset));
									FVector2.TransferValues(segmentItem.StartOffset, location);
								}
								builder.AppendLine(
									TransitToPositionZAbs(TransitZEnum.TopOfMaterial)
									);
								builder.AppendLine(
									PlungeZAbs(
										GetPositionZAbs(
											TransitZEnum.TopOfMaterial,
											segmentItem.Depth),
										feedRate));
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
								FVector2.TransferValues(segmentItem.EndOffset, location);
								break;
						}
						lastSegment = segmentItem;
					}
					toolNameLast = toolName;
					layerIndex++;

				}
				if(builder.Length > 0)
				{
					//	Flush the last tool.
					if(!bRetracted)
					{
						builder.AppendLine(
							TransitToPositionZAbs(TransitZEnum.FullyRetracted));
						bRetracted = true;
					}
					if(builder.Length > 0)
					{
						results.Add(builder.ToString());
						Clear(builder);
					}
				}
				if(results.Count > 0)
				{
					if(!(extension?.Length > 0))
					{
						localExtension = ".gcode";
					}
					else
					{
						localExtension = extension;
						if(!localExtension.StartsWith('.'))
						{
							localExtension = $".{localExtension}";
						}
					}
					if(!(filenameBase?.Length > 0))
					{
						localFilenameBase = "ShopTools-" +
							DateTime.Now.ToString("yyyyMMddhhmmss");
					}
					else
					{
						localFilenameBase = filenameBase;
					}
					count = results.Count;
					for(index = 1; index <= count; index ++)
					{
						text = results[index - 1];
						text = text.Replace("{FilenameBase}", localFilenameBase).
							Replace("{FileIndex}", index.ToString().PadLeft(2, '0')).
							Replace("{FileCount}", count.ToString().PadLeft(2, '0')).
							Replace("{Extension}", localExtension);
						results[index - 1] = text;
					}
				}
			}
			return results;
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
		public static string TransitToPositionXYAbs(FVector2 offset)
		{
			StringBuilder builder = new StringBuilder();

			if(offset != null)
			{
				builder.Append($"G00 X{offset.X:0.###} Y{offset.Y:0.###};");
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
			return $"G00 Z{GetPositionZAbs(zPositionType):0.###}";
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*


}
