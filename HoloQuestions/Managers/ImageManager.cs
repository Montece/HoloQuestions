using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace HoloQuestions
{
    public static class ImageManager
    {
        public static int BlurSize { get; set; } = 8;

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

            using (Graphics graphics = Graphics.FromImage(blurred))
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            BitmapData blurredData = blurred.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, blurred.PixelFormat);

            int bitsPerPixel = Image.GetPixelFormatSize(blurred.PixelFormat);

            byte* scan0 = (byte*)blurredData.Scan0.ToPointer();

            for (int xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
            {
                for (int yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
                {
                    int avgR = 0, avgG = 0, avgB = 0;
                    int blurPixelCount = 0;

                    for (int x = xx; (x < xx + BlurSize && x < image.Width); x++)
                    {
                        for (int y = yy; (y < yy + BlurSize && y < image.Height); y++)
                        {
                            byte* data = scan0 + y * blurredData.Stride + x * bitsPerPixel / 8;

                            avgB += data[0];
                            avgG += data[1];
                            avgR += data[2];

                            blurPixelCount++;
                        }
                    }

                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;

                    for (int x = xx; x < xx + BlurSize && x < image.Width && x < rectangle.Width; x++)
                    {
                        for (int y = yy; y < yy + BlurSize && y < image.Height && y < rectangle.Height; y++)
                        {
                            byte* data = scan0 + y * blurredData.Stride + x * bitsPerPixel / 8;

                            data[0] = (byte)avgB;
                            data[1] = (byte)avgG;
                            data[2] = (byte)avgR;
                        }
                    }
                }
            }

            blurred.UnlockBits(blurredData);

            return blurred;
        }
    }
}
