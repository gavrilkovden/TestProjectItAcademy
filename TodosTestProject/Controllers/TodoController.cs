using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using TodosTestProject.Models;

namespace TodosTestProject.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private static readonly List<Todo> _todoList = new List<Todo>();

        //GET /todos - �������� ��� ������.����������� ��������� GET ��������� limit(int), offset(int). 
        //    Limit - ����������� ���������� ������������ �������, offset - ���������� ������������ �������

        [HttpGet]
        public ActionResult<IEnumerable<Todo>> GetAllTodos([FromQuery] int? limit = null, [FromQuery] int? offset = null)
        {
            var todos = _todoList;
            if (todos == null)
            {
                return NotFound();
            }

            if (offset.HasValue && offset > 0)
            {
                todos = todos.Skip(offset.Value).ToList();
            }

            if (limit.HasValue && limit > 0)
            {
                todos = todos.Take(limit.Value).ToList();
            }

            return Ok(todos);
        }

        // GET /todos/{id} - �������� ������ �� Id
        [HttpGet("{id}")]
        public ActionResult<Todo> GetById(int id)
        {
            Todo todo = _todoList.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        //   GET /todos/{id}/IsDone - �������� ���� (������� json ���� {id:1, IsDone: true})

        [HttpGet("{id}/IsDone")]
        public ActionResult<Todo> GetByIdIsDone(int id)
        {
            Todo todo = _todoList.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            return Ok(new { todo.Id, todo.IsDone });
        }

        // POST /todos - ������� ����� ������, ������� ��������� ������ � ����� ������ 201 � ������� �� ��������� ������.��������� ����� �������� ������ � UTC �������(DateTime.UtcNow)

        [HttpPost]
        public ActionResult<Todo> CreateTodoItem(Todo todo)
        {
            todo.CreatedDate = DateTime.UtcNow;
            _todoList.Add(todo);
            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }

        //  PUT /todos/{id} - �������� ������, ������� ����������� ������. �������� ���� UpdatedDate ������� UTC ��������.
        //  �� ��������� ���� CreatedDate � UpdatedDate ������� �� ���������� ������� 

        [HttpPut("{id}")]
        public ActionResult<Todo> UpdateTodoItem(int id, [FromBody] Todo updatedTodo)
        {
            Todo existingTodo = _todoList.FirstOrDefault(t => t.Id == id);

            if (existingTodo == null)
            {
                return NotFound();
            }

            existingTodo.Label = updatedTodo.Label;
            existingTodo.IsDone = updatedTodo.IsDone;
            existingTodo.UpdatedDate = DateTime.UtcNow;

            return Ok(existingTodo);
        }

        //  PATCH /todos/{id}/IsDone - �������� ���� IsDone � ���������� ������, ������ ���������� json ���� {isDone:true}, ����� � ���� {id:1, IsDone: true}

        [HttpPatch("{id}/IsDone")]
        public ActionResult<Todo> UpdateTodoIsDone(int id, [FromBody] bool isDone)
        {
            Todo existingTodo = _todoList.FirstOrDefault(t => t.Id == id);

            if (existingTodo == null)
            {
                return NotFound();
            }

            existingTodo.IsDone = isDone;
 

            return Ok(new { existingTodo.Id, existingTodo.IsDone });
        }

        //    DELETE /todos/{id} - ������� ������ 

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            Todo todo = _todoList.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            _todoList.Remove(todo);
            return Ok("Todo deleted successfully.");
        }

                    
    }
}