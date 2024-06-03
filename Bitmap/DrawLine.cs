/*
 * Copyright (C) 2024 Valmet Automation Oy
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Aros.Gfx
{
	public partial class Bitmap
	{
		private enum Clipping
		{
			Inside,
			Left,
			Right,
			Top,
			Bottom
		}

		private Clipping IsInside(int x, int y)
		{
			Clipping c = Clipping.Inside;

			if (x < 0)
				c |= Clipping.Left;
			else if (x >= Width)
				c |= Clipping.Right;

			if (y < 0)
				c |= Clipping.Top;
			else if (y >= Height)
				c |= Clipping.Bottom;

			return c;
		}

		public void DrawLine(Color color, int x1, int y1, int x2, int y2)
		{
			DrawLine(color.ToArgb(), x1, y1, x2, y2);
		}

		public void DrawLine(int color, int x1, int y1, int x2, int y2)
		{
			int x_min = 0, y_min = 0;
			int x_max = Width - 1, y_max = Height - 1;

			var c1 = IsInside(x1, y1);
			var c2 = IsInside(x2, y2);

			bool drawLine = true;

			while (true)
			{
				if (c1 == Clipping.Inside && c2 == Clipping.Inside)
					break;

				if ((c1 & c2) == c1)	// Both outside rectangle
				{
					drawLine = false;
					break;
				}

				Clipping clipping;
				int x = 0, y = 0;

				if (c1 != Clipping.Inside)
					clipping = c1;
				else
					clipping = c2;

				if (clipping.HasFlag(Clipping.Bottom))
				{
					x = x1 + (x2 - x1) * (y_max - y1) / (y2 - y1);
					y = y_max;
				}
				else if (clipping.HasFlag(Clipping.Top))
				{
					x = x1 + (x2 - x1) * (y_min - y1) / (y2 - y1);
					y = y_min;
				}
				else if (clipping.HasFlag(Clipping.Right))
				{
					y = y1 + (y2 - y1) * (x_max - x1) / (x2 - x1);
					x = x_max;
				}
				else if (clipping.HasFlag(Clipping.Left))
				{
					y = y1 + (y2 - y1) * (x_min - x1) / (x2 - x1);
					x = x_min;
				}

				if (clipping == c1)
				{
					x1 = x;
					y1 = y;
					c1 = IsInside(x1, y1);
				}
				else
				{
					x2 = x;
					y2 = y;
					c2 = IsInside(x2, y2);
				}
			}

			if (drawLine)
			{
				int dx = Math.Abs(x2 - x1), sx = x1 < x2 ? 1 : -1;
				int dy = Math.Abs(y2 - y1), sy = y1 < y2 ? 1 : -1;
				int err = (dx > dy ? dx : -dy) / 2;

				switch (Format)
				{
					case PixelFormat.LUT8:
						DrawLineLUT8((byte)color, ref x1, ref y1, x2, y2, dx, sx, dy, sy, err);
						break;

					default:
						throw new NotImplementedException();
				}
			}
		}

		private void DrawLineLUT8(byte color, ref int x0, ref int y0, int x1, int y1, int dx, int sx, int dy, int sy, int err)
		{
			while (true)
			{
				WritePixelLUT8(color, x0, y0);

				if (x0 == x1 && y0 == y1)
					break;

				int e2 = err;
				if (e2 > -dx) { err -= dy; x0 += sx; }
				if (e2 < dy) { err += dx; y0 += sy; }
			}
		}
	}
}
