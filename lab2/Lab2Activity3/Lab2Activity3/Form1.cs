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

        private void check_variable_name(object sender, EventArgs e)
        {
            string variable_name = textBox1.Text;

            string pattern = @"^[A-Za-z]([A-Za-z0-9]){0,24}$";

            Regex rgx = new Regex(pattern);

            Match is_ok = rgx.Match(variable_name);
            if (is_ok.Success)
                MessageBox.Show("OK");
            else
                MessageBox.Show("Nottttttt");
        }

    }
}
