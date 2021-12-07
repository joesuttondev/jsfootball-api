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
    public class FixturesController : ControllerBase
    {
        private readonly IDocumentClient _documentClient;
        readonly String databaseId;
        readonly String collectionId;
        readonly String teamsCollectionId;

        public FixturesController(IDocumentClient documentClient)
        {
            _documentClient = documentClient;

            databaseId = "Football";
            collectionId = "Fixtures";
            teamsCollectionId = "Teams";

            BuildCollection().Wait();
        }
        private async Task BuildCollection()
        {
            await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseId });
            await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseId),
                new DocumentCollection { Id = collectionId });
        }

        [HttpGet]
        public IQueryable<Fixture> Get(string status = "")
        {
            var fixtures = _documentClient.CreateDocumentQuery<Fixture>(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId),
                new FeedOptions { MaxItemCount = 1 }).Where((i) => string.IsNullOrEmpty(status) || i.status.ToLower().Equals(status.ToLower()));
            InjectTeam(ref fixtures);
            return fixtures;
        }

        [Route("team/{teamid}")]
        [Route("~/api/teams/{teamid}/fixtures")]
        public IQueryable<Fixture> GetTeamFixtures(string teamid, string status = "")
        {
            var fixtures = _documentClient.CreateDocumentQuery<Fixture>(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId),
                new FeedOptions { MaxItemCount = 1 })
                .Where((i) => ((string.IsNullOrEmpty(status) || i.status.ToLower().Equals(status.ToLower())) && (i.homeTeam.id == teamid || i.awayTeam.id == teamid)));
            InjectTeam(ref fixtures);
            return fixtures;
        }

        private void InjectTeam(ref IQueryable<Fixture> fixtures)
        {
            var teams = _documentClient.CreateDocumentQuery<Team>(UriFactory.CreateDocumentCollectionUri(databaseId, teamsCollectionId)).ToDictionary(t => t.id, t => t);
            fixtures = fixtures.ToList().Select(f => { f.homeTeam = teams[f.homeTeam.id]; f.awayTeam = teams[f.awayTeam.id]; return f; }).AsQueryable<Fixture>();
        }
    }
}