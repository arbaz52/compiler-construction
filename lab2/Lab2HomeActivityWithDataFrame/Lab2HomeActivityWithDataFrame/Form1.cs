using System;
using System.Collections;
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

            dataGridView1.ColumnCount = 4;
            dataGridView1.Columns[0].Name = "Serial No";
            dataGridView1.Columns[1].Name = "Token ID";
            dataGridView1.Columns[2].Name = "Symbol";
            dataGridView1.Columns[3].Name = "Number of times";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void check(object sender, EventArgs e)
        {
            string p_art = @"^[+|\-|*|/]$";
            string p_variable = @"^[A-Za-z]([A-Za-z0-9]){0,24}$";

            string p_logical = @"[&]{2}|[|]{2}|[!]";
            string p_relational = @"([=]){2}|[<>][=]|[<>]";


            Regex r_art = new Regex(p_art);
            Regex r_variable = new Regex(p_variable);
            Regex r_logical = new Regex(p_logical);
            Regex r_relational = new Regex(p_relational);

            string[] words = input.Text.Split(' ');

            dataGridView1.Rows.Clear();

            for (int i = 0; i < words.Length; i++)
            {
                int symbol_row_index = get_symbol_row(words[i]);
                if (symbol_row_index != -1)
                {
                    string x = dataGridView1.Rows[symbol_row_index].Cells[3].Value.ToString();
                    int count = int.Parse(x) + 1;
                    dataGridView1.Rows[symbol_row_index].Cells[3].Value = count;
                }
                else
                {
                    string[] row = { i + "", "", words[i], "1" };
                    if (r_art.Match(words[i]).Success)
                    {
                        row[1] = "Arthematic";
                    }
                    else if (r_variable.Match(words[i]).Success)
                    {
                        row[1] = "Variable";
                    }
                    else if (r_logical.Match(words[i]).Success)
                    {
                        row[1] = "Logical";
                    }
                    else if (r_relational.Match(words[i]).Success)
                    {
                        row[1] = "Relational";
                    }

                    dataGridView1.Rows.Add(row);
                }
            }

        }


        public int get_symbol_row(string symbol)
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (dataGridView1.Rows[i].Cells[2].Value.ToString().Equals(symbol))
                    return i;
            }

           return -1;
        }
    }
}
