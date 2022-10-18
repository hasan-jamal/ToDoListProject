
using MessagesProject.Core.Managers.Interfaces;
using MessagesProject.ModelView;

namespace MessagesProject.Core.Managers.Interfaces
{
    public interface IUserManager : IManager
    {
        UserModel UpdateProfile(UserModel currentUser, UserModel request);

        LoginUserResponse Login(LoginModelView userReg);

        LoginUserResponse SignUp(UserRegistrationModel userReg);

        void DeleteUser(UserModel currentUser, int id);
    }
}
