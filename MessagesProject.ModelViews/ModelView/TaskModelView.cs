using MessagesProject.Models;
using MessagesProject.ModelView;
using System;

namespace CsvWorker.ModelViews.ModelView
{
    public class TaskModelView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int AssignedTo { get; set; }
        public int AssignedBy { get; set; }
       
    }
}
