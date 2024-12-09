using System;

namespace ToDoMVC.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Metadata Metadata { get; set; }
    }

    public class Metadata
    {
        public string CreatedDate { get; set; }
    }
}
