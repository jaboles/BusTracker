using System;
using System.Collections;

namespace BusTracker
{
	public delegate void BusInfoAvailableEventHandler(object sender, BusInfoAvailableEventArgs eventArgs);

	public class BusInfoAvailableEventArgs : EventArgs
	{
		public BusInfoAvailableEventArgs(int stopIndex, ArrayList busInfos, StopLocation loadedStop)
		{
			m_stopIndex = stopIndex;
			m_loadedStop = loadedStop;
			m_busInfos = busInfos;
		}

		public int StopIndex
		{
			get { return m_stopIndex; }
		}

		public ArrayList BusInfos
		{
			get { return m_busInfos; }
		}

		public StopLocation LoadedStop
		{
			get { return m_loadedStop; }
		}

		private ArrayList m_busInfos;
		private int m_stopIndex;
		private StopLocation m_loadedStop;
	}
}
