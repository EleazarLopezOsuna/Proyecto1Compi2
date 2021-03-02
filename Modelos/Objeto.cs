using System;
using System.Collections.Generic;
using System.Text;

namespace Proyecto1_Compiladores2.Modelos
{
    class Objeto
    {
        public Dictionary<String, Simbolo> parametros;

        public Objeto()
        {
            parametros = new Dictionary<string, Simbolo>();
        }
    }
}
