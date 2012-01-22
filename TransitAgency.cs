using System;

namespace BusTracker
{
	/// <summary>
	/// Summary description for TransitAgency.
	/// </summary>
	public class TransitAgency
	{
		private int m_id;
		private string m_name;

		public TransitAgency(int id, string name)
		{
			m_id = id;
			m_name = name;
		}

		public int Id
		{
			get { return m_id; }
		}

		public string Name
		{
			get { return m_name; }
		}
	}
}
