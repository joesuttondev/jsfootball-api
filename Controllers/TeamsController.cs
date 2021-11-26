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
    public class TeamsController : ControllerBase
    {
     private readonly IDocumentClient _documentClient;
        readonly String databaseId;
        readonly String collectionId;

        public TeamsController(IDocumentClient documentClient)
        {
              _documentClient = documentClient;

            databaseId = "Football";
            collectionId = "Teams"; 

            BuildCollection().Wait();
        }
 private async Task BuildCollection()
        {
            await _documentClient.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseId });
            await _documentClient.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseId),
                new DocumentCollection { Id = collectionId });
        }

        [HttpGet]
        public IQueryable<Team> Get()
        {
            return _documentClient.CreateDocumentQuery<Team>(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId),
                new FeedOptions { MaxItemCount = 20 });
        }

        [HttpGet("{id}")]
        public IQueryable<Team> Get(string id)
        {
            return _documentClient.CreateDocumentQuery<Team>(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId),
                new FeedOptions { MaxItemCount = 1 }).Where((i) => i.id == id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Fixture item) 
        {
            var response = await _documentClient.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId),item);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Fixture item) 
        {
            await _documentClient.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(databaseId, collectionId, id),
                item);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _documentClient.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseId, collectionId, id));
            return Ok();
        }


    }
}