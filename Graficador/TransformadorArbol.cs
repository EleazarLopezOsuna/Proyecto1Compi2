using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Irony.Ast;
using Irony.Parsing;
using Proyecto1_Compiladores2.Modelos;

namespace Proyecto1_Compiladores2.Graficador
{
    class TransformadorArbol
    {
        public NodoSintactico arbolTraducido;
        private int contadorNodos;
        private ArrayList tmp;
        private string valor;

        public TransformadorArbol()
        {
            arbolTraducido = null;
            contadorNodos = 0;
        }
        public void generarArbol(ParseTreeNode root)
        {
            convertirArbol(arbolTraducido, root);
        }
        private void verificarHijos(NodoSintactico nuevo, NodoSintactico padre)
        {
            if (nuevo.getHijos().Count != 0)
            {
                padre.addHijo(nuevo);
            }
        }
        private NodoSintactico getTipo(ParseTreeNode root)
        {
            NodoSintactico nuevo = new NodoSintactico("TIPO", contadorNodos++);
            nuevo.addHijo(buscarOperacion(root.ChildNodes[0]));
            return nuevo;
        }
        public void convertirArbol(NodoSintactico padre, ParseTreeNode root)
        {
            NodoSintactico nuevo, nuevo2, nuevo3;
            switch (root.ToString())
            {
                case "ABAJO":
                    break;
                case "ARRIBA":
                    break;
                case "ASIGNACION":
                    break;
                case "BEGIN_END":
                    nuevo = new NodoSintactico("BEGIN", contadorNodos++);
                    padre.addHijo(nuevo);
                    foreach (ParseTreeNode hijo in root.ChildNodes)
                    {
                        convertirArbol(nuevo, hijo);
                    }
                    break;
                case "CASE":
                    break;
                case "CONTROLADOR":
                    break;
                case "D_CONSTANTE":
                    if (padre.getHijos().Count == 0)
                    {
                        nuevo = new NodoSintactico("D_CONSTANTE", contadorNodos++);
                        nuevo2 = new NodoSintactico("CONSTANTE", contadorNodos++);
                        nuevo3 = new NodoSintactico("ASIGNACION", contadorNodos++);
                        nuevo.addHijo(nuevo2);
                        nuevo.addHijo(nuevo3);
                        padre.addHijo(nuevo);
                        if (root.ChildNodes.Count == 5) //Asignacion con tipo
                        {
                            tmp = nuevo.getHijos();
                            ((NodoSintactico)tmp[0]).addHijo(buscarOperacion(root.ChildNodes[0]));
                            ((NodoSintactico)tmp[1]).addHijo(getTipo(root.ChildNodes[2]));

                            convertirArbol((NodoSintactico)tmp[1], root.ChildNodes[4]);

                            nuevo.setHijos(tmp);
                        }
                        else if(root.ChildNodes.Count == 3) //Asingnacion sin tipo
                        {
                            tmp = nuevo.getHijos();
                            ((NodoSintactico)tmp[0]).addHijo(buscarOperacion(root.ChildNodes[0]));

                            convertirArbol((NodoSintactico)tmp[1], root.ChildNodes[2]);

                            nuevo.setHijos(tmp);
                        }
                    }
                    else //Elimina la recursividad vacia
                    {
                        tmp = padre.getHijos();
                        foreach (ParseTreeNode hijo in root.ChildNodes)
                        {
                            convertirArbol((NodoSintactico)tmp[0], hijo);
                        }
                    }
                    break;
                case "D_VARIABLE":
                    if (padre.getHijos().Count == 0)
                    {
                        nuevo = new NodoSintactico("D_VARIABLE", contadorNodos++);
                        nuevo2 = new NodoSintactico("VARIABLE", contadorNodos++);
                        nuevo3 = new NodoSintactico("ASIGNACION", contadorNodos++);
                        nuevo.addHijo(nuevo2);
                        nuevo.addHijo(nuevo3);
                        padre.addHijo(nuevo);
                        if (root.ChildNodes.Count == 3) //Contiene 1 variable sin asignacion
                        {
                            tmp = nuevo.getHijos();
                            ((NodoSintactico)tmp[0]).addHijo(buscarOperacion(root.ChildNodes[0]));
                            ((NodoSintactico)tmp[1]).addHijo(getTipo(root.ChildNodes[2]));
                            nuevo.setHijos(tmp);
                        }
                        else if (root.ChildNodes.Count == 4) //Contiene n variables sin asignacion
                        {
                            tmp = nuevo.getHijos();
                            //tmp[0] es el hijo que contiene a las variables
                            ((NodoSintactico)tmp[0]).addHijo(buscarOperacion(root.ChildNodes[0]));
                            convertirArbol(((NodoSintactico)tmp[0]), root.ChildNodes[1]);
                            //tmp[1] es el hijo que contiene los datos de asignacion, tipo y valor
                            ((NodoSintactico)tmp[1]).addHijo(getTipo(root.ChildNodes[3]));
                            nuevo.setHijos(tmp);
                        }
                        else if (root.ChildNodes.Count == 5) //Contiene 1 variable con asignacion
                        {
                            tmp = nuevo.getHijos();
                            ((NodoSintactico)tmp[0]).addHijo(buscarOperacion(root.ChildNodes[0]));
                            ((NodoSintactico)tmp[1]).addHijo(getTipo(root.ChildNodes[2]));

                            convertirArbol((NodoSintactico)tmp[1], root.ChildNodes[4]);

                            nuevo.setHijos(tmp);
                        }
                    }
                    else //Elimina la recursividad vacia
                    {
                        tmp = padre.getHijos();
                        foreach (ParseTreeNode hijo in root.ChildNodes)
                        {
                            convertirArbol((NodoSintactico)tmp[0], hijo);
                        }
                    }
                    break;
                case "DECLARACION_CAMPOS_TYPE":
                    break;
                case "ESTRUCTURA":
                    break;
                case "EXPRESION":
                    if (root.ChildNodes.Count != 0)
                    {
                        if (padre.getNombre() != "EXPRESION")
                        {
                            nuevo = new NodoSintactico("EXPRESION", contadorNodos++);

                            foreach (ParseTreeNode hijo in root.ChildNodes)
                            {
                                convertirArbol(nuevo, hijo);
                            }

                            padre.addHijo(nuevo);
                        }
                        else
                        {
                            nuevo = new NodoSintactico("NO TOMAR EN CUENTA", contadorNodos++);

                            foreach (ParseTreeNode hijo in root.ChildNodes)
                            {
                                convertirArbol(nuevo, hijo);
                            }

                            padre.setHijos(nuevo.getHijos());
                        }
                    }
                    break;
                case "FOR":
                    break;
                case "FUNCION":
                    break;
                case "FUNCION_HEAD":
                    break;
                case "IF":
                    break;
                case "INDICE":
                    break;
                case "LLAMADA":
                    break;
                case "OPCION_CASE":
                    break;
                case "OPERADOR":
                    nuevo = buscarOperacion(root.ChildNodes[0]);
                    if (!(nuevo is null))
                    {
                        padre.addHijo(nuevo);
                    }
                    break;
                case "PA":
                    break;
                case "PF":
                    break;
                case "PFVL":
                    break;
                case "PFVR":
                    break;
                case "PROCEDIMIENTO":
                    break;
                case "PROCEDIMIENTO_HEAD":
                    break;
                case "PROGRAMA":
                    arbolTraducido = new NodoSintactico("PROGRAMA", contadorNodos++);
                    foreach (ParseTreeNode hijo in root.ChildNodes)
                    {
                        convertirArbol(arbolTraducido, hijo);
                    }
                    break;
                case "RANGO":
                    break;
                case "REPEAT":
                    break;
                case "R_ID":
                    if (root.ChildNodes.Count != 0)
                    {
                        padre.addHijo(buscarOperacion(root.ChildNodes[1]));
                        convertirArbol(padre, root.ChildNodes[2]);
                    }
                    break;
                case "SENTENCIA":
                    break;
                case "SUBPROGRAMA":
                    break;
                case "T_DATO":
                    break;
                case "T_ELEMENTAL":
                    break;
                case "T_ESTRUCTURADO":
                    break;
                case "T_ORDINAL":
                    break;
                case "VALOR":
                    convertirArbol(padre, root.ChildNodes[0]);
                    break;
                case "VARIABLE":
                    break;
                case "WHILE":
                    break;
                case "Z_CONSTANTES":
                    if (root.ChildNodes.Count != 0)
                    {
                        nuevo = new NodoSintactico("CONSTANTES", contadorNodos++);
                        foreach (ParseTreeNode hijo in root.ChildNodes)
                        {
                            convertirArbol(nuevo, hijo);
                        }
                        verificarHijos(nuevo, padre);
                    }
                    break;
                case "Z_DECLARACIONES":
                    if (root.ChildNodes.Count != 0)
                    {
                        nuevo = new NodoSintactico("DECLARACIONES", contadorNodos++);
                        foreach (ParseTreeNode hijo in root.ChildNodes)
                        {
                            convertirArbol(nuevo, hijo);
                        }
                        verificarHijos(nuevo, padre);
                    }
                    break;
                case "Z_SUBPROGRAMAS":
                    if (root.ChildNodes.Count != 0)
                    {
                        nuevo = new NodoSintactico("SUBPROGRAMAS", contadorNodos++);
                        foreach (ParseTreeNode hijo in root.ChildNodes)
                        {
                            convertirArbol(nuevo, hijo);
                        }
                        verificarHijos(nuevo, padre);
                    }
                    break;
                case "Z_TIPOS":
                    if (root.ChildNodes.Count != 0)
                    {
                        nuevo = new NodoSintactico("TIPOS", contadorNodos++);
                        foreach (ParseTreeNode hijo in root.ChildNodes)
                        {
                            convertirArbol(nuevo, hijo);
                        }
                        verificarHijos(nuevo, padre);
                    }
                    break;
                case "Z_VARIABLES":
                    if (root.ChildNodes.Count != 0)
                    {
                        nuevo = new NodoSintactico("VARIABLES", contadorNodos++);
                        foreach (ParseTreeNode hijo in root.ChildNodes)
                        {
                            convertirArbol(nuevo, hijo);
                        }
                        verificarHijos(nuevo, padre);
                    }
                    break;
                default:
                    nuevo = buscarOperacion(root);
                    if(!(nuevo is null))
                    {
                        padre.addHijo(buscarOperacion(root));
                    }
                    break;
            }
        }

        private NodoSintactico buscarOperacion(ParseTreeNode root)
        {
            NodoSintactico nuevo;
            string token = root.ToString();
            if (token.Contains(" (integer)"))
            {
                nuevo = new NodoSintactico("ENTERO", contadorNodos++);
                valor = token.Replace("\"", "");
                valor = valor.Replace(" (integer)", "");
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Contains(" (real)"))
            {
                nuevo = new NodoSintactico("REAL", contadorNodos++);
                valor = token.Replace("\"", "");
                valor = valor.Replace(" (real)", "");
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Contains(" (cadena)"))
            {
                nuevo = new NodoSintactico("CADENA", contadorNodos++);
                valor = token;
                valor = valor.Replace(" (cadena)", "");
                valor = valor.Substring(0, valor.Length);
                valor = valor.Replace("\\\'", "\'");
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Contains(" (boleano)"))
            {
                nuevo = new NodoSintactico("BOLEANO", contadorNodos++);
                valor = token.Replace("\"", "");
                valor = valor.Replace(" (boleano)", "");
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Contains(" (id)"))
            {
                nuevo = new NodoSintactico("ID", contadorNodos++);
                valor = token.Replace("\"", "");
                valor = valor.Replace(" (id)", "");
                nuevo.setValor(valor);
                return nuevo;
            }
            if (token.Equals("* (Key symbol)"))
            {
                nuevo = new NodoSintactico("MULTIPLICACION", contadorNodos++);
                valor = "*";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("+ (Key symbol)"))
            {
                nuevo = new NodoSintactico("SUMA", contadorNodos++);
                valor = "+";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("/ (Key symbol)"))
            {
                nuevo = new NodoSintactico("DIVISION", contadorNodos++);
                valor = "/";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("- (Key symbol)"))
            {
                nuevo = new NodoSintactico("RESTA", contadorNodos++);
                valor = "-";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("% (Keyword)"))
            {
                nuevo = new NodoSintactico("MODULO", contadorNodos++);
                valor = "modulo";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("and (Keyword)"))
            {
                nuevo = new NodoSintactico("AND", contadorNodos++);
                valor = "AND";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("or (Keyword)"))
            {
                nuevo = new NodoSintactico("OR", contadorNodos++);
                valor = "OR";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("not (Keyword)"))
            {
                nuevo = new NodoSintactico("NOT", contadorNodos++);
                valor = "NOT";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("> (Key symbol)"))
            {
                nuevo = new NodoSintactico("MAYOR", contadorNodos++);
                valor = ">";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("< (Key symbol)"))
            {
                nuevo = new NodoSintactico("MENOR", contadorNodos++);
                valor = "<";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals(">= (Key symbol)"))
            {
                nuevo = new NodoSintactico("MAYORIGUAL", contadorNodos++);
                valor = ">=";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("<= (Key symbol)"))
            {
                nuevo = new NodoSintactico("MENORIGUAL", contadorNodos++);
                valor = ">=";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("= (Key symbol)"))
            {
                nuevo = new NodoSintactico("IGUAL", contadorNodos++);
                valor = "==";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("<> (Key symbol)"))
            {
                nuevo = new NodoSintactico("DESIGUAL", contadorNodos++);
                valor = "<>";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("break (Keyword)"))
            {
                nuevo = new NodoSintactico("BREAK", contadorNodos++);
                valor = "break";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("integer (Keyword)"))
            {
                nuevo = new NodoSintactico("ENTERO", contadorNodos++);
                valor = "integer";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("real (Keyword)"))
            {
                nuevo = new NodoSintactico("REAL", contadorNodos++);
                valor = "real";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("boolean (Keyword)"))
            {
                nuevo = new NodoSintactico("BOLEANO", contadorNodos++);
                valor = "boolean";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if (token.Equals("string (Keyword)"))
            {
                nuevo = new NodoSintactico("CADENA", contadorNodos++);
                valor = "string";
                nuevo.setValor(valor);
                return nuevo;
            }
            else if(token.Equals("continue (Keyword)"))
            {
                nuevo = new NodoSintactico("CONTINUE", contadorNodos++);
                valor = "continue";
                nuevo.setValor(valor);
                return nuevo;
            }
            else
            {
                return null;
            }
        }
    }
}
