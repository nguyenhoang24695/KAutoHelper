// Decompiled with JetBrains decompiler
// Type: KAutoHelper.Get_Text_From_Image
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace KAutoHelper
{
    public class Get_Text_From_Image
    {
        private static int saisot = 5;
        private static int red = 217;
        private static int collor_Byte_Start = 160;
        private static string path_langue = "C:\\";
        private static string TempFolder = "image_temp";
        private static string StandarFolder = "image_standand";
        private static List<Color> TemplateColors = new List<Color>()
    {
      Color.FromArgb((int) byte.MaxValue, 0, 0, 0)
    };

        public static void information(string Path_Langue) => Get_Text_From_Image.path_langue = Path_Langue;

        public static string Get_Text(Bitmap Bm_image_sour)
        {
            Bitmap image = (Bitmap)Bm_image_sour.Clone();
            Bm_image_sour.Dispose();
            return Get_Text_From_Image.Get_Text(Get_Text_From_Image.split_image(image));
        }

        public static Bitmap make_new_image(Bitmap Bm_image_sour)
        {
            int _width = Bm_image_sour.Width;
            int _height = Bm_image_sour.Height;
            Bitmap Bm_image = new Bitmap(_width, _height);
            int num = 230;
            for (int collorByteStart = Get_Text_From_Image.collor_Byte_Start; collorByteStart < num; ++collorByteStart)
            {
                Get_Text_From_Image.red = collorByteStart;
                Get_List_Point();
            }
            return Bm_image;

            void Get_List_Point()
            {
                for (int x = 0; x < _width; ++x)
                {
                    for (int y = 0; y < _height; ++y)
                    {
                        if (Check_sailenh_Color(Bm_image_sour.GetPixel(x, y), Get_Text_From_Image.TemplateColors, Get_Text_From_Image.saisot))
                        {
                            try
                            {
                                Bm_image.SetPixel(x, y, Color.Black);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }
        }

        private static bool Check_sailenh_Color(Color indexColor, List<Color> templateColor, int sailech)
        {
            bool flag = false;
            foreach (Color color in templateColor)
            {
                if ((int)indexColor.R + sailech >= (int)color.R && (int)indexColor.R - sailech <= (int)color.R && ((int)indexColor.G + sailech >= (int)color.G && (int)indexColor.G - sailech <= (int)color.G) && ((int)indexColor.B + sailech >= (int)color.B && (int)indexColor.B - sailech <= (int)color.B))
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }

        public static int split_image(Bitmap image, string name = "")
        {
            image.Save("aaa.png");
            int cout_picture = 0;
            bool flag = false;
            int width_start = 0;
            int width_stop = 0;
            int _height_top = 200;
            int _height_bottom = 0;
            int width = image.Width;
            int height = image.Height;
            for (int x = 0; x < width; ++x)
            {
                int num = 0;
                for (int y = 0; y < height; ++y)
                {
                    if (image.GetPixel(x, y).Name != "ff000000")
                    {
                        ++num;
                        if (_height_top > y)
                            _height_top = y;
                        if (_height_bottom < y)
                            _height_bottom = y;
                    }
                }
                if (num > 1 && !flag)
                {
                    width_start = x - 1;
                    flag = true;
                }
                if (num < 1 && flag)
                {
                    width_stop = x + 1;
                    flag = false;
                    save_image_splip();
                    cout_picture++;
                    _height_top = 200;
                    _height_bottom = 0;
                }
            }
            return cout_picture;

            void save_image_splip()
            {
                width = width_stop - width_start;
                height = _height_bottom - _height_top;
                Bitmap bitmap = new Bitmap(width, height);
                for (int x = 0; x < width; ++x)
                {
                    for (int y = 0; y < height; ++y)
                    {
                        try
                        {
                            Color pixel = image.GetPixel(width_start + x, _height_top + y);
                            bitmap.SetPixel(x, y, pixel);
                        }
                        catch
                        {
                        }
                    }
                }
                string tempFolder = Get_Text_From_Image.TempFolder;
                Get_Text_From_Image.check_folder_exists(tempFolder);
                string filename = tempFolder + "\\" + name + cout_picture.ToString() + ".jpg";
                bitmap.Save(filename);
                bitmap.Dispose();
            }
        }

        protected static string Get_Text(int cout_picture)
        {
            string str1 = "";
            List<string> stringList = new List<string>()
      {
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9"
      };
            for (int index1 = 0; index1 < cout_picture; ++index1)
            {
                List<double> doubleList = new List<double>();
                for (int index2 = 0; index2 < stringList.Count; ++index2)
                {
                    try
                    {
                        string str2 = stringList[index2];
                        double num1 = 0.0;
                        foreach (FileSystemInfo file in new DirectoryInfo(Get_Text_From_Image.StandarFolder + "\\" + str2).GetFiles())
                        {
                            Bitmap standand = new Bitmap(file.FullName);
                            Bitmap main = new Bitmap(Get_Text_From_Image.TempFolder + "\\" + index1.ToString() + ".jpg");
                            double num2 = Get_Text_From_Image.Image_Equal(main, standand);
                            standand.Dispose();
                            main.Dispose();
                            if (num2 > num1)
                                num1 = num2;
                        }
                        doubleList.Add(num1);
                    }
                    catch
                    {
                    }
                }
                int index3 = 0;
                double num = 0.0;
                for (int index2 = 0; index2 < stringList.Count; ++index2)
                {
                    if (num < doubleList[index2])
                    {
                        num = doubleList[index2];
                        index3 = index2;
                    }
                }
                str1 += stringList[index3];
            }
            return str1;
        }

        public static double Image_Equal(Bitmap main, Bitmap standand)
        {
            double num1 = 0.0;
            double num2 = 0.0;
            Bitmap bitmap = new Bitmap((Image)main, new Size(standand.Width, standand.Height));
            for (int x = 0; x < standand.Width; ++x)
            {
                for (int y = 0; y < standand.Height; ++y)
                {
                    ++num1;
                    if (bitmap.GetPixel(x, y).Equals((object)standand.GetPixel(x, y)))
                        ++num2;
                }
            }
            return num2 / num1;
        }

        protected static void check_folder_exists(string path)
        {
            if (Directory.Exists(path))
                return;
            Directory.CreateDirectory(path);
        }
    }
}
