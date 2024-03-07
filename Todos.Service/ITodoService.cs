﻿using Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Todos.Domain;
using Todos.Service.DTO;

namespace Todos.Service
{
    public interface ITodoService
    {
        public IEnumerable<Todo> GetAllTodos(int? offset = null, int? limit = null,
             int? ownerId = null, string? labelFreeText = null);
        public Todo GetTodo(Expression<Func<Todo, bool>>? predicate = null);
        public int GetTodoCount();
        public Todo GreateTodo(CreateTodoDTO todoDTO);
        public Todo UpdateTodo(UpdateTodoDTO todoDTO);
        public bool DeleteTodo(UpdateTodoDTO todoDTO);
    }

}