using System.Globalization;
using System;
using System.Text.RegularExpressions;

public static class Condicionales
{
    public static List<string> tokens = new List<string>();
    public static int posIf = 0;
    public static int posElse = 0;
    public static int postermina = tokens.Count;
    public static int posCierreB = 0;
    //Metodo para dividir cada una de las partes de una expresion condicional
    //parte booleana,cuerpo if,cuerpo else
    public static string[] DivideCondicional(List<string> tokens)
    {
        Condicionales.tokens = tokens;
        Condicionales.postermina = tokens.Count - 1;
        int cont1 = 0;
        int cont = 0;
        int posIf = 0;
        int posElse = 0;
        int posterminaElse = tokens.Count - 1;
        int posAbre = 0;
        int posCierre = 0;
        bool entro = true;
        List<string> parteBooleana = new List<string>();
        List<string> parteIf = new List<string>();
        List<string> parteElse = new List<string>();

        if (EvaluadorAritmetico.ParentesisBalanceados(String.Join(" ", tokens)))
        {
            for (int i = 0; i < tokens.Count; i++)
            {
                if (tokens[i] == "if")
                {
                    if (posIf == 0 && entro)
                    {
                        Condicionales.posIf = i;
                        posIf = i;
                        entro = false;
                    }
                    cont++;
                }
                else if (tokens[i] == "else")
                {
                    cont--;
                    if (cont == 0)
                    {
                        Condicionales.posElse = i;
                        posElse = i;
                        break;
                    }
                }
            }
            entro = true;
            for (int i = posElse; i < tokens.Count; i++)
            {
                if (tokens[posElse + 1] == "(" && entro)
                {
                    entro = false;
                    for (int j = posElse; j < tokens.Count; j++)
                    {
                        if (tokens[j] == "(")
                        {
                            cont1++;
                        }
                        else if (tokens[j] == ")")
                        {
                            cont1--;
                            if (cont1 == 0)
                            {
                                posterminaElse = j;
                                Condicionales.postermina = j;
                                i = j;
                                break;
                            }
                        }
                    }
                }
                else if (tokens[i] == ",")
                {
                    posterminaElse = i;
                    Condicionales.postermina = i;
                    break;
                }
            }
            cont1 = 0;
            for (int i = posIf; i < tokens.Count; i++)
            {
                if (tokens[i] == "(")
                {
                    if (posAbre == 0)
                    {
                        posAbre = i;
                    }
                    cont1++;
                }
                else if (tokens[i] == ")")
                {
                    cont1--;
                    if (cont1 == 0)
                    {
                        posCierreB = i;
                        posCierre = i;
                        break;
                    }
                }
            }


        }
        else
            throw new ParentesisNoBalanceados();


        if (cont > 0)
            throw new ErrorSintacticoIf();

        if (cont < 0)
            throw new ErrorSintacticoElse();

        for (int i = posAbre + 1; i < posCierre; i++)
        {
            parteBooleana.Add(tokens[i]);
        }
        for (int i = posCierre + 1; i < posElse; i++)
        {
            parteIf.Add(tokens[i]);
        }
        if (tokens[posterminaElse] != ",")
        {
            for (int i = posElse + 1; i <= posterminaElse; i++)
            {
                parteElse.Add(tokens[i]);
            }
        }
        else
        {
            for (int i = posElse + 1; i < posterminaElse; i++)
            {
                parteElse.Add(tokens[i]);
            }
        }

        string[] partesCondicional = new string[3];
        partesCondicional[0] = String.Join(" ", parteBooleana).Trim();
        partesCondicional[1] = String.Join(" ", parteIf).Trim();
        partesCondicional[2] = String.Join(" ", parteElse).Trim();
        if (partesCondicional[1] == "") throw new ParteiFVacia();
        if (partesCondicional[2] == "") throw new ParteElseVacia();

        return partesCondicional;

    }
    //Metodo para evaluar booleanos
    public static bool EvaluadorDeBooleanos(double miembroizq, string op, double miembroder)
    {
        switch (op)
        {
            case ">":
                return miembroizq > miembroder;
            case "<":
                return miembroizq < miembroder;
            case ">=":
                return miembroizq >= miembroder;
            case "<=":
                return miembroizq <= miembroder;
            case "==":
                return miembroizq == miembroder;
            case "!=":
                return miembroizq != miembroder;
            case "!>":
                return miembroizq! > miembroder;
            case "!<":
                return miembroizq! < miembroder;



            default:
                return false;
        }
    }
    //Metodo para saber si una expresion es del tipo boolean
    public static bool EsBoolean(string expresion, Funciones funciones1)
    {
        Match match = Regex.Match(expresion, Expresiones.expresionBooleana);
        Match match1 = Regex.Match(expresion, Expresiones.expresionBooleanaString);

        if (expresion == "true" || expresion == "false" || expresion == "True" || expresion == "False")
        {
            return true;
        }
        else if (match1.Success)
        {
            string[] expresionstring = expresion.Split(match1.Groups[0].ToString());
            string miiz = EvaluadorExpresiones.QueEs(expresionstring[0], funciones1);
            string mider = EvaluadorExpresiones.QueEs(expresionstring[1], funciones1);
            Match match4 = Regex.Match(miiz, Expresiones.expresionString);
            Match match5 = Regex.Match(mider, Expresiones.expresionString);
            if (match4.Success && match5.Success)
            {
                return true;
            }
            else
            {
                string[] expresionAritmetica = expresion.Split(match.Groups[1].ToString());
                string miembroizq = EvaluadorExpresiones.QueEs(expresionAritmetica[0], funciones1);
                string miembroder = EvaluadorExpresiones.QueEs(expresionAritmetica[1], funciones1);
                if (Expresiones.EsNumber(miembroizq) && Expresiones.EsNumber(miembroder))
                    return true;
                else
                    return false;
            }
        }
        else if (match.Success)
        {
            string[] expresionAritmetica = expresion.Split(match.Groups[1].ToString());
            string miembroizq = EvaluadorExpresiones.QueEs(expresionAritmetica[0], funciones1);
            string miembroder = EvaluadorExpresiones.QueEs(expresionAritmetica[1], funciones1);
            if (Expresiones.EsNumber(miembroizq) && Expresiones.EsNumber(miembroder))

                return true;
            else

                return false;
        }
        else

            return false;
    }
    //Metodo para evaluar booleanos
    public static bool ValorDelBooleano(string expresion, Funciones funciones1)
    {

        Match match = Regex.Match(expresion, Expresiones.expresionBooleana);
        Match match1 = Regex.Match(expresion, Expresiones.expresionBooleanaString);

        if (expresion == "true" || expresion == "True")
        {
            return true;
        }
        else if (expresion == "false" || expresion == "False")
        {
            return false;
        }
        else if (match1.Success)
        {
            expresion = Regex.Replace(expresion, "\\s+", " ");
            string[] expresionstring = expresion.Split(match1.Groups[0].ToString());
            string miiz = EvaluadorExpresiones.QueEs(expresionstring[0], funciones1);
            string mider = EvaluadorExpresiones.QueEs(expresionstring[1], funciones1);
            Match match4 = Regex.Match(miiz, Expresiones.expresionString);
            Match match5 = Regex.Match(mider, Expresiones.expresionString);
            if (match4.Success && match5.Success)
            {
                if (expresionstring[0] == expresionstring[1])
                {
                    return true;
                }
                else
                    return false;
            }
            else
            {
                string[] expresionAritmetica = expresion.Split(match.Groups[1].ToString());
                string miembroizq = EvaluadorExpresiones.QueEs(expresionAritmetica[0], funciones1);
                string miembroder = EvaluadorExpresiones.QueEs(expresionAritmetica[1], funciones1);
                if (EvaluadorDeBooleanos(double.Parse(miembroizq), match.Groups[1].ToString(), double.Parse(miembroder)))
                {
                    return true;
                }
                else return false;

            }
        }
        else if (match.Success)
        {
            string[] expresionAritmetica = expresion.Split(match.Groups[1].ToString());
            string miembroizq = EvaluadorExpresiones.QueEs(expresionAritmetica[0], funciones1);
            string miembroder = EvaluadorExpresiones.QueEs(expresionAritmetica[1], funciones1);
            if (Condicionales.EvaluadorDeBooleanos(int.Parse(miembroizq), match.Groups[1].ToString(), int.Parse(miembroder)))

                return true;
            else
                return false;
        }

        else
            return false;

    }

}