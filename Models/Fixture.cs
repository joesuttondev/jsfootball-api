using System;

namespace jsfootball_api.Models
{
  public class Fixture
	{
        public string id { get; set; }
        public string season { get; set; }
        public DateTime UtcDate { get; set; }
        public string status { get; set; }
        public long matchday { get; set; }
        public string stage { get; set; }
        public Score score { get; set; }
        public Team homeTeam { get; set; }
        public Team awayTeam { get; set; }
        //public List<Referee>? referees { get; set; }
    }

     public partial class Score
    {
        public string winner { get; set; }
        public string duration { get; set; }
        public Period fullTime { get; set; }
        public Period halfTime { get; set; }
        public Period extraTime { get; set; }
        public Period penalties { get; set; }
    }

     public partial class Period
    {
        public long homeTeam { get; set; }
        public long awayTeam { get; set; }
    }

      public partial class Referee
    {
        public string id { get; set; }
        public string name { get; set; }
        public string role { get; set; }
        public string nationality { get; set; }
    }
}