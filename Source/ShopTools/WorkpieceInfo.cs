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
	public class WorkpieceInfoCollection :
		ChangeObjectCollection<WorkpieceInfoItem>
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
	public class WorkpieceInfoItem : ChangeObjectItem
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* mArea_ValueChanged																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A value has changed on the Area property.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Float event arguments.
		/// </param>
		private void mArea_ValueChanged(object sender, FloatEventArgs e)
		{
			OnPropertyChanged("Area");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mRouterLocation_CoordinateChanged																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A coordinate has changed on the router location.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Floating point event arguments.
		/// </param>
		private void mRouterLocation_CoordinateChanged(object sender,
			FloatPointEventArgs e)
		{
			OnPropertyChanged("RouterLocation");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* mWorkspaceArea_ValueChanged																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// A value has changed on the WorkspaceArea property.
		/// </summary>
		/// <param name="sender">
		/// The object raising this event.
		/// </param>
		/// <param name="e">
		/// Float event arguments.
		/// </param>
		private void mWorkspaceArea_ValueChanged(object sender, FloatEventArgs e)
		{
			OnPropertyChanged("WorkspaceArea");
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
		/// Create a new instance of the WorkpieceInfoItem Item.
		/// </summary>
		public WorkpieceInfoItem()
		{
			mArea = new FArea();
			mArea.BottomChanged += mArea_ValueChanged;
			mArea.HeightChanged += mArea_ValueChanged;
			mArea.LeftChanged += mArea_ValueChanged;
			mArea.RightChanged += mArea_ValueChanged;
			mArea.TopChanged += mArea_ValueChanged;
			mArea.WidthChanged += mArea_ValueChanged;
			mCuts = new CutProfileCollection()
			{
				PropertyName = "Cuts"
			};
			mCuts.CollectionChanged += OnCollectionChanged;
			mRouterLocation = new FPoint();
			mRouterLocation.CoordinateChanged += mRouterLocation_CoordinateChanged;
			mWorkspaceArea = new FArea();
			mWorkspaceArea.BottomChanged += mWorkspaceArea_ValueChanged;
			mWorkspaceArea.BottomChanged += mArea_ValueChanged;
			mWorkspaceArea.HeightChanged += mArea_ValueChanged;
			mWorkspaceArea.LeftChanged += mArea_ValueChanged;
			mWorkspaceArea.RightChanged += mArea_ValueChanged;
			mWorkspaceArea.TopChanged += mArea_ValueChanged;
			mWorkspaceArea.WidthChanged += mArea_ValueChanged;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AltDepth																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="AltDepth">AltDepth</see>.
		/// </summary>
		private string mAltDepth = "";
		/// <summary>
		/// Get/Set the alternate user display value for the depth specified for
		/// the workpiece.
		/// </summary>
		[JsonIgnore]
		public string AltDepth
		{
			get { return mAltDepth; }
			set
			{
				bool bChanged = (mAltDepth != value);

				mAltDepth = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AltLength																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="AltLength">AltDepth</see>.
		/// </summary>
		private string mAltLength = "";
		/// <summary>
		/// Get/Set the alternate display value for the user length specified for
		/// the workpiece.
		/// </summary>
		[JsonIgnore]
		public string AltLength
		{
			get { return mAltLength; }
			set
			{
				bool bChanged = (mAltLength != value);

				mAltLength = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AltOffsetX																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="AltOffsetX">AltOffsetX</see>.
		/// </summary>
		private string mAltOffsetX = "";
		/// <summary>
		/// Get/Set the alternate display value of the user-specified X offset of
		/// the workpiece.
		/// </summary>
		[JsonIgnore]
		public string AltOffsetX
		{
			get { return mAltOffsetX; }
			set
			{
				bool bChanged = (mAltOffsetX != value);

				mAltOffsetX = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AltOffsetY																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="AltOffsetY">AltOffsetY</see>.
		/// </summary>
		private string mAltOffsetY = "";
		/// <summary>
		/// Get/Set the alternate display value for the user-specified Y workpiece
		/// offset.
		/// </summary>
		[JsonIgnore]
		public string AltOffsetY
		{
			get { return mAltOffsetY; }
			set
			{
				bool bChanged = (mAltOffsetY != value);

				mAltOffsetY = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AltRouterLocationX																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="AltRouterLocationX">AltRouterLocationX</see>.
		/// </summary>
		private string mAltRouterLocationX = "";
		/// <summary>
		/// Get/Set the alternate display value of the user-specified absolute
		/// starting X router location.
		/// </summary>
		[JsonIgnore]
		public string AltRouterLocationX
		{
			get { return mAltRouterLocationX; }
			set
			{
				bool bChanged = (mAltRouterLocationX != value);

				mAltRouterLocationX = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AltRouterLocationY																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="AltRouterLocationY">AltRouterLocationY</see>.
		/// </summary>
		private string mAltRouterLocationY = "";
		/// <summary>
		/// Get/Set the alternate display value of the user-specified absolute
		/// starting Y router location.
		/// </summary>
		[JsonIgnore]
		public string AltRouterLocationY
		{
			get { return mAltRouterLocationY; }
			set
			{
				bool bChanged = (mAltRouterLocationY != value);

				mAltRouterLocationY = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AltWidth																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="AltWidth">AltWidth</see>.
		/// </summary>
		private string mAltWidth = "";
		/// <summary>
		/// Get/Set the alternate display value of the user width specified for
		/// the workpiece.
		/// </summary>
		[JsonIgnore]
		public string AltWidth
		{
			get { return mAltWidth; }
			set
			{
				bool bChanged = (mAltWidth != value);

				mAltWidth = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Area																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Area">Area</see>.
		/// </summary>
		private FArea mArea = null;
		/// <summary>
		/// Get/Set a reference to the actual area occupied by the workpiece.
		/// </summary>
		[JsonIgnore]
		public FArea Area
		{
			get { return mArea; }
			set
			{
				bool bChanged = (mArea != value);

				//	Register events.
				if(bChanged)
				{
					if(mArea != null)
					{
						mArea.BottomChanged -= mArea_ValueChanged;
						mArea.HeightChanged -= mArea_ValueChanged;
						mArea.LeftChanged -= mArea_ValueChanged;
						mArea.RightChanged -= mArea_ValueChanged;
						mArea.TopChanged -= mArea_ValueChanged;
						mArea.WidthChanged -= mArea_ValueChanged;
					}
					if(value != null)
					{
						value.BottomChanged += mArea_ValueChanged;
						value.HeightChanged += mArea_ValueChanged;
						value.LeftChanged += mArea_ValueChanged;
						value.RightChanged += mArea_ValueChanged;
						value.TopChanged += mArea_ValueChanged;
						value.WidthChanged += mArea_ValueChanged;
					}
				}
				mArea = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
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
					mAltDepth = item.mAltDepth,
					mAltLength = item.mAltLength,
					mAltOffsetX = item.mAltOffsetX,
					mAltOffsetY = item.mAltOffsetY,
					mAltRouterLocationX = item.mAltRouterLocationX,
					mAltRouterLocationY = item.mAltRouterLocationY,
					mAltWidth = item.mAltWidth,
					mArea = FArea.Clone(item.mArea),
					mCuts = CutProfileCollection.Clone(item.mCuts),
					mMaterialTypeName = item.mMaterialTypeName,
					mRouterLocation = FPoint.Clone(item.mRouterLocation),
					mThickness = item.mThickness,
					mUserDepth = item.mUserDepth,
					mUserLength = item.mUserLength,
					mUserOffsetX = item.mUserOffsetX,
					mUserOffsetXOrigin = item.mUserOffsetXOrigin,
					mUserOffsetY = item.mUserOffsetY,
					mUserOffsetYOrigin = item.mUserOffsetYOrigin,
					mUserRouterLocationX = item.mUserRouterLocationX,
					mUserRouterLocationY = item.mUserRouterLocationY,
					mUserWidth = item.mUserWidth,
					mWorkspaceArea = FArea.Clone(item.mWorkspaceArea)
				};
			}
			if(result == null)
			{
				result = new WorkpieceInfoItem();
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
				measurement = GetMeasurementString(workpiece.mUserLength);
				workpiece.mAltLength =
					GetAltValue(measurement, workpiece.mUserLength);
				workpieceWidth = GetMillimeters(workpiece.mUserLength);
				//	Height.
				measurement = GetMeasurementString(workpiece.mUserWidth);
				workpiece.mAltWidth =
					GetAltValue(measurement, workpiece.mUserWidth);
				workpieceHeight = GetMillimeters(workpiece.mUserWidth);
				//	Depth.
				measurement = GetMeasurementString(workpiece.mUserDepth);
				workpiece.mAltDepth =
					GetAltValue(measurement, workpiece.mUserDepth);
				workpiece.mThickness = GetMillimeters(workpiece.mUserDepth);
				//	X.
				measurement = GetMeasurementString(workpiece.mUserOffsetX);
				workpiece.mAltOffsetX =
					GetAltValue(measurement, workpiece.mUserOffsetX);
				workpieceX = GetMillimeters(workpiece.mUserOffsetX);
				workpiece.mArea.Left =
					TranslateOffset(workspaceArea, workpieceWidth, workpieceX,
					workpiece.mUserOffsetXOrigin);
				workpiece.mArea.Right = workpiece.mArea.Left + workpieceWidth;
				//	Y.
				measurement = GetMeasurementString(workpiece.mUserOffsetY);
				workpiece.mAltOffsetY =
					GetAltValue(measurement, workpiece.mUserOffsetY);
				workpieceY = GetMillimeters(workpiece.mUserOffsetY);
				workpiece.mArea.Top =
					TranslateOffset(workspaceArea, workpieceHeight, workpieceY,
					workpiece.mUserOffsetYOrigin);
				workpiece.mArea.Bottom = workpiece.mArea.Top + workpieceHeight;
				//	Router Location X.
				measurement = GetMeasurementString(workpiece.mUserRouterLocationX);
				workpiece.mAltRouterLocationX =
					GetAltValue(measurement, workpiece.mUserRouterLocationX);
				workpiece.mRouterLocation.X =
					GetMillimeters(workpiece.mUserRouterLocationX);
				//	Router Location Y.
				measurement = GetMeasurementString(workpiece.mUserRouterLocationY);
				workpiece.mAltRouterLocationY =
					GetAltValue(measurement, workpiece.mUserRouterLocationY);
				workpiece.mRouterLocation.Y =
					GetMillimeters(workpiece.mUserRouterLocationY);
				//	Material Type Name.
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Cuts																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Cuts">Cuts</see>.
		/// </summary>
		private CutProfileCollection mCuts = null;
		/// <summary>
		/// Get a reference to the list of cuts on this job.
		/// </summary>
		[JsonProperty(Order = 10)]
		public CutProfileCollection Cuts
		{
			get { return mCuts; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MaterialTypeName																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="MaterialTypeName">MaterialTypeName</see>.
		/// </summary>
		private string mMaterialTypeName = "";
		/// <summary>
		/// Get/Set the material selected for this cut.
		/// </summary>
		[JsonProperty(Order = 9)]
		public string MaterialTypeName
		{
			get { return mMaterialTypeName; }
			set
			{
				bool bChanged = (mMaterialTypeName != value);

				mMaterialTypeName = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	RouterLocation																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="RouterLocation">RouterLocation</see>.
		/// </summary>
		private FPoint mRouterLocation = new FPoint();
		/// <summary>
		/// Get/Set the current calculated location of the router.
		/// </summary>
		/// <seealso href="https://danielanywhere.github.io/Geometry/html/T_Geometry_FPoint.htm">
		/// FPoint Documentation</seealso>
		[JsonIgnore]
		public FPoint RouterLocation
		{
			get { return mRouterLocation; }
			set
			{
				bool bChanged = (mRouterLocation != value);

				//	Register events.
				if(bChanged)
				{
					if(mRouterLocation != null)
					{
						mRouterLocation.CoordinateChanged -=
							mRouterLocation_CoordinateChanged;
					}
					if(value != null)
					{
						value.CoordinateChanged += mRouterLocation_CoordinateChanged;
					}
				}
				mRouterLocation = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Thickness																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Thickness">Thickness</see>.
		/// </summary>
		private float mThickness = 0f;
		/// <summary>
		/// Get/Set the thickness of the workpiece, in system units.
		/// </summary>
		[JsonIgnore]
		public float Thickness
		{
			get { return mThickness; }
			set
			{
				bool bChanged = (mThickness != value);

				mThickness = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserDepth																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="UserDepth">UserDepth</see>.
		/// </summary>
		private string mUserDepth = "";
		/// <summary>
		/// Get/Set the user depth specified for the workpiece.
		/// </summary>
		[JsonProperty(Order = 2)]
		public string UserDepth
		{
			get { return mUserDepth; }
			set
			{
				bool bChanged = (mUserDepth != value);

				mUserDepth = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserLength																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="UserLength">UserLength</see>.
		/// </summary>
		private string mUserLength = "";
		/// <summary>
		/// Get/Set the user length specified for the workpiece.
		/// </summary>
		[JsonProperty(Order = 0)]		
		public string UserLength
		{
			get { return mUserLength; }
			set
			{
				bool bChanged = (mUserLength != value);

				mUserLength = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserOffsetX																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="UserOffsetX">UserOffsetX</see>.
		/// </summary>
		private string mUserOffsetX = "";
		/// <summary>
		/// Get/Set the user-specified X offset of the workpiece.
		/// </summary>
		[JsonProperty(Order = 3)]
		public string UserOffsetX
		{
			get { return mUserOffsetX; }
			set
			{
				bool bChanged = (mUserOffsetX != value);

				mUserOffsetX = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserOffsetXOrigin																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="UserOffsetXOrigin">UserOffsetXOrigin</see>.
		/// </summary>
		private OffsetLeftRightEnum mUserOffsetXOrigin = OffsetLeftRightEnum.None;
		/// <summary>
		/// Get/Set the user-specified X offset of the workpiece from the canvas.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(Order = 4)]		
		public OffsetLeftRightEnum UserOffsetXOrigin
		{
			get { return mUserOffsetXOrigin; }
			set
			{
				bool bChanged = (mUserOffsetXOrigin != value);

				mUserOffsetXOrigin = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserOffsetY																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="UserOffsetY">UserOffsetY</see>.
		/// </summary>
		private string mUserOffsetY = "";
		/// <summary>
		/// Get/Set the user-specified Y workpiece offset.
		/// </summary>
		[JsonProperty(Order = 5)]
		public string UserOffsetY
		{
			get { return mUserOffsetY; }
			set
			{
				bool bChanged = (mUserOffsetY != value);

				mUserOffsetY = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserOffsetYOrigin																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="UserOffsetYOrigin">UserOffsetYOrigin</see>.
		/// </summary>
		private OffsetTopBottomEnum mUserOffsetYOrigin = OffsetTopBottomEnum.None;
		/// <summary>
		/// Get/Set the user-specified Y workpiece offset.
		/// </summary>
		[JsonConverter(typeof(StringEnumConverter))]
		[JsonProperty(Order = 6)]
		public OffsetTopBottomEnum UserOffsetYOrigin
		{
			get { return mUserOffsetYOrigin; }
			set
			{
				bool bChanged = (mUserOffsetYOrigin != value);

				mUserOffsetYOrigin = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserRouterLocationX																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="UserRouterLocationX">UserRouterLocationX</see>.
		/// </summary>
		private string mUserRouterLocationX = "";
		/// <summary>
		/// Get/Set the user-specified absolute starting X router location.
		/// </summary>
		[JsonProperty(Order = 7)]
		public string UserRouterLocationX
		{
			get { return mUserRouterLocationX; }
			set
			{
				bool bChanged = (mUserRouterLocationX != value);

				mUserRouterLocationX = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserRouterLocationY																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="UserRouterLocationY">UserRouterLocationY</see>.
		/// </summary>
		private string mUserRouterLocationY = "";
		/// <summary>
		/// Get/Set the user-specified absolute starting Y router location.
		/// </summary>
		[JsonProperty(Order = 8)]
		public string UserRouterLocationY
		{
			get { return mUserRouterLocationY; }
			set
			{
				bool bChanged = (mUserRouterLocationY != value);

				mUserRouterLocationY = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserWidth																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="UserWidth">UserWidth</see>.
		/// </summary>
		private string mUserWidth = "";
		/// <summary>
		/// Get/Set the user width specified for the workpiece.
		/// </summary>
		[JsonProperty(Order = 1)]
		public string UserWidth
		{
			get { return mUserWidth; }
			set
			{
				bool bChanged = (mUserWidth != value);

				mUserWidth = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WorkspaceArea																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="WorkspaceArea">WorkspaceArea</see>.
		/// </summary>
		private FArea mWorkspaceArea = null;
		/// <summary>
		/// Get/Set a reference to the current workspace area or canvas for this
		/// session.
		/// </summary>
		[JsonIgnore]
		public FArea WorkspaceArea
		{
			get { return mWorkspaceArea; }
			set
			{
				bool bChanged = (mWorkspaceArea != value);

				//	Register events.
				if(bChanged)
				{
					if(mWorkspaceArea != null)
					{
						mWorkspaceArea.BottomChanged -= mWorkspaceArea_ValueChanged;
						mWorkspaceArea.BottomChanged -= mArea_ValueChanged;
						mWorkspaceArea.HeightChanged -= mArea_ValueChanged;
						mWorkspaceArea.LeftChanged -= mArea_ValueChanged;
						mWorkspaceArea.RightChanged -= mArea_ValueChanged;
						mWorkspaceArea.TopChanged -= mArea_ValueChanged;
						mWorkspaceArea.WidthChanged -= mArea_ValueChanged;
					}
					if(value != null)
					{
						value.BottomChanged += mWorkspaceArea_ValueChanged;
						value.BottomChanged += mArea_ValueChanged;
						value.HeightChanged += mArea_ValueChanged;
						value.LeftChanged += mArea_ValueChanged;
						value.RightChanged += mArea_ValueChanged;
						value.TopChanged += mArea_ValueChanged;
						value.WidthChanged += mArea_ValueChanged;
					}
				}
				mWorkspaceArea = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*


}
