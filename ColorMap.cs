/*
 * Copyright (C) 2024 Valmet Automation Oy
 */

using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aros.Gfx
{
	/// <summary>
	/// Defines a color map for indexed bitmaps.
	/// </summary>
	public class ColorMap
	{
		/// <summary>
		/// Array of Color structures.
		/// </summary>
		public readonly Color[] Entries;

		/// <summary>
		/// Create new color map.
		/// The default color map is filled with gray values.
		/// </summary>
		/// <param name="entries"></param>
		public ColorMap(int entries)
		{
			Entries = GC.AllocateUninitializedArray<Color>(entries);

			for (int i = 0; i < entries; i++)
			{
				Entries[i] = Color.FromArgb(255, i, i, i);
			}
		}
	}
}
