using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

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

        public Objeto(Objeto objeto)
        {
            this.nombre = objeto.nombre;
            this.nombreTipo = objeto.nombreTipo;
            this.parametros = new Dictionary<string, Simbolo>();
            foreach (KeyValuePair<string, Simbolo> parametro in objeto.parametros)
            {
                this.parametros.Add(parametro.Key, parametro.Value);
            }
            this.tipo = objeto.tipo;
            if (!(objeto.arreglo is null))
            {
                this.arreglo = objeto.arreglo;
            }
        }

        public Simbolo buscar(String nombre)
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
                Simbolo viejo;
                parametros.TryGetValue(nombre, out viejo);
                if (viejo.tipo == simbolo.tipo)
                {
                    parametros.Remove(nombre);
                    parametros.Add(nombre, simbolo);
                    return true;
                }
                return false;
            }
            return false;
        }    }
}
