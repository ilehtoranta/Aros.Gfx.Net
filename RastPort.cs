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
	public class RastPort
	{
		public Bitmap? Bitmap { get; set; }

		public RastPort(Bitmap? bmp = null)
		{
			Bitmap = bmp;
		}
	}
}
