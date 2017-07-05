using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.LA
{
    public class Const
    {
        public int Kod;
        public string Con;

        public override bool Equals(object obj)
        {
            return ((Const)obj).Con == this.Con;
        }
    }
}
