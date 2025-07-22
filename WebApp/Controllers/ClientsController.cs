using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApp.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {

        private readonly ICRUDable<Client, int> _clients;

        public ClientsController(ICRUDable<Client, int> clients)
        {
            _clients = clients;
        }

        // GET: api/<ClientsController>
        [HttpGet]
        public async Task<Results<Ok<IReadOnlyList<Client>>,NoContent>> Get(CancellationToken cancellationToken)
        {
            var clients = await _clients.GetAllAsync(cancellationToken);
            return clients.Count > 0 ? TypedResults.Ok(clients) : TypedResults.NoContent();
        }

        // GET api/<ClientsController>/5
        [HttpGet("{id}")]
        public async Task<Results<Ok<Client>,NotFound>> Get(int id, CancellationToken cancellationToken)
        {
            Client? client = await _clients.GetByIdAsync(id, cancellationToken);
            return client is not null ? TypedResults.Ok(client) : TypedResults.NotFound();
        }

        // POST api/<ClientsController>
        [HttpPost]
        public async Task<Results<Created<Client>,BadRequest>> Post([FromBody] Client client, CancellationToken cancellationToken)
        {
            if (client == null) return TypedResults.BadRequest();

            await _clients.AddAsync(client, cancellationToken);

            return TypedResults.Created($"/api/clients/{client.Id}", client);
        }

        // PUT api/<ClientsController>
        [HttpPut]
        public async Task<Results<NoContent,BadRequest>> Put([FromBody] Client client, CancellationToken cancellationToken)
        {
            if (client == null) return TypedResults.BadRequest();

            await _clients.UpdateAsync(client, cancellationToken);

            return TypedResults.NoContent();
        }

        // DELETE api/<ClientsController>/5
        [HttpDelete("{id}")]
        public async Task<Results<NoContent, NotFound>> Delete(int id, CancellationToken cancellationToken)
        {
            Client? client = await _clients.GetByIdAsync(id, cancellationToken);
            if(client is not null)
            {
                await _clients.DeleteAsync(id, cancellationToken);
                return TypedResults.NoContent();
            }
            else
            {
                return TypedResults.NotFound();
            }
        }
    }
}
