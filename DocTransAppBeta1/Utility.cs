using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DocTransAppBeta1
{
    /// <summary>
    /// 公用工具类
    /// </summary>
    public static class Utility
    {
        //public static class DllExportCall
        //{
        //    public const IntPtr STD_OUTPUT_HANDLE = -11;
        //    [DllImport("Kernel32.dll")]
        //    //闭嘴函数1
        //    public extern static bool SetStdHandle(IntPtr nStdHandle, SafeHandleZeroOrMinusOneIsInvalid handle);
        //    [DllImport("Kernel32.dll")]
        //    //闭嘴函数2
        //    public extern static SafePipeHandle GetStdHandle(IntPtr nStdHandle);
        //}
        /// <summary>
        /// 文件/图片哈希计算
        /// </summary>
        public static class HashCompute
        {
            /// <summary>
            /// 位图MD5
            /// </summary>
            /// <param name="img"></param>
            /// <returns></returns>
            public static string CalculateImageMD5(Image img)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;
                    byte[] hashBytes;
                    using (HashAlgorithm algorithm = MD5.Create())
                    {
                        hashBytes = algorithm.ComputeHash(ms);
                    }

                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hashBytes)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            /// <summary>
            /// 文件MD5
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public static string CalculateFileMD5(string path)
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] hashBytes;
                    using (HashAlgorithm algorithm = MD5.Create())
                    {
                        hashBytes = algorithm.ComputeHash(fs);
                    }

                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hashBytes)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            /// <summary>
            /// 位图SHA1
            /// </summary>
            /// <param name="img"></param>
            /// <returns></returns>
            public static string CalculateImageSHA1(Image img)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;
                    byte[] hashBytes;
                    using (HashAlgorithm algorithm = SHA1.Create())
                    {
                        hashBytes = algorithm.ComputeHash(ms);
                    }

                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hashBytes)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
            /// <summary>
            /// 文件SHA1
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public static string CalculateFileSHA1(string path)
            {
                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] hashBytes;
                    using (HashAlgorithm algorithm = SHA1.Create())
                    {
                        hashBytes = algorithm.ComputeHash(fs);
                    }

                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hashBytes)
                    {
                        sb.Append(b.ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }
        /// <summary>
        /// 图片服务和字体分析
        /// </summary>
        public static class ImageAndFontAnalysis
        {
            /// <summary>
            /// 颜色翻译器
            /// </summary>
            public static class ColorTranslator
            {
                public static Color FromHtml(string htmlColor)
                {
                    if (htmlColor.StartsWith("#"))
                    {
                        return ColorTranslator.FromHtmlWithoutHash(htmlColor.Substring(1));
                    }
                    throw new ArgumentException("Fatal Error: invalid html color code.");
                }

                private static Color FromHtmlWithoutHash(string htmlColor)
                {
                    switch (htmlColor.Length)
                    {
                        case 3:
                            return Color.FromArgb(
                                int.Parse(htmlColor[0].ToString() + htmlColor[0], System.Globalization.NumberStyles.HexNumber),
                                int.Parse(htmlColor[1].ToString() + htmlColor[1], System.Globalization.NumberStyles.HexNumber),
                                int.Parse(htmlColor[2].ToString() + htmlColor[2], System.Globalization.NumberStyles.HexNumber));
                        case 6:
                            return Color.FromArgb(
                                int.Parse(htmlColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                                int.Parse(htmlColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                                int.Parse(htmlColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
                        default:
                            throw new ArgumentException("Fatal Error: invalid html color code.");
                    }
                }
            }
            /// <summary>
            /// 图片裁切
            /// </summary>
            /// <param name="src">图片源</param>
            /// <param name="cropped">要裁切的矩形</param>
            /// <param name="margin">白边宽度</param>
            /// <returns></returns>
            public static Image CropImageByRectangle(Image src, Rectangle cropped, int margin = 0)
            {
                // 加载源图像
                using (Bitmap originalImage = new Bitmap(src))
                {
                    // 确保矩形区域不超出图像边界
                    int x = Math.Max(0, cropped.X);
                    int y = Math.Max(0, cropped.Y);
                    int width = Math.Min(cropped.Width, originalImage.Width - x);
                    int height = Math.Min(cropped.Height, originalImage.Height - y);

                    // 创建一个新的位图对象用于存储裁剪后的图像
                    Bitmap croppedImage = new Bitmap(width + margin * 2, height + margin * 2);

                    // 使用 Graphics 对象进行裁剪
                    using (Graphics g = Graphics.FromImage(croppedImage))
                    {
                        g.Clear(Color.White);
                        g.DrawImage(originalImage, new Rectangle(margin, margin, width, height),
                                    new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
                    }

                    return croppedImage;
                }
            }
            /// <summary>
            /// 按行分析字体
            /// </summary>
            /// <param name="height">总高</param>
            /// <param name="content">文本（要分行的）</param>
            /// <param name="line_dist">行间距像素个数</param>
            /// <param name="margin">边距</param>
            /// <returns></returns>
            public static int ImageFontSizeAnalyzeLinear(int height, string content, int line_dist = 0, int margin = 0)
            {
                if (string.IsNullOrEmpty(content)) return 0;
                //width -= margin;
                height -= margin;
                int str_length = content.Trim().Length;
                string[] lines = content.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

                return height / (lines.Length + line_dist) / 4;
            }
            public static int ImageFontSizeAnalyzeGuess(int width, int height, string content, int margin = 0)
            {
                //300dpi 4pix per fontsz
                width -= margin;
                height -= margin;
                int str_length = content.Trim().Length;

                //SortedDictionary<int, int> hashSet = new SortedDictionary<int, int>();
                int diff = int.MaxValue;
                int font_size = 0;
                if (str_length <= 0) return font_size;
                for (int i = 1; i <= 100; i++)
                {
                    int font_diff = Math.Abs((width / (8 * i)) * (height / (8 * i)) - str_length);
                    if (diff > font_diff)
                    {
                        diff = font_diff;
                        font_size = i;
                    }
                }
                return font_size * 2;
            }
        }
    }
}
