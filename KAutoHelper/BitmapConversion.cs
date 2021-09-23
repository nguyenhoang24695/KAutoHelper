// Decompiled with JetBrains decompiler
// Type: KAutoHelper.BitmapConversion
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;

namespace KAutoHelper
{
  public static class BitmapConversion
  {
    public static BitmapSource BitmapToBitmapSource(Bitmap source) => source == null ? (BitmapSource) null : System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(source.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
  }
}
