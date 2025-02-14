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

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	MeasurementTokenType																										*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of measurement token types.
	/// </summary>
	public enum MeasurementTokenType
	{
		/// <summary>
		/// No token type specified or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// Forward slash used as a fraction separator symbol, as in 5/16.
		/// </summary>
		Fraction,
		/// <summary>
		/// Floating point number where leading decimal point with no leading
		/// zero is legal.
		/// </summary>
		Number,
		/// <summary>
		/// An opening or closing parenthesis symbol.
		/// </summary>
		Parenthesis,
		/// <summary>
		/// Mathematical operator or parenthesis.
		/// </summary>
		Symbol,
		/// <summary>
		/// Unit specification.
		/// </summary>
		Unit
	}
	//*-------------------------------------------------------------------------*

}
