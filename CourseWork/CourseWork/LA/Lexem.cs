using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.LA
{
    public class Lexem
    {
        public int Number;
        public int Line;
        public string Token;
        public int Kod;
        public int Kod_idn_con;

        public override bool Equals(object obj)
        {
            return ((Lexem)obj).Token == this.Token;
        }
    }
}
