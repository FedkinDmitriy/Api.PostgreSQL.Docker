using Data.DTO;
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
        [ProducesResponseType(typeof(IReadOnlyList<Client>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IReadOnlyList<Client>>> Get(CancellationToken cancellationToken)
        {
            var clients = await _clients.GetAllAsync(cancellationToken);

            return clients.Count > 0 ? Ok(clients) : NoContent();
        }


        // GET api/<ClientsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClientDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientDTO>> Get(int id, CancellationToken cancellationToken)
        {
            var client = await _clients.GetByIdAsync(id, cancellationToken);
            if (client is null) return NotFound();

            var dto = new ClientDTO
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                DateOfBirth = client.DateOfBirth,
                Orders = client.Orders?.Select(order => new OrderDTO
                {
                    Id = order.Id,
                    OrderSum = order.OrderSum,
                    OrdersDateTime = order.OrdersDateTime,
                    Status = order.Status.ToString()
                }).ToList() ?? new List<OrderDTO>()
            };
            return Ok(dto);
        }

        // POST api/<ClientsController>
        [HttpPost]
        [ProducesResponseType(typeof(Client), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Client>> Post([FromBody] Client client, CancellationToken cancellationToken)
        {
            if (client == null) return BadRequest();

            await _clients.AddAsync(client, cancellationToken);

            //return Created($"/api/clients/{client.Id}", client);
            return CreatedAtAction(nameof(Get),new { id = client.Id },client);
        }

        // PUT api/<ClientsController>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id,[FromBody] Client client,CancellationToken cancellationToken)
        {
            // 1. Проверка валидации модели
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // 2. Проверка соответствия ID в пути и теле
            if (client == null || client.Id != id)
                return BadRequest("ID в пути и теле запроса не совпадают");

            // 3. Проверка существования клиента
            var existingClient = await _clients.GetByIdAsync(id, cancellationToken);
            if (existingClient == null)
                return NotFound();

            // 4. Обновление
            await _clients.UpdateAsync(client, cancellationToken);

            return NoContent();
        }

        // DELETE api/<ClientsController>/5

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            // Валидация ID
            if (id <= 0)
                return BadRequest("ID должен быть положительным числом");

            // Проверка существования клиента
            Client? client = await _clients.GetByIdAsync(id, cancellationToken);
            if (client is null)
                return NotFound();

            // Удаление
            await _clients.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
