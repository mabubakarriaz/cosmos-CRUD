using cosmos_crud.Models;
using cosmos_crud.Services;
using Microsoft.AspNetCore.Mvc;

namespace cosmos_crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoService _todoService;

        public TodoController(TodoService todoService)
        {
            _todoService = todoService;
        }

        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodoItem(TodoItem todoItem)
        {
            todoItem.CreatedOn = DateTime.UtcNow;
            var createdItem = await _todoService.CreateTodoItemAsync(todoItem);
            return CreatedAtAction(nameof(GetTodoItem), new { id = createdItem.Id }, createdItem);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(string id)
        {
            var item = await _todoService.GetTodoItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllTodoItems()
        {
            var items = await _todoService.GetAllTodoItemsAsync();
            return Ok(items);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(string id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            var item = await _todoService.GetTodoItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            todoItem.PartitionKey = item.PartitionKey;
            todoItem.RowKey = item.RowKey;
            await _todoService.UpdateTodoItemAsync(todoItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(string id)
        {
            var item = await _todoService.GetTodoItemAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            await _todoService.DeleteTodoItemAsync(id);
            return NoContent();
        }
    }
}
