﻿using System;
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

        private void button1_Click(object sender, EventArgs e)
        {
            // take input from a richtextbox/textbox 
            String var = richTextBox1.Text;
            // split the input on the basis of space
            String[] words = var.Split(' ');
            // Regular Expression for operators
            Regex regex1 = new Regex(@"^[+|\-|*|/]$");
            for (int i = 0; i < words.Length; i++)
            {
                Match match1 = regex1.Match(words[i]);
                if (match1.Success)
                {
                    richTextBox2.Text += words[i] + " ";
                }
                else
                {
                    MessageBox.Show("invalid " + words[i]);
                }
            }
        }

    }
}
