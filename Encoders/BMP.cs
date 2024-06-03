/*
 * Copyright (C) 2024 Valmet Automation Oy
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Aros.Gfx.Encoders
{
	public static class BMP
	{
		static public byte[] GetFromLUT8(byte[] src, int w, int h, int modulo, ColorMap cm)
		{
			var bytes = GC.AllocateUninitializedArray<byte>(w * h * 4);

			for (int y = 0; y < h; y++)
			{
				for (int i = 0; i < w; i++)
				{
					var idx = src[i + (h - y - 1) * modulo];
					var c = cm.Entries[idx];

					bytes[i * 4 + 0 + y * w * 4] = c.B;
					bytes[i * 4 + 1 + y * w * 4] = c.G;
					bytes[i * 4 + 2 + y * w * 4] = c.R;
					bytes[i * 4 + 3 + y * w * 4] = c.A;
				}
			}

			var b = GetBitmapBytes(bytes, w, h);
			return b;
		}

		static public byte[] GetFromARGB(byte[] src, int w, int h, int modulo)
		{
			var bytes = GC.AllocateUninitializedArray<byte>(w * h * 4);

			for (int y = 0; y < h; y++)
			{
				Array.Copy(src, (h - y - 1) * modulo, bytes, y * w, w);
			}

			var b = GetBitmapBytes(bytes, w, h);
			return b;
		}

		static private byte[] GetBitmapBytes(byte[] ImageBytes, int Width, int Height)
		{
			const int imageHeaderSize = 54;
			byte[] bmpBytes = new byte[ImageBytes.Length + imageHeaderSize];
			bmpBytes[0] = (byte)'B';
			bmpBytes[1] = (byte)'M';
			bmpBytes[14] = 40;
			Array.Copy(BitConverter.GetBytes(bmpBytes.Length), 0, bmpBytes, 2, 4);
			Array.Copy(BitConverter.GetBytes(imageHeaderSize), 0, bmpBytes, 10, 4);
			Array.Copy(BitConverter.GetBytes(Width), 0, bmpBytes, 18, 4);
			Array.Copy(BitConverter.GetBytes(Height), 0, bmpBytes, 22, 4);
			Array.Copy(BitConverter.GetBytes(32), 0, bmpBytes, 28, 2);
			Array.Copy(BitConverter.GetBytes(ImageBytes.Length), 0, bmpBytes, 34, 4);
			Array.Copy(ImageBytes, 0, bmpBytes, imageHeaderSize, ImageBytes.Length);
			return bmpBytes;
		}
	}
}
