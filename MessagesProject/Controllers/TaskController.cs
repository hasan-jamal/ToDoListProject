using CsvWorker.Core.Managers.Interfaces;
using MessagesProject.Controllers;
using MessagesProject.ModelViews.ModelView;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CSVWorker.Controllers
{
    [ApiController]
    [Authorize]
    public class TaskController : ApiBaseController
    {
        private ITaskManager _taskManager;
        private readonly ILogger<UserController> _logger;

        public TaskController(ILogger<UserController> logger,
                              ITaskManager TaskManager)
        {
            _logger = logger;
            _taskManager = TaskManager;
        }

    

        [Route("api/Task/GetAllTasks")]
        [HttpGet]
        public IActionResult GetTasks(int page = 1, int pageSize = 5, string sortColumn = "", string sortDirection = "ascending", string searchText = "")
        {
            var result = _taskManager.AllUserTasks(LoggedInUser,page, pageSize, sortColumn, sortDirection, searchText);
            return Ok(result);
        }
        [Route("api/Task/ArchiveTask/{id}")]
        [HttpDelete]
        public IActionResult ArchiveTask(int id)
        {
            _taskManager.ArchiveTask(LoggedInUser,id);
            return Ok();
        }


        [Route("api/Task/CreateTask")]
        [HttpPost]
        public IActionResult CreateTask([FromBody] CreateTaskMV taskModelView)
        {
            var result = _taskManager.CreateTask(LoggedInUser,taskModelView);
            return Ok(result);
        }

        [Route("api/Task/UpdateTasks")]
        [HttpPut]
        public IActionResult UpdateTasks(UpdateTaskMV updateTaskMV)
        {
            var result = _taskManager.UpdateTask(LoggedInUser, updateTaskMV);
            return Ok(result);
        }
        [Route("api/Task/GetTasksIsRead")]
        [HttpGet]
        public IActionResult GetTasksIsRead()
        {
            var result = _taskManager.GetTasksIsRead(LoggedInUser);
            return Ok(result);
        }
        [Route("api/Task/GetTasksIsNotRead")]
        [HttpGet]

        public IActionResult GetTasksIsNotRead()
        {
            var result = _taskManager.GetTasksIsNotRead(LoggedInUser);
            return Ok(result);
        }

    }
}
