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
        public Form1()
        {
            InitializeComponent();

            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Registration No";
            dataGridView1.Columns[1].Name = "Name";
            dataGridView1.Columns[2].Name = "Degree";
        }

        public void add_to_records(object sender, EventArgs e)
        {
            string[] row = { reg_no.Text,
                               name.Text,
                               degree.Text };

            dataGridView1.Rows.Add(row);

            reg_no.Text = "";
            name.Text = "";
            degree.Text = "";

        }


        private void label1_Click(object sender, EventArgs e)
        {

        }


        public void displayx(object sender, EventArgs e)
        {
            string s = "";
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    s += cell.Value;

                }
            }
            label4.Text = s;
        }
    }
}
