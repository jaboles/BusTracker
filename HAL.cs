using System;

namespace BusTracker
{
	public class HAL
	{
		public static void Reset()
		{
			uint IOCTL_HAL_REBOOT = NativeMethods.CTL_CODE(NativeMethods.FILE_DEVICE_HAL, 15, NativeMethods.METHOD_BUFFERED, NativeMethods.FILE_ANY_ACCESS);
			int dummy = 0;
			NativeMethods.KernelIoControl(IOCTL_HAL_REBOOT, null, 0, null, 0, ref dummy);
		}

		public static void ScreenOff()
		{
			throw new NotSupportedException();
		}

		public static void ScreenOn()
		{
			throw new NotSupportedException();
		}
	}
}
