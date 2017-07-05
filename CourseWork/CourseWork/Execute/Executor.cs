using CourseWork.LA;
using CourseWork.Poliz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Execute
{
    public class Executor
    {
        private readonly List<WorkItem> _Poliz;
        private readonly List<PolizLabel> _PolizLabels;
        private readonly List<PolizCell> _PolizCells;
        public List<Identify> _IdentifyTable;
        public Stack<WorkItem> _Stack;
        public List<Table_7> table;
        public string _Status;
        public int _Position;
        public object _OutParam;
        public Executor(List<WorkItem> poliz, List<PolizLabel> polizLabel, List<PolizCell> polizCells, List<Idn> identifyTable)
        {
            _Poliz = poliz;
            _PolizLabels = polizLabel;
            _PolizCells = polizCells;
            _Status = "";
            _Position = 0;
            _Stack = new Stack<WorkItem>();
            table = new List<Table_7>();
            _IdentifyTable = new List<Identify>();
            for (int i = 0; i < identifyTable.Count; i++)
                _IdentifyTable.Add(new Identify(identifyTable[i].Token, identifyTable[i].Type, null));
        }
        public void GoNextStep()
        {
            if (_Position != _Poliz.Count)
            {
                table.Add(new Table_7(_Poliz[_Position].Token, GetStack(), GetPoliz()));
            }
            _Status = "";
            if (_Position > _Poliz.Count - 1)
            {
                _Status = "Successful Done";
                return;
            }
            if (_Poliz[_Position].Type == "label" && _Poliz[_Position].Token[_Poliz[_Position].Token.Length - 1] == ':')
            {
                _Position++;
            }
            else if (_Poliz[_Position].Type == "idn" || _Poliz[_Position].Type == "con" || _Poliz[_Position].Type == "label" || _Poliz[_Position].Type == "cell")
            {
                _Stack.Push(_Poliz[_Position]);
                _Position++;
            }
            else if (_Poliz[_Position].Type == "operation")
            {
                if (_Poliz[_Position].Token == ">>")
                {
                    _Status = "Input ";
                    if (_Stack.Count > 0)
                    {
                        string idn = _Stack.Peek().Token;
                        var obj = _IdentifyTable.Find(o => o.Token == idn);
                        if (obj != null)
                        {
                            _Status += obj.Type;
                            return;
                        }
                        return;
                    }
                }
                else if (_Poliz[_Position].Token == "<<")
                {
                    _Status = "Output ";
                    if (_Stack.Count > 0)
                    {
                        string idn = _Stack.Peek().Token;
                        var obj = _IdentifyTable.Find(o => o.Token == idn);
                        if (obj != null)
                        {
                            _Status += obj.Type;
                            _OutParam = obj.Value;
                            return;
                        }
                        return;
                    }
                }
                else if (_Poliz[_Position].Token == "+" || _Poliz[_Position].Token == "/" || _Poliz[_Position].Token == "*" || _Poliz[_Position].Token == "-" || _Poliz[_Position].Token == "^")
                {
                    if (_Stack.Count > 1)
                    {
                        var item1 = _Stack.Pop();
                        var item2 = _Stack.Pop();

                        double val2 = GetItemValue(item1);
                        double val1 = GetItemValue(item2);
                        string operation = _Poliz[_Position].Token;
                        double res = 0;
                        if (operation == "+")
                            res = val1 + val2;
                        else if (operation == "-")
                            res = val1 - val2;
                        else if (operation == "*")
                            res = val1 * val2;
                        else if (operation == "/")
                            res = val1 / val2;
                        else if (operation == "^")
                            res = (int)Math.Pow(val1, val2);
                        _Stack.Push(new WorkItem(res.ToString(), "con"));
                        _Position++;
                    }
                    else
                        throw new InvalidOperationException();
                }
                else if (_Poliz[_Position].Token == "@")
                {
                    var item1 = _Stack.Pop();
                    double val1 = GetItemValue(item1);
                    double res = -val1;
                    _Stack.Push(new WorkItem(res.ToString(), "con"));
                    _Position++;
                }
                else if (_Poliz[_Position].Token == ">" || _Poliz[_Position].Token == "<" || _Poliz[_Position].Token == "<=" || _Poliz[_Position].Token == ">=" || _Poliz[_Position].Token == "==" || _Poliz[_Position].Token == "!=")
                {
                    var item1 = _Stack.Pop();
                    var item2 = _Stack.Pop();

                    double val2 = GetItemValue(item1);
                    double val1 = GetItemValue(item2);

                    string operation = _Poliz[_Position].Token;

                    bool res = false;

                    if (operation == ">")
                        res = val1 > val2;
                    else if (operation == "<")
                        res = val1 < val2;
                    else if (operation == "<=")
                        res = val1 <= val2;
                    else if (operation == ">=")
                        res = val1 >= val2;
                    else if (operation == "==")
                        res = val1 == val2;
                    else if (operation == "!=")
                        res = val1 != val2;
                    _Stack.Push(new WorkItem(res.ToString(), "con"));
                    _Position++;
                }
                else if (_Poliz[_Position].Token == "=")
                {
                    var item1 = _Stack.Pop();
                    var item2 = _Stack.Pop();
                    double val = GetItemValue(item1);
                    Identify obj;
                    PolizCell ob;
                    if (item2.Type == "idn")
                    {
                        obj = _IdentifyTable.Find(o => o.Token == item2.Token);
                        if (obj.Type == "int")
                        {
                            int res = (int)val;
                            obj.Value = res;
                        }
                        else
                            throw new InvalidOperationException();
                    }
                    else if (item2.Type == "cell")
                    {
                        ob = _PolizCells.Find(o => o.Cell == item2.Token);
                        int res = (int)val;
                        ob.Position = res;
                    }
                    _Position++;
                }
                else if (_Poliz[_Position].Token == "УПХ")
                {
                    var item1 = _Stack.Pop();
                    var item2 = _Stack.Pop();

                    if (item2.Token == "False")
                    {
                        var obj = _PolizLabels.Find(o => o.Label == item1.Token);
                        _Position = obj.Position;
                    }
                    else
                        _Position++;
                }
                else if (_Poliz[_Position].Token == "БП")
                {
                    var item1 = _Stack.Pop();
                    var obj = _PolizLabels.Find(o => o.Label == item1.Token);
                    _Position = obj.Position;
                }
                else if (_Poliz[_Position].Token == "and")
                {
                    var item1 = _Stack.Pop();
                    var item2 = _Stack.Pop();
                    if (item1.Token == "True" && item2.Token == "True")
                        _Stack.Push(new WorkItem("True", "con"));
                    else
                        _Stack.Push(new WorkItem("False", "con"));

                    _Position++;
                }
                else if (_Poliz[_Position].Token == "or")
                {
                    var item1 = _Stack.Pop();
                    var item2 = _Stack.Pop();
                    if (item1.Token == "True" || item2.Token == "True")
                        _Stack.Push(new WorkItem("True", "con"));
                    else
                        _Stack.Push(new WorkItem("False", "con"));
                    _Position++;
                }
                else if (_Poliz[_Position].Token == "not")
                {
                    var item1 = _Stack.Pop();
                    if (item1.Token == "True")
                        _Stack.Push(new WorkItem("False", "con"));
                    else if (item1.Token == "False")
                        _Stack.Push(new WorkItem("True", "con"));
                    else
                        throw new InvalidOperationException("Not, can be used with logical value");
                    _Position++;
                }
                else
                {
                    throw new InvalidOperationException("undefiend operation");
                }
            }
        }
        private string GetStack()
        {
            string result = "";
            for (int i = 0; i < _Stack.Count; i++)
                result += _Stack.ElementAt(i).Token + " ";
            return result;
        }
        private string GetPoliz()
        {
            string result = "";
            for (int i = 0; i < _Poliz.Count; i++)
                result += _Poliz.ElementAt(i).Token + " ";
            return result;
        }
        private double GetItemValue(WorkItem item)
        {
            double result = 0;
            if (item.Type == "con")
            {
                if (Double.TryParse(item.Token, out result))
                    return result;
                else
                    throw new ArgumentException();
            }
            else if (item.Type == "idn")
            {
                var idn = _IdentifyTable.Find(o => o.Token == item.Token);
                if (idn.Type == "int")
                    if (idn.Value != null)
                        result = (int)idn.Value;
                    else
                        throw new NullReferenceException("The identify " + idn.Token + " didn't initialized");
                else
                    throw new ArgumentException();
            }
            else if (item.Type == "cell")
            {
                var cell = _PolizCells.Find(o => o.Cell == item.Token);
                result = (int)cell.Position;
            }
            else
                throw new ArgumentException();
            return result;
        }
    }
}
