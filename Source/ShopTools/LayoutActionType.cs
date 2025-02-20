using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopTools
{
	//*-------------------------------------------------------------------------*
	//*	LayoutActionType																												*
	//*-------------------------------------------------------------------------*
	/// <summary>
	/// Enumeration of recognized layout actions.
	/// </summary>
	public enum LayoutActionType
	{
		/// <summary>
		/// No layout action defined or unknown.
		/// </summary>
		None = 0,
		/// <summary>
		/// Plot an arc.
		/// </summary>
		DrawArc,
		/// <summary>
		/// Plot an ellipse.
		/// </summary>
		DrawEllipse,
		/// <summary>
		/// Plot a line.
		/// </summary>
		DrawLine,
		/// <summary>
		/// Plot a rectangle.
		/// </summary>
		DrawRectangle,
		/// <summary>
		/// Plot a filled ellipse.
		/// </summary>
		FillEllipse,
		/// <summary>
		/// Plot a filled rectangle.
		/// </summary>
		FillRectangle,
		/// <summary>
		/// Move the tool.
		/// </summary>
		Move,
		/// <summary>
		/// Drill a hole.
		/// </summary>
		Point
	}
	//*-------------------------------------------------------------------------*

}
