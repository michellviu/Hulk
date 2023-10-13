
using System;
using System.Text.RegularExpressions;

public class Funciones
{
    public Dictionary<string, string> Diccfunciones { get; private set; }
    public Dictionary<string, string[]> Diccnameparametros { get; private set; }
    public List<string> NombreFunciones { get; private set; }
    public static string pattern = @"^function\s+(?<name>[a-zA-Z]+\.*)\s*\((?<parametros>.*)\)\s*=>\s*(?<cuerpo>.*)$";
    public static string llamado = @"\s*[a-zA-Z]+[\w]*\s*\(.*\)";
    public static int PosLLamado = 0;
    public static int posPI = 0;
    public static int posPD = 0;
    //public static string patronllamado = @"[a-zA-Z]";

    public Funciones()
    {
        this.Diccfunciones = new Dictionary<string, string>();
        this.Diccnameparametros = new Dictionary<string, string[]>();
        this.NombreFunciones = new List<string>();
    }

    public static Match IsFunction(string input, string patron)
    {
        // string pattern = @"^function\s+(?<>name[a-zA-Z]\.*)\s*\(.*\)\s*=>\s*\(?<cuerpo>.*)";
        Match match = Regex.Match(input, patron);
        return match;
    }

    public void Function(string input, Match match)
    {

        // string[] tokens = input.Split("=>");
        string cuerpoFuncion = match.Groups[3].ToString();
        string nombre = match.Groups[1].ToString();
        string parametros = match.Groups[2].ToString();
        string[] p = parametros.Split(",");

        // tokens = nombre.Split(" ");
        //  nombre = tokens[1];


        if (Diccfunciones.ContainsKey(nombre))
        {
            Console.WriteLine("La función '{0}' ya está definida.", nombre);
        }
        else
        {
            for (int i = 0; i < Let_In.palabrasreservadas.Length; i++)
            {
                //Si el nombre de la variable es una palabra reservada lanzar un error
                if (nombre == Let_In.palabrasreservadas[i])
                {
                    throw new PalabrasResevadasExcepcion(Let_In.palabrasreservadas[i]);
                }

            }
            this.NombreFunciones.Add(nombre);
            this.Diccfunciones.Add(nombre, cuerpoFuncion);
            this.Diccnameparametros.Add(nombre, p);
            // Console.WriteLine("La función '{0}' ha sido definida.", nombre);
        }

    }

    public static Match IsLlamado(string input)
    {
        Match match = Regex.Match(input, llamado);
        return match;

    }
    public static string LlamadoFunciones(List<string> tokens, Funciones funciones)
    {


        string result = "";
        for (int i = tokens.Count - 1; i >= 0; i--)
        {
            if (Let_In.EsVariable(tokens[i]) && funciones.NombreFunciones.Contains(tokens[i]))
            {
                int cont = 0;
                //bool bandera = true;
                int posAbre = 0;
                int posllamado = 0;
                int posCierre = 0;
                posllamado = i;
                for (int j = i + 1; j < tokens.Count; j++)
                {
                    if (tokens[i + 1] != "(")
                    {
                        throw new SeEspreaParentesis(tokens[i]);
                    }
                    if (tokens[j] == "(")
                    {
                        if (posAbre == 0)
                        {
                            posAbre = j;
                            posPI = j;
                        }
                        cont++;
                    }
                    else if (tokens[j] == ")")
                    {
                        cont--;
                        if (cont == 0)
                        {
                            posCierre = j;
                            posPD = j;
                            break;

                        }
                    }
                }
                // List<string> listaparametros = new List<string>();
                string parametros = String.Join(" ", tokens.GetRange(posAbre + 1, posCierre - posAbre - 1));

                string[] arrayParametros = parametros.Split(",");
                if (arrayParametros.Length > 1)
                {
                    for (int j = 0; j < arrayParametros.Length; j++)
                    {
                        if (arrayParametros[j] == "")
                        {
                            throw new ParametroVacio(tokens[i], j);
                        }
                    }
                }
                /////////////
                if (funciones.Diccnameparametros[tokens[i]].Length > 1 && arrayParametros.Length == 1 && arrayParametros[0] == "")
                {
                    throw new CantidadParametros(tokens[i], funciones.Diccnameparametros[tokens[i]].Length, 0);
                }
                if (funciones.Diccnameparametros[tokens[i]].Length != arrayParametros.Length)
                {
                    throw new CantidadParametros(tokens[i], funciones.Diccnameparametros[tokens[i]].Length, arrayParametros.Length);
                }
                if (funciones.Diccnameparametros[tokens[i]].Length == 1 && funciones.Diccnameparametros[tokens[i]][0] != "" && arrayParametros.Length == 1 && arrayParametros[0] == "")
                {
                    throw new CantidadParametros(tokens[i], funciones.Diccnameparametros[tokens[i]].Length, 0);
                }
                //////////////
                if (funciones.Diccnameparametros[tokens[i]].Length == 1 && funciones.Diccnameparametros[tokens[i]][0] == "" && arrayParametros.Length == 1 && arrayParametros[0] == "")
                {
                    string cuerpoF = funciones.Diccfunciones[tokens[i]];
                    tokens.RemoveRange(posllamado, posCierre - posllamado + 1);
                    tokens.Insert(posllamado, EvaluadorExpresiones.QueEs(cuerpoF, funciones));
                    result = String.Join(" ", tokens);
                }
                else
                {
                    Dictionary<string, string> variables = new Dictionary<string, string>();
                    for (int j = 0; j < arrayParametros.Length; j++)
                    {
                        string[] p = funciones.Diccnameparametros[tokens[i]];
                        Let_In.Define(p[j], EvaluadorExpresiones.QueEs(arrayParametros[j].ToString().Trim(), funciones), variables);
                    }
                    string cuerpoF = funciones.Diccfunciones[tokens[i]];
                    string cuerpo = Let_In.EvaluadorVariables(cuerpoF, variables);
                    tokens.RemoveRange(posllamado, posCierre - posllamado + 1);
                    tokens.Insert(posllamado, EvaluadorExpresiones.QueEs(cuerpo, funciones));
                    result = String.Join(" ", tokens);


                }
            }
            else if (Let_In.EsVariable(tokens[i]) && (tokens[i] == "sin" || tokens[i] == "cos" || tokens[i] == "log"))
            {
                int cont = 0;
                //bool bandera = true;
                int posAbre = 0;
                int posllamado = 0;
                int posCierre = 0;
                posllamado = i;
                for (int j = i + 1; j < tokens.Count; j++)
                {
                    if (tokens[i + 1] != "(")
                    {
                        throw new SeEspreaParentesis(tokens[i]);
                    }
                    if (tokens[j] == "(")
                    {
                        if (posAbre == 0)
                        {
                            posAbre = j;
                            posPI = j;
                        }
                        cont++;
                    }
                    else if (tokens[j] == ")")
                    {
                        cont--;
                        if (cont == 0)
                        {
                            posCierre = j;
                            posPD = j;
                            break;

                        }
                    }
                }
                string parametros = String.Join(" ", tokens.GetRange(posAbre + 1, posCierre - posAbre - 1));
                string[] arrayParametros = parametros.Split(",");
                if (arrayParametros.Length > 1) throw new CantidadParametros(tokens[i], 1, arrayParametros.Length);
                string v;
                if (tokens[i] == "sin")
                    v = Math.Sin(EvaluadorAritmetico.EvaluarPosfija(parametros)).ToString();
                else if (tokens[i] == "cos")
                    v = Math.Cos(EvaluadorAritmetico.EvaluarPosfija(parametros)).ToString();
                else
                    v = Math.Log(EvaluadorAritmetico.EvaluarPosfija(parametros)).ToString();

                tokens.RemoveRange(posllamado, posCierre - posllamado + 1);
                tokens.Insert(posllamado, v);
                result = String.Join(" ", tokens);

            }
            else if (Let_In.EsVariable(tokens[i]))
            {
                throw new VariableNoAsignada(tokens[i]);
            }
            else
                continue;

        }
        return result;
    }

}
