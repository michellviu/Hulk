using System;
using System.Text.RegularExpressions;

public class EvaluadorExpresiones
{

    //Metodo para recortar una condicional en booleano, cuerpo del if y cuerpo del else
    public static string[] RecortaIf(string input)
    {
        string[] partesIf = new string[3];
        int ultelse = 0;
        int posAbre = 0;
        int posPCierre = 0;
        int cont = 0;
        bool bandera = true;
        if (EvaluadorAritmetico.ParentesisBalanceados(input))
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                {
                    if (posAbre == 0)
                    {
                        posAbre = i;
                    }
                    cont++;
                }
                else if (input[i] == ')')
                {
                    cont--;
                    if (cont == 0 && bandera)
                    {
                        posPCierre = i;
                        break;

                    }
                }
            }
            string[] ArrayIf = input.Split(" ");
            int cont1 = 0;
            bool flag = true;
            int k = 0;
            int pos = 0;

            for (int i = 0; i < ArrayIf.Length - 1; i++)
            {
                Match match = Regex.Match(ArrayIf[i], Expresiones.condif);
                Match match1 = Regex.Match(ArrayIf[i], Expresiones.condelse);
                bool esstring = Expresiones.EsString(ArrayIf[i]).Success;


                if (match.Success && !esstring)
                {
                    cont1++;
                }
                else if (match1.Success && !esstring)
                {
                    cont1--;
                    if (cont1 == 0 && flag)
                    {
                        k = i - 1;
                        pos = k;
                        while (k != -1)
                        {
                            ultelse += ArrayIf[k].Length;
                            k--;
                        }
                        ultelse += pos;
                        break;
                    }
                }

            }
            string booleano = input.Substring(posAbre + 1, posPCierre - posAbre - 1);
            string cuerpoIf = input.Substring(posPCierre + 1, ultelse - posPCierre - 1);
            string cuerpoelse = input.Substring(ultelse + 5, input.Length - ultelse - 5);
            partesIf[0] = booleano;
            partesIf[1] = cuerpoIf;
            partesIf[2] = cuerpoelse;
            return partesIf;
        }
        else
        {
            throw new ParentesisNoBalanceados();
        }

    }

    public static string QueEs(string input, Funciones funciones)
    {
        if (input[0] == ' ' || input[input.Length - 1] == ' ')
            input = input.Trim();

        if (Funciones.IsFunction(input, Funciones.pattern).Success)
        {
            Match match = Funciones.IsFunction(input, Funciones.pattern);
            funciones.Function(input, match);
            return "La función " + match.Groups[1].ToString() + " ha sido definida.";
        }
        else if (Expresiones.EsLetIn(input).Success)
        {
            Dictionary<string, string> variables = new Dictionary<string, string>();
            int i;
            int j;
            try
            {
                //Let_In va = v;
                List<string> TL = Lexer.Tokenizador(input);
                string[] partes = Let_In.DivideLet_In(TL);
                i = Let_In.posl;
                j = Let_In.posIn;

                //Dividir variables
                List<List<string>> fragmentosvari = Let_In.DivideVariables(Lexer.Tokenizador(partes[0]), funciones);

                Let_In.GuardarVariables(fragmentosvari, variables, funciones);
                // TL = Lexer.Tokenizador(QueEs(Let_In.EvaluadorVariables(partes[1], variables)));
                TL.RemoveRange(i, j - i + 1);
                // TL.InsertRange(Let_In.posl, new string[] { v });
                input = String.Join(" ", TL);
                return
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
                        TC.RemoveRange(k, l - k);
                        TC.RemoveRange(j, i - j + 1);
                        input = String.Join(" ", TC);
                        return QueEs(input, funciones);
                    }
                    else
                    {
                        TC.RemoveRange(j, k - j + 1);
                        input = String.Join(" ", TC);
                        return QueEs(input, funciones);
                    }
                }
                else return "Expresion en el interior del 'if' es incorrecta";
            }
            catch (ParentesisNoBalanceados e)
            {
                throw e;
            }
        }
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

        else if (Expresiones.EsExpresionAritmetica(input).Success)
        {
            try
            {
                /*//Console.WriteLine(EvaluadorAritmetico.EvaluarPosfija(input));
                Match match = Expresiones.EsExpresionAritmetica(input);
                string ce = match.Groups[0].ToString();
                string regex1 = Regex.Escape(ce.ToString());
                string[] partes = input.Split(new string[] { regex1 }, 2, StringSplitOptions.None);
                //string[] partes = input.Split(match.Groups[0].ToString());
                string miiz = QueEs(partes[0], v);
                string mider = QueEs(partes[1], v);
                input = miiz + match.Groups[0] + mider;
                if (miiz == "Expresion invalida" || mider == "Expresion invalida")
                    return "Expresion invalida";
                else*/
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
        /* else if(Expresiones.EsConcatenar(input).Success)
         {
             Match match = Expresiones.EsConcatenar(input);


         }*/
        else if (Expresiones.EsNumber(input))
        {
            // Console.WriteLine(input);
            return input.ToString();
        }
        else if (Expresiones.EsString(input).Success)
        {
            //Console.WriteLine(input);
            /* if (v.diccionario.ContainsKey(input))
                 return v.diccionario[input];
             else*/
            return input.Trim();
        }
        else if (Condicionales.EsBoolean(input, funciones))
        {
            return Condicionales.ValorDelBooleano(input, funciones).ToString();
        }

        else
        {
            return "Expresion invalida";
        }
    }
}