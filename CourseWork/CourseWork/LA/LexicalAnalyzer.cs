using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.LA
{
    public class LexicalAnalyzer
    {
        public static List<string> constlexems;
        public List<Lexem> tableLex = new List<Lexem>();
        public List<Idn> tableIdn = new List<Idn>();
        public List<Const> tableCon = new List<Const>();
        public static bool toread;
        public static char ch;
        public static char[] str;
        public static string lex;
        public static int line;
        public static int i;
        public static int column;
        public static int k;
        public static int lexemeCounter;
        public static int testCount;
        public LexicalAnalyzer(char[] text, int _column, bool _toread, List<string> constLexems)
        {
            lex = "";
            line = 1;
            i = 0;
            column = 1;
            k = 0;
            lexemeCounter = 0;
            testCount = 0;
            str = text;
            column = _column;
            toread = _toread;
            constlexems = new List<string>(constLexems);
            tableLex.Clear();
            tableIdn.Clear();
            tableCon.Clear();
        }
        private static char getChar()
        {
            if (i < str.Length)
            {
                column++;
                return str[i++];
            }
            else
            {
                return (char)0;
            }
        }
        public void State1()
        {
            while (k < str.Length)
            {
                if (toread)
                {
                    ch = getChar();
                }
                else toread = true;
                if (ch == ' ')
                {
                    State1();
                }
                else if (ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z')
                {
                    lex = "" + ch;
                    State2();
                }
                else if (ch >= '0' && ch <= '9')
                {
                    lex = "" + ch;
                    State3();
                }
                else if (ch == '<')
                {
                    lex = "" + ch;
                    State4();
                }
                else if (ch == '>')
                {
                    lex = "" + ch;
                    State5();
                }
                else if (ch == '=')
                {
                    lex = "" + ch;
                    State6();
                }
                else if (ch == '!')
                {
                    lex = "" + ch;
                    State7();
                }
                else
                if (ch == '\t' || ch == '\r' || ch == ';' || ch == ',' || ch == '{' || ch == '}' || ch == '(' || ch == ')' || ch == '[' || ch == ']' || ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '^')
                {
                    lex = "" + ch;

                    if (ch == '\r')
                    {
                        line++;
                    }
                    else
                    {
                        AddLex(lex, constlexems.IndexOf(lex));
                    }
                    toread = true;
                    State1();
                }
                else
                {
                    if (ch > 33 && ch < 40 || ch == 46 || ch == 58 || ch == 63 || ch == 64 || ch == 92 || ch == 96 || ch == 124 || ch == 126)
                    {
                        lex = "" + ch;
                        throw new Exception("Немає такої лексеми: " + lex + "   рядок: " + line);
                    }
                }
                k++;
            }
        }
        private void State2()
        {
            ch = getChar();
            if (ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z' || ch == '_')
            {
                lex += ch;
                State2();
            }
            else if (ch >= '0' && ch <= '9')
            {
                lex += ch;
                State2();
            }
            else if (ch == 0)
                toread = false;
            else
            {
                toread = false;
                AddLex(lex, 38);
            }
        }
        private void State3()
        {
            ch = getChar();
            if (ch >= '0' && ch <= '9')
            {
                lex += ch;
                State3();
            }
            else
            {
                toread = false;
                AddLex(lex, 39);
            }
        }
        private void State4()
        {
            ch = getChar();
            if (ch == '<')
            {
                lex += ch;
                AddLex(lex, 33);
            }
            else if (ch == '=')
            {
                lex += ch;
                AddLex(lex, 34);
            }
            else if (ch == 0)
                toread = false;
            else
            {
                toread = false;
                AddLex(lex, 30);
            }
        }
        private void State5()
        {
            ch = getChar();
            if (ch == '>')
            {
                lex += ch;
                AddLex(lex, 32);
            }
            else if (ch == '=')
            {
                lex += ch;
                AddLex(lex, 35);
            }
            else if (ch == 0)
                toread = false;
            else
            {
                toread = false;
                AddLex(lex, 31);
            }
        }
        private void State6()
        {
            if (toread)
            {
                ch = getChar();
            }
            else toread = true;

            if (ch == '=')
            {
                lex += ch;
                AddLex(lex, 36);
            }
            else if (ch == 0)
                toread = false;
            else
            {
                toread = false;
                AddLex(lex, 29);
            }
        }
        private void State7()
        {
            ch = getChar();
            if (ch == '=')
            {
                lex += ch;
                AddLex(lex, 37);
            }
            else if (ch == 0)
                toread = false;
            else
            {
                throw new Exception("Неправильний роздільник: " + lex + "   рядок: " + line);
            }
        }
        private void AddLex(String lex, int code)
        {
            Lexem lexStruct = new Lexem();
            Const constStruct = new Const();
            Idn idnStruct = new Idn();
            if (code == 38)
            {
                MakeIdn(lex, lexStruct, idnStruct);
            }
            else if (code == 39)
            {
                MakeConst(lex, lexStruct, constStruct);
            }
            else
            {
                lexStruct.Token = lex;
                lexStruct.Line = line;
                lexStruct.Kod = code;
            }
            lexStruct.Number = lexemeCounter;
            lexemeCounter++;
            tableLex.Add(lexStruct);
            testCount++;
            State1();
        }
        private void MakeConst(string lex, Lexem lexStruct, Const constStruct)
        {
            constStruct.Con = lex;

            if (tableCon.IndexOf(constStruct) == -1)
            {
                tableCon.Add(constStruct);
            }
            constStruct.Kod = tableCon.IndexOf(constStruct);
            lexStruct.Token = lex;
            lexStruct.Line = line;
            lexStruct.Kod = 39;
            lexStruct.Kod_idn_con = tableCon.IndexOf(constStruct) + 1;
        }
        private void MakeIdn(string lex, Lexem lexStruct, Idn idnStruct)
        {
            if (constlexems.IndexOf(lex) != -1)
            {
                lexStruct.Kod = constlexems.IndexOf(lex);
                lexStruct.Token = constlexems[lexStruct.Kod];
                lexStruct.Line = line;
            }
            else
            {
                idnStruct.Token = lex;
                if (tableLex[testCount - 1].Token.Equals("program"))
                {
                    idnStruct.Type = "program";
                }
                if (tableLex[testCount - 1].Token.Equals("int") || tableLex[testCount - 1].Token.Equals(","))
                {
                    if (tableIdn.Contains(idnStruct))
                    {
                        throw new Exception("Дублювання лексеми: " + lex + "   рядок: " + line);
                    }
                    idnStruct.Type = "int";
                }
                else
                {
                    if (!tableIdn.Contains(idnStruct) && !tableLex[testCount - 1].Token.Equals("program"))
                    {
                        throw new Exception("Необ'явлений ідентифікатор: " + lex + "   рядок: " + line);
                    }
                }
                if (tableIdn.IndexOf(idnStruct) == -1)
                {
                    tableIdn.Add(idnStruct);
                }
                idnStruct.Kod = tableIdn.IndexOf(idnStruct);
                lexStruct.Token = lex;
                lexStruct.Line = line;
                lexStruct.Kod = 38;
                lexStruct.Kod_idn_con = tableIdn.IndexOf(idnStruct) + 1;
            }
        }
        private static Boolean isConstant(String lexeme)
        {
            Boolean result = false;
            for (int j = 0; j < lexeme.Count(); j++)
            {
                if (Char.IsDigit(lexeme[j]))
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }
        private static Boolean isIdn(String lexeme)
        {
            Boolean result = false;
            if (lexeme.Count() != 0)
            {
                if (lexeme[0] > 64 && lexeme[0] < 91 || lexeme[0] > 96 && lexeme[0] < 123 || lexeme[0] == 95)
                {
                    result = true;
                    for (int j = 1; j < lexeme.Count(); j++)
                    {
                        if (lexeme[j] > 64 && lexeme[j] < 91 || lexeme[j] > 96 && lexeme[j] < 123 || lexeme[j] > 47 && lexeme[j] < 58 || lexeme[j] == 95)
                            result = true;
                    }
                }
                else result = false;
            }
            return result;
        }
    }
}
