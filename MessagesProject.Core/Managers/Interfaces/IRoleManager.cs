using MessagesProject.ModelView;

namespace MessagesProject.Core.Managers.Interfaces
{
    public interface IRoleManager : IManager
    {
        bool CheckAccess(UserModel userModel);
    }
}
