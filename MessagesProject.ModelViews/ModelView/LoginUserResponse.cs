using System.ComponentModel;

namespace MessagesProject.ModelView
{
    public class LoginUserResponse
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DefaultValue("")]
        public string Image { get; set; }

        public string Email { get; set; }

        public string  Token { get; set; }
    }
}
