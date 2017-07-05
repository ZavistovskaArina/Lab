using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Poliz
{
    public class PriorityTable
    {
        public string Token { get; }
        public int Priority { get; }

        public PriorityTable(string token, int priority)
        {
            Token = token;
            Priority = priority;
        }
    }
}
