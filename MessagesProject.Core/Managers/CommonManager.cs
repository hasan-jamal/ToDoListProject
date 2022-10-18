using AutoMapper;
using MessagesProject.Core.Managers.Interfaces;
using MessagesProject.ModelView;
using System.Linq;
using Tazeez.Common.Extensions;
using MessagesProject.Models;

namespace MessagesProject.Core.Managers
{
    public class CommonManager : ICommonManager
    {
        private ToDoListDataContext _taskdbContext;
        private IMapper _mapper;

        public CommonManager(ToDoListDataContext csvdbContext, IMapper mapper)
        {
            _taskdbContext = csvdbContext;
            _mapper = mapper;
        }

        public UserModel GetUserRole(UserModel user)
        {
            var dbUser = _taskdbContext.Users
                                      .FirstOrDefault(a => a.Id == user.Id)
                                      ?? throw new ServiceValidationException("Invalid user id received");

            return _mapper.Map<UserModel>(dbUser);
        }

      
    }
}
