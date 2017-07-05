using CourseWork.LA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseWork.SA
{
    public class SyntaticAnalyzer
    {
        private static List<Lexem> tableLex;
        private int lexNumber;

        public SyntaticAnalyzer(List<Lexem> a)
        {
            tableLex = a;
            lexNumber = 0;
            program();
        }
        private bool outOfRange()
        {
            return lexNumber < tableLex.Count;
        }

        bool program()
        {
            if (outOfRange() && tableLex[lexNumber].Kod == 1)//program
            {
                lexNumber++;
                if (outOfRange() && tableLex[lexNumber].Kod == 38)//IDN
                {
                    lexNumber++;
                    if (SpOg())//список оглошень
                    {
                        if (outOfRange() && tableLex[lexNumber].Kod == 16)//{
                        {
                            lexNumber++;
                            if (SpOp())//список операторів
                            {
                                if (outOfRange() && tableLex[lexNumber].Kod == 17)//}
                                {
                                    lexNumber++;
                                    Console.WriteLine("Помилок немає");
                                    return true;
                                }
                                else
                                {
                                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": відсутня закриваюча дужка }");
                                    MessageBox.Show("Відсутня закриваюча дужка " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                    return false;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний список операторів");
                                return false;
                                MessageBox.Show("Невірний список операторів " + " рядок: " + (tableLex[lexNumber].Line - 1));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Відсутня відкриваюча дужка " + " рядок: " + (tableLex[lexNumber].Line - 1));
                            Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": відсутня відкриваюча дужка {");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Невірний список оголошень " + " рядок: " + (tableLex[lexNumber].Line - 1));
                        Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний список оголошень");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Немає назви програми " + " рядок: " + (tableLex[lexNumber].Line - 1));
                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає назви програми");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Програма має починатися зі слова 'program' " + " рядок: ");
                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": Програма має починатися зі слова 'program'");
                return false;
            }
        }

        bool SpOg()
        {
            if (outOfRange() && tableLex[lexNumber].Kod == 2)//int
            {
                lexNumber++;
                if (SpIdn())
                {
                    if (outOfRange() && tableLex[lexNumber].Kod == 22)//;
                    {
                        lexNumber++;
                        while (outOfRange() && tableLex[lexNumber].Kod != 16) //{
                        {
                            if (outOfRange() && tableLex[lexNumber].Kod == 2)//int
                            {
                                lexNumber++;
                                if (SpIdn() == false)
                                {
                                    MessageBox.Show("Немає списку ідентифікаторів " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає списку ідентифікаторів");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Відсутній тип " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": відсутній тип");
                                return false;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Відсутня ; " + " рядок: " + (tableLex[lexNumber].Line - 1));
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Немає списку ідентифікаторів " + " рядок: " + (tableLex[lexNumber].Line - 1));
                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає списку ідентифікаторів");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Відсутній тип " + " рядок: " + (tableLex[lexNumber].Line - 1));
                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": відсутній тип");
                return false;
            }
        }

        bool SpIdn()
        {
            if (outOfRange() && tableLex[lexNumber].Kod == 38)//idn
            {
                lexNumber++;
                while (outOfRange() && tableLex[lexNumber].Kod == 23)//,
                {
                    lexNumber++;
                    if (outOfRange() && tableLex[lexNumber].Kod == 38)//idn
                    {
                        lexNumber++;
                        Console.WriteLine("Помилок немає");
                    }
                    else
                    {
                        MessageBox.Show("Немає ідентифікатора " + " рядок: " + (tableLex[lexNumber].Line - 1));
                        Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає ідентифікатора");
                        return false;
                    }
                }
                return true;
            }
            else
            {
                MessageBox.Show("Немає ідентифікатора " + " рядок: " + (tableLex[lexNumber].Line - 1));
                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає ідентифікатора");
                return false;
            }
        }

        bool SpOp()
        {
            if (Op())
            {
                if (outOfRange() && tableLex[lexNumber].Kod == 22)//;
                {
                    lexNumber++;
                    while (outOfRange() && tableLex[lexNumber].Kod != 17 && tableLex[lexNumber].Kod != 12 && tableLex[lexNumber].Kod != 6 && tableLex[lexNumber].Kod != 7)//}
                    {
                        if (Op())
                        {
                            if (outOfRange() && tableLex[lexNumber].Kod == 22)//;
                            {
                                lexNumber++;
                            }
                            else
                            {
                                MessageBox.Show("Немає ; " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає ;");
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Невірний оператор " + " рядок: " + (tableLex[lexNumber].Line - 1));
                            Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний оператор");
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("Немає ; " + " рядок: " + (tableLex[lexNumber].Line - 1));
                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає ;");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Невірний оператор " + " рядок: " + (tableLex[lexNumber].Line - 1));
                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний оператор");
                return false;
            }
        }

        bool Op()
        {
            if (outOfRange() && tableLex[lexNumber].Kod == 4)//cin
            {
                lexNumber++;
                if (outOfRange() && tableLex[lexNumber].Kod == 32)//>>
                {
                    lexNumber++;
                    if (outOfRange() &&
                        tableLex[lexNumber].Kod == 38)//idn
                    {
                        lexNumber++;
                        while (outOfRange() && tableLex[lexNumber].Kod == 32)//>>
                        {
                            lexNumber++;
                            if (outOfRange() && tableLex[lexNumber].Kod == 38)//idn
                            {
                                lexNumber++;
                            }
                            else
                            {
                                MessageBox.Show("Немає idn " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає idn");
                                return false;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Немає idn " + " рядок: " + (tableLex[lexNumber].Line - 1));
                        Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає idn");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Немає >> " + " рядок: " + (tableLex[lexNumber].Line - 1));
                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає >>");
                    return false;
                }
            }
            else
            {
                if (outOfRange() && tableLex[lexNumber].Kod == 3)//cout
                {
                    lexNumber++;
                    if (outOfRange() && tableLex[lexNumber].Kod == 33)//<<
                    {
                        lexNumber++;
                        if (outOfRange() && (tableLex[lexNumber].Kod == 38 || tableLex[lexNumber].Kod == 39))//idn con
                        {
                            lexNumber++;
                            while (outOfRange() && tableLex[lexNumber].Kod == 33)//<<
                            {
                                lexNumber++;
                                if (outOfRange() && (tableLex[lexNumber].Kod == 38 || tableLex[lexNumber].Kod == 39))//idn con
                                {
                                    lexNumber++;
                                }
                                else
                                {
                                    MessageBox.Show("Немає idn|con " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає idn|con");
                                    return false;
                                }
                            }
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Немає idn|con " + " рядок: " + (tableLex[lexNumber].Line - 1));
                            Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає idn|con");
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Немає << " + " рядок: " + (tableLex[lexNumber].Line - 1));
                        Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає <<");
                        return false;
                    }
                }
                else
                {
                    if (outOfRange() && tableLex[lexNumber].Kod == 38)//idn
                    {
                        lexNumber++;
                        if (outOfRange() && tableLex[lexNumber].Kod == 29)//=
                        {
                            lexNumber++;
                            if (Exp())
                            {
                                return true;
                            }
                            else
                            {
                                MessageBox.Show("Невірний вираз " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний вираз");
                                return false;
                            }
                        }
                        else
                        {
                            MessageBox.Show("Немає = " + " рядок: " + (tableLex[lexNumber].Line - 1));
                            Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає =");
                            return false;
                        }
                    }
                    else
                    {
                        if (outOfRange() && tableLex[lexNumber].Kod == 8)//for
                        {
                            lexNumber++;
                            if (outOfRange() && tableLex[lexNumber].Kod == 38)//idn
                            {
                                lexNumber++;
                                if (outOfRange() && tableLex[lexNumber].Kod == 29)//=
                                {
                                    lexNumber++;
                                    if (Exp())
                                    {
                                        if (outOfRange() && tableLex[lexNumber].Kod == 9)//to
                                        {
                                            lexNumber++;
                                            if (Exp())
                                            {
                                                if (outOfRange() && tableLex[lexNumber].Kod == 10)//step
                                                {
                                                    lexNumber++;
                                                    if (Exp())
                                                    {
                                                        if (outOfRange() && tableLex[lexNumber].Kod == 11)//do
                                                        {
                                                            lexNumber++;
                                                            if (SpOp())
                                                            {
                                                                if (outOfRange() && tableLex[lexNumber].Kod == 12)//next
                                                                {
                                                                    lexNumber++;
                                                                    return true;
                                                                }
                                                                else
                                                                {
                                                                    MessageBox.Show("Немає next " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                                                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає next");
                                                                    return false;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                MessageBox.Show("Немає списку операторів " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                                                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає списку операторів");
                                                                return false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("Немає do " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                                            Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає do");
                                                            return false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Немає виразу " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                                        Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає виразу");
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Немає step " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає step");
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Немає виразу " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає виразу");
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Немає to " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                            Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає to");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Немає виразу " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                        Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає виразу");
                                        return false;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Немає = " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає =");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Немає ідентифікатора " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає ідентифікатора");
                                return false;
                            }
                        }
                        else
                        {
                            if (outOfRange() && tableLex[lexNumber].Kod == 5)//if
                            {
                                lexNumber++;
                                if (LogExp())
                                {
                                    if (outOfRange() && tableLex[lexNumber].Kod == 22)//;
                                    {
                                        lexNumber++;
                                        if (SpOp())
                                        {
                                            if (outOfRange() && tableLex[lexNumber].Kod == 6)//else
                                            {
                                                lexNumber++;
                                                if (SpOp())
                                                {
                                                    if (outOfRange() && tableLex[lexNumber].Kod == 7)//endif
                                                    {
                                                        lexNumber++;
                                                        return true;
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Немає endif " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                                        Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає endif");
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Невірний список операторів " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний список операторів");
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Немає else " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає else");
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Невірний список операторів " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                            Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний список операторів");
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Немає ; " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                        Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає ;");
                                        return false;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Невірний логічний вираз " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний логічний вираз");
                                    return false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Невірний оператор " + " рядок: " + (tableLex[lexNumber].Line - 1));
                                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний оператор");
                                return false;
                            }
                        }
                    }
                }
            }
        }

        bool Exp()
        {
            if (Term())
            {
                while (outOfRange() && (tableLex[lexNumber].Kod == 24 || tableLex[lexNumber].Kod == 25))//+ -
                {
                    lexNumber++;
                    Term();
                }
                return true;
            }
            else
            {
                MessageBox.Show("Невірний перший доданок " + " рядок: " + (tableLex[lexNumber].Line - 1));
                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний перший доданок");
                return false;
            }
        }

        bool Term()
        {
            if (Power())
            {
                while (outOfRange() && (tableLex[lexNumber].Kod == 26 || tableLex[lexNumber].Kod == 27))// * /
                {
                    lexNumber++;
                    Power();
                }
                return true;
            }
            else
            {
                MessageBox.Show("Невірний перший множник " + " рядок: " + (tableLex[lexNumber].Line - 1));
                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний перший множник");
                return false;

            }
        }

        bool Power()
        {
            if (Operand())
            {
                if (outOfRange() && tableLex[lexNumber].Kod == 28)//^
                {
                    lexNumber++;
                    Power();
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Невірний операнд " + " рядок: " + (tableLex[lexNumber].Line - 1));
                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний операнд");
                return false;
            }
        }

        bool Operand()
        {
            if (outOfRange() && (tableLex[lexNumber].Kod == 38 || tableLex[lexNumber].Kod == 39))//idn con
            {
                lexNumber++;
                return true;
            }
            else
            {
                if (outOfRange() && tableLex[lexNumber].Kod == 18)//(
                {
                    lexNumber++;
                    Exp();
                    if (outOfRange() && tableLex[lexNumber].Kod == 19)//)
                    {
                        lexNumber++;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Невірний ) " + " рядок: " + (tableLex[lexNumber].Line - 1));
                        Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний )");
                        return false;
                    }

                }
                else
                {
                    if (outOfRange() && tableLex[lexNumber].Kod == 25)//-
                    {
                        lexNumber++;
                        if (Operand())
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show("Невірний операнд " + " рядок: " + (tableLex[lexNumber].Line - 1));
                            Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний операнд");
                            return false;
                        }
                    }
                }
                MessageBox.Show("Невірний операнд " + " рядок: " + (tableLex[lexNumber].Line - 1));
                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний операнд");
                return false;
            }
        }

        bool LogExp()
        {
            if (LogTerm())
            {
                while (outOfRange() && tableLex[lexNumber].Kod == 13)//or 
                {
                    lexNumber++;
                    LogTerm();
                }
                return true;
            }
            else
            {
                MessageBox.Show("Невірний перший логічний терм " + " рядок: " + (tableLex[lexNumber].Line - 1));
                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний перший логічний терм");
                return false;
            }
        }

        bool LogTerm()
        {
            if (LogFactor())
            {
                while (outOfRange() && tableLex[lexNumber].Kod == 14)//and
                {
                    lexNumber++;
                    LogFactor();
                }
                return true;
            }
            else
            {
                MessageBox.Show("Невірний перший логічний множник " + " рядок: " + (tableLex[lexNumber].Line - 1));
                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний перший логічний множник");
                return false;
            }
        }

        bool LogFactor()
        {
            if (outOfRange() && tableLex[lexNumber].Kod == 20)//[
            {
                lexNumber++;
                LogExp();
                if (outOfRange() && tableLex[lexNumber].Kod == 21)//]
                {
                    lexNumber++;
                    return true;
                }
                else
                {
                    MessageBox.Show("Немає ] " + " рядок: " + (tableLex[lexNumber].Line - 1));
                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": немає ]");
                    return false;
                }
            }
            else
            {
                if (outOfRange() && tableLex[lexNumber].Kod == 15)//not
                {
                    lexNumber++;
                    if (LogFactor())
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Невірний логічний множник " + " рядок: " + (tableLex[lexNumber].Line - 1));
                        Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний логічний множник");
                        return false;
                    }
                }
                else
                if (Relat())
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Невірний логічний множник " + " рядок: " + (tableLex[lexNumber].Line - 1));
                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний логічний множник");
                    return false;
                }
            }

        }

        bool Relat()
        {
            if (Exp())
            {
                if (RelatSign())
                {
                    if (Exp())
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Невірний вираз " + " рядок: " + (tableLex[lexNumber].Line - 1));
                        Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний вираз");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Невірний знак відношення " + " рядок: " + (tableLex[lexNumber].Line - 1));
                    Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний знак відношення");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Невірний вираз " + " рядок: " + (tableLex[lexNumber].Line - 1));
                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний вираз");
                return false;
            }
        }

        bool RelatSign()
        {
            if (outOfRange() && (tableLex[lexNumber].Kod == 31 || tableLex[lexNumber].Kod == 30 || tableLex[lexNumber].Kod == 34
                || tableLex[lexNumber].Kod == 35 || tableLex[lexNumber].Kod == 36 || tableLex[lexNumber].Kod == 37))
            {
                lexNumber++;
                return true;
            }
            else
            {
                MessageBox.Show("Невірний знак відношення " + " рядок: " + (tableLex[lexNumber].Line - 1));
                Console.WriteLine("Рядок " + (tableLex[lexNumber].Line - 1) + ": невірний знак відношення");
                return false;
            }
        }
    }
}
