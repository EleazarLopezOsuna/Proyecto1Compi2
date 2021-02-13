using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Irony.Ast;
using Irony.Parsing;
using Proyecto1_Compiladores2.Graficador;
using Proyecto1_Compiladores2.Modelos;

namespace Proyecto1_Compiladores2.Analizador
{
    class Sintactico : Grammar
    {
        public static ParseTree Analizar(String cadena)
        {
            Gramatica gramatica = new Gramatica();
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser parser = new Parser(lenguaje);
            ParseTree arbol = parser.Parse(cadena);
            return arbol;
        }

        public static void crearImagen(ParseTreeNode raiz, NodoSintactico root, int tipo)
        {
            String grafoDot = Arbol.getDot(raiz, tipo, root);
            try
            {
                // Create the file.
                if (tipo == 0)
                {
                    using (FileStream fs = File.Create(@"C:\compiladores2\Arbol.dot"))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(grafoDot);
                        fs.Write(info, 0, info.Length);
                    }
                }
                else if (tipo == 1)
                {
                    using (FileStream fs = File.Create(@"C:\compiladores2\Arbol.dot"))
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(grafoDot);
                        fs.Write(info, 0, info.Length);
                    }
                }
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.FileName = "cmd.exe";
                startInfo.Arguments = "/C dot -Tpng C:/compiladores2/Arbol.dot -o C:/compiladores2/Arbol.png";
                process.StartInfo = startInfo;
                process.Start();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
