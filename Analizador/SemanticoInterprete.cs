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
        private ArrayList subProgramas;
        private Entorno entornoGlobal;
        private Expresion retornoFuncion;
        private bool parar;
        private bool continuar;
        public SemanticoInterprete()
        {

        }
        public void iniciarAnalisisSintactico(ParseTreeNode root)
        {
            consola = new ArrayList();
            errores = new ArrayList();
            entornos = new ArrayList();
            subProgramas = new ArrayList();
            entornoGlobal = new Entorno(null, "programa");
            entornos.Add(entornoGlobal);
            retornoFuncion = null;
            parar = false;
            continuar = false;
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
        private void ejecutarCase(ParseTreeNode root, Entorno entorno)
        {

        }
        private void ejecutarIf(ParseTreeNode root, Entorno entorno)
        {

        }
        private void ejecutarRepeat(ParseTreeNode root, Entorno entorno)
        {

        }
        private void ejecutarWhile(ParseTreeNode root, Entorno entorno)
        {

        }
        private void ejecutarFuncion(ParseTreeNode root, Entorno entorno)
        {

        }
        private void ejecutarProcedimiento(ParseTreeNode root, Entorno entorno)
        {

        }
        private void ejecutarLlamada(ParseTreeNode root, Entorno entorno)
        {

        }
        private void ejecutarAsignacion(ParseTreeNode root, Entorno entorno)
        {

        }
        private void ejecutarFor(ParseTreeNode root, Entorno entorno)
        {

        }
        private bool verificarRango(Expresion exp1, Expresion exp2)
        {
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
                            //AGREGAR ERROR exp1 debe ser menor a exp2
                            return false;
                        }
                        //AGREGAR ERROR exp2 debe ser entero
                        return false;
                    }
                    //AGREGAR ERROR exp1 debe ser entero
                    return false;
                }
                //AGREGAR ERROR ver error
                return false;
            }
            //AGREGAR ERROR ver error
            return false;
        }
        private Expresion resolverExpresion(ParseTreeNode root, Entorno ent)
        {
            switch (root.ToString())
            {
                case "EXPRESION":
                    if (root.ChildNodes.Count == 3)//Operador binario
                    {
                        if (root.ChildNodes[1].ToString().Contains("and"))
                        {
                            return operarAnd(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains("="))
                        {
                            return operarIgual(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains("<>"))
                        {
                            return operarDesigual(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains(">="))
                        {
                            return operarMayorIgual(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
                        }
                        else if (root.ChildNodes[1].ToString().Contains("<="))
                        {
                            return operarMenorIgual(resolverExpresion(root.ChildNodes[0], ent), resolverExpresion(root.ChildNodes[2], ent));
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
                    return resolverEstructura(root.ChildNodes[0], ent);
                case "LLAMADA":
                    return resolverLlamada(root.ChildNodes[0], ent);
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
        private Expresion resolverEstructura(ParseTreeNode root, Entorno entorno)
        {
            return new Expresion(Simbolo.EnumTipo.error, "Error desconocido");
        }
        private Expresion operarNegativo(Expresion expresion1)
        {
            String exp1 = expresion1.valor.ToString();
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
            return new Expresion(Simbolo.EnumTipo.error, "Error desconocido");
        }
        private Expresion buscarVariable(ParseTreeNode root, Entorno entorno)
        {
            Simbolo resultadoBusqueda = entorno.buscar(removerExtras(root.ToString()), root.Token.Location.Line, root.Token.Location.Column);
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
                case Simbolo.EnumTipo.cadena:
                    //Cadena (Entero || real || Caracter || boleano || Cadena)
                    return new Expresion(Simbolo.EnumTipo.cadena, expresion1.valor.ToString() + expresion2.valor.ToString());
                case Simbolo.EnumTipo.real:
                    switch (expresion2.tipo)
                    {
                        case Simbolo.EnumTipo.cadena:
                            //real Cadena
                            return new Expresion(Simbolo.EnumTipo.cadena, expresion1.valor.ToString() + expresion2.valor.ToString());
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
                        case Simbolo.EnumTipo.cadena:
                            //Entero Cadena
                            return new Expresion(Simbolo.EnumTipo.cadena, expresion1.valor.ToString() + expresion2.valor.ToString());
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
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) >= int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //real Entero
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
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) >= int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //Entero real
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) >= Double.Parse(expresion2.valor.ToString()));
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
                            return new Expresion(Simbolo.EnumTipo.boleano, Double.Parse(expresion1.valor.ToString()) <= int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //real Entero
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
                            //Entero Entero
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) <= int.Parse(expresion2.valor.ToString()));
                        case Simbolo.EnumTipo.real:
                            //Entero real
                            return new Expresion(Simbolo.EnumTipo.boleano, int.Parse(expresion1.valor.ToString()) <= Double.Parse(expresion2.valor.ToString()));
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
            if(root.ChildNodes.Count == 4)
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
                        //REPORTAR ERROR
                    }
                    else
                    {
                        //REPORTAR ERROR el id no es de tipo arreglo o tipo objeto, es una variable normal
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
                        //AGREGAR ERROR
                        simbolo.tipo = Simbolo.EnumTipo.error;
                    }
                }
                else
                {
                    //AGREGAR ERROR
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
                    Expresion tmp = buscarVariable(root.ChildNodes[1].ChildNodes[0], entorno);
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
            else if(root.ChildNodes.Count == 2)
            {
                agregarCamposObjeto(root.ChildNodes[0], objeto, entorno);
                agregarCamposObjeto(root.ChildNodes[1], objeto, entorno);
            }
        }
        private void recorrer(ParseTreeNode root, Entorno entorno)
        {
            if (!parar && !continuar) //Comprueba si existe un break o continue
            {
                Entorno nuevoEntorno;
                Simbolo simbolo = null;
                Expresion expresion;
                switch (root.ToString())
                {
                    case "CASE":
                        ejecutarCase(root, entorno);
                        break;
                    case "IF":
                        ejecutarIf(root, entorno);
                        break;
                    case "REPEAT":
                        ejecutarRepeat(root, entorno);
                        break;
                    case "WHILE":
                        ejecutarWhile(root, entorno);
                        break;
                    case "FUNCION":
                        nuevoEntorno = new Entorno(entorno, "");
                        ejecutarFuncion(root, nuevoEntorno);
                        break;
                    case "PROCEDIMIENTO":
                        nuevoEntorno = new Entorno(entorno, "");
                        ejecutarProcedimiento(root, nuevoEntorno);
                        break;
                    case "LLAMADA":
                        ejecutarLlamada(root, entorno);
                        break;
                    case "Z_TIPOS":
                        if(root.ChildNodes.Count != 0)
                        {
                            string nombreTipo = removerExtras(root.ChildNodes[0].ToString());
                            if (root.ChildNodes[2].ChildNodes[0].ToString().Contains("object"))
                            {
                                //Es un objeto
                                Objeto nuevoObjeto = new Objeto(nombreTipo);
                                agregarCamposObjeto(root.ChildNodes[2].ChildNodes[1], nuevoObjeto, entorno);
                                agregarCamposObjeto(root.ChildNodes[2].ChildNodes[2], nuevoObjeto, entorno);
                                entorno.insertar(nombreTipo, new Simbolo(Simbolo.EnumTipo.objeto, nuevoObjeto), root.ChildNodes[2].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[0].Token.Location.Column);
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
                                        if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("real"))
                                        {
                                            nuevoArreglo = new Objeto(nombreTipo);
                                            nuevoArreglo.arreglo = new double[index];
                                            nuevoArreglo.tipo = Simbolo.EnumTipo.real;
                                        }
                                        else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("boolean"))
                                        {
                                            nuevoArreglo = new Objeto(nombreTipo);
                                            nuevoArreglo.arreglo = new bool[index];
                                            nuevoArreglo.tipo = Simbolo.EnumTipo.boleano;
                                        }
                                        else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("integer"))
                                        {
                                            nuevoArreglo = new Objeto(nombreTipo);
                                            nuevoArreglo.arreglo = new int[index];
                                            nuevoArreglo.tipo = Simbolo.EnumTipo.entero;
                                        }
                                        else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("string"))
                                        {
                                            nuevoArreglo = new Objeto(nombreTipo);
                                            nuevoArreglo.arreglo = new string[index];
                                            nuevoArreglo.tipo = Simbolo.EnumTipo.cadena;
                                        }
                                        else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("id"))
                                        {
                                            Expresion tmp = buscarVariable(root.ChildNodes[2].ChildNodes[3].ChildNodes[0], entorno);
                                            if (tmp.tipo == Simbolo.EnumTipo.objeto)
                                            {
                                                nuevoArreglo = new Objeto(nombreTipo);
                                                nuevoArreglo.arreglo = new Objeto[index];
                                                for(int i = 0; i < index; i++)
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
                                                //REPORTAR ERROR ver error
                                            }
                                            else
                                            {
                                                //REPORTAR ERROR el id no es de tipo arreglo o de tipo objeto
                                            }
                                        }
                                        if (nuevoArreglo.arreglo != null)
                                        {
                                            entorno.insertar(nombreTipo, new Simbolo(Simbolo.EnumTipo.arreglo, nuevoArreglo), root.ChildNodes[2].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[0].Token.Location.Column);
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
                                                    if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("real"))
                                                    {
                                                        nuevoArreglo = new Objeto(nombreTipo);
                                                        nuevoArreglo.arreglo = new double[index1, index2];
                                                        nuevoArreglo.tipo = Simbolo.EnumTipo.real;
                                                    }
                                                    else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("boolean"))
                                                    {
                                                        nuevoArreglo = new Objeto(nombreTipo);
                                                        nuevoArreglo.arreglo = new bool[index1, index2];
                                                        nuevoArreglo.tipo = Simbolo.EnumTipo.boleano;
                                                    }
                                                    else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("integer"))
                                                    {
                                                        nuevoArreglo = new Objeto(nombreTipo);
                                                        nuevoArreglo.arreglo = new int[index1, index2];
                                                        nuevoArreglo.tipo = Simbolo.EnumTipo.entero;
                                                    }
                                                    else if (root.ChildNodes[2].ChildNodes[3].ChildNodes[0].ToString().Contains("string"))
                                                    {
                                                        nuevoArreglo = new Objeto(nombreTipo);
                                                        nuevoArreglo.arreglo = new string[index1, index2];
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
                                                            //REPORTAR ERROR ver error
                                                        }
                                                        else
                                                        {
                                                            //REPORTAR ERROR el id no es de tipo arreglo o de tipo objeto
                                                        }
                                                    }
                                                    if (nuevoArreglo != null)
                                                    {
                                                        entorno.insertar(nombreTipo, new Simbolo(Simbolo.EnumTipo.arreglo, nuevoArreglo), root.ChildNodes[2].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[0].Token.Location.Column);
                                                    }
                                                }
                                            }
                                            if (nuevoArreglo != null)
                                            {
                                                entorno.insertar(nombreTipo, new Simbolo(Simbolo.EnumTipo.arreglo, nuevoArreglo), root.ChildNodes[2].ChildNodes[0].Token.Location.Line, root.ChildNodes[2].ChildNodes[0].Token.Location.Column);
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
                            recorrer(root.ChildNodes[4], entorno);
                        }
                        break;
                    case "ASIGNACION":
                        ejecutarAsignacion(root, entorno);
                        break;
                    case "FOR":
                        ejecutarFor(root, entorno);
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
                    case "ABAJO":
                        break;
                    case "ARRIBA":
                        break;
                    case "FUNCION_HEAD":
                        break;
                    case "PROCEDIMIENTO_HEAD":
                        break;
                    case "OPCION_CASE":
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
                                        //REPORTAR ERROR las constantes solo pueden ser de datos primitivos
                                    }
                                    expresion = resolverExpresion(root.ChildNodes[3], entorno);
                                    if (expresion.tipo == Simbolo.EnumTipo.error)
                                    {
                                        //AGREGAR ERROR
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
                                                //AGREGAR ERROR
                                            }
                                        }
                                        else
                                        {
                                            simbolo.valor = expresion.valor;
                                        }
                                    }
                                    if (simbolo is null)
                                    {
                                        //AGREGAR ERROR el error se reporta arriba
                                    }
                                    else if(simbolo.tipo != Simbolo.EnumTipo.error)
                                    {
                                        simbolo.constante = true;
                                        entorno.insertar(removerExtras(root.ChildNodes[0].ToString()), simbolo, root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column);
                                    }
                                    else
                                    {
                                        //AGREGAR ERROR ver error en simbolo
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
                                            simbolo = new Simbolo(tmp.tipo, tmp.valor);
                                        }
                                        else if (tmp.tipo == Simbolo.EnumTipo.error)
                                        {
                                            //REPORTAR ERROR ver error
                                        }
                                        else
                                        {
                                            //REPORTAR ERROR el id no es de tipo arreglo o de tipo objeto
                                        }
                                    }
                                    expresion = resolverExpresion(root.ChildNodes[3], entorno);
                                    if (expresion.tipo == Simbolo.EnumTipo.error)
                                    {
                                        //AGREGAR ERROR
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
                                                //AGREGAR ERROR
                                            }
                                        }
                                        else
                                        {
                                            simbolo.valor = expresion.valor;
                                        }
                                    }
                                    if(simbolo is null)
                                    {
                                        //AGREGAR ERROR el error se reporta arriba
                                    }
                                    else if (simbolo.tipo != Simbolo.EnumTipo.error)
                                    {
                                        entorno.insertar(removerExtras(root.ChildNodes[0].ToString()), simbolo, root.ChildNodes[0].Token.Location.Line, root.ChildNodes[0].Token.Location.Column);
                                    }
                                    else
                                    {
                                        //AGREGAR ERROR ver error en simbolo
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
                                            simbolo = new Simbolo(tmp.tipo, tmp.valor);
                                        }
                                        else if (tmp.tipo == Simbolo.EnumTipo.error)
                                        {
                                            //REPORTAR ERROR ver error
                                        }
                                        else
                                        {
                                            //REPORTAR ERROR el id no es de tipo arreglo o de tipo objeto
                                        }
                                    }
                                    if (simbolo is null)
                                    {
                                        //AGREGAR ERROR el error se reporta arriba
                                    }
                                    else
                                    {
                                        while (temp.ChildNodes.Count != 0)
                                        {
                                            entorno.insertar(removerExtras(temp.ChildNodes[0].ToString()), simbolo, temp.ChildNodes[0].Token.Location.Line, temp.ChildNodes[0].Token.Location.Column);
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
                                            simbolo = new Simbolo(tmp.tipo, tmp.valor);
                                        }
                                        else if (tmp.tipo == Simbolo.EnumTipo.error)
                                        {
                                            //REPORTAR ERROR ver error
                                        }
                                        else
                                        {
                                            //REPORTAR ERROR el id no es de tipo arreglo o de tipo objeto
                                        }
                                    }
                                    if (simbolo is null)
                                    {
                                        //AGREGAR ERROR el error se reporta arriba
                                    }
                                    else
                                    {
                                        entorno.insertar(removerExtras(temp.ChildNodes[0].ToString()), simbolo, temp.ChildNodes[0].Token.Location.Line, temp.ChildNodes[0].Token.Location.Column);
                                    }
                                }
                            }
                        }
                        break;
                    case "ESTRUCTURA":
                        break;
                    case "CONTROLADOR":
                        break;
                    case "VALOR":
                        break;
                    case "OPERADOR":
                        break;
                    case "PA":
                        break;
                    case "PF":
                        break;
                    case "PFVL":
                        break;
                    case "PFVR":
                        break;
                    case "RANGO":
                        break;
                    case "R_ID":
                        break;
                    case "INDICE":
                        break;
                    case "T_DATO":
                        break;
                    case "T_ELEMENTAL":
                        break;
                    case "T_ESTRUCTURADO":
                        break;
                    case "T_ORDINAL":
                        break;
                    case "VARIABLE":
                        break;
                    default:
                        if (!root.ToString().Contains(" ("))
                            MessageBox.Show("Falto agregar " + root.ToString() + " al switch");
                        break;
                }
            }
        }
    }
}