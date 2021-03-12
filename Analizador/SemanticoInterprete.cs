using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Proyecto1_Compiladores2.Modelos;
using Irony.Parsing;
using System.Windows.Forms;

namespace Proyecto1_Compiladores2.Analizador
{
    class SemanticoInterprete
    {
        public ArrayList consola;
        public ArrayList errores;
        public ArrayList entornos;
        private Entorno entornoGlobal;
        private Expresion retornoFuncion;
        private bool parar;
        private bool continuar;
        private bool salir;
        public SemanticoInterprete()
        {

        }
        public void iniciarAnalisisSintactico(ParseTreeNode root)
        {
            consola = new ArrayList();
            errores = new ArrayList();
            entornos = new ArrayList();
            entornoGlobal = new Entorno(null, "programa");
            entornos.Add(entornoGlobal);
            retornoFuncion = null;
            parar = false;
            continuar = false;
            salir = false;
            retornoFuncion = new Expresion(Simbolo.EnumTipo.nulo, "");
            recorrer(root, entornoGlobal);
        }
        private string removerExtras(string token)
        {
            token = token.Replace(" (id)", "");
            token = token.Replace(" (Keyword)", "");
            token = token.Replace(" (Key symbol)", "");
            token = token.Replace(" (entero)", "");
            token = token.Replace(" (cadena)", "");
            token = token.Replace(" (real)", "");
            token = token.Replace(" (boleano)", "");

            return token;
        }
        private bool compararCase(Expresion comparativa, ParseTreeNode root, Entorno entorno, bool ejecutado)
        {
            if (root.ChildNodes.Count != 0)
            {
                if (root.ChildNodes[0].ToString().Equals("OPCION_CASE"))
                {
                    ejecutado = compararCase(comparativa, root.ChildNodes[0], entorno, ejecutado);
                    compararCase(comparativa, root.ChildNodes[1], entorno, ejecutado);
                }
                else
                {
                    Expresion expresion = resolverExpresion(root.ChildNodes[0].ChildNodes[0], entorno);
                    if (expresion.tipo != Simbolo.EnumTipo.error)
                    {
                        if (expresion.tipo == comparativa.tipo)
                        {
                            if (expresion.valor.ToString().Equals(comparativa.valor.ToString()))
                            {
                                if (!ejecutado)
                                {
                                    recorrer(root.ChildNodes[1], entorno);
                                    return true;
                                }
                                Error error = new Error(0, 0, "Semantico", "Case duplicado");
                                errores.Add(error);
                            }
                        }
                        else
                        {
                            Error error = new Error(0, 0, "Semantico", "Error de tipos");
                            errores.Add(error);
                            return ejecutado;
                        }
                    }
                    else
                    {
                        Error error = new Error(0, 0, "Semantico", expresion.valor.ToString());
                        errores.Add(error);
                        return ejecutado;
                    }
                }
            }
            return ejecutado;
        }
        private void ejecutarCase(ParseTreeNode root, Entorno entorno)
        {
            Expresion comparativa = resolverExpresion(root.ChildNodes[0], entorno);
            if (comparativa.tipo != Simbolo.EnumTipo.error)
            {
                if (comparativa.tipo != Simbolo.EnumTipo.arreglo && comparativa.tipo != Simbolo.EnumTipo.funcion && comparativa.tipo != Simbolo.EnumTipo.nulo && comparativa.tipo != Simbolo.EnumTipo.objeto && comparativa.tipo != Simbolo.EnumTipo.procedimiento)
                {
                    //Es de tipo primitivo, se opera
                    if (compararCase(comparativa, root.ChildNodes[1], entorno, false))
                    {
                        //Es el primer case
                    }
                    else if (!compararCase(comparativa, root.ChildNodes[2], entorno, false))
                    {
                        //No es ningun case
                        if (root.ChildNodes.Count == 4)
                        {
                            recorrer(root.ChildNodes[3], entorno);
                        }
                    }
                }
                else
                {
                    Error error = new Error(0, 0, "Semantico", "Se esperaba tipo primitivo");
                    errores.Add(error);
                }
            }
            else
            {
                Error error = new Error(0, 0, "Semantico", comparativa.valor.ToString());
                errores.Add(error);
            }
        }
        private void ejecutarIf(ParseTreeNode root, Entorno entorno)
        {
            Expresion condicion = resolverExpresion(root.ChildNodes[0], entorno);
            if (root.ChildNodes.Count == 2) //IF sentencia
            {
                if (condicion.tipo != Simbolo.EnumTipo.error)
                {
                    if (condicion.tipo == Simbolo.EnumTipo.boleano)
                    {
                        if (bool.Parse(condicion.valor.ToString()))
                        {
                            recorrer(root.ChildNodes[1], entorno);
                        }
                    }
                    else
                    {
                        Error error = new Error(0, 0, "Semantico", "Se esperaba tipo boleano");
                        errores.Add(error);
                    }
                }
                else
                {
                    Error error = new Error(0, 0, "Semantico", condicion.valor.ToString());
                    errores.Add(error);
                }
            }
            else if (root.ChildNodes.Count == 3)//IF sentencia ELSE sentencia
            {
                if (condicion.tipo != Simbolo.EnumTipo.error)
                {
                    if (condicion.tipo == Simbolo.EnumTipo.boleano)
                    {
                        if (bool.Parse(condicion.valor.ToString()))
                        {
                            recorrer(root.ChildNodes[1], entorno);
                        }
                        else
                        {
                            recorrer(root.ChildNodes[2], entorno);
                        }
                    }
                    else
                    {
                        Error error = new Error(0, 0, "Semantico", "Se esperaba tipo boleano");
                        errores.Add(error);
                    }
                }
                else
                {
                    Error error = new Error(0, 0, "Semantico", condicion.valor.ToString());
                    errores.Add(error);
                }
            }
            else
            {
                MessageBox.Show("FALTO ALGO DENTRO DEL IF");
            }
        }
        private void ejecutarRepeat(ParseTreeNode root, Entorno entorno)
        {
            Expresion expresion = resolverExpresion(root.ChildNodes[2], entorno);
            bool continuar = true;
            if (expresion.tipo != Simbolo.EnumTipo.error)
            {
                if (expresion.tipo == Simbolo.EnumTipo.boleano)
                {
                    do
                    {
                        parar = false;
                        continuar = false;
                        recorrer(root.ChildNodes[0], entorno);
                        recorrer(root.ChildNodes[1], entorno);
                        expresion = resolverExpresion(root.ChildNodes[2], entorno);
                        continuar = bool.Parse(expresion.valor.ToString());
                    } while (!continuar);
                }
                else
                {
                    Error error = new Error(0, 0, "Semantico", "Se esperaba tipo boleano");
                    errores.Add(error);
                }
            }
            else
            {
                Error error = new Error(0, 0, "Semantico", expresion.valor.ToString());
                errores.Add(error);
            }
        }
        private void ejecutarWhile(ParseTreeNode root, Entorno entorno)
        {
            Expresion expresion = resolverExpresion(root.ChildNodes[0], entorno);
            bool continuar = true;
            if (expresion.tipo != Simbolo.EnumTipo.error)
            {
                if (expresion.tipo == Simbolo.EnumTipo.boleano)
                {
                    continuar = bool.Parse(expresion.valor.ToString());
                    while (continuar)
                    {
                        parar = false;
                        continuar = false;
                        recorrer(root.ChildNodes[1], entorno);
                        expresion = resolverExpresion(root.ChildNodes[0], entorno);
                        continuar = bool.Parse(expresion.valor.ToString());
                    }
                }
                else
                {
                    Error error = new Error(0, 0, "Semantico", "Se esperaba tipo boleano");
                    errores.Add(error);
                }
            }
            else
            {
                Error error = new Error(0, 0, "Semantico", expresion.valor.ToString());
                errores.Add(error);
            }
        }
        public Simbolo buscarTipo(string cadena, Entorno entorno)
        {
            cadena = cadena.ToLower();
            switch (cadena)
            {
                case "string":
                    return new Simbolo(Simbolo.EnumTipo.cadena, "");
                case "integer":
                    return new Simbolo(Simbolo.EnumTipo.entero, 0);
                case "real":
                    return new Simbolo(Simbolo.EnumTipo.entero, 0.0);
                case "boolean":
                    return new Simbolo(Simbolo.EnumTipo.boleano, false);
                default:
                    Simbolo sim = entorno.buscar(cadena);
                    if (sim.tipo == Simbolo.EnumTipo.arreglo || sim.tipo == Simbolo.EnumTipo.objeto)
                    {
                        return new Simbolo(sim.tipo, new Objeto((Objeto)sim.valor));
                    }
                    return new Simbolo(Simbolo.EnumTipo.error, "Error");
            }
        }
        private void agregarParametros(ParseTreeNode root, SubPrograma subPrograma)
        {
            string nombreParametro;
            Simbolo nuevoSimbolo;
            ParseTreeNode temp;
            switch (root.ToString())
            {
                case "PF":
                    if (root.ChildNodes.Count != 0)
                    {
                        agregarParametros(root.ChildNodes[0], subPrograma);
                        agregarParametros(root.ChildNodes[1], subPrograma);
                    }
                    break;
                case "PFVL":
                case "PFVR":
                    nuevoSimbolo = buscarTipo(removerExtras(root.ChildNodes[2].ToString()), entornoGlobal);
                    if(nuevoSimbolo.tipo != Simbolo.EnumTipo.error)
                    {
                        //Operamos el primer parametro
                        temp = root;
                        while (temp.ChildNodes.Count != 0)
                        {
                            nombreParametro = removerExtras(temp.ChildNodes[0].ToString());
                            if (!subPrograma.ordenParametros.Contains(nombreParametro))
                            {
                                if (root.ToString().Equals("PFVL"))
                                {
                                    subPrograma.parametrosValor.Add(nombreParametro, new Simbolo(nuevoSimbolo.tipo, nuevoSimbolo.valor));
                                }
                                else
                                {
                                    subPrograma.parametrosVariable.Add(nombreParametro, new Simbolo(nuevoSimbolo.tipo, nuevoSimbolo.valor));
                                }
                                subPrograma.ordenParametros.Add(nombreParametro);
                            }
                            else
                            {
                                Error error = new Error(temp.ChildNodes[0].Token.Location.Line, temp.ChildNodes[0].Token.Location.Column, "Semantico", "El parametro ya existe");
                                errores.Add(error);
                            }
                            temp = temp.ChildNodes[1];
                        }
                    }
                    else
                    {
                        Error error = new Error(root.ChildNodes[2].Token.Location.Line, root.ChildNodes[2].Token.Location.Column, "Semantico", nuevoSimbolo.valor.ToString());
                        errores.Add(error);
                    }
                    break;
            }
        }
        private Simbolo agregarFuncion(ParseTreeNode root, Entorno entorno, Entorno nuevoEntorno)
        {
            Simbolo sim = entorno.buscar(root.ChildNodes[0].ChildNodes[0].ToString());
            if (sim is null)
            {
                SubPrograma funcion = new SubPrograma(root, nuevoEntorno);
                if (root.ChildNodes[0].ChildNodes.Count == 4)
                {
                    //Tiene parametros
                    funcion.buscarTipo(removerExtras(root.ChildNodes[0].ChildNodes[3].ToString()), entorno);
                    agregarParametros(root.ChildNodes[0].ChildNodes[1], funcion);
                    agregarParametros(root.ChildNodes[0].ChildNodes[2], funcion);
                    funcion.agregarEntorno();
                }
                else if (root.ChildNodes[0].ChildNodes.Count == 2)
                {
                    //No tiene parametros
                    funcion.buscarTipo(removerExtras(root.ChildNodes[0].ChildNodes[1].ToString()), entorno);
                }
                sim = new Simbolo(Simbolo.EnumTipo.funcion, funcion);
                //Declaramos las variables de la funcion
                recorrer(root.ChildNodes[1], nuevoEntorno);
                return sim;
            }
            else
            {
                Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", "El identificador"+ removerExtras(root.ChildNodes[0].ChildNodes[0].ToString()) +" ya existe");
                errores.Add(error);
                return new Simbolo(Simbolo.EnumTipo.error, "El nombre ya existe");
            }
        }
        private Simbolo agregarProcedimiento(ParseTreeNode root, Entorno entorno, Entorno nuevoEntorno)
        {
            Simbolo sim = entorno.buscar(root.ChildNodes[0].ChildNodes[0].ToString());
            if (sim is null)
            {
                SubPrograma procedimiento = new SubPrograma(root, nuevoEntorno);
                if (root.ChildNodes[0].ChildNodes.Count == 3)
                {
                    //Tiene parametros
                    agregarParametros(root.ChildNodes[0].ChildNodes[1], procedimiento);
                    agregarParametros(root.ChildNodes[0].ChildNodes[2], procedimiento);
                    procedimiento.agregarEntorno();
                }
                sim = new Simbolo(Simbolo.EnumTipo.procedimiento, procedimiento);
                //Declaramos las variables de la funcion
                recorrer(root.ChildNodes[1], nuevoEntorno);
                return sim;
            }
            else
            {
                Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", "El identificador " + removerExtras(root.ChildNodes[0].ChildNodes[0].ToString()) + " ya existe");
                errores.Add(error);
                return new Simbolo(Simbolo.EnumTipo.error, "El nombre ya existe");
            }
        }
        private void ejecutarLlamada(ParseTreeNode root, Entorno entorno)
        {
            string nombreSubPrograma = removerExtras(root.ChildNodes[0].ToString());
            Simbolo sim = entorno.buscar(nombreSubPrograma);
            if (!(sim is null))
            {
                if (sim.tipo == Simbolo.EnumTipo.funcion || sim.tipo == Simbolo.EnumTipo.procedimiento)
                {
                    SubPrograma subProg = (SubPrograma)sim.valor;
                    Simbolo temp = null;
                    if (sim.tipo == Simbolo.EnumTipo.funcion)
                    {
                        temp = agregarFuncion(subProg.root, entornoGlobal, new Entorno(entornoGlobal, removerExtras(root.ChildNodes[0].ToString())));
                    }
                    else
                    {
                        temp = agregarProcedimiento(subProg.root, entornoGlobal, new Entorno(entornoGlobal, removerExtras(root.ChildNodes[0].ToString())));
                    }
                    subProg = (SubPrograma)temp.valor;
                    if (root.ChildNodes.Count == 1 && subProg.ordenParametros.Count == 0)
                    {
                        subProg.modificarEntorno();
                        //sim.valor = subProg;
                        entorno.modificar(nombreSubPrograma, sim);
                        recorrer(subProg.root.ChildNodes[2], subProg.entorno);
                        subProg.modificarVariablesOriginales(entorno);
                    }
                    else
                    {
                        if (enviarParametros(root, entorno, subProg, 0))
                        {
                            subProg.modificarEntorno();
                            //sim.valor = subProg;
                            entorno.modificar(nombreSubPrograma, sim);
                            recorrer(subProg.root.ChildNodes[2], subProg.entorno);
                            subProg.modificarVariablesOriginales(entorno);
                        }
                        else
                        {
                            Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Los requerimientos del subprograma" + removerExtras(root.ChildNodes[0].ToString()) + " no se pudieron completar");
                            errores.Add(error);
                        }
                    }
                }
                else
                {
                    Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba funcion o procedimiento, se encontro " + sim.tipo);
                    errores.Add(error);
                }
            }
            else
            {
                Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "No se pudo encontrar el subprograma " + removerExtras(root.ChildNodes[0].ToString()));
                errores.Add(error);
            }
        }
        private void ejecutarFor(ParseTreeNode root, Entorno entorno)
        {
            Expresion posicionFinal = resolverExpresion(root.ChildNodes[1].ChildNodes[0], entorno);
            bool tipo = root.ChildNodes[1].ToString().Equals("ARRIBA") ? true : false;
            if (posicionFinal.tipo != Simbolo.EnumTipo.error)
            {
                if (posicionFinal.tipo == Simbolo.EnumTipo.entero)
                {
                    Expresion expresion = buscarVariable(root.ChildNodes[0].ChildNodes[0].ChildNodes[0], entorno);
                    if (expresion.tipo == Simbolo.EnumTipo.entero)
                    {
                        recorrer(root.ChildNodes[0], entorno);
                        expresion = buscarVariable(root.ChildNodes[0].ChildNodes[0].ChildNodes[0], entorno);
                        int iterador = int.Parse(expresion.valor.ToString());
                        int final = int.Parse(posicionFinal.valor.ToString());
                        if (tipo)
                        {
                            for (int i = iterador; i <= final; i++)
                            {
                                parar = false;
                                continuar = false;
                                entorno.modificar(removerExtras(root.ChildNodes[0].ChildNodes[0].ChildNodes[0].ToString()), new Simbolo(Simbolo.EnumTipo.entero, i));
                                recorrer(root.ChildNodes[2], entorno);
                            }
                        }
                        else
                        {
                            for (int i = iterador; i >= final; i--)
                            {
                                parar = false;
                                continuar = false;
                                salir = false;
                                entorno.modificar(removerExtras(root.ChildNodes[0].ChildNodes[0].ChildNodes[0].ToString()), new Simbolo(Simbolo.EnumTipo.entero, i));
                                recorrer(root.ChildNodes[2], entorno);
                            }
                        }
                    }
                    else
                    {
                        Error error = new Error(root.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba tipo entero");
                        errores.Add(error);
                    }
                }
                else
                {
                    Error error = new Error(root.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba tipo entero");
                    errores.Add(error);
                }
            }
            else
            {
                Error error = new Error(root.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", posicionFinal.valor.ToString());
                errores.Add(error);
            }
        }
        private bool verificarRango(Expresion exp1, Expresion exp2)
        {
            Error error;
            if (exp1.tipo != Simbolo.EnumTipo.error)
            {
                if (exp2.tipo != Simbolo.EnumTipo.error)
                {
                    if (exp1.tipo == Simbolo.EnumTipo.entero)
                    {
                        if (exp2.tipo == Simbolo.EnumTipo.entero)
                        {
                            if (int.Parse(exp1.valor.ToString()) < int.Parse(exp2.valor.ToString()))
                            {
                                return true;
                            }
                            error = new Error(0, 0, "Semantico", "El operando izquiero debe ser menor al derecho");
                            errores.Add(error);
                            return false;
                        }
                        error = new Error(0, 0, "Semantico", "El segundo operando debe ser de tipo entero");
                        errores.Add(error);
                        return false;
                    }
                    error = new Error(0, 0, "Semantico", "El primer operando debe ser de tipo entero");
                    errores.Add(error);
                    return false;
                }
                error = new Error(0, 0, "Semantico", exp2.valor.ToString());
                errores.Add(error);
                return false;
            }
            error = new Error(0, 0, "Semantico", exp1.valor.ToString());
            errores.Add(error);
            return false;
        }
        private Expresion resolverEstructura(ParseTreeNode root, Object objetoPadre, Entorno ent)
        {
            if (root.ChildNodes.Count != 0)
            {
                Objeto obj = (Objeto)objetoPadre;
                //Se busca el parametro
                Simbolo sim = obj.buscar(removerExtras(root.ChildNodes[0].ToString()));
                if (sim != null) //El parametro si existe
                {
                    if (sim.tipo == Simbolo.EnumTipo.objeto)
                    {
                        if (root.ChildNodes.Count == 2) //campo.campo
                        {
                            //Verificamos si es el ultimo campo
                            if (root.ChildNodes[1].ChildNodes.Count != 0)
                            {
                                return resolverEstructura(root.ChildNodes[1], sim.valor, ent);
                            }
                            return new Expresion(sim.tipo, sim.valor);
                        }
                        else
                        {
                            Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba un objeto, se encontro un arreglo");
                            errores.Add(error);
                        }
                    }
                    else if (sim.tipo == Simbolo.EnumTipo.arreglo)
                    {
                        if (root.ChildNodes.Count == 2)
                        {
                            Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba un arreglo, se encontro un objeto");
                            errores.Add(error);
                        }
                        else //campo[index{, index}].campo o campo[index{, index}]
                        {
                            //Se busca el index
                            Expresion indice = resolverExpresion(root.ChildNodes[1], ent);
                            if (indice.tipo != Simbolo.EnumTipo.error)
                            {
                                if (indice.tipo == Simbolo.EnumTipo.entero)
                                {
                                    int index1 = int.Parse(indice.valor.ToString());
                                    if (root.ChildNodes[2].ChildNodes.Count == 0)
                                    {
                                        //Se espera que simbolo sea un arreglo de 1 dimension
                                        Array tempo = (Array)((Objeto)sim.valor).arreglo;
                                        if (tempo.Rank == 1)
                                        {
                                            if (index1 < tempo.Length)
                                            {
                                                //Verificamos si es el ultimo campo
                                                if (root.ChildNodes[3].ChildNodes.Count != 0)
                                                {
                                                    return resolverEstructura(root.ChildNodes[3], tempo.GetValue(index1), ent);
                                                }
                                                sim = (Simbolo)tempo.GetValue(index1);
                                                return new Expresion(sim.tipo, sim.valor);
                                            }
                                            else
                                            {
                                                Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "indice fuera del limite");
                                                errores.Add(error);
                                            }
                                            MessageBox.Show("21");
                                        }
                                        else
                                        {
                                            Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba arreglo de 1 dimension");
                                            errores.Add(error);
                                            MessageBox.Show("20");
                                        }
                                        MessageBox.Show("19");
                                    }
                                    else
                                    {
                                        //Se espera que simbolo sea un arreglo de 2 (podrian ser mas pero solo puedo operar 2 dimensiones)
                                        Array tempo = (Array)((Objeto)sim.valor).arreglo;
                                        if (tempo.Rank != 1)
                                        {
                                            Expresion indice2 = resolverExpresion(root.ChildNodes[2].ChildNodes[0], ent);
                                            if (indice2.tipo != Simbolo.EnumTipo.error)
                                            {
                                                if (indice2.tipo == Simbolo.EnumTipo.entero)
                                                {
                                                    int index2 = int.Parse(indice2.valor.ToString());
                                                    if (index1 < tempo.GetLength(0))
                                                    {
                                                        if (index2 < tempo.GetLength(1))
                                                        {
                                                            //Verificamos si es el ultimo campo
                                                            if (root.ChildNodes[3].ChildNodes.Count != 0)
                                                            {
                                                                return resolverEstructura(root.ChildNodes[3], tempo.GetValue(index1, index2), ent);
                                                            }
                                                            sim = (Simbolo)tempo.GetValue(index1, index2);
                                                            return new Expresion(sim.tipo, sim.valor);
                                                        }
                                                        else
                                                        {
                                                            Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Indice fuera del limite");
                                                            errores.Add(error);
                                                            MessageBox.Show("18");
                                                        }
                                                        MessageBox.Show("17");
                                                    }
                                                    else
                                                    {
                                                        Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Indice fuera del limite");
                                                        errores.Add(error);
                                                        MessageBox.Show("16");
                                                    }
                                                    MessageBox.Show("15");
                                                }
                                                else
                                                {
                                                    Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Error de tipos");
                                                    errores.Add(error);
                                                    MessageBox.Show("14");
                                                }
                                                MessageBox.Show("13");
                                            }
                                            else
                                            {
                                                Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", indice2.valor.ToString());
                                                errores.Add(error);
                                                MessageBox.Show("12");
                                            }
                                            MessageBox.Show("11");
                                        }
                                        else
                                        {
                                            Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba arreglo de 2 dimensiones");
                                            errores.Add(error);
                                            MessageBox.Show("10");
                                        }
                                    }
                                    MessageBox.Show("9");
                                }
                                else
                                {
                                    Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Error de tipos");
                                    errores.Add(error);
                                    MessageBox.Show("8");
                                }
                                MessageBox.Show("7");
                            }
                            else
                            {
                                Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", indice.valor.ToString());
                                errores.Add(error);
                                MessageBox.Show("6");
                            }
                            MessageBox.Show("5");
                        }
                        MessageBox.Show("4");
                    }
                    return new Expresion(sim.tipo, sim.valor);
                }
                else
                {
                    Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "La variable no existe");
                    errores.Add(error);
                    MessageBox.Show("2");
                }
                MessageBox.Show("1");
            }
            return new Expresion(Simbolo.EnumTipo.entero, "ERROR DESCONOCIDO EN resolverEstructura");
        }
        private Expresion resolverExpresion(ParseTreeNode root, Entorno ent)
        {
            Expresion tmp;
            Simbolo sim;
            switch (root.ToString())
            {
                case "EXPRESION":
                    if (root.ChildNodes.Count == 3)//Operador binario
                    {
                        if (root.ChildNodes[1].ToString().Contains("and"))
                        {
                            return operarAnd(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains(">") && root.ChildNodes[1].ToString().Contains("="))
                        {
                            return operarMayorIgual(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains("<") && root.ChildNodes[1].ToString().Contains("="))
                        {
                            return operarMenorIgual(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains("="))
                        {
                            return operarIgual(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains("<>"))
                        {
                            return operarDesigual(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains(">"))
                        {
                            return operarMayor(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains("<"))
                        {
                            return operarMenor(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains("+"))
                        {
                            return operarSuma(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains("-"))
                        {
                            return operarResta(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains("*"))
                        {
                            return operarMultiplicacion(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains("/"))
                        {
                            return operarDivision(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains("%"))
                        {
                            return operarModulo(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains("or"))
                        {
                            return operarOr(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                    }
                    else if (root.ChildNodes.Count == 2)//Operador unario
                    {
                        if (root.ChildNodes[0].ToString().Contains("not"))
                        {
                            return operarNot(resolverExpresion(root.ChildNodes[1], ent));
                        }
                        else if (root.ChildNodes[0].ToString().Contains("-"))
                        {
                            return operarNegativo(resolverExpresion(root.ChildNodes[1], ent));
                        }
                    }
                    return resolverExpresion(root.ChildNodes[0], ent);
                case "ESTRUCTURA":
                    if (root.ChildNodes[0].ToString().Equals("LLAMADA"))
                    {
                        return resolverExpresion(root.ChildNodes[0], ent);
                    }
                    else
                    {
                        if (root.ChildNodes.Count != 0)
                        {
                            //Se busca la variable
                            tmp = buscarVariable(root.ChildNodes[0], ent);
                            sim = new Simbolo(tmp.tipo, tmp.valor);
                            if (sim != null) //La variable si existe
                            {
                                if (sim.tipo == Simbolo.EnumTipo.objeto)
                                {
                                    if (root.ChildNodes.Count == 2) //identificador.campo
                                    {
                                        return resolverEstructura(root.ChildNodes[1], tmp.valor, ent);
                                    }
                                    else //identificador[index].campo o identificador[index]
                                    {
                                        Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba objeto, se encontro arreglo");
                                        errores.Add(error);
                                    }
                                }
                                else if (sim.tipo == Simbolo.EnumTipo.arreglo)
                                {
                                    if (root.ChildNodes.Count == 2) //identificador.campo
                                    {
                                        Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba arreglo, se encontro objeto");
                                        errores.Add(error);
                                    }
                                    else //identificador[index{, index}].campo o identificador[index{, index}]
                                    {
                                        //Se busca el index
                                        Expresion indice = resolverExpresion(root.ChildNodes[1], ent);
                                        if (indice.tipo != Simbolo.EnumTipo.error)
                                        {
                                            if (indice.tipo == Simbolo.EnumTipo.entero)
                                            {
                                                int index1 = int.Parse(indice.valor.ToString());
                                                if (root.ChildNodes[2].ChildNodes.Count == 0)
                                                {
                                                    //Se espera que simbolo sea un arreglo de 1 dimension
                                                    Array tempo = (Array)((Objeto)sim.valor).arreglo;
                                                    if (tempo.Rank == 1)
                                                    {
                                                        if (index1 < tempo.Length)
                                                        {
                                                            //Verificamos si es el ultimo campo
                                                            if (root.ChildNodes[3].ChildNodes.Count != 0)
                                                            {
                                                                return resolverEstructura(root.ChildNodes[3], tempo.GetValue(index1), ent);
                                                            }
                                                            if (typeof(Objeto).IsInstanceOfType(tempo.GetValue(index1)))
                                                            {
                                                                sim = new Simbolo(Simbolo.EnumTipo.objeto, tempo.GetValue(index1));
                                                            }
                                                            else
                                                            {
                                                                sim = (Simbolo)tempo.GetValue(index1);
                                                            }
                                                            return new Expresion(sim.tipo, sim.valor);
                                                        }
                                                        else
                                                        {
                                                            Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Indice fuera del limite");
                                                            errores.Add(error);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba arreglo de 1 dimension");
                                                        errores.Add(error);
                                                    }
                                                }
                                                else
                                                {
                                                    //Se espera que simbolo sea un arreglo de 2 (podrian ser mas pero solo puedo operar 2 dimensiones)
                                                    Array tempo = (Array)((Objeto)sim.valor).arreglo;
                                                    if (tempo.Rank != 1)
                                                    {
                                                        Expresion indice2 = resolverExpresion(root.ChildNodes[2].ChildNodes[0], ent);
                                                        if (indice2.tipo != Simbolo.EnumTipo.error)
                                                        {
                                                            if (indice2.tipo == Simbolo.EnumTipo.entero)
                                                            {
                                                                int index2 = int.Parse(indice2.valor.ToString());
                                                                if (index1 < tempo.GetLength(0))
                                                                {
                                                                    if (index2 < tempo.GetLength(1))
                                                                    {
                                                                        //Verificamos si es el ultimo campo
                                                                        if (root.ChildNodes[3].ChildNodes.Count != 0)
                                                                        {
                                                                            return resolverEstructura(root.ChildNodes[3], tempo.GetValue(index1, index2), ent);
                                                                        }
                                                                        sim = (Simbolo)tempo.GetValue(index1, index2);
                                                                        return new Expresion(sim.tipo, sim.valor);
                                                                    }
                                                                    else
                                                                    {
                                                                        Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Indice fuera del limite");
                                                                        errores.Add(error);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Indice fuera del limite");
                                                                    errores.Add(error);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Error de tipos");
                                                                errores.Add(error);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", indice2.valor.ToString());
                                                            errores.Add(error);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba arreglo de 2 dimensiones");
                                                        errores.Add(error);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Error de tipos");
                                                errores.Add(error);
                                            }
                                        }
                                        else
                                        {
                                            Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", indice.valor.ToString());
                                            errores.Add(error);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "La variable " + removerExtras(root.ChildNodes[0].ToString()) + " no existe");
                                errores.Add(error);
                            }
                        }
                    }
                    return new Expresion(Simbolo.EnumTipo.error, "ERROR EN Z_TIPOS");
                case "LLAMADA":
                    return resolverLlamada(root, ent);
                case "VARIABLE":
                    return buscarVariable(root.ChildNodes[0], ent);
                default:
                    if (root.ToString().Contains("cadena"))
                    {
                        return new Expresion(Simbolo.EnumTipo.cadena, root.ToString().Replace(" (cadena)", ""));
                    }
                    else if (root.ToString().Contains("entero"))
                    {
                        return new Expresion(Simbolo.EnumTipo.entero, root.ToString().Replace(" (entero)", ""));
                    }
                    else if (root.ToString().Contains("real"))
                    {
                        return new Expresion(Simbolo.EnumTipo.real, root.ToString().Replace(" (real)", ""));
                    }
                    else if (root.ToString().Contains("boleano"))
                    {
                        return new Expresion(Simbolo.EnumTipo.boleano, root.ToString().Replace(" (boleano)", ""));
                    }
                    break;
            }
            return new Expresion(Simbolo.EnumTipo.error, "ERROR");
        }
        private Expresion operarNegativo(Expresion expresion1)
        {
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.real:
                    return new Expresion(Simbolo.EnumTipo.real, 0 - double.Parse(expresion1.valor.ToString()));
                case Simbolo.EnumTipo.entero:
                    return new Expresion(Simbolo.EnumTipo.entero, 0 - int.Parse(expresion1.valor.ToString()));
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Negativo no definido para el tipo " + expresion1.tipo);
            }
        }
        private Expresion resolverLlamada(ParseTreeNode root, Entorno entorno)
        {
            if (root.ChildNodes[0].ToString().ToLower().Contains("writeln") || root.ChildNodes[0].ToString().ToLower().Contains("write") || root.ChildNodes[0].ToString().ToLower().Contains("break"))
            {
                return new Expresion(Simbolo.EnumTipo.error, "Las funciones nativas no pueden ser llamadas desde expresiones");
            }
            else if (root.ChildNodes[0].ToString().ToLower().Contains("continue") || root.ChildNodes[0].ToString().ToLower().Contains("graficar_ts") || root.ChildNodes[0].ToString().ToLower().Contains("exit"))
            {

            }
            else
            {
                //Buscamos el nombre de la funcion
                Simbolo sim = entorno.buscar(removerExtras(root.ChildNodes[0].ToString()));
                if (!(sim is null))
                {
                    if (sim.tipo == Simbolo.EnumTipo.funcion)
                    {
                        ejecutarLlamada(root, entorno);
                        salir = false;
                        return retornoFuncion;
                    }
                    else
                    {
                        Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba funcion, se encontro " + sim.tipo);
                        errores.Add(error);
                    }
                }
                return new Expresion(Simbolo.EnumTipo.error, "No se encontro la funcion");
            }
            return new Expresion(Simbolo.EnumTipo.error, "Error desconocido");
        }
        private Expresion buscarVariable(ParseTreeNode root, Entorno entorno)
        {
            Simbolo resultadoBusqueda = entorno.buscar(removerExtras(root.ToString()));
            if (resultadoBusqueda is null)
            {
                return new Expresion(Simbolo.EnumTipo.error, "Variable no encontrada");
            }
            else
            {
                return new Expresion(resultadoBusqueda.tipo, resultadoBusqueda.valor);
            }
        }
        private Expresion operarSuma(Expresion expresion1, Expresion expresion2)
        {
            switch (expresion1.tipo)
            {
                /*case Simbolo.EnumTipo.cadena:
                    //Cadena (Entero || real || Caracter || boleano || Cadena)
                    return new Expresion(Simbolo.EnumTipo.cadena, expresion1.valor.ToString() + expresion2.valor.ToString());*/
                case Simbolo.EnumTipo.real:
                    switch (expresion2.tipo)
                    {
                        /*case Simbolo.EnumTipo.cadena:
                            //real Cadena
                            return new Expresion(Simbolo.EnumTipo.cadena, expresion1.valor.ToString() + expresion2.valor.ToString());*/
                        case Simbolo.EnumTipo.entero:
                        case Simbolo.EnumTipo.real:
                            //real Entero
                            return new Expresion(Simbolo.EnumTipo.real, Double.Parse(expresion1.valor.ToString()) + Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Suma no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.entero, int.Parse(expresion1.valor.ToString()) + int.Parse(expresion2.valor.ToString()));
                        /*case Simbolo.EnumTipo.cadena:
                            //Entero Cadena
                            return new Expresion(Simbolo.EnumTipo.cadena, expresion1.valor.ToString() + expresion2.valor.ToString());*/
                        case Simbolo.EnumTipo.real:
                            //Entero real
                            return new Expresion(Simbolo.EnumTipo.real, Double.Parse(expresion1.valor.ToString()) + Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Suma no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Suma no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }
        private Expresion operarResta(Expresion expresion1, Expresion expresion2)
        {
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.real:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                        case Simbolo.EnumTipo.real:
                            //real Entero
                            return new Expresion(Simbolo.EnumTipo.real, Double.Parse(expresion1.valor.ToString()) - Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Resta no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.entero, int.Parse(expresion1.valor.ToString()) - int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //Entero real
                            return new Expresion(Simbolo.EnumTipo.real, Double.Parse(expresion1.valor.ToString()) - Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Resta no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Resta no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }
        private Expresion operarMultiplicacion(Expresion expresion1, Expresion expresion2)
        {
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.real:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                        case Simbolo.EnumTipo.real:
                            //real Entero
                            return new Expresion(Simbolo.EnumTipo.real, Double.Parse(expresion1.valor.ToString()) * Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Multiplicacion no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.entero, int.Parse(expresion1.valor.ToString()) * int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //Entero real
                            return new Expresion(Simbolo.EnumTipo.real, Double.Parse(expresion1.valor.ToString()) * Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Multiplicacion no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Multiplicacion no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }
        private Expresion operarDivision(Expresion expresion1, Expresion expresion2)
        {
            if (expresion2.valor.ToString().Equals("0") || expresion2.valor.ToString().Equals("0.0"))
            {
                return new Expresion(Simbolo.EnumTipo.error, "Division por 0 indefinida");
            }
            else
            {
                switch (expresion1.tipo)
                {
                    case Simbolo.EnumTipo.real:
                        switch (expresion2.tipo)
                        {
                            case Simbolo.EnumTipo.entero:
                            case Simbolo.EnumTipo.real:
                                //real Entero
                                return new Expresion(Simbolo.EnumTipo.real, Double.Parse(expresion1.valor.ToString()) / Double.Parse(expresion2.valor.ToString()));
                            case Simbolo.EnumTipo.error:
                                return expresion2;
                            default:
                                return new Expresion(Simbolo.EnumTipo.error, "Division no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                        }
                    case Simbolo.EnumTipo.entero:
                        switch (expresion2.tipo)
                        {
                            case Simbolo.EnumTipo.entero:
                                //Entero Entero
                                return new Expresion(Simbolo.EnumTipo.real, Double.Parse(expresion1.valor.ToString()) / Double.Parse(expresion2.valor.ToString()));
                            case Simbolo.EnumTipo.real:
                                //Entero real
                                return new Expresion(Simbolo.EnumTipo.real, Double.Parse(expresion1.valor.ToString()) / Double.Parse(expresion2.valor.ToString()));
                            case Simbolo.EnumTipo.error:
                                return expresion2;
                            default:
                                return new Expresion(Simbolo.EnumTipo.error, "Division no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                        }
                    case Simbolo.EnumTipo.error:
                        return expresion1;
                    default:
                        return new Expresion(Simbolo.EnumTipo.error, "Division no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                }
            }
        }
        private Expresion operarModulo(Expresion expresion1, Expresion expresion2)
        {
            if (expresion2.valor.ToString().Equals("0") || expresion2.valor.ToString().Equals("0.0"))
            {
                return new Expresion(Simbolo.EnumTipo.error, "Modulo 0 indefinido");
            }
            else
            {
                switch (expresion1.tipo)
                {
                    case Simbolo.EnumTipo.real:
                        switch (expresion2.tipo)
                        {
                            case Simbolo.EnumTipo.entero:
                            case Simbolo.EnumTipo.real:
                                return new Expresion(Simbolo.EnumTipo.real, Double.Parse(expresion1.valor.ToString()) % Double.Parse(expresion2.valor.ToString()));
                            case Simbolo.EnumTipo.error:
                                return expresion2;
                            default:
                                return new Expresion(Simbolo.EnumTipo.error, "Modulo no definido entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                        }
                    case Simbolo.EnumTipo.entero:
                        switch (expresion2.tipo)
                        {
                            case Simbolo.EnumTipo.entero:
                                //Entero Entero
                                return new Expresion(Simbolo.EnumTipo.real, Double.Parse(expresion1.valor.ToString()) % Double.Parse(expresion2.valor.ToString()));
                            case Simbolo.EnumTipo.real:
                                //Entero real
                                return new Expresion(Simbolo.EnumTipo.real, Double.Parse(expresion1.valor.ToString()) % Double.Parse(expresion2.valor.ToString()));
                            case Simbolo.EnumTipo.error:
                                return expresion2;
                            default:
                                return new Expresion(Simbolo.EnumTipo.error, "Modulo no definido entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                        }
                    case Simbolo.EnumTipo.error:
                        return expresion1;
                    default:
                        return new Expresion(Simbolo.EnumTipo.error, "Division no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                }
            }
        }
        private Expresion operarAnd(Expresion expresion1, Expresion expresion2)
        {
            bool resultado1;
            bool resultado2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.boleano:
                    if (expresion1.valor.ToString().ToLower().Equals("true"))
                    {
                        resultado1 = true;
                    }
                    else
                    {
                        resultado1 = false;
                    }
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.boleano:
                            if (expresion2.valor.ToString().ToLower().Equals("true"))
                            {
                                resultado2 = true;
                            }
                            else
                            {
                                resultado2 = false;
                            }
                            return new Expresion(Simbolo.EnumTipo.boleano, resultado1 && resultado2);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion AND no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion AND no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }
        private Expresion operarOr(Expresion expresion1, Expresion expresion2)
        {
            bool resultado1;
            bool resultado2;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.boleano:
                    if (expresion1.valor.ToString().ToLower().Equals("true"))
                    {
                        resultado1 = true;
                    }
                    else
                    {
                        resultado1 = false;
                    }
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.boleano:
                            if (expresion2.valor.ToString().ToLower().Equals("true"))
                            {
                                resultado2 = true;
                            }
                            else
                            {
                                resultado2 = false;
                            }
                            return new Expresion(Simbolo.EnumTipo.boleano, resultado1 || resultado2);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion OR no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion OR no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }
        private Expresion operarNot(Expresion expresion1)
        {
            bool resultado1;
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.boleano:
                    if (expresion1.valor.ToString().ToLower().Equals("true"))
                    {
                        resultado1 = true;
                    }
                    else
                    {
                        resultado1 = false;
                    }
                    return new Expresion(Simbolo.EnumTipo.boleano, !resultado1);
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion NOT no definida para el tipo " + expresion1.tipo);
            }
        }
        private Expresion operarMayor(Expresion expresion1, Expresion expresion2)
        {
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.real:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) > int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //real Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) > Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) > int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //Entero real
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) > Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }
        private Expresion operarMenor(Expresion expresion1, Expresion expresion2)
        {
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.real:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) < int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //real Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) < Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion menor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) < int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //Entero real
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) < Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion menor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion menor no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }
        private Expresion operarMayorIgual(Expresion expresion1, Expresion expresion2)
        {
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.real:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) >= Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) >= Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) >= int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) >= Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion mayor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }
        private Expresion operarMenorIgual(Expresion expresion1, Expresion expresion2)
        {
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.real:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) <= Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) <= Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion menor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) <= int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) <= Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion menor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion menor o igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }
        private Expresion operarIgual(Expresion expresion1, Expresion expresion2)
        {
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.boleano:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.boleano:
                            return new Expresion(Simbolo.EnumTipo.boleano, expresion1.valor.ToString().ToLower().Equals(expresion2.valor.ToString().ToLower()));
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.cadena:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.cadena:
                            return new Expresion(Simbolo.EnumTipo.boleano, expresion1.valor.ToString().Equals(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, false);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.real:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) == int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //real Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) == Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, false);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) == int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //Entero real
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) == Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, false);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                case Simbolo.EnumTipo.nulo:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, true);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.boleano, false);
                    }
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }
        private Expresion operarDesigual(Expresion expresion1, Expresion expresion2)
        {
            switch (expresion1.tipo)
            {
                case Simbolo.EnumTipo.boleano:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.boleano:
                            return new Expresion(Simbolo.EnumTipo.boleano, !expresion1.valor.ToString().ToLower().Equals(expresion2.valor.ToString().ToLower()));
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion igual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.cadena:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.cadena:
                            return new Expresion(Simbolo.EnumTipo.boleano, !expresion1.valor.ToString().Equals(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, true);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion desigual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.real:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) != int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //real Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) != Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, true);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion desigual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.entero:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.entero:
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) != int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //Entero real
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) != Double.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, true);
                        default:
                            return new Expresion(Simbolo.EnumTipo.error, "Operacion desigual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
                    }
                case Simbolo.EnumTipo.error:
                    return expresion1;
                case Simbolo.EnumTipo.nulo:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.nulo:
                            return new Expresion(Simbolo.EnumTipo.boleano, false);
                        case Simbolo.EnumTipo.error:
                            return expresion2;
                        default:
                            return new Expresion(Simbolo.EnumTipo.boleano, true);
                    }
                default:
                    return new Expresion(Simbolo.EnumTipo.error, "Operacion desigual no definida entre los tipos " + expresion1.tipo + " y " + expresion2.tipo);
            }
        }
        private void agregarCamposObjeto(ParseTreeNode root, Objeto objeto, Entorno entorno)
        {
            Simbolo simbolo = null;
            if (root.ChildNodes.Count == 4)
            {
                if (root.ChildNodes[1].ToString().Contains("real"))
                {
                    simbolo = new Simbolo(Simbolo.EnumTipo.real, 0.0);
                }
                else if (root.ChildNodes[1].ToString().Contains("boolean"))
                {
                    simbolo = new Simbolo(Simbolo.EnumTipo.boleano, false);
                }
                else if (root.ChildNodes[1].ToString().Contains("integer"))
                {
                    simbolo = new Simbolo(Simbolo.EnumTipo.entero, 0);
                }
                else if (root.ChildNodes[1].ToString().Contains("string"))
                {
                    simbolo = new Simbolo(Simbolo.EnumTipo.cadena, "");
                }
                else if (root.ChildNodes[1].ToString().Contains("id"))
                {
                    Expresion tmp = buscarVariable(root.ChildNodes[1].ChildNodes[0], entorno);
                    if (tmp.tipo == Simbolo.EnumTipo.arreglo || tmp.tipo == Simbolo.EnumTipo.objeto)
                    {
                        simbolo = new Simbolo(tmp.tipo, tmp.valor);
                    }
                    else if (tmp.tipo == Simbolo.EnumTipo.error)
                    {
                        Error error = new Error(root.ChildNodes[1].ChildNodes[0].Token.Location.Line, root.ChildNodes[1].ChildNodes[0].Token.Location.Column, "Semantico", tmp.valor.ToString());
                        errores.Add(error);
                    }
                    else
                    {
                        Error error = new Error(root.ChildNodes[1].ChildNodes[0].Token.Location.Line, root.ChildNodes[1].ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba arreglo u objeto, se encontro " + removerExtras(root.ChildNodes[1].ChildNodes[0].ToString()));
                        errores.Add(error);
                    }
                }
                Expresion exp = resolverExpresion(root.ChildNodes[3], entorno);
                if (exp.tipo != Simbolo.EnumTipo.error)
                {
                    if (exp.tipo == simbolo.tipo)
                    {
                        simbolo.valor = exp.valor;
                        simbolo.constante = true;
                        objeto.parametros.Add(removerExtras(root.ChildNodes[0].ToString()), simbolo);
                    }
                    else
                    {
                        Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba tipo " + simbolo.tipo + " se encontro " + exp.tipo);
                        errores.Add(error);
                        simbolo.tipo = Simbolo.EnumTipo.error;
                    }
                }
                else
                {
                    Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", exp.valor.ToString());
                    errores.Add(error);
                }
            }
            else if (root.ChildNodes.Count == 3)
            {
                if (root.ChildNodes[2].ToString().Contains("real"))
                {
                    simbolo = new Simbolo(Simbolo.EnumTipo.real, 0.0);
                }
                else if (root.ChildNodes[2].ToString().Contains("boolean"))
                {
                    simbolo = new Simbolo(Simbolo.EnumTipo.boleano, false);
                }
                else if (root.ChildNodes[2].ToString().Contains("integer"))
                {
                    simbolo = new Simbolo(Simbolo.EnumTipo.entero, 0);
                }
                else if (root.ChildNodes[2].ToString().Contains("string"))
                {
                    simbolo = new Simbolo(Simbolo.EnumTipo.cadena, "");
                }
                else if (root.ChildNodes[2].ToString().Contains("id"))
                {
                    Expresion tmp = buscarVariable(root.ChildNodes[2], entorno);
                    if (tmp.tipo == Simbolo.EnumTipo.arreglo || tmp.tipo == Simbolo.EnumTipo.objeto)
                    {
                        simbolo = new Simbolo(tmp.tipo, tmp.valor);
                    }
                    else
                    {
                        simbolo = new Simbolo(tmp.tipo, tmp.valor);
                    }
                }
                ParseTreeNode temp = root;
                while (temp.ChildNodes.Count != 0)
                {
                    objeto.parametros.Add(removerExtras(temp.ChildNodes[0].ToString()), simbolo);
                    temp = temp.ChildNodes[1];
                }
            }
            else if (root.ChildNodes.Count == 2)
            {
                agregarCamposObjeto(root.ChildNodes[0], objeto, entorno);
                agregarCamposObjeto(root.ChildNodes[1], objeto, entorno);
            }
        }
        private void ejecutarWriteLn(ParseTreeNode root, Entorno entorno)
        {
            if (root.ChildNodes.Count > 1)
            {
                string cadena = "";
                Expresion exp = null;
                exp = resolverExpresion(root.ChildNodes[1], entorno);
                if (exp.tipo != Simbolo.EnumTipo.error)
                {
                    cadena += exp.valor.ToString();
                }
                ParseTreeNode tmp = root.ChildNodes[2];
                while (tmp.ChildNodes.Count != 0)
                {
                    exp = resolverExpresion(tmp.ChildNodes[0], entorno);
                    if (exp.tipo != Simbolo.EnumTipo.error)
                    {
                        cadena += exp.valor.ToString();
                    }
                    tmp = tmp.ChildNodes[1];
                }
                consola.Add(cadena + "\n");
            }
            else
            {
                consola.Add("\n");
            }
        }
        private void ejecutarWrite(ParseTreeNode root, Entorno entorno)
        {
            if (root.ChildNodes.Count > 1)
            {
                string cadena = "";
                Expresion exp = null;
                exp = resolverExpresion(root.ChildNodes[1], entorno);
                if (exp.tipo != Simbolo.EnumTipo.error)
                {
                    cadena += exp.valor.ToString();
                }
                ParseTreeNode tmp = root.ChildNodes[2];
                while (tmp.ChildNodes.Count != 0)
                {
                    exp = resolverExpresion(tmp.ChildNodes[0], entorno);
                    if (exp.tipo != Simbolo.EnumTipo.error)
                    {
                        cadena += exp.valor.ToString();
                    }
                    tmp = tmp.ChildNodes[1];
                }
                consola.Add(cadena);
            }
        }
        private void ejecutarExit(ParseTreeNode root, Entorno entorno)
        {
            if (root.ChildNodes.Count == 1)
            {
                salir = true;
            }
            else
            {
                if (root.ChildNodes[2].ChildNodes.Count == 0)
                {
                    retornoFuncion = resolverExpresion(root.ChildNodes[1], entorno);
                    salir = true;
                }
                else
                {
                    Error error = new Error(root.Token.Location.Line, root.Token.Location.Column, "Semantico", "La funcion exit debe tener solo 1 parametro");
                    errores.Add(error);
                }
            }
        }
        private void ejecutarGraficarTS()
        {

        }
        private bool enviarParametros(ParseTreeNode root, Entorno entorno, SubPrograma subPrograma, int contador)
        {
            Error error;
            if (contador == 0)
            {
                Expresion expresion = resolverExpresion(root.ChildNodes[1], entorno);
                Simbolo simbolo = null;
                if (expresion.tipo != Simbolo.EnumTipo.error)
                {
                    if (expresion.tipo != Simbolo.EnumTipo.funcion && expresion.tipo != Simbolo.EnumTipo.procedimiento && expresion.tipo != Simbolo.EnumTipo.nulo)
                    {
                        if (contador < subPrograma.ordenParametros.Count)
                        {
                            string nombreParametro = subPrograma.ordenParametros[contador].ToString();
                            subPrograma.parametrosValor.TryGetValue(nombreParametro, out simbolo);
                            if (simbolo != null)
                            {
                                //Es un parametro por valor
                                subPrograma.modificarValor(nombreParametro, new Simbolo(expresion.tipo, expresion.valor));
                                if (root.ChildNodes[2].ChildNodes.Count != 0)
                                {
                                    contador++;
                                    return enviarParametros(root.ChildNodes[2], entorno, subPrograma, contador);
                                }
                                else
                                {
                                    if (contador == (subPrograma.ordenParametros.Count - 1))
                                    {
                                        return true;
                                    }
                                    error = new Error(0, 0, "Semantico", "Faltan parametros");
                                    errores.Add(error);
                                    MessageBox.Show("faltan parametros  ");
                                    return false;
                                }
                            }
                            else
                            {
                                //Es un parametro por variable, se espera que el valor sea una variable, no una expresion
                                if (root.ChildNodes[2].ChildNodes.Count == 0)
                                {
                                    if (root.ChildNodes[1].ChildNodes[0].ToString().Equals("VARIABLE"))
                                    {
                                        subPrograma.modificarVariable(nombreParametro, new Simbolo(expresion.tipo, expresion.valor));
                                        subPrograma.correlacionParametros.Add(nombreParametro, removerExtras(root.ChildNodes[1].ChildNodes[0].ChildNodes[0].ToString()));
                                        if (contador == (subPrograma.ordenParametros.Count - 1))
                                        {
                                            return true;
                                        }
                                        error = new Error(0, 0, "Semantico", "Faltan parametros");
                                        errores.Add(error);
                                        MessageBox.Show("faltan parametros");
                                        return false;
                                    }
                                }
                                else
                                {
                                    if (root.ChildNodes[1].ChildNodes[0].ToString().Equals("VARIABLE"))
                                    {
                                        subPrograma.modificarVariable(nombreParametro, new Simbolo(expresion.tipo, expresion.valor));
                                        subPrograma.correlacionParametros.Add(nombreParametro, removerExtras(root.ChildNodes[1].ChildNodes[0].ChildNodes[0].ToString()));
                                        if (root.ChildNodes[1].ChildNodes.Count != 0)
                                        {
                                            contador++;
                                            return enviarParametros(root.ChildNodes[2], entorno, subPrograma, contador);
                                        }
                                        else
                                        {
                                            if (contador == (subPrograma.ordenParametros.Count - 1))
                                            {
                                                return true;
                                            }
                                            error = new Error(0, 0, "Semantico", "Faltan parametros");
                                            errores.Add(error);
                                            MessageBox.Show("faltan parametros");
                                            return false;
                                        }
                                    }
                                }
                                error = new Error(0, 0, "Semantico", "Se esperaba una variable y se obtuvo una expresion");
                                errores.Add(error);
                                MessageBox.Show("se esperaba una variable y se obtuvo una expresion");
                                return false;
                            }
                        }
                        error = new Error(0, 0, "Semantico", "El numero de parametros es mayor al esperado");
                        errores.Add(error);
                        MessageBox.Show("el numero de parametros el mayor al esperado");
                        return false;
                    }
                    error = new Error(0, 0, "Semantico", "Se esperaba un valor primitivo, objeto o arreglo");
                    errores.Add(error);
                    MessageBox.Show("se esperaba un valor primitivo, objeto o arreglo");
                    return false;
                }
                MessageBox.Show(expresion.valor.ToString() + root.ChildNodes[1].ToString());
                return false;
            }
            else
            {
                Expresion expresion = resolverExpresion(root.ChildNodes[0], entorno);
                Simbolo simbolo = null;
                if (expresion.tipo != Simbolo.EnumTipo.error)
                {
                    if (expresion.tipo != Simbolo.EnumTipo.funcion && expresion.tipo != Simbolo.EnumTipo.procedimiento && expresion.tipo != Simbolo.EnumTipo.nulo)
                    {
                        if (contador < subPrograma.ordenParametros.Count)
                        {
                            string nombreParametro = subPrograma.ordenParametros[contador].ToString();
                            subPrograma.parametrosValor.TryGetValue(nombreParametro, out simbolo);
                            if (simbolo != null)
                            {
                                //Es un parametro por valor
                                subPrograma.modificarValor(nombreParametro, new Simbolo(expresion.tipo, expresion.valor));
                                if (root.ChildNodes[1].ChildNodes.Count != 0)
                                {
                                    contador++;
                                    return enviarParametros(root.ChildNodes[1], entorno, subPrograma, contador);
                                }
                                else
                                {
                                    if (contador == (subPrograma.ordenParametros.Count - 1))
                                    {
                                        return true;
                                    }
                                    error = new Error(0, 0, "Semantico", "Faltan parametros");
                                    errores.Add(error);
                                    MessageBox.Show("2    -    faltan parametros");
                                    return false;
                                }
                            }
                            else
                            {
                                //Es un parametro por variable, se espera que el valor sea una variable, no una expresion
                                if (root.ChildNodes[0].ChildNodes[0].ToString().Equals("VARIABLE"))
                                {
                                    subPrograma.modificarVariable(nombreParametro, new Simbolo(expresion.tipo, expresion.valor));
                                    subPrograma.correlacionParametros.Add(nombreParametro, removerExtras(root.ChildNodes[0].ChildNodes[0].ChildNodes[0].ToString()));
                                    if (root.ChildNodes[1].ChildNodes.Count != 0)
                                    {
                                        contador++;
                                        return enviarParametros(root.ChildNodes[1], entorno, subPrograma, contador);
                                    }
                                    else
                                    {
                                        if (contador == (subPrograma.ordenParametros.Count - 1))
                                        {
                                            return true;
                                        }
                                        error = new Error(0, 0, "Semantico", "Faltan parametros");
                                        errores.Add(error);
                                        MessageBox.Show("2    -    faltan parametros");
                                        return false;
                                    }
                                }
                                error = new Error(0, 0, "Semantico", "Se esperaba una variable y se obtuvo una expresion");
                                errores.Add(error);
                                MessageBox.Show("2    -    se esperaba una variable y se obtuvo una expresion");
                                return false;
                            }
                        }
                        error = new Error(0, 0, "Semantico", "El numero de parametros es mayor al esperado");
                        errores.Add(error);
                        MessageBox.Show("2    -    el numero de parametros el mayor al esperado");
                        return false;
                    }
                    error = new Error(0, 0, "Semantico", "Se esperaba un valor primitivo, objeto o arreglo");
                    errores.Add(error);
                    MessageBox.Show("2    -    se esperaba un valor primitivo, objeto o arreglo");
                    return false;
                }
                MessageBox.Show(expresion.valor.ToString());
                return false;
            }
        }
        private Simbolo obtenerCampo(ParseTreeNode root, Object objectPadre, Expresion nuevoSimbolo, Entorno entorno)
        {
            Simbolo simboloPadre = null;
            Objeto temp = null;
            if (root.ChildNodes.Count == 2) //Es un objeto o parametro
            {
                if (root.ChildNodes[1].ChildNodes.Count == 0) //Es el ultimo parametro
                {
                    if (typeof(Objeto).IsInstanceOfType(objectPadre))
                    {
                        temp = new Objeto((Objeto)objectPadre);
                        //Recibe un object de tipo Objeto
                        if (temp.buscar(removerExtras(root.ChildNodes[0].ToString())) != null)
                        {
                            //El parametro que buscamos si existe
                            if (!temp.modificar(removerExtras(root.ChildNodes[0].ToString()), new Simbolo(nuevoSimbolo.tipo, nuevoSimbolo.valor)))
                            {
                                Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Los tipos no coinciden");
                                errores.Add(error);
                                return null;
                            }
                            simboloPadre = new Simbolo(temp.tipo, temp);
                            return simboloPadre;
                        }
                    }
                    else
                    {
                        temp = new Objeto((Objeto)((Simbolo)objectPadre).valor);
                        //Recibe un object de tipo Objeto
                        if (temp.buscar(removerExtras(root.ChildNodes[0].ToString())) != null)
                        {
                            //El parametro que buscamos si existe
                            if (!temp.modificar(removerExtras(root.ChildNodes[0].ToString()), new Simbolo(nuevoSimbolo.tipo, nuevoSimbolo.valor)))
                            {
                                Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "Los tipos no coinciden");
                                errores.Add(error);
                                return null;
                            }
                            simboloPadre = (Simbolo)objectPadre;
                            simboloPadre.valor = temp;
                            return simboloPadre;
                        }
                    }
                }
                else //Hay mas parametros
                {
                    if (typeof(Objeto).IsInstanceOfType(((Simbolo)objectPadre).valor))
                    {
                        //Recibe un object de tipo Objeto
                        temp = new Objeto((Objeto)((Simbolo)objectPadre).valor);
                        Simbolo simbolo = temp.buscar(removerExtras(root.ChildNodes[0].ToString()));
                        if (temp.buscar(removerExtras(root.ChildNodes[0].ToString())) != null)
                        {
                            //El parametro que buscamos si existe
                            if (root.ChildNodes.Count == 2)
                            {
                                simboloPadre = (Simbolo)objectPadre;
                                simboloPadre = obtenerCampo(root.ChildNodes[1], simbolo, nuevoSimbolo, entorno);
                                temp.modificar(removerExtras(root.ChildNodes[0].ToString()), new Simbolo(simbolo.tipo, simbolo.valor));
                                ((Simbolo)objectPadre).valor = temp;
                                return (Simbolo)objectPadre;
                            }
                            else
                            {
                                return null;
                            }
                        }
                    }
                }
            }
            else if (root.ChildNodes.Count == 4) //Es un arreglo de tipo objeto
            {
                return null;
            }
            else //Es un arreglo de tipo primitivo
            {
                return new Simbolo(nuevoSimbolo.tipo, nuevoSimbolo.valor);
            }
            return null;
        }
        private void recorrer(ParseTreeNode root, Entorno entorno)
        {
            if (!parar && !continuar && !salir) //Comprueba si existe un break o continue
            {
                Entorno nuevoEntorno;
                Simbolo simbolo = null;
                Expresion expresion;
                switch (root.ToString())
                {
                    case "BREAK":
                        parar = true;
                        break;
                    case "CONTINUE":
                        continuar = true;
                        break;
                    case "CASE":
                        ejecutarCase(root, entorno);
                        parar = false;
                        continuar = false;
                        break;
                    case "IF":
                        ejecutarIf(root, entorno);
                        break;
                    case "REPEAT":
                        ejecutarRepeat(root, entorno);
                        parar = false;
                        continuar = false;
                        break;
                    case "WHILE":
                        ejecutarWhile(root, entorno);
                        parar = false;
                        continuar = false;
                        break;
                    case "FUNCION":
                        nuevoEntorno = new Entorno(entorno, removerExtras(root.ChildNodes[0].ChildNodes[0].ToString()));
                        entornos.Add(nuevoEntorno);
                        simbolo = agregarFuncion(root, entorno, nuevoEntorno);
                        if (!(simbolo is null))
                        {
                            entorno.insertar(removerExtras(root.ChildNodes[0].ChildNodes[0].ToString()), simbolo, root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column);
                        }
                        break;
                    case "PROCEDIMIENTO":
                        nuevoEntorno = new Entorno(entorno, removerExtras(root.ChildNodes[0].ChildNodes[0].ToString()));
                        entornos.Add(nuevoEntorno);
                        simbolo = agregarProcedimiento(root, entorno, nuevoEntorno);
                        if (!(simbolo is null))
                        {
                            entorno.insertar(removerExtras(root.ChildNodes[0].ChildNodes[0].ToString()), simbolo, root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column);
                        }
                        break;
                    case "LLAMADA":
                        if (root.ChildNodes[0].ToString().ToLower().Contains("writeln"))
                        {
                            ejecutarWriteLn(root, entorno);
                        }
                        else if (root.ChildNodes[0].ToString().ToLower().Contains("write"))
                        {
                            ejecutarWrite(root, entorno);
                        }
                        else if (root.ChildNodes[0].ToString().ToLower().Contains("graficar_ts"))
                        {
                            ejecutarGraficarTS();
                        }
                        else if (root.ChildNodes[0].ToString().ToLower().Contains("exit"))
                        {
                            ejecutarExit(root, entorno);
                        }
                        else
                        {
                            ejecutarLlamada(root, entorno);
                            salir = false;
                        }
                        break;
                    case "Z_TIPOS":
                        if (root.ChildNodes.Count != 0)
                        {
                            string nombreTipo = removerExtras(root.ChildNodes[0].ToString());
                            if (root.ChildNodes[2].ChildNodes[0].ToString().Contains("object"))
                            {
                                //Es un objeto
                                Objeto nuevoObjeto = new Objeto(nombreTipo);
                                agregarCamposObjeto(root.ChildNodes[2].ChildNodes[1], nuevoObjeto, entorno);
                                agregarCamposObjeto(root.ChildNodes[2].ChildNodes[2], nuevoObjeto, entorno);
                                if(!entorno.insertar(nombreTipo, new Simbolo(Simbolo.EnumTipo.objeto, nuevoObjeto), root.ChildNodes[2].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[0].Token.Location.Column))
                                {
                                    Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "El identificador " + removerExtras(root.ChildNodes[0].ToString() + " ya existe"));
                                    errores.Add(error);
                                }
                            }
                            else
                            {
                                //Es un array
                                ParseTreeNode t_ordinal = root.ChildNodes[2].ChildNodes[1];
                                ParseTreeNode indice = root.ChildNodes[2].ChildNodes[2];
                                Expresion exp1 = resolverExpresion(t_ordinal.ChildNodes[0], entorno);
                                Expresion exp2 = resolverExpresion(t_ordinal.ChildNodes[1], entorno);
                                //Array nuevoArreglo = null;
                                Objeto nuevoArreglo = new Objeto("test");
                                if (indice.ChildNodes.Count == 0)
                                {
                                    if (verificarRango(exp1, exp2))
                                    {
                                        int index = int.Parse(exp2.valor.ToString());
                                        nuevoArreglo = new Objeto(nombreTipo);
                                        nuevoArreglo.arreglo = new Simbolo[index];
                                        if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("real"))
                                        {
                                            for (int i = 0; i < index; i++)
                                            {
                                                nuevoArreglo.arreglo.SetValue(new Simbolo(Simbolo.EnumTipo.real, 0.0), i);
                                            }
                                            nuevoArreglo.tipo = Simbolo.EnumTipo.real;
                                        }
                                        else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("boolean"))
                                        {
                                            for (int i = 0; i < index; i++)
                                            {
                                                nuevoArreglo.arreglo.SetValue(new Simbolo(Simbolo.EnumTipo.boleano, false), i);
                                            }
                                            nuevoArreglo.tipo = Simbolo.EnumTipo.boleano;
                                        }
                                        else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("integer"))
                                        {
                                            for (int i = 0; i < index; i++)
                                            {
                                                nuevoArreglo.arreglo.SetValue(new Simbolo(Simbolo.EnumTipo.entero, 0), i);
                                            }
                                            nuevoArreglo.tipo = Simbolo.EnumTipo.entero;
                                        }
                                        else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("string"))
                                        {
                                            for (int i = 0; i < index; i++)
                                            {
                                                nuevoArreglo.arreglo.SetValue(new Simbolo(Simbolo.EnumTipo.cadena, ""), i);
                                            }
                                            nuevoArreglo.tipo = Simbolo.EnumTipo.cadena;
                                        }
                                        else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("id"))
                                        {
                                            Expresion tmp = buscarVariable(root.ChildNodes[2].ChildNodes[3].ChildNodes[0], entorno);
                                            if (tmp.tipo == Simbolo.EnumTipo.objeto)
                                            {
                                                nuevoArreglo = new Objeto(nombreTipo);
                                                nuevoArreglo.arreglo = new Objeto[index];
                                                for (int i = 0; i < index; i++)
                                                {
                                                    nuevoArreglo.arreglo.SetValue((Objeto)tmp.valor, i);
                                                }
                                                nuevoArreglo.nombreTipo = removerExtras(root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString());
                                                nuevoArreglo.tipo = Simbolo.EnumTipo.objeto;
                                            }
                                            else if (tmp.tipo == Simbolo.EnumTipo.arreglo)
                                            {
                                                nuevoArreglo = new Objeto(nombreTipo);
                                                nuevoArreglo.arreglo = new Array[index];
                                                Objeto objtmp;
                                                for (int i = 0; i < index; i++)
                                                {
                                                    objtmp = (Objeto)tmp.valor;
                                                    nuevoArreglo.arreglo.SetValue(objtmp.arreglo, i);
                                                }
                                                nuevoArreglo.nombreTipo = removerExtras(root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString());
                                                nuevoArreglo.tipo = Simbolo.EnumTipo.objeto;

                                            }
                                            else if (tmp.tipo == Simbolo.EnumTipo.error)
                                            {
                                                Error error = new Error(root.ChildNodes[2].ChildNodes[3].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[3].ChildNodes[0].Token.Location.Column, "Semantico", tmp.valor.ToString());
                                                errores.Add(error);
                                            }
                                            else
                                            {
                                                Error error = new Error(root.ChildNodes[2].ChildNodes[3].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[3].ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba objeto o arreglo");
                                                errores.Add(error);
                                            }
                                        }
                                        if (nuevoArreglo.arreglo != null)
                                        {
                                            if (!entorno.insertar(nombreTipo, new Simbolo(Simbolo.EnumTipo.arreglo, nuevoArreglo), root.ChildNodes[2].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[0].Token.Location.Column))
                                            {
                                                Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "El identificador " + removerExtras(root.ChildNodes[0].ToString()) + " ya existe");
                                                errores.Add(error);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    t_ordinal = root.ChildNodes[2].ChildNodes[1];
                                    indice = root.ChildNodes[2].ChildNodes[2].ChildNodes[1];
                                    exp1 = resolverExpresion(t_ordinal.ChildNodes[0], entorno);
                                    exp2 = resolverExpresion(t_ordinal.ChildNodes[1], entorno);
                                    nuevoArreglo = null;
                                    if (indice.ChildNodes.Count == 0)
                                    {
                                        if (verificarRango(exp1, exp2))
                                        {
                                            int index1 = int.Parse(exp2.valor.ToString());
                                            t_ordinal = root.ChildNodes[2].ChildNodes[2].ChildNodes[0];
                                            indice = root.ChildNodes[2].ChildNodes[2].ChildNodes[1];
                                            exp1 = resolverExpresion(t_ordinal.ChildNodes[0], entorno);
                                            exp2 = resolverExpresion(t_ordinal.ChildNodes[1], entorno);
                                            if (indice.ChildNodes.Count == 0)
                                            {
                                                if (verificarRango(exp1, exp2))
                                                {
                                                    int index2 = int.Parse(exp2.valor.ToString());
                                                    nuevoArreglo = new Objeto(nombreTipo);
                                                    nuevoArreglo.arreglo = new Simbolo[index1, index2];
                                                    if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("real"))
                                                    {
                                                        for (int i = 0; i < index1; i++)
                                                        {
                                                            for (int j = 0; j < index2; j++)
                                                            {
                                                                nuevoArreglo.arreglo.SetValue(new Simbolo(Simbolo.EnumTipo.real, 0.0), i, j);
                                                            }
                                                        }
                                                        nuevoArreglo.tipo = Simbolo.EnumTipo.real;
                                                    }
                                                    else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("boolean"))
                                                    {
                                                        for (int i = 0; i < index1; i++)
                                                        {
                                                            for (int j = 0; j < index2; j++)
                                                            {
                                                                nuevoArreglo.arreglo.SetValue(new Simbolo(Simbolo.EnumTipo.boleano, false), i, j);
                                                            }
                                                        }
                                                        nuevoArreglo.tipo = Simbolo.EnumTipo.boleano;
                                                    }
                                                    else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("integer"))
                                                    {
                                                        for (int i = 0; i < index1; i++)
                                                        {
                                                            for (int j = 0; j < index2; j++)
                                                            {
                                                                nuevoArreglo.arreglo.SetValue(new Simbolo(Simbolo.EnumTipo.entero, 0), i, j);
                                                            }
                                                        }
                                                        nuevoArreglo.tipo = Simbolo.EnumTipo.entero;
                                                    }
                                                    else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("string"))
                                                    {
                                                        for (int i = 0; i < index1; i++)
                                                        {
                                                            for (int j = 0; j < index2; j++)
                                                            {
                                                                nuevoArreglo.arreglo.SetValue(new Simbolo(Simbolo.EnumTipo.cadena, ""), i, j);
                                                            }
                                                        }
                                                        nuevoArreglo.tipo = Simbolo.EnumTipo.cadena;
                                                    }
                                                    else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("id"))
                                                    {
                                                        Expresion tmp = buscarVariable(root.ChildNodes[2].ChildNodes[3].ChildNodes[0], entorno);
                                                        if (tmp.tipo == Simbolo.EnumTipo.arreglo || tmp.tipo == Simbolo.EnumTipo.objeto)
                                                        {
                                                            nuevoArreglo = new Objeto(nombreTipo);
                                                            nuevoArreglo.arreglo = new Objeto[index1, index2];
                                                            for (int i = 0; i < index1; i++)
                                                            {
                                                                for (int j = 0; j < index2; j++)
                                                                {
                                                                    nuevoArreglo.arreglo.SetValue((Objeto)tmp.valor, i, j);
                                                                }
                                                            }
                                                            nuevoArreglo.nombreTipo = removerExtras(root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString());
                                                            nuevoArreglo.tipo = Simbolo.EnumTipo.objeto;
                                                        }
                                                        else if (tmp.tipo == Simbolo.EnumTipo.error)
                                                        {
                                                            Error error = new Error(0, 0, "Semantico", tmp.valor.ToString());
                                                            errores.Add(error);
                                                        }
                                                        else
                                                        {
                                                            Error error = new Error(0, 0, "Semantico", "Se esperaba tipo objeto o arreglo");
                                                            errores.Add(error);
                                                        }
                                                    }
                                                    if (nuevoArreglo != null)
                                                    {
                                                        if(!entorno.insertar(nombreTipo, new Simbolo(Simbolo.EnumTipo.arreglo, nuevoArreglo), root.ChildNodes[2].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[0].Token.Location.Column))
                                                        {
                                                            Error error = new Error(root.ChildNodes[2].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[0].Token.Location.Column, "Semantico", "El identificador" + nombreTipo + " ya existe");
                                                            errores.Add(error);
                                                        }
                                                    }
                                                }
                                            }
                                            if (nuevoArreglo != null)
                                            {
                                                if (!entorno.insertar(nombreTipo, new Simbolo(Simbolo.EnumTipo.arreglo, nuevoArreglo), root.ChildNodes[2].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[0].Token.Location.Column))
                                                {
                                                    Error error = new Error(root.ChildNodes[2].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[0].Token.Location.Column, "Semantico", "El identificador " + nombreTipo + " ya existe");
                                                    errores.Add(error);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //NO BORRAR
                                        MessageBox.Show("OJO NO HICE ESTE METODO PARA MAS DE 2 DIMENSIONES, USAR ARRAY DE ARAYYS");
                                    }
                                }
                            }
                            recorrer(root.ChildNodes[3], entorno);
                            if (root.ChildNodes.Count == 5)
                            {
                                recorrer(root.ChildNodes[4], entorno);
                            }
                        }
                        break;
                    case "ASIGNACION":
                        if (root.ChildNodes[0].ToString().Equals("VARIABLE"))
                        {
                            expresion = resolverExpresion(root.ChildNodes[1], entorno);
                            if (expresion.tipo != Simbolo.EnumTipo.error)
                            {
                                simbolo = entorno.buscar(removerExtras(root.ChildNodes[0].ChildNodes[0].ToString()));
                                if (simbolo != null)
                                {
                                    if (simbolo.tipo == Simbolo.EnumTipo.funcion)
                                    {
                                        SubPrograma p = (SubPrograma)simbolo.valor;
                                        if (p.retorno.tipo == expresion.tipo)
                                        {
                                            p.retorno = new Simbolo(expresion.tipo, expresion.valor);
                                            simbolo.valor = p;
                                            entorno.modificar(removerExtras(root.ChildNodes[0].ChildNodes[0].ToString()), simbolo);
                                            retornoFuncion = new Expresion(expresion.tipo, expresion.valor);
                                        }
                                        else
                                        {
                                            Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba tipo " + expresion.tipo + " se encontro tipo " + p.retorno.tipo);
                                            errores.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        if (expresion.tipo == simbolo.tipo)
                                        {
                                            simbolo = new Simbolo(expresion.tipo, expresion.valor);
                                            entorno.modificar(removerExtras(root.ChildNodes[0].ChildNodes[0].ToString()), simbolo);
                                        }
                                        else
                                        {
                                            Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba tipo " + simbolo.tipo + " se encontro tipo " + expresion.tipo);
                                            errores.Add(error);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Es una estructura
                            simbolo = entorno.buscar(removerExtras(root.ChildNodes[0].ChildNodes[0].ToString()));
                            if (simbolo != null)
                            {
                                //La variable existe
                                Expresion nuevoValor = resolverExpresion(root.ChildNodes[1], entorno);
                                if (nuevoValor.tipo != Simbolo.EnumTipo.error)
                                {
                                    Simbolo nuevoSimbolo = null;
                                    if (root.ChildNodes[0].ChildNodes.Count == 2)
                                    {
                                        nuevoSimbolo = obtenerCampo(root.ChildNodes[0].ChildNodes[1], simbolo, nuevoValor, entorno);
                                    }
                                    else
                                    {
                                        //Es un arreglo
                                        if (simbolo.tipo == Simbolo.EnumTipo.arreglo)
                                        {
                                            Expresion indice = resolverExpresion(root.ChildNodes[0].ChildNodes[1], entorno);
                                            if (indice.tipo != Simbolo.EnumTipo.error)
                                            {
                                                if (indice.tipo == Simbolo.EnumTipo.entero)
                                                {
                                                    int index1 = int.Parse(indice.valor.ToString());
                                                    if (root.ChildNodes[0].ChildNodes[2].ChildNodes.Count == 0)
                                                    {
                                                        //Se espera que simbolo sea un arreglo de 1 dimension
                                                        Array temp = (Array)((Objeto)simbolo.valor).arreglo;
                                                        if (temp.Rank == 1)
                                                        {
                                                            if (index1 < temp.Length)
                                                            {
                                                                Objeto tmpObj = (Objeto)simbolo.valor;
                                                                nuevoSimbolo = obtenerCampo(root.ChildNodes[0].ChildNodes[3], temp.GetValue(index1), nuevoValor, entorno);
                                                                if (nuevoSimbolo.tipo != Simbolo.EnumTipo.error)
                                                                {
                                                                    if (typeof(Objeto).IsInstanceOfType(nuevoSimbolo.valor))
                                                                    {
                                                                        tmpObj.arreglo.SetValue(nuevoSimbolo.valor, index1);
                                                                    }
                                                                    else
                                                                    {
                                                                        tmpObj.arreglo.SetValue(nuevoSimbolo, index1);
                                                                    }
                                                                    nuevoSimbolo = new Simbolo(simbolo.tipo, tmpObj);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", "Indice fuera del limite");
                                                                errores.Add(error);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba arreglo de 1 dimension");
                                                            errores.Add(error);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Se espera que simbolo sea un arreglo de 2 (podrian ser mas pero solo puedo operar 2 dimensiones)
                                                        Array temp = (Array)((Objeto)simbolo.valor).arreglo;
                                                        if (temp.Rank != 1)
                                                        {
                                                            Expresion indice2 = resolverExpresion(root.ChildNodes[0].ChildNodes[2].ChildNodes[0], entorno);
                                                            if (indice2.tipo != Simbolo.EnumTipo.error)
                                                            {
                                                                if (indice2.tipo == Simbolo.EnumTipo.entero)
                                                                {
                                                                    int index2 = int.Parse(indice2.valor.ToString());
                                                                    if (index1 < temp.GetLength(0))
                                                                    {
                                                                        if (index2 < temp.GetLength(1))
                                                                        {
                                                                            Objeto tmpObj = (Objeto)simbolo.valor;
                                                                            nuevoSimbolo = obtenerCampo(root.ChildNodes[0].ChildNodes[3], temp.GetValue(index1, index2), nuevoValor, entorno);
                                                                            if (nuevoSimbolo.tipo != Simbolo.EnumTipo.error)
                                                                            {
                                                                                if (typeof(Objeto).IsInstanceOfType(nuevoSimbolo.valor))
                                                                                {
                                                                                    tmpObj.arreglo.SetValue(nuevoSimbolo.valor, index1, index2);
                                                                                }
                                                                                else
                                                                                {
                                                                                    tmpObj.arreglo.SetValue(nuevoSimbolo, index1, index2);
                                                                                }
                                                                                nuevoSimbolo = new Simbolo(simbolo.tipo, tmpObj);
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", "Indice fuera del limite");
                                                                            errores.Add(error);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", "Indice fuera del limite");
                                                                        errores.Add(error);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", "Error de tipos");
                                                                    errores.Add(error);
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", indice2.valor.ToString());
                                                                errores.Add(error);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba un arreglo de 2 dimensiones");
                                                            errores.Add(error);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", "Error de tipos");
                                                    errores.Add(error);
                                                }
                                            }
                                            else
                                            {
                                                Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", indice.valor.ToString());
                                                errores.Add(error);
                                            }
                                        }
                                    }
                                    if (nuevoSimbolo != null)
                                    {
                                        entorno.modificar(removerExtras(root.ChildNodes[0].ChildNodes[0].ToString()), nuevoSimbolo);
                                    }
                                }
                                else
                                {
                                    Error error = new Error(root.ChildNodes[0].ChildNodes[0].Token.Location.Line, root.ChildNodes[0].ChildNodes[0].Token.Location.Column, "Semantico", nuevoValor.valor.ToString());
                                    errores.Add(error);
                                }
                            }
                        }
                        break;
                    case "FOR":
                        ejecutarFor(root, entorno);
                        parar = false;
                        continuar = false;
                        break;
                    case "SUBPROGRAMA":
                    case "PROGRAMA":
                    case "SENTENCIA":
                    case "BEGIN_END":
                    case "Z_CONSTANTES":
                    case "Z_VARIABLES":
                    case "Z_DECLARACIONES":
                    case "Z_SUBPROGRAMAS":
                        foreach (ParseTreeNode hijo in root.ChildNodes)
                        {
                            recorrer(hijo, entorno);
                        }
                        break;
                    case "PROCEDIMIENTO_HEAD":
                        break;
                    case "DECLARACION_CAMPOS_TYPE":
                        break;
                    case "D_CONSTANTE":
                        simbolo = null;
                        if (root.ChildNodes.Count != 0)
                        {
                            if (root.ChildNodes[0].ToString().Equals("D_CONSTANTE"))
                            {
                                foreach (ParseTreeNode hijo in root.ChildNodes)
                                {
                                    recorrer(hijo, entorno);
                                }
                            }
                            else
                            {
                                if (root.ChildNodes.Count == 4)
                                {
                                    if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("real"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.real, 0.0);
                                    }
                                    else if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("boolean"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.boleano, false);
                                    }
                                    else if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("integer"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.entero, 0);
                                    }
                                    else if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("string"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.cadena, "");
                                    }
                                    else if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("id"))
                                    {
                                        Error error = new Error(root.ChildNodes[1].ChildNodes[0].Token.Location.Line, root.ChildNodes[1].ChildNodes[0].Token.Location.Column, "Semantico", "Las constantes solo pueden ser datos primitivos");
                                        errores.Add(error);
                                    }
                                    expresion = resolverExpresion(root.ChildNodes[3], entorno);
                                    if (expresion.tipo == Simbolo.EnumTipo.error)
                                    {
                                        Error error = new Error(root.ChildNodes[1].ChildNodes[0].Token.Location.Line, root.ChildNodes[1].ChildNodes[0].Token.Location.Column, "Semantico", expresion.valor.ToString());
                                        errores.Add(error);
                                    }
                                    else
                                    {
                                        if (expresion.tipo != simbolo.tipo)
                                        {
                                            if (expresion.tipo == Simbolo.EnumTipo.entero && simbolo.tipo == Simbolo.EnumTipo.real)
                                            {
                                                simbolo.valor = expresion.valor;
                                            }
                                            else
                                            {
                                                simbolo.tipo = Simbolo.EnumTipo.error;
                                                Error error = new Error(root.ChildNodes[1].ChildNodes[0].Token.Location.Line, root.ChildNodes[1].ChildNodes[0].Token.Location.Column, "Semantico", "Error de tipos");
                                                errores.Add(error);
                                            }
                                        }
                                        else
                                        {
                                            simbolo.valor = expresion.valor;
                                        }
                                    }
                                    if (simbolo is null)
                                    {
                                    }
                                    else if (expresion.tipo != Simbolo.EnumTipo.error && simbolo.tipo != Simbolo.EnumTipo.error)
                                    {
                                        simbolo.constante = true;
                                        if(!entorno.insertar(removerExtras(root.ChildNodes[0].ToString()), simbolo, root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column))
                                        {
                                            Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "El identificador " + root.ChildNodes[0].ToString() + " ya existe");
                                            errores.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        Error error = new Error(root.ChildNodes[1].ChildNodes[0].Token.Location.Line, root.ChildNodes[1].ChildNodes[0].Token.Location.Column, "Semantico", simbolo.valor.ToString());
                                        errores.Add(error);
                                    }
                                }
                                else if (root.ChildNodes.Count == 3)
                                {
                                    expresion = resolverExpresion(root.ChildNodes[2], entorno);
                                    if(expresion.tipo != Simbolo.EnumTipo.error)
                                    {
                                        simbolo = new Simbolo(expresion.tipo, expresion.valor);
                                        simbolo.constante = true;
                                        if (!entorno.insertar(removerExtras(root.ChildNodes[0].ToString()), simbolo, root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column))
                                        {
                                            Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "El identificador " + root.ChildNodes[0].ToString() + " ya existe");
                                            errores.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", expresion.valor.ToString());
                                        errores.Add(error);
                                    }
                                }
                            }
                        }
                        break;
                    case "D_VARIABLE":
                        simbolo = null;
                        if (root.ChildNodes.Count != 0)
                        {
                            if (root.ChildNodes[0].ToString().Equals("D_VARIABLE"))
                            {
                                foreach (ParseTreeNode hijo in root.ChildNodes)
                                {
                                    recorrer(hijo, entorno);
                                }
                            }
                            else
                            {
                                if (root.ChildNodes.Count == 4)
                                {
                                    if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("real"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.real, 0.0);
                                    }
                                    else if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("boolean"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.boleano, false);
                                    }
                                    else if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("integer"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.entero, 0);
                                    }
                                    else if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("string"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.cadena, "");
                                    }
                                    else if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("id"))
                                    {
                                        Expresion tmp = buscarVariable(root.ChildNodes[1].ChildNodes[0], entorno);
                                        if (tmp.tipo == Simbolo.EnumTipo.arreglo || tmp.tipo == Simbolo.EnumTipo.objeto)
                                        {
                                            Objeto obj = new Objeto((Objeto)tmp.valor);
                                            simbolo = new Simbolo(tmp.tipo, obj);
                                        }
                                        else if (tmp.tipo == Simbolo.EnumTipo.error)
                                        {
                                            Error error = new Error(root.ChildNodes[1].ChildNodes[0].Token.Location.Line, root.ChildNodes[1].ChildNodes[0].Token.Location.Column, "Semantico", tmp.valor.ToString());
                                            errores.Add(error);
                                        }
                                        else
                                        {
                                            Error error = new Error(root.ChildNodes[1].ChildNodes[0].Token.Location.Line, root.ChildNodes[1].ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba tipo objeto o arreglo");
                                            errores.Add(error);
                                        }
                                    }
                                    expresion = resolverExpresion(root.ChildNodes[3], entorno);
                                    if (expresion.tipo == Simbolo.EnumTipo.error)
                                    {
                                        Error error = new Error(root.ChildNodes[1].ChildNodes[0].Token.Location.Line, root.ChildNodes[1].ChildNodes[0].Token.Location.Column, "Semantico", expresion.valor.ToString());
                                        errores.Add(error);
                                    }
                                    else
                                    {
                                        if (expresion.tipo != simbolo.tipo)
                                        {
                                            if (expresion.tipo == Simbolo.EnumTipo.entero && simbolo.tipo == Simbolo.EnumTipo.real)
                                            {
                                                simbolo.valor = expresion.valor;
                                            }
                                            else
                                            {
                                                simbolo.tipo = Simbolo.EnumTipo.error;
                                                Error error = new Error(root.ChildNodes[1].ChildNodes[0].Token.Location.Line, root.ChildNodes[1].ChildNodes[0].Token.Location.Column, "Semantico", simbolo.valor.ToString());
                                                errores.Add(error);
                                            }
                                        }
                                        else
                                        {
                                            simbolo.valor = expresion.valor;
                                        }
                                    }
                                    if (simbolo is null)
                                    {
                                    }
                                    else if (expresion.tipo != Simbolo.EnumTipo.error && simbolo.tipo != Simbolo.EnumTipo.error)
                                    {
                                        if (!entorno.insertar(removerExtras(root.ChildNodes[0].ToString()), simbolo, root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column))
                                        {
                                            Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "El identificador " + removerExtras(root.ChildNodes[0].ToString()) + " ya existe");
                                            errores.Add(error);
                                        }
                                    }
                                    else
                                    {
                                        Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", simbolo.valor.ToString());
                                        errores.Add(error);
                                    }
                                }
                                else if (root.ChildNodes.Count == 3)
                                {
                                    ParseTreeNode temp = root;
                                    if (root.ChildNodes[2].ChildNodes[0].ToString().Contains("real"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.real, 0.0);
                                    }
                                    else if (root.ChildNodes[2].ChildNodes[0].ToString().Contains("boolean"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.boleano, false);
                                    }
                                    else if (root.ChildNodes[2].ChildNodes[0].ToString().Contains("integer"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.entero, 0);
                                    }
                                    else if (root.ChildNodes[2].ChildNodes[0].ToString().Contains("string"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.cadena, "");
                                    }
                                    else if (root.ChildNodes[2].ChildNodes[0].ToString().Contains("id"))
                                    {
                                        Expresion tmp = buscarVariable(root.ChildNodes[2].ChildNodes[0], entorno);
                                        if (tmp.tipo == Simbolo.EnumTipo.arreglo || tmp.tipo == Simbolo.EnumTipo.objeto)
                                        {
                                            Objeto obj = new Objeto((Objeto)tmp.valor);
                                            simbolo = new Simbolo(tmp.tipo, obj);
                                        }
                                        else if (tmp.tipo == Simbolo.EnumTipo.error)
                                        {
                                            Error error = new Error(root.ChildNodes[2].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[0].Token.Location.Column, "Semantico", tmp.valor.ToString());
                                            errores.Add(error);
                                        }
                                        else
                                        {
                                            Error error = new Error(root.ChildNodes[2].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba tipo objeto o arreglo");
                                            errores.Add(error);
                                        }
                                    }
                                    if (simbolo is null)
                                    {
                                    }
                                    else
                                    {
                                        while (temp.ChildNodes.Count != 0)
                                        {
                                            if (!entorno.insertar(removerExtras(temp.ChildNodes[0].ToString()), new Simbolo(simbolo.tipo, simbolo.valor), temp.ChildNodes[0].Token.Location.Line, temp.ChildNodes[0].Token.Location.Column))
                                            {
                                                Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "El identificador " + removerExtras(temp.ChildNodes[0].ToString()) + " ya existe");
                                                errores.Add(error);
                                            }
                                            temp = temp.ChildNodes[1];
                                        }
                                    }
                                }
                                else
                                {
                                    ParseTreeNode temp = root;
                                    if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("real"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.real, 0.0);
                                    }
                                    else if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("boolean"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.boleano, false);
                                    }
                                    else if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("integer"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.entero, 0);
                                    }
                                    else if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("string"))
                                    {
                                        simbolo = new Simbolo(Simbolo.EnumTipo.cadena, "");
                                    }
                                    else if (root.ChildNodes[1].ChildNodes[0].ToString().Contains("id"))
                                    {
                                        Expresion tmp = buscarVariable(root.ChildNodes[1].ChildNodes[0], entorno);
                                        if (tmp.tipo == Simbolo.EnumTipo.arreglo || tmp.tipo == Simbolo.EnumTipo.objeto)
                                        {
                                            Objeto obj = new Objeto((Objeto)tmp.valor);
                                            simbolo = new Simbolo(tmp.tipo, obj);
                                        }
                                        else if (tmp.tipo == Simbolo.EnumTipo.error)
                                        {
                                            Error error = new Error(root.ChildNodes[1].ChildNodes[0].Token.Location.Line, root.ChildNodes[1].ChildNodes[0].Token.Location.Column, "Semantico", tmp.valor.ToString());
                                            errores.Add(error);
                                        }
                                        else
                                        {
                                            //REPORTAR ERROR el id no es de tipo arreglo o de tipo objeto
                                            Error error = new Error(root.ChildNodes[1].ChildNodes[0].Token.Location.Line, root.ChildNodes[1].ChildNodes[0].Token.Location.Column, "Semantico", "Se esperaba tipo objeto o arreglo");
                                            errores.Add(error);
                                        }
                                    }
                                    if (simbolo is null)
                                    {
                                    }
                                    else
                                    {
                                        if(!entorno.insertar(removerExtras(temp.ChildNodes[0].ToString()), simbolo, temp.ChildNodes[0].Token.Location.Line, temp.ChildNodes[0].Token.Location.Column))
                                        {
                                            Error error = new Error(root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column, "Semantico", "El identificador " + removerExtras(temp.ChildNodes[0].ToString()) + " ya existe");
                                            errores.Add(error);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    case "CONTROLADOR":
                        break;
                    case "VALOR":
                        break;
                    case "PF":
                        break;
                    case "PFVL":
                        break;
                    case "PFVR":
                        break;
                    case "T_ESTRUCTURADO":
                        break;
                    case "T_ORDINAL":
                        break;
                    default:
                        if (!root.ToString().Contains(" ("))
                            MessageBox.Show("Falto agregar " + root.ToString() + " al switch");
                        break;
                }
            }
            else
            {
                        parar = false;
                        continuar = false;
            }
        }
    }
}