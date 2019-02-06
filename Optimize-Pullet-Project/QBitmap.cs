using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab5
{
    static class QBitmap
    {

        public static Bitmap PrepareBitmap(Bitmap bitmap)
        {
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, Color.FromArgb(1, bitmap.GetPixel(x, y)));
                }
            }
            return bitmap;
        }


        public static long ColorEST(Color lc, Color rc)
        {
            return (long)(Math.Pow(lc.R - rc.R, 2.0) + Math.Pow(lc.G - rc.G, 2.0) + Math.Pow(lc.B - rc.B, 2.0));
        }

        public static long BitmapEST(Bitmap lb, Bitmap rb)
        {
            long error = 0;
            for (int x = 0; x < lb.Width; x++)
            {
                for (int y = 0; y < lb.Height; y++)
                {
                    error += ColorEST(lb.GetPixel(x, y), rb.GetPixel(x, y));
                }
            }
            return error;
        }

        
        public static Bitmap LinearQuantization(Bitmap bitmap, int numberOfColor)
        {
            Bitmap result = new Bitmap(bitmap.Width, bitmap.Height);
            byte trimDegree = (byte)Math.Pow(2, ((24 - (byte)Math.Log(numberOfColor, 2)) / 3));

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    color = Color.FromArgb(color.A, color.R - color.R % trimDegree, color.G - color.G % trimDegree, color.B - color.B % trimDegree);
                    result.SetPixel(i, j, color);
                }
            }
            return result;
        }


        public static Bitmap PopularQuantization(Bitmap bitmap, int numberOfColor)
        {
            return ApplyPalette(bitmap, GetPopularColors(bitmap, numberOfColor));
        }

        public static List<Color> GetPopularColors(Bitmap bitmap, int numberOfColor)
        {
            Dictionary<Color, long> colors = new Dictionary<Color, long>();
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color color = bitmap.GetPixel(x, y);
                    if (colors.Keys.Contains(color)) colors[color]++;
                    else colors.Add(color, 1);
                }
            }
            return colors.OrderBy(entry => entry.Value).Select(x => x.Key).Take(numberOfColor).ToList();
        }

        public static Bitmap ApplyPalette(Bitmap bitmap, List<Color> palette)
        {
            Bitmap result = new Bitmap(bitmap.Width, bitmap.Height);

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color nowColor = bitmap.GetPixel(x, y);
                    Color newColor = palette[0];
                    double est = ColorEST(newColor, nowColor);
                    foreach (Color tryColor in palette)
                    {
                        double tryEST = ColorEST(nowColor, tryColor);
                        if (tryEST < est)
                        {
                            newColor = tryColor;
                            est = tryEST;
                        }
                    }
                    result.SetPixel(x, y, newColor);
                }
            }
            return result;

        }

        public static Bitmap Tonning(Bitmap baseImage, Bitmap picture2)
        {
            Bitmap editImage = new Bitmap(baseImage.Width, baseImage.Height);

            for (int x = 0; x < baseImage.Width; x++)
            {
                editImage.SetPixel(x, 0, picture2.GetPixel(x, 0));
            }

            for (int y = 0; y < baseImage.Height; y++)
            {
                for (int x = 0; x < baseImage.Width; x++)
                {
                    Color color1 = baseImage.GetPixel(x, y);
                    Color color2 = editImage.GetPixel(x, y);
                    int errorR = color1.R - color2.R;
                    int errorG = color1.G - color2.G;
                    int errorB = color1.B - color2.B;

                    if (y != baseImage.Height - 1)
                    {
                        if (x != 0)
                        {
                            editImage.SetPixel(x - 1, y + 1, Color.FromArgb(color2.A, (int)(color2.R + 0.25 * errorR),
                                                                     (int)(color2.G + 0.25 * errorG),
                                                                     (int)(color2.B + 0.25 * errorB)));
                        }
                        editImage.SetPixel(x, y + 1, Color.FromArgb(color2.A, (int)(color2.R + 0.25 * errorR),
                                                                     (int)(color2.G + 0.25 * errorG),
                                                                     (int)(color2.B + 0.25 * errorB)));
                    }
                    if (x != baseImage.Width - 1)
                    {
                        if (y != baseImage.Height - 1)
                        {
                            editImage.SetPixel(x + 1, y + 1, Color.FromArgb(color2.A, (int)(color2.R + 0.25 * errorR),
                                             (int)(color2.G + 0.25 * errorG),
                                             (int)(color2.B + 0.25 * errorB)));
                        }
                        editImage.SetPixel(x + 1, y, Color.FromArgb(color2.A, (int)(color2.R + 0.25 * errorR),
                                             (int)(color2.G + 0.25 * errorG),
                                             (int)(color2.B + 0.25 * errorB)));
                    }
                }
            }
            return editImage;
        }


    }
}
