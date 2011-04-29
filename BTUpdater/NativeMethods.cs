using System;
using System.Runtime.InteropServices;

namespace BTUpdater
{
	public class NativeMethods
	{
		[StructLayout(LayoutKind.Sequential)]
		public struct ProcessInfo
		{
			public IntPtr hProcess;
			public IntPtr hThread;
			public Int32 ProcessId;
			public Int32 ThreadId;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SecurityAttributes
		{
			public int length;
			public IntPtr lpSecurityDescriptor;
			public bool bInheritHandle;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct StartupInfo
		{
			public uint cb;
			public string lpReserved;
			public string lpDesktop;
			public string lpTitle;
			public uint dwX;
			public uint dwY;
			public uint dwXSize;
			public uint dwYSize;
			public uint dwXCountChars;
			public uint dwYCountChars;
			public uint dwFillAttribute;
			public uint dwFlags;
			public short wShowWindow;
			public short cbReserved2;
			public IntPtr lpReserved2;
			public IntPtr hStdInput;
			public IntPtr hStdOutput;
			public IntPtr hStdError;
		}


		[DllImport("coredll.dll", CallingConvention = CallingConvention.Winapi,CharSet = CharSet.Auto)]
		public static extern bool CreateProcess(
			string lpApplicationName,
			string lpCommandLine,
			IntPtr lpProcessAttributes,
			IntPtr lpThreadAttributes,
			bool bInheritHandles,
			uint dwCreationFlags,
			IntPtr lpEnvironment,
			string lpCurrentDirectory,
			byte[] lpStartupInfo,
			out ProcessInfo lpProcessInformation
			);
	}
}
