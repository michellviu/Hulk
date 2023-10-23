using System;
using System.Text.RegularExpressions;

public class EvaluadorExpresiones
{
    //Metodo que parsea un input
    public static string QueEs(string input, Funciones funciones)
    {
        if (input[0] == ' ' || input[input.Length - 1] == ' ')
            input = input.Trim();
        //Comprobar primeramente si es la declaracion de una funcion
        //de ser asi llamar a los metodos definidos de la clase funcion para realizar los procesos requeridos
        if (Funciones.IsFunction(input, Funciones.pattern).Success)
        {
            Match match = Funciones.IsFunction(input, Funciones.pattern);
            try
            {
                funciones.Function(input, match);
                return "La función " + match.Groups[1].ToString() + " ha sido definida.";
            }
            catch (FuncionAsignada e)
            {
                throw e;
            }

        }
        //Comprobar si es una expresion Let In y utilizar los metodos de la clase Let In que evaluar expresiones de este tipo
        else if (Expresiones.EsLetIn(input).Success)
        {
            Dictionary<string, string> variables = new Dictionary<string, string>();
            int i;
            int j;
            try
            {
                List<string> TL = Lexer.Tokenizador(input);
                //Dividimos la expresion
                string[] partes = Let_In.DivideLet_In(TL);
                i = Let_In.posl;
                j = Let_In.posIn;

                //Dividir variables
                List<List<string>> fragmentosvari = Let_In.DivideVariables(Lexer.Tokenizador(partes[0]), funciones);
                //Guardar las variables
                Let_In.GuardarVariables(fragmentosvari, variables, funciones);
                //Removemos desde Let hasta In
                TL.RemoveRange(i, j - i + 1);

                input = String.Join(" ", TL);
                return
                //Retornamos usando recursivamente el metodo para ver si queda algo para evaluar
                QueEs(Let_In.EvaluadorVariables(input, variables), funciones);
            }
            catch (PalabrasResevadasExcepcion e)
            {
                throw e;
            }
            catch (VariableAsignada e)
            {
                throw e;
            }
            catch (VariableNoAsignada e)
            {
                throw e;
            }
            catch (MalAsignacion e)
            {
                throw e;
            }
            catch (FaltaIgual e)
            {
                throw e;
            }
            catch (Asignacion e)
            {
                throw e;
            }
            catch (VariableNoValida e)
            {
                throw e;
            }
            catch (ParentesisNoBalanceados e)
            {
                throw e;
            }

        }
        //Comprobar si es una condicional
        else if (Expresiones.EsCondicional(input).Success)
        {
            int i;
            int j;
            int k;
            int l;
            try
            {
                List<string> TC = Lexer.Tokenizador(input);
                string[] partesCondicional = Condicionales.DivideCondicional(TC);
                i = Condicionales.posCierreB;
                j = Condicionales.posIf;
                k = Condicionales.posElse;
                l = Condicionales.postermina;

                if (Condicionales.EsBoolean(partesCondicional[0], funciones))
                {
                    if (Condicionales.ValorDelBooleano(partesCondicional[0], funciones))
                    {
                        if (TC[l] != ",")
                        {
                            TC.RemoveRange(k, l - k + 1);
                            TC.RemoveRange(j, i - j + 1);
                            input = String.Join(" ", TC);
                            return QueEs(input, funciones);
                        }
                        else
                        {
                            TC.RemoveRange(k, l - k);
                            TC.RemoveRange(j, i - j + 1);
                            input = String.Join(" ", TC);
                            return QueEs(input, funciones);
                        }
                    }
                    else
                    {
                        TC.RemoveRange(j, k - j + 1);
                        input = String.Join(" ", TC);
                        return QueEs(input, funciones);
                    }
                }
                else throw new ParteBooleanaIncorrecta();
            }
            catch (ParentesisNoBalanceados e)
            {
                throw e;
            }
            catch (ErrorSintacticoIf e)
            {
                throw e;
            }
            catch (ErrorSintacticoElse e)
            {
                throw e;
            }
            catch (ParteiFVacia e)
            {
                throw e;
            }
            catch (ParteElseVacia e)
            {
                throw e;
            }
        }
        //Comprobar si es una llamado de funcion
        else if (Funciones.IsLlamado(input).Success)
        {
            try
            {
                return QueEs(Funciones.LlamadoFunciones(Lexer.Tokenizador(input), funciones), funciones);
            }
            catch (CantidadParametros e)
            {
                throw e;
            }
            catch (ParametroVacio e)
            {
                throw e;
            }

        }
        //Comprobar si es una expresion aritmetica
        else if (Expresiones.EsExpresionAritmetica(input).Success)
        {
            try
            {
                return EvaluadorAritmetico.EvaluarPosfija(input).ToString();
            }
            catch (VariableNoAsignada e)
            {
                throw e;
            }
            catch (InvalidOperationException e)
            {
                throw e;
            }

        }
        else if (Expresiones.EsConcatenar(input).Success)
        {
            Match match = Expresiones.EsConcatenar(input);
            input = input.Replace("\"", "");
            string[] operandos = input.Split("@");
            return Expresiones.ConcatenarString(operandos);
        }
        //Comprobar si  es un numero
        else if (Expresiones.EsNumber(input))
        {
            // Console.WriteLine(input);
            return input.ToString();
        }
        //Comprobar si es una string
        else if (Expresiones.EsString(input).Success)
        {
            return input.Trim();
        }
        //Comprobar si es un booleano
        else if (Condicionales.EsBoolean(input, funciones))
        {
            return Condicionales.ValorDelBooleano(input, funciones).ToString();
        }
        else if (input == "PI")
        {
            return 3.14.ToString();
        }
        else if (input[0] == '(' && input[input.Length - 1] == ')')
        {
            input = input.Remove(0, 1);
            input = input.Remove(input.Length - 1, 1);
            return QueEs(input, funciones);
        }
        //De no ser ninguno de estos casos la expresion es invalida
        else
        {
            if (Let_In.EsVariable(input))
            {
                throw new VariableNoAsignada(input);
            }
            else
                return "Expresion invalida";
        }
    }
}