using System.ComponentModel.DataAnnotations;

namespace MessagesProject.ModelView
{
    public class LoginModelView
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
