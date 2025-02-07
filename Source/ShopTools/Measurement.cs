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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using static ShopTools.ShopToolsUtil;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	MeasurementCollection																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Collection of MeasurementItem Items.
	/// </summary>
	public class MeasurementCollection : List<MeasurementItem>
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
		//* GetMeasurements																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the user's value as a set of discrete measurements.
		/// </summary>
		/// <param name="value">
		/// The raw string value to parse.
		/// </param>
		/// <param name="defaultUnit">
		/// Name of the default unit to use when not specified.
		/// </param>
		/// <returns>
		/// Reference to a collection of whole and fractional measurements.
		/// </returns>
		public static MeasurementCollection GetMeasurements(string value,
			string defaultUnit)
		{
			MatchCollection matches = null;
			MeasurementCollection result = new MeasurementCollection();
			string numeric = "";
			string unit = "";

			if(value?.Length > 0)
			{
				//	In this version, the RegEx returns a last blank match.
				//	This method accomodates blank matches.
				matches = Regex.Matches(value, ResourceMain.rxNumericNonNumeric);
				foreach(Match matchItem in matches)
				{
					numeric = GetValue(matchItem, "numeric");
					unit = GetValue(matchItem, "nonnumeric");
					if(numeric.Length > 0)
					{
						if(unit.Length == 0 && defaultUnit?.Length > 0)
						{
							unit = defaultUnit;
						}
						if(unit == "'")
						{
							unit = "ft";
						}
						if(unit == "\"")
						{
							unit = "in";
						}
						result.Add(new MeasurementItem()
						{
							Value = numeric,
							Unit = unit
						});
					}
				}
			}
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SumInches																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the sum of measurements in this collection, in inches.
		/// </summary>
		/// <returns>
		/// Total value of the measurements, in inches.
		/// </returns>
		public float SumInches()
		{
			float denominator = 0f;
			Match match = null;
			float number = 0f;
			float numerator = 0f;
			float result = 0f;
			string text = "";

			foreach(MeasurementItem measurementItem in this)
			{
				number = 0f;
				match = Regex.Match(measurementItem.Value, ResourceMain.rxFractional);
				if(match.Success)
				{
					number = ToFloat(GetValue(match, "number"));
					text = GetValue(match, "numerator");
					if(text.Length > 0)
					{
						numerator = ToFloat(GetValue(match, "numerator"));
						denominator = ToFloat(GetValue(match, "denominator"));
						if(denominator != 0f)
						{
							number += (numerator / denominator);
						}
					}
				}
				if(measurementItem.Unit?.Length > 0)
				{
					number = (float)SessionConverter.Convert(number,
						measurementItem.Unit, "in");
				}
				result += number;
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
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
			MeasurementCollection measurements =
				GetMeasurements(measurement, defaultUnit);
			float result = measurements.SumInches();
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* SumMillimeters																												*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the sum of measurements in this collection, in
		/// millimeters.
		/// </summary>
		/// <returns>
		/// Total value of the measurements, in millimeters.
		/// </returns>
		public float SumMillimeters()
		{
			float denominator = 0f;
			Match match = null;
			float number = 0f;
			float numerator = 0f;
			float result = 0f;
			string text = "";

			foreach(MeasurementItem measurementItem in this)
			{
				number = 0f;
				match = Regex.Match(measurementItem.Value, ResourceMain.rxFractional);
				if(match.Success)
				{
					number = ToFloat(GetValue(match, "number"));
					text = GetValue(match, "numerator");
					if(text.Length > 0)
					{
						numerator = ToFloat(GetValue(match, "numerator"));
						denominator = ToFloat(GetValue(match, "denominator"));
						if(denominator != 0f)
						{
							number += (numerator / denominator);
						}
					}
				}
				if(measurementItem.Unit?.Length > 0)
				{
					number = (float)SessionConverter.Convert(number,
						measurementItem.Unit, "mm");
				}
				result += number;
			}
			return result;
		}
		//*- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -*
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
			MeasurementCollection measurements =
				GetMeasurements(measurement, defaultUnit);
			float result = measurements.SumMillimeters();
			return result;
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	MeasurementItem																													*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// An individual measurement with a value and a unit.
	/// </summary>
	public class MeasurementItem
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
		//*	Unit																																	*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Private member for <see cref="Unit">Unit</see>.
		/// </summary>
		private string mUnit = "";
		/// <summary>
		/// Get/Set the unit of the measurement.
		/// </summary>
		public string Unit
		{
			get { return mUnit; }
			set { mUnit = value; }
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
