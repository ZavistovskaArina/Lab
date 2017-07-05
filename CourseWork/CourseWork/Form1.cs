using CourseWork.Execute;
using CourseWork.LA;
using CourseWork.Poliz;
using CourseWork.SA;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseWork
{
    public partial class Form1 : Form
    {
        public List<string> constlexems = new List<string>();
        public static Executor Execute;
        public static PolizBilder bilder;
        public static LexicalAnalyzer LA;
        public static SyntaticAnalyzer SA;
        public static List<WorkItem> InputVariables = new List<WorkItem>();
        public static List<Identify> Output = new List<Identify>();

        public static bool toread;
        public static char[] str;
        public static int count;
        public Form1()
        {
            InitializeComponent();
            ConstLexems();
        }

        private void Form1_Load(object sender, EventArgs e) { }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(openFileDialog1.FileName);
                textBox1.Clear();
                textBox1.AppendText(sr.ReadToEnd());
                sr.Close();
            }
        }
        public void ConstLexems()
        {
            constlexems.Add("");
            constlexems.Add("program");//1
            constlexems.Add("int");//2
            constlexems.Add("cout");//3
            constlexems.Add("cin");//4
            constlexems.Add("if");//5
            constlexems.Add("else");//6
            constlexems.Add("endif");//7
            constlexems.Add("for");//8
            constlexems.Add("to");//9
            constlexems.Add("step");//10
            constlexems.Add("do");//11
            constlexems.Add("next");//12
            constlexems.Add("or");//13
            constlexems.Add("and");//14
            constlexems.Add("not");//15
            constlexems.Add("{");//16
            constlexems.Add("}");//17
            constlexems.Add("(");//18
            constlexems.Add(")");//19
            constlexems.Add("[");//20
            constlexems.Add("]");//21
            constlexems.Add(";");//22
            constlexems.Add(",");//23
            constlexems.Add("+");//24
            constlexems.Add("-");//25
            constlexems.Add("*");//26
            constlexems.Add("/");//27
            constlexems.Add("^");//28
            constlexems.Add("=");//29
            constlexems.Add("<");//30
            constlexems.Add(">");//31
            constlexems.Add(">>");//32
            constlexems.Add("<<");//33
            constlexems.Add("<=");//34
            constlexems.Add(">=");//35
            constlexems.Add("==");//36
            constlexems.Add("!=");//37
            constlexems.Add("idn");//38
            constlexems.Add("con");//39
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearAll();
            TextRead();
            try
            {
                LA.State1();
                SA = new SyntaticAnalyzer(LA.tableLex);
                TableLexChange();
                bilder = new PolizBilder(InputVariables);
                bilder.Builder();
                Execute = new Executor(bilder.Poliz, bilder.PolizLabels, bilder.PolizCells, LA.tableIdn);
                ExecuteProgram();
                Tables();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Tables()
        {
            /*for (int i = 1; i < constlexems.Count(); i++)
            {
                dataGridView1.Rows.Add(i, constlexems[i]);
            }*/
            dataGridView2.DataSource = LA.tableLex.Select(x => new { N = x.Number + 1, Рядок = x.Line, Лексема = x.Token, Код = x.Kod, Код_Idn_Con = x.Kod_idn_con }).ToList();
            dataGridView3.DataSource = LA.tableIdn.Select(x => new { Код = x.Kod + 1, Idn = x.Token, Тип = x.Type }).ToList();
            dataGridView4.DataSource = LA.tableCon.Select(x => new { Код = x.Kod + 1, Con = x.Con }).ToList();
            for (int i = 0; i < Output.Count; i++)
            {
                textBox2.Text += Output[i].Token + ":" + (int)Output[i].Value + Environment.NewLine;
            }
            textBox3.Text = bilder.GetPolizString();
            dataGridView5.DataSource = bilder.table.Select(x => new { Вхідний = x.Input, Стек = x.Stack, Поліз = x.Poliz }).ToList();
            dataGridView7.DataSource = bilder.PolizLabels.Select(x => new { Мітка = x.Label, Позиція = x.Position }).ToList();
            dataGridView6.DataSource = Execute.table.Select(x => new { Вхідний = x.Input, Стек = x.Stack }).ToList();
        }

        private void ExecuteProgram()
        {
            while (Execute._Status != "Successful Done")
            {
                while (Execute._Status == "")
                {
                    Execute.GoNextStep();
                }
                if (Execute._Status.IndexOf("Input") != -1)
                {
                    Form2 f = new Form2(Execute._Stack.Peek().Token);
                    f.btnClick += F_btnClick;
                    f.ShowDialog();
                    while (Execute._Status != "")
                    {
                        Thread.Sleep(1000);
                    }
                }
                else if (Execute._Status.IndexOf("Output") != -1)
                {
                    var ind = Execute._IdentifyTable.Find(o => o.Token == Execute._Stack.Peek().Token);
                    Output.Add(new Identify(Execute._Stack.Peek().Token, "int", ind.Value));
                    Execute._Status = "";
                    Execute._Position++;
                }
            }
        }

        private void F_btnClick(object sender, InputArgs_ e)
        {
            string idn = Execute._Stack.Peek().Token;
            var obj = Execute._IdentifyTable.Find(o => o.Token == idn);
            obj.Value = e.Value;
            Execute._Status = "";
            Execute._Position++;
        }
        private void ClearAll()
        {
            count = 0;
            toread = false;
            Output.Clear();
            InputVariables.Clear();
            textBox2.Clear();
            textBox3.Clear();
            //dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView3.DataSource = null;
            dataGridView4.DataSource = null;
            dataGridView5.DataSource = null;
            dataGridView6.DataSource = null;
            dataGridView7.DataSource = null;
        }

        private void TextRead()
        {
            str = new char[textBox1.Text.Length];
            foreach (char ch in textBox1.Text)
            {
                str[count] = ch;
                count++;
            }
            if (str != null)
            {
                toread = true;
                LA = new LexicalAnalyzer(str, count, toread, constlexems);
            }
        }

        private static void TableLexChange()
        {
            for (int i = 0; i < LA.tableLex.Count; i++)
            {
                if (LA.tableLex[i].Kod == 38)
                    InputVariables.Add(new WorkItem(LA.tableLex[i].Token, "idn"));
                else if (LA.tableLex[i].Kod == 39)
                    InputVariables.Add(new WorkItem(LA.tableLex[i].Token, "con"));
                else if (LA.tableLex[i].Token == "-")
                {
                    if (i - 1 >= 0)
                    {
                        if (LA.tableLex[i - 1].Kod != 38 && LA.tableLex[i - 1].Kod != 39 && LA.tableLex[i - 1].Token != ")")
                            InputVariables.Add(new WorkItem("@", "operation"));
                        else
                            InputVariables.Add(new WorkItem(LA.tableLex[i].Token, "operation"));
                    }
                }
                else
                    InputVariables.Add(new WorkItem(LA.tableLex[i].Token, "operation"));
            }
        }
    }
}
