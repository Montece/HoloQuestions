using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holo
{
    public static class Utility
    {
        private const int BLUR_SIZE = 8;
        private static readonly Random random = new Random(Guid.NewGuid().GetHashCode());

        public static void BlurImage(string path)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                FileStream fs = new FileStream(Path.ChangeExtension(path, "temp"), FileMode.Open, FileAccess.Read);
                fs.CopyTo(ms);
                fs.Close();
                fs.Dispose();

                Bitmap bitmap = new Bitmap(ms);
                bitmap = Blur(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));

                bitmap.Save(path);
                bitmap.Dispose();
                File.Delete(Path.ChangeExtension(path, "temp"));
            }
        }

        private unsafe static Bitmap Blur(Bitmap image, Rectangle rectangle)
        {
            Bitmap blurred = new Bitmap(image.Width, image.Height);

            // make an exact copy of the bitmap provided
            using (Graphics graphics = Graphics.FromImage(blurred))
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            // Lock the bitmap's bits
            BitmapData blurredData = blurred.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, blurred.PixelFormat);

            // Get bits per pixel for current PixelFormat
            int bitsPerPixel = Image.GetPixelFormatSize(blurred.PixelFormat);

            // Get pointer to first line
            byte* scan0 = (byte*)blurredData.Scan0.ToPointer();

            // look at every pixel in the blur rectangle
            for (int xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
            {
                for (int yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
                {
                    int avgR = 0, avgG = 0, avgB = 0;
                    int blurPixelCount = 0;

                    // average the color of the red, green and blue for each pixel in the
                    // blur size while making sure you don't go outside the image bounds
                    for (int x = xx; (x < xx + BLUR_SIZE && x < image.Width); x++)
                    {
                        for (int y = yy; (y < yy + BLUR_SIZE && y < image.Height); y++)
                        {
                            // Get pointer to RGB
                            byte* data = scan0 + y * blurredData.Stride + x * bitsPerPixel / 8;

                            avgB += data[0]; // Blue
                            avgG += data[1]; // Green
                            avgR += data[2]; // Red

                            blurPixelCount++;
                        }
                    }

                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;

                    // now that we know the average for the blur size, set each pixel to that color
                    for (int x = xx; x < xx + BLUR_SIZE && x < image.Width && x < rectangle.Width; x++)
                    {
                        for (int y = yy; y < yy + BLUR_SIZE && y < image.Height && y < rectangle.Height; y++)
                        {
                            // Get pointer to RGB
                            byte* data = scan0 + y * blurredData.Stride + x * bitsPerPixel / 8;

                            // Change values
                            data[0] = (byte)avgB;
                            data[1] = (byte)avgG;
                            data[2] = (byte)avgR;
                        }
                    }
                }
            }

            // Unlock the bits
            blurred.UnlockBits(blurredData);

            return blurred;
        }

        public static T GetRandomElement<T>(this List<T> list)
        {
            if (list.Count.Equals(0)) return default(T);
            return list[random.Next(0, list.Count)];
        }
    }
}
