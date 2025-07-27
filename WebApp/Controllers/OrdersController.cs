using Data.DTO;
using Data.Models;
using Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : Controller
    {
        private readonly ICRUDable<Order, int> _orders;

        public OrdersController(ICRUDable<Order, int> orders)
        {
            _orders = orders;
        }

        // GET: api/<OrdersController>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<Order>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<IReadOnlyList<Order>>> Get(CancellationToken cancellationToken)
        {
            var orders = await _orders.GetAllAsync(cancellationToken);
            return orders.Count > 0 ? Ok(orders) : NoContent();
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDTO>> Get(int id, CancellationToken cancellationToken)
        {
            var order = await _orders.GetByIdAsync(id, cancellationToken);
            if (order is null) return NotFound();

            var dto = new OrderDTO { Id = order.Id, OrdersDateTime = order.OrdersDateTime, OrderSum = order.OrderSum, Status = order.Status.ToString() };

            return Ok(dto);
        }

        // POST api/<OrdersController>
        [HttpPost]
        [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> Post([FromBody] Order order, CancellationToken cancellationToken)
        {
            if (order == null) return BadRequest();

            await _orders.AddAsync(order, cancellationToken);

            //return TypedResults.Created($"/api/orders/{order.Id}", order);
            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        // PUT api/<OrdersController>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Put(int id, [FromBody] Order order, CancellationToken cancellationToken)
        {
            // 1. Проверка валидации модели
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            // 2. Проверка соответствия ID в пути и теле
            if (order == null || order.Id != id)
                return BadRequest("ID в пути и теле запроса не совпадают");

            if (order == null) return NotFound();

            await _orders.UpdateAsync(order, cancellationToken);

            return NoContent();
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            // Валидация ID
            if (id <= 0)
                return BadRequest("ID должен быть положительным числом");

            Order? order = await _orders.GetByIdAsync(id, cancellationToken);
            if (order is not null)
            {
                await _orders.DeleteAsync(id, cancellationToken);
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
