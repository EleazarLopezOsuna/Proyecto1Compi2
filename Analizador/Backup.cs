using System;
using System.Collections.Generic;
using System.Text;
using Irony.Ast;
using Irony.Parsing;

namespace Proyecto1_Compiladores2.Analizador
{
    class Backup : Grammar
    {
        public Backup() : base(caseSensitive: false)
        {
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

            #region Palabras Reservadas
            var tipo_string = ToTerm("string");
            var tipo_integer = ToTerm("integer");
            var tipo_real = ToTerm("real");
            var tipo_boolean = ToTerm("boolean");
            var tipo_object = ToTerm("object");
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
            var continue_res = ToTerm("continue");
            var break_res = ToTerm("break");
            var exit_res = ToTerm("exit");
            var write_res = ToTerm("write");
            var writeln_res = ToTerm("writeln");
            var graficar_res = ToTerm("graficar_ts");
            var logicoOR = "or";
            var logicoAND = "and";
            var logicoNOT = "not";
            #endregion

            #region Presedencia
            this.RegisterOperators(4, Associativity.Neutral, "=", "<>");
            this.RegisterOperators(5, Associativity.Neutral, ">", ">=", "<", "<=");
            this.RegisterOperators(6, Associativity.Left, "+", "-", logicoOR);
            this.RegisterOperators(7, Associativity.Left, "*", "/", "%", logicoAND);
            this.RegisterOperators(9, Associativity.Right, logicoNOT);
            #endregion

            #region No Terminales
            NonTerminal INICIO = new NonTerminal("INICIO"); //Completado
            NonTerminal PROGRAMA = new NonTerminal("PROGRAMA"); //Completado
            NonTerminal CUERPO = new NonTerminal("CUERPO"); //Completado
            NonTerminal VARIABLE = new NonTerminal("VARIABLE"); //Completado
            NonTerminal CONSTANTE = new NonTerminal("CONSTANTE"); //Completado
            NonTerminal OBJETO = new NonTerminal("OBJETO"); //Completado
            NonTerminal LISTA_IDENTIFICADORES = new NonTerminal("LISTA_IDENTIFICADORES"); //Completado
            NonTerminal TIPO = new NonTerminal("TIPO"); //Completado
            NonTerminal EXPRESION = new NonTerminal("EXPRESION"); //Completado
            NonTerminal OPCIONES_OBJETO = new NonTerminal("OPCIONES_OBJETO"); //Completado
            NonTerminal INSTRUCCIONES_PRI = new NonTerminal("INSTRUCCIONES"); //Completado
            NonTerminal IF_FUNC = new NonTerminal("IF"); //Completado
            NonTerminal WHILE_FUNC = new NonTerminal("WHILE"); //Completado
            NonTerminal CASE_FUNC = new NonTerminal("CASE"); //Completado
            NonTerminal REPEAT_FUNC = new NonTerminal("REPEAT"); //Completado
            NonTerminal FOR_FUNC = new NonTerminal("FOR"); //Completado
            NonTerminal REASIGNACION = new NonTerminal("REASIGNACION"); //Completado
            NonTerminal IMPRIMIR = new NonTerminal("IMPRIMIR"); //Completado
            NonTerminal GRAFICAR = new NonTerminal("GRAFICAR"); //Completado
            NonTerminal CONTINUIDAD = new NonTerminal("CONTINUIDAD"); //Completado
            NonTerminal OPCIONES_FUNC = new NonTerminal("INSTRUCCIONES"); //Completado
            NonTerminal LISTA_CASES = new NonTerminal("LISTA_CASES"); //Completado
            NonTerminal LISTA_EXPRESIONES = new NonTerminal("LISTA_EXPRESIONES"); //Completado
            NonTerminal LISTA_PARAMETROS = new NonTerminal("LISTA_PARAMETROS"); //Completado
            NonTerminal ATRIBUTOS_OBJECT = new NonTerminal("ATRIBUTOS_OBJECT"); //Completado
            NonTerminal TIPO_OBJETO = new NonTerminal("TIPO_OBJETO"); //Completado
            NonTerminal ARREGLO = new NonTerminal("ARREGLO"); //Completado
            NonTerminal FUNCION = new NonTerminal("FUNCION");
            NonTerminal METODO = new NonTerminal("METODO");
            #endregion

            #region Gramatica
            INICIO.Rule = program_res + id + ";" + PROGRAMA + begin_res + CUERPO + end_res + "."
                ;

            PROGRAMA.Rule = VARIABLE + PROGRAMA
                | CONSTANTE + PROGRAMA
                | OBJETO + PROGRAMA
                | ARREGLO + PROGRAMA
                | VARIABLE
                | CONSTANTE
                | OBJETO
                | ARREGLO
                //| FUNCION + end_res + ";" 
                //| METODO + end_res + ";"
                ;

            VARIABLE.Rule = var_res + LISTA_IDENTIFICADORES + ":" + TIPO + ";"
                | var_res + id + ":" + TIPO + ";"
                | var_res + id + ":" + TIPO + "=" + EXPRESION + ";"
                ;

            CONSTANTE.Rule = const_res + id + ":" + TIPO + "=" + EXPRESION + ";"
                ;

            EXPRESION.Rule = EXPRESION + "+" + EXPRESION
                | EXPRESION + "-" + EXPRESION
                | EXPRESION + "*" + EXPRESION
                | EXPRESION + "/" + EXPRESION
                | EXPRESION + "%" + EXPRESION
                | EXPRESION + ">" + EXPRESION
                | EXPRESION + "<" + EXPRESION
                | EXPRESION + ">=" + EXPRESION
                | EXPRESION + "<=" + EXPRESION
                | EXPRESION + "=" + EXPRESION
                | EXPRESION + "<>" + EXPRESION
                | EXPRESION + logicoAND + EXPRESION
                | EXPRESION + logicoNOT + EXPRESION
                | EXPRESION + logicoOR + EXPRESION
                | "(" + EXPRESION + ")"
                | "-" + EXPRESION
                | cadena | N_entero | N_real | boleano | id
                | id + "(" + ")"
                | id + "(" + LISTA_PARAMETROS + ")"
                | id + ATRIBUTOS_OBJECT
                | id + "[" + EXPRESION + "]"
                ;

            ATRIBUTOS_OBJECT.Rule = "." + ATRIBUTOS_OBJECT
                | id
                ;

            REASIGNACION.Rule = id + ":" + "=" + EXPRESION + ";"
                | ATRIBUTOS_OBJECT + ":" + "=" + EXPRESION + ";"
                ;

            IMPRIMIR.Rule = write_res + "(" + LISTA_EXPRESIONES + ")" + ";"
                | writeln_res + "(" + LISTA_EXPRESIONES + ")" + ";"
                ;

            CUERPO.Rule = INSTRUCCIONES_PRI + CUERPO
                | INSTRUCCIONES_PRI
                ;

            GRAFICAR.Rule = graficar_res + "(" + ")" + ";"
                ;

            CONTINUIDAD.Rule = break_res + ";" | continue_res + ";" | exit_res + "(" + EXPRESION + ")" + ";"
                ;

            LISTA_IDENTIFICADORES.Rule = id + "," + LISTA_IDENTIFICADORES
                | id
                ;

            IF_FUNC.Rule = if_res + EXPRESION + then_res + OPCIONES_FUNC
                | if_res + EXPRESION + then_res + OPCIONES_FUNC + else_res + IF_FUNC + OPCIONES_FUNC
                | if_res + EXPRESION + then_res + OPCIONES_FUNC + else_res + OPCIONES_FUNC
                ;

            CASE_FUNC.Rule = case_res + EXPRESION + of_res + LISTA_CASES + end_res + ";"
                | case_res + EXPRESION + of_res + LISTA_CASES + else_res + OPCIONES_FUNC + end_res + ";"
                ;

            LISTA_CASES.Rule = LISTA_CASES + LISTA_EXPRESIONES + ":" + OPCIONES_FUNC
                | LISTA_EXPRESIONES + ":" + OPCIONES_FUNC
                ;

            LISTA_EXPRESIONES.Rule = LISTA_EXPRESIONES + "," + EXPRESION
                | EXPRESION
                ;

            INSTRUCCIONES_PRI.Rule = IF_FUNC | WHILE_FUNC | CASE_FUNC | REPEAT_FUNC | FOR_FUNC | REASIGNACION
                | IMPRIMIR | GRAFICAR | CONTINUIDAD
                ;

            WHILE_FUNC.Rule = while_res + EXPRESION + do_res + OPCIONES_FUNC
                ;

            FOR_FUNC.Rule = for_res + id + ":" + "=" + EXPRESION + to_res + EXPRESION + do_res + OPCIONES_FUNC
                | for_res + id + ":" + "=" + EXPRESION + to_res + EXPRESION + do_res + ";"
                ;

            REPEAT_FUNC.Rule = repeat_res + CUERPO + until_res + EXPRESION + ";"
                | repeat_res + until_res + EXPRESION + ";"
                ;

            OPCIONES_FUNC.Rule = INSTRUCCIONES_PRI
                | begin_res + CUERPO + end_res + ";"
                ;

            TIPO.Rule = tipo_boolean | tipo_integer | tipo_real | tipo_string | id
                ;

            LISTA_PARAMETROS.Rule = EXPRESION + "," + LISTA_PARAMETROS
                | EXPRESION
                ;

            OBJETO.Rule = type_res + id + "=" + TIPO_OBJETO + OPCIONES_OBJETO + end_res + ";"
                ;

            ARREGLO.Rule = type_res + id + "=" + array_res + "[" + EXPRESION + ".." + EXPRESION + "]" + of_res + TIPO + ";"
                ;

            TIPO_OBJETO.Rule = tipo_object
                | id
                ;

            OPCIONES_OBJETO.Rule = VARIABLE + OPCIONES_OBJETO
                | CONSTANTE + OPCIONES_OBJETO
                | VARIABLE
                | CONSTANTE
                ;

            //VARIABLE_OBJETO.Rule = 

            //Para arriba funciona

            #endregion

            #region Preferencias
            this.Root = INICIO;
            #endregion
        }
    }
}
