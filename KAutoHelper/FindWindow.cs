// Decompiled with JetBrains decompiler
// Type: KAutoHelper.FindWindow
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace KAutoHelper
{
  public class FindWindow
  {
    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool EnumWindows(FindWindow.EnumWindowsProc enumProc, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    public static List<IntPtr> GetWindowHandles(string processName, string className)
    {
      List<IntPtr> handleList = new List<IntPtr>();
      Process[] processes = Process.GetProcessesByName(processName);
      Process proc = (Process) null;
      FindWindow.EnumWindows((FindWindow.EnumWindowsProc) ((hWnd, lParam) =>
      {
        int processId;
        FindWindow.GetWindowThreadProcessId(hWnd, out processId);
        proc = ((IEnumerable<Process>) processes).FirstOrDefault<Process>((Func<Process, bool>) (p => p.Id == processId));
        if (proc != null)
        {
          StringBuilder lpClassName = new StringBuilder(256);
          FindWindow.GetClassName(hWnd, lpClassName, 256);
          if (lpClassName.ToString() == className)
            handleList.Add(hWnd);
        }
        return true;
      }), IntPtr.Zero);
      return handleList;
    }

    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
  }
}
