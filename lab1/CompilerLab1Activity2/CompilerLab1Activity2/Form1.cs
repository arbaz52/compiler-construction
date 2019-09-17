using System;
using System.Collections.Generic;
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
        private double accumulator = 0;
        private char lastOperation;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        //custom methods
        private void operator_pressed(object sender, EventArgs e)
        {
            char operation = (sender as Button).Text[0];
            if (operation == 'C')
            {
                accumulator = 0;
            }
            else
            {
                double currentValue = double.Parse(Display.Text);
                switch (lastOperation)
                {
                    case '+': accumulator += currentValue; break;
                    case '-': accumulator -= currentValue; break;
                    case '×': accumulator *= currentValue; break;
                    case '÷': accumulator /= currentValue; break;
                    default: accumulator = currentValue; break;
                }
            }

            lastOperation = operation;
            Display.Text = operation == '=' ? accumulator.ToString() : "0";

        }
        private void number_pressed(object sender, EventArgs e)
        {
            string number = (sender as Button).Text;
            Display.Text = Display.Text == "0" ? number : Display.Text + number;

        }



        private void trig_pressed(object sender, EventArgs e)
        {
            double currentValue = double.Parse(Display.Text);
            string operation = (sender as Button).Text;
            double newvalue = 0;
            switch (operation)
            {
                case "Sine":
                    newvalue = Math.Sin(currentValue);
                    break;

                case "Cosine":
                    newvalue = Math .Cos(currentValue);
                    break;

                case "Tan":
                    newvalue = Math.Tan(currentValue);
                    break;


            }

            Display.Text = newvalue.ToString();
        }
    }
}
