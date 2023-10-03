using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using TodoListService;

namespace TodogRPCClient.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoService.TodoServiceClient _todoClient;
        public TodosController()
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5000");
            _todoClient = new TodoService.TodoServiceClient(channel);
        }

        [HttpPost]
        public IActionResult AddTodo(TodoItem todoItem)
        {
            var response = _todoClient.AddTodo(todoItem);

            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            var request = new Google.Protobuf.WellKnownTypes.Empty();
            var todos = _todoClient.GetTodos(request);

            var todoList = new List<TodoItem>();

            await foreach (var todo in todos.ResponseStream.ReadAllAsync())
            {
                todoList.Add(todo);
            }

            return Ok(todoList);
        }
    }
}
