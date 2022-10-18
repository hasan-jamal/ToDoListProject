using System;
using System.Collections.Generic;

#nullable disable

namespace MessagesProject.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int AssignedTo { get; set; }
        public int AssignedBy { get; set; }
        public bool IsRead { get; set; }
        public bool Archived { get; set; }

        public virtual User AssignedByNavigation { get; set; }
        public virtual User AssignedToNavigation { get; set; }
    }
}
