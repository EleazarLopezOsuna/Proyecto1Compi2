using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1_Compiladores2.Modelos
{
    class Expresion
    {
        public Simbolo.EnumTipo tipo;
        public Object valor;

        public Expresion(Simbolo.EnumTipo tipo, Object valor)
        {
            this.tipo = tipo;
            this.valor = valor;
        }
    }
}
