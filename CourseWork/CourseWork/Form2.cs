using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourseWork
{
    public partial class Form2 : Form
    {
        public Form2(string var)
        {
            InitializeComponent();
            btn.Click += Btn_Click;
            label1.Text += var + ":";
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            int a;
            if (int.TryParse(textBox1.Text, out a))
            {
                btnClick.Invoke(this, new InputArgs_((object)a));
                this.Close();
            }
            else
                MessageBox.Show("Invalid value");
        }
        public event EventHandler<InputArgs_> btnClick;
        private void Form2__Load(object sender, EventArgs e) { }
    }
    public class InputArgs_ : EventArgs
    {
        public object Value;
        public InputArgs_(object obj)
        {
            Value = obj;
        }
    }
}
