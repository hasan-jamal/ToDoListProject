using CsvWorker.ModelViews.ModelView;
using MessagesProject.ModelView;
using MessagesProject.ModelViews.ModelView;
using System.Collections.Generic;
using Tazeez.ModelViews.Request;

namespace CsvWorker.Core.Managers.Interfaces
{
    public interface ITaskManager
    {
        TaskResponse AllUserTasks(UserModel currentUser, int page = 1, int pageSize = 10,
            string sortColumn = "", string sortDirection = "ascending", string searchText = "");

        TaskModelView CreateTask(UserModel currentUser, CreateTaskMV TaskModelView);

        TaskModelView UpdateTask(UserModel currentUser, UpdateTaskMV updateTaskMV);

        void ArchiveTask(UserModel currentUser,int id);
        List<TaskModelView> GetTasksIsRead(UserModel currentUser);
        List<TaskModelView> GetTasksIsNotRead(UserModel currentUser);
    }
}
