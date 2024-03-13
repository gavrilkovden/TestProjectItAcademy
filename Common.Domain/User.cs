

using Todos.Domain;

namespace Common.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }

        public List<Todo> Todos { get; set; }
    }
}