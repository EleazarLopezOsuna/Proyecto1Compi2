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
    }
}
