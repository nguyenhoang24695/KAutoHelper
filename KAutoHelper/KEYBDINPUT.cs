// Decompiled with JetBrains decompiler
// Type: KAutoHelper.KEYBDINPUT
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System;

namespace KAutoHelper
{
  public struct KEYBDINPUT
  {
    public ushort Vk;
    public ushort Scan;
    public uint Flags;
    public uint Time;
    public IntPtr ExtraInfo;
  }
}
