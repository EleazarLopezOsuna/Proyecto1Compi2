﻿
namespace Proyecto1_Compiladores2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.translateButton = new System.Windows.Forms.Button();
            this.runButton = new System.Windows.Forms.Button();
            this.code_textbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.console_textbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.reportsButton = new System.Windows.Forms.Button();
            this.symbol_table = new System.Windows.Forms.DataGridView();
            this.table_label = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.error_table = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Nombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ambito = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fila = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Columna = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Valor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.symbol_table)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.error_table)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 42);
            this.button1.TabIndex = 0;
            this.button1.Text = "Open File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // translateButton
            // 
            this.translateButton.Enabled = false;
            this.translateButton.Location = new System.Drawing.Point(155, 12);
            this.translateButton.Name = "translateButton";
            this.translateButton.Size = new System.Drawing.Size(137, 42);
            this.translateButton.TabIndex = 1;
            this.translateButton.Text = "Translate";
            this.translateButton.UseVisualStyleBackColor = true;
            this.translateButton.Click += new System.EventHandler(this.translateButton_Click);
            // 
            // runButton
            // 
            this.runButton.Enabled = false;
            this.runButton.Location = new System.Drawing.Point(298, 12);
            this.runButton.Name = "runButton";
            this.runButton.Size = new System.Drawing.Size(137, 42);
            this.runButton.TabIndex = 2;
            this.runButton.Text = "Run";
            this.runButton.UseVisualStyleBackColor = true;
            this.runButton.Click += new System.EventHandler(this.runButton_Click);
            // 
            // code_textbox
            // 
            this.code_textbox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.code_textbox.Location = new System.Drawing.Point(12, 105);
            this.code_textbox.Multiline = true;
            this.code_textbox.Name = "code_textbox";
            this.code_textbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.code_textbox.Size = new System.Drawing.Size(551, 536);
            this.code_textbox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("BubbleGum", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(12, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 24);
            this.label1.TabIndex = 4;
            this.label1.Text = "Code";
            // 
            // console_textbox
            // 
            this.console_textbox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.console_textbox.Location = new System.Drawing.Point(583, 107);
            this.console_textbox.Multiline = true;
            this.console_textbox.Name = "console_textbox";
            this.console_textbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.console_textbox.Size = new System.Drawing.Size(760, 242);
            this.console_textbox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("BubbleGum", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(583, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 24);
            this.label2.TabIndex = 6;
            this.label2.Text = "Console";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(96, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Current position: ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(215, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 18);
            this.label4.TabIndex = 8;
            // 
            // reportsButton
            // 
            this.reportsButton.Enabled = false;
            this.reportsButton.Location = new System.Drawing.Point(441, 12);
            this.reportsButton.Name = "reportsButton";
            this.reportsButton.Size = new System.Drawing.Size(137, 42);
            this.reportsButton.TabIndex = 9;
            this.reportsButton.Text = "Reports";
            this.reportsButton.UseVisualStyleBackColor = true;
            // 
            // symbol_table
            // 
            this.symbol_table.AllowUserToAddRows = false;
            this.symbol_table.AllowUserToDeleteRows = false;
            this.symbol_table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.symbol_table.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Nombre,
            this.Tipo,
            this.Ambito,
            this.Fila,
            this.Columna,
            this.Valor});
            this.symbol_table.Location = new System.Drawing.Point(583, 399);
            this.symbol_table.Name = "symbol_table";
            this.symbol_table.ReadOnly = true;
            this.symbol_table.RowTemplate.Height = 25;
            this.symbol_table.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.symbol_table.Size = new System.Drawing.Size(760, 242);
            this.symbol_table.TabIndex = 10;
            this.symbol_table.Visible = false;
            // 
            // table_label
            // 
            this.table_label.AutoSize = true;
            this.table_label.Font = new System.Drawing.Font("BubbleGum", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.table_label.Location = new System.Drawing.Point(583, 372);
            this.table_label.Name = "table_label";
            this.table_label.Size = new System.Drawing.Size(169, 24);
            this.table_label.TabIndex = 11;
            this.table_label.Text = "Symbol Table";
            this.table_label.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // error_table
            // 
            this.error_table.AllowUserToAddRows = false;
            this.error_table.AllowUserToDeleteRows = false;
            this.error_table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.error_table.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            this.error_table.Location = new System.Drawing.Point(583, 399);
            this.error_table.Name = "error_table";
            this.error_table.ReadOnly = true;
            this.error_table.RowTemplate.Height = 25;
            this.error_table.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.error_table.Size = new System.Drawing.Size(760, 242);
            this.error_table.TabIndex = 12;
            this.error_table.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Tipo";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Descripcion";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn3.Width = 497;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Linea";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn4.Width = 60;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Columna";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn5.Width = 60;
            // 
            // Nombre
            // 
            this.Nombre.HeaderText = "Nombre";
            this.Nombre.Name = "Nombre";
            this.Nombre.ReadOnly = true;
            this.Nombre.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Nombre.Width = 153;
            // 
            // Tipo
            // 
            this.Tipo.HeaderText = "Tipo";
            this.Tipo.Name = "Tipo";
            this.Tipo.ReadOnly = true;
            this.Tipo.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Tipo.Width = 110;
            // 
            // Ambito
            // 
            this.Ambito.HeaderText = "Ambito";
            this.Ambito.Name = "Ambito";
            this.Ambito.ReadOnly = true;
            this.Ambito.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Fila
            // 
            this.Fila.HeaderText = "F";
            this.Fila.Name = "Fila";
            this.Fila.ReadOnly = true;
            this.Fila.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Fila.Width = 40;
            // 
            // Columna
            // 
            this.Columna.HeaderText = "C";
            this.Columna.Name = "Columna";
            this.Columna.ReadOnly = true;
            this.Columna.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Columna.Width = 40;
            // 
            // Valor
            // 
            this.Valor.HeaderText = "Valor";
            this.Valor.Name = "Valor";
            this.Valor.ReadOnly = true;
            this.Valor.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Valor.Width = 274;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1355, 662);
            this.Controls.Add(this.error_table);
            this.Controls.Add(this.table_label);
            this.Controls.Add(this.symbol_table);
            this.Controls.Add(this.reportsButton);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.console_textbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.code_textbox);
            this.Controls.Add(this.runButton);
            this.Controls.Add(this.translateButton);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.symbol_table)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.error_table)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button translateButton;
        private System.Windows.Forms.Button runButton;
        private System.Windows.Forms.TextBox code_textbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox console_textbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button reportsButton;
        private System.Windows.Forms.DataGridView symbol_table;
        private System.Windows.Forms.Label table_label;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridView error_table;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Nombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ambito;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fila;
        private System.Windows.Forms.DataGridViewTextBoxColumn Columna;
        private System.Windows.Forms.DataGridViewTextBoxColumn Valor;
    }
}

