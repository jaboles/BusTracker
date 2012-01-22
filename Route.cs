using System;

namespace BusTracker
{
	public class Route
	{
		private int m_number;
		private TransitAgency m_transitAgency;

		public Route(int number, TransitAgency transitAgency)
		{
			m_number = number;
			m_transitAgency = transitAgency;
		}

		public int Number
		{
			get { return m_number; }
		}

		public TransitAgency TransitAgency
		{
			get { return m_transitAgency; }
		}

		public string Id
		{
			get { return string.Format("{0}_{1}", TransitAgency.Id, Number); }
		}

		public static Route[] AllRoutes = new Route[] { };
	}
}
