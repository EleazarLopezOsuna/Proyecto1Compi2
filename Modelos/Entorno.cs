﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Proyecto1_Compiladores2.Modelos
{
    class Entorno
    {
        public Dictionary<String, Simbolo> tabla;
        public Entorno anterior;
        public string nombreEntorno;

        public Entorno(Entorno anterior, string nombreEntorno)
        {
            this.anterior = anterior;
            this.tabla = new Dictionary<String, Simbolo>();
            this.nombreEntorno = nombreEntorno;
        }

        public bool insertar(String nombre, Simbolo sim, int linea, int columna)
        {
            nombre = nombre.ToLower();
            if (tabla.ContainsKey(nombre))
            {
                return false;
            }
            if (sim.tipo == Simbolo.EnumTipo.cadena)
            {
                String cadena = sim.valor.ToString();
                if (cadena.Contains("\\n"))
                {
                    String[] stringSeparators = new string[] { "\\n" };
                    String[] texto = cadena.Split(stringSeparators, StringSplitOptions.None);
                    String nuevaCadena = "";
                    for (int i = 0; i < texto.Length; i++)
                    {
                        if (i == 0)
                        {
                            nuevaCadena += texto[i];
                        }
                        else
                        {
                            nuevaCadena += "\n" + texto[i];
                        }
                    }
                    sim = new Simbolo(Simbolo.EnumTipo.cadena, nuevaCadena);
                }
            }
            sim.fila = linea;
            sim.columna = columna;
            tabla.Add(nombre, sim);
            return true;
        }

        public Simbolo buscar(String nombre)
        {
            nombre = nombre.ToLower();
            for (Entorno e = this; e != null; e = e.anterior)
            {
                if (e.tabla.ContainsKey(nombre))
                {
                    e.tabla.TryGetValue(nombre, out Simbolo sim);
                    return sim;
                }
            }
            return null;
        }

        public bool modificar(String nombre, Simbolo simbolo)
        {
            nombre = nombre.ToLower();
            for (Entorno e = this; e != null; e = e.anterior)
            {
                if (e.tabla.ContainsKey(nombre))
                {
                    Simbolo nuevo;
                    e.tabla.TryGetValue(nombre, out nuevo);
                    if (simbolo is null)
                        return false;
                    nuevo.valor = simbolo.valor;
                    e.tabla.Remove(nombre);
                    e.tabla.Add(nombre, nuevo);
                    return true;
                }
            }
            return false;
        }
    }
}