using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace ClassLibrary
{
    public class Expresiones
    {

        //Expresion regular para let-in
        public static string Let_In = @"\s*let\s+(?<declaraciondevariables>.+)\s+in\s+(?<cuerpoin>.+)$";
        //Expresion regular para las expresiones condicionales
        public static string condicional = @"\s*if\s*\((?<expresionbooleana>.+)\)\s*(?<cuerpoif>[^{]+)(?<cuerpoelse>[^{]+)";
        //public static string condicional =@"\s*if|else";
        //Expresion regular para expresiones aritmeticas
        public static string expresionAritmetica = @"(?<operadores>\+|-|\^|/|%|\*)";
        // public static string expresionAritmetica = @"^\s*\(*(\d+(\.?\d+)*)\)*\s*(?<operadores>\+|-|\^|/|\*)\s*\(*(\d+(\.?\d+)*)\)*\s*";
        //Expresion regular para expresiones booleanas
        public static string expresionBooleana = @"(?<comparadores>!=|!>|!<|>=|<=|<|>|==){1}";
        //Expresion regular para expresiones booleanas de tipo string
        public static string expresionBooleanaString = @"=={1}";
        //Expresion regular para expresiones numericas
        public static string expresionNumber = "^\\s*\\-?\\d+(\\.\\d+)?\\s*$";
        //Expresion regular para expresiones de tipo string
        public static string expresionString = @"^\s*""(.*)""\s*$";

        //Expresion regular para determinar si es una asignacion de variable
        public static string expresionAsignacion = @"^\s*[a-zA-Z]+\d*\s*=";

        //Expresion regular para expresiones que contienen string
        public static string contieneString = @"""(.*)""";

        //Expresion regular para saber si una expresion contiene letras
        public static string letras = @"[a-zA-Z]";

        //Expresion regular para saber si una expresion es un else
        public static string condelse = @"\s*else\s*$";

        //Expresion regular para saber si una expresion es un else
        public static string condif = @"\s*if";
        //Expresion regular para saber si una exapresion es un let
        public static string Let = @"\s*let\s*$";

        //Expresion regular para saber si una expresion es un in
        public static string In = @"\s*in\s*$";
        public static string Concatena = @"\@";

        //Metodo para saber si una expresion es una condicional
        public static Match EsCondicional(string expresion)
        {
            Match match = Regex.Match(expresion, condicional);
            return match;
        }
        //Metodo para saber si una expresion es una declaracion de variables
        public static Match EsLetIn(string expresion)
        {
            Match match = Regex.Match(expresion, Let_In);
            return match;
        }
        //Metodo para saber si una expresion es una asignacion a una variable
        public static bool EsAsignacion(string expresion)
        {
            Match match = Regex.Match(expresion, expresionAsignacion);
            return match.Success;
        }
        //Metodo para saber si una expresion es un string
        public static Match EsString(string expresion)
        {
            Match match = Regex.Match(expresion, expresionString);
            return match;
        }
        //Metodo para saber si una expresion es un numero
        public static bool EsNumber(string expresion)
        {
            Match match = Regex.Match(expresion, expresionNumber);
            return match.Success;
        }
        //Metodo para saber si una expresion es una operacion aritmetica
        public static Match EsExpresionAritmetica(string expresion)
        {
            Match match = Regex.Match(expresion, expresionAritmetica);
            // Match match1 = Regex.Match(expresion, letras);
            // if (match1.Success)
            //   return false;
            //else

            if (!EvaluadorAritmetico.ParentesisBalanceados(expresion))
            {
                // Console.WriteLine("Los parentesis de la expresion no estan bien balanceados");
                throw new ParentesisNoBalanceados();
                //return false;
            }

            return match;

        }
        public static Match EsConcatenar(string expresion)
        {
            Match match = Regex.Match(expresion, Concatena);
            return match;
        }
        public static string ConcatenarString(string[] operandos)
        {
            for (int i = 0; i < operandos.Length; i++)
            {
                if (operandos[i].Trim() != "")
                {
                    operandos[i] = operandos[i].Trim();
                }
                else
                    operandos[i] = Regex.Replace(operandos[i], "\\s+", " ");
            }
            string resultado = String.Concat(operandos, " ");
            resultado = resultado.Insert(0, "\"");
            resultado = resultado.Insert(resultado.Length, "\"");
            return resultado;
        }

    }
}