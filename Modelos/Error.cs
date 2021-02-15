using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1_Compiladores2.Modelos
{
    class Error
    {
        public String archivo;
        public String fila;
        public String columna;
        public String tipo;
        public String error;

        public Error(String archivo, String fila, String columna, String tipo, String error)
        {
            this.archivo = archivo;
            this.fila = fila;
            this.columna = columna;
            this.tipo = tipo;
            this.error = error;
        }
    }
}
