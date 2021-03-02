using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1_Compiladores2.Modelos
{
    class Simbolo
    {
        public EnumTipo tipo;
        public Object valor;
        public int fila;
        public int columna;
        public bool constante;

        public Simbolo(EnumTipo tipo, Object valor)
        {
            this.tipo = tipo;
            this.valor = valor;
            this.constante = false;
        }
        public Simbolo(EnumTipo tipo, Object valor, int fila, int columna)
        {
            this.tipo = tipo;
            this.valor = valor;
            this.fila = fila;
            this.columna = columna;
        }

        public enum EnumTipo
        {
            cadena, entero, real, boleano, nulo, error, funcion, procedimiento, objeto, arreglo
        }
    }
}
