using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aros.Gfx
{
	public partial class Bitmap
	{
		public Bitmap Shrink(int divider)
		{
			var bm = new Bitmap(Width / divider, Height / divider, Format, BitmapFlags.DontClear, Modulo);

			switch (Format)
			{
				case PixelFormat.LUT8:
					throw new NotImplementedException();
					break;

				case PixelFormat.RGB24:
				case PixelFormat.ARGB32:
				default:
					throw new NotImplementedException();
			}

			return bm;
		}
	}
}
