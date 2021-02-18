using System;
using System.Collections.Generic;
using System.Text;
using Irony.Ast;
using Irony.Parsing;

namespace Proyecto1_Compiladores2.Analizador
{
    class GramaticaTraductor : Grammar
    {
        public GramaticaTraductor() : base(caseSensitive: false)
        {
            #region Palabras Reservadas
            var tipo_string = ToTerm("string");
            var tipo_integer = ToTerm("integer");
            var tipo_real = ToTerm("real");
            var tipo_boolean = ToTerm("boolean");
            var object_res = ToTerm("object");
            var type_res = ToTerm("type");
            var program_res = ToTerm("program");
            var var_res = ToTerm("var");
            var const_res = ToTerm("const");
            var begin_res = ToTerm("begin");
            var end_res = ToTerm("end");
            var array_res = ToTerm("array");
            var of_res = ToTerm("of");
            var procedure_res = ToTerm("procedure");
            var function_res = ToTerm("function");
            var case_res = ToTerm("case");
            var do_res = ToTerm("do");
            var else_res = ToTerm("else");
            var for_res = ToTerm("for");
            var repeat_res = ToTerm("repeat");
            var then_res = ToTerm("then");
            var to_res = ToTerm("to");
            var until_res = ToTerm("until");
            var while_res = ToTerm("while");
            var if_res = ToTerm("if");
            var downto_res = ToTerm("downto");
            #endregion

            #region Expresiones Regulares
            RegexBasedTerminal N_entero = new RegexBasedTerminal("entero", "[0-9]+");
            StringLiteral cadena = new StringLiteral("cadena", "\'", StringOptions.AllowsLineBreak);
            RegexBasedTerminal N_real = new RegexBasedTerminal("real", @"-?[0-9]+(\.[0-9]+)?");
            RegexBasedTerminal boleano = new RegexBasedTerminal("boleano", "(true|false)");
            IdentifierTerminal id = new IdentifierTerminal("id");
            CommentTerminal comentarioLinea = new CommentTerminal("comentario linea", "//", "\n", "\r\n");
            CommentTerminal comentarioBloque = new CommentTerminal("comentario bloque", "(*", "*)");
            CommentTerminal comentarioBloque2 = new CommentTerminal("comentario bloque", "{", "}");

            base.NonGrammarTerminals.Add(comentarioBloque);
            base.NonGrammarTerminals.Add(comentarioLinea);
            base.NonGrammarTerminals.Add(comentarioBloque2);
            #endregion

            #region No Terminales
            NonTerminal IF_D = new NonTerminal("IF");
            NonTerminal CASE = new NonTerminal("CASE");
            NonTerminal IF_S = new NonTerminal("IF");
            NonTerminal FOR = new NonTerminal("FOR");
            NonTerminal ABAJO = new NonTerminal("ABAJO");
            NonTerminal ARRIBA = new NonTerminal("ARRIBA");
            NonTerminal REPEAT = new NonTerminal("REPEAT");
            NonTerminal WHILE = new NonTerminal("WHILE");
            NonTerminal FUNCION_HEAD = new NonTerminal("FUNCION_HEAD");
            NonTerminal PROCEDIMIENTO_HEAD = new NonTerminal("PROCEDIMIENTO_HEAD");
            NonTerminal OPCION_CASE = new NonTerminal("OPCION_CASE");
            NonTerminal DECLARACION_CAMPOS_TYPE = new NonTerminal("DECLARACION_CAMPOS_TYPE");
            NonTerminal D_CONSTANTE = new NonTerminal("D_CONSTANTE");
            NonTerminal FUNCION = new NonTerminal("FUNCION");
            NonTerminal PROCEDIMIENTO = new NonTerminal("PROCEDIMIENTO");
            NonTerminal SUBPROGRAMA = new NonTerminal("SUBPROGRAMA");
            NonTerminal D_VARIABLE = new NonTerminal("D_VARIABLE");
            NonTerminal ESTRUCTURA = new NonTerminal("ESTRUCTURA");
            NonTerminal CONTROLADOR = new NonTerminal("CONTROLADOR");
            NonTerminal EXPRESION = new NonTerminal("EXPRESION");
            NonTerminal EB = new NonTerminal("EXPRESION");
            NonTerminal EU = new NonTerminal("EXPRESION");
            NonTerminal VALOR = new NonTerminal("VALOR");
            NonTerminal LLAMADA = new NonTerminal("LLAMADA");
            NonTerminal OB = new NonTerminal("OPERADOR");
            NonTerminal OU = new NonTerminal("OPERADOR");
            NonTerminal PA = new NonTerminal("PA");
            NonTerminal PF = new NonTerminal("PF");
            NonTerminal PFVL = new NonTerminal("PFVL"); //Parametros Formales por VaLor
            NonTerminal PFVR = new NonTerminal("PFVR"); //Parametros Formales por VARIABLE
            NonTerminal PROGRAMA = new NonTerminal("PROGRAMA");
            NonTerminal RANGO = new NonTerminal("RANGO");
            NonTerminal R_OPCION_CASE = new NonTerminal("OPCION_CASE");
            NonTerminal R_DECLARACION_CAMPOS_TYPE = new NonTerminal("DECLARACION_CAMPOS_TYPE");
            NonTerminal R_CONSTANTE = new NonTerminal("D_CONSTANTE");
            NonTerminal R_SUBPROGRAMA = new NonTerminal("SUBPROGRAMA");
            NonTerminal R_VARIABLE = new NonTerminal("D_VARIABLE");
            NonTerminal R_TYPE = new NonTerminal("Z_TIPOS");
            NonTerminal R_EXPRESION = new NonTerminal("EXPRESION");
            NonTerminal R_ID = new NonTerminal("R_ID");
            NonTerminal R_OBJETO_CAMPO = new NonTerminal("ESTRUCTURA");
            NonTerminal R_PA = new NonTerminal("PA");
            NonTerminal R_PF = new NonTerminal("PF");
            NonTerminal R_PFVL = new NonTerminal("PFVL");
            NonTerminal R_RANGO = new NonTerminal("RANGO");
            NonTerminal R_SENTENCIA = new NonTerminal("SENTENCIA");
            NonTerminal R_INDICE = new NonTerminal("INDICE");
            NonTerminal SENTENCIA = new NonTerminal("SENTENCIA");
            NonTerminal BEGIN_END = new NonTerminal("BEGIN_END");
            NonTerminal ASIGNACION = new NonTerminal("ASIGNACION");
            NonTerminal T_DATO = new NonTerminal("T_DATO");
            NonTerminal T_ELEMENTAL = new NonTerminal("T_ELEMENTAL");
            NonTerminal T_ESTRUCTURADO = new NonTerminal("T_ESTRUCTURADO");
            NonTerminal T_ORDINAL = new NonTerminal("T_ORDINAL");
            NonTerminal VARIABLE = new NonTerminal("VARIABLE");
            NonTerminal Z_CONSTANTES = new NonTerminal("Z_CONSTANTES");
            NonTerminal Z_SUBPROGRAMAS = new NonTerminal("Z_SUBPROGRAMAS");
            NonTerminal Z_VARIABLES = new NonTerminal("Z_VARIABLES");
            NonTerminal Z_DECLARACIONES = new NonTerminal("Z_DECLARACIONES");
            NonTerminal Z_TIPOS = new NonTerminal("Z_TIPOS");
            #endregion

            #region Gramatica

            IF_D.Rule = if_res + EXPRESION + then_res + SENTENCIA + else_res + SENTENCIA
                ;

            CASE.Rule = case_res + EXPRESION + of_res + OPCION_CASE + R_OPCION_CASE + else_res + SENTENCIA + end_res
                | case_res + EXPRESION + of_res + OPCION_CASE + R_OPCION_CASE + end_res
                ;

            IF_S.Rule = if_res + EXPRESION + then_res + SENTENCIA
                ;

            FOR.Rule = for_res + ASIGNACION + ARRIBA + SENTENCIA
                | for_res + ASIGNACION + ABAJO + SENTENCIA
                ;

            ABAJO.Rule = downto_res + EXPRESION + do_res
                ;

            ARRIBA.Rule = to_res + EXPRESION + do_res
                ;

            REPEAT.Rule = repeat_res + SENTENCIA + R_SENTENCIA + until_res + EXPRESION
                ;

            WHILE.Rule = while_res + EXPRESION + do_res + SENTENCIA
                ;

            FUNCION_HEAD.Rule = function_res + id + ":" + id + ";"
                | function_res + id + ":" + T_ELEMENTAL + ";"
                | function_res + id + "(" + PFVL + R_PFVL + ")" + ":" + id + ";"
                | function_res + id + "(" + PFVL + R_PFVL + ")" + ":" + T_ELEMENTAL + ";"
                | function_res + id + "(" + ")" + ":" + id + ";"
                | function_res + id + "(" + ")" + ":" + T_ELEMENTAL + ";"
                ;

            PROCEDIMIENTO_HEAD.Rule = procedure_res + id + ";"
                | procedure_res + id + "(" + PF + R_PF + ")" + ";"
                | procedure_res + id + "(" + ")" + ";"
                ;

            OPCION_CASE.Rule = RANGO + ":" + SENTENCIA
                | Empty
                ;

            DECLARACION_CAMPOS_TYPE.Rule = var_res + id + R_ID + ":" + id
                | var_res + id + R_ID + ":" + T_ELEMENTAL
                | id + R_ID + ":" + id
                | id + R_ID + ":" + T_ELEMENTAL
                | const_res + id + ":" + id + "=" + EXPRESION
                | const_res + id + ":" + T_ELEMENTAL + "=" + EXPRESION
                | id + "=" + EXPRESION
                ;

            D_CONSTANTE.Rule = id + "=" + EXPRESION
                | id + ":" + T_DATO + "=" + EXPRESION
                ;

            FUNCION.Rule = FUNCION_HEAD + Z_DECLARACIONES + BEGIN_END
                ;

            PROCEDIMIENTO.Rule = PROCEDIMIENTO_HEAD + Z_DECLARACIONES + BEGIN_END
                ;

            SUBPROGRAMA.Rule = FUNCION
                | PROCEDIMIENTO
                ;

            D_VARIABLE.Rule = id + R_ID + ":" + T_DATO
                | id + ":" + T_DATO + "=" + EXPRESION
                | id + ":" + T_DATO
                ;

            ESTRUCTURA.Rule = id + "[" + EXPRESION + R_EXPRESION + "]"
                | id + R_OBJETO_CAMPO
                | LLAMADA + R_OBJETO_CAMPO
                ;

            CONTROLADOR.Rule = IF_S
                | IF_D
                | CASE
                | WHILE
                | REPEAT
                | FOR
                ;

            EXPRESION.Rule = VALOR
                | VARIABLE
                | EU
                | EB
                | LLAMADA
                | id + "(" + EXPRESION + ")"
                | "(" + EXPRESION + ")"
                ;

            EB.Rule = EXPRESION + OB + EXPRESION
                ;

            EU.Rule = OU + EXPRESION
                ;

            VALOR.Rule = N_entero | N_real | boleano | cadena
                ;

            LLAMADA.Rule = id
                | id + "(" + ")"
                | id + "(" + PA + R_PA + ")"
                ;

            OB.Rule = ToTerm("+")
                | ToTerm("-")
                | ToTerm("*")
                | ToTerm("%")
                | ToTerm("/")
                | ToTerm("and")
                | ToTerm("or")
                | ToTerm("=")
                | ToTerm("<>")
                | ToTerm("<")
                | ToTerm(">")
                | ToTerm("<=")
                | ToTerm(">=")
                ;

            OU.Rule = ToTerm("not")
                | ToTerm("-")
                ;

            PA.Rule = EXPRESION
                | VARIABLE
                ;

            PF.Rule = PFVL
                | PFVR
                ;

            PFVL.Rule = id + R_ID + ":" + id
                ;

            PFVR.Rule = var_res + id + R_ID + ":" + id
                ;

            PROGRAMA.Rule = program_res + id + ";" + Z_DECLARACIONES + BEGIN_END + "."
                ;

            RANGO.Rule = EXPRESION
                | EXPRESION + ".." + EXPRESION
                | RANGO + R_RANGO
                ;

            R_OPCION_CASE.Rule = ";" + OPCION_CASE + R_OPCION_CASE
                | Empty
                ;

            R_DECLARACION_CAMPOS_TYPE.Rule = ";" + DECLARACION_CAMPOS_TYPE + R_DECLARACION_CAMPOS_TYPE
                | DECLARACION_CAMPOS_TYPE + R_DECLARACION_CAMPOS_TYPE
                | Empty
                | ";"
                ;

            R_CONSTANTE.Rule = ";" + D_CONSTANTE + R_CONSTANTE
                | ";"
                ;

            R_SUBPROGRAMA.Rule = ";" + SUBPROGRAMA + R_SUBPROGRAMA
                | Empty
                ;

            R_VARIABLE.Rule = ";" + D_VARIABLE + R_VARIABLE
                | ";"
                ;

            R_TYPE.Rule = ";" + id + "=" + T_DATO + R_TYPE
                | Empty
                ;

            R_EXPRESION.Rule = "," + EXPRESION + R_EXPRESION
                | Empty
                ;

            R_ID.Rule = "," + id + R_ID
                | Empty
                ;

            R_OBJETO_CAMPO.Rule = "." + id + R_OBJETO_CAMPO
                | "." + id + "[" + EXPRESION + R_EXPRESION + "]" + R_OBJETO_CAMPO
                | "." + LLAMADA + R_OBJETO_CAMPO
                | Empty
                ;

            R_PA.Rule = "," + PA + R_PA
                | Empty
                ;

            R_PF.Rule = ";" + PF + R_PF
                | "," + PF + R_PF
                | Empty
                ;

            R_PFVL.Rule = ";" + PFVL + R_PFVL
                | "," + PFVL + R_PFVL
                | Empty
                ;

            R_RANGO.Rule = "," + RANGO + R_RANGO
                | Empty
                ;

            R_SENTENCIA.Rule = ";" + SENTENCIA + R_SENTENCIA
                | Empty
                ;

            R_INDICE.Rule = "," + T_ORDINAL + R_INDICE
                | Empty
                ;

            SENTENCIA.Rule = ASIGNACION
                | BEGIN_END
                | LLAMADA
                | CONTROLADOR
                | Empty
                ;

            BEGIN_END.Rule = begin_res + SENTENCIA + R_SENTENCIA + end_res
                | begin_res + end_res
                ;

            ASIGNACION.Rule = VARIABLE + ":=" + EXPRESION
                ;

            T_DATO.Rule = tipo_integer
                | tipo_boolean
                | tipo_real
                | tipo_string
                | array_res + "[" + T_ORDINAL + R_INDICE + "]" + of_res + T_DATO
                | object_res + DECLARACION_CAMPOS_TYPE + R_DECLARACION_CAMPOS_TYPE + end_res
                | id
                ;

            T_ELEMENTAL.Rule = T_ORDINAL
                | tipo_real
                | id
                ;

            T_ESTRUCTURADO.Rule = array_res + "[" + T_ORDINAL + R_INDICE + "]" + of_res + T_DATO
                | id
                ;

            T_ORDINAL.Rule = EXPRESION
                | EXPRESION + ".." + EXPRESION
                ;

            VARIABLE.Rule = id
                | ESTRUCTURA
                ;

            Z_CONSTANTES.Rule = Empty
                | const_res + D_CONSTANTE + R_CONSTANTE + Z_DECLARACIONES
                | D_CONSTANTE + R_CONSTANTE + Z_DECLARACIONES
                ;

            Z_VARIABLES.Rule = Empty
                | var_res + D_VARIABLE + R_VARIABLE + Z_DECLARACIONES
                | D_VARIABLE + R_VARIABLE + Z_DECLARACIONES
                ;

            Z_DECLARACIONES.Rule = Z_CONSTANTES
                | Z_TIPOS
                | Z_VARIABLES
                | Z_SUBPROGRAMAS
                | Empty
                ;

            Z_TIPOS.Rule = Empty
                | type_res + id + "=" + T_DATO + ";" + R_TYPE + Z_DECLARACIONES
                | id + "=" + T_DATO + ";" + R_TYPE + Z_DECLARACIONES
                ;

            Z_SUBPROGRAMAS.Rule = Empty
                | SUBPROGRAMA + ";" + R_SUBPROGRAMA + Z_DECLARACIONES
                ;

            #endregion

            #region Preferencias
            this.Root = PROGRAMA;
            #endregion
        }
    }
}