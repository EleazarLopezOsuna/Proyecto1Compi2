using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1_Compiladores2.Modelos
{
    class Entorno
    {
        public Dictionary<String, Simbolo> tabla;
        public Entorno anterior;

        public Entorno(Entorno anterior)
        {
            this.anterior = anterior;
            this.tabla = new Dictionary<String, Simbolo>();
        }

        public bool insertar(String nombre, Simbolo sim, int linea, int columna)
        {
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
            tabla.Add(nombre, sim);
            return true;
        }

        public Simbolo buscar(String nombre, int linea, int columna)
        {
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
            for (Entorno e = this; e != null; e = e.anterior)
            {
                if (e.tabla.ContainsKey(nombre))
                {
                    e.tabla.Remove(nombre);
                    e.tabla.Add(nombre, simbolo);
                    return true;
                }
            }
            return false;
        }
    }
}