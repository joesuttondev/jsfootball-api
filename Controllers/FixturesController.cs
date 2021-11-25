using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using jsfootball_api.Models;

namespace jsfootball_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FixturesController : ControllerBase
    {
         private readonly FixturesContext _context;

        public FixturesController(FixturesContext context)
        {
            _context = context;
        }

           // GET: api/Fixtures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fixture>>> GetFixtures()
        {             
            return await _context.Fixtures.ToListAsync();
        }

        // GET: api/Teams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fixture>> GetFixture(string id)
        {
            var fixture = await _context.Fixtures.FindAsync(id);

            if (fixture == null)
            {
                return NotFound();
            }

            return fixture;
        }        
    }
}