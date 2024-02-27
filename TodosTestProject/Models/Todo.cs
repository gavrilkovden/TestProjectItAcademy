using Newtonsoft.Json;

namespace TodosTestProject.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public bool IsDone { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
