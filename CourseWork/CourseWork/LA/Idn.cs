using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.LA
{
    public class Idn
    {
        public int Kod;
        public string Token;
        public string Type;

        public override bool Equals(object obj)
        {
            return ((Idn)obj).Token == this.Token;
        }
    }
}
