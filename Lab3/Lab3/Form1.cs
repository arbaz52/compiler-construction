using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void find_constants(object sender, EventArgs e)
        {
            Regex rgx = new Regex(@"^([0-9]+(.[0-9]*))$");
            //split
            string[] words = inputField.Text.Split(' ');
            for(int  i = 0; i < words.Length; i++)
            {
                if (rgx.Match(words[i]).Success)
                {
                    richTextBox1.Text += words[i] + " ";
                }
            }
        }

        private void find_keywords(object sender, EventArgs e)
        {
            Regex rgx = new Regex(@"^(int|float|char)$");
            //split
            string[] words = inputField.Text.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (rgx.Match(words[i]).Success)
                {
                    richTextBox1.Text += words[i] + " ";
                }
            }
        }

        private void activity3(object sender, EventArgs e)
        {
            Regex rgx = new Regex(@"^([tm])");
            //split
            string[] words = inputField.Text.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (rgx.Match(words[i]).Success)
                {
                    richTextBox1.Text += words[i] + " ";
                }
            }
        }
    }
}
