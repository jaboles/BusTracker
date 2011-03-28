using System;

namespace BusTracker
{
	public class StopsToLoad
	{
		public static StopLocation[] Stops = new StopLocation[4]
		{
			new StopLocation("U-District", 2, "http://www.onebusaway.org/where/standard/stop.action?id=1_29350"),
			new StopLocation("Cap Hill", 2, "http://www.onebusaway.org/where/standard/stop.action?id=1_29248&route=1_43"),
			new StopLocation("Eastside", 3, "http://www.onebusaway.org/where/standard/stop.action?id=1_71350&id=1_25243&route=1_268&route=1_280&route=40_545&route=40_555&route=1_271&route=1_272&route=40_542&route=40_556", 5),
			new StopLocation("City - Freeway", 3, "http://www.onebusaway.org/where/standard/stop.action?id=1_71344", 5)

		};
	}
}
