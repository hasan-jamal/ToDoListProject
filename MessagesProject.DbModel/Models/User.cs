using System;
using System.Collections.Generic;

#nullable disable

namespace MessagesProject.Models
{
    public  class User
    {
        public User()
        {
            TaskAssignedByNavigations = new HashSet<Task>();
            TaskAssignedToNavigations = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsAdmin { get; set; }
        public bool Archived { get; set; }

        public virtual ICollection<Task> TaskAssignedByNavigations { get; set; }
        public virtual ICollection<Task> TaskAssignedToNavigations { get; set; }
    }
}
