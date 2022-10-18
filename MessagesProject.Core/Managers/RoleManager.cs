using AutoMapper;
using MessagesProject.Core.Managers.Interfaces;
using MessagesProject.ModelView;
using System.Linq;
using MessagesProject.Models;

namespace MessagesProject.Core.Managers
{
    public class RoleManager : IRoleManager
    {

        private ToDoListDataContext _taskdbContext;
        private IMapper _mapper;

        public RoleManager(ToDoListDataContext csvdbContext, IMapper mapper)
        {
            _taskdbContext = csvdbContext;
            _mapper = mapper;
        }

        public bool CheckAccess(UserModel userModel)
        {
            var isAdmin = _taskdbContext.Users
                                       .Any(a => a.Id == userModel.Id 
                                                 && a.IsAdmin);
            return isAdmin;
        }
    }
}
