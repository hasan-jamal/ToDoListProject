using MessagesProject.ModelView;

namespace MessagesProject.Core.Managers.Interfaces
{
    public interface ICommonManager : IManager
    {
        UserModel GetUserRole(UserModel user);
    }
}
