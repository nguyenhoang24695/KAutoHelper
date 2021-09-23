// Decompiled with JetBrains decompiler
// Type: KAutoHelper.AutoClient
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System;

namespace KAutoHelper
{
  public class AutoClient
  {
    public static int cmd_start = 1000;
    public static int cmd_end = 1001;
    public static int cmd_push = 1002;
    public const int cmd_sendchar = 1003;
    public IntPtr WindowHwnd;
    public uint processId;
    public uint HookMsg;
    private bool _isInjected = false;

    public void Attach(IntPtr hwnd)
    {
      this.WindowHwnd = hwnd;
      int windowThreadProcessId = (int) MemoryHelper.GetWindowThreadProcessId(this.WindowHwnd, out this.processId);
      MemoryHelper.OpenProcess(this.processId);
    }

    public bool isInjected => this._isInjected;

    public int Inject()
    {
      int num = HookGame.InjectDll(this.WindowHwnd);
      if (num == 1)
      {
        this._isInjected = true;
        this.HookMsg = HookGame.GetMsg();
      }
      return num;
    }

    public int DeInject()
    {
      int num = HookGame.UnmapDll(this.WindowHwnd);
      this._isInjected = false;
      return num;
    }
  }
}
