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
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

using Newtonsoft.Json;

using Geometry;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	ShopToolsUtil																														*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Utility features and functionality for the ShopTools application.
	/// </summary>
	public class ShopToolsUtil
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
		//*	BaseUnit																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the base unit of measure for the specified display unit.
		/// </summary>
		public static string BaseUnit(DisplayUnitEnum displayUnit)
		{
			string result = "mm";

			if(displayUnit == DisplayUnitEnum.UnitedStates)
			{
				result = "in";
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CenteredLeft																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the left coordinate of an object centered within an area.
		/// </summary>
		/// <param name="areaWidth">
		/// Width of the area in which the object is centered.
		/// </param>
		/// <param name="objectWidth">
		/// Width of the object to be placed.
		/// </param>
		/// <returns>
		/// The left coordinate of the centered object.
		/// </returns>
		public static int CenteredLeft(int areaWidth, int objectWidth)
		{
			int result = (int)(((float)areaWidth / 2f) - ((float)objectWidth / 2f));
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* CenteredTop																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the top coordinate of an object centered within an area.
		/// </summary>
		/// <param name="areaHeight">
		/// Height of the area in which the object is centered.
		/// </param>
		/// <param name="objectHeight">
		/// Height of the object to be placed.
		/// </param>
		/// <returns>
		/// The top coordinate of the centered object.
		/// </returns>
		public static int CenteredTop(int areaHeight, int objectHeight)
		{
			int result =
				(int)(((float)areaHeight / 2f) - ((float)objectHeight / 2f));
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clear																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Clear the contents of the specified string builder.
		/// </summary>
		/// <param name="builder">
		/// Reference to the string builder to clear.
		/// </param>
		public static void Clear(StringBuilder builder)
		{
			if(builder?.Length > 0)
			{
				builder.Remove(0, builder.Length);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ConfigProfile																													*
		//*-----------------------------------------------------------------------*
		private static ShopToolsConfigItem mConfigProfile = null;
		/// <summary>
		/// Get/Set a reference to the loaded configuration for this session.
		/// </summary>
		public static ShopToolsConfigItem ConfigProfile
		{
			get { return mConfigProfile; }
			set { mConfigProfile = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ConfigurationFilename																									*
		//*-----------------------------------------------------------------------*
		private static string mConfigurationFilename = "";
		/// <summary>
		/// Get/Set the path and filename of the configuration file that was loaded
		/// for this session.
		/// </summary>
		public static string ConfigurationFilename
		{
			get { return mConfigurationFilename; }
			set { mConfigurationFilename = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DeepClone																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Create a complete, recursive deep clone of the provided object.
		/// </summary>
		/// <typeparam name="T">
		/// The type to be cloned.
		/// </typeparam>
		/// <param name="source">
		/// Reference to the source object to be cloned.
		/// </param>
		/// <returns>
		/// Reference to the newly cloned object, where all values
		/// have been duplicated on a primitive level.
		/// </returns>
		public static T DeepClone<T>(T source)
		{
			object clone = null;
			object clonedFieldValue = null;
			MethodInfo cloneMethod = null;
			FieldInfo[] fields = null;
			Type fieldType = null;
			object fieldValue = null;
			//MethodInfo genericCloneMethod = null;
			//Type genericFieldType = null;
			T result = default(T);
			List<string> stringListNew = null;
			List<string> stringListOriginal = null;
			Type type = null;
			//Type[] typeArgs = { typeof(object) };

			if(source != null)
			{
				type = source.GetType();
				// Create a new instance of the type.
				// This technique requires a parameterless constructor.
				clone = Activator.CreateInstance(type);
				//	Return all private instance fields.
				fields = type.GetFields(
					BindingFlags.Instance | BindingFlags.NonPublic);
				foreach(FieldInfo fieldItem in fields)
				{
					fieldValue = fieldItem.GetValue(source);
					if(fieldValue == null)
					{
						fieldItem.SetValue(clone, null);
					}
					else
					{
						fieldType = fieldItem.FieldType;
						if(fieldType.IsPrimitive ||
							fieldType == typeof(string) ||
							fieldType == typeof(decimal))
						{
							//	We can just copy the value if immutable or primitive.
							fieldItem.SetValue(clone, fieldValue);
						}
						else if(fieldType == typeof(List<string>))
						{
							//	Special handling for basic string lists.
							stringListOriginal = (List<string>)fieldValue;
							//	Create a new list with the same elements.
							stringListNew = new List<string>();
							foreach(string entryItem in stringListOriginal)
							{
								stringListNew.Add(entryItem);
							}
							fieldItem.SetValue(clone, stringListNew);
						}
						else
						{
							//	Attempt to clone other objects using a static clone method.
							cloneMethod = fieldType.GetMethod("Clone",
								BindingFlags.Public | BindingFlags.Static);
							if(cloneMethod != null)
							{
								//	The Clone public static method exists for this class.
								//genericCloneMethod = cloneMethod.MakeGenericMethod(fieldType);
								//clonedFieldValue =
								//	genericCloneMethod.Invoke(null, new object[] { fieldValue });
								clonedFieldValue =
									cloneMethod.Invoke(null, new object[] { fieldValue });
								fieldItem.SetValue(clone, clonedFieldValue);
							}
							else
							{
								//	No Clone method exists for that item. Just pass a
								//	simple reference.
								fieldItem.SetValue(clone, fieldValue);
							}
						}
					}
				}
				result = (T)clone;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DeepTransfer																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Conduct a deep transfer of all of the lowest level values from the
		/// source object to the target.
		/// </summary>
		/// <typeparam name="T">
		/// The type to be cloned.
		/// </typeparam>
		/// <param name="source">
		/// Reference to the source object whose values will be cloned.
		/// </param>
		/// <param name="target">
		/// Reference to the target object whose values will be updated at a base
		/// level.
		/// </param>
		public static void DeepTransfer<T>(T source, T target)
		{
			bool bProcessed = false;
			object clonedFieldValue = null;
			MethodInfo cloneMethod = null;
			FieldInfo[] fields = null;
			Type fieldType = null;
			object fieldValue = null;
			MethodInfo genericCloneMethod = null;
			List<string> stringListNew = null;
			List<string> stringListOriginal = null;
			object targetFieldValue = null;
			Type type = null;

			if(source != null && target != null &&
				source.GetType() == target.GetType())
			{
				type = source.GetType();
				//	Return all private instance fields.
				fields = type.GetFields(
					BindingFlags.Instance | BindingFlags.NonPublic);
				foreach(FieldInfo fieldItem in fields)
				{
					fieldValue = fieldItem.GetValue(source);
					if(fieldValue == null)
					{
						fieldItem.SetValue(target, null);
						bProcessed = true;
					}
					if(!bProcessed)
					{
						fieldType = fieldItem.FieldType;
						if(fieldType.IsPrimitive ||
							fieldType == typeof(string) ||
							fieldType == typeof(decimal))
						{
							//	We can just copy the value if immutable or primitive.
							fieldItem.SetValue(target, fieldValue);
							bProcessed = true;
						}
					}
					if(!bProcessed && fieldType == typeof(List<string>))
					{
						//	Special handling for basic string lists.
						stringListOriginal = (List<string>)fieldValue;
						//	Get the target list.
						targetFieldValue = fieldItem.GetValue(target);
						if(targetFieldValue == null)
						{
							stringListNew = new List<string>();
							fieldItem.SetValue(target, stringListNew);
						}
						else
						{
							stringListNew = (List<string>)targetFieldValue;
						}
						foreach(string entryItem in stringListOriginal)
						{
							stringListNew.Add(entryItem);
						}
						bProcessed = true;
					}
					if(!bProcessed)
					{
						//	Attempt to transfer values from the source to the target.
						cloneMethod = fieldType.GetMethod("TransferValues",
							BindingFlags.Public | BindingFlags.Static);
						if(cloneMethod != null)
						{
							//	The TransferValues public static method exists for this
							//	class.
							genericCloneMethod = cloneMethod.MakeGenericMethod(fieldType);
							genericCloneMethod.Invoke(null,
								new object[] { fieldValue, target });
							bProcessed = true;
						}
					}
					if(!bProcessed)
					{
						//	Attempt to clone other objects using a static clone method.
						cloneMethod = fieldType.GetMethod("Clone",
							BindingFlags.Public | BindingFlags.Static);
						if(cloneMethod != null)
						{
							//	The Clone public static method exists for this class.
							genericCloneMethod = cloneMethod.MakeGenericMethod(fieldType);
							clonedFieldValue =
								genericCloneMethod.Invoke(null, new object[] { fieldValue });
							fieldItem.SetValue(target, clonedFieldValue);
							bProcessed = true;
						}
					}
					if(!bProcessed)
					{
						//	No Clone method exists for that item. Just pass a
						//	simple reference.
						fieldItem.SetValue(target, fieldValue);
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawHole																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw the drill hole symbol.
		/// </summary>
		/// <param name="displayLocation">
		/// Reference to the raw display-space location at which the symbol will
		/// be centered.
		/// </param>
		/// <param name="graphics">
		/// Reference to the currently active graphics device.
		/// </param>
		/// <param name="workspaceArea">
		/// Workspace area representing the client offset from the control.
		/// </param>
		/// <param name="scale">
		/// Scale to apply to the positioning of the symbol on the drawing area.
		/// </param>
		/// <param name="selected">
		/// Value indicating whether the hole and its offset should be drawn as
		/// selected.
		/// </param>
		public static void DrawHole(
			FPoint displayLocation, Graphics graphics,
			Rectangle workspaceArea, float scale, bool selected = false)
		{
			Rectangle targetArea = new Rectangle(0, 0, 12, 12);
			Brush targetBackgroundBrush =
				new SolidBrush(ColorTranslator.FromHtml("#3fffffff"));
			Pen targetBorderPen = null;
			int x1 = 0;
			int x2 = 0;
			int y1 = 0;
			int y2 = 0;

			if(displayLocation != null && graphics != null)
			{
				if(selected)
				{
					targetBorderPen =
						new Pen(ColorTranslator.FromHtml("#f00000ff"), 1f);
				}
				else
				{
					targetBorderPen =
						new Pen(ColorTranslator.FromHtml("#fffffff0"), 1f);
				}
				targetArea.X =
					workspaceArea.Left +
						(int)(displayLocation.X * scale) - (targetArea.Width / 2);
				targetArea.Y =
					workspaceArea.Top +
						(int)(displayLocation.Y * scale) - (targetArea.Height / 2);
				graphics.FillEllipse(targetBackgroundBrush, targetArea);
				graphics.DrawEllipse(targetBorderPen, targetArea);
				x1 = x2 = targetArea.Left + (targetArea.Width / 2);
				y1 = targetArea.Top;
				y2 = targetArea.Bottom;
				graphics.DrawLine(targetBorderPen,
					new Point(x1, y1), new Point(x2, y2));
				x1 = targetArea.Left;
				x2 = targetArea.Right;
				y1 = y2 = targetArea.Top + (targetArea.Height / 2);
				graphics.DrawLine(targetBorderPen,
					new Point(x1, y1), new Point(x2, y2));
			}
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* DrawLine																															*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Draw a line using floating point coordinates, scaling, and translation.
		///// </summary>
		///// <param name="graphics">
		///// Reference to the graphics device to which the line will be painted.
		///// </param>
		///// <param name="pen">
		///// Reference to the pen used to draw the line.
		///// </param>
		///// <param name="startX">
		///// Raw starting X position.
		///// </param>
		///// <param name="startY">
		///// Raw starting Y position.
		///// </param>
		///// <param name="endX">
		///// Raw ending X position.
		///// </param>
		///// <param name="endY">
		///// Raw ending Y position.
		///// </param>
		///// <param name="workspaceArea">
		///// Target workspace on the current canvas.
		///// </param>
		///// <param name="scale">
		///// Scale to apply to the points.
		///// </param>
		//public static void DrawLine(Graphics graphics, Pen pen,
		//	float startX, float startY, float endX, float endY,
		//	Rectangle workspaceArea, float scale)
		//{
		//	int x1 = 0;
		//	int x2 = 0;
		//	int y1 = 0;
		//	int y2 = 0;

		//	if(graphics != null && pen != null)
		//	{
		//		x1 = (int)(startX * scale) + workspaceArea.X;
		//		y1 = (int)(startY * scale) + workspaceArea.Y;
		//		x2 = (int)(endX * scale) + workspaceArea.X;
		//		y2 = (int)(endY * scale) + workspaceArea.Y;
		//		graphics.DrawLine(pen,
		//			new Point(x1, y1), new Point(x2, y2));
		//	}
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawLine																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw a line using floating point coordinates, scaling, and translation.
		/// </summary>
		/// <param name="start">
		/// Raw starting coordinate.
		/// </param>
		/// <param name="end">
		/// Raw ending coordinate.
		/// </param>
		/// <param name="graphics">
		/// Reference to the graphics device to which the line will be painted.
		/// </param>
		/// <param name="pen">
		/// Reference to the pen used to draw the line.
		/// </param>
		/// <param name="workspaceArea">
		/// Target workspace on the current canvas.
		/// </param>
		/// <param name="scale">
		/// Scale to apply to the points.
		/// </param>
		public static void DrawLine(FPoint start, FPoint end,
			Graphics graphics, Pen pen, Rectangle workspaceArea, float scale)
		{
			int x1 = 0;
			int x2 = 0;
			int y1 = 0;
			int y2 = 0;

			if(start != null && end != null && graphics != null && pen != null)
			{
				x1 = (int)(start.X * scale) + workspaceArea.X;
				y1 = (int)(start.Y * scale) + workspaceArea.Y;
				x2 = (int)(end.X * scale) + workspaceArea.X;
				y2 = (int)(end.Y * scale) + workspaceArea.Y;
				graphics.DrawLine(pen,
					new Point(x1, y1), new Point(x2, y2));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawOperation																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw the specified operation to the provided graphics context.
		/// </summary>
		/// <param name="operation">
		/// The operation for which user values have been provided.
		/// </param>
		/// <param name="workpiece">
		/// Reference to information about the workpiece.
		/// </param>
		/// <param name="startingLocation">
		/// Reference to the starting location from which the operation will
		/// begin, in system units
		/// </param>
		/// <param name="previousToolName">
		/// Name of the previous tool.
		/// </param>
		/// <param name="g">
		/// Reference to the current graphics device.
		/// </param>
		/// <param name="workspaceArea">
		/// Target workspace on the current canvas.
		/// </param>
		/// <param name="scale">
		/// Precalculated scale of drawing space to canvas space.
		/// </param>
		/// <param name="selected">
		/// Optional value indicating whether the operation is selected.
		/// </param>
		/// <returns>
		/// Reference to the location at which the current operation will end,
		/// in system units.
		/// </returns>
		public static FPoint DrawOperation(
			PatternOperationItem operation,
			WorkpieceInfoItem workpiece, FPoint startingLocation,
			string previousToolName, Graphics g, Rectangle workspaceArea,
			float scale, bool selected = false)
		{
			Pen drawPen = new Pen(ColorTranslator.FromHtml("#ffff0000"), 2f);
			FPoint endOffset = new FPoint();
			float endOffsetX = 0f;
			float endOffsetY = 0f;
			FPoint location = null;
			float[] moveDashes = { 4, 4 };
			Pen movePen = new Pen(ColorTranslator.FromHtml("#ffffffff"), 2f)
			{
				DashPattern = moveDashes
			};
			FPoint result = new FPoint();
			FPoint startOffset = new FPoint();
			float startOffsetX = 0f;
			float startOffsetY = 0f;
			SizeF systemSize = GetSystemSize();
			string toolName = "";
			FPoint transform = new FPoint();

			if(operation != null && workpiece != null)
			{
				if(selected)
				{
					drawPen = new Pen(ColorTranslator.FromHtml("#ff0000ff"), 2f);
					movePen = new Pen(ColorTranslator.FromHtml("#ff0000ff"), 2f)
					{
						DashPattern = moveDashes
					};
				}
				location = FPoint.Clone(startingLocation);
				toolName = operation.Tool;
				if(toolName.Length == 0 ||
					toolName.ToLower() == "{generaltoolname}")
				{
					//	General tool specification.
					toolName = ConfigProfile.GeneralCuttingTool;
				}
				else if(toolName.ToLower() == "{previoustoolname}")
				{
					//	Previously used tool.
					toolName = previousToolName;
				}
				startOffset = location =
					GetOperationStartLocation(operation, workpiece, location);
				startOffsetX = location.X;
				startOffsetY = location.Y;

				endOffset = location =
					GetOperationEndLocation(operation, workpiece, location);
				endOffsetX = location.X;
				endOffsetY = location.Y;
				result = location;

				//	We have start and end offsets.
				switch(operation.Action)
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
						if(operation.OffsetX.Length > 0 ||
							operation.OffsetY.Length > 0)
						{
							//	The head needs to be moved to the starting position.
							DrawLine(startingLocation, startOffset,
								g, movePen, workspaceArea, scale);
						}
						DrawLine(startOffset, endOffset,
							g, drawPen, workspaceArea, scale);
						break;
					case OperationActionEnum.DrawLineLengthWidth:
						break;
					case OperationActionEnum.DrawLineXY:
						if(operation.StartOffsetX.Length > 0 ||
							operation.StartOffsetY.Length > 0)
						{
							//	The head needs to be moved to the starting position.
							DrawLine(startingLocation, startOffset,
								g, movePen, workspaceArea, scale);
						}
						DrawLine(startOffset, endOffset,
							g, drawPen, workspaceArea, scale);
						break;
					case OperationActionEnum.DrawPath:
					case OperationActionEnum.DrawRectangleLengthWidth:
						break;
					case OperationActionEnum.DrawRectangleXY:
						if(operation.StartOffsetX.Length > 0)
						{
							//	The head needs to be moved to the starting position.
						}
						break;
					case OperationActionEnum.FillCircleCenterDiameter:
					case OperationActionEnum.FillCircleCenterRadius:
					case OperationActionEnum.FillCircleDiameter:
					case OperationActionEnum.FillCircleRadius:
					case OperationActionEnum.FillEllipseCenterDiameterXY:
					case OperationActionEnum.FillEllipseCenterRadiusXY:
					case OperationActionEnum.FillEllipseDiameterXY:
					case OperationActionEnum.FillEllipseLengthWidth:
					case OperationActionEnum.FillEllipseRadiusXY:
						break;
					case OperationActionEnum.FillEllipseXY:
						if(operation.StartOffsetX.Length > 0)
						{
							//	The head needs to be moved to the starting position.
						}
						break;
					case OperationActionEnum.FillPath:
					case OperationActionEnum.FillRectangleLengthWidth:
						break;
					case OperationActionEnum.FillRectangleXY:
						if(operation.StartOffsetX.Length > 0)
						{
							//	The head needs to be moved to the starting position.
						}
						break;
					case OperationActionEnum.MoveAngleLength:
					case OperationActionEnum.MoveXY:
						DrawLine(startingLocation, endOffset,
							g, movePen, workspaceArea, scale);
						break;
					case OperationActionEnum.None:
						break;
					case OperationActionEnum.PointXY:
						if(operation.OffsetX.Length > 0 ||
							operation.OffsetY.Length > 0)
						{
							//	The head needs to be moved to the starting position.
							DrawLine(startingLocation, startOffset,
								g, movePen, workspaceArea, scale);
						}
						DrawHole(endOffset, g, workspaceArea, scale, selected);
						break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawRouter																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw the starting or ending router symbol.
		/// </summary>
		/// <param name="displayLocation">
		/// Reference to the raw display-space location at which the symbol will
		/// be centered.
		/// </param>
		/// <param name="startEnd">
		/// The type of symbol to draw: Start or End.
		/// </param>
		/// <param name="graphics">
		/// Reference to the currently active graphics device.
		/// </param>
		/// <param name="workspaceArea">
		/// Workspace area representing the client offset from the control.
		/// </param>
		/// <param name="scale">
		/// Scale to apply to the positioning of the symbol on the drawing area.
		/// </param>
		public static void DrawRouter(
			FPoint displayLocation, StartEndEnum startEnd, Graphics graphics,
			Rectangle workspaceArea, float scale)
		{
			Rectangle targetArea = new Rectangle(0, 0, 16, 16);
			Brush targetBackgroundBrush =
				new SolidBrush(ColorTranslator.FromHtml("#7fffffff"));
			Pen targetBorderPen = null;
			int x1 = 0;
			int x2 = 0;
			int y1 = 0;
			int y2 = 0;

			if(displayLocation != null && graphics != null)
			{
				switch(startEnd)
				{
					case StartEndEnum.End:
						targetBorderPen =
							new Pen(ColorTranslator.FromHtml("#f0ff0000"), 2f);
						break;
					case StartEndEnum.Start:
					default:
						targetBorderPen =
							new Pen(ColorTranslator.FromHtml("#f0007f00"), 2f);
						break;
				}
				targetArea.X =
					workspaceArea.Left +
						(int)(displayLocation.X * scale) - (targetArea.Width / 2);
				targetArea.Y =
					workspaceArea.Top +
						(int)(displayLocation.Y * scale) - (targetArea.Height / 2);
				graphics.FillEllipse(targetBackgroundBrush, targetArea);
				graphics.DrawEllipse(targetBorderPen, targetArea);
				x1 = x2 = targetArea.Left + (targetArea.Width / 2);
				y1 = targetArea.Top;
				y2 = targetArea.Bottom;
				graphics.DrawLine(targetBorderPen,
					new Point(x1, y1), new Point(x2, y2));
				x1 = targetArea.Left;
				x2 = targetArea.Right;
				y1 = y2 = targetArea.Top + (targetArea.Height / 2);
				graphics.DrawLine(targetBorderPen,
					new Point(x1, y1), new Point(x2, y2));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* DrawTable																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Draw the work table and the workpiece.
		/// </summary>
		/// <param name="control">
		/// Reference to the panel control being painted.
		/// </param>
		/// <param name="workpieceInfo">
		/// Information about the workpiece and its cuts.
		/// </param>
		/// <param name="graphics">
		/// Reference to the active graphics device.
		/// </param>
		public static void DrawTable(Panel control,
			WorkpieceInfoItem workpieceInfo,
			Graphics graphics)
		{
			Brush backgroundBrush =
				new SolidBrush(ColorTranslator.FromHtml("#113366"));
			Pen borderPen = new Pen(new SolidBrush(Color.Black), 2f);
			int index = 0;
			Rectangle offsetArea = Rectangle.Empty;
			FPoint offsetLocation = new FPoint();
			float scale = 1f;
			string[] shadowLevels = new string[]
			{
				"af",
				"7f",
				"5f",
				"3f",
				"1f"
			};
			Pen shadowPen = new Pen(Color.Black);
			SizeF systemSize = GetSystemSize();
			Rectangle targetArea = new Rectangle(0, 0, 16, 16);
			//Brush targetBackgroundBrush =
			//	new SolidBrush(ColorTranslator.FromHtml("#7fffffff"));
			//Pen targetBorderPen =
			//	new Pen(ColorTranslator.FromHtml("#f0007f00"), 2f);
			Pen workpieceBorderPen =
				new Pen(new SolidBrush(ColorTranslator.FromHtml("#6e6d11")), 2f);
			Brush workpieceBrush =
				new SolidBrush(ColorTranslator.FromHtml("#333333"));
			Rectangle workspaceArea = Rectangle.Empty;
			Brush workspaceBrush =
				new SolidBrush(ColorTranslator.FromHtml("#603e1f"));
			Size workspaceSize = Size.Empty;
			float workspaceRatio = GetWorkspaceRatio();
			int x1 = 0;
			int x2 = 0;
			int y1 = 0;
			int y2 = 0;

			if(control != null && graphics != null)
			{
				//Debug.WriteLine("Paint Workspace...");
				graphics.CompositingMode = CompositingMode.SourceOver;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.Bicubic;
				graphics.SmoothingMode = SmoothingMode.AntiAlias;

				//	Clear the background.
				graphics.FillRectangle(backgroundBrush,
					new RectangleF(0, 0, control.Width, control.Height));
				//	Draw the shadow for the workspace.
				workspaceSize = ResizeArea(
					control.Width - 16, control.Height - 16, workspaceRatio);
				scale = (float)workspaceSize.Width / systemSize.Width;
				workspaceArea = new Rectangle(
					CenteredLeft(control.Width, workspaceSize.Width),
					CenteredTop(control.Height, workspaceSize.Height),
					workspaceSize.Width, workspaceSize.Height);
				//Debug.WriteLine($"Workspace: {workspaceArea}");
				//	Draw the shadow.
				for(index = 4; index > 0; index--)
				{
					offsetArea = OffsetArea(workspaceArea, index, index);
					shadowPen.Color =
						ColorTranslator.FromHtml($"#{shadowLevels[index]}000000");
					graphics.DrawLine(shadowPen,
						new Point(offsetArea.Left, offsetArea.Bottom),
						new Point(offsetArea.Right, offsetArea.Bottom));
					graphics.DrawLine(shadowPen,
						new Point(offsetArea.Right, offsetArea.Top),
						new Point(offsetArea.Right, offsetArea.Bottom));
				}
				//	Draw the workspace.
				graphics.FillRectangle(workspaceBrush, workspaceArea);
				graphics.DrawRectangle(borderPen, workspaceArea);
				//	Draw the workpiece.
				if(FArea.HasVolume(workpieceInfo.Area))
				{
					offsetArea =
						TransformToDisplay(workspaceArea, mSessionWorkpieceInfo);
					//offsetArea = new Rectangle(
					//	(int)(workspaceArea.Left + (workpieceInfo.Area.Left * scale)),
					//	(int)(workspaceArea.Top + (workpieceInfo.Area.Top * scale)),
					//	(int)(workpieceInfo.Area.Width * scale),
					//	(int)(workpieceInfo.Area.Height * scale));
					graphics.FillRectangle(workpieceBrush, offsetArea);
					graphics.DrawRectangle(workpieceBorderPen, offsetArea);
				}
				//	X pattern at center.
				shadowPen.Color =
					ColorTranslator.FromHtml($"#7f7f7f");
				x1 = x2 = workspaceArea.Left + (workspaceArea.Width / 2);
				y1 = workspaceArea.Top;
				y2 = workspaceArea.Bottom;
				graphics.DrawLine(shadowPen,
					new Point(x1, y1),
					new Point(x2, y2));
				x1 = workspaceArea.Left;
				x2 = workspaceArea.Right;
				y1 = y2 = workspaceArea.Top + (workspaceArea.Height / 2);
				graphics.DrawLine(shadowPen,
					new Point(x1, y1),
					new Point(x2, y2));
				////	Draw the router.
				//offsetLocation = TransformFromAbsolute(workpieceInfo.RouterLocation);
				//targetArea.X =
				//	workspaceArea.Left +
				//		(int)(offsetLocation.X * scale) - (targetArea.Width / 2);
				//targetArea.Y =
				//	workspaceArea.Top +
				//		(int)(offsetLocation.Y * scale) - (targetArea.Height / 2);
				////offsetLocation =
				////	GetOffsetDisplayFromOrigin(
				////		workpieceInfo.RouterLocation, scale);
				////targetArea.X =
				////	workspaceArea.Left +
				////		(int)offsetLocation.X - (targetArea.Width / 2);
				////targetArea.Y =
				////	workspaceArea.Top +
				////		(int)offsetLocation.Y - (targetArea.Height / 2);
				//graphics.FillEllipse(targetBackgroundBrush, targetArea);
				//graphics.DrawEllipse(targetBorderPen, targetArea);
				//x1 = x2 = targetArea.Left + (targetArea.Width / 2);
				//y1 = targetArea.Top;
				//y2 = targetArea.Bottom;
				//graphics.DrawLine(targetBorderPen,
				//	new Point(x1, y1), new Point(x2, y2));
				//x1 = targetArea.Left;
				//x2 = targetArea.Right;
				//y1 = y2 = targetArea.Top + (targetArea.Height / 2);
				//graphics.DrawLine(targetBorderPen,
				//	new Point(x1, y1), new Point(x2, y2));
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ExpandCamelCase																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Expand a camel-case value to alphanumeric with spaces.
		/// </summary>
		/// <param name="value">
		/// Value to expand.
		/// </param>
		/// <returns>
		/// Alphanumeric string with spaces.
		/// </returns>
		public static string ExpandCamelCase(string value)
		{
			StringBuilder builder = new StringBuilder();
			char charc = '\0';
			char[] chars = null;
			int count = 0;
			int index = 0;

			if(value?.Length > 0)
			{
				chars = value.ToCharArray();
				count = chars.Length;
				for(index = 0; index < count; index++)
				{
					charc = chars[index];
					if(index == 0)
					{
						//	Capitalize first item.
						builder.Append(char.ToUpper(charc));
					}
					else if(char.IsUpper(charc))
					{
						builder.Append(' ');
						builder.Append(charc);
					}
					else
					{
						builder.Append(charc);
					}
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* FromTitleCase																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Convert a string from its title case representation to one with
		/// string separators.
		/// </summary>
		/// <param name="titleCaseString">
		/// The title case string to separate.
		/// </param>
		/// <returns>
		/// A string with separators prior to each capital letter except the first.
		/// </returns>
		public static string FromTitleCase(string titleCaseString)
		{
			char cchar = '\0';
			byte cValue = 0;
			char[] chars = null;
			int count = 0;
			int index = 0;
			StringBuilder builder = new StringBuilder();

			if(titleCaseString?.Length > 0)
			{
				chars = titleCaseString.ToCharArray();
				count = chars.Length;
				for(index = 0; index < count; index ++)
				{
					if(index > 0)
					{
						cchar = chars[index];
						cValue = (byte)cchar;
						if(cValue >= 65 && cValue <= 90)
						{
							builder.Append(' ');
						}
						builder.Append(cchar);
					}
					else
					{
						builder.Append(chars[index]);
					}
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetAltValue																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the formatted alternate measurement value from the caller's
		/// value.
		/// </summary>
		/// <param name="value">
		/// The text value to inspect.
		/// </param>
		/// <param name="normalValue">
		/// The normal value already set for this object.
		/// </param>
		/// <param name="useParethesis">
		/// Optional value indicating whether to place the alternate value in
		/// parenthesis. Default = true.
		/// </param>
		/// <returns>
		/// The formatted alternate measurement value for the supplied text, where
		/// a blank string remains blank, a repeat of the normal value is blank,
		/// a space is replaced by elipsis, and any other value is placed in
		/// parenthesis.
		/// </returns>
		public static string GetAltValue(string value, string normalValue = "",
			bool useParethesis = true)
		{
			string result = "";

			if(value?.Length > 0)
			{
				switch(value)
				{
					case " ":
					case "...":
						result = "...";
						break;
					default:
						if(value.Replace(" ", "") != normalValue.Replace(" ", ""))
						{
							if(useParethesis &&
								!(value.StartsWith("(") && value.EndsWith(")")))
							{
								result = $"({value})";
							}
							else
							{
								result = value;
							}
						}
						break;
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetAngle																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the binary representation of the caller's angle, in degrees,
		/// after solving for variables and expressions.
		/// </summary>
		/// <param name="angleText">
		/// The freehand angle text to convert to a single binary value, in
		/// degrees.
		/// </param>
		/// <returns>
		/// The binary floating point representation of the caller's specified
		/// angle, after variables and expressions have been solved.
		/// </returns>
		/// <remarks>
		/// In this version, the text can be purely numeric, in which case, it will
		/// be interpreted as degrees, or can have the suffices 'deg', 'degrees',
		/// 'rad', or 'radians'.
		/// </remarks>
		public static float GetAngle(string angleText)
		{
			Match match = null;
			float result = 0f;
			string text = "";

			if(angleText?.Length > 0)
			{
				match = Regex.Match(angleText, ResourceMain.rxAngleUnit);
				if(match.Success)
				{
					result = ToFloat(GetValue(match, "angle"));
					text = GetValue(match, "unit").ToLower();
					switch(text)
					{
						case "rad":
						case "radians":
							//	The result was already expressed in radians.
							break;
						case "deg":
						case "degrees":
						default:
							result = Trig.DegToRad(result);
							break;
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetFloatString																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the formally formatted version of the caller's floating point
		/// number.
		/// </summary>
		/// <param name="value">
		/// A value to convert to float then format.
		/// </param>
		/// <returns>
		/// String representation of a valid floating point number, if the caller's
		/// input was legitimate. Otherwise, "0".
		/// </returns>
		public static string GetFloatString(string value)
		{
			float result = ToFloat(value);

			return result.ToString("0.###");
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetIntegerString																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the formally formatted version of the caller's integer number.
		/// </summary>
		/// <param name="value">
		/// A value to convert to integer then format.
		/// </param>
		/// <returns>
		/// String representation of a valid integer number, if the caller's
		/// input was legitimate. Otherwise, "0".
		/// </returns>
		public static string GetIntegerString(string value)
		{
			float result = ToInt(value);

			return result.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetMeasurementString																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the full measurement string corresponding to the caller's value
		/// and default unit.
		/// </summary>
		/// <param name="value">
		/// The value, optionally including a specific measurement unit.
		/// </param>
		/// <param name="defaultUnit">
		/// The default measurement unit to apply, if no unit was provided.
		/// </param>
		/// <returns>
		/// The caller's measurement value and unit.
		/// </returns>
		public static string GetMeasurementString(string value, string defaultUnit)
		{
			StringBuilder builder = new StringBuilder();
			MeasurementCollection measurements = null;

			if(value?.Length > 0 && defaultUnit?.Length > 0)
			{
				measurements =
					MeasurementCollection.GetMeasurements(value, defaultUnit);
				switch(defaultUnit)
				{
					case "in":
						builder.Append(measurements.SumInches().ToString("0.###"));
						builder.Append("in");
						break;
					case "mm":
						builder.Append(measurements.SumMillimeters().ToString("0.###"));
						builder.Append("mm");
						break;
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* GetMeasurementString																									*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return the full measurement string corresponding to the caller's value
		///// and default unit.
		///// </summary>
		///// <param name="value">
		///// The value, optionally including a specific measurement unit.
		///// </param>
		///// <param name="defaultUnit">
		///// The default measurement unit to apply, if no unit was provided.
		///// </param>
		///// <param name="finalUnit">
		///// The name of the unit to which the value will be converted for return
		///// to the caller.
		///// </param>
		///// <returns>
		///// The caller's measurement value and unit.
		///// </returns>
		//public static string GetMeasurementString(string value, string defaultUnit,
		//	string finalUnit)
		//{
		//	StringBuilder builder = new StringBuilder();
		//	MeasurementCollection measurements = null;
		//	double number = 0d;

		//	if(value?.Length > 0 && defaultUnit?.Length > 0)
		//	{
		//		measurements = GetMeasurements(value, defaultUnit);
		//		switch(defaultUnit)
		//		{
		//			case "in":
		//				number = mSessionConverter.Convert(
		//					measurements.SumInches(), "in", finalUnit);
		//				builder.Append(number.ToString("0.###"));
		//				builder.Append(finalUnit);
		//				break;
		//			case "mm":
		//				number = mSessionConverter.Convert(
		//					measurements.SumMillimeters(), "mm", finalUnit);
		//				builder.Append(number.ToString("0.###"));
		//				builder.Append(finalUnit);
		//				break;
		//		}
		//	}
		//	return builder.ToString();
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetMillimeters																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the caller's value, represented in system millimeters.
		/// </summary>
		/// <param name="value">
		/// The freehand numerical value to convert.
		/// </param>
		/// <returns>
		/// The binary representation of the caller's value, in millimeters, using
		/// the default base unit of this session, where appropriate.
		/// </returns>
		public static float GetMillimeters(string value)
		{
			//	TODO: Variable and expression handling on Util.GetMillimeters.
			return MeasurementCollection.SumMillimeters(value,
				BaseUnit(mConfigProfile.DisplayUnits));
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetNonNumeric																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the first non-numeric portion of the caller's string, not
		/// including spaces.
		/// </summary>
		/// <param name="value">
		/// The user input value to parse.
		/// </param>
		/// <returns>
		/// The first non-numeric portion of the caller's string, not including
		/// spaces.
		/// </returns>
		/// <remarks>
		/// In this version, the non-numeric value trails the numeric, and does
		/// not include spaces.
		/// </remarks>
		public static string GetNonNumeric(string value)
		{
			string result =
				GetValue(value, ResourceMain.rxNumericNonNumeric, "nonnumeric");
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetNumeric																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the first numeric portion of the string.
		/// </summary>
		/// <param name="value">
		/// The user input value to parse.
		/// </param>
		/// <returns>
		/// The first numeric portion of the caller's string, not including
		/// spaces.
		/// </returns>
		public static string GetNumeric(string value)
		{
			string result =
				GetValue(value, ResourceMain.rxNumericNonNumeric, "numeric");

			return result;
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* GetOffsetDisplayFromOrigin																						*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return the display coordinates of the offset from the currently defined
		///// origin, given a relative system offset and scaling factor.
		///// </summary>
		///// <param name="relativeSystemOffset">
		///// The relative system offset from which to determine the current position
		///// in relation to the defined origin.
		///// </param>
		///// <param name="scale">
		///// The optional scale to apply to the resulting coordinates.
		///// </param>
		///// <returns>
		///// A point the specified distance from the currently defined origin,
		///// scaled by the optional factor.
		///// </returns>
		///// <remarks>
		///// The physical origin is the place on the working table where the
		///// coordinates are said to be X:0; Y:0, while the display origin is the
		///// same point expressed as a distance from the top left corner of the
		///// visual representation.
		///// </remarks>
		//public static FPoint GetOffsetDisplayFromOrigin(
		//	FPoint relativeSystemOffset, float scale = 1f)
		//{
		//	float height =
		//		MeasurementCollection.SumMillimeters(mConfigProfile.YDimension,
		//			BaseUnit(mConfigProfile.DisplayUnits));
		//	FPoint result = new FPoint();
		//	float width =
		//		MeasurementCollection.SumMillimeters(mConfigProfile.XDimension,
		//			BaseUnit(mConfigProfile.DisplayUnits));
		//	float x = 0f;
		//	float y = 0f;

		//	//	X.
		//	switch(mConfigProfile.XYOrigin)
		//	{
		//		case OriginLocationEnum.BottomLeft:
		//		case OriginLocationEnum.Left:
		//		case OriginLocationEnum.TopLeft:
		//			x = 0f;
		//			break;
		//		case OriginLocationEnum.Bottom:
		//		case OriginLocationEnum.Center:
		//		case OriginLocationEnum.None:
		//		case OriginLocationEnum.Top:
		//			x = width / 2f;
		//			break;
		//		case OriginLocationEnum.BottomRight:
		//		case OriginLocationEnum.Right:
		//		case OriginLocationEnum.TopRight:
		//			x = width;
		//			break;
		//	}
		//	//	Y.
		//	switch(mConfigProfile.XYOrigin)
		//	{
		//		case OriginLocationEnum.Top:
		//		case OriginLocationEnum.TopLeft:
		//		case OriginLocationEnum.TopRight:
		//			y = 0f;
		//			break;
		//		case OriginLocationEnum.Center:
		//		case OriginLocationEnum.Left:
		//		case OriginLocationEnum.None:
		//		case OriginLocationEnum.Right:
		//			y = height / 2f;
		//			break;
		//		case OriginLocationEnum.Bottom:
		//		case OriginLocationEnum.BottomLeft:
		//		case OriginLocationEnum.BottomRight:
		//			y = height;
		//			break;
		//	}
		//	//	Apply offset.
		//	if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
		//	{
		//		x -= relativeSystemOffset.X;
		//	}
		//	else
		//	{
		//		x += relativeSystemOffset.X;
		//	}
		//	if(mConfigProfile.TravelY == DirectionUpDownEnum.Up)
		//	{
		//		y -= relativeSystemOffset.Y;
		//	}
		//	else
		//	{
		//		y += relativeSystemOffset.Y;
		//	}
		//	if(scale != 1f)
		//	{
		//		//	Apply a scale.
		//		x *= scale;
		//		y *= scale;
		//	}
		//	result.X = x;
		//	result.Y = y;
		//	return result;
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* GetOffsetDisplayFromWorkpiece																					*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return the display coordinates of the offset from the currently
		///// positioned workpiece, given a relative system offset and scaling
		///// factor.
		///// </summary>
		///// <param name="workpiece">
		///// Reference to the workpiece information for which the offset is being
		///// transformed.
		///// </param>
		///// <param name="relativeSystemOffset">
		///// The relative system offset from which to determine the current position
		///// in relation to the defined workpiece.
		///// </param>
		///// <param name="scale">
		///// The optional scale to apply to the resulting coordinates.
		///// </param>
		///// <returns>
		///// A point the specified distance from the currently defined workpiece,
		///// scaled by the optional factor.
		///// </returns>
		///// <remarks>
		///// The physical origin is the place on the working table where the
		///// coordinates are said to be X:0; Y:0, and the material origin is the
		///// corner of the material closest to the physical origin, while the
		///// display origin is the same point expressed as a distance from the
		///// top left corner of the visual screen.
		///// </remarks>
		//public static FPoint GetOffsetDisplayFromWorkpiece(
		//	WorkpieceInfoItem workpiece, FPoint relativeSystemOffset,
		//	float scale = 1f)
		//{
		//	FPoint result = null;

		//	if(workpiece != null && relativeSystemOffset != null)
		//	{
		//		result = GetOffsetDisplayFromOrigin(
		//			new FPoint(workpiece.Area.X + relativeSystemOffset.X,
		//				workpiece.Area.Y + relativeSystemOffset.Y), scale);
		//	}
		//	if(result == null)
		//	{
		//		result = new FPoint();
		//	}
		//	return result;
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetOperationEndLocation																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the end offset location or extent for the represented operation
		/// and its associated variables.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation to inspect.
		/// </param>
		/// <param name="workpiece">
		/// Reference to the workpiece against which relative actions are taken.
		/// </param>
		/// <param name="location">
		/// Reference to the virtual default location.
		/// </param>
		/// <returns>
		/// Reference to the starting offset of the operation, given the action
		/// and the filled user variables.
		/// </returns>
		public static FPoint GetOperationEndLocation(
			PatternOperationItem operation,
			WorkpieceInfoItem workpiece, FPoint location)
		{
			float angle = 0f;
			float length = 0f;
			FPoint newLocation = null;
			float positionX = 0f;
			float positionY = 0f;
			FPoint result = new FPoint();
			float width = 0f;

			if(operation != null)
			{
				//	Extent.
				switch(operation.Action)
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
					case OperationActionEnum.PointXY:
						//	End = Start.
						positionX = location.X;
						positionY = location.Y;
						break;
					case OperationActionEnum.DrawLineAngleLength:
					case OperationActionEnum.MoveAngleLength:
						//	Angle, Length.
						angle = GetAngle(operation.Angle);
						length = GetMillimeters(operation.Length);
						newLocation = Trig.GetDestPoint(location, angle, length);
						positionX = newLocation.X;
						positionY = newLocation.Y;
						break;
					case OperationActionEnum.DrawLineLengthWidth:
					case OperationActionEnum.DrawRectangleLengthWidth:
					case OperationActionEnum.FillRectangleLengthWidth:
						//	Length, Width.
						length = GetMillimeters(operation.Length);
						width = GetMillimeters(operation.Width);
						if(ConfigProfile.AxisXIsOpenEnded)
						{
							positionX += length;
							positionY += width;
						}
						else
						{
							positionX += width;
							positionY += length;
						}
						break;
					case OperationActionEnum.DrawLineXY:
					case OperationActionEnum.DrawRectangleXY:
					case OperationActionEnum.FillRectangleXY:
						//	EndOffsetX, EndOffsetY.
						//	X.
						if(operation.EndOffsetX.Length > 0 ||
							operation.EndOffsetXOrigin != OffsetLeftRightEnum.None)
						{
							//	End offset was specified.
							positionX = GetMillimeters(operation.EndOffsetX);
							positionX = TranslateOffset(workpiece.Area, positionX,
								operation.EndOffsetXOrigin, location.X);
						}
						else
						{
							positionX = location.X;
						}
						//	Y.
						if(operation.EndOffsetY.Length > 0 ||
							(operation.EndOffsetYOrigin != OffsetTopBottomEnum.None))
						{
							//	End offset was specified.
							positionY = GetMillimeters(operation.EndOffsetY);
							positionY = TranslateOffset(workpiece.Area, positionY,
								operation.EndOffsetYOrigin, location.Y);
						}
						else
						{
							positionY = location.Y;
						}
						break;
					case OperationActionEnum.MoveXY:
						//	Ending OffsetX, OffsetY.
						//	X.
						if(operation.OffsetX.Length > 0 ||
							operation.OffsetXOrigin != OffsetLeftRightEnum.None)
						{
							//	End offset was specified.
							positionX = GetMillimeters(operation.OffsetX);
							positionX = TranslateOffset(workpiece.Area, positionX,
								operation.OffsetXOrigin, location.X);
						}
						else
						{
							positionX = location.X;
						}
						//	Y.
						if(operation.OffsetY.Length > 0 ||
							operation.OffsetYOrigin != OffsetTopBottomEnum.None)
						{
							//	End offset was specified.
							positionY = GetMillimeters(operation.OffsetY);
							positionY = TranslateOffset(workpiece.Area, positionY,
								operation.OffsetYOrigin, location.Y);
						}
						else
						{
							positionY = location.Y;
						}
						break;
				}
				//	Update the location from the ending location.
				result.X = positionX;
				result.Y = positionY;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetOperationStartLocation																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the start offset location for the represented operation and its
		/// associated variables.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation to inspect.
		/// </param>
		/// <param name="workpiece">
		/// Reference to the workpiece against which relative actions are taken.
		/// </param>
		/// <param name="location">
		/// Reference to the virtual default location.
		/// </param>
		/// <returns>
		/// Reference to the starting offset of the operation, given the action
		/// and the filled user variables.
		/// </returns>
		public static FPoint GetOperationStartLocation(
			PatternOperationItem operation,
			WorkpieceInfoItem workpiece, FPoint location)
		{
			float positionX = 0f;
			float positionY = 0f;
			FPoint result = new FPoint();

			if(location != null)
			{
				positionX = location.X;
				positionY = location.Y;
			}
			if(operation != null)
			{
				//	StartX,Y.
				switch(operation.Action)
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
					case OperationActionEnum.DrawLineAngleLength:
					case OperationActionEnum.DrawLineLengthWidth:
					case OperationActionEnum.DrawRectangleLengthWidth:
					case OperationActionEnum.FillCircleCenterDiameter:
					case OperationActionEnum.FillCircleCenterRadius:
					case OperationActionEnum.FillCircleDiameter:
					case OperationActionEnum.FillCircleRadius:
					case OperationActionEnum.FillEllipseCenterDiameterXY:
					case OperationActionEnum.FillEllipseCenterRadiusXY:
					case OperationActionEnum.FillEllipseDiameterXY:
					case OperationActionEnum.FillEllipseLengthWidth:
					case OperationActionEnum.FillEllipseRadiusXY:
					case OperationActionEnum.FillRectangleLengthWidth:
					case OperationActionEnum.PointXY:
						//	Starting OffsetX, OffsetY.
						//	X.
						if(operation.OffsetX.Length > 0 ||
							operation.OffsetXOrigin != OffsetLeftRightEnum.None)
						{
							//	X was specified.
							positionX = GetMillimeters(operation.OffsetX);
							positionX = TranslateOffset(workpiece.Area, positionX,
								operation.OffsetXOrigin, location.X);
						}
						else
						{
							positionX = location.X;
						}
						//	Y.
						if(operation.OffsetY.Length > 0 ||
							operation.OffsetYOrigin != OffsetTopBottomEnum.None)
						{
							//	Y was specified.
							positionY = GetMillimeters(operation.OffsetY);
							positionY = TranslateOffset(workpiece.Area, positionY,
								operation.OffsetYOrigin, location.Y);
						}
						else
						{
							positionY = location.Y;
						}
						break;
					case OperationActionEnum.DrawEllipseXY:
					case OperationActionEnum.DrawLineXY:
					case OperationActionEnum.DrawRectangleXY:
					case OperationActionEnum.FillEllipseXY:
					case OperationActionEnum.FillRectangleXY:
						//	StartOffsetX, StartOffsetY.
						//	X.
						if(operation.StartOffsetX.Length > 0 ||
							operation.StartOffsetXOrigin != OffsetLeftRightEnum.None)
						{
							//	X was specified.
							positionX = GetMillimeters(operation.StartOffsetX);
							positionX = TranslateOffset(workpiece.Area, positionX,
								operation.StartOffsetXOrigin, location.X);
						}
						else
						{
							positionX = location.X;
						}
						//	Y.
						if(operation.StartOffsetY.Length > 0 ||
							operation.StartOffsetYOrigin != OffsetTopBottomEnum.None)
						{
							//	Y was specified.
							positionY = GetMillimeters(operation.StartOffsetY);
							positionY = TranslateOffset(workpiece.Area, positionY,
								operation.StartOffsetYOrigin, location.Y);
						}
						else
						{
							positionY = location.Y;
						}
						break;
				}
				//	Start offset X and Y have been set, where appropriate.
				//	Update the location from the starting point.
				result.X = positionX;
				result.Y = positionY;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetSystemSize																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the system size of the workspace.
		/// </summary>
		/// <returns>
		/// The system workspace size for the currently active profile.
		/// </returns>
		public static SizeF GetSystemSize()
		{
			string unit = BaseUnit(mConfigProfile.DisplayUnits);

			SizeF result = new SizeF(
				MeasurementCollection.SumMillimeters(mConfigProfile.XDimension, unit),
				MeasurementCollection.SumMillimeters(mConfigProfile.YDimension, unit));
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetToolNames																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return an array of tool names defined for this session.
		/// </summary>
		/// <returns>
		/// Reference to the string array containing the names of all of the
		/// currently defined tools, if found. Otherwise, an empty array.
		/// </returns>
		public static string[] GetToolNames()
		{
			List<string> names = null;
			string[] result = new string[0];


			if(mConfigProfile.UserTools.Count > 0)
			{
				names = UserToolCollection.Clone(mConfigProfile.UserTools).
					OrderBy(x => x.ToolName).Select(y => y.ToolName).ToList();
				result = names.ToArray();
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetValue																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the value of the specified group member in the provided match.
		/// </summary>
		/// <param name="match">
		/// Reference to the match to be inspected.
		/// </param>
		/// <param name="groupName">
		/// Name of the group for which the value will be found.
		/// </param>
		/// <returns>
		/// The value found in the specified group, if found. Otherwise, empty
		/// string.
		/// </returns>
		public static string GetValue(Match match, string groupName)
		{
			string result = "";

			if(match != null && match.Groups[groupName] != null &&
				match.Groups[groupName].Value != null)
			{
				result = match.Groups[groupName].Value;
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the value of the specified group member in a match found with
		/// the provided source and pattern.
		/// </summary>
		/// <param name="source">
		/// Source string to search.
		/// </param>
		/// <param name="pattern">
		/// Regular expression pattern to apply.
		/// </param>
		/// <param name="groupName">
		/// Name of the group for which the value will be found.
		/// </param>
		/// <returns>
		/// The value found in the specified group, if found. Otherwise, empty
		/// string.
		/// </returns>
		public static string GetValue(string source, string pattern,
			string groupName)
		{
			Match match = null;
			string result = "";

			if(source?.Length > 0 && pattern?.Length > 0 && groupName?.Length > 0)
			{
				match = Regex.Match(source, pattern);
				if(match.Success)
				{
					result = GetValue(match, groupName);
				}
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Return the value associated with the specified property name of the
		/// provided pattern operation.
		/// </summary>
		/// <param name="type">
		/// Reference to the type for which the property value will be retrieved.
		/// </param>
		/// <param name="instance">
		/// Reference to the instance of the object from which to retrieve the
		/// property.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to retrieve.
		/// </param>
		/// <returns>
		/// Value of the specified property on the provided pattern operation
		/// item, if found. Otherwise, an empty string.
		/// </returns>
		public static object GetValue(Type type, object instance,
			string propertyName)
		{
			PropertyInfo property = null;
			object result = null;
			object item = null;

			if(type != null && propertyName?.Length > 0)
			{
				try
				{
					property = type.GetProperty(propertyName,
						System.Reflection.BindingFlags.GetProperty |
						System.Reflection.BindingFlags.IgnoreCase |
						System.Reflection.BindingFlags.Instance |
						System.Reflection.BindingFlags.Public);
					if(property != null)
					{
						item = property.GetValue(instance);
					}
					if(item != null)
					{
						result = item;
					}
				}
				catch { }
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetWorkspaceArea																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a reference to the currently configured workspace area for this
		/// configuration.
		/// </summary>
		/// <returns>
		/// Reference to the established workspace area, where the top and left
		/// coordinates are aligned with the top left visual corner of the display.
		/// </returns>
		public static FArea GetWorkspaceArea()
		{
			float height = GetMillimeters(mConfigProfile.YDimension);
			//float offset = 0f;
			FArea result = null;
			float width = GetMillimeters(mConfigProfile.XDimension);

			result = new FArea(0f, 0f, width, height);

			//if(width > 0f)
			//{
			//	switch(mConfigProfile.XYOrigin)
			//	{
			//		case OriginLocationEnum.BottomLeft:
			//		case OriginLocationEnum.Left:
			//		case OriginLocationEnum.TopLeft:
			//			//	Origin is on the left side of visual space.
			//			result.Left = 0f;
			//			offset = width;
			//			if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
			//			{
			//				offset *= -1f;
			//			}
			//			result.Right = offset;
			//			break;
			//		case OriginLocationEnum.Bottom:
			//		case OriginLocationEnum.Center:
			//		case OriginLocationEnum.None:
			//		case OriginLocationEnum.Top:
			//			//	Origin is in the center of the visual space.
			//			offset = width / 2f;
			//			if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
			//			{
			//				offset *= -1f;
			//			}
			//			result.Left = 0f - offset;
			//			result.Right = offset;
			//			break;
			//		case OriginLocationEnum.BottomRight:
			//		case OriginLocationEnum.Right:
			//		case OriginLocationEnum.TopRight:
			//			//	Origin is on the right side of visual space.
			//			result.Right = 0f;
			//			offset = width;
			//			if(mConfigProfile.TravelX == DirectionLeftRightEnum.Right)
			//			{
			//				offset *= -1f;
			//			}
			//			result.Left = offset;
			//			break;
			//	}
			//}
			//if(height > 0f)
			//{
			//	switch(mConfigProfile.XYOrigin)
			//	{
			//		case OriginLocationEnum.Bottom:
			//		case OriginLocationEnum.BottomLeft:
			//		case OriginLocationEnum.BottomRight:
			//			//	Origin is at the bottom of visual space.
			//			result.Bottom = 0f;
			//			offset = height;
			//			if(mConfigProfile.TravelY == DirectionUpDownEnum.Down)
			//			{
			//				offset *= -1f;
			//			}
			//			result.Top = offset;
			//			break;
			//		case OriginLocationEnum.Center:
			//		case OriginLocationEnum.Left:
			//		case OriginLocationEnum.None:
			//		case OriginLocationEnum.Right:
			//			//	Origin is at the center of visual space.
			//			offset = height / 2f;
			//			if(mConfigProfile.TravelY == DirectionUpDownEnum.Up)
			//			{
			//				offset *= -1f;
			//			}
			//			result.Top = 0f - offset;
			//			result.Bottom = offset;
			//			break;
			//		case OriginLocationEnum.Top:
			//		case OriginLocationEnum.TopLeft:
			//		case OriginLocationEnum.TopRight:
			//			//	Origin is at the top of visual space.
			//			result.Top = 0f;
			//			offset = height;
			//			if(mConfigProfile.TravelY == DirectionUpDownEnum.Up)
			//			{
			//				offset *= -1f;
			//			}
			//			result.Bottom = offset;
			//			break;
			//	}
			//}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	GetWorkspaceRatio																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the ratio of the current workspace's width to its height.
		/// </summary>
		public static float GetWorkspaceRatio()
		{
			float height = 0f;
			MeasurementCollection measurements = null;
			float result = 0f;
			float width = 0f;

			measurements =
				MeasurementCollection.GetMeasurements(mConfigProfile.XDimension,
					BaseUnit(mConfigProfile.DisplayUnits));
			width = measurements.SumMillimeters();
			measurements =
				MeasurementCollection.GetMeasurements(mConfigProfile.YDimension,
					BaseUnit(mConfigProfile.DisplayUnits));
			height = measurements.SumMillimeters();

			if(height != 0f)
			{
				result = width / height;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* HasFraction																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the caller's specified value contains
		/// a fraction.
		/// </summary>
		/// <param name="value">
		/// The value to inspect for fractions.
		/// </param>
		/// <returns>
		/// True if the value contains a fraction. Otherwise, false.
		/// </returns>
		public static bool HasFraction(string value)
		{
			return GetValue(value, ResourceMain.rxFractional, "fraction").Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* HasProperty																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the provided type has a public
		/// property with the specified name.
		/// </summary>
		/// <param name="type">
		/// Reference to the object type to consider.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property to search for.
		/// </param>
		/// <returns>
		/// True if a property exists in this class with the specified name.
		/// Otherwise, false.
		/// </returns>
		public static bool HasProperty(Type type, string propertyName)
		{
			bool result = false;

			if(type != null && propertyName?.Length > 0)
			{
				try
				{
					result = (type.GetProperty(propertyName,
						System.Reflection.BindingFlags.GetProperty |
						System.Reflection.BindingFlags.IgnoreCase |
						System.Reflection.BindingFlags.Instance |
						System.Reflection.BindingFlags.Public) != null);
				}
				catch { }
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* InitializeDefaultProfile																							*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Initialize the default configuration profile for this session.
		///// </summary>
		//public static void InitializeDefaultProfile()
		//{
		//	string content = "";

		//	try
		//	{
		//		content = File.ReadAllText("ShopToolsConfig.json");
		//		mConfigProfile =
		//			JsonConvert.DeserializeObject<ShopToolsConfigItem>(content);
		//	}
		//	catch { }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* IsXFeed																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the workspace is primarily viewed as
		/// X-feed, rather than Y-feed.
		/// </summary>
		/// <returns>
		/// Value indicating whether the router workspace is primarily considered
		/// to be an X-feed space.
		/// </returns>
		public static bool IsXFeed()
		{
			float height = 0f;
			bool result = false;
			float width = 0f;

			width = MeasurementCollection.SumMillimeters(mConfigProfile.XDimension,
				BaseUnit(mConfigProfile.DisplayUnits));
			height = MeasurementCollection.SumMillimeters(mConfigProfile.YDimension,
				BaseUnit(mConfigProfile.DisplayUnits));

			result = (width >= height &&
				(mConfigProfile.AxisXIsOpenEnded || !mConfigProfile.AxisYIsOpenEnded));
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	OffsetArea																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a rectangle whose area has been offset by the specified amount
		/// from the source.
		/// </summary>
		/// <param name="source">
		/// The source rectangle to reference.
		/// </param>
		/// <param name="offsetX">
		/// The horizontal amount by which to offset the new area from the source.
		/// </param>
		/// <param name="offsetY">
		/// The vertical amount by which to offset the new area from the source.
		/// </param>
		/// <returns>
		/// New rectangle that has been offset by the specified horizontal and
		/// vertical amounts from the source.
		/// </returns>
		public static Rectangle OffsetArea(Rectangle source, int offsetX,
			int offsetY)
		{
			Rectangle result =
				new Rectangle(
					source.X + offsetX, source.Y + offsetY,
					source.Width, source.Height);
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ReadConfiguration																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Read the configuration file.
		/// </summary>
		public static void ReadConfiguration()
		{
			List<string> allOperationActions =
				Enum.GetNames(typeof(OperationActionEnum)).ToList();
			string content = "";
			string filename = "";
			List<OperationActionPropertyItem> operationActionProperties = null;
			ToolTypeDefinitionCollection toolTypeDefinitions = null;

			if(!(mConfigurationFilename?.Length > 0))
			{
				//	The configuration filename has not yet been set. Use the app
				//	directory.
				mConfigurationFilename = Path.Combine(
					AppDomain.CurrentDomain.BaseDirectory, "ShopToolsConfig.json");
			}
			if(Path.Exists(mConfigurationFilename))
			{
				content = File.ReadAllText(mConfigurationFilename);
				try
				{
					mConfigProfile =
						JsonConvert.DeserializeObject<ShopToolsConfigItem>(content);
				}
				catch(Exception ex)
				{
					MessageBox.Show($"Error loading configuration file: {ex.Message}",
						"Read Configuration File");
					System.Environment.Exit(1);
				}
			}
			if(mConfigProfile == null)
			{
				mConfigProfile = new ShopToolsConfigItem();
			}
			//	Tool type definitions.
			filename =
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ToolTypes.json");
			if(Path.Exists(filename))
			{
				content = File.ReadAllText(filename);
				try
				{
					toolTypeDefinitions =
						JsonConvert.DeserializeObject<ToolTypeDefinitionCollection>(
							content);
					mConfigProfile.ToolTypeDefinitions.AddRange(toolTypeDefinitions);
				}
				catch(Exception ex)
				{
					MessageBox.Show($"Error loading tool definition file: {ex.Message}",
						"Read Tool Definition File");
					System.Environment.Exit(1);
				}
			}
			//	Operation action properties.
			filename =
				Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
				"OperationActionProperties.json");
			if(Path.Exists(filename))
			{
				content = File.ReadAllText(filename);
				try
				{
					operationActionProperties =
						JsonConvert.DeserializeObject<OperationActionPropertyCollection>(
							content);
					//	Sort the properties by their user order.
					operationActionProperties =
						operationActionProperties.OrderBy(x => x.SortIndex).ToList();
					//	Process the include / exclude lists on each item.
					foreach(OperationActionPropertyItem propertyItem in
						operationActionProperties)
					{
						if(propertyItem.Internal)
						{
							//	An internal property doesn't autofill anywhere.
							propertyItem.IncludeOperationActions.Clear();
							propertyItem.ExcludeOperationActions.Clear();
						}
						else
						{
							//	If no included items have been specified, then include all.
							if(propertyItem.IncludeOperationActions.Count == 0)
							{
								propertyItem.IncludeOperationActions.
									AddRange(allOperationActions);
							}
							//	Remove any excluded operations.
							foreach(string entryItem in propertyItem.ExcludeOperationActions)
							{
								propertyItem.IncludeOperationActions.Remove(entryItem);
							}
						}
					}
					//	Add the prepared list to the session configuration.
					mConfigProfile.OperationActionProperties.AddRange(
						operationActionProperties);
				}
				catch(Exception ex)
				{
					MessageBox.Show(
						$"Error operation property definition file: {ex.Message}",
						"Read Operation Property Definition File");
					System.Environment.Exit(1);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ResizeArea																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a rectangle of the specified size ratio that perfectly fits
		/// into the caller's space.
		/// </summary>
		/// <param name="targetWidth">
		/// The width of the target area to cover.
		/// </param>
		/// <param name="targetHeight">
		/// The height of the target area to cover.
		/// </param>
		/// <param name="widthToHeightRatio">
		/// The Width to Height ratio of the new rectangle.
		/// </param>
		/// <returns>
		/// Size of the rectangle that will tightly fit into the specified
		/// area with the given width to height ratio.
		/// </returns>
		public static Size ResizeArea(int targetWidth, int targetHeight,
			float widthToHeightRatio)
		{
			double nHeight = 0d;
			double nWidth = 0d;
			Size result = new Size();
			double tHeight = (double)targetHeight;
			double tWidth = (double)targetWidth;
			double whRatio = (double)widthToHeightRatio;

			if(tHeight != 0d && tWidth / tHeight > widthToHeightRatio)
			{
				nHeight = tHeight;
				nWidth = nHeight * whRatio;
			}
			else if(whRatio != 0d)
			{
				nWidth = tWidth;
				nHeight = nWidth / whRatio;
			}
			result.Width = (int)nWidth;
			result.Height = (int)nHeight;
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ResizeImage																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Resize the image to the specified width and height.
		/// </summary>
		/// <param name="image">
		/// The image to resize.
		/// </param>
		/// <param name="width">
		/// The width to resize to.
		/// </param>
		/// <param name="height">
		/// The height to resize to.
		/// </param>
		/// <returns>
		/// The resized image.
		/// </returns>
		public static Bitmap ResizeImage(Image image, int width, int height)
		{
			Rectangle dRect = new Rectangle(0, 0, width, height);
			Bitmap dImage = new Bitmap(width, height);

			dImage.SetResolution(image.HorizontalResolution,
				image.VerticalResolution);

			using(Graphics graphics = Graphics.FromImage(dImage))
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				using(ImageAttributes wrapMode = new ImageAttributes())
				{
					wrapMode.SetWrapMode(WrapMode.TileFlipXY);
					graphics.DrawImage(image, dRect,
						0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
				}
			}

			return dImage;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ResolveDepth																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the desired depth for the specified action.
		/// </summary>
		/// <param name="operation">
		/// Reference to the operation for which the depth will be checked.
		/// </param>
		/// <returns>
		/// The depth of the operation, if specified, or the thickness of the
		/// defined workpiece, if not specified.
		/// </returns>
		public static float ResolveDepth(PatternOperationItem operation)
		{
			float result = 0f;

			if(operation != null)
			{
				if(operation.Depth.Length == 0)
				{
					//	If no depth was specified, the full depth is assumed.
					result = mSessionWorkpieceInfo.Thickness;
				}
				else
				{
					result = GetMillimeters(operation.Depth);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SelectItem																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// The select the first specified item in the provided list by item text.
		/// </summary>
		/// <param name="comboBox">
		/// Reference to the combobox to search.
		/// </param>
		/// <param name="text">
		/// The text to search for.
		/// </param>
		/// <param name="caseSensitive">
		/// Optional value indicating whether to perform a case-sensitive search.
		/// Default = false.
		/// </param>
		public static void SelectItem(ComboBox comboBox, string text,
			bool caseSensitive = false)
		{
			int count = 0;
			int index = 0;
			string ltext = "";
			object item = null;

			if(comboBox?.Items.Count > 0)
			{
				//	Deselect by default.
				comboBox.SelectedItem = null;
				if(!caseSensitive && text?.Length > 0)
				{
					ltext = text.ToLower();
				}
				count = comboBox.Items.Count;
				for(index = 0; index < count; index ++)
				{
					item = comboBox.Items[index];
					if(item != null)
					{
						if(caseSensitive)
						{
							if(text == item.ToString())
							{
								comboBox.SelectedItem = item;
								break;
							}
						}
						else
						{
							if(ltext == item.ToString().ToLower())
							{
								comboBox.SelectedItem = item;
								break;
							}
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SessionConverter																											*
		//*-----------------------------------------------------------------------*
		private static ConversionCalc.Converter mSessionConverter =
			new ConversionCalc.Converter();
		/// <summary>
		/// Get a reference to the singleton converter for this session.
		/// </summary>
		public static ConversionCalc.Converter SessionConverter
		{
			get { return mSessionConverter; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SessionWorkpieceInfo																									*
		//*-----------------------------------------------------------------------*
		private static WorkpieceInfoItem mSessionWorkpieceInfo =
			new WorkpieceInfoItem();
		/// <summary>
		/// Get/Set a reference to the session workpiece information.
		/// </summary>
		public static WorkpieceInfoItem SessionWorkpieceInfo
		{
			get { return mSessionWorkpieceInfo; }
			set { mSessionWorkpieceInfo = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToBool																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Provide fail-safe conversion of string to boolean value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Boolean value. False if not convertible.
		/// </returns>
		public static bool ToBool(object value)
		{
			bool result = false;
			if(value != null)
			{
				result = ToBool(value.ToString());
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Provide fail-safe conversion of string to boolean value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <param name="defaultValue">
		/// The default value to return if the value was not present.
		/// </param>
		/// <returns>
		/// Boolean value. False if not convertible.
		/// </returns>
		public static bool ToBool(string value, bool defaultValue = false)
		{
			//	A try .. catch block was originally implemented here, but the
			//	following text was being sent to output on each unsuccessful
			//	match.
			//	Exception thrown: 'System.FormatException' in mscorlib.dll
			bool result = false;

			if(value?.Length > 0)
			{
				if(!bool.TryParse(value, out result))
				{
					Debug.WriteLine($"Error on ToBool");
				}
			}
			else
			{
				result = defaultValue;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToFloat																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Floating point value. 0 if not convertible.
		/// </returns>
		public static float ToFloat(object value)
		{
			float result = 0f;
			if(value != null)
			{
				result = ToFloat(value.ToString());
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Floating point value. 0 if not convertible.
		/// </returns>
		/// <remarks>
		/// This version supports fractions.
		/// </remarks>
		public static float ToFloat(string value)
		{
			bool bIsNegative = false;
			int denominator = 0;
			Match match = null;
			int numerator = 0;
			float result = 0f;
			int whole = 0;

			try
			{
				if(HasFraction(value))
				{
					match = Regex.Match(value, ResourceMain.rxFractional);
					whole = ToInt(GetValue(match, "whole"));
					numerator = ToInt(GetValue(match, "numerator"));
					denominator = ToInt(GetValue(match, "denominator"));
					bIsNegative = GetValue(match, "fractional").StartsWith("-");
					result = (float)whole;
					if(denominator > 0)
					{
						result += (float)numerator / (float)denominator;
					}
					if(bIsNegative)
					{
						result = 0f - result;
					}
				}
				else
				{
					result = float.Parse(value);
				}
			}
			catch { }
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToInt																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Int32 value. 0 if not convertible.
		/// </returns>
		public static int ToInt(object value)
		{
			int result = 0;
			if(value != null)
			{
				result = ToInt(value.ToString());
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Provide fail-safe conversion of string to numeric value.
		/// </summary>
		/// <param name="value">
		/// Value to convert.
		/// </param>
		/// <returns>
		/// Int32 value. 0 if not convertible.
		/// </returns>
		public static int ToInt(string value)
		{
			int result = 0;
			try
			{
				result = int.Parse(value);
			}
			catch { }
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToMultiLineString																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the multi-line single string representation of the items found
		/// in the caller's list, where lines that end with a space are continued
		/// without a line-break and all other lines are ended with a line break.
		/// </summary>
		/// <param name="list">
		/// Reference to the list of string values to be converted to a single
		/// string.
		/// </param>
		/// <returns>
		/// A single multi-line representation of the caller's string list.
		/// </returns>
		public static string ToMultiLineString(List<string> list)
		{
			StringBuilder builder = new StringBuilder();

			if(list?.Count > 0)
			{
				foreach(string listItem in list)
				{
					if(listItem.EndsWith(' '))
					{
						builder.Append(listItem);
					}
					else
					{
						builder.AppendLine(listItem);
					}
				}
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransformFromAbsolute																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a point on the top, left working space that corresponds to an
		/// absolute coordinate on the defined physical table.
		/// </summary>
		/// <param name="absolutePoint">
		/// The absolute point to transform.
		/// </param>
		/// <returns>
		/// Reference to the point corresponding to the caller's absolute
		/// coordinate.
		/// </returns>
		public static FPoint TransformFromAbsolute(FPoint absolutePoint)
		{
			float height = GetMillimeters(mConfigProfile.YDimension);
			FPoint result = new FPoint();
			FVector2 scale = new FVector2();
			FVector2 target = null;
			FVector2 translation = new FVector2();
			float width = GetMillimeters(mConfigProfile.XDimension);

			if(absolutePoint != null)
			{
				if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
				{
					//	Flip X axis.
					scale.Values[0] = -1f;
				}
				else
				{
					//	Normal X axis.
					scale.Values[0] = 1f;
				}
				if(mConfigProfile.TravelY == DirectionUpDownEnum.Up)
				{
					//	Flip Y axis.
					scale.Values[1] = -1f;
				}
				else
				{
					//	Normal Y axis.
					scale.Values[1] = 1f;
				}
				target = FMatrix3.Scale((FVector2)absolutePoint, scale);
				switch(mConfigProfile.XYOrigin)
				{
					case OriginLocationEnum.Bottom:
						//	Bottom center to top left.
						translation.Values[0] = width / 2f;
						translation.Values[1] = height;
						break;
					case OriginLocationEnum.BottomLeft:
						//	Bottom left to top left.
						translation.Values[0] = 0f;
						translation.Values[1] = height;
						break;
					case OriginLocationEnum.BottomRight:
						//	Bottom right to top left.
						translation.Values[0] = width;
						translation.Values[1] = height;
						break;
					case OriginLocationEnum.Center:
						//	Center center to top left.
						translation.Values[0] = width / 2f;
						translation.Values[1] = height / 2f;
						break;
					case OriginLocationEnum.Left:
						//	Center left to top left.
						translation.Values[0] = 0f;
						translation.Values[1] = height / 2f;
						break;
					case OriginLocationEnum.Right:
						//	Center right to top left.
						translation.Values[0] = width;
						translation.Values[1] = height / 2f;
						break;
					case OriginLocationEnum.Top:
						//	Top center to top left.
						translation.Values[0] = width / 2f;
						translation.Values[1] = 0f;
						break;
					case OriginLocationEnum.TopLeft:
					case OriginLocationEnum.None:
						break;
					case OriginLocationEnum.TopRight:
						//	Top right to top left.
						translation.Values[0] = width;
						translation.Values[1] = 0f;
						break;
				}
				if(translation.Values[0] != 0f ||
					translation.Values[1] != 0f)
				{
					target = FMatrix3.Translate(target, translation);
				}
				result = target;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransformToAbsolute																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a point on the physical working space that corresponds to a
		/// display-based coordinate.
		/// </summary>
		/// <param name="displayPoint">
		/// The display point to transform, based on top, left anchor.
		/// </param>
		/// <returns>
		/// Reference to the absolute point corresponding to the caller's display
		/// coordinate.
		/// </returns>
		public static FPoint TransformToAbsolute(FPoint displayPoint)
		{
			bool bFlipX = false;
			bool bFlipY = false;
			float height = GetMillimeters(mConfigProfile.YDimension);
			FPoint result = new FPoint();
			FVector2 scale = new FVector2();
			FVector2 target = null;
			FVector2 translation = new FVector2();
			float width = GetMillimeters(mConfigProfile.XDimension);

			if(displayPoint != null)
			{
				if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
				{
					//	Flip X axis.
					bFlipX = true;
					scale.Values[0] = -1f;
				}
				else
				{
					//	Normal X axis.
					scale.Values[0] = 1f;
				}
				if(mConfigProfile.TravelY == DirectionUpDownEnum.Up)
				{
					//	Flip Y axis.
					bFlipY = true;
					scale.Values[1] = -1f;
				}
				else
				{
					//	Normal Y axis.
					scale.Values[1] = 1f;
				}
				target = FMatrix3.Scale((FVector2)displayPoint, scale);
				switch(mConfigProfile.XYOrigin)
				{
					case OriginLocationEnum.Bottom:
						//	Top left to Bottom center.
						//	TODO: Check top left to bottom center.
						translation.Values[0] = 0f - (width / 2f);
						translation.Values[1] = 0f - height;
						break;
					case OriginLocationEnum.BottomLeft:
						//	Bottom left to top left.
						//	TODO: Check top left to bottom left.
						translation.Values[0] = 0f;
						translation.Values[1] = 0f - height;
						break;
					case OriginLocationEnum.BottomRight:
						//	Top left to bottom right.
						translation.Values[0] =
							(bFlipX ? width : 0f - width);
						translation.Values[1] =
							(bFlipY ? height : 0f - height);
						break;
					case OriginLocationEnum.Center:
						//	Top left to center center.
						//	TODO: Check top left to center center.
						translation.Values[0] =
							(bFlipX ? width / 2f : 0f - (width / 2f));
						translation.Values[1] =
							(bFlipY ? height / 2f : 0f - (height / 2f));
						break;
					case OriginLocationEnum.Left:
						//	Center left to top left.
						//	TODO: Check top left to center left.
						translation.Values[0] = 0f;
						translation.Values[1] = 0f - (height / 2f);
						break;
					case OriginLocationEnum.Right:
						//	Center right to top left.
						//	TODO: Check top left to center right.
						translation.Values[0] = 0f - width;
						translation.Values[1] = 0f - (height / 2f);
						break;
					case OriginLocationEnum.Top:
						//	Top center to top left.
						//	TODO: Check top left to top center.
						translation.Values[0] = 0f - (width / 2f);
						translation.Values[1] = 0f;
						break;
					case OriginLocationEnum.TopLeft:
					case OriginLocationEnum.None:
						break;
					case OriginLocationEnum.TopRight:
						//	Top right to top left.
						//	TODO: Check top left to top right.
						translation.Values[0] = 0f - width;
						translation.Values[1] = 0f;
						break;
				}
				if(translation.Values[0] != 0f ||
					translation.Values[1] != 0f)
				{
					target = FMatrix3.Translate(target, translation);
				}
				result = target;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransformToDisplay																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the display area of the workpiece, given the established
		/// </summary>
		/// <param name="workspace">
		/// The display workspace.
		/// </param>
		/// <param name="workpiece">
		/// Reference to the workpiece information currently being considered.
		/// </param>
		/// <returns>
		/// Reference to the display area of the workpiece, if legitimate.
		/// Otherwise, an empty area.
		/// </returns>
		public static Rectangle TransformToDisplay(Rectangle workspace,
			WorkpieceInfoItem workpiece)
		{
			Rectangle result = Rectangle.Empty;
			float scale = 1f;
			FPoint translate = null;

			if(!workspace.IsEmpty && workpiece != null)
			{
				translate = new FPoint(0f - workpiece.WorkspaceArea.Left,
					0f - workpiece.WorkspaceArea.Top);
				if((float)workspace.Width != Math.Abs(workpiece.WorkspaceArea.Width))
				{
					scale =
						(float)workspace.Width / Math.Abs(workpiece.WorkspaceArea.Width);
				}
				result = new Rectangle(
					workspace.X + (int)((workpiece.Area.Left + translate.X) * scale),
					workspace.Y + (int)((workpiece.Area.Top + translate.Y) * scale),
					(int)(workpiece.Area.Width * scale),
					(int)(workpiece.Area.Height * scale));
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TranslateDirection																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Translate the relative directional offset from the visual perspective
		/// to the defined direction on the table.
		/// </summary>
		/// <param name="offset">
		/// The offset to polarize.
		/// </param>
		/// <param name="directionType">
		/// The visual relative direction with which the offset is associated.
		/// </param>
		/// <returns>
		/// The physical offset, relative to the established direction of travel
		/// on the table.
		/// </returns>
		public static float TranslateDirection(float offset,
			DirectionLeftRightEnum directionType)
		{
			float result = offset;

			if(directionType == DirectionLeftRightEnum.Left)
			{
				//	Opposite direction.
				result *= -1f;
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Translate the relative directional offset from the visual perspective
		/// to the defined direction on the table.
		/// </summary>
		/// <param name="offset">
		/// The offset to polarize.
		/// </param>
		/// <param name="offsetType">
		/// The visual relative direction with which the offset is associated.
		/// </param>
		/// <returns>
		/// The physical offset, relative to the established direction of travel
		/// on the table.
		/// </returns>
		public static float TranslateDirection(float offset,
			DirectionUpDownEnum offsetType)
		{
			float result = offset;
			if(offsetType == DirectionUpDownEnum.Up)
			{
				//	Opposite direction.
				result *= -1f;
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TranslateOffset																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Translate the relative positional offset from the visual perspective
		/// to absolute positions based upon the physical anchors on the table or
		/// material.
		/// </summary>
		/// <param name="area">
		/// Reference to the area from which the offset is relative. This
		/// value represents physical coordinates.
		/// </param>
		/// <param name="offset">
		/// The offset to polarize.
		/// </param>
		/// <param name="offsetType">
		/// The visual relative direction with which the offset is associated.
		/// </param>
		/// <param name="relativeOffset">
		/// A base offset for the entire space.
		/// </param>
		/// <returns>
		/// The physical offset, relative to the established direction of travel
		/// on the table.
		/// </returns>
		/// <remarks>
		/// This method assumes that the left and top positions constitute the
		/// local anchor of the area.
		/// </remarks>
		public static float TranslateOffset(FArea area, float offset,
			OffsetLeftRightEnum offsetType, float relativeOffset = float.MinValue)
		{
			return TranslateOffset(area, 0f, offset, offsetType, relativeOffset);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Translate the relative positional offset from the visual perspective
		/// to absolute positions based upon the physical anchors on the table or
		/// material.
		/// </summary>
		/// <param name="area">
		/// Reference to the area from which the offset is relative. This
		/// value represents physical coordinates.
		/// </param>
		/// <param name="offset">
		/// The offset to polarize.
		/// </param>
		/// <param name="offsetType">
		/// The visual relative direction with which the offset is associated.
		/// </param>
		/// <param name="relativeOffset">
		/// A base offset for the entire space.
		/// </param>
		/// <returns>
		/// The physical offset, relative to the established direction of travel
		/// on the table.
		/// </returns>
		/// <remarks>
		/// This method assumes that the left and top positions constitute the
		/// local anchor of the area.
		/// </remarks>
		public static float TranslateOffset(FArea area, float offset,
			OffsetTopBottomEnum offsetType, float relativeOffset = float.MinValue)
		{
			return TranslateOffset(area, 0f, offset, offsetType, relativeOffset);
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Translate the relative positional offset from the visual perspective
		/// to absolute positions based upon the physical anchors on the table or
		/// material.
		/// </summary>
		/// <param name="area">
		/// Reference to the area from which the offset is relative. This
		/// value represents physical coordinates.
		/// </param>
		/// <param name="subjectHeight">
		/// Height of the subject being positioned.
		/// </param>
		/// <param name="offset">
		/// The offset to polarize.
		/// </param>
		/// <param name="offsetType">
		/// The visual relative direction with which the offset is associated.
		/// </param>
		/// <param name="relativeOffset">
		/// A base offset for the entire space.
		/// </param>
		/// <returns>
		/// The physical offset, relative to the established direction of travel
		/// on the table.
		/// </returns>
		/// <remarks>
		/// This method assumes that the left and top positions constitute the
		/// local anchor of the area.
		/// </remarks>
		public static float TranslateOffset(FArea area, float subjectHeight,
			float offset, OffsetTopBottomEnum offsetType,
			float relativeOffset = float.MinValue)
		{
			float result = offset;

			if(area != null)
			{
				switch(offsetType)
				{
					case OffsetTopBottomEnum.Absolute:
						result = TransformFromAbsolute(new FPoint(0f, offset)).Y;
						break;
					case OffsetTopBottomEnum.Bottom:
						result = area.Bottom - subjectHeight +
							TranslateDirection(offset, mConfigProfile.TravelY);
						break;
					case OffsetTopBottomEnum.Center:
						result = (area.Top + (area.Height / 2f) +
							TranslateDirection(offset, mConfigProfile.TravelY)) -
							(subjectHeight / 2f);
						break;
					case OffsetTopBottomEnum.BottomEdgeToCenter:
						result = (area.Top + (area.Height / 2f) +
							TranslateDirection(offset, mConfigProfile.TravelY)) -
							subjectHeight;
						break;
					case OffsetTopBottomEnum.TopEdgeToCenter:
						result = area.Top + (area.Height / 2f) +
							TranslateDirection(offset, mConfigProfile.TravelY);
						break;
					case OffsetTopBottomEnum.None:
					case OffsetTopBottomEnum.Relative:
						//	Relative to the area.
						if(relativeOffset == float.MinValue)
						{
							result = area.Top +
								TranslateDirection(offset, mConfigProfile.TravelY);
						}
						else
						{
							//	Relative offset specified.
							result = relativeOffset +
								TranslateDirection(offset, mConfigProfile.TravelY);
						}
						break;
					case OffsetTopBottomEnum.Top:
						result = area.Top +
							TranslateDirection(offset, mConfigProfile.TravelY);
						break;
				}
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		/// <summary>
		/// Translate the relative positional offset from the visual perspective
		/// to absolute positions based upon the physical anchors on the table or
		/// material.
		/// </summary>
		/// <param name="area">
		/// Reference to the area from which the offset is relative. This
		/// value represents physical coordinates.
		/// </param>
		/// <param name="subjectWidth">
		/// Width of the subject being positioned.
		/// </param>
		/// <param name="offset">
		/// The offset to polarize.
		/// </param>
		/// <param name="offsetType">
		/// The visual relative direction with which the offset is associated.
		/// </param>
		/// <param name="relativeOffset">
		/// A base offset for the entire space.
		/// </param>
		/// <returns>
		/// The physical offset, relative to the established direction of travel
		/// on the table.
		/// </returns>
		/// <remarks>
		/// This method assumes that the left and top positions constitute the
		/// local anchor of the area.
		/// </remarks>
		public static float TranslateOffset(FArea area, float subjectWidth,
			float offset, OffsetLeftRightEnum offsetType,
			float relativeOffset = float.MinValue)
		{
			float result = offset;

			if(area != null)
			{
				switch(offsetType)
				{
					case OffsetLeftRightEnum.Absolute:
						result = TransformFromAbsolute(new FPoint(offset, 0f)).X;
						break;
					case OffsetLeftRightEnum.Center:
						result = (area.Left + (area.Width / 2f) +
							TranslateDirection(offset, mConfigProfile.TravelX)) -
							(subjectWidth / 2f);
						break;
					case OffsetLeftRightEnum.LeftEdgeToCenter:
						result = area.Left + (area.Width / 2f) +
							TranslateDirection(offset, mConfigProfile.TravelX);
						break;
					case OffsetLeftRightEnum.RightEdgeToCenter:
						result = (area.Left + (area.Width / 2f) +
							TranslateDirection(offset, mConfigProfile.TravelX)) -
							subjectWidth;
						break;
					case OffsetLeftRightEnum.Left:
						result = area.Left +
							TranslateDirection(offset, mConfigProfile.TravelX);
						break;
					case OffsetLeftRightEnum.None:
					case OffsetLeftRightEnum.Relative:
						//	Relative to the area.
						if(relativeOffset == float.MinValue)
						{
							result = area.Left +
								TranslateDirection(offset, mConfigProfile.TravelX);
						}
						else
						{
							//	Relative offset specified.
							result = relativeOffset +
								TranslateDirection(offset, mConfigProfile.TravelX);
						}
						break;
					case OffsetLeftRightEnum.Right:
						result = area.Right - subjectWidth +
							TranslateDirection(offset, mConfigProfile.TravelX);
						break;
				}
			}
			return result;
		}
		////*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
		///// <summary>
		///// Return the absolute offset of the provided point from the currently
		///// configured origin of the workspace.
		///// </summary>
		///// <param name="offset">
		///// The relative coordinate.
		///// </param>
		///// <param name="offsetXType">
		///// The type of offset represented by the X coordinate.
		///// </param>
		///// <param name="offsetYType">
		///// The type of offset represented by the Y coordinate.
		///// </param>
		///// <returns>
		///// Reference to a newly created point that represents the absolute
		///// position of the caller's relative location upon the configured
		///// workspace, if elligible. Otherwise, an empty point.
		///// </returns>
		//public static FPoint TranslateOffset(FPoint offset,
		//	OffsetLeftRightEnum offsetXType, OffsetTopBottomEnum offsetYType)
		//{
		//	float height =
		//		MeasurementCollection.SumMillimeters(mConfigProfile.YDimension,
		//			BaseUnit(mConfigProfile.DisplayUnits));
		//	FPoint result = new FPoint();
		//	float width =
		//		MeasurementCollection.SumMillimeters(mConfigProfile.XDimension,
		//			BaseUnit(mConfigProfile.DisplayUnits));
		//	float x = 0f;
		//	float y = 0f;

		//	if(offset != null)
		//	{
		//		//	X.
		//		switch(mConfigProfile.XYOrigin)
		//		{
		//			case OriginLocationEnum.BottomLeft:
		//			case OriginLocationEnum.Left:
		//			case OriginLocationEnum.TopLeft:
		//				//	The origin is at the left side of the table.
		//				switch(offsetXType)
		//				{
		//					case OffsetLeftRightEnum.Center:
		//						x = (width / 2f) + offset.X;
		//						if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
		//						{
		//							x *= -1f;
		//						}
		//						break;
		//					case OffsetLeftRightEnum.Left:
		//						x = offset.X;
		//						if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
		//						{
		//							x *= -1f;
		//						}
		//						break;
		//					case OffsetLeftRightEnum.None:
		//					case OffsetLeftRightEnum.Relative:
		//						//	Raw relative offset.
		//						x = offset.X;
		//						break;
		//					case OffsetLeftRightEnum.Right:
		//						x = width - offset.X;
		//						if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
		//						{
		//							x *= -1f;
		//						}
		//						break;
		//				}
		//				break;
		//			case OriginLocationEnum.Bottom:
		//			case OriginLocationEnum.Center:
		//			case OriginLocationEnum.None:
		//			case OriginLocationEnum.Top:
		//				//	The origin is in the center of the table.
		//				switch(offsetXType)
		//				{
		//					case OffsetLeftRightEnum.Center:
		//						x = offset.X;
		//						break;
		//					case OffsetLeftRightEnum.Left:
		//						x = (0f - (width / 2f)) + offset.X;
		//						if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
		//						{
		//							x *= -1f;
		//						}
		//						break;
		//					case OffsetLeftRightEnum.None:
		//					case OffsetLeftRightEnum.Relative:
		//						//	Raw relative offset.
		//						x = offset.X;
		//						break;
		//					case OffsetLeftRightEnum.Right:
		//						x = (width / 2f) - offset.X;
		//						if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
		//						{
		//							x *= -1f;
		//						}
		//						break;
		//				}
		//				break;
		//			case OriginLocationEnum.BottomRight:
		//			case OriginLocationEnum.Right:
		//			case OriginLocationEnum.TopRight:
		//				//	The origin is at the right side of the table.
		//				switch(offsetXType)
		//				{
		//					case OffsetLeftRightEnum.Center:
		//						x = (0f - (width / 2f)) + offset.X;
		//						if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
		//						{
		//							x *= -1f;
		//						}
		//						break;
		//					case OffsetLeftRightEnum.Left:
		//						x = (0f - width) + offset.X;
		//						if(mConfigProfile.TravelX == DirectionLeftRightEnum.Left)
		//						{
		//							x *= -1f;
		//						}
		//						break;
		//					case OffsetLeftRightEnum.None:
		//					case OffsetLeftRightEnum.Relative:
		//						//	Raw relative offset.
		//						x = offset.X;
		//						break;
		//					case OffsetLeftRightEnum.Right:
		//						x = offset.X;
		//						break;
		//				}
		//				break;
		//		}
		//		//	Y.
		//		switch(mConfigProfile.XYOrigin)
		//		{
		//			case OriginLocationEnum.Top:
		//			case OriginLocationEnum.TopLeft:
		//			case OriginLocationEnum.TopRight:
		//				//	Origin is at the top of the table.
		//				switch(offsetYType)
		//				{
		//					case OffsetTopBottomEnum.Bottom:
		//						y = height - offset.Y;
		//						if(mConfigProfile.TravelY == DirectionUpDownEnum.Up)
		//						{
		//							y *= -1f;
		//						}
		//						break;
		//					case OffsetTopBottomEnum.Center:
		//						y = (height / 2f) - offset.Y;
		//						if(mConfigProfile.TravelY == DirectionUpDownEnum.Up)
		//						{
		//							y *= -1f;
		//						}
		//						break;
		//					case OffsetTopBottomEnum.None:
		//					case OffsetTopBottomEnum.Relative:
		//						y = offset.Y;
		//						break;
		//					case OffsetTopBottomEnum.Top:
		//						y = offset.Y;
		//						break;
		//				}
		//				break;
		//			case OriginLocationEnum.Center:
		//			case OriginLocationEnum.Left:
		//			case OriginLocationEnum.None:
		//			case OriginLocationEnum.Right:
		//				//	Origin is at the center of the table.
		//				switch(offsetYType)
		//				{
		//					case OffsetTopBottomEnum.Bottom:
		//						y = (height / 2f) - offset.Y;
		//						if(mConfigProfile.TravelY == DirectionUpDownEnum.Up)
		//						{
		//							y *= -1f;
		//						}
		//						break;
		//					case OffsetTopBottomEnum.Center:
		//						y = offset.Y;
		//						break;
		//					case OffsetTopBottomEnum.None:
		//					case OffsetTopBottomEnum.Relative:
		//						y = offset.Y;
		//						break;
		//					case OffsetTopBottomEnum.Top:
		//						y = (0f - (height / 2f)) + offset.Y;
		//						if(mConfigProfile.TravelY == DirectionUpDownEnum.Up)
		//						{
		//							y *= -1f;
		//						}
		//						break;
		//				}
		//				break;
		//			case OriginLocationEnum.Bottom:
		//			case OriginLocationEnum.BottomLeft:
		//			case OriginLocationEnum.BottomRight:
		//				//	Origin is at the bottom of the table.
		//				switch(offsetYType)
		//				{
		//					case OffsetTopBottomEnum.Bottom:
		//						y = offset.Y;
		//						break;
		//					case OffsetTopBottomEnum.Center:
		//						y = (height / 2f) + offset.Y;
		//						if(mConfigProfile.TravelY == DirectionUpDownEnum.Down)
		//						{
		//							y *= -1f;
		//						}
		//						break;
		//					case OffsetTopBottomEnum.None:
		//					case OffsetTopBottomEnum.Relative:
		//						y = offset.Y;
		//						break;
		//					case OffsetTopBottomEnum.Top:
		//						y = height - offset.Y;
		//						if(mConfigProfile.TravelY == DirectionUpDownEnum.Down)
		//						{
		//							y *= -1f;
		//						}
		//						break;
		//				}
		//				break;
		//		}
		//	}
		//	if(result == null)
		//	{
		//		result = new FPoint();
		//	}
		//	return result;
		//}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* WriteConfiguration																										*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Write the contents of the configuration file.
		/// </summary>
		public static void WriteConfiguration()
		{
			string content = "";

			if(mConfigurationFilename?.Length > 0)
			{
				content = JsonConvert.SerializeObject(mConfigProfile);
				File.WriteAllText(mConfigurationFilename, content);
			}
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
