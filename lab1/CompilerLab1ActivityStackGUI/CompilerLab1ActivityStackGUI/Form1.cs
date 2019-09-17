using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        Stack st;
        public Form1()
        {
            InitializeComponent();
            st = new Stack();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        public void display_stack()
        {

            string str = "";
            foreach (char c in st)
            {
                str += c + " ";
            }
            displayStackState.Text = str;
        }


        public void insert_into_stack(object sender, EventArgs e)
        {
            char c = charToInsert.Text[0];
            st.Push(c);
            display_stack();
        }

        public void pop_from_stack(object sender, EventArgs e)
        {
            st.Pop();
            display_stack();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
    }
}
