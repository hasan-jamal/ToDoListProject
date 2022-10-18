using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessagesProject.ModelViews.ModelView
{
    public class CreateTaskMV
    {
        public string Title { get; set; }
        public string Content { get; set; }

        public string ImageString { get; set; }
        public int AssignedTo { get; set; }
    }
}
