using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace BusTracker
{
	/// <summary>
	/// Summary description for InfoFetcher.
	/// </summary>
	public class InfoFetcher
	{
		public event BusInfoAvailableEventHandler BusInfoAvailable;
		public event EventHandler BusInfoFetchComplete;
		public event EventHandler BusInfoSourceAvailabilityChanged;

		public InfoFetcher()
		{
			m_thread = null;
			m_busInfoList = new ArrayList();
			m_shouldStop = false;
			m_shouldRun = false;

			m_busInfoRegex = new Regex(
				"arrivalsRouteEntry\">[^>]+>(\\w*)<.+?arrivalsDestinationEntry\">[^>]+>([^<]*)<.+?arrivalsTimeEntry\">([^<]+)<.+?statusLabel[^>]+>([^<]*)<.+?arrivalsStatusEntry.+?>(NOW|[0-9\\-]+)<",
				RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline
			);

			m_thread = new Thread(new ThreadStart(ThreadEntry));
			m_thread.Start();
		}

		public void Start(StopLocation[] stopsToLoad)
		{
			m_stopsToLoad = stopsToLoad;
			m_shouldRun = true;
		}

		public void Stop()
		{
			m_shouldStop = true;
		}

		private void ThreadEntry()
		{
			try
			{
				while (!m_shouldStop)
				{
					System.Threading.Thread.Sleep(1000);
					MainRoutine();
				}
			}
			catch (Exception)
			{
				// This will crash the app, so restart the PDA.
				if (!MainForm.s_attemptingQuit)
				{
					HAL.Reset();
				}
			}
		}

		private void MainRoutine()
		{
			int errorCount = 0;

			if (m_shouldRun)
			{
				m_shouldRun = false;

				for (int i = 0; i < m_stopsToLoad.Length && errorCount < ERROR_LIMIT; i++)
				{
					try
					{
						StopLocation sl = m_stopsToLoad[i];
						HttpWebRequest req = (HttpWebRequest)WebRequest.Create(sl.Url);
						req.Timeout = 10000;
						req.KeepAlive = true;
						WebResponse response = req.GetResponse();
						Stream receiveStream = response.GetResponseStream();
						Encoding encoding = Encoding.GetEncoding("utf-8");
						StreamReader sr = new StreamReader(receiveStream, encoding);
						string data = sr.ReadToEnd();
						sr.Close();
						
						m_busInfoList.Clear();
						for (Match m = m_busInfoRegex.Match(data); m.Success; m = m.NextMatch())
						{
							string routeNumber = m.Groups[1].Value.Trim();
							string destination = m.Groups[2].Value.Trim();
							string arrivalTime = m.Groups[3].Value.Trim();
							string arrivalStatus = m.Groups[4].Value.Trim();
							string minutesToDeparture = m.Groups[5].Value.Trim();

							BusInfo b = new BusInfo();
							b.RouteNumber = routeNumber;
							b.Destination = HttpUtility.HtmlDecode(destination);
							b.ArrivalTime = arrivalTime;
							b.ArrivalStatus = arrivalStatus;
							if (minutesToDeparture.ToUpper().Equals("NOW"))
							{
								b.MinutesToDeparture = 0;
								b.IsDepartingNow = true;
							}
							else
							{
								b.IsDepartingNow = false;
								b.MinutesToDeparture = Convert.ToInt32(minutesToDeparture);
							}

							m_busInfoList.Add(b);
						}
						
						// A successful request. We're online!
						errorCount = 0;
						UpdateInfoSourceAvailable(true);

						BusInfoAvailableEventArgs ea = new BusInfoAvailableEventArgs(i, m_busInfoList, sl);
						BusInfoAvailable(this, ea);
					}
					catch (WebException)
					{
						// try again
						i--;
						errorCount++;
					}
					catch (SocketException)
					{
						// try again
						i--;
						errorCount++;
					}
				}

				if (errorCount >= ERROR_LIMIT)
				{
					UpdateInfoSourceAvailable(false);
				}
				else
				{
					BusInfoFetchComplete(this, EventArgs.Empty);
				}
			}
		}

		private void UpdateInfoSourceAvailable(bool available)
		{
			bool hasChanged = m_infoSourceAvailable != available;
			m_infoSourceAvailable = available;
			if (hasChanged)
				BusInfoSourceAvailabilityChanged(this, EventArgs.Empty);
		}

		public bool InfoSourceAvailable
		{
			get
			{
				return m_infoSourceAvailable;
			}
		}

		private StopLocation[] m_stopsToLoad;
		private Thread m_thread;
		private Regex m_busInfoRegex;
		private ArrayList m_busInfoList;
		private bool m_shouldRun;
		private bool m_shouldStop;
		private bool m_infoSourceAvailable = true;

		private const int ERROR_LIMIT = 2;
	}
}
