/*
 * Copyright (C) 2024 Valmet Automation Oy
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Aros.Gfx
{
	public partial class Bitmap
	{
		public void WritePixel(int argb, int x, int y)
		{
			if (IsInside(x, y) == Clipping.Inside)
			{
				switch (Format)
				{
					case PixelFormat.LUT8:
						WritePixelLUT8((byte)argb, x, y);
						break;

					case PixelFormat.ARGB32:
						WritePixelARGB(argb, x, y);
						break;

					default:
						throw new NotImplementedException();
				}
			}
		}

		private void WritePixelLUT8(byte color, int x, int y)
		{
			ImgBuffer[y * Modulo + x] = color;
		}

		private void WritePixelARGB(int color, int x, int y)
		{
			var bytes = new Span<byte>(ImgBuffer);
			var span = MemoryMarshal.Cast<byte, int>(bytes);
			var m = Modulo / 4;

			span[y * m + x] = color;
		}
	}
}
