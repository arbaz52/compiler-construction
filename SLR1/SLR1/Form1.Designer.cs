namespace SLR1
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InputProductionRules = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.OutputStates = new System.Windows.Forms.RichTextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.InputString = new System.Windows.Forms.TextBox();
            this.Parse = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // InputProductionRules
            // 
            this.InputProductionRules.Location = new System.Drawing.Point(12, 25);
            this.InputProductionRules.Name = "InputProductionRules";
            this.InputProductionRules.Size = new System.Drawing.Size(168, 218);
            this.InputProductionRules.TabIndex = 0;
            this.InputProductionRules.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter production rules:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 249);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(168, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "compute NFA";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.compute_NFA_button);
            // 
            // OutputStates
            // 
            this.OutputStates.Location = new System.Drawing.Point(12, 278);
            this.OutputStates.Name = "OutputStates";
            this.OutputStates.Size = new System.Drawing.Size(168, 160);
            this.OutputStates.TabIndex = 3;
            this.OutputStates.Text = "";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(186, 25);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(602, 364);
            this.dataGridView1.TabIndex = 5;
            // 
            // InputString
            // 
            this.InputString.Location = new System.Drawing.Point(186, 418);
            this.InputString.Name = "InputString";
            this.InputString.Size = new System.Drawing.Size(242, 20);
            this.InputString.TabIndex = 6;
            // 
            // Parse
            // 
            this.Parse.Location = new System.Drawing.Point(444, 415);
            this.Parse.Name = "Parse";
            this.Parse.Size = new System.Drawing.Size(75, 23);
            this.Parse.TabIndex = 7;
            this.Parse.Text = "Parse String";
            this.Parse.UseVisualStyleBackColor = true;
            this.Parse.Click += new System.EventHandler(this.Parse_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Parse);
            this.Controls.Add(this.InputString);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.OutputStates);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.InputProductionRules);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox InputProductionRules;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RichTextBox OutputStates;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox InputString;
        private System.Windows.Forms.Button Parse;
    }
}

