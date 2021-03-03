using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Irony.Ast;
using Irony.Parsing;
using Proyecto1_Compiladores2.Analizador;
using Proyecto1_Compiladores2.Graficador;
using Proyecto1_Compiladores2.Modelos;

namespace Proyecto1_Compiladores2
{
    public partial class Form1 : Form
    {
        private string fileName;
        private ParseTree resultadoAnalisis = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dataGridView1.Rows.Add("Nombre", "Tipo", "Ambito", "Fila", "Columna", "Valor");
            console_textbox.Text = "";
            code_textbox.Text = "";
            fileName = "";

            //Parametros para el File Dialog
            openFileDialog1.InitialDirectory = @"C:\compiladores2\";
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "Text files (*.txt)|*.txt";


            openFileDialog1.ShowDialog();
            fileName = openFileDialog1.FileName;
            if(!fileName.Equals(""))
            {
                string textFromFile = File.ReadAllText(fileName);
                code_textbox.Text = textFromFile;
                translateButton.Enabled = true;
                runButton.Enabled = true;
            }
        }

        private void translateButton_Click(object sender, EventArgs e)
        {
            resultadoAnalisis = null;
            resultadoAnalisis = SintacticoTraductor.Analizar(code_textbox.Text);
            error_table.Rows.Clear();
            symbol_table.Rows.Clear();

            if (resultadoAnalisis != null)
            {
                if(resultadoAnalisis.ParserMessages.Count == 0)
                {
                    table_label.Text = "Symbol Table";
                    symbol_table.Visible = true;
                    error_table.Visible = false;

                    //Graficar Arbol Irony
                    SintacticoTraductor.crearImagen(resultadoAnalisis.Root, null, 0);

                    Thread.Sleep(1000);
                    var p = new Process();

                    //Abrir imagen Irony
                    p.StartInfo = new ProcessStartInfo(@"C:\compiladores2\ArbolIrony.png")
                    {
                        UseShellExecute = true
                    };
                    p.Start();
                }
                else
                {
                    table_label.Text = "Error Table";
                    symbol_table.Visible = false;
                    error_table.Visible = true;

                    string mensajeTraducido = "";

                    foreach (Irony.LogMessage error in resultadoAnalisis.ParserMessages)
                    {
                        if (error.Message.Contains("expected"))
                            mensajeTraducido = error.Message.Replace("Syntax error, expected: ", "Se esperaba el token: ");
                        else
                            mensajeTraducido = "No se encontro simbolo para finalizar la cadena";
                        error_table.Rows.Add("Sintactico", mensajeTraducido, error.Location.Line + 1, error.Location.Column + 1);
                    }
                }
            }
            else
            {
                
            }
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            resultadoAnalisis = null;
            resultadoAnalisis = SintacticoInterprete.Analizar(code_textbox.Text);
            error_table.Rows.Clear();
            symbol_table.Rows.Clear();
            console_textbox.Text = "";

            if (resultadoAnalisis != null)
            {
                if (resultadoAnalisis.ParserMessages.Count == 0)
                {
                    table_label.Text = "Symbol Table";
                    symbol_table.Visible = true;
                    error_table.Visible = false;

                    SemanticoInterprete semanticoInterprete = new SemanticoInterprete();
                    semanticoInterprete.iniciarAnalisisSintactico(resultadoAnalisis.Root);

                    foreach (Entorno entorno in semanticoInterprete.entornos)
                    {
                        foreach (KeyValuePair<string, Simbolo> variable in entorno.tabla)
                        {
                            if (typeof(Objeto).IsInstanceOfType(variable.Value.valor))
                            {
                                string valor = "";
                                if (((Objeto)variable.Value.valor).arreglo is null)
                                {
                                    foreach (KeyValuePair<string, Simbolo> parametro in ((Objeto)variable.Value.valor).parametros)
                                    {
                                        if (valor == "")
                                        {
                                            valor = parametro.Key;
                                        }
                                        else
                                        {
                                            valor += ", " + parametro.Key;
                                        }
                                    }
                                }
                                else
                                {
                                    Objeto tmp = (Objeto)variable.Value.valor;
                                    if (tmp.tipo != Simbolo.EnumTipo.objeto)
                                    {
                                        valor = tmp.tipo + "[" + tmp.arreglo.GetLength(0);
                                    }
                                    else
                                    {
                                        valor = tmp.nombreTipo + "[" + tmp.arreglo.GetLength(0);
                                    }
                                    for (int i = 1; i < tmp.arreglo.Rank; i++)
                                    {
                                        valor += ", " + tmp.arreglo.GetLength(i);
                                    }
                                    valor += "]";
                                }
                                symbol_table.Rows.Add(variable.Key, variable.Value.tipo, entorno.nombreEntorno, variable.Value.fila, variable.Value.columna, valor);
                            }
                            else
                            {
                                symbol_table.Rows.Add(variable.Key, variable.Value.tipo, entorno.nombreEntorno, variable.Value.fila, variable.Value.columna, variable.Value.valor.ToString());
                            }
                        }
                    }
                    foreach (string texto in semanticoInterprete.consola)
                    {
                        if (texto.Contains("\n"))
                        {
                            console_textbox.Text += texto.Replace("\n", "") + Environment.NewLine;
                        }
                        else
                        {
                            console_textbox.Text += texto;
                        }
                    }

                    //Graficar Arbol Irony
                    SintacticoInterprete.crearImagen(resultadoAnalisis.Root, null, 0);

                    /*Thread.Sleep(1000);
                    var p = new Process();

                    //Abrir imagen Irony
                    p.StartInfo = new ProcessStartInfo(@"C:\compiladores2\ArbolIrony.png")
                    {
                        UseShellExecute = true
                    };
                    p.Start();*/
                }
                else
                {
                    table_label.Text = "Error Table";
                    symbol_table.Visible = false;
                    error_table.Visible = true;

                    string mensajeTraducido = "";

                    foreach (Irony.LogMessage error in resultadoAnalisis.ParserMessages)
                    {
                        if (error.Message.Contains("expected"))
                            mensajeTraducido = error.Message.Replace("Syntax error, expected: ", "Se esperaba el token: ");
                        else
                            mensajeTraducido = "No se encontro simbolo para finalizar la cadena";
                        error_table.Rows.Add("Sintactico", mensajeTraducido, error.Location.Line + 1, error.Location.Column + 1);
                    }
                }
            }
            else
            {

            }
        }
    }
}
