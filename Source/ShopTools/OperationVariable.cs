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
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	OperationVariableCollection																							*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of OperationVariableItem Items.
	/// </summary>
	public class OperationVariableCollection : BindingList<OperationVariableItem>
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
		//* AddOperation																													*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add the provided operation and its associated variables to the
		/// collection.
		/// </summary>
		/// <param name="operation">
		/// Reference to the pattern operation to add.
		/// </param>
		/// <remarks>
		/// If the base variable name has already been established for this
		/// operation's OperationName property, then the current operation is
		/// added to that variable's list.
		/// </remarks>
		public void AddOperation(PatternOperationItem operation)
		{
			StringBuilder builder = new StringBuilder();
			List<OperationActionPropertyItem> operationActionProperties = null;
			string text = "";
			OperationVariableItem variable = null;

			if(operation != null)
			{
				//	Get the list of all properties associated with this operation.
				operationActionProperties = ConfigProfile.OperationActionProperties.
					FindAll(x => x.IncludeOperationActions.Contains(
						operation.Action.ToString()));
				foreach(OperationActionPropertyItem propertyItem in
					operationActionProperties)
				{
					if(!mSharedVariables.Contains(propertyItem.PropertyName) &&
						!operation.HiddenVariables.Contains(propertyItem.PropertyName))
					{
						variable =
							this.FirstOrDefault(x =>
								x.OperationName == operation.OperationName &&
								x.BaseName == propertyItem.PropertyName);
						text = PatternOperationItem.GetValue(operation,
							propertyItem.PropertyName);
						if(variable == null)
						{
							//	The variable entry should be created.
							ShopToolsUtil.Clear(builder);
							if(operation.OperationName?.Length > 0)
							{
								builder.Append($"{operation.OperationName} ");
							}
							builder.Append(ExpandCamelCase(propertyItem.PropertyName));
							variable = new OperationVariableItem()
							{
								BaseName = propertyItem.PropertyName,
								DisplayName = builder.ToString(),
								OperationName = operation.OperationName,
								Value = text
							};
							//	Store a cross-reference from this variable.
							variable.PatternOperations.Add(operation);
							this.Add(variable);
						}
						else
						{
							//	This variable already exists with the same operation name.
							//	Update the cross-reference list and possibly the value.
							if(!variable.PatternOperations.Exists(x => x == operation))
							{
								variable.PatternOperations.Add(operation);
							}
							if(!(variable.Value?.Length > 0))
							{
								//	No value has been set. Update with this property's value,
								//	if applicable.
								if(text?.Length > 0)
								{
									variable.Value = text;
								}
							}
						}
					}
					else if(mSharedVariables.Contains(propertyItem.PropertyName))
					{
						//	This is a shared property. Add it once if not already added.
						variable =
							this.FirstOrDefault(x =>
								x.BaseName == propertyItem.PropertyName);
						text = PatternOperationItem.GetValue(operation,
							propertyItem.PropertyName);
						if(variable == null)
						{
							//	The variable entry should be created without an
							//	operation name.
							ShopToolsUtil.Clear(builder);
							builder.Append(ExpandCamelCase(propertyItem.PropertyName));
							variable = new OperationVariableItem()
							{
								BaseName = propertyItem.PropertyName,
								DisplayName = builder.ToString(),
								Value = text
							};
							//	Store a cross-reference from this variable.
							variable.PatternOperations.Add(operation);
							this.Add(variable);
						}
						else
						{
							//	This variable already exists with the same operation name.
							//	Update the cross-reference list and possibly the value.
							if(!variable.PatternOperations.Exists(x => x == operation))
							{
								variable.PatternOperations.Add(operation);
							}
							if(!(variable.Value?.Length > 0))
							{
								//	No value has been set. Update with this property's value,
								//	if applicable.
								if(text?.Length > 0)
								{
									variable.Value = text;
								}
							}
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	SharedVariables																												*
		//*-----------------------------------------------------------------------*
		private List<string> mSharedVariables = new List<string>();
		/// <summary>
		/// Get a reference to a list of variable names in this operation that are
		/// shared for the entire pattern.
		/// </summary>
		public List<string> SharedVariables
		{
			get { return mSharedVariables; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* TransferWorkingValues																									*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Transfer operation variable working values to the provided pattern
		/// operation.
		/// </summary>
		/// <param name="variables">
		/// Reference to a collection of variables containing the working values
		/// to transfer.
		/// </param>
		/// <param name="patternOperation">
		/// Reference to the pattern operation whose properties will be updated.
		/// </param>
		public static void TransferWorkingValues(
			List<OperationVariableItem> variables,
			PatternOperationItem patternOperation)
		{
			OperationActionPropertyItem actionProperty = null;
			DirectionLeftRightEnum directionLeftRight = DirectionLeftRightEnum.None;
			OffsetLeftRightEnum offsetLeftRight = OffsetLeftRightEnum.None;
			OffsetTopBottomEnum offsetTopBottom = OffsetTopBottomEnum.None;

			if(variables?.Count > 0 && patternOperation != null)
			{
				foreach(OperationVariableItem variableItem in variables)
				{
					actionProperty =
						ConfigProfile.OperationActionProperties.FirstOrDefault(x =>
							x.PropertyName.ToLower() == variableItem.BaseName.ToLower());
					if(actionProperty != null)
					{
						switch(actionProperty.DataType)
						{
							case "AngleString":
							case "MeasurementString":
							case "String":
							case "ToolName":
								//	Direct conversion to string.
								PatternOperationItem.SetValue(patternOperation,
									variableItem.BaseName, variableItem.WorkingValue);
								break;
							case "DirectionLeftRightEnum":
								if(Enum.TryParse<DirectionLeftRightEnum>(
									variableItem.WorkingValue, true, out directionLeftRight))
								{
									PatternOperationItem.SetValue(patternOperation,
										variableItem.BaseName, directionLeftRight);
								}
								break;
							case "OffsetLeftRightEnum":
								if(Enum.TryParse<OffsetLeftRightEnum>(
									variableItem.WorkingValue, true, out offsetLeftRight))
								{
									PatternOperationItem.SetValue(patternOperation,
										variableItem.BaseName, offsetLeftRight);
								}
								break;
							case "OffsetTopBottomEnum":
								if(Enum.TryParse<OffsetTopBottomEnum>(
									variableItem.WorkingValue, true, out offsetTopBottom))
								{
									PatternOperationItem.SetValue(patternOperation,
										variableItem.BaseName, offsetTopBottom);
								}
								break;
						}
					}
				}
			}
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	OperationVariableItem																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Representation of a runtime operation and its variables.
	/// </summary>
	public class OperationVariableItem : INotifyPropertyChanged
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* OnPropertyChanged																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Raises the PropertyChanged event when the value of a property has
		/// changed.
		/// </summary>
		/// <param name="propertyName">
		/// Name of the changed property.
		/// </param>
		protected virtual void OnPropertyChanged(
			[CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this,
				new PropertyChangedEventArgs(propertyName));
		}
		//*-----------------------------------------------------------------------*

		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	BaseName																															*
		//*-----------------------------------------------------------------------*
		private string mBaseName = "";
		/// <summary>
		/// Get/Set the base property name of this item.
		/// </summary>
		[Browsable(false)]
		public string BaseName
		{
			get { return mBaseName; }
			set
			{
				bool bChanged = (mBaseName != value);

				mBaseName = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	DisplayName																														*
		//*-----------------------------------------------------------------------*
		private string mDisplayName = "";
		/// <summary>
		/// Get/Set the display name of this item.
		/// </summary>
		[DisplayName("Name")]
		public string DisplayName
		{
			get { return mDisplayName; }
			set
			{
				bool bChanged = (mDisplayName != value);

				mDisplayName = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	OperationName																													*
		//*-----------------------------------------------------------------------*
		private string mOperationName = "";
		/// <summary>
		/// Get/Set the distinct operation group name for this variable.
		/// </summary>
		[Browsable(false)]
		public string OperationName
		{
			get { return mOperationName; }
			set
			{
				bool bChanged = (mOperationName != value);

				mOperationName = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	PatternOperations																											*
		//*-----------------------------------------------------------------------*
		private List<PatternOperationItem> mPatternOperations =
			new List<PatternOperationItem>();
		/// <summary>
		/// Get a reference to the collection of pattern operations to which this
		/// variable applies.
		/// </summary>
		[Browsable(false)]
		public List<PatternOperationItem> PatternOperations
		{
			get { return mPatternOperations; }
			set { mPatternOperations = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* PropertyChanged																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Fired when the value of a property has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* UpdateOperations																											*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Update the linked operations with the value established on the
		/// item's Value property.
		/// </summary>
		/// <param name="operationVariable">
		/// Reference to the operation variable whose linked operation properties
		/// will be updated.
		/// </param>
		public static void UpdateOperations(
			OperationVariableItem operationVariable)
		{
			if(operationVariable?.mPatternOperations.Count > 0)
			{
				foreach(PatternOperationItem operationItem in
					operationVariable.mPatternOperations)
				{
					PatternOperationItem.SetValue(operationItem,
						operationVariable.mBaseName, operationVariable.mValue);
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Value																																	*
		//*-----------------------------------------------------------------------*
		private string mValue = "";
		/// <summary>
		/// Get/Set the value of this variable.
		/// </summary>
		public string Value
		{
			get { return mValue; }
			set
			{
				bool bChanged = (mValue != value);

				mValue = value;
				//	Setting the main value overrides the intermediate value.
				mWorkingValue = value;
				if(bChanged)
				{
					OnPropertyChanged();
				}
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	WorkingValue																													*
		//*-----------------------------------------------------------------------*
		private string mWorkingValue = "";
		/// <summary>
		/// Get/Set the working (in-edit) value of this item.
		/// </summary>
		[Browsable(false)]
		public string WorkingValue
		{
			get { return mWorkingValue; }
			set { mWorkingValue = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
