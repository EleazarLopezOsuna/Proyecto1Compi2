using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1_Compiladores2.Modelos
{
    class Objeto
    {
        public Dictionary<String, Simbolo> parametros;
        public Array arreglo;
        public Simbolo.EnumTipo tipo;
        public string nombreTipo;
        public string nombre;

        public Objeto(string nombre)
        {
            parametros = new Dictionary<string, Simbolo>();
            this.nombre = nombre;
        }

        public Simbolo buscar(String nombre, int linea, int columna)
        {
            nombre = nombre.ToLower();
            if (parametros.ContainsKey(nombre))
            {
                parametros.TryGetValue(nombre, out Simbolo sim);
                return sim;
            }
            return null;
        }

        public bool modificar(String nombre, Simbolo simbolo)
        {
            nombre = nombre.ToLower();
            if (parametros.ContainsKey(nombre))
            {
                parametros.Remove(nombre);
                parametros.Add(nombre, simbolo);
                return true;
            }
            return false;
        }

        public Simbolo.EnumTipo buscarTipo(string cadena)
        {
            switch (cadena)
            {
                case "cadena":
                    return Simbolo.EnumTipo.cadena;
                case "entero":
                    return Simbolo.EnumTipo.entero;
                case "real":
                    return Simbolo.EnumTipo.real;
                case "boleano":
                    return Simbolo.EnumTipo.boleano;
                case "nulo":
                    return Simbolo.EnumTipo.nulo;
                case "funcion":
                    return Simbolo.EnumTipo.funcion;
                case "procedimiento":
                    return Simbolo.EnumTipo.procedimiento;
                case "objeto":
                    return Simbolo.EnumTipo.objeto;
                case "arreglo":
                    return Simbolo.EnumTipo.arreglo;
                default:
                    return Simbolo.EnumTipo.error;
            }
        }
    }
}
