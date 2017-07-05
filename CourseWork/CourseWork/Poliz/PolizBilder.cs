using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseWork.Poliz
{
    public class PolizBilder
    {
        public List<WorkItem> InputItems;
        public List<PriorityTable> TablePriority;
        public Stack<WorkItem> _Stack = new Stack<WorkItem>();
        public List<WorkItem> Poliz;
        public List<PolizLabel> PolizLabels;
        public List<PolizCell> PolizCells;
        public List<Table_7> table;
        bool isLoopNow = false;
        int loopEnd = 0;
        string loop = "";
        bool ind = false;

        public PolizBilder(List<WorkItem> input)
        {
            InputItems = input;
            TablePriority = new List<PriorityTable>();
            Poliz = new List<WorkItem>();
            PolizLabels = new List<PolizLabel>();
            PolizCells = new List<PolizCell>();
            table = new List<Table_7>();
            TablePriority.Add(new PriorityTable("{", 0));
            TablePriority.Add(new PriorityTable("if", 1));
            TablePriority.Add(new PriorityTable("for", 1));
            TablePriority.Add(new PriorityTable("}", 1));
            TablePriority.Add(new PriorityTable("<<", 1));
            TablePriority.Add(new PriorityTable("cout", 1));
            TablePriority.Add(new PriorityTable("cin", 1));
            TablePriority.Add(new PriorityTable(">>", 1));
            TablePriority.Add(new PriorityTable(";", 1));
            TablePriority.Add(new PriorityTable("(", 2));
            TablePriority.Add(new PriorityTable("[", 2));
            TablePriority.Add(new PriorityTable("else", 2));
            TablePriority.Add(new PriorityTable("to", 2));
            TablePriority.Add(new PriorityTable("step", 2));
            TablePriority.Add(new PriorityTable("do", 2));
            TablePriority.Add(new PriorityTable("next", 2));
            TablePriority.Add(new PriorityTable("endif", 2));
            TablePriority.Add(new PriorityTable(")", 3));
            TablePriority.Add(new PriorityTable("]", 3));
            TablePriority.Add(new PriorityTable("or", 3));
            TablePriority.Add(new PriorityTable("=", 3));
            TablePriority.Add(new PriorityTable("and", 4));
            TablePriority.Add(new PriorityTable("not", 5));
            TablePriority.Add(new PriorityTable("<", 6));
            TablePriority.Add(new PriorityTable(">", 6));
            TablePriority.Add(new PriorityTable("<=", 6));
            TablePriority.Add(new PriorityTable(">=", 6));
            TablePriority.Add(new PriorityTable("==", 6));
            TablePriority.Add(new PriorityTable("!=", 6));
            TablePriority.Add(new PriorityTable("+", 7));
            TablePriority.Add(new PriorityTable("-", 7));
            TablePriority.Add(new PriorityTable("*", 8));
            TablePriority.Add(new PriorityTable("@", 8));
            TablePriority.Add(new PriorityTable("/", 8));
            TablePriority.Add(new PriorityTable("^", 9));
        }

        public void Builder()
        {
            while (InputItems.Count > 0 && InputItems[0].Token != "{")
            {
                InputItems.RemoveAt(0);
            }
            while (InputItems.Count > 0)
            {
                table.Add(new Table_7(GetInput(), GetStack(), GetPoliz()));
                if (InputItems[0].Type == "idn" || InputItems[0].Type == "con")
                {
                    Poliz.Add(InputItems[0]);
                    InputItems.RemoveAt(0);
                }
                else
                {
                    if (_Stack.Count == 0)
                    {
                        _Stack.Push(InputItems[0]);
                        InputItems.RemoveAt(0);
                    }
                    else
                    {
                        if (InputItems[0].Token == "for")
                        {
                            string newLabel1 = "m" + (PolizLabels.Count + 1).ToString();
                            string newLabel2 = "m" + (PolizLabels.Count + 2).ToString();
                            string newLabel3 = "m" + (PolizLabels.Count + 3).ToString();
                            PolizLabels.Add(new PolizLabel(newLabel1, PolizLabels.Count + 1));
                            PolizLabels.Add(new PolizLabel(newLabel2, PolizLabels.Count + 1));
                            PolizLabels.Add(new PolizLabel(newLabel3, PolizLabels.Count + 1));
                            _Stack.Push(new WorkItem(newLabel3, "label"));
                            _Stack.Push(new WorkItem(newLabel2, "label"));
                            _Stack.Push(new WorkItem(newLabel1, "label"));
                            _Stack.Push(InputItems[0]);
                            InputItems.RemoveAt(0);
                            isLoopNow = true;
                            loopEnd++;
                            continue;
                        }
                        else if (InputItems[0].Token == "=" && isLoopNow)
                        {
                            if (loopEnd == 1)
                            {
                                loop = Poliz[Poliz.Count - 1].Token;
                                loopEnd--;
                            }
                            _Stack.Push(InputItems[0]);
                            InputItems.RemoveAt(0);
                            continue;
                        }
                        else if (InputItems[0].Token == "to")
                        {
                            while (_Stack.Peek().Token != "for")
                            {
                                Poliz.Add(_Stack.Pop());
                            }
                            string newCell1 = "r" + (PolizCells.Count + 1).ToString();
                            string newCell2 = "r" + (PolizCells.Count + 2).ToString();
                            string newCell3 = "r" + (PolizCells.Count + 3).ToString();
                            PolizCells.Add(new PolizCell(newCell1, PolizCells.Count + 1));
                            PolizCells.Add(new PolizCell(newCell2, PolizCells.Count + 2));
                            PolizCells.Add(new PolizCell(newCell3, PolizCells.Count + 3));
                            Poliz.Add(new WorkItem(newCell1, "cell"));
                            Poliz.Add(new WorkItem("1", "con"));
                            Poliz.Add(new WorkItem("=", "operation"));
                            Poliz.Add(new WorkItem(_Stack.ElementAt(1).Token + ":", "label"));
                            Poliz.Add(new WorkItem(newCell3, "cell"));
                            InputItems.RemoveAt(0);
                            continue;
                        }
                        else if (InputItems[0].Token == "step")
                        {
                            while (_Stack.Peek().Token != "for")
                            {
                                Poliz.Add(_Stack.Pop());
                            }
                            Poliz.Add(new WorkItem("=", "operation"));
                            Poliz.Add(new WorkItem(PolizCells[PolizCells.Count - 2].Cell, "cell"));
                            InputItems.RemoveAt(0);
                            continue;
                        }
                        else if (InputItems[0].Token == "do")
                        {
                            while (_Stack.Peek().Token != "for")
                            {
                                Poliz.Add(_Stack.Pop());
                            }
                            Poliz.Add(new WorkItem("=", "operation"));
                            Poliz.Add(new WorkItem(PolizCells[PolizCells.Count - 3].Cell, "cell"));
                            Poliz.Add(new WorkItem("0", "con"));
                            Poliz.Add(new WorkItem("==", "operation"));
                            Poliz.Add(new WorkItem(_Stack.ElementAt(2).Token, "label"));
                            Poliz.Add(new WorkItem("УПХ", "operation"));
                            Poliz.Add(new WorkItem(loop, "idn"));
                            Poliz.Add(new WorkItem(loop, "idn"));
                            Poliz.Add(new WorkItem(PolizCells[PolizCells.Count - 2].Cell, "cell"));
                            Poliz.Add(new WorkItem("+", "operation"));
                            Poliz.Add(new WorkItem("=", "operation"));
                            Poliz.Add(new WorkItem(_Stack.ElementAt(2).Token + ":", "label"));
                            Poliz.Add(new WorkItem(PolizCells[PolizCells.Count - 3].Cell, "cell"));
                            Poliz.Add(new WorkItem("0", "con"));
                            Poliz.Add(new WorkItem("=", "operation"));
                            Poliz.Add(new WorkItem(loop, "idn"));
                            Poliz.Add(new WorkItem(PolizCells[PolizCells.Count - 1].Cell, "cell"));
                            Poliz.Add(new WorkItem("-", "operation"));
                            Poliz.Add(new WorkItem(PolizCells[PolizCells.Count - 2].Cell, "cell"));
                            Poliz.Add(new WorkItem("*", "operation"));
                            Poliz.Add(new WorkItem("0", "con"));
                            Poliz.Add(new WorkItem("<=", "operation"));
                            Poliz.Add(new WorkItem(_Stack.ElementAt(3).Token, "label"));
                            Poliz.Add(new WorkItem("УПХ", "operation"));
                            InputItems.RemoveAt(0);
                            continue;
                        }
                        else if (InputItems[0].Token == "cout" || InputItems[0].Token == "cin")
                        {
                            _Stack.Push(InputItems[0]);
                            InputItems.RemoveAt(0);
                            continue;
                        }
                        else if (InputItems[0].Token == "(" || InputItems[0].Token == "[")
                        {
                            _Stack.Push(InputItems[0]);
                            InputItems.RemoveAt(0);
                            continue;
                        }
                        else if (InputItems[0].Token == "next")
                        {
                            while (_Stack.Peek().Token != "for")
                            {
                                if (_Stack.Peek().Token != "cout" || _Stack.Peek().Token != "cin")
                                    Poliz.Add(_Stack.Pop());
                                else
                                    _Stack.Pop();
                            }
                            _Stack.Pop();
                            if (_Stack.Count > 1)
                            {
                                Poliz.Add(_Stack.Pop());
                                _Stack.Pop();
                                Poliz.Add(new WorkItem("БП", "operation"));
                            }
                            Poliz.Add(new WorkItem(_Stack.Pop().Token + ":", "label"));
                            InputItems.RemoveAt(0);
                        }
                        else if (InputItems[0].Token == "if")
                        {
                            ind = true;
                            _Stack.Push(InputItems[0]);
                            InputItems.RemoveAt(0);
                            continue;
                        }

                        int priority1 = GetPriority(_Stack.Peek().Token);
                        int priority2 = GetPriority(InputItems[0].Token);
                        while (priority1 >= priority2 && _Stack.Count > 0)
                        {
                            if (isLoopNow && _Stack.Peek().Token == "for")
                                break;
                            if (loopEnd > 0 && _Stack.Peek().Token == "for")
                                break;
                            if (_Stack.Peek().Token == "cout" || _Stack.Peek().Token == "cin")
                                if (InputItems[0].Token == "<<" || InputItems[0].Token == ">>")
                                {
                                    break;
                                }
                                else if (InputItems[0].Token == ";")
                                {
                                    _Stack.Pop();
                                    if (_Stack.Count > 0)
                                        priority1 = GetPriority(_Stack.Peek().Token);
                                    break;
                                }
                            if (_Stack.Peek().Token == "if")
                            {
                                break;
                            }
                            else
                            {
                                Poliz.Add(_Stack.Pop());
                                if (_Stack.Count > 0)
                                    priority1 = GetPriority(_Stack.Peek().Token);
                            }
                        }
                        if (InputItems[0].Token == ";" && ind == true)
                        {
                            while (_Stack.Peek().Token != "if")
                            {
                                Poliz.Add(_Stack.Pop());
                            }
                            string newLabel1 = "m" + (PolizLabels.Count + 1).ToString();
                            PolizLabels.Add(new PolizLabel(newLabel1, PolizLabels.Count + 1));
                            _Stack.Push(new WorkItem(newLabel1, "label"));
                            Poliz.Add(new WorkItem(newLabel1, "label"));
                            Poliz.Add(new WorkItem("УПХ", "operation"));
                            InputItems.RemoveAt(0);
                            ind = false;
                        }
                        else if (InputItems[0].Token == ";")
                        {
                            InputItems.RemoveAt(0);
                            continue;
                        }
                        else if (InputItems[0].Token == ")" || InputItems[0].Token == "]")
                        {
                            while (!(_Stack.Peek().Token == "(" || _Stack.Peek().Token == "["))
                            {
                                Poliz.Add(_Stack.Pop());
                            }
                            InputItems.RemoveAt(0);
                            _Stack.Pop();
                        }
                        else if (InputItems[0].Token == "else")
                        {
                            Stack<WorkItem> temp = new Stack<WorkItem>();
                            temp.Push(_Stack.Pop());
                            while (_Stack.Peek().Type == "label")
                                temp.Push(_Stack.Pop());
                            string newLabel2 = "m" + (PolizLabels.Count + 1).ToString();
                            PolizLabels.Add(new PolizLabel(newLabel2, PolizLabels.Count + 1));
                            _Stack.Push(new WorkItem(newLabel2, "label"));
                            Poliz.Add(new WorkItem(newLabel2, "label"));
                            Poliz.Add(new WorkItem("БП", "operation"));
                            while (_Stack.Peek().Type != "label")
                            {
                                Poliz.Add(_Stack.Pop());
                            }
                            Poliz.Add(new WorkItem(temp.ElementAt(temp.Count - 1).Token + ":", "label"));
                            InputItems.RemoveAt(0);
                        }
                        else if (InputItems[0].Token == "endif")
                        {
                            while (_Stack.Peek().Token != "if")
                            {
                                if (_Stack.Peek().Token != "cout" && _Stack.Peek().Token != "cin")
                                {
                                    if (_Stack.Peek().Type == "label")
                                        Poliz.Add(new WorkItem(_Stack.Pop().Token + ":", "label"));
                                    else
                                        Poliz.Add(_Stack.Pop());
                                }
                                else
                                    _Stack.Pop();
                            }
                            _Stack.Pop();
                            InputItems.RemoveAt(0);
                        }
                        else if (InputItems[0].Token == "}")
                        {
                            while (_Stack.Peek().Token != "{")
                            {
                                Poliz.Add(_Stack.Pop());
                            }
                            InputItems.RemoveAt(0);
                            _Stack.Pop();
                            SetPosition();
                            //return;
                        }
                        else
                        {
                            _Stack.Push(InputItems[0]);
                            InputItems.RemoveAt(0);
                        }
                    }
                }
            }
        }
        private void SetPosition()
        {
            foreach (WorkItem item in Poliz)
            {
                foreach (PolizLabel label in PolizLabels)
                {
                    if (item.Type == "label" && item.Token.Equals(label.Label + ":"))
                    {
                        label.Position = Poliz.IndexOf(item) + 1;
                    }
                }
            }
        }
        private int GetPriority(string token)
        {
            var obj = TablePriority.Find(o => o.Token == token);
            if (obj != null)
                return obj.Priority;
            else
                return -1;
        }
        public string GetPolizString()
        {
            string result = "";
            for (int i = 0; i < Poliz.Count; i++)
                result += Poliz[i].Token + " ";
            return result;
        }
        private string GetInput()
        {
            return InputItems[0].Token;
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
            for (int i = 0; i < Poliz.Count; i++)
                result += Poliz.ElementAt(i).Token + " ";
            return result;
        }
    }
}
