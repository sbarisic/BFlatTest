using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.IO;
using System.Text;

namespace FishFormats
{
	public unsafe struct FishImage
	{
		public fixed byte Name[128];
		public uint Width;
		public uint Height;
		public uint PixelDataLength;
		public byte[] PixelData;
	}

	internal unsafe class Program
	{
		static void Main(string[] args)
		{
			FishImage Img = ConvertPNG("in/boot.png");

		}

		static FishImage ConvertPNG(string InFile)
		{
			string FileName = Path.GetFileNameWithoutExtension(InFile);
			FishImage Img = new FishImage();
			fixed (byte* ptr = Encoding.ASCII.GetBytes(FileName))
			{
				for (int i = 0; i < 128; i++)
				{
					if (i < FileName.Length)
						Img.Name[i] = ptr[i];
					else
						Img.Name[i] = 0;
				}
			}

			using (Image<Rgba32> image = Image.Load<Rgba32>(InFile))
			{
				int width = image.Width;
				int height = image.Height;

				Img.Width = (uint)width;
				Img.Height = (uint)height;
				Img.PixelDataLength = Img.Width * Img.Height * 4;
				Img.PixelData = new byte[Img.PixelDataLength];

				fixed (byte* pixelDataPtr = Img.PixelData)
				{
					for (int y = 0; y < height; y++)
					{
						Span<Rgba32> pixelRow = image.GetPixelRowSpan(y);
						for (int x = 0; x < width; x++)
						{
							((Rgba32*)pixelDataPtr)[(y * width + x)] = pixelRow[x];
						}
					}
				}

				/*for (int y = 0; y < height; y++)
				{
					Span<Rgba32> pixelRow = image.GetPixelRowSpan(y);
					for (int x = 0; x < width; x++)
					{
						Rgba32 pixel = pixelRow[x];
						Console.WriteLine($"Pixel ({x},{y}): R={pixel.R}, G={pixel.G}, B={pixel.B}, A={pixel.A}");
					}
				}*/
			}

			return Img;
		}
	}
}
