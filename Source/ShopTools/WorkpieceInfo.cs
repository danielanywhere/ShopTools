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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Geometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	WorkpieceInfoCollection																									*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of WorkpieceInfoItem Items.
	/// </summary>
	public class WorkpieceInfoCollection : List<WorkpieceInfoItem>
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
		/// Return a deep clone of the provided collection of workpiece
		/// information items.
		/// </summary>
		/// <param name="items">
		/// Reference to the collection of items to clone.
		/// </param>
		/// <returns>
		/// Reference to a newly cloned collection of workpiece information items.
		/// </returns>
		public static WorkpieceInfoCollection Clone(WorkpieceInfoCollection items)
		{
			WorkpieceInfoCollection result = new WorkpieceInfoCollection();

			if(items?.Count > 0)
			{
				foreach(WorkpieceInfoItem infoItem in items)
				{
					result.Add(WorkpieceInfoItem.Clone(infoItem));
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	WorkpieceInfoItem																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a single workpiece, including the starting router
	/// position and any cuts.
	/// </summary>
	public class WorkpieceInfoItem
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
		//*	AltDepth																															*
		//*-----------------------------------------------------------------------*
		private string mAltDepth = "";
		/// <summary>
		/// Get/Set the alternate user display value for the depth specified for
		/// the workpiece.
		/// </summary>
		[JsonIgnore]
		public string AltDepth
		{
			get { return mAltDepth; }
			set { mAltDepth = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AltLength																															*
		//*-----------------------------------------------------------------------*
		private string mAltLength = "";
		/// <summary>
		/// Get/Set the alternate display value for the user length specified for
		/// the workpiece.
		/// </summary>
		[JsonIgnore]
		public string AltLength
		{
			get { return mAltLength; }
			set { mAltLength = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AltOffsetX																														*
		//*-----------------------------------------------------------------------*
		private string mAltOffsetX = "";
		/// <summary>
		/// Get/Set the alternate display value of the user-specified X offset of
		/// the workpiece.
		/// </summary>
		[JsonIgnore]
		public string AltOffsetX
		{
			get { return mAltOffsetX; }
			set { mAltOffsetX = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AltOffsetY																														*
		//*-----------------------------------------------------------------------*
		private string mAltOffsetY = "";
		/// <summary>
		/// Get/Set the alternate display value for the user-specified Y workpiece
		/// offset.
		/// </summary>
		[JsonIgnore]
		public string AltOffsetY
		{
			get { return mAltOffsetY; }
			set { mAltOffsetY = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AltRouterLocationX																										*
		//*-----------------------------------------------------------------------*
		private string mAltRouterLocationX = "";
		/// <summary>
		/// Get/Set the alternate display value of the user-specified absolute
		/// starting X router location.
		/// </summary>
		[JsonIgnore]
		public string AltRouterLocationX
		{
			get { return mAltRouterLocationX; }
			set { mAltRouterLocationX = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AltRouterLocationY																										*
		//*-----------------------------------------------------------------------*
		private string mAltRouterLocationY = "";
		/// <summary>
		/// Get/Set the alternate display value of the user-specified absolute
		/// starting Y router location.
		/// </summary>
		[JsonIgnore]
		public string AltRouterLocationY
		{
			get { return mAltRouterLocationY; }
			set { mAltRouterLocationY = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AltWidth																															*
		//*-----------------------------------------------------------------------*
		private string mAltWidth = "";
		/// <summary>
		/// Get/Set the alternate display value of the user width specified for
		/// the workpiece.
		/// </summary>
		[JsonIgnore]
		public string AltWidth
		{
			get { return mAltWidth; }
			set { mAltWidth = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Area																																	*
		//*-----------------------------------------------------------------------*
		private FArea mArea = new FArea();
		/// <summary>
		/// Get/Set a reference to the actual area occupied by the workpiece.
		/// </summary>
		[JsonIgnore]
		public FArea Area
		{
			get { return mArea; }
			set { mArea = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clone																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a deep clone of the provided workpieceinformation item.
		/// </summary>
		/// <param name="item">
		/// Reference to the item to clone.
		/// </param>
		/// <returns>
		/// Reference to a newly cloned workpiece information item.
		/// </returns>
		public static WorkpieceInfoItem Clone(WorkpieceInfoItem item)
		{
			WorkpieceInfoItem result = null;

			if(item != null)
			{
				result = new WorkpieceInfoItem()
				{
					mArea = FArea.Clone(item.mArea),
					mCuts = CutProfileCollection.Clone(item.mCuts),
					mRouterLocation = FPoint.Clone(item.mRouterLocation),
					mThickness = item.mThickness,
					mWorkspaceArea = FArea.Clone(item.mWorkspaceArea)
				};
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ConfigureFromUserValues																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Configure the workpiece members from user values.
		/// </summary>
		/// <param name="workpiece">
		/// Reference to the workpiece whose area and router settings will be
		/// configured.
		/// </param>
		public static void ConfigureFromUserValues(WorkpieceInfoItem workpiece)
		{
			string measurement = "";
			float workpieceHeight = 0f;
			float workpieceWidth = 0f;
			float workpieceX = 0f;
			float workpieceY = 0f;
			FArea workspaceArea = GetWorkspaceArea();

			if(workpiece != null)
			{
				//	Full Workspace.
				workpiece.mWorkspaceArea = workspaceArea;
				//	Length.
				measurement = GetMeasurementString(workpiece.mUserLength,
					BaseUnit(ConfigProfile.DisplayUnits));
				workpiece.mAltLength =
					GetAltValue(measurement, workpiece.mUserLength);
				workpieceWidth = GetMillimeters(workpiece.mUserLength);
				//	Height.
				measurement = GetMeasurementString(workpiece.mUserWidth,
					BaseUnit(ConfigProfile.DisplayUnits));
				workpiece.mAltWidth =
					GetAltValue(measurement, workpiece.mUserWidth);
				workpieceHeight = GetMillimeters(workpiece.mUserWidth);
				//	Depth.
				measurement = GetMeasurementString(workpiece.mUserDepth,
					BaseUnit(ConfigProfile.DisplayUnits));
				workpiece.mAltDepth =
					GetAltValue(measurement, workpiece.mUserDepth);
				workpiece.mThickness = GetMillimeters(workpiece.mUserDepth);
				//	X.
				measurement = GetMeasurementString(workpiece.mUserOffsetX,
					BaseUnit(ConfigProfile.DisplayUnits));
				workpiece.mAltOffsetX =
					GetAltValue(measurement, workpiece.mUserOffsetX);
				workpieceX = GetMillimeters(workpiece.mUserOffsetX);
				workpiece.mArea.Left =
					TranslateOffset(workspaceArea, workpieceWidth, workpieceX,
					workpiece.mUserOffsetXOrigin);
				workpiece.mArea.Right = workpiece.mArea.Left + workpieceWidth;
				//	Y.
				measurement = GetMeasurementString(workpiece.mUserOffsetY,
					BaseUnit(ConfigProfile.DisplayUnits));
				workpiece.mAltOffsetY =
					GetAltValue(measurement, workpiece.mUserOffsetY);
				workpieceY = GetMillimeters(workpiece.mUserOffsetY);
				workpiece.mArea.Top =
					TranslateOffset(workspaceArea, workpieceHeight, workpieceY,
					workpiece.mUserOffsetYOrigin);
				workpiece.mArea.Bottom = workpiece.mArea.Top + workpieceHeight;
				//	Router Location X.
				measurement = GetMeasurementString(workpiece.mUserRouterLocationX,
					BaseUnit(ConfigProfile.DisplayUnits));
				workpiece.mAltRouterLocationX =
					GetAltValue(measurement, workpiece.mUserRouterLocationX);
				workpiece.mRouterLocation.X =
					GetMillimeters(workpiece.mUserRouterLocationX);
				//	Router Location Y.
				measurement = GetMeasurementString(workpiece.mUserRouterLocationY,
					BaseUnit(ConfigProfile.DisplayUnits));
				workpiece.mAltRouterLocationY =
					GetAltValue(measurement, workpiece.mUserRouterLocationY);
				workpiece.mRouterLocation.Y =
					GetMillimeters(workpiece.mUserRouterLocationY);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Cuts																																	*
		//*-----------------------------------------------------------------------*
		private CutProfileCollection mCuts = new CutProfileCollection();
		/// <summary>
		/// Get a reference to the list of cuts on this job.
		/// </summary>
		[JsonProperty(Order = 19)]
		public CutProfileCollection Cuts
		{
			get { return mCuts; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	RouterLocation																												*
		//*-----------------------------------------------------------------------*
		private FPoint mRouterLocation = new FPoint();
		/// <summary>
		/// Get/Set the current calculated location of the router.
		/// </summary>
		[JsonIgnore]
		public FPoint RouterLocation
		{
			get { return mRouterLocation; }
			set { mRouterLocation = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Thickness																															*
		//*-----------------------------------------------------------------------*
		private float mThickness = 0f;
		/// <summary>
		/// Get/Set the thickness of the workpiece, in system units.
		/// </summary>
		[JsonIgnore]
		public float Thickness
		{
			get { return mThickness; }
			set { mThickness = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserDepth																															*
		//*-----------------------------------------------------------------------*
		private string mUserDepth = "";
		/// <summary>
		/// Get/Set the user depth specified for the workpiece.
		/// </summary>
		[JsonProperty(Order = 12)]
		public string UserDepth
		{
			get { return mUserDepth; }
			set { mUserDepth = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserLength																														*
		//*-----------------------------------------------------------------------*
		private string mUserLength = "";
		/// <summary>
		/// Get/Set the user length specified for the workpiece.
		/// </summary>
		[JsonProperty(Order = 10)]		
		public string UserLength
		{
			get { return mUserLength; }
			set { mUserLength = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserOffsetX																														*
		//*-----------------------------------------------------------------------*
		private string mUserOffsetX = "";
		/// <summary>
		/// Get/Set the user-specified X offset of the workpiece.
		/// </summary>
		[JsonProperty(Order = 13)]
		public string UserOffsetX
		{
			get { return mUserOffsetX; }
			set { mUserOffsetX = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserOffsetXOrigin																											*
		//*-----------------------------------------------------------------------*
		private OffsetLeftRightEnum mUserOffsetXOrigin = OffsetLeftRightEnum.None;
		/// <summary>
		/// Get/Set the user-specified X offset of the workpiece from the canvas.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(Order = 14)]		
		public OffsetLeftRightEnum UserOffsetXOrigin
		{
			get { return mUserOffsetXOrigin; }
			set { mUserOffsetXOrigin = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserOffsetY																														*
		//*-----------------------------------------------------------------------*
		private string mUserOffsetY = "";
		/// <summary>
		/// Get/Set the user-specified Y workpiece offset.
		/// </summary>
		[JsonProperty(Order = 15)]
		public string UserOffsetY
		{
			get { return mUserOffsetY; }
			set { mUserOffsetY = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserOffsetYOrigin																											*
		//*-----------------------------------------------------------------------*
		private OffsetTopBottomEnum mUserOffsetYOrigin = OffsetTopBottomEnum.None;
		/// <summary>
		/// Get/Set the user-specified Y workpiece offset.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(Order = 16)]
		public OffsetTopBottomEnum UserOffsetYOrigin
		{
			get { return mUserOffsetYOrigin; }
			set { mUserOffsetYOrigin = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserRouterLocationX																										*
		//*-----------------------------------------------------------------------*
		private string mUserRouterLocationX = "";
		/// <summary>
		/// Get/Set the user-specified absolute starting X router location.
		/// </summary>
		[JsonProperty(Order = 17)]
		public string UserRouterLocationX
		{
			get { return mUserRouterLocationX; }
			set { mUserRouterLocationX = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserRouterLocationY																										*
		//*-----------------------------------------------------------------------*
		private string mUserRouterLocationY = "";
		/// <summary>
		/// Get/Set the user-specified absolute starting Y router location.
		/// </summary>
		[JsonProperty(Order = 18)]
		public string UserRouterLocationY
		{
			get { return mUserRouterLocationY; }
			set { mUserRouterLocationY = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserWidth																															*
		//*-----------------------------------------------------------------------*
		private string mUserWidth = "";
		/// <summary>
		/// Get/Set the user width specified for the workpiece.
		/// </summary>
		[JsonProperty(Order = 11)]
		public string UserWidth
		{
			get { return mUserWidth; }
			set { mUserWidth = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WorkspaceArea																													*
		//*-----------------------------------------------------------------------*
		private FArea mWorkspaceArea = new FArea();
		/// <summary>
		/// Get/Set a reference to the current workspace area or canvas for this
		/// session.
		/// </summary>
		[JsonIgnore]
		public FArea WorkspaceArea
		{
			get { return mWorkspaceArea; }
			set { mWorkspaceArea = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*


}
