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

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	TrackToolCollection																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of TrackToolItem Items.
	/// </summary>
	public class TrackToolCollection : List<TrackToolItem>
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
		//* Initialize																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Configure all of the tools that will be needed on this job.
		/// </summary>
		/// <param name="layouts">
		/// Reference to a collection of operation layout elements.
		/// </param>
		public void Initialize(OperationLayoutCollection layouts)
		{
			float diameter = 0f;
			string toolName = "";
			UserToolItem tool = null;

			this.Clear();
			if(ConfigProfile.GeneralCuttingTool.Length > 0)
			{
				//	A general cutting tool is configured.
				tool = ConfigProfile.UserTools.FirstOrDefault(x =>
					x.ToolName == ConfigProfile.GeneralCuttingTool);
				if(tool != null)
				{
					diameter = GetMillimeters(tool.Properties["Diameter"].Value);
					//	This item supports implicit tool selection.
					this.Add(new TrackToolItem()
					{
						Diameter = diameter,
						KerfClearance = diameter / 2f,
						MaxDepthPerPass = diameter / 2f,
						IsDefault = true,
						ToolName = ConfigProfile.GeneralCuttingTool,
					});
				}
			}
			if(layouts?.Count > 0)
			{
				foreach(OperationLayoutItem layoutItem in layouts)
				{
					if(layoutItem.Operation?.Tool.Length > 0)
					{
						toolName = layoutItem.Operation.Tool;
						if(!this.Exists(x => x.ToolName.ToLower() == toolName.ToLower()))
						{
							//	This item will be unique in the collection.
							tool = ConfigProfile.UserTools.FirstOrDefault(x =>
								x.ToolName.ToLower() == toolName.ToLower());
							if(tool != null)
							{
								diameter = GetMillimeters(tool.Properties["Diameter"].Value);
								this.Add(new TrackToolItem()
								{
									Diameter = diameter,
									KerfClearance = diameter / 2f,
									MaxDepthPerPass = diameter / 2f,
									ToolName = tool.ToolName
								});
							}
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SelectTool																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Select the tool appropriate for the current operation.
		/// </summary>
		/// <param name="toolName">
		/// Name of the tool to select.
		/// </param>
		/// <returns>
		/// Reference to the track tool matching the specified tool name, if
		/// found. Otherwise, null.
		/// </returns>
		public TrackToolItem SelectTool(string toolName)
		{
			TrackToolItem result = null;

			if(toolName?.Length > 0)
			{
				result = this.FirstOrDefault(x =>
					x.ToolName.ToLower() == toolName.ToLower());
			}
			else
			{
				//	If no name was specified, select the default tool.
				result = this.FirstOrDefault(x => x.IsDefault == true);
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	TrackToolItem																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Information about a tool used on a track layer.
	/// </summary>
	public class TrackToolItem
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
		//*	Diameter																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Diameter">Diameter</see>.
		/// </summary>
		private float mDiameter = 0f;
		/// <summary>
		/// Get/Set the diameter of the tool.
		/// </summary>
		public float Diameter
		{
			get { return mDiameter; }
			set { mDiameter = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IsDefault																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="IsDefault">IsDefault</see>.
		/// </summary>
		private bool mIsDefault = false;
		/// <summary>
		/// Get/Set a value indicating whether this is the default tool for
		/// implicit selection.
		/// </summary>
		public bool IsDefault
		{
			get { return mIsDefault; }
			set { mIsDefault = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	KerfClearance																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="KerfClearance">KerfClearance</see>.
		/// </summary>
		private float mKerfClearance = 0f;
		/// <summary>
		/// Get/Set the clearance of the center of this bit from the saved edge of
		/// the work.
		/// </summary>
		/// <remarks>
		/// This value is also sometimes referred to as the tool track offset.
		/// </remarks>
		public float KerfClearance
		{
			get { return mKerfClearance; }
			set { mKerfClearance = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	MaxDepthPerPass																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="MaxDepthPerPass">MaxDepthPerPass</see>.
		/// </summary>
		private float mMaxDepthPerPass = 0f;
		/// <summary>
		/// Get/Set the maximum depth of this bit per pass.
		/// </summary>
		public float MaxDepthPerPass
		{
			get { return mMaxDepthPerPass; }
			set { mMaxDepthPerPass = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToolName																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="ToolName">ToolName</see>.
		/// </summary>
		private string mToolName = "";
		/// <summary>
		/// Get/Set the name of the tool associated with this item.
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
