// Decompiled with JetBrains decompiler
// Type: KAutoHelper.LDPlayer
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace KAutoHelper
{
  public class LDPlayer
  {
    public static string pathLD = "C:\\LDPlayer\\LDPlayer4.0\\ldconsole.exe";

    public void Open(string param, string NameOrId) => this.ExecuteLD(string.Format("launch --{0} {1}", (object) param, (object) NameOrId));

    public void Open_App(string param, string NameOrId, string Package_Name) => this.ExecuteLD(string.Format("launchex --{0} {1} --packagename {2}", (object) param, (object) NameOrId, (object) Package_Name));

    public void Close(string param, string NameOrId) => this.ExecuteLD(string.Format("quit --{0} {1}", (object) param, (object) NameOrId));

    public void CloseAll() => this.ExecuteLD("quitall");

    public void ReBoot(string param, string NameOrId) => this.ExecuteLD(string.Format("reboot --{0} {1}", (object) param, (object) NameOrId));

    public void Create(string Name) => this.ExecuteLD("add --name " + Name);

    public void Copy(string Name, string From_NameOrId) => this.ExecuteLD(string.Format("copy --name {0} --from {1}", (object) Name, (object) From_NameOrId));

    public void Delete(string param, string NameOrId) => this.ExecuteLD(string.Format("remove --{0} {1}", (object) param, (object) NameOrId));

    public void ReName(string param, string NameOrId, string title_new) => this.ExecuteLD(string.Format("rename --{0} {1} --title {2}", (object) param, (object) NameOrId, (object) title_new));

    public void InstallApp_File(string param, string NameOrId, string File_Name) => this.ExecuteLD(string.Format("installapp --{0} {1} --filename \"{2}\"", (object) param, (object) NameOrId, (object) File_Name));

    public void InstallApp_Package(string param, string NameOrId, string Package_Name) => this.ExecuteLD(string.Format("installapp --{0} {1} --packagename {2}", (object) param, (object) NameOrId, (object) Package_Name));

    public void UnInstallApp(string param, string NameOrId, string Package_Name) => this.ExecuteLD(string.Format("uninstallapp --{0} {1} --packagename {2}", (object) param, (object) NameOrId, (object) Package_Name));

    public void RunApp(string param, string NameOrId, string Package_Name) => this.ExecuteLD(string.Format("runapp --{0} {1} --packagename {2}", (object) param, (object) NameOrId, (object) Package_Name));

    public void KillApp(string param, string NameOrId, string Package_Name) => this.ExecuteLD(string.Format("killapp --{0} {1} --packagename {2}", (object) param, (object) NameOrId, (object) Package_Name));

    public void Locate(string param, string NameOrId, string Lng, string Lat) => this.ExecuteLD(string.Format("locate --{0} {1} --LLI {2},{3}", (object) param, (object) NameOrId, (object) Lng, (object) Lat));

    public void Change_Property(string param, string NameOrId, string cmd) => this.ExecuteLD(string.Format("modify --{0} {1} {2}", (object) param, (object) NameOrId, (object) cmd));

    public void SetProp(string param, string NameOrId, string key, string value) => this.ExecuteLD(string.Format("setprop --{0} {1} --key {2} --value {3}", (object) param, (object) NameOrId, (object) key, (object) value));

    public string GetProp(string param, string NameOrId, string key) => this.ExecuteLD_Result(string.Format("getprop --{0} {1} --key {2}", (object) param, (object) NameOrId, (object) key));

    public string ADB(string param, string NameOrId, string cmd) => this.ExecuteLD_Result(string.Format("adb --{0} {1} --command {2}", (object) param, (object) NameOrId, (object) cmd));

    public void DownCPU(string param, string NameOrId, string rate) => this.ExecuteLD(string.Format("downcpu --{0} {1} --rate {2}", (object) param, (object) NameOrId, (object) rate));

    public void Backup(string param, string NameOrId, string file_path) => this.ExecuteLD(string.Format("backup --{0} {1} --file \"{2}\"", (object) param, (object) NameOrId, (object) file_path));

    public void Restore(string param, string NameOrId, string file_path) => this.ExecuteLD(string.Format("restore --{0} {1} --file \"{2}\"", (object) param, (object) NameOrId, (object) file_path));

    public void Action(string param, string NameOrId, string key, string value) => this.ExecuteLD(string.Format("action --{0} {1} --key {2} --value {3}", (object) param, (object) NameOrId, (object) key, (object) value));

    public void Scan(string param, string NameOrId, string file_path) => this.ExecuteLD(string.Format("scan --{0} {1} --file {2}", (object) param, (object) NameOrId, (object) file_path));

    public void SortWnd() => this.ExecuteLD("sortWnd");

    public void zoomIn(string param, string NameOrId) => this.ExecuteLD(string.Format("zoomIn --{0} {1}", (object) param, (object) NameOrId));

    public void zoomOut(string param, string NameOrId) => this.ExecuteLD(string.Format("zoomOut --{0} {1}", (object) param, (object) NameOrId));

    public void Pull(
      string param,
      string NameOrId,
      string remote_file_path,
      string local_file_path)
    {
      this.ExecuteLD(string.Format("pull --{0} {1} --remote \"{2}\" --local \"{3}\"", (object) param, (object) NameOrId, (object) remote_file_path, (object) local_file_path));
    }

    public void Push(
      string param,
      string NameOrId,
      string remote_file_path,
      string local_file_path)
    {
      this.ExecuteLD(string.Format("push --{0} {1} --remote \"{2}\" --local \"{3}\"", (object) param, (object) NameOrId, (object) remote_file_path, (object) local_file_path));
    }

    public void BackupApp(string param, string NameOrId, string Package_Name, string file_path) => this.ExecuteLD(string.Format("backupapp --{0} {1} --packagename {2} --file \"{3}\"", (object) param, (object) NameOrId, (object) Package_Name, (object) file_path));

    public void RestoreApp(string param, string NameOrId, string Package_Name, string file_path) => this.ExecuteLD(string.Format("restoreapp --{0} {1} --packagename {2} --file \"{3}\"", (object) param, (object) NameOrId, (object) Package_Name, (object) file_path));

    public void Golabal_Config(
      string param,
      string NameOrId,
      string fps,
      string audio,
      string fast_play,
      string clean_mode)
    {
      this.ExecuteLD(string.Format("globalsetting --{0} {1} --audio {2} --fastplay {3} --cleanmode {4}", (object) param, (object) NameOrId, (object) audio, (object) fast_play, (object) clean_mode));
    }

    public List<string> GetDevices()
    {
      string[] strArray = this.ExecuteLD_Result("list").Trim().Split('\n');
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index] == "")
          return new List<string>();
        strArray[index] = strArray[index].Trim();
      }
      return ((IEnumerable<string>) strArray).ToList<string>();
    }

    public List<string> GetDevices_Running()
    {
      string[] strArray = this.ExecuteLD_Result("runninglist").Trim().Split('\n');
      for (int index = 0; index < strArray.Length; ++index)
      {
        if (strArray[index] == "")
          return new List<string>();
        strArray[index] = strArray[index].Trim();
      }
      return ((IEnumerable<string>) strArray).ToList<string>();
    }

    public bool IsDevice_Running(string param, string NameOrId) => this.ExecuteLD_Result(string.Format("isrunning --{0} {1}", (object) param, (object) NameOrId)).Trim() == "running";

    public List<Info_Devices> GetDevices2()
    {
      try
      {
        List<Info_Devices> infoDevicesList = new List<Info_Devices>();
        string str1 = this.ExecuteLD_Result("list2").Trim();
        char[] chArray = new char[1]{ '\n' };
        foreach (string str2 in str1.Split(chArray))
        {
          Info_Devices infoDevices = new Info_Devices();
          string[] strArray = str2.Trim().Split(',');
          infoDevices.index = int.Parse(strArray[0]);
          infoDevices.name = strArray[1];
          infoDevices.adb_id = "-1";
          infoDevicesList.Add(infoDevices);
        }
        return infoDevicesList;
      }
      catch
      {
        return new List<Info_Devices>();
      }
    }

    public List<Info_Devices> GetDevices2_Running()
    {
      try
      {
        int index = 0;
        List<string> devices = ADBHelper.GetDevices();
        List<Info_Devices> infoDevicesList = new List<Info_Devices>();
        List<string> devicesRunning = this.GetDevices_Running();
        string str1 = this.ExecuteLD_Result("list2").Trim();
        char[] chArray = new char[1]{ '\n' };
        foreach (string str2 in str1.Split(chArray))
        {
          Info_Devices infoDevices = new Info_Devices();
          string[] strArray = str2.Trim().Split(',');
          infoDevices.index = int.Parse(strArray[0]);
          infoDevices.name = strArray[1];
          if (devicesRunning.Contains(infoDevices.name))
          {
            infoDevices.adb_id = devices[index];
            infoDevicesList.Add(infoDevices);
            ++index;
          }
        }
        return infoDevicesList;
      }
      catch
      {
        return new List<Info_Devices>();
      }
    }

    public void ExecuteLD(string cmd)
    {
      Process process = new Process();
      process.StartInfo.FileName = LDPlayer.pathLD;
      process.StartInfo.Arguments = cmd;
      process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
      process.StartInfo.UseShellExecute = false;
      process.StartInfo.CreateNoWindow = true;
      process.EnableRaisingEvents = true;
      process.Start();
      process.WaitForExit();
      process.Close();
    }

    public string ExecuteLD_Result(string cmdCommand)
    {
      string str;
      try
      {
        Process process = new Process();
        process.StartInfo = new ProcessStartInfo()
        {
          FileName = LDPlayer.pathLD,
          Arguments = cmdCommand,
          CreateNoWindow = true,
          UseShellExecute = false,
          WindowStyle = ProcessWindowStyle.Hidden,
          RedirectStandardInput = true,
          RedirectStandardOutput = true
        };
        process.Start();
        process.WaitForExit();
        str = process.StandardOutput.ReadToEnd();
      }
      catch
      {
        str = (string) null;
      }
      return str;
    }

    public void Back(string deviceID) => ADBHelper.Key(deviceID, ADBKeyEvent.KEYCODE_BACK);

    public void Home(string deviceID) => ADBHelper.Key(deviceID, ADBKeyEvent.KEYCODE_HOME);

    public void Menu(string deviceID) => ADBHelper.Key(deviceID, ADBKeyEvent.KEYCODE_APP_SWITCH);

    public void Tap_Img(string deviceID, Bitmap ImgFind)
    {
      Bitmap subBitmap = (Bitmap) ImgFind.Clone();
      Point? outPoint = ImageScanOpenCV.FindOutPoint(ADBHelper.ScreenShoot(deviceID), subBitmap);
      if (!outPoint.HasValue)
        return;
      string deviceID1 = deviceID;
      Point point = outPoint.Value;
      int x = point.X;
      point = outPoint.Value;
      int y = point.Y;
      ADBHelper.Tap(deviceID1, x, y);
    }

    public void Change_Proxy(string deviceID, string ip_proxy, string port_proxy) => ADBHelper.ExecuteCMD(string.Format("adb -s {0} shell settings put global http_proxy {1}:{2}", (object) deviceID, (object) ip_proxy, (object) port_proxy));

    public void Remove_Proxy(string deviceID) => ADBHelper.ExecuteCMD(string.Format("adb -s {0} shell settings put global http_proxy :0", (object) deviceID));
  }
}
