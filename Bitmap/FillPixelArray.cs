/*
 * Copyright (C) 2024 Valmet Automation Oy
 */

using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Aros.Gfx
{
	public partial class Bitmap
	{
		public void FillPixelArray(Color color, int x, int y, int width, int height)
		{
			FillPixelArray(color.ToArgb(), x, y, width, height);
		}

		public void FillPixelArray(Color color, Rectangle rect)
		{
			FillPixelArray(color.ToArgb(), rect.Left, rect.Top, rect.Width, rect.Height);
		}

		/// <summary>
		/// Writes the same color value to all pixels in a rectangular region.
		/// </summary>
		/// <param name="argb"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <exception cref="NotImplementedException"></exception>
		public void FillPixelArray(int argb, int x, int y, int width, int height)
		{
			switch (Format)
			{
				case PixelFormat.LUT8: FillLUT8(argb, x, y, width, height); break;
				case PixelFormat.ARGB32: FillARGB(argb , x, y, width, height); break;
				default: throw new NotImplementedException();
			}
		}

		private void FillLUT8(int val, int x, int y, int height, int width)
		{
			for (int dsty = y; dsty < x + height && dsty < Height; dsty++)
			{
				for (int dstx = x; dstx < x + width && dstx < Width; dstx++)
				{
					ImgBuffer[y * Modulo + x] = (byte) val;
				}
			}
		}

		private void FillARGB(int val, int x, int y, int height, int width)
		{
			var bytes = new Span<byte>(ImgBuffer);
			var span = MemoryMarshal.Cast<byte, int>(bytes);
			var m = Modulo / 4;

			for (int dsty = y; dsty < x + height && dsty < Height; dsty++)
			{
				for (int dstx = x; dstx < x + width && dstx < Width; dstx++)
				{
					span[y * m + x] = val;
				}
			}
		}
	}
}
