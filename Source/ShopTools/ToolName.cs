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

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	ToolName																																*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// A string depicting the name of a selected tool.
	/// </summary>
	public class ToolName
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
		//*	_Implicit ToolName = string																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the string instance to an ToolName.
		/// </summary>
		public static implicit operator ToolName(string value)
		{
			ToolName result = new ToolName();

			result.mValue = value;
			return result;
		}
		//*-----------------------------------------------------------------------*

		//*-----------------------------------------------------------------------*
		//*	_Implicit string = ToolName																						*
		//*-----------------------------------------------------------------------*
		/// <summary>
		/// Cast the ToolName instance to a string.
		/// </summary>
		public static implicit operator string(ToolName value)
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
	//*	ToolNameConverter																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Newtonsoft.Json serialization converter for the ToolName class.
	/// </summary>
	public class ToolNameConverter : JsonConverter<ToolName>
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
		public override ToolName ReadJson(JsonReader reader,
			Type objectType, ToolName existingValue, bool hasExistingValue,
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
		public override void WriteJson(JsonWriter writer, ToolName value,
			JsonSerializer serializer)
		{
			writer.WriteValue(value.ToString());
		}
		//*-----------------------------------------------------------------------*

	}
	//*-------------------------------------------------------------------------*

}
