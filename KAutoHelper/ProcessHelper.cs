// Decompiled with JetBrains decompiler
// Type: KAutoHelper.ProcessHelper
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace KAutoHelper
{
    public class ProcessHelper
    {
        public static List<string> windowTitles = new List<string>();

        [DllImport("user32")]
        private static extern bool EnumWindows(ProcessHelper.EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(
          IntPtr hWndStart,
          ProcessHelper.EnumWindowsProc callback,
          IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SendMessageTimeout(
          IntPtr hWnd,
          uint Msg,
          IntPtr wParam,
          IntPtr lParam,
          uint fuFlags,
          uint uTimeout,
          out IntPtr lpdwResult);

        public static List<string> GetWindowTitles(bool includeChildren)
        {
            ProcessHelper.EnumWindows(new ProcessHelper.EnumWindowsProc(ProcessHelper.EnumWindowsCallback), includeChildren ? (IntPtr)1 : IntPtr.Zero);
            return ProcessHelper.windowTitles;
        }

        public static bool EnumWindowsCallback(IntPtr testWindowHandle, IntPtr includeChildren)
        {
            string windowTitle = ProcessHelper.GetWindowTitle(testWindowHandle);
            if (ProcessHelper.TitleMatches(windowTitle))
                ProcessHelper.windowTitles.Add(windowTitle);
            if (!includeChildren.Equals((object)IntPtr.Zero))
                ProcessHelper.EnumChildWindows(testWindowHandle, new ProcessHelper.EnumWindowsProc(ProcessHelper.EnumWindowsCallback), IntPtr.Zero);
            return true;
        }

        public static bool TitleMatches(string title) => title.Contains("e");

        public static string GetWindowTitle(IntPtr windowHandle)
        {
            uint fuFlags = 2;
            uint Msg = 13;
            int cb = 32768;
            string empty = string.Empty;
            IntPtr num = Marshal.AllocCoTaskMem(cb);
            Marshal.Copy(empty.ToCharArray(), 0, num, empty.Length);
            ProcessHelper.SendMessageTimeout(windowHandle, Msg, (IntPtr)cb, num, fuFlags, 1000U, out IntPtr _);
            string stringAuto = Marshal.PtrToStringAuto(num);
            Marshal.FreeCoTaskMem(num);
            return stringAuto;
        }

        public static List<Port> GetNetStatPorts()
        {
            List<Port> portList = new List<Port>();
            try
            {
                using (Process process = new Process())
                {
                    process.StartInfo = new ProcessStartInfo()
                    {
                        Arguments = "-a -n -o",
                        FileName = "netstat.exe",
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    process.Start();
                    StreamReader standardOutput = process.StandardOutput;
                    StreamReader standardError = process.StandardError;
                    string input1 = standardOutput.ReadToEnd() + standardError.ReadToEnd();
                    if (!(process.ExitCode.ToString() != "0"))
                        ;
                    foreach (string input2 in Regex.Split(input1, "\r\n"))
                    {
                        string[] strArray = Regex.Split(input2, "\\s+");
                        if (strArray.Length > 4 && (strArray[1].Equals("UDP") || strArray[1].Equals("TCP")))
                        {
                            string str = Regex.Replace(strArray[2], "\\[(.*?)\\]", "1.1.1.1");
                            portList.Add(new Port()
                            {
                                protocol = str.Contains("1.1.1.1") ? string.Format("{0}v6", (object)strArray[1]) : string.Format("{0}v4", (object)strArray[1]),
                                port_number = str.Split(':')[1],
                                process_name = strArray[1] == "UDP" ? ProcessHelper.LookupProcess((int)Convert.ToInt16(strArray[4])) : ProcessHelper.LookupProcess((int)Convert.ToInt16(strArray[5])),
                                pid = (int)Convert.ToInt16(strArray[5])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return portList;
        }

        public static string LookupProcess(int pid)
        {
            string str;
            try
            {
                str = Process.GetProcessById(pid).ProcessName;
            }
            catch (Exception ex)
            {
                str = "-";
            }
            return str;
        }

        private delegate bool EnumWindowsProc(IntPtr windowHandle, IntPtr lParam);
    }
}
