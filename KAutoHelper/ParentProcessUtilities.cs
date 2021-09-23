// Decompiled with JetBrains decompiler
// Type: KAutoHelper.ParentProcessUtilities
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace KAutoHelper
{
  public struct ParentProcessUtilities
  {
    internal IntPtr Reserved1;
    internal IntPtr PebBaseAddress;
    internal IntPtr Reserved2_0;
    internal IntPtr Reserved2_1;
    internal IntPtr UniqueProcessId;
    internal IntPtr InheritedFromUniqueProcessId;

    [DllImport("ntdll.dll")]
    private static extern int NtQueryInformationProcess(
      IntPtr processHandle,
      int processInformationClass,
      ref ParentProcessUtilities processInformation,
      int processInformationLength,
      out int returnLength);

    public static Process GetParentProcess() => ParentProcessUtilities.GetParentProcess(Process.GetCurrentProcess().Handle);

    public static Process GetParentProcess(int id) => ParentProcessUtilities.GetParentProcess(Process.GetProcessById(id).Handle);

    public static Process GetParentProcess(IntPtr handle)
    {
      ParentProcessUtilities processInformation = new ParentProcessUtilities();
      int error = ParentProcessUtilities.NtQueryInformationProcess(handle, 0, ref processInformation, Marshal.SizeOf((object) processInformation), out int _);
      if ((uint) error > 0U)
        throw new Win32Exception(error);
      try
      {
        return Process.GetProcessById(processInformation.InheritedFromUniqueProcessId.ToInt32());
      }
      catch (ArgumentException ex)
      {
        return (Process) null;
      }
    }
  }
}
