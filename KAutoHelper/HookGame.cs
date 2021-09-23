// Decompiled with JetBrains decompiler
// Type: KAutoHelper.HookGame
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System;
using System.Runtime.InteropServices;

namespace KAutoHelper
{
  public static class HookGame
  {
    [DllImport("khook.dll", SetLastError = true)]
    public static extern int InjectDll(IntPtr gameHwnd);

    [DllImport("khook.dll", SetLastError = true)]
    public static extern int UnmapDll(IntPtr gameHwnd);

    [DllImport("khook.dll", SetLastError = true)]
    public static extern uint GetMsg();
  }
}
