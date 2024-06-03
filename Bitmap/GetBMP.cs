/*
 * Copyright (C) 2024 Valmet Automation Oy
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aros.Gfx
{
	public partial class Bitmap
	{
		public void SaveBMP(string filename)
		{
			var bmp = GetBMP();
			File.WriteAllBytes(filename, bmp);
		}

		public byte[] GetBMP()
		{
			switch (Format)
			{
				case PixelFormat.LUT8:
					return Encoders.BMP.GetFromLUT8(ImgBuffer, Width, Height, Modulo, ColorMap ?? new ColorMap(256));

				case PixelFormat.ARGB32:
					return Encoders.BMP.GetFromARGB(ImgBuffer, Width, Height, Modulo);

				default:
					throw new NotImplementedException();
			}
		}
	}
}
