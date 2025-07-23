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
        public async Task<Results<Ok<IReadOnlyList<Order>>, NoContent>> Get(CancellationToken cancellationToken)
        {
            var orders = await _orders.GetAllAsync(cancellationToken);
            return orders.Count > 0 ? TypedResults.Ok(orders) : TypedResults.NoContent();
        }

        // GET api/<OrdersController>/5
        [HttpGet("{id}")]
        public async Task<Results<Ok<OrderDTO>, NotFound>> Get(int id, CancellationToken cancellationToken)
        {
            var order = await _orders.GetByIdAsync(id, cancellationToken);
            if (order is null) return TypedResults.NotFound();

            var dto = new OrderDTO { Id = order.Id, OrdersDateTime = order.OrdersDateTime, OrderSum = order.OrderSum, Status = order.Status.ToString() };

            return TypedResults.Ok(dto);
        }

        // POST api/<OrdersController>
        [HttpPost]
        public async Task<Results<Created<Order>, BadRequest>> Post([FromBody] Order order, CancellationToken cancellationToken)
        {
            if (order == null) return TypedResults.BadRequest();

            await _orders.AddAsync(order, cancellationToken);

            return TypedResults.Created($"/api/orders/{order.Id}", order);
        }

        // PUT api/<OrdersController>
        [HttpPut]
        public async Task<Results<NoContent, BadRequest>> Put([FromBody] Order order, CancellationToken cancellationToken)
        {
            if (order == null) return TypedResults.BadRequest();

            await _orders.UpdateAsync(order, cancellationToken);

            return TypedResults.NoContent();
        }

        // DELETE api/<OrdersController>/5
        [HttpDelete("{id}")]
        public async Task<Results<NoContent, NotFound>> Delete(int id, CancellationToken cancellationToken)
        {
            Order? order = await _orders.GetByIdAsync(id, cancellationToken);
            if (order is not null)
            {
                await _orders.DeleteAsync(id, cancellationToken);
                return TypedResults.NoContent();
            }
            else
            {
                return TypedResults.NotFound();
            }
        }
    }
}
