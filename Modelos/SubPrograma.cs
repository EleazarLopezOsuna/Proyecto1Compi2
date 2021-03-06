using System;
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
        public ParseTreeNode root;
        public Simbolo.EnumTipo tipo;
        public Simbolo retorno;

        public SubPrograma(ParseTreeNode root)
        {
            this.root = root;
            parametrosValor = new Dictionary<string, Simbolo>();
            parametrosVariable = new Dictionary<string, Simbolo>();
            correlacionParametros = new Dictionary<string, string>();
            retorno = new Simbolo(Simbolo.EnumTipo.nulo, "");
            tipo = Simbolo.EnumTipo.nulo;
        }

        public void modificarVariables(Entorno entorno)
        {
            foreach (KeyValuePair<string, Simbolo> parametro in parametrosVariable)
            {
                string nombreVariable = "";
                correlacionParametros.TryGetValue(parametro.Key, out nombreVariable);
                if (nombreVariable != "")
                {
                    entorno.modificar(nombreVariable, parametro.Value);
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
                    Simbolo sim = entorno.buscar(cadena, 0, 0);
                    if (sim.tipo == Simbolo.EnumTipo.arreglo || sim.tipo == Simbolo.EnumTipo.objeto)
                    {
                        tipo = sim.tipo;
                        break;
                    }
                    tipo = Simbolo.EnumTipo.error;
                    break;
            }
        }
    }
}
