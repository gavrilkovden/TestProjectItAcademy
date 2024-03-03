using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Todos.Api
{
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        private static readonly List<Todo> _todoList = new List<Todo>();

        //GET /todos - получить все записи.Опционально принимать GET параметры limit(int), offset(int). 
        //    Limit - максимально количество возвращаемых записей, offset - количество пропускаемых записей

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

        // GET /todos/{id} - получить запись по Id
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

        //   GET /todos/{id}/IsDone - получить флаг (вернуть json вида {id:1, IsDone: true})

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

        // POST /todos - создать новую запись, вернуть созданную запись с кодом ответа 201 и ссылкой на созданный ресурс.Сохранить время создание записи в UTC формате(DateTime.UtcNow)

        [HttpPost]
        public ActionResult<Todo> CreateTodoItem(Todo todo)
        {
            todo.CreatedDate = DateTime.UtcNow;
            _todoList.Add(todo);
            return CreatedAtAction(nameof(GetById), new { id = todo.Id }, todo);
        }

        //  PUT /todos/{id} - обновить запись, вернуть обновленную запись. Обновить поле UpdatedDate текущим UTC временем.
        //  Не обновлять поля CreatedDate и UpdatedDate данными от клиентской стороны 

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

        //  PATCH /todos/{id}/IsDone - обновить поле IsDone у конкретной записи, запрос отправляет json вида {isDone:true}, ответ в виде {id:1, IsDone: true}

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

        //    DELETE /todos/{id} - удалить запись 

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
