namespace jsfootball_api.Models
{
    public class LeaguePosition
    {
        public int Position {get;set;}
        public Team Team {get;set;}
        public int Played {get;set;}
        public int Won {get;set;}
        public int Drawn {get;set;}
        public int Lost {get;set;}
        public int For {get;set;}
        public int Against {get;set;}
        public int GoalDifference {get;set;}
        public int Points {get;set;}        
    }
}