// Decompiled with JetBrains decompiler
// Type: KAutoHelper.Port
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

namespace KAutoHelper
{
    public class Port
    {
        public string name
        {
            get => string.Format("{0} ({1} port {2})", (object)this.process_name, (object)this.protocol, (object)this.port_number);
            set
            {
            }
        }

        public string port_number { get; set; }

        public string process_name { get; set; }

        public string protocol { get; set; }

        public int pid { get; set; }
    }
}
