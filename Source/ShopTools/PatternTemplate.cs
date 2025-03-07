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
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	PatternTemplateCollection																								*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of PatternTemplateItem Items.
	/// </summary>
	public class PatternTemplateCollection :
		ChangeObjectCollection<PatternTemplateItem>
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
		/// Return a deep clone of the specified collection.
		/// </summary>
		/// <param name="items">
		/// Reference to the collection of template items to clone.
		/// </param>
		/// <returns>
		/// Reference to the newly created clone.
		/// </returns>
		public static PatternTemplateCollection Clone(
			PatternTemplateCollection items)
		{
			PatternTemplateCollection result = new PatternTemplateCollection();

			if(items?.Count > 0)
			{
				foreach(PatternTemplateItem templateItem in items)
				{
					result.Add(PatternTemplateItem.Clone(templateItem));
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* MoveDown																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Move the specified item down in the collection by one space, if
		/// possible.
		/// </summary>
		/// <param name="patternTemplate">
		/// Reference to the item to be moved downward in the collection.
		/// </param>
		public void MoveDown(PatternTemplateItem patternTemplate)
		{
			int index = 0;

			if(patternTemplate != null && this.Contains(patternTemplate))
			{
				index = this.IndexOf(patternTemplate);
				if(index + 1 < this.Count)
				{
					this.Remove(patternTemplate);
					this.Insert(index + 1, patternTemplate);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* MoveUp																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Move the specified item up in the collection by one space, if
		/// possible.
		/// </summary>
		/// <param name="patternTemplate">
		/// Reference to the item to be moved upward in the collection.
		/// </param>
		public void MoveUp(PatternTemplateItem patternTemplate)
		{
			int index = 0;

			if(patternTemplate != null && this.Contains(patternTemplate))
			{
				index = this.IndexOf(patternTemplate);
				if(index > 0)
				{
					this.Remove(patternTemplate);
					this.Insert(index - 1, patternTemplate);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransferValues																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Transfer clone all values from the source collection to the target.
		/// </summary>
		/// <param name="source">
		/// Reference to the source collection to be cloned.
		/// </param>
		/// <param name="target">
		/// Reference to the target collection to receive the new information.
		/// </param>
		public static void TransferValues(PatternTemplateCollection source,
			PatternTemplateCollection target)
		{
			if(source != null && target != null)
			{
				target.Clear();
				foreach(PatternTemplateItem templateItem in source)
				{
					target.Add(PatternTemplateItem.Clone(templateItem));
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	PatternTemplateItem																											*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// An individual pattern template that describes one or more predefined
	/// actions that can be applied to a piece of material.
	/// </summary>
	public class PatternTemplateItem : ChangeObjectItem
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
		/// Create a new instance of the PatternTemplateItem Item.
		/// </summary>
		public PatternTemplateItem()
		{
			mOperations = new PatternOperationCollection()
			{
				PropertyName = "Operations"
			};
			mOperations.CollectionChanged += OnCollectionChanged;
			mRemarks = new ChangeObjectCollection<string>()
			{
				PropertyName = "Remarks"
			};
			mRemarks.CollectionChanged += OnCollectionChanged;
			mSharedVariables = new ChangeObjectCollection<string>()
			{
				PropertyName = "SharedVariables"
			};
			mSharedVariables.CollectionChanged += OnCollectionChanged;
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	AvailableProperties																										*
		////*-----------------------------------------------------------------------*
		//private List<string> mAvailableProperties = new List<string>();
		///// <summary>
		///// Get a reference to the list of available property names to be assigned
		///// by the user at runtime, whose entries include a basic conditional
		///// logic capability for whether or not to use each of those properties.
		///// </summary>
		//[JsonProperty(Order = 8)]
		//public List<string> AvailableProperties
		//{
		//	get { return mAvailableProperties; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Clone																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a deep clone of the specified item.
		/// </summary>
		/// <param name="item">
		/// Reference to the template item to clone.
		/// </param>
		/// <returns>
		/// Reference to the newly created clone.
		/// </returns>
		public static PatternTemplateItem Clone(
			PatternTemplateItem item)
		{
			PatternTemplateItem result = null;

			if(item != null)
			{
				result = new PatternTemplateItem()
				{
					mDisplayFormat = item.mDisplayFormat,
					mIconFilename = item.mIconFilename,
					mIconFileData = item.mIconFileData,
					mOperations = PatternOperationCollection.Clone(item.mOperations),
					//mOrientation = item.mOrientation,
					//mPatternLength = item.mPatternLength,
					mPatternTemplateId = item.mPatternTemplateId,
					//mPatternWidth = item.mPatternWidth,
					mTemplateName = item.mTemplateName
					//mToolPaths = ToolPathCollection.Clone(item.mToolPaths),
					//mToolSequenceStrict = item.mToolSequenceStrict
				};
				//foreach(string entryItem in item.mAvailableProperties)
				//{
				//	result.mAvailableProperties.Add(entryItem);
				//}
				foreach(string entryItem in item.mRemarks)
				{
					result.mRemarks.Add(entryItem);
				}
				foreach(string entryItem in item.mSharedVariables)
				{
					result.mSharedVariables.Add(entryItem);
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	DisplayFormat																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="DisplayFormat">DisplayFormat</see>.
		/// </summary>
		private string mDisplayFormat = "";
		/// <summary>
		/// Get/Set the custom display format to use when preparing the string
		/// information for this item.
		/// </summary>
		[JsonProperty(Order = 3)]
		public string DisplayFormat
		{
			get { return mDisplayFormat; }
			set
			{
				bool bChanged = (mDisplayFormat != value);

				mDisplayFormat = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* GetDisplayString																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a formatted version of the pattern containing some information
		/// about its settings, location, or function.
		/// </summary>
		/// <param name="pattern">
		/// Reference to the pattern template or derived item for which a display
		/// string is being created.
		/// </param>
		/// <returns>
		/// Well-formatted display string containing some information about its
		/// settings, location, or function, as described in the DisplayFormat
		/// property. If DisplayFormat is blank, the name of the pattern will be
		/// returned.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Variable names are specified within curly braces in the format
		/// {VariableName[:Format]}.
		/// </para>
		/// <para>
		/// Following are the currently recognized formats.
		/// </para>
		/// <list type="bullet">
		/// <item><b>Abbreviation</b>. Use an abbreviation of an entry
		/// from a enumeration for the native item type.</item>
		/// <item><b>+-</b>. Explicitly prefix a numerical value with '+', if
		/// positive, and the natural '-' if negative.</item>
		/// </list>
		/// <para>
		/// When the name of an operational variable is specified, that reference
		/// should be prefixed with a space-compressed version of the
		/// corresponding OperationName for that operation, if one has been
		/// defined.
		/// </para>
		/// </remarks>
		public static string GetDisplayString(PatternTemplateItem pattern)
		{
			StringBuilder builder = new StringBuilder();
			string format = "";
			Match match = null;
			float number = 0f;
			string operationNametl = "";
			object property = null;
			OperationActionPropertyItem propertyDef = null;
			string propertyName = "";
			//string replacement = "";
			string text = "";
			string variable = "";
			string variabletl = "";

			if(pattern != null)
			{
				if(pattern.mDisplayFormat.Length > 0)
				{
					//	A display format has been specified.
					text = pattern.mDisplayFormat;
					while(Regex.IsMatch(text,
						ResourceMain.rxInterpolatedVariableAndFormat))
					{
						Clear(builder);
						match = Regex.Match(text,
							ResourceMain.rxInterpolatedVariableAndFormat);
						variable = GetValue(match, "variable");
						format = GetValue(match, "format");
						if(variable.Length > 0)
						{
							property = GetValue(
								typeof(PatternTemplateItem), pattern, variable);
							if(property != null)
							{
								//	This is a pattern template property.
								if(property is string @pString)
								{
									builder.Append(pString);
								}
								else if(property is TemplateOrientationEnum @pTempOrient)
								{
									if(pTempOrient != TemplateOrientationEnum.None)
									{
										builder.Append(pTempOrient.ToString());
									}
									else
									{
										builder.Append("Relative");
									}
								}
								else if(property is List<string> @pList)
								{
									builder.Append(ToMultiLineString(pList));
								}
								else if(property is bool @pBool)
								{
									builder.Append(pBool.ToString());
								}
							}
							else
							{
								//	Attempt to get the OperationName/PropertyName from the
								//	operations collection.
								variabletl = variable.ToLower();
								foreach(PatternOperationItem operationItem in
									pattern.mOperations)
								{
									operationNametl =
										operationItem.OperationName.Replace(" ", "").ToLower();
									if(operationNametl.Length == 0 ||
										(operationNametl.Length > 0 &&
										variabletl.StartsWith(operationNametl)))
									{
										//	The operation has a name and this variable begins with
										//	that name.
										propertyName = variable.Substring(operationNametl.Length);
										property = GetValue(operationItem.GetType(),
											operationItem, propertyName);
										if(property != null)
										{
											//	The property was retrieved from this operation.
											propertyDef = ConfigProfile.OperationActionProperties.
												FirstOrDefault(x => x.PropertyName == propertyName);
											if(propertyDef != null)
											{
												switch(propertyDef.DataType)
												{
													case "AngleString":
														if(format == "+-")
														{
															number = GetAngle(property.ToString());
															if(number >= 0f)
															{
																builder.Append('+');
															}
														}
														if(property.ToString().Length == 0)
														{
															property = "0";
														}
														builder.Append(property.ToString());
														break;
													case "DirectionLeftRightEnum":
														builder.Append(property.ToString());
														if(property is
															DirectionLeftRightEnum @pDirLeftRight)
														{
															switch(pDirLeftRight)
															{
																case DirectionLeftRightEnum.Center:
																case DirectionLeftRightEnum.None:
																	builder.Append("Center");
																	break;
																case DirectionLeftRightEnum.Left:
																case DirectionLeftRightEnum.Right:
																	builder.Append(pDirLeftRight.ToString());
																	break;
															}
														}
														break;
													case "MeasurementString":
														if(format == "+-")
														{
															number = GetMillimeters(property.ToString());
															if(number >= 0f)
															{
																builder.Append('+');
															}
														}
														if(property.ToString().Length == 0)
														{
															property = "0";
														}
														builder.Append(property.ToString());
														break;
													case "OffsetLeftRightEnum":
														if(property is OffsetLeftRightEnum @pOffsLeftRight)
														{
															switch(pOffsLeftRight)
															{
																case OffsetLeftRightEnum.Absolute:
																	builder.Append("Abs");
																	break;
																case OffsetLeftRightEnum.Center:
																case OffsetLeftRightEnum.Left:
																case OffsetLeftRightEnum.Right:
																	builder.Append(pOffsLeftRight.ToString());
																	break;
																case OffsetLeftRightEnum.LeftEdgeToCenter:
																	builder.Append("LToCenter");
																	break;
																case OffsetLeftRightEnum.None:
																case OffsetLeftRightEnum.Relative:
																	builder.Append("Rel");
																	break;
																case OffsetLeftRightEnum.RightEdgeToCenter:
																	builder.Append("RToCenter");
																	break;
															}
														}
														break;
													case "OffsetTopBottomEnum":
														if(property is OffsetTopBottomEnum @pOffsTopBot)
														{
															switch(pOffsTopBot)
															{
																case OffsetTopBottomEnum.Absolute:
																	builder.Append("Abs");
																	break;
																case OffsetTopBottomEnum.Center:
																case OffsetTopBottomEnum.Top:
																case OffsetTopBottomEnum.Bottom:
																	builder.Append(pOffsTopBot.ToString());
																	break;
																case OffsetTopBottomEnum.BottomEdgeToCenter:
																	builder.Append("BToCenter");
																	break;
																case OffsetTopBottomEnum.None:
																case OffsetTopBottomEnum.Relative:
																	builder.Append("Rel");
																	break;
																case OffsetTopBottomEnum.TopEdgeToCenter:
																	builder.Append("TToCenter");
																	break;
															}
														}
														break;
													case "PlotActionEnum":
														builder.Append(property.ToString());
														break;
													case "String":
													case "ToolName":
														builder.Append(property.ToString());
														break;
												}
											}
											break;
										}
									}
								}
							}
							//	Replace the current variable and check for another.
							text = text.Replace(match.Value, builder.ToString());
						}
						else
						{
							//	This value can't be resolved for some reason.
							break;
						}
					}
				}
				else if(pattern.mTemplateName.Length > 0)
				{
					//	Use the base name from the template.
					text = pattern.mTemplateName;
				}
			}
			return text;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IconFileData																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="IconFileData">IconFileData</see>.
		/// </summary>
		private string mIconFileData = "";
		/// <summary>
		/// Get/Set the Base64 DataURL file data of the icon to load for the
		/// specified pattern.
		/// </summary>
		[JsonProperty(Order = 7)]
		public string IconFileData
		{
			get { return mIconFileData; }
			set
			{
				bool bChanged = (mIconFileData != value);

				mIconFileData = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	IconFilename																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="IconFilename">IconFilename</see>.
		/// </summary>
		private string mIconFilename = "";
		/// <summary>
		/// Get/Set the filename of the icon to load for the specified pattern.
		/// </summary>
		[JsonProperty(Order = 2)]
		public string IconFilename
		{
			get { return mIconFilename; }
			set
			{
				bool bChanged = (mIconFilename != value);

				mIconFilename = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Operations																														*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Operations">Operations</see>.
		/// </summary>
		private PatternOperationCollection mOperations = null;
		/// <summary>
		/// Get a reference to the collection of operations defined for this
		/// pattern.
		/// </summary>
		[JsonProperty(Order = 6)]
		public PatternOperationCollection Operations
		{
			get { return mOperations; }
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	Orientation																														*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for <see cref="Orientation">Orientation</see>.
		///// </summary>
		//private TemplateOrientationEnum mOrientation =
		//	TemplateOrientationEnum.Workpiece;
		///// <summary>
		///// Get/Set the orientation space used for measurements in the tool paths.
		///// </summary>
		//[JsonConverter(typeof(StringEnumConverter))]
		//[JsonProperty(Order = 6)]
		//public TemplateOrientationEnum Orientation
		//{
		//	get { return mOrientation; }
		//	set
		//	{
		//		bool bChanged = (mOrientation != value);

		//		mOrientation = value;
		//		if(bChanged)
		//		{
		//			OnPropertyChanged();
		//		}
		//	}
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	PatternLength																													*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for <see cref="PatternLength">PatternLength</see>.
		///// </summary>
		//private string mPatternLength = "";
		///// <summary>
		///// Get/Set the length dimension of the pattern, in the specified
		///// orientation.
		///// </summary>
		//[JsonProperty(Order = 8)]
		//public string PatternLength
		//{
		//	get { return mPatternLength; }
		//	set
		//	{
		//		bool bChanged = (mPatternLength != value);

		//		mPatternLength = value;
		//		if(bChanged)
		//		{
		//			OnPropertyChanged();
		//		}
		//	}
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PatternTemplateId																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for
		/// <see cref="PatternTemplateId">PatternTemplateId</see>.
		/// </summary>
		private string mPatternTemplateId = Guid.NewGuid().ToString("D").ToLower();
		/// <summary>
		/// Get/Set the globally unique ID of this pattern template.
		/// </summary>
		[JsonProperty(Order = 0)]
		public string PatternTemplateId
		{
			get { return mPatternTemplateId; }
			set
			{
				bool bChanged = (mPatternTemplateId != value);

				mPatternTemplateId = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	PatternWidth																													*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for <see cref="PatternWidth">PatternWidth</see>.
		///// </summary>
		//private string mPatternWidth = "";
		///// <summary>
		///// Get/Set the width dimension of the pattern, in the specified
		///// orientation.
		///// </summary>
		//[JsonProperty(Order = 7)]		
		//public string PatternWidth
		//{
		//	get { return mPatternWidth; }
		//	set
		//	{
		//		bool bChanged = (mPatternWidth != value);

		//		mPatternWidth = value;
		//		if(bChanged)
		//		{
		//			OnPropertyChanged();
		//		}
		//	}
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Remarks																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Remarks">Remarks</see>.
		/// </summary>
		private ChangeObjectCollection<string> mRemarks = null;
		/// <summary>
		/// Get a reference to the list of line continuation remarks used to
		/// describe this template.
		/// </summary>
		[JsonProperty(Order = 4)]
		public List<string> Remarks
		{
			get { return mRemarks; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SharedVariables																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="SharedVariables">SharedVariables</see>.
		/// </summary>
		private ChangeObjectCollection<string> mSharedVariables = null;
		/// <summary>
		/// Get a reference to a list of variable names in this operation that are
		/// shared for the entire pattern.
		/// </summary>
		[JsonProperty(Order = 5)]
		public ChangeObjectCollection<string> SharedVariables
		{
			get { return mSharedVariables; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeDisplayFormat																					*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the DisplayFormat property will be
		/// serialized.
		/// </summary>
		/// <returns>
		/// A value indicating whether to serialize the property.
		/// </returns>
		public bool ShouldSerializeDisplayFormat()
		{
			return mDisplayFormat?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeIconFileData																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the IconFileData property will be
		/// serialized.
		/// </summary>
		/// <returns>
		/// A value indicating whether to serialize the property.
		/// </returns>
		public bool ShouldSerializeIconFileData()
		{
			return mIconFileData?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeIconFilename																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the IconFilename property will be
		/// serialized.
		/// </summary>
		/// <returns>
		/// A value indicating whether to serialize the property.
		/// </returns>
		public bool ShouldSerializeIconFilename()
		{
			return mIconFilename?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeOperations																							*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the Operations property will be
		/// serialized.
		/// </summary>
		/// <returns>
		/// A value indicating whether to serialize the property.
		/// </returns>
		public bool ShouldSerializeOperations()
		{
			return mOperations.Count > 0;
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* ShouldSerializeOrientation																						*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return a value indicating whether the Orientation property will be
		///// serialized.
		///// </summary>
		///// <returns>
		///// A value indicating whether to serialize the property.
		///// </returns>
		//public bool ShouldSerializeOrientation()
		//{
		//	return mOrientation != TemplateOrientationEnum.None;
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* ShouldSerializePatternLength																					*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return a value indicating whether the PatternLength property will be
		///// serialized.
		///// </summary>
		///// <returns>
		///// A value indicating whether to serialize the property.
		///// </returns>
		//public bool ShouldSerializePatternLength()
		//{
		//	return mPatternLength?.Length > 0;
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializePatternTemplateId																			*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the PatternTemplateId property will
		/// be serialized.
		/// </summary>
		/// <returns>
		/// A value indicating whether to serialize the property.
		/// </returns>
		public bool ShouldSerializePatternTemplateId()
		{
			return mPatternTemplateId?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* ShouldSerializePatternWidth																						*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return a value indicating whether the PatternWidth property will be
		///// serialized.
		///// </summary>
		///// <returns>
		///// A value indicating whether to serialize the property.
		///// </returns>
		//public bool ShouldSerializePatternWidth()
		//{
		//	return mPatternWidth?.Length > 0;
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeRemarks																								*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the Remarks property will be
		/// serialized.
		/// </summary>
		/// <returns>
		/// A value indicating whether to serialize the property.
		/// </returns>
		public bool ShouldSerializeRemarks()
		{
			return mRemarks.Count > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeSharedVariables																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the SharedVariables property will be
		/// serialized.
		/// </summary>
		/// <returns>
		/// A value indicating whether to serialize the property.
		/// </returns>
		public bool ShouldSerializeSharedVariables()
		{
			return mSharedVariables.Count > 0;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ShouldSerializeTemplateName																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return a value indicating whether the TemplateName property will be
		/// serialized.
		/// </summary>
		/// <returns>
		/// A value indicating whether to serialize the property.
		/// </returns>
		public bool ShouldSerializeTemplateName()
		{
			return mTemplateName?.Length > 0;
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////* ShouldSerializeToolSequenceStrict																			*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Return a value indicating whether the ToolSequenceStrict property will
		///// be serialized.
		///// </summary>
		///// <returns>
		///// A value indicating whether to serialize the property.
		///// </returns>
		//public bool ShouldSerializeToolSequenceStrict()
		//{
		//	return mToolSequenceStrict == false;
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TemplateName																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="TemplateName">TemplateName</see>.
		/// </summary>
		private string mTemplateName = "";
		/// <summary>
		/// Get/Set the name of this template.
		/// </summary>
		[JsonProperty(Order = 1)]
		public string TemplateName
		{
			get { return mTemplateName; }
			set
			{
				bool bChanged = (mTemplateName != value);

				mTemplateName = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	ToolSequenceStrict																										*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for
		///// <see cref="ToolSequenceStrict">ToolSequenceStrict</see>.
		///// </summary>
		//private bool mToolSequenceStrict = true;
		///// <summary>
		///// Get/Set a value indicating whether the entries in the ToolPaths
		///// collection must be followed in strict order. If false, items with the
		///// same tool as the previous operation will be preferred over the sort
		///// order of those entries.
		///// </summary>
		//[JsonProperty(Order = 5)]
		//public bool ToolSequenceStrict
		//{
		//	get { return mToolSequenceStrict; }
		//	set
		//	{
		//		bool bChanged = (mToolSequenceStrict != value);

		//		mToolSequenceStrict = value;
		//		if(bChanged)
		//		{
		//			OnPropertyChanged();
		//		}
		//	}
		//}
		////*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

}
