using System;
using jsfootball_api.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace jsfootball_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeagueController : ControllerBase
    {
        private readonly IDocumentClient _documentClient;
        readonly String databaseId;
        readonly String collectionId;
        readonly String fixtrueCollectionId;

        public LeagueController(IDocumentClient documentClient)
        {
            _documentClient = documentClient;

            databaseId = "Football";
            collectionId = "Teams";
            fixtrueCollectionId = "Fixtures";

            BuildCollection().Wait();
        }

        private async Task BuildCollection()
        {
            await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseId });
            await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseId),
                new DocumentCollection { Id = collectionId });
        }

        [HttpGet]
        public IQueryable<LeaguePosition> Get()
        {
            var teams = _documentClient.CreateDocumentQuery<Team>(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId)).ToList();
            var fixtures = _documentClient.CreateDocumentQuery<Fixture>(UriFactory.CreateDocumentCollectionUri(databaseId, fixtrueCollectionId)).Where(f => f.status.ToLower().Equals("finished")).ToList();

            var positionDict = new Dictionary<string, LeaguePosition>();
            var positions = new List<LeaguePosition>();
            foreach (var fixture in fixtures)
            {
                LeaguePosition home, away;
                if (!positionDict.TryGetValue(fixture.homeTeam.id, out home))
                {
                    home = new LeaguePosition() { Team = teams.Where(t => t.id == fixture.homeTeam.id).First() };
                    positions.Add(home);
                    positionDict.Add(fixture.homeTeam.id, home);
                }
                if (!positionDict.TryGetValue(fixture.awayTeam.id, out away))
                {
                    away = new LeaguePosition() { Team = teams.Where(t => t.id == fixture.awayTeam.id).First() };
                    positions.Add(away);
                    positionDict.Add(fixture.awayTeam.id, away);
                }

                bool homeWin = ((fixture.score.fullTime.awayTeam ?? 0) < (fixture.score.fullTime.homeTeam ?? 0)) ? true : false;
                bool draw = ((fixture.score.fullTime.awayTeam ?? 0) == (fixture.score.fullTime.homeTeam ?? 0)) ? true : false;
                bool awayWin = ((fixture.score.fullTime.awayTeam ?? 0) > (fixture.score.fullTime.homeTeam ?? 0)) ? true : false;

                home.Against += fixture.score.fullTime.awayTeam ?? 0;
                home.Drawn += draw ? 1 : 0;
                home.For += fixture.score.fullTime.homeTeam ?? 0;
                home.GoalDifference = home.For - home.Against;
                home.Lost += awayWin ? 1 : 0;
                home.Played++;
                home.Points += homeWin ? 3 : draw ? 1 : 0;
                home.Won += homeWin ? 1 : 0;

                away.Against += fixture.score.fullTime.homeTeam ?? 0;
                away.Drawn += draw ? 1 : 0;
                away.For += fixture.score.fullTime.awayTeam ?? 0;
                away.GoalDifference = away.For - away.Against;
                away.Lost += homeWin ? 1 : 0;
                away.Played++;
                away.Points += awayWin ? 3 : draw ? 1 : 0;
                away.Won += awayWin ? 1 : 0;
            }

            positions = positions.OrderByDescending(p => p.Points).ThenByDescending(p => p.GoalDifference).ThenByDescending(positions => positions.For).ToList();
            positions.Select((p, i) => {p.Position = i+1; return p;}).ToList();

            return positions.OrderByDescending(p => p.Points).ThenByDescending(p => p.GoalDifference).ThenByDescending(positions => positions.For).AsQueryable<LeaguePosition>();
        }      
    }
}