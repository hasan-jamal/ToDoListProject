using AutoMapper;
using MessagesProject.ModelView;
using MessagesProject.Models;
using CsvWorker.ModelViews.ModelView;
using MessagesProject.ModelViews.ModelView;
using CSVWorker.Common.Extensions;

namespace MessagesProject.Mapper
{
    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<User, LoginUserResponse>().ReverseMap();
            CreateMap<UserResult, User>().ReverseMap();
            CreateMap<UserModel, User>().ReverseMap();
            CreateMap<TaskModelView, Task>().ReverseMap();
            CreateMap<CreateTaskMV, Task>().ReverseMap();
            CreateMap<PagedResult<TaskModelView>, PagedResult<Task>>().ReverseMap();



        }
    }
}
