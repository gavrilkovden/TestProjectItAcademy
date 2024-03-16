using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todos.Service.DTO
{
    public class CreateTodoDTO
    {
        public string Label { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int OwnerId { get; set; }
    }
}
