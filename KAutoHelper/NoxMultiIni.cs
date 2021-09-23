// Decompiled with JetBrains decompiler
// Type: KAutoHelper.NoxMultiIni
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace KAutoHelper
{
    public class NoxMultiIni
    {
        public int pid { get; set; }

        public int vmpid { get; set; }

        private static List<Port> Ports { get; set; }

        public static List<NoxMultiIni> GetNoxMultiIni()
        {
            List<NoxMultiIni> noxMultiIniList = new List<NoxMultiIni>();
            string[] strArray = File.ReadAllLines(Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName + "\\Local\\Nox\\multi.ini");
            for (int index = 0; index < strArray.Length; index += 4)
                noxMultiIniList.Add(new NoxMultiIni()
                {
                    pid = Convert.ToInt32(strArray[index + 1].Split('=')[1]),
                    vmpid = Convert.ToInt32(strArray[index + 2].Split('=')[1])
                });
            return noxMultiIniList;
        }

        public static string GetNoxTitleFromADBPort(string port)
        {
            NoxMultiIni.Ports = ProcessHelper.GetNetStatPorts();
            List<NoxMultiIni> noxMultiIni = NoxMultiIni.GetNoxMultiIni();
            Port abc = NoxMultiIni.Ports.Where<Port>((Func<Port, bool>)(p => p.port_number == port)).FirstOrDefault<Port>();
            return Process.GetProcessById(noxMultiIni.Where<NoxMultiIni>((Func<NoxMultiIni, bool>)(p => p.vmpid == abc.pid)).FirstOrDefault<NoxMultiIni>().pid).MainWindowTitle;
        }
    }
}
