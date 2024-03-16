

using Todos.Domain;

namespace Common.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }

        //public string Login { get; set; } = default;
        //public string PasswordHash { get; set; } = default;


        public List<Todo> Todos { get; set; }
    }
}