using System;
using System.Runtime.InteropServices;

namespace BusTracker
{
	public class NativeMethods
	{
		public const int SHFS_SHOWTASKBAR = 0x0001;
		public const int SHFS_HIDETASKBAR = 0x0002;
		public const int SHFS_SHOWSIPBUTTON = 0x0004;
		public const int SHFS_HIDESIPBUTTON = 0x0008;
		public const int SHFS_SHOWSTARTICON = 0x0010;
		public const int SHFS_HIDESTARTICON = 0x0020;
		
		// Control Code flags
		public const uint FILE_DEVICE_UNKNOWN = 0x00000022;
		public const uint FILE_DEVICE_HAL = 0x00000101;
		public const uint FILE_DEVICE_CONSOLE = 0x00000102;
		public const uint FILE_DEVICE_PSL = 0x00000103;
		public const uint METHOD_BUFFERED = 0;
		public const uint METHOD_IN_DIRECT = 1;
		public const uint METHOD_OUT_DIRECT = 2;
		public const uint METHOD_NEITHER = 3;
		public const uint FILE_ANY_ACCESS = 0;
		public const uint FILE_READ_ACCESS = 0x0001;
		public const uint FILE_WRITE_ACCESS = 0x0002;

		public static uint CTL_CODE(uint DeviceType, uint Function, uint Method, uint Access)
		{
			return ((DeviceType << 16) | (Access << 14) | (Function << 2) | Method);
		}

		public enum DevicePowerState : int
		{

			Unspecified = -1,
			D0 = 0, // Full On: full power, full functionality
			D1, // Low Power On: fully functional at low power/performance
			D2, // Standby: partially powered with automatic wake
			D3, // Sleep: partially powered with device initiated wake
			D4, // Off: unpowered
		}

		public const int POWER_STATE_ON = 0x00010000;
		public const int POWER_STATE_OFF = 0x00020000;
		public const int POWER_STATE_SUSPEND = 0x00200000;
		public const int POWER_FORCE = 4096;
		public const int POWER_STATE_RESET = 0x00800000;

		[DllImport("coredll.dll")]
		public extern static bool SHFullScreen(IntPtr hWnd, long dwState);

		[DllImport("coredll.dll")]
		public static extern IntPtr GetCapture();

		[DllImport("coredll.dll")] 
		public static extern IntPtr FindWindow(string lpClassName, string 
			lpWindowName); 

		[DllImport("coredll.dll")] 
		public static extern IntPtr ShowWindow(IntPtr hWnd, int visible); 

		[DllImport("coredll.dll")]
		public static extern int ReleasePowerRequirement(int hPowerReq);

		[DllImport("coredll.dll")]
		public static extern int SetPowerRequirement(string DeviceName, DevicePowerState State, uint dwDeviceFlags, string Name, ulong Reserved);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern bool KernelIoControl(uint dwIoControlCode, byte[] inBuf, int inBufSize, byte[] outBuf, int outBufSize, ref int bytesReturned);

		[DllImport("coredll.dll", SetLastError=true)]
		public static extern int SetSystemPowerState(string psState, int StateFlags, int Options);
	}
}
