// Decompiled with JetBrains decompiler
// Type: KAutoHelper.ADBHelper
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace KAutoHelper
{
    public class ADBHelper
    {
        private static string LIST_DEVICES = "adb devices";
        private static string TAP_DEVICES = "adb -s {0} shell input tap {1} {2}";
        private static string SWIPE_DEVICES = "adb -s {0} shell input swipe {1} {2} {3} {4} {5}";
        private static string KEY_DEVICES = "adb -s {0} shell input keyevent {1}";
        private static string INPUT_TEXT_DEVICES = "adb -s {0} shell input text \"{1}\"";
        private static string CAPTURE_SCREEN_TO_DEVICES = "adb -s {0} shell screencap -p \"{1}\"";
        private static string PULL_SCREEN_FROM_DEVICES = "adb -s {0} pull \"{1}\"";
        private static string REMOVE_SCREEN_FROM_DEVICES = "adb -s {0} shell rm -f \"{1}\"";
        private static string GET_SCREEN_RESOLUTION = "adb -s {0} shell dumpsys display | Find \"mCurrentDisplayRect\"";
        private const int DEFAULT_SWIPE_DURATION = 100;
        private static string ADB_FOLDER_PATH = "";
        private static string ADB_PATH = "";

        public static string SetADBFolderPath(string folderPath)
        {
            ADBHelper.ADB_FOLDER_PATH = folderPath;
            ADBHelper.ADB_PATH = folderPath + "\\adb.exe";
            return !File.Exists(ADBHelper.ADB_PATH) ? "ADB Path not Exits!!!" : "OK";
        }

        public void SetTextFromClipboard(string deviceID, string text)
        {
            string[] strArray = text.Split(new string[1] { "\r\n" }, StringSplitOptions.None);
            int num = 0;
            foreach (string str in strArray)
            {
                ADBHelper.ExecuteCMDBat(deviceID, "adb -s " + deviceID + " shell am broadcast -a clipper.set -e text \"\\\"" + str + "\\\"\"");
                ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell input keyevent 279");
                ++num;
                if (num < strArray.Length)
                    ADBHelper.Key(deviceID, ADBKeyEvent.KEYCODE_ENTER);
            }
        }

        private void Note(string deviceID)
        {
            ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell am force-stop com.zing.zalo");
            string str1 = ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell rm -f /sdcard/Pictures/Images/*");
            str1 = ADBHelper.ExecuteCMD("adb -s " + deviceID + " shell mkdir /sdcard/Pictures/Images");
            foreach (string str2 in ((IEnumerable<FileInfo>)new DirectoryInfo("C:\\images").GetFiles()).Select<FileInfo, string>((Func<FileInfo, string>)(x => x.FullName)))
                ADBHelper.ExecuteCMD("adb -s " + deviceID + " push " + str2 + " sdcard/Pictures/Images");
        }

        public static string ExecuteCMD(string cmdCommand)
        {
            try
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    WorkingDirectory = ADBHelper.ADB_FOLDER_PATH,
                    FileName = "cmd.exe",
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    Verb = "runas"
                };
                process.Start();
                process.StandardInput.WriteLine(cmdCommand);
                process.StandardInput.Flush();
                process.StandardInput.Close();
                process.WaitForExit(3000);
                return process.StandardOutput.ReadToEnd();
            }
            catch
            {
                return (string)null;
            }
        }

        public static string ExecuteCMDBat(string deviceID, string cmdCommand)
        {
            try
            {
                string path = "bat_" + deviceID + ".bat";
                File.WriteAllText(path, cmdCommand);
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo()
                {
                    WorkingDirectory = ADBHelper.ADB_FOLDER_PATH,
                    FileName = path,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    Verb = "runas"
                };
                process.Start();
                process.StandardInput.WriteLine(cmdCommand);
                process.StandardInput.Flush();
                process.StandardInput.Close();
                process.WaitForExit();
                return process.StandardOutput.ReadToEnd();
            }
            catch
            {
                return (string)null;
            }
        }

        public static List<string> GetDevices()
        {
            List<string> stringList = new List<string>();
            MatchCollection matchCollection = Regex.Matches(ADBHelper.ExecuteCMD("adb devices"), "(?<=List of devices attached)([^\\n]*\\n+)+", RegexOptions.Singleline);
            if (matchCollection.Count > 0)
            {
                foreach (string str1 in Regex.Split(matchCollection[0].Groups[0].Value, "\r\n"))
                {
                    if (!string.IsNullOrEmpty(str1) && str1 != " ")
                    {
                        string[] strArray = str1.Trim().Split('\t');
                        string str2 = strArray[0];
                        try
                        {
                            if (strArray[1] != "device")
                                continue;
                        }
                        catch
                        {
                        }
                        stringList.Add(str2.Trim());
                    }
                }
            }
            return stringList;
        }

        public static string GetDeviceName(string deviceID)
        {
            string str = "";
            ADBHelper.ExecuteCMD("");
            return str;
        }

        public static void TapByPercent(string deviceID, double x, double y, int count = 1)
        {
            Point screenResolution = ADBHelper.GetScreenResolution(deviceID);
            int num1 = (int)(x * ((double)screenResolution.X * 1.0 / 100.0));
            int num2 = (int)(y * ((double)screenResolution.Y * 1.0 / 100.0));
            string cmdCommand = string.Format(ADBHelper.TAP_DEVICES, (object)deviceID, (object)num1, (object)num2);
            for (int index = 1; index < count; ++index)
                cmdCommand = cmdCommand + " && " + string.Format(ADBHelper.TAP_DEVICES, (object)deviceID, (object)x, (object)y);
            ADBHelper.ExecuteCMD(cmdCommand);
        }

        public static void Tap(string deviceID, int x, int y, int count = 1)
        {
            string cmdCommand = string.Format(ADBHelper.TAP_DEVICES, (object)deviceID, (object)x, (object)y);
            for (int index = 1; index < count; ++index)
                cmdCommand = cmdCommand + " && " + string.Format(ADBHelper.TAP_DEVICES, (object)deviceID, (object)x, (object)y);
            ADBHelper.ExecuteCMD(cmdCommand);
        }

        public static void Key(string deviceID, ADBKeyEvent key) => ADBHelper.ExecuteCMD(string.Format(ADBHelper.KEY_DEVICES, (object)deviceID, (object)key));

        public static void InputText(string deviceID, string text) => ADBHelper.ExecuteCMD(string.Format(ADBHelper.INPUT_TEXT_DEVICES, (object)deviceID, (object)text.Replace(" ", "%s").Replace("&", "\\&").Replace("<", "\\<").Replace(">", "\\>").Replace("?", "\\?").Replace(":", "\\:").Replace("{", "\\{").Replace("}", "\\}").Replace("[", "\\[").Replace("]", "\\]").Replace("|", "\\|")));

        public static void SwipeByPercent(
          string deviceID,
          double x1,
          double y1,
          double x2,
          double y2,
          int duration = 100)
        {
            Point screenResolution = ADBHelper.GetScreenResolution(deviceID);
            int num1 = (int)(x1 * ((double)screenResolution.X * 1.0 / 100.0));
            int num2 = (int)(y1 * ((double)screenResolution.Y * 1.0 / 100.0));
            int num3 = (int)(x2 * ((double)screenResolution.X * 1.0 / 100.0));
            int num4 = (int)(y2 * ((double)screenResolution.Y * 1.0 / 100.0));
            ADBHelper.ExecuteCMD(string.Format(ADBHelper.SWIPE_DEVICES, (object)deviceID, (object)num1, (object)num2, (object)num3, (object)num4, (object)duration));
        }

        public static void Swipe(string deviceID, int x1, int y1, int x2, int y2, int duration = 100) => ADBHelper.ExecuteCMD(string.Format(ADBHelper.SWIPE_DEVICES, (object)deviceID, (object)x1, (object)y1, (object)x2, (object)y2, (object)duration));

        public static void LongPress(string deviceID, int x, int y, int duration = 100) => ADBHelper.ExecuteCMD(string.Format(ADBHelper.SWIPE_DEVICES, (object)deviceID, (object)x, (object)y, (object)x, (object)y, (object)duration));

        public static Point GetScreenResolution(string deviceID)
        {
            string str1 = ADBHelper.ExecuteCMD(string.Format(ADBHelper.GET_SCREEN_RESOLUTION, (object)deviceID));
            string str2 = str1.Substring(str1.IndexOf("- "));
            string[] strArray = str2.Substring(str2.IndexOf(' '), str2.IndexOf(')') - str2.IndexOf(' ')).Split(',');
            return new Point(Convert.ToInt32(strArray[0].Trim()), Convert.ToInt32(strArray[1].Trim()));
        }

        public static Bitmap ScreenShoot(
          string deviceID = null,
          bool isDeleteImageAfterCapture = true,
          string fileName = "screenShoot.png")
        {
            if (string.IsNullOrEmpty(deviceID))
            {
                List<string> devices = ADBHelper.GetDevices();
                if (devices == null || devices.Count <= 0)
                    return (Bitmap)null;
                deviceID = devices.First<string>();
            }
            string str1 = deviceID;
            try
            {
                str1 = deviceID.Split(':')[1];
            }
            catch
            {
            }
            string path = Path.GetFileNameWithoutExtension(fileName) + str1 + Path.GetExtension(fileName);
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch (Exception ex)
                {
                }
            }
            string filename = Directory.GetCurrentDirectory() + "\\" + path;
            string str2 = "\"" + Directory.GetCurrentDirectory().Replace("\\\\", "\\") + "\"";
            string cmdCommand1 = string.Format("adb -s {0} shell screencap -p \"{1}\"", (object)deviceID, (object)("/sdcard/" + path));
            string cmdCommand2 = string.Format("adb -s " + deviceID + " pull /sdcard/" + path + " " + str2);
            ADBHelper.ExecuteCMD(cmdCommand1);
            ADBHelper.ExecuteCMD(cmdCommand2);
            Bitmap bitmap1 = (Bitmap)null;
            try
            {
                using (Bitmap bitmap2 = new Bitmap(filename))
                    bitmap1 = new Bitmap((Image)bitmap2);
            }
            catch
            {
            }
            if (isDeleteImageAfterCapture)
            {
                try
                {
                    File.Delete(path);
                }
                catch
                {
                }
                try
                {
                    ADBHelper.ExecuteCMD(string.Format("adb -s " + deviceID + " shell \"rm /sdcard/" + path + "\""));
                }
                catch
                {
                }
            }
            return bitmap1;
        }

        public static void ConnectNox(int count = 1)
        {
            string str = "";
            int num = 62000;
            string cmdCommand;
            if (count <= 1)
            {
                cmdCommand = str + "adb connect 127.0.0.1:" + (num + 1).ToString();
            }
            else
            {
                cmdCommand = str + "adb connect 127.0.0.1:" + (num + 1).ToString();
                for (int index = 25; index < count + 24; ++index)
                    cmdCommand = cmdCommand + Environment.NewLine + "adb connect 127.0.0.1:" + (num + index).ToString();
            }
            ADBHelper.ExecuteCMD(cmdCommand);
        }

        public static void PlanModeON(string deviceID, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            ADBHelper.ExecuteCMD("adb -s " + deviceID + " settings put global airplane_mode_on 1" + Environment.NewLine + "adb -s " + deviceID + " am broadcast -a android.intent.action.AIRPLANE_MODE");
        }

        public static void PlanModeOFF(string deviceID, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;
            ADBHelper.ExecuteCMD("adb -s " + deviceID + " settings put global airplane_mode_on 0" + Environment.NewLine + "adb -s " + deviceID + " am broadcast -a android.intent.action.AIRPLANE_MODE");
        }

        public static void Delay(double delayTime)
        {
            for (double num = 0.0; num < delayTime; num += 100.0)
                Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
        }

        public static Point? FindImage(
          string deviceID,
          string ImagePath,
          int delayPerCheck = 2000,
          int count = 5)
        {
            FileInfo[] files = new DirectoryInfo(ImagePath).GetFiles();
            do
            {
                Bitmap mainBitmap = (Bitmap)null;
                int num = 3;
                do
                {
                    try
                    {
                        mainBitmap = ADBHelper.ScreenShoot(deviceID);
                        break;
                    }
                    catch (Exception ex)
                    {
                        --num;
                        ADBHelper.Delay(1000.0);
                    }
                }
                while (num > 0);
                if (mainBitmap == null)
                    return new Point?();
                Point? nullable = new Point?();
                foreach (FileSystemInfo fileSystemInfo in files)
                {
                    Bitmap subBitmap = (Bitmap)Image.FromFile(fileSystemInfo.FullName);
                    nullable = ImageScanOpenCV.FindOutPoint(mainBitmap, subBitmap);
                    if (nullable.HasValue)
                        break;
                }
                if (nullable.HasValue)
                    return nullable;
                ADBHelper.Delay(2000.0);
                --count;
            }
            while (count > 0);
            return new Point?();
        }

        public static bool FindImageAndClick(
          string deviceID,
          string ImagePath,
          int delayPerCheck = 2000,
          int count = 5)
        {
            FileInfo[] files = new DirectoryInfo(ImagePath).GetFiles();
            do
            {
                Bitmap mainBitmap = (Bitmap)null;
                int num = 3;
                do
                {
                    try
                    {
                        mainBitmap = ADBHelper.ScreenShoot(deviceID);
                        break;
                    }
                    catch (Exception ex)
                    {
                        --num;
                        ADBHelper.Delay(1000.0);
                    }
                }
                while (num > 0);
                if (mainBitmap == null)
                    return false;
                Point? nullable = new Point?();
                foreach (FileSystemInfo fileSystemInfo in files)
                {
                    Bitmap subBitmap = (Bitmap)Image.FromFile(fileSystemInfo.FullName);
                    nullable = ImageScanOpenCV.FindOutPoint(mainBitmap, subBitmap);
                    if (nullable.HasValue)
                        break;
                }
                if (nullable.HasValue)
                {
                    ADBHelper.Tap(deviceID, nullable.Value.X, nullable.Value.Y);
                    return true;
                }
                ADBHelper.Delay((double)delayPerCheck);
                --count;
            }
            while (count > 0);
            return false;
        }
    }
}
