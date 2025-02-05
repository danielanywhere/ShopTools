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

using Newtonsoft.Json;

using Geometry;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	ShopToolsConfigCollection																								*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of ShopToolsConfigItem Items.
	/// </summary>
	public class ShopToolsConfigCollection : List<ShopToolsConfigItem>
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
	//*	ShopToolsConfigItem																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a configuration profile.
	/// </summary>
	public class ShopToolsConfigItem
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
		//*	AxisXIsOpenEnded																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set a value indicating whether access along the X axis is
		/// open-ended.
		/// </summary>
		[JsonIgnore]		
		public bool AxisXIsOpenEnded
		{
			get { return ToBool(mProperties["AxisXOpenEnded"].Value); }
			set { mProperties["AxisXOpenEnded"].Value = value.ToString(); }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	AxisYIsOpenEnded																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Get/Set a value indicating whether access along the Y axis is
		/// open-ended.
		/// </summary>
		[JsonIgnore]
		public bool AxisYIsOpenEnded
		{
			get { return ToBool(mProperties["AxisYOpenEnded"].Value); }
			set { mProperties["AxisYOpenEnded"].Value = value.ToString(); }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clone																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a memberwise clone of the caller's configuration profile.
		/// </summary>
		/// <param name="item">
		/// Reference to the item to be cloned.
		/// </param>
		/// <returns>
		/// Reference to the full memberwise clone of the user's instance, if
		/// legitimate. Otherwise, an empty configuration profile.
		/// </returns>
		public static ShopToolsConfigItem Clone(ShopToolsConfigItem item)
		{
			ShopToolsConfigItem result = new ShopToolsConfigItem();

			if(item != null)
			{
				result.mPatternTemplates =
					PatternTemplateCollection.Clone(item.mPatternTemplates);
				result.mProperties =
					PropertyCollection.Clone(item.mProperties);
				//result.mPropertyDefinitions =
				//	PropertyDefinitionCollection.Clone(item.mPropertyDefinitions);
				result.mUserTools =
					UserToolCollection.Clone(item.mUserTools);


				//	Unchanging members.
				//	Since the action properties and tool type definitions are
				//	static, we can just assign the references.
				result.mOperationActionProperties = item.mOperationActionProperties;
				result.mToolTypeDefinitions = item.mToolTypeDefinitions;

			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	DisplayUnits																													*
		//*-----------------------------------------------------------------------*
		//private string mDisplayUnits = "in";
		/// <summary>
		/// Get/Set the default display units for this profile.
		/// </summary>
		/// <remarks>
		/// Choices are UnitedStates and Metric. System units are always in
		/// millimeters.
		/// </remarks>
		[JsonIgnore]
		public DisplayUnitEnum DisplayUnits
		{
			get
			{
				DisplayUnitEnum result = DisplayUnitEnum.UnitedStates;
				string text = mProperties["DisplayUnits"].Value;
				DisplayUnitEnum unit = DisplayUnitEnum.UnitedStates;

				if(Enum.TryParse<DisplayUnitEnum>(text, true, out unit))
				{
					result = unit;
				}
				return result;
			}
			set
			{
				DisplayUnitEnum unit = value;

				if(unit == DisplayUnitEnum.None)
				{
					unit = DisplayUnitEnum.UnitedStates;
				}
				mProperties["DisplayUnits"].Value = unit.ToString();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Depth																																	*
		//*-----------------------------------------------------------------------*
		//private string mDepth = "";
		/// <summary>
		/// Get/Set the system depth (Z) of the table, as valid in the registered
		/// units.
		/// </summary>
		[JsonIgnore]
		public string Depth
		{
			get { return mProperties["Depth"].Value; }
			set { mProperties["Depth"].Value = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GeneralCuttingTool																										*
		//*-----------------------------------------------------------------------*
		//private string mGeneralCuttingTool = "";
		/// <summary>
		/// Get/Set the name of the cutting tool to be used during general cuts
		/// where no specific tool is specified.
		/// </summary>
		[JsonIgnore]
		public string GeneralCuttingTool
		{
			get { return mProperties["GeneralCuttingTool"].Value; }
			set { mProperties["GeneralCuttingTool"].Value = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	OperationActionProperties																							*
		//*-----------------------------------------------------------------------*
		private OperationActionPropertyCollection mOperationActionProperties =
			new OperationActionPropertyCollection();
		/// <summary>
		/// Get a reference to the collection of operation action property
		/// definitions for this session.
		/// </summary>
		public OperationActionPropertyCollection OperationActionProperties
		{
			get { return mOperationActionProperties; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PatternTemplates																											*
		//*-----------------------------------------------------------------------*
		private PatternTemplateCollection mPatternTemplates =
			new PatternTemplateCollection();
		/// <summary>
		/// Get a reference to the collection of pattern templates defined for
		/// this configuration.
		/// </summary>
		[JsonProperty(Order = 4)]
		public PatternTemplateCollection PatternTemplates
		{
			get { return mPatternTemplates; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Properties																														*
		//*-----------------------------------------------------------------------*
		private PropertyCollection mProperties = new PropertyCollection();
		/// <summary>
		/// Get a reference to the collection of settings properties on this
		/// configuration.
		/// </summary>
		[JsonProperty(Order = 0)]
		public PropertyCollection Properties
		{
			get { return mProperties; }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	PropertyDefinitions																										*
		////*-----------------------------------------------------------------------*
		//private PropertyDefinitionCollection mPropertyDefinitions =
		//	new PropertyDefinitionCollection();
		///// <summary>
		///// Get a reference to the collection of definitions for properties used in
		///// this configuration.
		///// </summary>
		//[JsonProperty(Order = 2)]
		//public PropertyDefinitionCollection PropertyDefinitions
		//{
		//	get { return mPropertyDefinitions; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToolTypeDefinitions																										*
		//*-----------------------------------------------------------------------*
		private ToolTypeDefinitionCollection mToolTypeDefinitions =
			new ToolTypeDefinitionCollection();
		/// <summary>
		/// Get a reference to the collection of tool type definitions available
		/// in this configuration.
		/// </summary>
		[JsonIgnore]
		public ToolTypeDefinitionCollection ToolTypeDefinitions
		{
			get { return mToolTypeDefinitions; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TravelX																																*
		//*-----------------------------------------------------------------------*
		//private string mTravelX = "";
		/// <summary>
		/// Get/Set the X positive travel direction.
		/// </summary>
		/// <remarks>
		/// Valid choices are Left and Right. Default is Right.
		/// </remarks>
		[JsonIgnore]
		public DirectionLeftRightEnum TravelX
		{
			get
			{
				DirectionLeftRightEnum direction = DirectionLeftRightEnum.Right;
				DirectionLeftRightEnum result = DirectionLeftRightEnum.Right;
				string text = mProperties["TravelX"].Value;

				if(Enum.TryParse<DirectionLeftRightEnum>(text, true, out direction))
				{
					result = direction;
				}
				return result;
			}
			set
			{
				DirectionLeftRightEnum direction = value;

				if(direction == DirectionLeftRightEnum.None)
				{
					direction = DirectionLeftRightEnum.Right;
				}
				mProperties["TravelX"].Value = direction.ToString();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TravelY																																*
		//*-----------------------------------------------------------------------*
		//private string mTravelY = "";
		/// <summary>
		/// Get/Set the Y positive travel direction.
		/// </summary>
		/// <remarks>
		/// Valid choices are Up and Down. Default is Up.
		/// </remarks>
		[JsonIgnore]
		public DirectionUpDownEnum TravelY
		{
			get
			{
				DirectionUpDownEnum direction = DirectionUpDownEnum.Up;
				DirectionUpDownEnum result = DirectionUpDownEnum.Up;
				string text = mProperties["TravelY"].Value;

				if(Enum.TryParse<DirectionUpDownEnum>(text, true, out direction))
				{
					result = direction;
				}
				return result;
			}
			set
			{
				DirectionUpDownEnum direction = value;

				if(direction == DirectionUpDownEnum.None)
				{
					direction = DirectionUpDownEnum.Up;
				}
				mProperties["TravelY"].Value = direction.ToString();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TravelZ																																*
		//*-----------------------------------------------------------------------*
		//private string mTravelZ = "";
		/// <summary>
		/// Get/Set the Z positive travel direction.
		/// </summary>
		/// <remarks>
		/// Valid choices are Up and Down. Default is Up.
		/// </remarks>
		[JsonIgnore]
		public DirectionUpDownEnum TravelZ
		{
			get
			{
				DirectionUpDownEnum direction = DirectionUpDownEnum.Up;
				DirectionUpDownEnum result = DirectionUpDownEnum.Up;
				string text = mProperties["TravelZ"].Value;

				if(Enum.TryParse<DirectionUpDownEnum>(text, true, out direction))
				{
					result = direction;
				}
				return result;
			}
			set
			{
				DirectionUpDownEnum direction = value;

				if(direction == DirectionUpDownEnum.None)
				{
					direction = DirectionUpDownEnum.Up;
				}
				mProperties["TravelZ"].Value = direction.ToString();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserDepth																															*
		//*-----------------------------------------------------------------------*
		//private string mUserDepth = "";
		/// <summary>
		/// Get/Set the user-supplied depth (Z) dimension of the table.
		/// </summary>
		[JsonIgnore]
		public string UserDepth
		{
			get { return mProperties["UserDepth"].Value; }
			set { mProperties["UserDepth"].Value = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserTools																															*
		//*-----------------------------------------------------------------------*
		private UserToolCollection mUserTools = new UserToolCollection();
		/// <summary>
		/// Get a reference to the collection of user tools defined for this
		/// profile.
		/// </summary>
		[JsonProperty(Order = 1)]
		public UserToolCollection UserTools
		{
			get { return mUserTools; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserXDimension																												*
		//*-----------------------------------------------------------------------*
		//private string mUserXDimension = "";
		/// <summary>
		/// Get/Set the user-supplied X dimension of the table.
		/// </summary>
		[JsonIgnore]
		public string UserXDimension
		{
			get { return mProperties["UserXDimension"].Value; }
			set { mProperties["UserXDimension"].Value = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	UserYDimension																												*
		//*-----------------------------------------------------------------------*
		//private string mUserYDimension = "";
		/// <summary>
		/// Get/Set the user-supplied Y dimension of the table.
		/// </summary>
		[JsonIgnore]
		public string UserYDimension
		{
			get { return mProperties["UserYDimension"].Value; }
			set { mProperties["UserYDimension"].Value = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	XDimension																														*
		//*-----------------------------------------------------------------------*
		//private string mXDimension = "";
		/// <summary>
		/// Get/Set the system X dimension of the table, as valid in the registered
		/// units.
		/// </summary>
		[JsonIgnore]
		public string XDimension
		{
			get { return mProperties["XDimension"].Value; }
			set { mProperties["XDimension"].Value = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	XYOrigin																															*
		//*-----------------------------------------------------------------------*
		//private string mXYOrigin = "";
		/// <summary>
		/// Get/Set the XY origin location of the table.
		/// </summary>
		[JsonIgnore]
		public OriginLocationEnum XYOrigin
		{
			get
			{
				OriginLocationEnum location = OriginLocationEnum.Center;
				OriginLocationEnum result = OriginLocationEnum.Center;
				string text = mProperties["XYOrigin"].Value;

				if(Enum.TryParse<OriginLocationEnum>(text, true, out location))
				{
					result = location;
				}
				return result;
			}
			set
			{
				OriginLocationEnum location = value;
				
				if(value == OriginLocationEnum.None)
				{
					location = OriginLocationEnum.Center;
				}
				mProperties["XYOrigin"].Value = location.ToString();
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	YDimension																														*
		//*-----------------------------------------------------------------------*
		//private string mYDimension = "";
		/// <summary>
		/// Get/Set the system Y dimension of the table, as valid in the registered
		/// units.
		/// </summary>
		[JsonIgnore]
		public string YDimension
		{
			get { return mProperties["YDimension"].Value; }
			set { mProperties["YDimension"].Value = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ZOrigin																																*
		//*-----------------------------------------------------------------------*
		//private string mZOrigin = "";
		/// <summary>
		/// Get/Set the Z origin location of the table.
		/// </summary>
		/// <remarks>
		/// Valid values in this version are Top, Center, and Bottom.
		/// </remarks>
		[JsonIgnore]
		public OriginLocationEnum ZOrigin
		{
			get
			{
				OriginLocationEnum location = OriginLocationEnum.Center;
				OriginLocationEnum result = OriginLocationEnum.Center;
				string text = mProperties["ZOrigin"].Value;

				if(Enum.TryParse<OriginLocationEnum>(text, true, out location))
				{
					switch(location)
					{
						case OriginLocationEnum.BottomLeft:
						case OriginLocationEnum.BottomRight:
							location = OriginLocationEnum.Bottom;
							break;
						case OriginLocationEnum.Left:
						case OriginLocationEnum.None:
						case OriginLocationEnum.Right:
							location = OriginLocationEnum.Center;
							break;
						case OriginLocationEnum.TopLeft:
						case OriginLocationEnum.TopRight:
							location = OriginLocationEnum.Top;
							break;
					}
					result = location;
				}
				return result;
			}
			set
			{
				OriginLocationEnum location = value;

				switch(location)
				{
					case OriginLocationEnum.BottomLeft:
					case OriginLocationEnum.BottomRight:
						location = OriginLocationEnum.Bottom;
						break;
					case OriginLocationEnum.Left:
					case OriginLocationEnum.None:
					case OriginLocationEnum.Right:
						location = OriginLocationEnum.Center;
						break;
					case OriginLocationEnum.TopLeft:
					case OriginLocationEnum.TopRight:
						location = OriginLocationEnum.Top;
						break;
				}
				mProperties["ZOrigin"].Value = location.ToString();
			}
		}
		//*-----------------------------------------------------------------------*



	}
	//*-------------------------------------------------------------------------*

}
