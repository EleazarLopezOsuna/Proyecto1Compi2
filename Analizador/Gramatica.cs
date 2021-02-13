using System;
using System.Collections.Generic;
using System.Text;
using Irony.Ast;
using Irony.Parsing;

namespace Proyecto1_Compiladores2.Analizador
{
    class Gramatica : Grammar
    {
        public Gramatica() : base(caseSensitive: false)
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
            var record_res = ToTerm("record");

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

            #region Presedencia
            this.RegisterOperators(4, Associativity.Neutral, "=", "<>");
            this.RegisterOperators(5, Associativity.Neutral, ">", ">=", "<", "<=");
            this.RegisterOperators(6, Associativity.Left, "+", "-", logicoOR);
            this.RegisterOperators(7, Associativity.Left, "*", "/", "%", logicoAND);
            this.RegisterOperators(9, Associativity.Right, logicoNOT);
            #endregion

            #region No Terminales
            NonTerminal AlternativaDoble = new NonTerminal("AlternativaDoble");
            NonTerminal AlternativaMultiple = new NonTerminal("AlternativaMultiple");
            NonTerminal AlternativaSimple = new NonTerminal("AlternativaSimple");
            NonTerminal BucleConNumeroFijoDeIteraciones = new NonTerminal("BucleConNumeroFijoDeIteraciones");
            NonTerminal BucleConSalidaAlFinal = new NonTerminal("BucleConSalidaAlFinal");
            NonTerminal BucleConSalidaAlPrincipio = new NonTerminal("BucleConSalidaAlPrincipio");
            NonTerminal CabeceraDeFuncion = new NonTerminal("CabeceraDeFuncion");
            NonTerminal CabeceraDeProcedimiento = new NonTerminal("CabeceraDeProcedimiento");
            NonTerminal CabeceraDePrograma = new NonTerminal("CabeceraDePrograma");
            NonTerminal Caso = new NonTerminal("Caso");
            NonTerminal Condicion = new NonTerminal("Condicion");
            NonTerminal ConversionDeTipo = new NonTerminal("ConversionDeTipo");
            NonTerminal CuerpoDeFuncion = new NonTerminal("CuerpoDeFuncion");
            NonTerminal CuerpoDeProcedimiento = new NonTerminal("CuerpoDeProcedimiento");
            NonTerminal CuerpoDePrograma = new NonTerminal("CuerpoDePrograma");
            NonTerminal DeclaracionDeCampos = new NonTerminal("DeclaracionDeCampos");
            NonTerminal DeclaracionDeConstante = new NonTerminal("DeclaracionDeConstante");
            NonTerminal DeclaracionDeFuncion = new NonTerminal("DeclaracionDeFuncion");
            NonTerminal DeclaracionDeProcedimiento = new NonTerminal("DeclaracionDeProcedimiento");
            NonTerminal DeclaracionDeSubprograma = new NonTerminal("DeclaracionDeSubprograma");
            NonTerminal DeclaracionDeVariables = new NonTerminal("DeclaracionDeVariables");
            NonTerminal DefinicionDeTipo = new NonTerminal("DefinicionDeTipo");
            NonTerminal ElementoDeArray = new NonTerminal("ElementoDeArray");
            NonTerminal ElementoDeEstructura = new NonTerminal("ElementoDeEstructura");
            NonTerminal ElementoDeRegistro = new NonTerminal("ElementoDeRegistro");
            NonTerminal EstructuraAlternativa = new NonTerminal("EstructuraAlternativa");
            NonTerminal EstructuraDeControl = new NonTerminal("EstructuraDeControl");
            NonTerminal EstructuraIterativa = new NonTerminal("EstructuraIterativa");
            NonTerminal Expresion = new NonTerminal("Expresion");
            NonTerminal ExpresionBinaria = new NonTerminal("ExpresionBinaria");
            NonTerminal ExpresionUnaria = new NonTerminal("ExpresionUnaria");
            NonTerminal Literal = new NonTerminal("Literal");
            NonTerminal LiteralDeTipoBoolean = new NonTerminal("LiteralDeTipoBoolean");
            NonTerminal LiteralDeTipoEntero = new NonTerminal("LiteralDeTipoEntero");
            NonTerminal LiteralDeTipoReal = new NonTerminal("LiteralDeTipoReal");
            NonTerminal LiteralDeTipoString = new NonTerminal("LiteralDeTipoString");
            NonTerminal Llamada = new NonTerminal("Llamada");
            NonTerminal OperadorBinario = new NonTerminal("OperadorBinario");
            NonTerminal OperadorUnario = new NonTerminal("OperadorUnario");
            NonTerminal ParametrosActuales = new NonTerminal("ParametrosActuales");
            NonTerminal ParametrosActualesPorValor = new NonTerminal("ParametrosActualesPorValor");
            NonTerminal ParametrosActualesPorVariable = new NonTerminal("ParametrosActualesPorVariable");
            NonTerminal ParametrosFormales = new NonTerminal("ParametrosFormales");
            NonTerminal ParametrosFormalesPorValor = new NonTerminal("ParametrosFormalesPorValor");
            NonTerminal ParametrosFormalesPorVariable = new NonTerminal("ParametrosFormalesPorVariable");
            NonTerminal Programa = new NonTerminal("Programa");
            NonTerminal RangoDeLiterales = new NonTerminal("RangoDeLiterales");
            NonTerminal RangoDeValores = new NonTerminal("RangoDeValores");
            NonTerminal RecursividadCaso = new NonTerminal("RecursividadCaso");
            NonTerminal RecursividadDeclaracionDeCampos = new NonTerminal("RecursividadDeclaracionDeCampos");
            NonTerminal RecursividadDeclaracionDeConstante = new NonTerminal("RecursividadDeclaracionDeConstante");
            NonTerminal RecursividadDeclaracionDeSubprograma = new NonTerminal("RecursividadDeclaracionDeSubprograma");
            NonTerminal RecursividadDeclaracionDeVariables = new NonTerminal("RecursividadDeclaracionDeVariables");
            NonTerminal RecursividadDefinicionDeTipo = new NonTerminal("RecursividadDefinicionDeTipo");
            NonTerminal RecursividadExpresion = new NonTerminal("RecursividadExpresion");
            NonTerminal RecursividadIdentificador = new NonTerminal("RecursividadIdentificador");
            NonTerminal RecursividadObjetoCampo = new NonTerminal("RecursividadObjetoCampo");
            NonTerminal RecursividadParametrosActuales = new NonTerminal("RecursividadParametrosActuales");
            NonTerminal RecursividadParametrosActualesPorValor = new NonTerminal("RecursividadParametrosActualesPorValor");
            NonTerminal RecursividadParametrosFormales = new NonTerminal("RecursividadParametrosFormales");
            NonTerminal RecursividadParametrosFormalesPorValor = new NonTerminal("RecursividadParametrosFormalesPorValor");
            NonTerminal RecursividadRangoDeLiterales = new NonTerminal("RecursividadRangoDeLiterales");
            NonTerminal RecursividadRangoDeValores = new NonTerminal("RecursividadRangoDeValores");
            NonTerminal RecursividadSentencia = new NonTerminal("RecursividadSentencia");
            NonTerminal RecursividadTipoIndice = new NonTerminal("RecursividadTipoIndice");
            NonTerminal Sentencia = new NonTerminal("Sentencia");
            NonTerminal SentenciaCompuesta = new NonTerminal("SentenciaCompuesta");
            NonTerminal SentenciaDeAsignacion = new NonTerminal("SentenciaDeAsignacion");
            NonTerminal TipoArray = new NonTerminal("TipoArray");
            NonTerminal TipoDeDato = new NonTerminal("TipoDeDato");
            NonTerminal TipoElemental = new NonTerminal("TipoElemental");
            NonTerminal TipoEstructurado = new NonTerminal("TipoEstructurado");
            NonTerminal TipoIndice = new NonTerminal("TipoIndice");
            NonTerminal TipoOrdinal = new NonTerminal("TipoOrdinal");
            NonTerminal TipoOrdinalPredefinido = new NonTerminal("TipoOrdinalPredefinido");
            NonTerminal TipoReal = new NonTerminal("TipoReal");
            NonTerminal TipoRegistro = new NonTerminal("TipoRegistro");
            NonTerminal TipoSimple = new NonTerminal("TipoSimple");
            NonTerminal TipoString = new NonTerminal("TipoString");
            NonTerminal TipoSubrango = new NonTerminal("TipoSubrango");
            NonTerminal Variable = new NonTerminal("Variable");
            NonTerminal ZonaDeDeclaracionDeConstantes = new NonTerminal("ZonaDeDeclaracionDeConstantes");
            NonTerminal ZonaDeDeclaracionDeSubprogramas = new NonTerminal("ZonaDeDeclaracionDeSubprogramas");
            NonTerminal ZonaDeDeclaracionDeVariables = new NonTerminal("ZonaDeDeclaracionDeVariables");
            NonTerminal ZonaDeDeclaraciones = new NonTerminal("ZonaDeDeclaraciones");
            NonTerminal ZonaDeDefinicionDeTipos = new NonTerminal("ZonaDeDefinicionDeTipos");
            #endregion

            #region Gramatica
            AlternativaDoble.Rule = if_res + Condicion + then_res + Sentencia + else_res + Sentencia
                ;

            AlternativaMultiple.Rule = case_res + Expresion + of_res + Caso + RecursividadCaso + end_res
                | case_res + Expresion + of_res + Caso + RecursividadCaso + else_res + Sentencia + end_res
                ;

            AlternativaSimple.Rule = if_res + Condicion + then_res + Sentencia
                ;

            BucleConNumeroFijoDeIteraciones.Rule = for_res + SentenciaDeAsignacion + to_res + Expresion + do_res + Sentencia
                | for_res + SentenciaDeAsignacion + downto_res + Expresion + do_res + Sentencia
                ;

            BucleConSalidaAlFinal.Rule = repeat_res + Sentencia + RecursividadSentencia + until_res + Condicion
                ;

            BucleConSalidaAlPrincipio.Rule = while_res + Condicion + do_res + Sentencia
                ;

            CabeceraDeFuncion.Rule = function_res + id + ":" + id + ";"
                | function_res + id + ":" + TipoElemental + ";"
                | function_res + id + "(" + ParametrosFormalesPorValor + RecursividadParametrosFormalesPorValor + ")" + ":" + id + ";"
                | function_res + id + "(" + ParametrosFormalesPorValor + RecursividadParametrosFormalesPorValor + ")" + ":" + TipoElemental + ";"
                | function_res + id + "(" + ")" + ":" + id + ";"
                | function_res + id + "(" + ")" + ":" + TipoElemental + ";"
                ;

            CabeceraDeProcedimiento.Rule = procedure_res + id + ";"
                | procedure_res + id + "(" + ParametrosFormales + RecursividadParametrosFormales + ")" + ";"
                | procedure_res + id + "(" + ")" + ";"
                ;

            CabeceraDePrograma.Rule = program_res + id + ";"
                ;

            Caso.Rule = RangoDeLiterales + ":" + Sentencia
                ;

            Condicion.Rule = Expresion
                ;

            ConversionDeTipo.Rule = id + "(" + Expresion + ")"
                ;

            CuerpoDeFuncion.Rule = SentenciaCompuesta
                ;

            CuerpoDeProcedimiento.Rule = SentenciaCompuesta
                ;

            CuerpoDePrograma.Rule = SentenciaCompuesta
                ;

            DeclaracionDeCampos.Rule = id + RecursividadIdentificador + ":" + id
                | id + RecursividadIdentificador + ":" + TipoElemental
                ;

            DeclaracionDeConstante.Rule = id + "=" + Literal
                | id + ":" + id + "=" + Literal
                ;

            DeclaracionDeFuncion.Rule = CabeceraDeFuncion + ZonaDeDeclaraciones + CuerpoDeFuncion
                ;

            DeclaracionDeProcedimiento.Rule = CabeceraDeProcedimiento + ZonaDeDeclaraciones + CuerpoDeProcedimiento
                ;

            DeclaracionDeSubprograma.Rule = DeclaracionDeFuncion
                | DeclaracionDeProcedimiento
                ;

            DeclaracionDeVariables.Rule = id + RecursividadIdentificador + ":" + TipoDeDato
                | id + RecursividadIdentificador + ":" + id
                | id + ":" + TipoDeDato + "=" + Expresion
                //| id + ":" No se para que funciona EquizDe
                ;

            DefinicionDeTipo.Rule = id + "=" + TipoDeDato
                ;

            ElementoDeArray.Rule = id + "[" + Expresion + RecursividadExpresion + "]"
                ;

            ElementoDeEstructura.Rule = ElementoDeArray
                | ElementoDeRegistro
                ;

            ElementoDeRegistro.Rule = id + "." + id + RecursividadObjetoCampo
                | id + "." + ElementoDeArray + RecursividadObjetoCampo
                ;

            EstructuraAlternativa.Rule = AlternativaSimple
                | AlternativaDoble
                | AlternativaMultiple
                ;

            EstructuraDeControl.Rule = EstructuraAlternativa
                | EstructuraIterativa
                ;

            EstructuraIterativa.Rule = BucleConSalidaAlPrincipio
                | BucleConSalidaAlFinal
                | BucleConNumeroFijoDeIteraciones
                ;

            Expresion.Rule = Literal
                | Variable
                | ExpresionUnaria
                | ExpresionBinaria
                | Llamada
                | ConversionDeTipo
                | "(" + Expresion + ")"
                ;

            ExpresionBinaria.Rule = Expresion + OperadorBinario + Expresion
                ;

            ExpresionUnaria.Rule = OperadorUnario + Expresion
                ;

            Literal.Rule = LiteralDeTipoEntero | LiteralDeTipoReal | LiteralDeTipoBoolean | LiteralDeTipoString
                ;

            LiteralDeTipoBoolean.Rule = boleano
                ;

            LiteralDeTipoEntero.Rule = N_entero
                ;

            LiteralDeTipoReal.Rule = N_real
                ;

            LiteralDeTipoString.Rule = cadena
                ;

            Llamada.Rule = /*id
                |*/ id + "(" + ")"
                | id + "(" + ParametrosActualesPorValor + RecursividadParametrosActualesPorValor + ")"
                | id + "(" + ParametrosActuales + RecursividadParametrosActuales + ")"
                ;

            OperadorBinario.Rule = ToTerm("+")
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

            OperadorUnario.Rule = ToTerm("not")
                | ToTerm("-")
                ;

            ParametrosActuales.Rule = ParametrosActualesPorValor
                | ParametrosActualesPorVariable
                ;

            ParametrosActualesPorValor.Rule = Expresion
                ;

            ParametrosActualesPorVariable.Rule = Variable
                ;

            ParametrosFormales.Rule = ParametrosFormalesPorValor
                | ParametrosFormalesPorVariable
                ;

            ParametrosFormalesPorValor.Rule = id + RecursividadIdentificador + ":" + id
                ;

            ParametrosFormalesPorVariable.Rule = var_res + id + RecursividadIdentificador + ":" + id
                ;

            Programa.Rule = CabeceraDePrograma + ZonaDeDeclaraciones + CuerpoDePrograma + "."
                ;

            RangoDeLiterales.Rule = Literal
                | Literal + ".." + Literal
                | RangoDeLiterales + RecursividadRangoDeLiterales
                ;

            RangoDeValores.Rule = Expresion
                | Expresion + ".." + Expresion
                | RangoDeValores + RecursividadRangoDeValores
                ;

            RecursividadCaso.Rule = ";" + Caso + RecursividadCaso
                | Empty
                ;

            RecursividadDeclaracionDeCampos.Rule = ";" + DeclaracionDeCampos + RecursividadDeclaracionDeCampos
                | DeclaracionDeCampos + RecursividadDeclaracionDeCampos
                | Empty
                | ";"
                ;

            RecursividadDeclaracionDeConstante.Rule = ";" + DeclaracionDeConstante + RecursividadDeclaracionDeConstante
                | Empty
                ;

            RecursividadDeclaracionDeSubprograma.Rule = ";" + DeclaracionDeSubprograma + RecursividadDeclaracionDeSubprograma
                | Empty
                ;

            RecursividadDeclaracionDeVariables.Rule = ";" + DeclaracionDeVariables + RecursividadDeclaracionDeVariables
                | Empty
                ;

            RecursividadDefinicionDeTipo.Rule = ";" + DefinicionDeTipo + RecursividadDefinicionDeTipo
                | Empty
                ;

            RecursividadExpresion.Rule = "," + Expresion + RecursividadExpresion
                | Empty
                ;

            RecursividadIdentificador.Rule = "," + id + RecursividadIdentificador
                | Empty
                ;

            RecursividadObjetoCampo.Rule = "." + id + RecursividadObjetoCampo
                | "." + ElementoDeArray + RecursividadObjetoCampo
                | Empty
                ;

            RecursividadParametrosActuales.Rule = ";" + ParametrosActuales + RecursividadParametrosActuales
                | "," + ParametrosActuales + RecursividadParametrosActuales
                | Empty
                ;

            RecursividadParametrosActualesPorValor.Rule = ";" + ParametrosActualesPorValor + RecursividadParametrosActualesPorValor
                | "," + ParametrosActualesPorValor + RecursividadParametrosActualesPorValor
                | Empty
                ;

            RecursividadParametrosFormales.Rule = ";" + ParametrosFormales + RecursividadParametrosFormales
                | "," + ParametrosFormales + RecursividadParametrosFormales
                | Empty
                ;

            RecursividadParametrosFormalesPorValor.Rule = ";" + ParametrosFormalesPorValor + RecursividadParametrosFormalesPorValor
                | "," + ParametrosFormalesPorValor + RecursividadParametrosFormalesPorValor
                | Empty
                ;

            RecursividadRangoDeLiterales.Rule = "," + RangoDeLiterales + RecursividadRangoDeLiterales
                | Empty
                ;

            RecursividadRangoDeValores.Rule = "," + RangoDeValores + RecursividadRangoDeValores
                | Empty
                ;

            RecursividadSentencia.Rule =  Sentencia + ";" + RecursividadSentencia
                | Empty
                ;

            RecursividadTipoIndice.Rule = "," + TipoIndice + RecursividadTipoIndice
                | Empty
                ;

            Sentencia.Rule = SentenciaDeAsignacion
                | SentenciaCompuesta
                | Llamada
                | EstructuraDeControl
                ;

            SentenciaCompuesta.Rule = begin_res + Sentencia + ";" + RecursividadSentencia + end_res
                | begin_res + end_res
                ;

            SentenciaDeAsignacion.Rule = Variable + ":=" + Expresion
                ;

            TipoArray.Rule = id
                | array_res + "[" + TipoIndice + RecursividadTipoIndice + "]" + of_res + TipoDeDato
                ;

            TipoDeDato.Rule = tipo_integer
                | tipo_boolean
                | tipo_real
                | tipo_string
                | array_res + "[" + TipoIndice + RecursividadTipoIndice + "]" + of_res + TipoDeDato
                | record_res + DeclaracionDeCampos + RecursividadDeclaracionDeCampos + end_res
                | object_res + var_res + DeclaracionDeCampos + RecursividadDeclaracionDeCampos + end_res
                ;

            TipoElemental.Rule = TipoSimple
                | TipoString
                ;

            TipoEstructurado.Rule = TipoArray
                | TipoRegistro
                | TipoString
                ;

            TipoIndice.Rule = TipoOrdinal
                ;

            TipoOrdinal.Rule = TipoOrdinalPredefinido
                | Expresion + ".." + Expresion
                ;

            TipoOrdinalPredefinido.Rule = N_entero
                | boleano
                ;

            TipoReal.Rule = tipo_real
                ;

            TipoRegistro.Rule = id
                | record_res + DeclaracionDeCampos + RecursividadDeclaracionDeCampos + end_res
                ;

            TipoSimple.Rule = TipoOrdinal
                | TipoReal
                ;

            TipoString.Rule = id
                ;

            TipoSubrango.Rule = id
                | Literal + ".." + Literal
                ;

            Variable.Rule = id
                | ElementoDeEstructura
                ;

            ZonaDeDeclaracionDeConstantes.Rule = Empty
                | const_res + DeclaracionDeConstante + ";" + RecursividadDeclaracionDeConstante + ZonaDeDeclaraciones
                | DeclaracionDeConstante + ";" + RecursividadDeclaracionDeConstante + ZonaDeDeclaraciones
                ;

            ZonaDeDeclaracionDeSubprogramas.Rule = Empty
                | DeclaracionDeSubprograma + ";" + RecursividadDeclaracionDeSubprograma + ZonaDeDeclaraciones
                ;

            ZonaDeDeclaracionDeVariables.Rule = Empty
                | var_res + DeclaracionDeVariables + ";" + RecursividadDeclaracionDeVariables + ZonaDeDeclaraciones
                | DeclaracionDeVariables + ";" + RecursividadDeclaracionDeVariables + ZonaDeDeclaraciones
                ;

            ZonaDeDeclaraciones.Rule = ZonaDeDeclaracionDeConstantes
                | ZonaDeDefinicionDeTipos
                | ZonaDeDeclaracionDeSubprogramas
                | ZonaDeDeclaracionDeVariables
                | Empty
                ;

            ZonaDeDefinicionDeTipos.Rule = Empty
                | type_res + DefinicionDeTipo + ";" + RecursividadDefinicionDeTipo + ZonaDeDeclaraciones
                | DefinicionDeTipo + ";" + RecursividadDefinicionDeTipo + ZonaDeDeclaraciones
                ;

            #endregion

            #region Preferencias
            this.Root = Programa;
            #endregion
        }
    }
}
