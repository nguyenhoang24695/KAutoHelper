// Decompiled with JetBrains decompiler
// Type: KAutoHelper.MOUSEKEYBDHARDWAREINPUT
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System.Runtime.InteropServices;

namespace KAutoHelper
{
  [StructLayout(LayoutKind.Explicit)]
  public struct MOUSEKEYBDHARDWAREINPUT
  {
    [FieldOffset(0)]
    public HARDWAREINPUT Hardware;
    [FieldOffset(0)]
    public KEYBDINPUT Keyboard;
    [FieldOffset(0)]
    public MOUSEINPUT Mouse;
  }
}
