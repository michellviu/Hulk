
using System;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

public class Funciones
{
    //Diccionario que se usa para guardar el nombre de la funcion con su cuerpo
    public Dictionary<string, string> Diccfunciones { get; private set; }
    //Diccionario que se usa para guardar el nombre de la funcion y un array con sus parametros
    public Dictionary<string, string[]> Diccnameparametros { get; private set; }
    //Lista para guardar todos los nombres de las funciones que se van definiendo
    public List<string> NombreFunciones { get; private set; }
    //Patron para la declaracion de una funcion
    public static string pattern = @"^function\s+(?<name>[a-zA-Z]+\.*)\s*\((?<parametros>.*)\)\s*=>\s*(?<cuerpo>.*)$";
    //Patron para el llamado de una funcion
    public static string llamado = @"\s*[a-zA-Z]+[\w]*\s*\(.*\)";
    //Posicion del llamado
    public static int PosLLamado = 0;
    //Posicion donde se abre el parentesis izquierdo
    public static int posPI = 0;
    //Posicion donde se abre el parentesis derecho
    public static int posPD = 0;

    public Funciones()
    {
        this.Diccfunciones = new Dictionary<string, string>();
        this.Diccnameparametros = new Dictionary<string, string[]>();
        this.NombreFunciones = new List<string>();
    }
    //Funcion para determinar si una input es la declaracion de una nueva funcion
    public static Match IsFunction(string input, string patron)
    {
        Match match = Regex.Match(input, patron);
        return match;
    }
    //Metodo para tratar una declaracion de funcion
    public void Function(string input, Match match)
    {

        //Nuestra declaracion de funcion cuenta con tres partes(nombre,parametros,cuerpo)
        string cuerpoFuncion = match.Groups[3].ToString();
        string nombre = match.Groups[1].ToString();
        string parametros = match.Groups[2].ToString();
        //Array para guardar cada uno de los parametros
        string[] p = parametros.Split(",");
        //Si ya existe una funcion con ese nombre lanzar una excepcion
        if (Diccfunciones.ContainsKey(nombre))
        {
            throw new FuncionAsignada(nombre);
        }
        //Si no existe rellenar cada una de las propiedades del objeto funcion
        else
        {
            for (int i = 0; i < Let_In.palabrasreservadas.Length; i++)
            {
                //Si el nombre de la funcion es una palabra reservada lanzar un error
                if (nombre == Let_In.palabrasreservadas[i])
                {
                    throw new PalabrasResevadasExcepcion(Let_In.palabrasreservadas[i]);
                }

            }
            this.NombreFunciones.Add(nombre);
            this.Diccfunciones.Add(nombre, cuerpoFuncion);
            this.Diccnameparametros.Add(nombre, p);
        }

    }
    //Metodo para saber si una instruccion contiene un llamado de funcion
    public static Match IsLlamado(string input)
    {
        Match match = Regex.Match(input, llamado);
        return match;

    }
    //Metodo para tratar con llamados de funciones
    public static string LlamadoFunciones(List<string> tokens, Funciones funciones)
    {

        //String que devolveremos al sustituir el llamado por el valor que da el resultado del llamado de la funcion
        string result = "";
        bool recorriendostring = false;
        //Recorrer la lista de tokes de atras hacia delante para asegurar no tener conflicto con las funciones que sean
        //llamadas como parametros de otras funciones
        for (int i = tokens.Count - 1; i >= 0; i--)
        {
            //Si en el recorrido nos encontramos con una variable y ademas nuestra lsta de nombres la contiene
            //estamos en presencia de un posible llamado de funcion
            if (tokens[i] == "\"" && !recorriendostring)
            {
                recorriendostring = true;
            }
            else if (tokens[i] == "\"" && recorriendostring)
            {
                recorriendostring = false;
            }
            if (Let_In.EsVariable(tokens[i]) && funciones.NombreFunciones.Contains(tokens[i]) && !recorriendostring)
            {
                //Para contar los parentesis
                int cont = 0;
                //Posicion donde abre el parentesis de los parametros
                int posAbre = 0;
                int posllamado = 0;
                //posicion donde cierra el parentesis de los parametros
                int posCierre = 0;
                //Posicion del llamado
                posllamado = i;
                //Realizamos un ciclo a partir del nombre del llamado hasta el parentesis que cierra los parametros
                //para obtener esa posicion
                for (int j = i + 1; j < tokens.Count; j++)
                {
                    //Si despues del nombre de esa funcion no aparece un PI lanzamos una excepcion
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
                //Recortamos la lista de tokens para quedarnos solo con los parametros
                string parametros = String.Join(" ", tokens.GetRange(posAbre + 1, posCierre - posAbre - 1));
                //Split por , para guardar cada uno de los parametros
                string[] arrayParametros = parametros.Split(",");
                //Si la cantidad de parametros es mayor que 1 significa que no puede estar vacio
                if (arrayParametros.Length > 1)
                {   //Verificar que ninguno de los parametros este vacio
                    for (int j = 0; j < arrayParametros.Length; j++)
                    {   //En caso de estar vacio lanzar una excepcion
                        if (arrayParametros[j] == "")
                        {
                            throw new ParametroVacio(tokens[i], j);
                        }
                    }
                }
                /////////////
                //varios casos de excepciones que tienen que ver con la cantidad de parametros
                if (funciones.Diccnameparametros[tokens[i]].Length > 1 && arrayParametros.Length == 1 && arrayParametros[0] == "")
                {
                    throw new CantidadParametros(tokens[i], funciones.Diccnameparametros[tokens[i]].Length, 0);
                }
                //Si la cantidad de parametros es distinta de la cantidad de parametros que recibe la funcion lanzar excepcion
                if (funciones.Diccnameparametros[tokens[i]].Length != arrayParametros.Length)
                {
                    throw new CantidadParametros(tokens[i], funciones.Diccnameparametros[tokens[i]].Length, arrayParametros.Length);
                }
                if (funciones.Diccnameparametros[tokens[i]].Length == 1 && funciones.Diccnameparametros[tokens[i]][0] != "" && arrayParametros.Length == 1 && arrayParametros[0] == "")
                {
                    throw new CantidadParametros(tokens[i], funciones.Diccnameparametros[tokens[i]].Length, 0);
                }
                //////////////
                //Si la funcion no recibe parametros
                if (funciones.Diccnameparametros[tokens[i]].Length == 1 && funciones.Diccnameparametros[tokens[i]][0] == "" && arrayParametros.Length == 1 && arrayParametros[0] == "")
                {
                    //Obtenemos el cuerpo de la funcion
                    string cuerpoF = funciones.Diccfunciones[tokens[i]];
                    //Removemos la lista desde la posicion de llamado hasta el cierre del parentesis de los parametros
                    tokens.RemoveRange(posllamado, posCierre - posllamado + 1);
                    //Insertamos en la posicion del llamado el valor de evaluar el cuerpo de la funcion
                    tokens.Insert(posllamado, EvaluadorExpresiones.QueEs(cuerpoF, funciones));
                    result = String.Join(" ", tokens);
                }
                //Si la funcion recibe parametros
                else
                {   //Diccionario donde guardaremos el nombre de cada una de las variables con su valor pasado como parametros
                    Dictionary<string, string> variables = new Dictionary<string, string>();
                    for (int j = 0; j < arrayParametros.Length; j++)
                    {   //Obtenemos el array de parametros de la funcion
                        string[] p = funciones.Diccnameparametros[tokens[i]];
                        Let_In.Define(p[j], EvaluadorExpresiones.QueEs(arrayParametros[j].ToString().Trim(), funciones), variables);
                    }
                    //Obtenemos el cuerpo de la funcion
                    string cuerpoF = funciones.Diccfunciones[tokens[i]];
                    //Sustituimos el valor de cada parametro por cada vez que aparece en el cuerpo
                    string cuerpo = Let_In.EvaluadorVariables(cuerpoF, variables);
                    tokens.RemoveRange(posllamado, posCierre - posllamado + 1);
                    tokens.Insert(posllamado, EvaluadorExpresiones.QueEs(cuerpo, funciones));
                    result = String.Join(" ", tokens);


                }
            }
            //Si no la tenemos incluida en la lista de nombres verificamos si es alguna de las funciones matematicas sin,cos,log
            else if (Let_In.EsVariable(tokens[i]) && (tokens[i] == "sin" || tokens[i] == "cos" || tokens[i] == "log") && !recorriendostring)
            {
                //Realizamos el mismo procedimiento hasta evaluar
                int cont = 0;
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
            else if (Let_In.EsVariable(tokens[i]) && !recorriendostring && tokens[i] == "PI")
            {
                tokens[i] = 3.14.ToString();
            }
            //Si no esta en nuestra lista de variables y ademas tampoco con las funciones matematicas basicas
            //y es una variable lanzar una excepcion
            else if (Let_In.EsVariable(tokens[i]) && !recorriendostring)
            {
                throw new VariableNoAsignada(tokens[i]);
            }
            //En cualquier otro caso continuar con el ciclo
            else
                continue;

        }
        return result;
    }

}
