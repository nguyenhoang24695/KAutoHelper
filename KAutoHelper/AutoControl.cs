// Decompiled with JetBrains decompiler
// Type: KAutoHelper.AutoControl
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KAutoHelper
{
  public class AutoControl
  {
    private static Random rand = new Random();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

    [DllImport("User32.dll")]
    public static extern bool EnumChildWindows(
      IntPtr hWndParent,
      AutoControl.CallBack lpEnumFunc,
      IntPtr lParam);

    [DllImport("User32.dll")]
    public static extern int GetWindowText(IntPtr hWnd, StringBuilder s, int nMaxCount);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    private static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(
      IntPtr hWnd,
      uint Msg,
      IntPtr wParam,
      IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(
      IntPtr hWnd,
      int Msg,
      IntPtr wParam,
      IntPtr lParam);

    [DllImport("user32.dll")]
    public static extern IntPtr SendMessage(
      IntPtr hWnd,
      int Msg,
      IntPtr wParam,
      string lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

    [DllImport("user32.dll")]
    private static extern IntPtr GetDlgItem(IntPtr hWnd, int nIDDlgItem);

    [DllImport("user32.dll")]
    private static extern bool SetDlgItemTextA(IntPtr hWnd, int nIDDlgItem, string gchar);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr FindWindowEx(
      IntPtr parentHandle,
      IntPtr childAfter,
      string lclassName,
      string windowTitle);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);

    [DllImport("user32.dll")]
    public static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

    [DllImport("user32")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool EnumChildWindows(
      IntPtr window,
      AutoControl.EnumWindowProc callback,
      IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    public static extern void mouse_event(
      uint dwFlags,
      int dx,
      int dy,
      int dwData,
      UIntPtr dwExtraInfo);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool MoveWindow(
      IntPtr hWnd,
      int X,
      int Y,
      int nWidth,
      int nHeight,
      bool bRepaint);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(
      uint numberOfInputs,
      INPUT[] inputs,
      int sizeOfInputStructure);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    public static IntPtr BringToFront(string className, string windowName = null)
    {
      IntPtr window = AutoControl.FindWindow(className, windowName);
      AutoControl.SetForegroundWindow(window);
      return window;
    }

    public static IntPtr BringToFront(IntPtr hWnd)
    {
      AutoControl.SetForegroundWindow(hWnd);
      return hWnd;
    }

    public static bool IsWindowVisible_(IntPtr handle) => AutoControl.IsWindowVisible(handle);

    public static IntPtr FindWindowHandle(string className, string windowName)
    {
      IntPtr zero = IntPtr.Zero;
      return AutoControl.FindWindow(className, windowName);
    }

    public static List<IntPtr> FindWindowHandlesFromProcesses(
      string className,
      string windowName,
      int maxCount = 1)
    {
      Process[] processes = Process.GetProcesses();
      List<IntPtr> numList = new List<IntPtr>();
      int num = 0;
      foreach (Process process in processes)
      {
        IntPtr mainWindowHandle = process.MainWindowHandle;
        string className1 = AutoControl.GetClassName(mainWindowHandle);
        string text = AutoControl.GetText(mainWindowHandle);
        if (className1 == className || text == windowName)
        {
          numList.Add(mainWindowHandle);
          if (num < maxCount)
            ++num;
          else
            break;
        }
      }
      return numList;
    }

    public static IntPtr FindWindowHandleFromProcesses(string className, string windowName)
    {
      Process[] processes = Process.GetProcesses();
      IntPtr num = IntPtr.Zero;
      foreach (Process process in ((IEnumerable<Process>) processes).Where<Process>((Func<Process, bool>) (p => p.MainWindowHandle != IntPtr.Zero)))
      {
        IntPtr mainWindowHandle = process.MainWindowHandle;
        string className1 = AutoControl.GetClassName(mainWindowHandle);
        string text = AutoControl.GetText(mainWindowHandle);
        if (className1 == className || text == windowName)
        {
          num = mainWindowHandle;
          break;
        }
      }
      return num;
    }

    public static IntPtr FindWindowExFromParent(
      IntPtr parentHandle,
      string text,
      string className)
    {
      return AutoControl.FindWindowEx(parentHandle, IntPtr.Zero, className, text);
    }

    private static IntPtr FindWindowByIndex(IntPtr hWndParent, int index)
    {
      if (index == 0)
        return hWndParent;
      int num = 0;
      IntPtr childAfter = IntPtr.Zero;
      do
      {
        childAfter = AutoControl.FindWindowEx(hWndParent, childAfter, "Button", (string) null);
        if (childAfter != IntPtr.Zero)
          ++num;
      }
      while (num < index && childAfter != IntPtr.Zero);
      return childAfter;
    }

    public static IntPtr GetControlHandleFromControlID(IntPtr parentHandle, int controlId) => AutoControl.GetDlgItem(parentHandle, controlId);

    public static List<IntPtr> GetChildHandle(IntPtr parentHandle)
    {
      List<IntPtr> numList = new List<IntPtr>();
      GCHandle gcHandle1 = GCHandle.Alloc((object) numList);
      IntPtr intPtr = GCHandle.ToIntPtr(gcHandle1);
      try
      {
        AutoControl.EnumWindowProc callback = (AutoControl.EnumWindowProc) ((hWnd, lParam) =>
        {
          GCHandle gcHandle2 = GCHandle.FromIntPtr(lParam);
          if (gcHandle2.Target == null)
            return false;
          (gcHandle2.Target as List<IntPtr>).Add(hWnd);
          return true;
        });
        AutoControl.EnumChildWindows(parentHandle, callback, intPtr);
      }
      finally
      {
        gcHandle1.Free();
      }
      return numList;
    }

    public static IntPtr FindHandleWithText(
      List<IntPtr> handles,
      string className,
      string text)
    {
      return handles.Find((Predicate<IntPtr>) (ptr =>
      {
        string className1 = AutoControl.GetClassName(ptr);
        string text1 = AutoControl.GetText(ptr);
        return className1 == className || text1 == text;
      }));
    }

    public static List<IntPtr> FindHandlesWithText(
      List<IntPtr> handles,
      string className,
      string text)
    {
      List<IntPtr> numList = new List<IntPtr>();
      return handles.Where<IntPtr>((Func<IntPtr, bool>) (ptr =>
      {
        string className1 = AutoControl.GetClassName(ptr);
        string text1 = AutoControl.GetText(ptr);
        return className1 == className || text1 == text;
      })).ToList<IntPtr>();
    }

    public static IntPtr FindHandle(IntPtr parentHandle, string className, string text) => AutoControl.FindHandleWithText(AutoControl.GetChildHandle(parentHandle), className, text);

    public static List<IntPtr> FindHandles(
      IntPtr parentHandle,
      string className,
      string text)
    {
      return AutoControl.FindHandlesWithText(AutoControl.GetChildHandle(parentHandle), className, text);
    }

    public static bool CallbackChild(IntPtr hWnd, IntPtr lParam)
    {
      string text = AutoControl.GetText(hWnd);
      string className = AutoControl.GetClassName(hWnd);
      if (!(text == "&Options >>") || !className.StartsWith("ToolbarWindow32"))
        return true;
      AutoControl.SendMessage(hWnd, 0, IntPtr.Zero, IntPtr.Zero);
      return false;
    }

    public static void SendClickOnControlById(IntPtr parentHWND, int controlID)
    {
      IntPtr dlgItem = AutoControl.GetDlgItem(parentHWND, controlID);
      int wParam = 0 | controlID & (int) ushort.MaxValue;
      AutoControl.SendMessage(parentHWND, 273, wParam, dlgItem);
    }

    public static void SendClickOnControlByHandle(IntPtr hWndButton)
    {
      AutoControl.SendMessage(hWndButton, 513, IntPtr.Zero, IntPtr.Zero);
      AutoControl.SendMessage(hWndButton, 514, IntPtr.Zero, IntPtr.Zero);
    }

    public static void SendClickOnPosition(
      IntPtr controlHandle,
      int x,
      int y,
      EMouseKey mouseButton = EMouseKey.LEFT,
      int clickTimes = 1)
    {
      int msg1 = 0;
      int msg2 = 0;
      if (mouseButton == EMouseKey.LEFT)
      {
        msg1 = 513;
        msg2 = 514;
      }
      if (mouseButton == EMouseKey.RIGHT)
      {
        msg1 = 516;
        msg2 = 517;
      }
      IntPtr lParam = AutoControl.MakeLParamFromXY(x, y);
      if (mouseButton == EMouseKey.LEFT || mouseButton == EMouseKey.RIGHT)
      {
        for (int index = 0; index < clickTimes; ++index)
        {
          AutoControl.PostMessage(controlHandle, 6, new IntPtr(1), lParam);
          AutoControl.PostMessage(controlHandle, msg1, new IntPtr(1), lParam);
          AutoControl.PostMessage(controlHandle, msg2, new IntPtr(0), lParam);
        }
      }
      else
      {
        if (mouseButton == EMouseKey.DOUBLE_LEFT)
        {
          msg1 = 515;
          msg2 = 514;
        }
        if (mouseButton == EMouseKey.DOUBLE_RIGHT)
        {
          msg1 = 518;
          msg2 = 517;
        }
        AutoControl.PostMessage(controlHandle, msg1, new IntPtr(1), lParam);
        AutoControl.PostMessage(controlHandle, msg2, new IntPtr(0), lParam);
      }
    }

    public static void SendClickOnPositionRandom(
      IntPtr controlHandle,
      int x,
      int y,
      int ranX,
      int ranY,
      EMouseKey mouseButton = EMouseKey.LEFT,
      int clickTimes = 1)
    {
      int msg1 = 0;
      int msg2 = 0;
      if (mouseButton == EMouseKey.LEFT)
      {
        msg1 = 513;
        msg2 = 514;
      }
      if (mouseButton == EMouseKey.RIGHT)
      {
        msg1 = 516;
        msg2 = 517;
      }
      ranX = AutoControl.rand.Next(ranX);
      ranY = AutoControl.rand.Next(ranY);
      IntPtr lParam = AutoControl.MakeLParamFromXY(x + ranX, y + ranY);
      if (mouseButton == EMouseKey.LEFT || mouseButton == EMouseKey.RIGHT)
      {
        for (int index = 0; index < clickTimes; ++index)
        {
          AutoControl.PostMessage(controlHandle, 6, new IntPtr(1), lParam);
          AutoControl.PostMessage(controlHandle, msg1, new IntPtr(1), lParam);
          AutoControl.PostMessage(controlHandle, msg2, new IntPtr(0), lParam);
        }
      }
      else
      {
        if (mouseButton == EMouseKey.DOUBLE_LEFT)
        {
          msg1 = 515;
          msg2 = 514;
        }
        if (mouseButton == EMouseKey.DOUBLE_RIGHT)
        {
          msg1 = 518;
          msg2 = 517;
        }
        AutoControl.PostMessage(controlHandle, msg1, new IntPtr(1), lParam);
        AutoControl.PostMessage(controlHandle, msg2, new IntPtr(0), lParam);
      }
    }

    public static void SendDragAndDropOnPosition(
      IntPtr controlHandle,
      int x,
      int y,
      int x2,
      int y2,
      int stepx = 10,
      int stepy = 10,
      double delay = 0.05)
    {
      int msg1 = 513;
      int msg2 = 514;
      IntPtr lParam1 = AutoControl.MakeLParamFromXY(x, y);
      IntPtr lParam2 = AutoControl.MakeLParamFromXY(x2, y2);
      if (x2 < x)
        stepx *= -1;
      if (y2 < y)
        stepy *= -1;
      AutoControl.PostMessage(controlHandle, 6, new IntPtr(1), lParam1);
      AutoControl.PostMessage(controlHandle, msg1, new IntPtr(1), lParam1);
      bool flag1 = false;
      bool flag2 = false;
      while (true)
      {
        AutoControl.PostMessage(controlHandle, 512, new IntPtr(1), AutoControl.MakeLParamFromXY(x, y));
        if (stepx > 0)
        {
          if (x < x2)
            x += stepx;
          else
            flag1 = true;
        }
        else if (x > x2)
          x += stepx;
        else
          flag1 = true;
        if (stepy > 0)
        {
          if (y < y2)
            y += stepy;
          else
            flag2 = true;
        }
        else if (y > y2)
          y += stepy;
        else
          flag2 = true;
        if (!(flag1 & flag2))
          Thread.Sleep(TimeSpan.FromSeconds(delay));
        else
          break;
      }
      AutoControl.PostMessage(controlHandle, msg2, new IntPtr(0), lParam2);
    }

    public static void SendDragAndDropOnMultiPosition(
      IntPtr controlHandle,
      Point[] points,
      int stepx = 10,
      int stepy = 10,
      double delay = 0.05)
    {
      if (points == null || points.Length < 2)
        return;
      int msg1 = 513;
      int msg2 = 514;
      IntPtr lParam = AutoControl.MakeLParamFromXY(points[0].X, points[0].Y);
      AutoControl.PostMessage(controlHandle, 6, new IntPtr(1), lParam);
      AutoControl.PostMessage(controlHandle, msg1, new IntPtr(1), lParam);
      for (int index = 0; index < points.Length - 1; ++index)
        AutoControl.MouseMoveDrag(controlHandle, points[index].X, points[index].Y, points[index + 1].X, points[index + 1].Y, stepx, stepy, delay);
      AutoControl.PostMessage(controlHandle, msg2, new IntPtr(0), AutoControl.MakeLParamFromXY(points[points.Length - 1].X, points[points.Length - 1].Y));
    }

    private static void MouseMoveDrag(
      IntPtr controlHandle,
      int x,
      int y,
      int x2,
      int y2,
      int stepx = 10,
      int stepy = 10,
      double delay = 0.05)
    {
      AutoControl.MakeLParamFromXY(x2, y2);
      if (x2 < x)
        stepx *= -1;
      if (y2 < y)
        stepy *= -1;
      bool flag1 = false;
      bool flag2 = false;
      while (true)
      {
        AutoControl.PostMessage(controlHandle, 512, new IntPtr(1), AutoControl.MakeLParamFromXY(x, y));
        if (stepx > 0)
        {
          if (x < x2)
            x += stepx;
          else
            flag1 = true;
        }
        else if (x > x2)
          x += stepx;
        else
          flag1 = true;
        if (stepy > 0)
        {
          if (y < y2)
            y += stepy;
          else
            flag2 = true;
        }
        else if (y > y2)
          y += stepy;
        else
          flag2 = true;
        if (!(flag1 & flag2))
          Thread.Sleep(TimeSpan.FromSeconds(delay));
        else
          break;
      }
    }

    public static void SendClickDownOnPosition(
      IntPtr controlHandle,
      int x,
      int y,
      EMouseKey mouseButton = EMouseKey.LEFT,
      int clickTimes = 1)
    {
      int msg = 0;
      if (mouseButton == EMouseKey.LEFT)
        msg = 513;
      if (mouseButton == EMouseKey.RIGHT)
        msg = 516;
      IntPtr lParam = AutoControl.MakeLParamFromXY(x, y);
      for (int index = 0; index < clickTimes; ++index)
      {
        AutoControl.PostMessage(controlHandle, 6, new IntPtr(1), lParam);
        AutoControl.PostMessage(controlHandle, msg, new IntPtr(1), lParam);
      }
    }

    public static void SendClickUpOnPosition(
      IntPtr controlHandle,
      int x,
      int y,
      EMouseKey mouseButton = EMouseKey.LEFT,
      int clickTimes = 1)
    {
      int Msg = 0;
      if (mouseButton == EMouseKey.LEFT)
        Msg = 514;
      if (mouseButton == EMouseKey.RIGHT)
        Msg = 517;
      IntPtr lParam = AutoControl.MakeLParamFromXY(x, y);
      for (int index = 0; index < clickTimes; ++index)
      {
        AutoControl.PostMessage(controlHandle, 6, new IntPtr(1), lParam);
        AutoControl.SendMessage(controlHandle, Msg, new IntPtr(0), lParam);
      }
    }

    public static void SendText(IntPtr handle, string text) => AutoControl.SendMessage(handle, 12, 0, text);

    public static void SendKeyBoardPress(IntPtr handle, VKeys key)
    {
      AutoControl.PostMessage(handle, 6, new IntPtr(1), new IntPtr(0));
      AutoControl.PostMessage(handle, 256, new IntPtr((int) key), new IntPtr(1));
      AutoControl.PostMessage(handle, 257, new IntPtr((int) key), new IntPtr(0));
    }

    public static void SendKeyBoardPressStepByStep(IntPtr handle, string message, float delay = 0.1f)
    {
      foreach (char ch in message.ToLower())
      {
        VKeys key = VKeys.VK_0;
        switch (ch)
        {
          case '0':
            key = VKeys.VK_0;
            break;
          case '1':
            key = VKeys.VK_1;
            break;
          case '2':
            key = VKeys.VK_2;
            break;
          case '3':
            key = VKeys.VK_3;
            break;
          case '4':
            key = VKeys.VK_4;
            break;
          case '5':
            key = VKeys.VK_5;
            break;
          case '6':
            key = VKeys.VK_6;
            break;
          case '7':
            key = VKeys.VK_7;
            break;
          case '8':
            key = VKeys.VK_8;
            break;
          case '9':
            key = VKeys.VK_9;
            break;
          case 'a':
            key = VKeys.VK_A;
            break;
          case 'b':
            key = VKeys.VK_B;
            break;
          case 'c':
            key = VKeys.VK_V;
            break;
          case 'd':
            key = VKeys.VK_D;
            break;
          case 'e':
            key = VKeys.VK_E;
            break;
          case 'f':
            key = VKeys.VK_F;
            break;
          case 'g':
            key = VKeys.VK_G;
            break;
          case 'h':
            key = VKeys.VK_H;
            break;
          case 'i':
            key = VKeys.VK_I;
            break;
          case 'j':
            key = VKeys.VK_J;
            break;
          case 'k':
            key = VKeys.VK_K;
            break;
          case 'l':
            key = VKeys.VK_L;
            break;
          case 'm':
            key = VKeys.VK_M;
            break;
          case 'n':
            key = VKeys.VK_N;
            break;
          case 'o':
            key = VKeys.VK_O;
            break;
          case 'p':
            key = VKeys.VK_P;
            break;
          case 'q':
            key = VKeys.VK_Q;
            break;
          case 'r':
            key = VKeys.VK_R;
            break;
          case 's':
            key = VKeys.VK_S;
            break;
          case 't':
            key = VKeys.VK_T;
            break;
          case 'u':
            key = VKeys.VK_U;
            break;
          case 'v':
            key = VKeys.VK_V;
            break;
          case 'w':
            key = VKeys.VK_W;
            break;
          case 'x':
            key = VKeys.VK_X;
            break;
          case 'y':
            key = VKeys.VK_Y;
            break;
          case 'z':
            key = VKeys.VK_Z;
            break;
        }
        AutoControl.SendKeyBoardPress(handle, key);
        Thread.Sleep(TimeSpan.FromSeconds((double) delay));
      }
    }

    public static void SendKeyBoardUp(IntPtr handle, VKeys key)
    {
      AutoControl.PostMessage(handle, 6, new IntPtr(1), new IntPtr(0));
      AutoControl.PostMessage(handle, 257, new IntPtr((int) key), new IntPtr(0));
    }

    public static void SendKeyChar(IntPtr handle, VKeys key)
    {
      AutoControl.PostMessage(handle, 6, new IntPtr(1), new IntPtr(0));
      AutoControl.PostMessage(handle, 258, new IntPtr((int) key), new IntPtr(0));
    }

    public static void SendKeyChar(IntPtr handle, int key)
    {
      AutoControl.PostMessage(handle, 6, new IntPtr(1), new IntPtr(0));
      AutoControl.PostMessage(handle, 258, new IntPtr(key), new IntPtr(0));
    }

    public static void SendKeyBoardDown(IntPtr handle, VKeys key)
    {
      AutoControl.PostMessage(handle, 6, new IntPtr(1), new IntPtr(0));
      AutoControl.PostMessage(handle, 256, new IntPtr((int) key), new IntPtr(0));
    }

    public static void SendTextKeyBoard(IntPtr handle, string text, float delay = 0.1f)
    {
      foreach (char ch in text)
        AutoControl.SendKeyChar(handle, (int) ch);
    }

    public static void SendKeyFocus(KeyCode key) => AutoControl.SendKeyPress(key);

    public static void SendMultiKeysFocus(KeyCode[] keys)
    {
      foreach (KeyCode key in keys)
        AutoControl.SendKeyDown(key);
      foreach (KeyCode key in keys)
        AutoControl.SendKeyUp(key);
    }

    public static void SendStringFocus(string message)
    {
      Clipboard.SetText(message);
      AutoControl.SendMultiKeysFocus(new KeyCode[2]
      {
        KeyCode.CONTROL,
        KeyCode.KEY_V
      });
    }

    public static void SendKeyPress(KeyCode keyCode)
    {
      INPUT input1 = new INPUT() { Type = 1 };
      input1.Data.Keyboard = new KEYBDINPUT()
      {
        Vk = (ushort) keyCode,
        Scan = (ushort) 0,
        Flags = 0U,
        Time = 0U,
        ExtraInfo = IntPtr.Zero
      };
      INPUT input2 = new INPUT() { Type = 1 };
      input2.Data.Keyboard = new KEYBDINPUT()
      {
        Vk = (ushort) keyCode,
        Scan = (ushort) 0,
        Flags = 2U,
        Time = 0U,
        ExtraInfo = IntPtr.Zero
      };
      if (AutoControl.SendInput(2U, new INPUT[2]
      {
        input1,
        input2
      }, Marshal.SizeOf(typeof (INPUT))) == 0U)
        throw new Exception();
    }

    public static void SendKeyPressStepByStep(string message, double delay = 0.5)
    {
      foreach (char ch in message)
      {
        switch (ch)
        {
          case '0':
            AutoControl.SendKeyPress(KeyCode.KEY_0);
            break;
          case '1':
            AutoControl.SendKeyPress(KeyCode.KEY_1);
            break;
          case '2':
            AutoControl.SendKeyPress(KeyCode.KEY_2);
            break;
          case '3':
            AutoControl.SendKeyPress(KeyCode.KEY_3);
            break;
          case '4':
            AutoControl.SendKeyPress(KeyCode.KEY_4);
            break;
          case '5':
            AutoControl.SendKeyPress(KeyCode.KEY_5);
            break;
          case '6':
            AutoControl.SendKeyPress(KeyCode.KEY_6);
            break;
          case '7':
            AutoControl.SendKeyPress(KeyCode.KEY_7);
            break;
          case '8':
            AutoControl.SendKeyPress(KeyCode.KEY_8);
            break;
          case '9':
            AutoControl.SendKeyPress(KeyCode.KEY_9);
            break;
        }
        Thread.Sleep(TimeSpan.FromSeconds(delay));
      }
    }

    public static void SendKeyDown(KeyCode keyCode)
    {
      INPUT input = new INPUT() { Type = 1 };
      input.Data.Keyboard = new KEYBDINPUT();
      input.Data.Keyboard.Vk = (ushort) keyCode;
      input.Data.Keyboard.Scan = (ushort) 0;
      input.Data.Keyboard.Flags = 0U;
      input.Data.Keyboard.Time = 0U;
      input.Data.Keyboard.ExtraInfo = IntPtr.Zero;
      if (AutoControl.SendInput(1U, new INPUT[1]
      {
        input
      }, Marshal.SizeOf(typeof (INPUT))) == 0U)
        throw new Exception();
    }

    public static void SendKeyUp(KeyCode keyCode)
    {
      INPUT input = new INPUT() { Type = 1 };
      input.Data.Keyboard = new KEYBDINPUT();
      input.Data.Keyboard.Vk = (ushort) keyCode;
      input.Data.Keyboard.Scan = (ushort) 0;
      input.Data.Keyboard.Flags = 2U;
      input.Data.Keyboard.Time = 0U;
      input.Data.Keyboard.ExtraInfo = IntPtr.Zero;
      if (AutoControl.SendInput(1U, new INPUT[1]
      {
        input
      }, Marshal.SizeOf(typeof (INPUT))) == 0U)
        throw new Exception();
    }

    public static void MouseClick(int x, int y, EMouseKey mouseKey = EMouseKey.LEFT)
    {
      Cursor.Position = new Point(x, y);
      AutoControl.Click(mouseKey);
    }

    public static void MouseDragX(Point startPoint, int deltaX, bool isNegative = false)
    {
      Cursor.Position = startPoint;
      AutoControl.mouse_event(2U, 0, 0, 0, UIntPtr.Zero);
      for (int index = 0; index < deltaX; ++index)
      {
        if (!isNegative)
          AutoControl.mouse_event(1U, 1, 0, 0, UIntPtr.Zero);
        else
          AutoControl.mouse_event(1U, -1, 0, 0, UIntPtr.Zero);
      }
      AutoControl.mouse_event(32772U, 0, 0, 0, UIntPtr.Zero);
    }

    public static void MouseDragY(Point startPoint, int deltaY, bool isNegative = false)
    {
      Cursor.Position = startPoint;
      AutoControl.mouse_event(2U, 0, 0, 0, UIntPtr.Zero);
      for (int index = 0; index < deltaY; ++index)
      {
        if (!isNegative)
          AutoControl.mouse_event(1U, 0, 1, 0, UIntPtr.Zero);
        else
          AutoControl.mouse_event(1U, 0, -1, 0, UIntPtr.Zero);
      }
      AutoControl.mouse_event(32772U, 0, 0, 0, UIntPtr.Zero);
    }

    public static void MouseScroll(Point startPoint, int deltaY, bool isNegative = false)
    {
      Cursor.Position = startPoint;
      AutoControl.mouse_event(2048U, 0, 0, deltaY, UIntPtr.Zero);
    }

    public static void MouseClick(Point point, EMouseKey mouseKey = EMouseKey.LEFT)
    {
      Cursor.Position = point;
      AutoControl.Click(mouseKey);
    }

    public static void Click(EMouseKey mouseKey = EMouseKey.LEFT)
    {
      switch (mouseKey)
      {
        case EMouseKey.LEFT:
          AutoControl.mouse_event(32774U, 0, 0, 0, UIntPtr.Zero);
          break;
        case EMouseKey.RIGHT:
          AutoControl.mouse_event(32792U, 0, 0, 0, UIntPtr.Zero);
          break;
        case EMouseKey.DOUBLE_LEFT:
          AutoControl.mouse_event(32774U, 0, 0, 0, UIntPtr.Zero);
          AutoControl.mouse_event(32774U, 0, 0, 0, UIntPtr.Zero);
          break;
        case EMouseKey.DOUBLE_RIGHT:
          AutoControl.mouse_event(32792U, 0, 0, 0, UIntPtr.Zero);
          AutoControl.mouse_event(32792U, 0, 0, 0, UIntPtr.Zero);
          break;
      }
    }

    public static RECT GetWindowRect(IntPtr hWnd)
    {
      RECT lpRect = new RECT();
      AutoControl.GetWindowRect(hWnd, ref lpRect);
      return lpRect;
    }

    public static Point GetGlobalPoint(IntPtr hWnd, Point? point = null)
    {
      Point point1 = new Point();
      RECT windowRect = AutoControl.GetWindowRect(hWnd);
      if (!point.HasValue)
        point = new Point?(new Point());
      point1.X = point.Value.X + windowRect.Left;
      point1.Y = point.Value.Y + windowRect.Top;
      return point1;
    }

    public static Point GetGlobalPoint(IntPtr hWnd, int x = 0, int y = 0)
    {
      Point point = new Point();
      RECT windowRect = AutoControl.GetWindowRect(hWnd);
      point.X = x + windowRect.Left;
      point.Y = y + windowRect.Top;
      return point;
    }

    public static string GetText(IntPtr hWnd)
    {
      StringBuilder s = new StringBuilder(256);
      AutoControl.GetWindowText(hWnd, s, 256);
      return s.ToString().Trim();
    }

    public static string GetClassName(IntPtr hWnd)
    {
      StringBuilder lpClassName = new StringBuilder(256);
      AutoControl.GetClassName(hWnd, lpClassName, 256);
      return lpClassName.ToString().Trim();
    }

    public static IntPtr MakeLParam(int LoWord, int HiWord) => (IntPtr) (HiWord << 16 | LoWord & (int) ushort.MaxValue);

    public static IntPtr MakeLParamFromXY(int x, int y) => (IntPtr) (y << 16 | x);

    public delegate bool CallBack(IntPtr hwnd, IntPtr lParam);

    private delegate bool EnumWindowProc(IntPtr hwnd, IntPtr lParam);

    [System.Flags]
    public enum MouseEventFlags : uint
    {
      LEFTDOWN = 2,
      LEFTUP = 4,
      MIDDLEDOWN = 32, // 0x00000020
      MIDDLEUP = 64, // 0x00000040
      MOVE = 1,
      ABSOLUTE = 32768, // 0x00008000
      RIGHTDOWN = 8,
      RIGHTUP = 16, // 0x00000010
      WHEEL = 2048, // 0x00000800
      XDOWN = 128, // 0x00000080
      XUP = 256, // 0x00000100
      XBUTTON1 = MOVE, // 0x00000001
      XBUTTON2 = LEFTDOWN, // 0x00000002
    }
  }
}
