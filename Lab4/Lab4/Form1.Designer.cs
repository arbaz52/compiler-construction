namespace Lab4
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
            this.tfInput = new System.Windows.Forms.RichTextBox();
            this.tfTokens = new System.Windows.Forms.RichTextBox();
            this.symbolTable = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tfInput
            // 
            this.tfInput.Location = new System.Drawing.Point(22, 12);
            this.tfInput.Name = "tfInput";
            this.tfInput.Size = new System.Drawing.Size(203, 85);
            this.tfInput.TabIndex = 0;
            this.tfInput.Text = "";
            // 
            // tfTokens
            // 
            this.tfTokens.Location = new System.Drawing.Point(22, 119);
            this.tfTokens.Name = "tfTokens";
            this.tfTokens.Size = new System.Drawing.Size(203, 85);
            this.tfTokens.TabIndex = 1;
            this.tfTokens.Text = "";
            // 
            // symbolTable
            // 
            this.symbolTable.Location = new System.Drawing.Point(22, 225);
            this.symbolTable.Name = "symbolTable";
            this.symbolTable.Size = new System.Drawing.Size(203, 85);
            this.symbolTable.TabIndex = 2;
            this.symbolTable.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(335, 146);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btn_Input_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 347);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.symbolTable);
            this.Controls.Add(this.tfTokens);
            this.Controls.Add(this.tfInput);
            this.Name = "Form1";
            this.Text = "tfInput";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox tfInput;
        private System.Windows.Forms.RichTextBox tfTokens;
        private System.Windows.Forms.RichTextBox symbolTable;
        private System.Windows.Forms.Button button1;
    }
}

