using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Execute
{
    public class Identify
    {
        public string Token { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }
        public Identify() { }
        public Identify(string token, string type, object value)
        {
            Token = token;
            Type = type;
            Value = value;
        }
    }
}
