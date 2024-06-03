/*
 * Copyright (C) 2024 Valmet Automation Oy
 */

using System.Drawing.Imaging;

namespace Aros.Gfx
{
	public partial class Bitmap
	{
		public ref struct PixBuf
		{
			public readonly byte[] Buffer;
			public readonly int Modulo;
			internal LockMode Mode = LockMode.ReadOnly;

			public PixBuf(byte[] buffer, int modulo)
			{
				Buffer = buffer;
				Modulo = modulo;
			}
		}

		public enum BitmapFlags
		{
			DontClear = 1,
			Pinned = 2,
		}

		public enum PixelFormat
		{
			LUT8,
			RGB24,
			ARGB32
		}

		public enum LockMode
		{
			ReadOnly = 0,
			ReadWrite = 1,
			WriteOnly = 2
		}

		public int Width { get; private set; }
		public int Height { get; private set; }
		public int Modulo { get; private set; }

		public PixelFormat Format { get; private set; }
		public BitmapFlags Flags { get; private set; }

		public ColorMap? ColorMap { get; private set; }

		private readonly byte[] ImgBuffer;

		private readonly ReaderWriterLockSlim ReaderWriterLock = new();

		public Bitmap(int width, int height, PixelFormat format, BitmapFlags flags = 0, int alignment = 0)
		{
			Width = width;
			Height = height;
			Format = format;
			Flags = flags;

			var bpp = GetBytesPerPixel(format);

			Modulo = alignment == 0 ? width * bpp : Math.Max(alignment, width * bpp);

			var size = Modulo * Height;

			if (flags.HasFlag(BitmapFlags.DontClear))
				ImgBuffer = GC.AllocateUninitializedArray<byte>(size, flags.HasFlag(BitmapFlags.Pinned));
			else
				ImgBuffer = GC.AllocateArray<byte>(size, flags.HasFlag(BitmapFlags.Pinned));
		}

		public static int GetBytesPerPixel(PixelFormat format)
		{
			switch (format)
			{
				case PixelFormat.LUT8: return 1;
				case PixelFormat.RGB24: return 3;
				case PixelFormat.ARGB32: return 4;
			}

			throw new ArgumentOutOfRangeException("Invalid pixel format.");
		}

		public PixBuf Lock(LockMode mode)
		{
			var pb = new PixBuf(ImgBuffer, Modulo);
			pb.Mode = mode;

			switch (mode)
			{
				case LockMode.ReadOnly: ReaderWriterLock.EnterReadLock(); break;
				default: ReaderWriterLock.EnterWriteLock(); break;
			}

			return pb;
		}

		public void Unlock(ref PixBuf pixBuf)
		{
			switch (pixBuf.Mode)
			{
				case LockMode.ReadOnly: ReaderWriterLock.ExitReadLock(); break;
				default: ReaderWriterLock.ExitWriteLock(); break;
			}
		}

		public void Clear()
		{
			ImgBuffer.SetValue(0, 0);
		}
	}
}
