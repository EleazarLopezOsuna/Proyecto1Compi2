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
            clase, dim1, dim2, dim3, caracter, cadena, entero, doble, boleano, nulo, error, funcion, metodo
        }
    }
}
