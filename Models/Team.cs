using System;

namespace jsfootball_api.Models
{
  public class Team
	{
		public string id { get; set; }
		public Area area { get; set; }
		public string name { get; set; }
		public string shortName { get; set; }
		public string tla { get; set; }
		public string crestUrl { get; set; }
		public string address { get; set; }
		public string phone { get; set; }
		public string website { get; set; }
		public string email { get; set; }
		public int founded { get; set; }
		public string clubColors { get; set; }
		public string venue { get; set; }
		public DateTime lastUpdated { get; set; }
	}

	public class Area
	{
		public string id { get; set; }
		public string name { get; set; }
	}
}