using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using TodoListService.Database;

namespace TodoListService.Services
{
    public class TodosService : TodoService.TodoServiceBase
    {
        private readonly ApplicationDbContext _context;
        public TodosService(ApplicationDbContext context)
        {
            _context = context;
        }
        public override async Task<TodoItem> AddTodo(TodoItem request, ServerCallContext context)
        {
            _context.TodoItems.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public override async Task GetTodos(Empty request, IServerStreamWriter<TodoItem> responseStream, 
                ServerCallContext context)
        {
            var todoList = await _context.TodoItems.ToListAsync();
            
            foreach (var todo in todoList)
            {
                await responseStream.WriteAsync(todo);
            }
        }
    }
}
