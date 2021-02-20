using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1_Compiladores2.Modelos
{
    class Simbolo
    {
        public EnumTipo tipo;
        public Object valor;

        public Simbolo(EnumTipo tipo, Object valor)
        {
            this.tipo = tipo;
            this.valor = valor;
        }

        public enum EnumTipo
        {
            cadena, entero, real, boleano, nulo, error, funcion, procedimiento
        }
    }
}
