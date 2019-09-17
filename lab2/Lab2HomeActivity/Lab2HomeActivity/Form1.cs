using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void check_for_logical_operators(object sender, EventArgs e)
        {
            string pattern = "[&]{2}|[|]{2}|[!]";

            string[] input = textBox1.Text.Split(' ');

            Regex rgx = new Regex(pattern);
            
            for(int i = 0; i < input.Length; i++)
            {
                if(!rgx.Match(input[i]+"").Success)
                    MessageBox.Show("Invalid " + input[i]);
                else
                    richTextBox1.Text = richTextBox1.Text + input[i] + " ";
            }

            
        }

        private void check_for_relational_operators(object sender, EventArgs e)
        {
            string pattern = "([=]){2}|[<>][=]|[<>]";

            string[] input = textBox1.Text.Split(' ');

            Regex rgx = new Regex(pattern);

            for (int i = 0; i < input.Length; i++)
            {
                if (!rgx.Match(input[i] + "").Success)
                    MessageBox.Show("Invalid " + input[i]);
                else
                    richTextBox1.Text = richTextBox1.Text + input[i] + " ";
            }


        }
    }
}
