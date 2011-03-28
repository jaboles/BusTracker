using System;

namespace BusTracker
{
	public class StopLocation
	{
		public StopLocation(string friendlyName, int maxBuses, string url, int timeThreshold)
		{
			m_friendlyName = friendlyName;
			m_url = url;
			m_maxBuses = maxBuses;
			m_timeThreshold = timeThreshold;
		}

		public StopLocation(string friendlyName, int maxBuses, string url)
			: this(friendlyName, maxBuses, url, -1)
		{
		}

		public string FriendlyName
		{
			get { return m_friendlyName; }
		}

		public string Url
		{
			get { return m_url; }
		}

		public int MaxBuses
		{
			get { return m_maxBuses; }
		}

		public int TimeThreshold
		{
			get { return m_timeThreshold; }
		}

		private string m_friendlyName;
		private string m_url;
		private int m_maxBuses;
		private int m_timeThreshold;
	}
}
