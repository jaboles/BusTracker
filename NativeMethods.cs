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
		
		public enum DevicePowerState : int
		{

			Unspecified = -1,
			D0 = 0, // Full On: full power, full functionality
			D1, // Low Power On: fully functional at low power/performance
			D2, // Standby: partially powered with automatic wake
			D3, // Sleep: partially powered with device initiated wake
			D4, // Off: unpowered
		}

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
	}
}
