using System;

namespace BusTracker
{
	public class HAL
	{
		private static bool s_screenOn = true;

		private static void _Reset_KernelIoControl()
		{
			uint IOCTL_HAL_REBOOT = NativeMethods.CTL_CODE(NativeMethods.FILE_DEVICE_HAL, 15, NativeMethods.METHOD_BUFFERED, NativeMethods.FILE_ANY_ACCESS);
			int dummy = 0;
			NativeMethods.KernelIoControl(IOCTL_HAL_REBOOT, null, 0, null, 0, ref dummy);
		}

		private static void _Reset_SetSystemPowerState()
		{
			NativeMethods.SetSystemPowerState(null, NativeMethods.POWER_STATE_RESET, 0);
		}

		public static void Reset()
		{
			_Reset_SetSystemPowerState();
		}

		public static void ScreenOff()
		{
			NativeMethods.DevicePowerNotify("BKL1:", NativeMethods.DevicePowerState.D3, 1);
			s_screenOn = false;
		}

		public static void ScreenOn()
		{
			NativeMethods.DevicePowerNotify("BKL1:", NativeMethods.DevicePowerState.D0, 1);
			s_screenOn = true;
		}

		public static bool IsScreenOn()
		{
			return s_screenOn;
		}
	}
}
