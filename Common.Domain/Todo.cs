﻿using Common.Domain;
using System.Text.Json.Serialization;

namespace Todos.Domain
{
    public class Todo
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public bool IsDone { get; set; } = false;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public int OwnerId { get; set; }
       
        [JsonIgnore]
        public ApplicationUser Owner { get; set; }
    }
}