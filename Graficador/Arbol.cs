using Irony.Parsing;
using Proyecto1_Compiladores2.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1_Compiladores2.Graficador
{
    class Arbol
    {

        private static int contador;
        private static String grafo;

        public static String getDot(ParseTreeNode raiz, int tipo, NodoSintactico root)
        {
            grafo = "digraph G{\n";
            grafo += "node[shape=\"box\"];\n";
            contador = 1;
            if (tipo == 0)
            {
                grafo += "nodo0[label=\"" + escapar(raiz.ToString()) + "\"];\n";
                recorrerAST("nodo0", raiz);
            }
            else if (tipo == 1)
            {
                grafo += graficarNodo(root);
            }
            grafo += "\n}";
            return grafo;
        }

        private static void recorrerAST(String padre, ParseTreeNode hijos)
        {
            foreach (ParseTreeNode hijo in hijos.ChildNodes)
            {
                String nombreHijo = "nodo" + contador.ToString();
                grafo += nombreHijo + "[label=\"" + escapar(hijo.ToString()) + "\"];\n";
                grafo += padre + "->" + nombreHijo + "\n";
                contador++;
                recorrerAST(nombreHijo, hijo);
            }
        }

        public static string graficarNodo(NodoSintactico nodo)
        {
            string cadena = "";
            foreach (NodoSintactico hijos in nodo.getHijos())
            {
                String nodoPadre = "";
                String nodoHijo = "";
                if (nodo.getValor() != null)
                {
                    nodoPadre = "\"" + nodo.getNumNodo() + "_" + nodo.getNombre() + "\"" + "[label = \"" + nodo.getValor() + "\"];";
                }
                else
                {
                    nodoPadre = "\"" + nodo.getNumNodo() + "_" + nodo.getNombre() + "\"" + "[label = \"" + nodo.getNombre() + "\"];";
                }
                if (hijos.getValor() != null)
                {
                    nodoHijo = "\"" + hijos.getNumNodo() + "_" + hijos.getNombre() + "\"" + "[label = \"" + hijos.getValor() + "\"];";
                }
                else
                {
                    nodoHijo = "\"" + hijos.getNumNodo() + "_" + hijos.getNombre() + "\"" + "[label = \"" + hijos.getNombre() + "\"];";
                }
                String apuntadorPadre = "\"" + nodo.getNumNodo() + "_" + nodo.getNombre() + "\"";
                String apuntadorHijo = "\"" + hijos.getNumNodo() + "_" + hijos.getNombre() + "\"";
                cadena += nodoPadre + "\n" + nodoHijo + "\n" + apuntadorPadre + "->" + apuntadorHijo + ";\n";
                cadena += graficarNodo(hijos);
            }
            return cadena;
        }

        private static String escapar(String cadena)
        {
            cadena = cadena.Replace("\\", "\\\\");
            cadena = cadena.Replace("\"", "\\\"");
            return cadena;
        }
    }
}
