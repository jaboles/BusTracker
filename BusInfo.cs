using System;

namespace BusTracker
{
	/// <summary>
	/// Summary description for BusInfo.
	/// </summary>
	public class BusInfo
	{
		public BusInfo()
		{
		}

		public string RouteNumber
		{
			get { return m_routeNumber; }
			set { m_routeNumber = value; }
		}

		public string Destination
		{
			get { return m_destination; }
			set { m_destination = value; }
		}

		public int MinutesToDeparture
		{
			get { return m_minutesToDeparture; }
			set { m_minutesToDeparture = value; }
		}

		public string ArrivalTime
		{
			get { return m_arrivalTime; }
			set { m_arrivalTime = value; }
		}

		public string ArrivalStatus
		{
			get { return m_arrivalStatus; }
			set { m_arrivalStatus = value; }
		}

		public bool IsDepartingNow
		{
			get { return m_isDepartingNow; }
			set { m_isDepartingNow = value; }
		}

		private string m_routeNumber;
		private string m_destination;
		private string m_arrivalTime;
		private string m_arrivalStatus;
		private bool m_isDepartingNow;
		private int m_minutesToDeparture;
	}
}
