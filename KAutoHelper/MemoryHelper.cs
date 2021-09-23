// Decompiled with JetBrains decompiler
// Type: KAutoHelper.MemoryHelper
// Assembly: KAutoHelper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 84D96AAE-4B9D-42FB-BF04-9C297C245338
// Assembly location: C:\Users\hoangnv\Downloads\KAutoHelper.dll

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace KAutoHelper
{
  public class MemoryHelper
  {
    private const uint INFINITE = 4294967295;
    private const uint WAIT_ABANDONED = 128;
    private const uint WAIT_OBJECT_0 = 0;
    private const uint WAIT_TIMEOUT = 258;

    [DllImport("kernel32.dll")]
    private static extern IntPtr OpenProcess(
      uint dwDesiredAccess,
      bool bInheritHandle,
      uint dwProcessId);

    [DllImport("kernel32.dll")]
    private static extern bool ReadProcessMemory(
      IntPtr hProcess,
      IntPtr lpBaseAddress,
      byte[] lpBuffer,
      UIntPtr nSize,
      uint lpNumberOfBytesWritten);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(
      IntPtr hProcess,
      IntPtr lpBaseAddress,
      byte[] lpBuffer,
      int nSize,
      out IntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(
      IntPtr hProcess,
      IntPtr lpBaseAddress,
      [MarshalAs(UnmanagedType.AsAny)] object lpBuffer,
      int dwSize,
      out IntPtr lpNumberOfBytesWritten);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern bool VirtualFreeEx(
      IntPtr hProcess,
      IntPtr lpAddress,
      IntPtr dwSize,
      MemoryHelper.FreeType dwFreeType);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern uint WaitForSingleObject(IntPtr hProcess, uint dwMilliseconds);

    [DllImport("kernel32.dll")]
    internal static extern IntPtr CreateRemoteThread(
      IntPtr hProcess,
      IntPtr lpThreadAttributes,
      IntPtr dwStackSize,
      IntPtr lpStartAddress,
      IntPtr lpParameter,
      uint dwCreationFlags,
      IntPtr lpThreadId);

    [DllImport("kernel32.dll", SetLastError = true)]
    internal static extern IntPtr VirtualAllocEx(
      IntPtr hProcess,
      IntPtr lpAddress,
      IntPtr dwSize,
      uint flAllocationType,
      uint flProtect);

    [DllImport("kernel32.dll")]
    internal static extern int CloseHandle(IntPtr hProcess);

    [DllImport("kernel32", SetLastError = true)]
    public static extern int GetProcessId(IntPtr hProcess);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

    public static IntPtr OpenProcess(int pId, MemoryHelper.ProcessAccessFlags ProcessAccess = MemoryHelper.ProcessAccessFlags.All) => MemoryHelper.OpenProcess((uint) ProcessAccess, false, (uint) pId);

    public static IntPtr OpenProcess(uint pId, MemoryHelper.ProcessAccessFlags ProcessAccess = MemoryHelper.ProcessAccessFlags.All) => MemoryHelper.OpenProcess((uint) ProcessAccess, false, pId);

    public static int AllocateMemory(IntPtr ProcessHandle, int memorySize) => (int) MemoryHelper.VirtualAllocEx(ProcessHandle, (IntPtr) 0, (IntPtr) memorySize, 4096U, 64U);

    public static IntPtr CreateRemoteThread(IntPtr ProcessHandle, int address) => MemoryHelper.CreateRemoteThread(ProcessHandle, (IntPtr) 0, (IntPtr) 0, (IntPtr) address, (IntPtr) 0, 0U, (IntPtr) 0);

    public static void WaitForSingleObject(IntPtr ProcessHandle, IntPtr threadHandle)
    {
      if (MemoryHelper.WaitForSingleObject(threadHandle, uint.MaxValue) <= 0U)
        return;
      Console.WriteLine("Failed waiting for single object");
    }

    public static void FreeMemory(IntPtr ProcessHandle, int address) => MemoryHelper.VirtualFreeEx(ProcessHandle, (IntPtr) address, (IntPtr) 0, MemoryHelper.FreeType.Release);

    public static void CloseProcess(IntPtr ProcessHandle, IntPtr handle) => MemoryHelper.CloseHandle(handle);

    public static bool WriteInt(IntPtr Handle, IntPtr pointer, uint offset, int value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      IntPtr lpNumberOfBytesWritten = IntPtr.Zero;
      return MemoryHelper.WriteProcessMemory(Handle, (IntPtr) (long) num, bytes, bytes.Length, out lpNumberOfBytesWritten);
    }

    public static bool WriteFloat(IntPtr Handle, IntPtr pointer, uint offset, float value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      IntPtr lpNumberOfBytesWritten = IntPtr.Zero;
      return MemoryHelper.WriteProcessMemory(Handle, (IntPtr) (long) num, bytes, bytes.Length, out lpNumberOfBytesWritten);
    }

    public static bool WriteUInt(IntPtr Handle, IntPtr pointer, uint offset, uint value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      IntPtr lpNumberOfBytesWritten = IntPtr.Zero;
      return MemoryHelper.WriteProcessMemory(Handle, (IntPtr) (long) num, bytes, bytes.Length, out lpNumberOfBytesWritten);
    }

    public static bool WriteString(IntPtr Handle, IntPtr pointer, uint offset, string value)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(value);
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      IntPtr lpNumberOfBytesWritten = IntPtr.Zero;
      return MemoryHelper.WriteProcessMemory(Handle, (IntPtr) (long) num, bytes, bytes.Length, out lpNumberOfBytesWritten);
    }

    public static bool WriteStruct(IntPtr Handle, IntPtr pointer, uint offset, object value)
    {
      byte[] lpBuffer = MemoryHelper.RawSerialize(value);
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      IntPtr lpNumberOfBytesWritten = IntPtr.Zero;
      return MemoryHelper.WriteProcessMemory(Handle, (IntPtr) (long) num, lpBuffer, lpBuffer.Length, out lpNumberOfBytesWritten);
    }

    public static bool WriteBytes(IntPtr Handle, IntPtr pointer, uint offset, byte[] bytes)
    {
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      IntPtr lpNumberOfBytesWritten = IntPtr.Zero;
      return MemoryHelper.WriteProcessMemory(Handle, (IntPtr) (long) num, bytes, bytes.Length, out lpNumberOfBytesWritten);
    }

    public static bool WriteByte(IntPtr Handle, IntPtr pointer, uint offset, byte value)
    {
      byte[] bytes = BitConverter.GetBytes((short) value);
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      IntPtr lpNumberOfBytesWritten = IntPtr.Zero;
      return MemoryHelper.WriteProcessMemory(Handle, (IntPtr) (long) num, bytes, bytes.Length, out lpNumberOfBytesWritten);
    }

    public static bool WriteUnicode(IntPtr Handle, IntPtr pointer, uint offset, string value)
    {
      byte[] bytes = Encoding.Unicode.GetBytes(value);
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      IntPtr lpNumberOfBytesWritten = IntPtr.Zero;
      return MemoryHelper.WriteProcessMemory(Handle, (IntPtr) (long) num, bytes, bytes.Length, out lpNumberOfBytesWritten);
    }

    public static int ReadInt(IntPtr Handle, IntPtr pointer, uint offset)
    {
      byte[] lpBuffer = new byte[24];
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      return MemoryHelper.ReadProcessMemory(Handle, (IntPtr) (long) num, lpBuffer, (UIntPtr) 4UL, 0U) ? BitConverter.ToInt32(lpBuffer, 0) : 0;
    }

    public static uint ReadUInt(IntPtr Handle, IntPtr pointer, uint offset)
    {
      byte[] lpBuffer = new byte[24];
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      return MemoryHelper.ReadProcessMemory(Handle, (IntPtr) (long) num, lpBuffer, (UIntPtr) 4UL, 0U) ? BitConverter.ToUInt32(lpBuffer, 0) : 0U;
    }

    public static float ReadFloat(IntPtr Handle, IntPtr pointer, uint offset)
    {
      byte[] lpBuffer = new byte[24];
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      return MemoryHelper.ReadProcessMemory(Handle, (IntPtr) (long) num, lpBuffer, (UIntPtr) 4UL, 0U) ? BitConverter.ToSingle(lpBuffer, 0) : 0.0f;
    }

    public static string ReadString(IntPtr Handle, IntPtr pointer, uint offset)
    {
      byte[] numArray = new byte[24];
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      return MemoryHelper.ReadProcessMemory(Handle, (IntPtr) (long) num, numArray, (UIntPtr) (ulong) Marshal.SizeOf("".GetType()), 0U) ? Encoding.UTF8.GetString(numArray) : (string) null;
    }

    public static string ReadUnicode(IntPtr Handle, IntPtr pointer, uint offset, uint maxSize)
    {
      byte[] numArray = new byte[(int) maxSize];
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      return MemoryHelper.ReadProcessMemory(Handle, (IntPtr) (long) num, numArray, (UIntPtr) maxSize, 0U) ? MemoryHelper.ByteArrayToString(numArray, MemoryHelper.EncodingType.Unicode) : (string) null;
    }

    public static byte[] ReadBytes(IntPtr Handle, IntPtr pointer, uint offset, uint maxSize)
    {
      byte[] lpBuffer = new byte[(int) maxSize];
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      return MemoryHelper.ReadProcessMemory(Handle, (IntPtr) (long) num, lpBuffer, (UIntPtr) maxSize, 0U) ? lpBuffer : (byte[]) null;
    }

    public static object ReadStruct<T>(IntPtr Handle, IntPtr pointer, uint offset)
    {
      int length = Marshal.SizeOf((object) default (T));
      byte[] numArray = new byte[length];
      uint num = (uint) MemoryHelper.ReadPointer(Handle, pointer) + offset;
      return MemoryHelper.ReadProcessMemory(Handle, (IntPtr) (long) num, numArray, (UIntPtr) (ulong) length, 0U) ? MemoryHelper.RawDeserialize<T>(numArray, 0) : (object) null;
    }

    public static int ReadPointer(IntPtr Handle, IntPtr pointer)
    {
      byte[] lpBuffer = new byte[24];
      MemoryHelper.ReadProcessMemory(Handle, pointer, lpBuffer, (UIntPtr) 4UL, 0U);
      return BitConverter.ToInt32(lpBuffer, 0);
    }

    private static object RawDeserialize<T>(byte[] rawData, int position)
    {
      int num1 = Marshal.SizeOf((object) default (T));
      if (num1 > rawData.Length)
        return (object) null;
      IntPtr num2 = Marshal.AllocHGlobal(num1);
      Marshal.Copy(rawData, position, num2, num1);
      object structure = Marshal.PtrToStructure(num2, typeof (T));
      Marshal.FreeHGlobal(num2);
      return structure;
    }

    private static byte[] RawSerialize(object anything)
    {
      int length = Marshal.SizeOf(anything);
      IntPtr num = Marshal.AllocHGlobal(length);
      Marshal.StructureToPtr(anything, num, false);
      byte[] destination = new byte[length];
      Marshal.Copy(num, destination, 0, length);
      Marshal.FreeHGlobal(num);
      return destination;
    }

    private static string ByteArrayToString(byte[] bytes) => MemoryHelper.ByteArrayToString(bytes, MemoryHelper.EncodingType.Unicode);

    private static string ByteArrayToString(byte[] bytes, MemoryHelper.EncodingType encodingType)
    {
      Encoding encoding = (Encoding) null;
      string str = "";
      switch (encodingType)
      {
        case MemoryHelper.EncodingType.ASCII:
          encoding = (Encoding) new ASCIIEncoding();
          break;
        case MemoryHelper.EncodingType.Unicode:
          encoding = (Encoding) new UnicodeEncoding();
          break;
        case MemoryHelper.EncodingType.UTF7:
          encoding = (Encoding) new UTF7Encoding();
          break;
        case MemoryHelper.EncodingType.UTF8:
          encoding = (Encoding) new UTF8Encoding();
          break;
      }
      for (int count = 0; count < bytes.Length; count += 2)
      {
        if (bytes[count] == (byte) 0 && bytes[count + 1] == (byte) 0)
        {
          str = encoding.GetString(bytes, 0, count);
          break;
        }
      }
      return str;
    }

    private enum EncodingType
    {
      ASCII,
      Unicode,
      UTF7,
      UTF8,
    }

    [Flags]
    internal enum FreeType
    {
      Decommit = 16384, // 0x00004000
      Release = 32768, // 0x00008000
    }

    public enum ProcessAccessFlags : uint
    {
      Terminate = 1,
      CreateThread = 2,
      VMOperation = 8,
      VMRead = 16, // 0x00000010
      VMWrite = 32, // 0x00000020
      DupHandle = 64, // 0x00000040
      SetInformation = 512, // 0x00000200
      QueryInformation = 1024, // 0x00000400
      Synchronize = 1048576, // 0x00100000
      All = 2035711, // 0x001F0FFF
    }
  }
}
