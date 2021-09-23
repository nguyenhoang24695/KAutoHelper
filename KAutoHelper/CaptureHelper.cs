// Decompiled with JetBrains decompiler
// Type: KAutoHelper.CaptureHelper
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace KAutoHelper
{
  public class CaptureHelper
  {
    public static int X;
    public static int Y;

    public static Image CaptureScreen() => CaptureHelper.CaptureWindow(CaptureHelper.User32.GetDesktopWindow());

    public static Image CaptureWindow(IntPtr handle)
    {
      IntPtr windowDc = CaptureHelper.User32.GetWindowDC(handle);
      CaptureHelper.User32.RECT rect = new CaptureHelper.User32.RECT();
      CaptureHelper.User32.GetWindowRect(handle, ref rect);
      int nWidth = rect.right - rect.left;
      int nHeight = rect.bottom - rect.top;
      IntPtr compatibleDc = CaptureHelper.GDI32.CreateCompatibleDC(windowDc);
      IntPtr compatibleBitmap = CaptureHelper.GDI32.CreateCompatibleBitmap(windowDc, nWidth, nHeight);
      IntPtr hObject = CaptureHelper.GDI32.SelectObject(compatibleDc, compatibleBitmap);
      CaptureHelper.GDI32.BitBlt(compatibleDc, 0, 0, nWidth, nHeight, windowDc, 0, 0, 13369376);
      CaptureHelper.GDI32.SelectObject(compatibleDc, hObject);
      CaptureHelper.GDI32.DeleteDC(compatibleDc);
      CaptureHelper.User32.ReleaseDC(handle, windowDc);
      Image image = (Image) Image.FromHbitmap(compatibleBitmap);
      CaptureHelper.GDI32.DeleteObject(compatibleBitmap);
      return image;
    }

    public static Bitmap ScaleImage(Image a, double zoomin)
    {
      double num = zoomin;
      try
      {
        return CaptureHelper.ResizeImage(a, a.Size.Width + (int) ((double) a.Size.Width * num), a.Size.Height + (int) ((double) a.Size.Height * num));
      }
      catch (Exception ex)
      {
        return (Bitmap) null;
      }
    }

    public static Bitmap ResizeImage(Image image, int width, int height)
    {
      Rectangle destRect = new Rectangle(0, 0, width, height);
      Bitmap bitmap = new Bitmap(width, height);
      bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
      using (Graphics graphics = Graphics.FromImage((Image) bitmap))
      {
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
        using (ImageAttributes imageAttr = new ImageAttributes())
        {
          imageAttr.SetWrapMode(WrapMode.TileFlipXY);
          graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
        }
      }
      return bitmap;
    }

    public static void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format) => CaptureHelper.CaptureWindow(handle).Save(filename, format);

    public static void CaptureScreenToFile(string filename, ImageFormat format) => CaptureHelper.CaptureScreen().Save(filename, format);

    public static Bitmap CaptureImage(Size size, Point position)
    {
      try
      {
        Bitmap bitmap = new Bitmap(size.Width, size.Height, PixelFormat.Format32bppRgb);
        Graphics.FromImage((Image) bitmap).CopyFromScreen(position.X + CaptureHelper.X, position.Y + CaptureHelper.Y, 0, 0, size, CopyPixelOperation.SourceCopy);
        return bitmap;
      }
      catch
      {
        return (Bitmap) null;
      }
    }

    [DllImport("gdi32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DeleteObject([In] IntPtr hObject);

    public static Bitmap CropImage(Image img, Rectangle cropRect)
    {
      Bitmap bitmap1 = img as Bitmap;
      Bitmap bitmap2 = new Bitmap(cropRect.Width, cropRect.Height);
      using (Graphics graphics = Graphics.FromImage((Image) bitmap2))
        graphics.DrawImage((Image) bitmap1, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), cropRect, GraphicsUnit.Pixel);
      return bitmap2;
    }

    public static Bitmap CropImage(Bitmap img, Rectangle cropRect)
    {
      Bitmap bitmap1 = img;
      Bitmap bitmap2 = new Bitmap(cropRect.Width, cropRect.Height);
      using (Graphics graphics = Graphics.FromImage((Image) bitmap2))
        graphics.DrawImage((Image) bitmap1, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), cropRect, GraphicsUnit.Pixel);
      return bitmap2;
    }

    private class GDI32
    {
      public const int SRCCOPY = 13369376;

      [DllImport("gdi32.dll")]
      public static extern bool BitBlt(
        IntPtr hObject,
        int nXDest,
        int nYDest,
        int nWidth,
        int nHeight,
        IntPtr hObjectSource,
        int nXSrc,
        int nYSrc,
        int dwRop);

      [DllImport("gdi32.dll")]
      public static extern IntPtr CreateCompatibleBitmap(IntPtr hDC, int nWidth, int nHeight);

      [DllImport("gdi32.dll")]
      public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

      [DllImport("gdi32.dll")]
      public static extern bool DeleteDC(IntPtr hDC);

      [DllImport("gdi32.dll")]
      public static extern bool DeleteObject(IntPtr hObject);

      [DllImport("gdi32.dll")]
      public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);
    }

    private class User32
    {
      [DllImport("user32.dll")]
      public static extern IntPtr GetDesktopWindow();

      [DllImport("user32.dll")]
      public static extern IntPtr GetWindowDC(IntPtr hWnd);

      [DllImport("user32.dll")]
      public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

      [DllImport("user32.dll")]
      public static extern IntPtr GetWindowRect(
        IntPtr hWnd,
        ref CaptureHelper.User32.RECT rect);

      public struct RECT
      {
        public int left;
        public int top;
        public int right;
        public int bottom;
      }
    }
  }
}
