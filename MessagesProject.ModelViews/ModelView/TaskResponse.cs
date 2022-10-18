using CSVWorker.Common.Extensions;
using MessagesProject.ModelView;
using System.Collections.Generic;

namespace CsvWorker.ModelViews.ModelView
{
    public class TaskResponse
    {
        public PagedResult<TaskModelView> Task { get; set; }

        public  Dictionary<int, UserResult> User { get; set; }
    }
}
