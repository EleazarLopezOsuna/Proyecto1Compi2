using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Irony.Parsing;

namespace Proyecto1_Compiladores2.Modelos
{
    class SubPrograma
    {
        public Dictionary<string, Simbolo> parametrosVariable;
        public Dictionary<string, Simbolo> parametrosValor;
        public Dictionary<string, string> correlacionParametros;
        public ArrayList ordenParametros;
        public ParseTreeNode root;
        public Simbolo.EnumTipo tipo;
        public Simbolo retorno;
        public Entorno entorno;

        public SubPrograma(ParseTreeNode root, Entorno entorno)
        {
            this.root = root;
            parametrosValor = new Dictionary<string, Simbolo>();
            parametrosVariable = new Dictionary<string, Simbolo>();
            correlacionParametros = new Dictionary<string, string>();
            retorno = new Simbolo(Simbolo.EnumTipo.nulo, "");
            tipo = Simbolo.EnumTipo.nulo;
            this.entorno = entorno;
            ordenParametros = new ArrayList();
        }
        public void modificarEntorno()
        {
            foreach (KeyValuePair<string, Simbolo> parametro in parametrosVariable)
            {
                entorno.modificar(parametro.Key, parametro.Value);
            }
            foreach (KeyValuePair<string, Simbolo> parametro in parametrosValor)
            {
                entorno.modificar(parametro.Key, parametro.Value);
            }
        }
        public void agregarEntorno()
        {
            foreach (KeyValuePair<string, Simbolo> parametro in parametrosVariable)
            {
                entorno.insertar(parametro.Key, parametro.Value, 0, 0);
            }
            foreach (KeyValuePair<string, Simbolo> parametro in parametrosValor)
            {
                entorno.insertar(parametro.Key, parametro.Value, 0, 0);
            }
        }
        public bool modificarValor(String nombre, Simbolo simbolo)
        {
            nombre = nombre.ToLower();
            if (parametrosValor.ContainsKey(nombre))
            {
                Simbolo viejo;
                parametrosValor.TryGetValue(nombre, out viejo);
                if (viejo.tipo == simbolo.tipo)
                {
                    parametrosValor.Remove(nombre);
                    parametrosValor.Add(nombre, simbolo);
                    return true;
                }
                return false;
            }
            return false;
        }
        public bool modificarVariable(String nombre, Simbolo simbolo)
        {
            nombre = nombre.ToLower();
            if (parametrosVariable.ContainsKey(nombre))
            {
                Simbolo viejo;
                parametrosVariable.TryGetValue(nombre, out viejo);
                if (viejo.tipo == simbolo.tipo)
                {
                    parametrosVariable.Remove(nombre);
                    parametrosVariable.Add(nombre, simbolo);
                    return true;
                }
                return false;
            }
            return false;
        }
        public void modificarVariablesOriginales(Entorno entornoPrincipal)
        {
            foreach (KeyValuePair<string, Simbolo> parametro in parametrosVariable)
            {
                string nombreVariable = "";
                correlacionParametros.TryGetValue(parametro.Key, out nombreVariable);
                if (nombreVariable != null)
                {
                    Simbolo s = null;
                    entorno.tabla.TryGetValue(parametro.Key, out s);
                    entornoPrincipal.modificar(nombreVariable, s);
                }
            }
        }
        public void buscarTipo(string cadena, Entorno entorno)
        {
            switch (cadena)
            {
                case "string":
                    tipo = Simbolo.EnumTipo.cadena;
                    break;
                case "integer":
                    tipo = Simbolo.EnumTipo.entero;
                    break;
                case "real":
                    tipo = Simbolo.EnumTipo.real;
                    break;
                case "boolean":
                    tipo = Simbolo.EnumTipo.boleano;
                    break;
                default:
                    Simbolo sim = entorno.buscar(cadena);
                    if (sim.tipo == Simbolo.EnumTipo.arreglo || sim.tipo == Simbolo.EnumTipo.objeto)
                    {
                        tipo = sim.tipo;
                        break;
                    }
                    tipo = Simbolo.EnumTipo.error;
                    break;
            }
            retorno.tipo = this.tipo;
        }
    }
}
