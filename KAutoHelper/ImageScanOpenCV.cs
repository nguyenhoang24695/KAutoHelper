// Decompiled with JetBrains decompiler
// Type: KAutoHelper.ImageScanOpenCV
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace KAutoHelper
{
    public class ImageScanOpenCV
    {
        public static Bitmap GetImage(string path) => new Bitmap(path);

        public static Bitmap Find(string main, string sub, double percent = 0.9)
        {
            ImageScanOpenCV.GetImage(main);
            ImageScanOpenCV.GetImage(sub);
            return ImageScanOpenCV.Find(main, sub, percent);
        }

        public static Bitmap Find(Bitmap mainBitmap, Bitmap subBitmap, double percent = 0.9)
        {
            Image<Bgr, byte> image1 = new Image<Bgr, byte>(mainBitmap);
            Image<Bgr, byte> image2 = new Image<Bgr, byte>(subBitmap);
            Image<Bgr, byte> image3 = image1.Copy();
            using (Image<Gray, float> image4 = image1.MatchTemplate(image2, (TemplateMatchingType)5))
            {
                double[] numArray1;
                double[] numArray2;
                Point[] pointArray1;
                Point[] pointArray2;
                image4.MinMax(out numArray1, out numArray2, out pointArray1, out pointArray2);
                if (numArray2[0] > percent)
                {
                    Rectangle rectangle = new Rectangle(pointArray2[0], ((CvArray<byte>)image2).Size);
                    image3.Draw(rectangle, new Bgr(System.Drawing.Color.Red), 2, (LineType)8, 0);
                }
                else
                    image3 = (Image<Bgr, byte>)null;
            }
            return image3 == null ? (Bitmap)null : image3.ToBitmap();
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static Point? FindOutPoint(Bitmap mainBitmap, Bitmap subBitmap, double percent = 0.9)
        {
            if (subBitmap == null || mainBitmap == null)
                return new Point?();
            if (subBitmap.Width > mainBitmap.Width || subBitmap.Height > mainBitmap.Height)
                return new Point?();
            Image<Bgr, byte> image1 = new Image<Bgr, byte>(mainBitmap);
            Image<Bgr, byte> image2 = new Image<Bgr, byte>(subBitmap);
            Point? nullable = new Point?();
            using (Image<Gray, float> image3 = image1.MatchTemplate(image2, (TemplateMatchingType)5))
            {
                double[] numArray1;
                double[] numArray2;
                Point[] pointArray1;
                Point[] pointArray2;
                image3.MinMax(out numArray1, out numArray2, out pointArray1, out pointArray2);
                if (numArray2[0] > percent)
                    nullable = new Point?(pointArray2[0]);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return nullable;
        }

        public static List<Point> FindOutPoints(
          Bitmap mainBitmap,
          Bitmap subBitmap,
          double percent = 0.9)
        {
            Image<Bgr, byte> image1 = new Image<Bgr, byte>(mainBitmap);
            Image<Bgr, byte> image2 = new Image<Bgr, byte>(subBitmap);
            List<Point> pointList = new List<Point>();
            while (true)
            {
                using (Image<Gray, float> image3 = image1.MatchTemplate(image2, (TemplateMatchingType)5))
                {
                    double[] numArray1;
                    double[] numArray2;
                    Point[] pointArray1;
                    Point[] pointArray2;
                    image3.MinMax(out numArray1, out numArray2, out pointArray1, out pointArray2);
                    if (numArray2[0] > percent)
                    {
                        Rectangle rectangle = new Rectangle(pointArray2[0], ((CvArray<byte>)image2).Size);
                        image1.Draw(rectangle, new Bgr(System.Drawing.Color.Blue), -1, (LineType)8, 0);
                        pointList.Add(pointArray2[0]);
                    }
                    else
                        break;
                }
            }
            return pointList;
        }

        public static List<Point> FindColor(Bitmap mainBitmap, System.Drawing.Color color)
        {
            int argb = color.ToArgb();
            List<Point> pointList = new List<Point>();
            using (Bitmap bitmap = mainBitmap)
            {
                for (int x = 0; x < bitmap.Width; ++x)
                {
                    for (int y = 0; y < bitmap.Height; ++y)
                    {
                        if (argb.Equals(bitmap.GetPixel(x, y).ToArgb()))
                            pointList.Add(new Point(x, y));
                    }
                }
            }
            return pointList;
        }

        public static List<Point> FindColor(Bitmap mainBitmap, string color)
        {
            System.Drawing.Color color1 = (System.Drawing.Color)System.Windows.Media.ColorConverter.ConvertFromString(color);
            return ImageScanOpenCV.FindColor(mainBitmap, color1);
        }

        public static void TestDilate(Bitmap bmp)
        {
            while (true)
            {
                Image<Gray, byte> image1 = new Image<Gray, byte>(bmp);
                ((CvArray<byte>)image1).Save("old.png");
                Image<Gray, byte> image2 = new Image<Gray, byte>(((CvArray<byte>)image1).Width, ((CvArray<byte>)image1).Height, new Gray((double)byte.MaxValue)).Sub(image1);
                ((CvArray<byte>)image2).Save("img23.png");
                Image<Gray, byte> image3 = new Image<Gray, byte>(((CvArray<byte>)image2).Size);
                Image<Gray, byte> image4 = new Image<Gray, byte>(((CvArray<byte>)image2).Size);
                Image<Gray, byte> image5 = new Image<Gray, byte>(((CvArray<byte>)image2).Size);
                ((CvArray<byte>)image5).SetValue(0.0, (CvArray<byte>)null);
                CvInvoke.Threshold((IInputArray)image2, (IOutputArray)image2, (double)sbyte.MaxValue, (double)byte.MaxValue, (ThresholdType)0);
                Mat structuringElement = CvInvoke.GetStructuringElement((ElementShape)0, new Size(2, 2), new Point(-1, -1));
                CvInvoke.Dilate((IInputArray)image1, (IOutputArray)image3, (IInputArray)structuringElement, new Point(-1, -1), 1, (BorderType)2, new MCvScalar());
                ((CvArray<byte>)image3).CopyTo((CvArray<byte>)image2);
                image5.Bitmap.Save("ele.png");
                ((CvArray<byte>)image2).Save("img2.png");
                ((CvArray<byte>)image3).Save("eroded.png");
                ((CvArray<byte>)image4).Save("temp.png");
            }
        }

        public static string RecolizeText(Bitmap img) => Get_Text_From_Image.Get_Text(img);

        public static void SplitImageInFolder(string folderPath)
        {
            foreach (FileInfo file in new DirectoryInfo(folderPath).GetFiles())
            {
                Bitmap bitmap = new Bitmap(file.FullName);
                Bitmap image = Get_Text_From_Image.make_new_image(new Image<Gray, byte>(bitmap).ToBitmap());
                bitmap.Dispose();
                Get_Text_From_Image.split_image(image, Path.GetFileNameWithoutExtension(file.Name));
            }
        }

        public static Bitmap ThreshHoldBinary(Bitmap bmp, byte threshold = 190) => new Image<Gray, byte>(bmp).ThresholdBinary(new Gray((double)threshold), new Gray((double)byte.MaxValue)).ToBitmap();

        public static Bitmap NotWhiteToTransparentPixelReplacement(Bitmap bmp)
        {
            bmp = ImageScanOpenCV.CreateNonIndexedImage((Image)bmp);
            for (int x = 0; x < bmp.Width; ++x)
            {
                for (int y = 0; y < bmp.Height; ++y)
                {
                    System.Drawing.Color pixel = bmp.GetPixel(x, y);
                    if (pixel.R > (byte)200 && pixel.G > (byte)200 && pixel.B > (byte)200)
                        bmp.SetPixel(x, y, System.Drawing.Color.Transparent);
                }
            }
            return bmp;
        }

        public static Bitmap WhiteToBlackPixelReplacement(Bitmap bmp)
        {
            bmp = ImageScanOpenCV.CreateNonIndexedImage((Image)bmp);
            for (int x = 0; x < bmp.Width; ++x)
            {
                for (int y = 0; y < bmp.Height; ++y)
                {
                    System.Drawing.Color pixel = bmp.GetPixel(x, y);
                    if (pixel.R > (byte)20 && pixel.G > (byte)230 && pixel.B > (byte)230)
                        bmp.SetPixel(x, y, System.Drawing.Color.Black);
                }
            }
            return bmp;
        }

        public static Bitmap TransparentToWhitePixelReplacement(Bitmap bmp)
        {
            bmp = ImageScanOpenCV.CreateNonIndexedImage((Image)bmp);
            for (int x = 0; x < bmp.Width; ++x)
            {
                for (int y = 0; y < bmp.Height; ++y)
                {
                    if (bmp.GetPixel(x, y).A >= (byte)1)
                        bmp.SetPixel(x, y, System.Drawing.Color.White);
                }
            }
            return bmp;
        }

        public static Bitmap CreateNonIndexedImage(Image src)
        {
            Bitmap bitmap = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics graphics = Graphics.FromImage((Image)bitmap))
                graphics.DrawImage(src, 0, 0);
            return bitmap;
        }
    }
}
