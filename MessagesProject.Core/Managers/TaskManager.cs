using AutoMapper;
using CsvWorker.Core.Managers.Interfaces;
using CsvWorker.ModelViews.ModelView;
using CSVWorker.Common.Extensions;
using CSVWorker.Helper;
using MessagesProject.Models;
using MessagesProject.ModelView;
using MessagesProject.ModelViews.ModelView;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Tazeez.Common.Extensions;

namespace CsvWorker.Core.Managers
{
    public class TaskManager : ITaskManager
    {
        private ToDoListDataContext _taskdbContext;
        private IMapper _mapper;

        public TaskManager(ToDoListDataContext csvdbContext, IMapper mapper)
        {
            _taskdbContext = csvdbContext;
            _mapper = mapper;
        }

        public void ArchiveTask(UserModel currentUser, int id)
        {
            if (!currentUser.IsAdmin) throw new ServiceValidationException(401, "You have no permission to delete an Task!");

            var data = _taskdbContext.Tasks.FirstOrDefault(b => b.Id == id)
              ?? throw new ServiceValidationException(403, "There is no ID for this task");

            data.Archived = true;
            _taskdbContext.SaveChanges();
        }

        public TaskModelView CreateTask(UserModel currentUser, CreateTaskMV taskModelView)
        {
            var newTask = _mapper.Map<Task>(taskModelView);
            newTask.AssignedBy = currentUser.Id;
            if (currentUser.IsAdmin)
            {
                newTask.AssignedTo = taskModelView.AssignedTo;
            }
            else
            {
                newTask.AssignedTo = currentUser.Id;
            }
            var url = "";
            if (!string.IsNullOrWhiteSpace(taskModelView.ImageString))
            {
                url = Helper.SaveImage(taskModelView.ImageString, "TasksImages");
            }

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseURL = "https://localhost:44335/";
                newTask.ImageUrl = @$"{baseURL}/api/v1/toDo/retrieve?filename={url}";
            }

            var user = _taskdbContext.Tasks.Add(newTask).Entity;
            _taskdbContext.SaveChanges();

            var result = _mapper.Map<TaskModelView>(user);
            return result;

        }


        public TaskResponse AllUserTasks(UserModel currentUser, int page = 1, int pageSize = 10,string sortColumn = "", string sortDirection = "ascending", string searchText = "")
        {
            IQueryable<Task> queryRes;
            if (!currentUser.IsAdmin)
            {
                queryRes = _taskdbContext.Tasks.Where(a => (string.IsNullOrWhiteSpace(searchText)
                                                   || (a.Title.Contains(searchText)
                                                       || a.Content.Contains(searchText))) && currentUser.Id == a.AssignedTo);
                queryRes.ToList().ForEach(a => a.IsRead = true);
                _taskdbContext.SaveChanges();
            }
            else
            {
                queryRes = _taskdbContext.Tasks.Where(a => (string.IsNullOrWhiteSpace(searchText)
                                                                   || (a.Title.Contains(searchText)
                                                                       || a.Content.Contains(searchText))) && currentUser.Id == a.AssignedTo);
            }

            if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("ascending", StringComparison.InvariantCultureIgnoreCase))
            {
                queryRes = queryRes.OrderBy(sortColumn);
            }
            else if (!string.IsNullOrWhiteSpace(sortColumn) && sortDirection.Equals("descending", StringComparison.InvariantCultureIgnoreCase))
            {
                queryRes = queryRes.OrderByDescending(sortColumn);
            }

            var res = queryRes.GetPaged(page, pageSize);

            var userIds = res.Data
                             .Select(a => a.Id)
                             .Distinct()
                             .ToList();

            var users = _taskdbContext.Users
                                     .Where(a => userIds.Contains(a.Id))
                                     .ToDictionary(a => a.Id, x => _mapper.Map<UserResult>(x));

            var data = new TaskResponse
            {
                Task = _mapper.Map<PagedResult<TaskModelView>>(res),
                User = users
            };

            data.Task.Sortable.Add("Title", "Title");
            data.Task.Sortable.Add("CreatedDate", "Created Date");

            return data;
        }

        public TaskModelView UpdateTask(UserModel currentUser, UpdateTaskMV updateTaskMV)
        {
            var task = _taskdbContext.Tasks
                                    .Include(a => a.AssignedByNavigation)
                                    .FirstOrDefault(task => task.Id == updateTaskMV.Id
                                                    && task.AssignedBy == currentUser.Id)
                                    ?? throw new ServiceValidationException("Task not found");

            var url = "";
            if (!string.IsNullOrWhiteSpace(updateTaskMV.ImageString))
            {
                url = Helper.SaveImage(updateTaskMV.ImageString, "TasksImages");
            }

            task.Title = updateTaskMV.Title;
            task.Content = updateTaskMV.Content;
            if (task.AssignedByNavigation.IsAdmin && task.AssignedTo != updateTaskMV.AssignedTo)
            {
                task.AssignedTo = updateTaskMV.AssignedTo;
            }

            if (!string.IsNullOrWhiteSpace(url))
            {
                var baseURL = "https://localhost:44335/";
                task.ImageUrl = @$"{baseURL}/api/v1/toDo/retrieve?filename={url}";
            }

            _taskdbContext.SaveChanges();
            return _mapper.Map<TaskModelView>(task);
        }

        public List<TaskModelView> GetTasksIsRead(UserModel currentUser)
        {
            IQueryable<Task> data = _taskdbContext.Tasks;   
                data = data
               .Where(a => a.AssignedTo == currentUser.Id && a.IsRead);
                data.ToList().ForEach(a => a.IsRead = true);
                _taskdbContext.SaveChanges();

            var result = _mapper.Map<List<TaskModelView>>(data);
            return result;
        }

        public List<TaskModelView> GetTasksIsNotRead(UserModel currentUser)
        {
            IQueryable<Task> data = _taskdbContext.Tasks;
            if (!currentUser.IsAdmin)
            {
                data = data
               .Where(a => a.AssignedTo == currentUser.Id && a.IsRead == false);
                data.ToList().ForEach(a => a.IsRead = true);
            }
            else
            {
                data = data
               .Where(a => a.AssignedTo == currentUser.Id && a.IsRead == false);
                data.ToList();
            }
            _taskdbContext.SaveChanges();
            var result = _mapper.Map<List<TaskModelView>>(data);
            return result;
        }

    }
}
