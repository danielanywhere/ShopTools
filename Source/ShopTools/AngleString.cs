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

using Newtonsoft.Json;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	AngleString																															*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// A string depicting a user-specified angle.
	/// </summary>
	public class AngleString
	{
		//*************************************************************************
		//*	Private																																*
		//*************************************************************************
		/// <summary>
		/// The value of this item.
		/// </summary>
		private string mValue = "";
		//*************************************************************************
		//*	Protected																															*
		//*************************************************************************
		//*************************************************************************
		//*	Public																																*
		//*************************************************************************
		//*-----------------------------------------------------------------------*
		//*	_Implicit AngleString = string																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the string instance to an AngleString.
		/// </summary>
		public static implicit operator AngleString(string value)
		{
			AngleString result = new AngleString();

			result.mValue = value;
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	_Implicit string = AngleString																				*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the AngleString instance to a string.
		/// </summary>
		public static implicit operator string(AngleString value)
		{
			return value.mValue;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	ToString																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Return the string representation of this object.
		/// </summary>
		public override string ToString()
		{
			return mValue;
		}
		//*-----------------------------------------------------------------------*


	}
	//*-------------------------------------------------------------------------*

	//*-------------------------------------------------------------------------*
	//*	AngleStringConverter																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Newtonsoft.Json serialization converter for the AngleString class.
	/// </summary>
	public class AngleStringConverter : JsonConverter<AngleString>
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
		//* ReadJson																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Read the value from the JSON stream.
		/// </summary>
		/// <param name="reader">
		/// Reference to the active JSON reader.
		/// </param>
		/// <param name="objectType">
		/// Reference to the type of object being handled.
		/// </param>
		/// <param name="existingValue">
		/// Reference to any existing value in the target.
		/// </param>
		/// <param name="hasExistingValue">
		/// Value indicating whether a value already exists at the target.
		/// </param>
		/// <param name="serializer">
		/// Reference to the active JSON serializer handling this activity.
		/// </param>
		/// <returns>
		/// Reference to the newly converted object.
		/// </returns>
		public override AngleString ReadJson(JsonReader reader, Type objectType,
			AngleString existingValue, bool hasExistingValue,
			JsonSerializer serializer)
		{
			return (string)reader.Value;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//* WriteJson																															*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Write JSON content for this item to the stream.
		/// </summary>
		/// <param name="writer">
		/// Reference to the active JSON writer.
		/// </param>
		/// <param name="value">
		/// Value to serialize.
		/// </param>
		/// <param name="serializer">
		/// Reference to the active serializer.
		/// </param>
		public override void WriteJson(JsonWriter writer, AngleString value,
			JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*


}
