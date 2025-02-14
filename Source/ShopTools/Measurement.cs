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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Flee;
using Flee.PublicTypes;
using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	MeasurementProcessor																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Handler for user-readable measurement expressions in any unit.
	/// </summary>
	public class MeasurementProcessor
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//* GetTokens																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the user's value as a set of discrete measurement tokens.
		/// </summary>
		/// <param name="value">
		/// The raw string value to parse.
		/// </param>
		/// <param name="defaultUnit">
		/// Name of the default unit to use when not specified.
		/// </param>
		/// <returns>
		/// Reference to a collection of measurement tokens.
		/// </returns>
		private static MeasurementTokenCollection GetTokens(string value,
			string defaultUnit)
		{
			int count = 6;
			int index = 0;
			MatchCollection matches = null;
			MeasurementTokenCollection result = new MeasurementTokenCollection();
			string[] rawValues = new string[count];
			string text = "";
			//MeasurementTokenCollection target = result;
			MeasurementTokenItem token = null;
			//MeasurementTokenCollection tokens = result;

			if(value?.Length > 0)
			{
				for(index = 0; index < count; index ++)
				{
					rawValues[index] = "";
				}
				matches = Regex.Matches(value, ResourceMain.rxMeasurementExpression);
				foreach(Match matchItem in matches)
				{
					token = new MeasurementTokenItem();

					rawValues[(int)MeasurementTokenType.Fraction] =
						GetValue(matchItem, "fraction");
					rawValues[(int)MeasurementTokenType.Number] =
						GetValue(matchItem, "numeric");
					rawValues[(int)MeasurementTokenType.Parenthesis] =
						GetValue(matchItem, "paren");
					rawValues[(int)MeasurementTokenType.Symbol] =
						GetValue(matchItem, "symbol");
					rawValues[(int)MeasurementTokenType.Unit] =
						GetValue(matchItem, "unit");

					text = "";
					for(index = 1; index < count; index ++)
					{
						text = rawValues[index];
						if(text.Length > 0)
						{
							//	Value found.
							token.Value = text;
							token.TokenType = (MeasurementTokenType)index;
							break;
						}
					}
					if(token.TokenType == MeasurementTokenType.Unit)
					{
						switch(token.Value)
						{
							case "'":
								token.Value = "ft";
								break;
							case "\"":
								token.Value = "in";
								break;
						}
					}
					//if(token.TokenType == MeasurementTokenType.Parenthesis &&
					//	text == ")" && target.Parent?.Parent != null)
					//{
					//	//	Closing parenthesis on an existing branch.
					//	target = target.Parent.Parent;
					//}
					//target.Add(token);
					result.Add(token);
					//if(token.TokenType == MeasurementTokenType.Parenthesis)
					//{
					//	if(text == "(")
					//	{
					//		//	Opening parenthesis.
					//		//	Next item is a child token.
					//		target = token.Tokens;
					//	}
					//}

					//if(numeric.Length > 0)
					//{
					//	if(unit.Length == 0 && defaultUnit?.Length > 0)
					//	{
					//		unit = defaultUnit;
					//	}
					//}
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
		//* SumInches																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the sum of elements in the caller's measurement string as
		/// inches.
		/// </summary>
		/// <param name="measurement">
		/// The measurement to parse.
		/// </param>
		/// <param name="defaultUnit">
		/// The unit to supply if no default unit was given.
		/// </param>
		/// <returns>
		/// Sum of the elements in the measurement, as inches.
		/// </returns>
		public static float SumInches(string measurement, string defaultUnit)
		{
			float result = 0f;
			MeasurementTokenCollection tokens = GetTokens(measurement, defaultUnit);

			result = (float)SessionConverter.Convert(
				MeasurementTokenCollection.Calculate(tokens, defaultUnit), "mm", "in");
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SumMillimeters																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the sum of elements in the caller's measurement string as
		/// millimeters.
		/// </summary>
		/// <param name="measurement">
		/// The measurement to parse.
		/// </param>
		/// <param name="defaultUnit">
		/// The unit to supply if no explicit unit was given.
		/// </param>
		/// <returns>
		/// Sum of the elements in the measurement, as millimeters.
		/// </returns>
		public static float SumMillimeters(string measurement, string defaultUnit)
		{
			float result = 0f;
			MeasurementTokenCollection tokens = GetTokens(measurement, defaultUnit);

			result = MeasurementTokenCollection.Calculate(tokens, defaultUnit);
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	MeasurementTokenCollection																							*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of MeasurementTokenItem Items.
	/// </summary>
	public class MeasurementTokenCollection : List<MeasurementTokenItem>
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		/// <summary>
		/// Session-level expression context for measurement conversion.
		/// </summary>
		private static ExpressionContext mExpressionContext =
			new ExpressionContext();

		//*-----------------------------------------------------------------------*
		//* Reduce																																*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Reduce the caller's token collection to a flat list of numbers and
		/// calculable symbols.
		/// </summary>
		/// <param name="tokens">
		/// Reference to the collection of tokens to reduce.
		/// </param>
		/// <param name="defaultUnit">
		/// The default unit to assign if no unit is specified for a number.
		/// </param>
		/// <returns>
		/// Reference to a flat collection of decimal numbers, mathematical
		/// operators, and parenthesis.
		/// </returns>
		private static MeasurementTokenCollection Reduce(
			MeasurementTokenCollection tokens, string defaultUnit)
		{
			bool bFound = false;
			int count = 0;
			float denominator = 0f;
			int index = 0;
			string lastOperator = "";
			Match match = null;
			float number = 0f;
			float numberPart = 0f;
			float numerator = 0f;
			int openLeft = 0;
			int openRight = 0;
			MeasurementTokenCollection result = new MeasurementTokenCollection();
			MeasurementTokenItem newToken = null;
			MeasurementTokenItem nextToken = null;
			MeasurementTokenItem prevToken = null;
			string text = "";
			MeasurementTokenItem token = null;
			string unit = "";

			if(tokens?.Count > 0)
			{
				count = tokens.Count;
				number = 0f;
				numberPart = 0f;
				for(index = 0; index < count; index ++)
				{
					token = tokens[index];
					switch(token.TokenType)
					{
						case MeasurementTokenType.Fraction:
							match = Regex.Match(token.Value, ResourceMain.rxFraction);
							if(match.Success)
							{
								text = GetValue(match, "numerator");
								if(text.Length > 0)
								{
									numerator = ToFloat(GetValue(match, "numerator"));
									denominator = ToFloat(GetValue(match, "denominator"));
									if(denominator != 0f)
									{
										numberPart = (numerator / denominator);
									}
								}
							}
							//	This value will either have an associated number or be
							//	a stand-alone fraction.
							//	This value will only be followed by a unit.
							unit = "";
							if(index + 1 < count)
							{
								nextToken = tokens[index + 1];
								if(nextToken.TokenType == MeasurementTokenType.Unit)
								{
									//	The next item is a unit.
									unit = nextToken.Value;
									//	The unit will be skipped in the next iteration.
									index++;
								}
							}
							if(unit.Length == 0 && defaultUnit?.Length > 0 &&
								lastOperator != "*" && lastOperator != "/")
							{
								//	A unit was not specified. Use the default.
								unit = defaultUnit;
							}
							if(unit.Length > 0)
							{
								number = (float)SessionConverter.Convert(number, unit, "mm");
								number +=
									(float)SessionConverter.Convert(numberPart, unit, "mm");
							}
							else
							{
								//	If no explicit or default units available, assume the value
								//	is in system units.
								number += numberPart;
							}
							newToken = new MeasurementTokenItem()
							{
								TokenType = MeasurementTokenType.Number,
								Value = number.ToString("0.###")
							};
							result.Add(newToken);
							number = 0f;
							numberPart = 0f;
							break;
						case MeasurementTokenType.Number:
							bFound = false;
							unit = "";
							number = ToFloat(token.Value);
							numberPart = 0f;
							if(index + 1 < count)
							{
								nextToken = tokens[index + 1];
								if(nextToken.TokenType == MeasurementTokenType.Unit)
								{
									//	The next item is a unit.
									//	The unit will be skipped in the next iteration.
									index++;
									unit = nextToken.Value;
									if(unit.Length > 0)
									{
										number =
											(float)SessionConverter.Convert(number, unit, "mm");
									}
									//	Finish the number now, since the unit has been processed.
									newToken = new MeasurementTokenItem()
									{
										TokenType = MeasurementTokenType.Number,
										Value = number.ToString("0.###")
									};
									result.Add(newToken);
									number = 0f;
									bFound = true;
								}
								else if(nextToken.TokenType == MeasurementTokenType.Fraction)
								{
									//	If the next token is a unit or a fraction, then use
									//	the fraction's unit.
									bFound = true;
								}
							}
							if(!bFound)
							{
								//	Add the default numeric value.
								if(defaultUnit?.Length > 0 &&
									lastOperator != "*" && lastOperator != "/")
								{
									number =
										(float)SessionConverter.Convert(number, defaultUnit, "mm");
								}
								newToken = new MeasurementTokenItem()
								{
									TokenType = MeasurementTokenType.Number,
									Value = number.ToString("0.###")
								};
								result.Add(newToken);
								number = 0f;
								bFound = true;
							}
							break;
						case MeasurementTokenType.Parenthesis:
							//	In this version, just transfer values across.
							newToken = new MeasurementTokenItem()
							{
								TokenType = token.TokenType,
								Value = token.Value
							};
							result.Add(newToken);
							number = 0f;
							break;
						case MeasurementTokenType.Symbol:
							//	In this version, just transfer values across.
							lastOperator = token.Value;
							newToken = new MeasurementTokenItem()
							{
								TokenType = token.TokenType,
								Value = token.Value
							};
							result.Add(newToken);
							number = 0f;
							break;
						case MeasurementTokenType.Unit:
							//	We shouldn't reach this item because the preceding number
							//	will already be converted.
							break;
					}
				}
				//	Clean up the list.
				if(!tokens.Exists(x => x.TokenType == MeasurementTokenType.Unit) &&
					tokens.Exists(x => x.TokenType == MeasurementTokenType.Number) &&
					defaultUnit?.Length > 0)
				{
					//	No units were specified in this expression.
					//	Convert the non-multiplicative numbers to system units from the
					//	default.
					prevToken = null;
					count = result.Count;
					for(index = 0; index < count; index ++)
					{
						token = result[index];
						if(token.TokenType == MeasurementTokenType.Number &&
							(prevToken == null ||
							prevToken.TokenType != MeasurementTokenType.Symbol ||
							(prevToken.Value != "*" && prevToken.Value != "/")))
						{
							number =
								(float)SessionConverter.Convert(
									ToFloat(token.Value), defaultUnit, "mm");
							token.Value = number.ToString("0.###");
						}
						prevToken = token;
					}
				}
				//	Insert addition symbols where appropriate.
				count = result.Count;
				for(index = 0; index < count; index ++)
				{
					token = result[index];
					if(token.TokenType == MeasurementTokenType.Number &&
						index + 1 < result.Count)
					{
						nextToken = result[index + 1];
						if(nextToken.TokenType == MeasurementTokenType.Number)
						{
							//	Back-to-back numbers imply addition.
							result.Insert(index + 1, new MeasurementTokenItem()
							{
								TokenType = MeasurementTokenType.Symbol,
								Value = "+"
							});
						}
					}
				}
				//	Remove leading operations.
				while(result.Count > 0 &&
					result[0].TokenType == MeasurementTokenType.Symbol)
				{
					result.RemoveAt(0);
				}
				//	Remove trailing operations.
				while(result.Count > 0 &&
					result[^1].TokenType == MeasurementTokenType.Symbol)
				{
					result.RemoveAt(result.Count - 1);
				}
				//	Balance parenthesis.
				openLeft = result.FindAll(x =>
					x.TokenType == MeasurementTokenType.Parenthesis && x.Value == "(").
					Count;
				openRight = result.FindAll(x =>
					x.TokenType == MeasurementTokenType.Parenthesis && x.Value == ")").
					Count;
				while(openLeft > openRight)
				{
					//	Add right parenthesis until equal to left.
					result.Add(new MeasurementTokenItem()
					{
						TokenType = MeasurementTokenType.Parenthesis,
						Value = ")"
					});
					openRight++;
				}
				while(openRight > openLeft)
				{
					//	Add left parenthesis until equal to right.
					result.Insert(0, new MeasurementTokenItem()
					{
						TokenType = MeasurementTokenType.Parenthesis,
						Value = "("
					});
					openLeft++;
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
		//* Add																																		*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Add a MeasurementTokenItem to the collection.
		/// </summary>
		/// <param name="item">
		/// Reference to the item to be added.
		/// </param>
		public new void Add(MeasurementTokenItem item)
		{
			if(item != null)
			{
				//item.Parent = this;
				base.Add(item);
			}
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* Calculate																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the calculated value of the measurement tokens, in system units
		/// (mm).
		/// </summary>
		/// <param name="items">
		/// Reference to the collection of items to be calculated.
		/// </param>
		/// <param name="defaultUnit">
		/// The default unit to apply if the number doesn't have an accompanying
		/// unit.
		/// </param>
		/// <returns>
		/// </returns>
		public static float Calculate(MeasurementTokenCollection items,
			string defaultUnit)
		{
			IGenericExpression<double> eGeneric = null;
			string expression = "";
			MeasurementTokenCollection tokens = null;
			float result = 0f;

			if(items?.Count > 0)
			{
				tokens = Reduce(items, defaultUnit);
				expression = tokens.ToString();
				try
				{
					eGeneric = mExpressionContext.CompileGeneric<double>(expression);
					result = (float)eGeneric.Evaluate();
				}
				catch { }
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	Parent																																*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for <see cref="Parent">Parent</see>.
		///// </summary>
		//private MeasurementTokenItem mParent = null;
		///// <summary>
		///// Get/Set the parent item of this collection.
		///// </summary>
		//public MeasurementTokenItem Parent
		//{
		//	get { return mParent; }
		//	set { mParent = value; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of the items in this collection.
		/// </summary>
		/// <returns>
		/// String representation of this collection's contents.
		/// </returns>
		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();

			foreach(MeasurementTokenItem tokenItem in this)
			{
				if(builder.Length > 0)
				{
					builder.Append(' ');
				}
				builder.Append(tokenItem.Value);
			}
			return builder.ToString();
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*


	//*-------------------------------------------------------------------------*
	//*	MeasurementTokenItem																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// An individual measurement token with a value and a unit.
	/// </summary>
	public class MeasurementTokenItem
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
		/// Create a new instance of the MeasurementTokenItem Item.
		/// </summary>
		public MeasurementTokenItem()
		{
			//mTokens = new MeasurementTokenCollection();
			//mTokens.Parent = this;
		}
		//*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	Parent																																*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for <see cref="Parent">Parent</see>.
		///// </summary>
		//private MeasurementTokenCollection mParent = null;
		///// <summary>
		///// Get/Set the parent collection of this token.
		///// </summary>
		//public MeasurementTokenCollection Parent
		//{
		//	get { return mParent; }
		//	set { mParent = value; }
		//}
		////*-----------------------------------------------------------------------*

		////*-----------------------------------------------------------------------*
		////*	Tokens																																*
		////*-----------------------------------------------------------------------*
		///// <summary>
		///// Private member for <see cref="Tokens">Tokens</see>.
		///// </summary>
		//private MeasurementTokenCollection mTokens = null;
		///// <summary>
		///// Get a reference to the list of tokens found under this one.
		///// </summary>
		//public MeasurementTokenCollection Tokens
		//{
		//	get { return mTokens; }
		//}
		////*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	TokenType																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="TokenType">TokenType</see>.
		/// </summary>
		private MeasurementTokenType mTokenType = MeasurementTokenType.None;
		/// <summary>
		/// Get/Set the token type used to identify this token.
		/// </summary>
		public MeasurementTokenType TokenType
		{
			get { return mTokenType; }
			set { mTokenType = value; }
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	Value																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Value">Value</see>.
		/// </summary>
		private string mValue = "";
		/// <summary>
		/// Get/Set the value of the measurement.
		/// </summary>
		public string Value
		{
			get { return mValue; }
			set { mValue = value; }
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
