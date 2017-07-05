using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Poliz
{
    public class WorkItem
    {
        public string Token;
        public string Type;

        public WorkItem(string token, string type)
        {
            Type = type;
            Token = token;
        }
    }
}
