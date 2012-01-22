using System;

namespace BusTracker
{
	public class StopLocation
	{
		public StopLocation(string friendlyName, string id, int maxBuses, int timeThreshold, params Route[] routes)
		{
			m_friendlyName = friendlyName;
			m_id = id;
			m_maxBuses = maxBuses;
			m_timeThreshold = timeThreshold;
			m_routes = routes;

			string routeString = null;
			for (int i = 0; i < m_routes.Length; i++)
			{
				routeString = routeString + "&route=" + m_routes[i].Id;
			}
			m_url = string.Format("http://www.onebusaway.org/where/standard/stop.action?id={0}{1}", m_id, routeString);
		}

		public StopLocation(string friendlyName, string id, int maxBuses, params Route[] routes)
			: this(friendlyName, id, maxBuses, -1, routes)
		{
		}

		public string Id
		{
			get { return m_id; }
		}

		public string FriendlyName
		{
			get { return m_friendlyName; }
		}

		public Route[] Routes
		{
			get { return m_routes; }
		}

		public int MaxBuses
		{
			get { return m_maxBuses; }
		}

		public int TimeThreshold
		{
			get { return m_timeThreshold; }
		}


		public string Url
		{
			get	{ return m_url;	}
		}

		private string m_friendlyName;
		private string m_id;
		private Route[] m_routes;
		private string m_url;
		private int m_maxBuses;
		private int m_timeThreshold;
	}
}
