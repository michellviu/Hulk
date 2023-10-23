using System;
using System.Text.RegularExpressions;

public class Let_In
{
    public static string[] palabrasreservadas { get; } = new string[] { "if", "else", "let", "in", "function", "true", "false" };

    public static int posl = 0;
    public static int posIn = 0;
    public static int posT = 0;

    //Metodo para definir una variable con un nombre y un valor
    public static void Define(string name, string value, Dictionary<string, string> variables)
    {
        for (int i = 0; i < palabrasreservadas.Length; i++)
        {
            //Si el nombre de la variable es una palabra reservada lanzar un error
            if (name == palabrasreservadas[i])
            {
                throw new PalabrasResevadasExcepcion(palabrasreservadas[i]);
            }

        }
        //Si ya existe una variable con ese nombre lanzar una excepcion
        if (variables.ContainsKey(name))
        {
            throw new VariableAsignada(name);
        }
        else
            variables[name] = value;

    }

    ///////////////////////////////////////////////////////////////////////////////////////
    //Metodo para dividir una expresion LetIn
    //El funcionamiento se basa en buscar el in que le corresponde a la instruccion let, asi como verificar que ni el cuerpo
    //ni la declaracion de variables sea vacio
    public static string[] DivideLet_In(List<string> tokens)
    {
        int cont = 0;
        int poslet = 0;
        int posin = 0;
        int postermina = tokens.Count;
       Let_In.posT = tokens.Count;
        bool entro = true;
        List<string> partelet = new List<string>();
        List<string> partein = new List<string>();
        for (int i = 0; i < tokens.Count; i++)
        {   //Encontrar el let llevando un contador
            if (tokens[i] == "let")
            {
                if (poslet == 0 && entro)
                {
                    posl = i;
                    poslet = i;
                    entro = false;
                }
                cont++;
            }
            //Encontrar el In correspondiente
            else if (tokens[i] == "in")
            {
                cont--;
                if (cont == 0)
                {
                    posIn = i;
                    posin = i;
                    break;
                }
            }
        }
        //Comprobar que hay la misma cantidad de let que de in
        if (cont > 0)
            throw new ErrorSintacticoLet(poslet);

        if (cont < 0)
            throw new ErrorSintacticoIn(posin);

        /* if (tokens[posin + 1] == "(")
         {
             int cont2 = 0;
             posEmIn = posin + 1;
             for (int i = posin + 1; i < tokens.Count; i++)
             {
                 if (tokens[i] == "(")
                 {
                     cont2++;
                 }
                 if (tokens[i] == ")")
                 {
                     cont2--;
                     if (cont2 == 0)
                     {
                         postermina = i;
                         posT = i;
                         break;
                     }
                 }

             }
         }*/

        for (int i = poslet + 1; i < posin; i++)
        {
            partelet.Add(tokens[i]);
        }
        if (tokens[posin + 1] == "(")
        {
            for (int i = posin + 2; i < postermina; i++)
            {
                partein.Add(tokens[i]);
            }
        }
        else
        {
            for (int i = posin + 1; i < postermina; i++)
            {
                partein.Add(tokens[i]);
            }
        }
        if (partelet == null) throw new ParteLetVacia();
        if (partein == null) throw new ParteInVacia();

        string[] DivisionLet = new string[2];
        DivisionLet[0] = String.Join(" ", partelet).Trim();
        DivisionLet[1] = String.Join(" ", partein).Trim();

        return DivisionLet;

    }
    //Metodo para dividir la declaracion de variables cuando se tratar de la asignacion de varias variables
    public static List<List<string>> DivideVariables(List<string> tokens, Funciones funciones1)
    {
        // List<string> tokens = new List<string>();
        bool recorriendoLet = false;
        int cont = 0;
        bool recorriendoString = false;
        List<int> posiciones = new List<int>();
        List<List<string>> variables = new List<List<string>>();

        for (int i = 0; i < tokens.Count; i++)
        {
            if (tokens[i] == "let")
            {
                cont++;
                recorriendoLet = true;
            }
            if (tokens[i] == "in")
            {
                cont--;
                if (cont == 0) recorriendoLet = false;
            }
            if (tokens[i] == "\"" && !recorriendoString)
            {
                recorriendoString = true;
            }
            if (tokens[i] == "\"" && recorriendoString)
            {
                recorriendoString = false;
            }
            if (funciones1.NombreFunciones.Contains(tokens[i]))
            {
                int cont1 = 0;
                int posCierre = 0;
                if (!EvaluadorAritmetico.ParentesisBalanceados(String.Join(" ", tokens)))
                {
                    throw new ParentesisNoBalanceados(tokens[i]);
                }
                for (int j = i; j < tokens.Count; j++)
                {
                    if (tokens[i + 1] != "(")
                    {
                        break;
                    }
                    if (tokens[j] == "(")
                    {

                        cont1++;
                    }
                    else if (tokens[j] == ")")
                    {
                        cont1--;
                        if (cont1 == 0)
                        {
                            posCierre = j;
                            break;
                        }
                    }
                }
                i = posCierre;
            }
            if (tokens[i] == "," && !recorriendoLet && !recorriendoString)
            {
                posiciones.Add(i);
            }

        }
        //Añadir la posicion 0
        posiciones.Insert(0, 0);
        posiciones.Add(tokens.Count);
        //Recorrer la lista de posiciones y obtener las sublistas correspondientes
        for (int i = 0; i < posiciones.Count - 1; i++)
        {
            //Obtener el inicio y fin de cada sublista
            int inicio = posiciones[i];
            int fin = posiciones[i + 1];

            //Obtener la lista con los elementos entre inicio y fin
            List<string> sublista = tokens.GetRange(inicio, fin - inicio);

            //Añadir
            if (sublista[0] == ",") sublista.RemoveAt(0);
            variables.Add(sublista);

        }
        return variables;

    }
    //Metodo para asignar el valor a una variable 
    public static void AsignaVariable(List<string> item, Dictionary<string, string> variables, Funciones funciones1)
    {
        //caracter para separar
        char sep = '=';

        string regex = Regex.Escape(sep.ToString()); //Expresion regular del separador
                                                     //separar el nombre del valor
        string[] nameValue = String.Join(" ", item).Split(new string[] { regex }, 2, StringSplitOptions.None);
        //nombre de la variable
        string name = nameValue[0].Trim();
        //valor de la variable
        string value = nameValue[1].Trim();
        if (value == "" && name != "") throw new MalAsignacion(name);
        if (name == "") throw new SinNombre(value);

        Define(name, EvaluadorExpresiones.QueEs(value, funciones1), variables);
    }
    //Metodo para determinar si un input es una variable
    public static bool EsVariable(string v)
    {   //Patron de variables
        string variable = @"^[a-zA-Z]+[\w]*$";
        Match match = Regex.Match(v, variable);
        if (match.Success)
        {   //Excluir el caso de que sea una palabra reservada
            for (int i = 0; i < palabrasreservadas.Length; i++)
            {
                if (v == palabrasreservadas[i]) return false;
            }
            return true;
        }
        return false;
    }
    //Metodo para guardar el valor de cada variable en un diccionario
    public static void GuardarVariables(List<List<string>> variables, Dictionary<string, string> Diccvariables, Funciones funciones1)
    {

        foreach (var item in variables)
        {
            if (item.Count <= 2)
            { throw new Asignacion(); }
            if (EsVariable(item[0]) && item[1] == "=")
            {
                AsignaVariable(item, Diccvariables, funciones1);
            }
            else if (!EsVariable(item[0]))
                throw new VariableNoValida(item[0]);

        }

    }

    //Metodo para analizar el cuerpo de una expresion Let in y asignar a cada variable su valor
    public static string EvaluadorVariables(string input, Dictionary<string, string> variables)
    {

        //Para saber si estamos recorriendo una variable
        bool recorriendovariable = false;
        //Para saber si estamos recorriendo un string
        bool recorriendostring = false;
        //En caso de que estemos recorriendo una variable ir guardando su valor hasta el momento
        string variableactual = "";
        //String final con las variables asignadas
        string resultado = "";

        //Recorrer caracter a caracter el cuerpo del in
        for (int i = 0; i < input.Length; i++)
        {
            //Obtener el caracter actual
            char c = input[i];
            if (c == '"' && !recorriendostring) //Si es una comilla y no estamos recorriendo un string significa que es 
            {                                   //el comienzo de un string
                resultado += c;
                recorriendostring = true;
            }
            else if (c == '"' && recorriendostring)//Si es una comilla y estamos recorriendo un string significa que es
            {                                      //el fin de un string
                resultado += c;
                recorriendostring = false;
            }
            else if (char.IsLetter(c) && !recorriendostring)//Si es una letra y no estamos recorriendo un string
            {                                               // significa que estamos leyendo una variable
                                                            //Estamos leyendo una variable
                recorriendovariable = true;
                variableactual += c;
            }
            else if (char.IsNumber(c) && recorriendovariable)//Si es un numero y estamos recorriendo una variable
            {                                                //se lo agregamos al nombre de la variable actual
                variableactual += c;
            }
            else
            {
                //Para saber si el nombre coincide con una palabra reservada del lenguaje
                bool reservada = false;
                for (int j = 0; j < palabrasreservadas.Length; j++)
                {
                    if (variableactual == palabrasreservadas[j])
                    {
                        reservada = true;
                        break;
                    }
                }
                if (reservada) //Si coincide con una palabra reservada estamos seguros de que no es una variable
                {              //y por tanto la sumamos al resultado
                    //Verificar si es una declaracion de variables dentro de otra declaracion de variables
                    //Con el objetivo de verificar si la variable que se usa esta declarada o no
                    if (variableactual == "let")
                    {
                        int aux;
                        string aux2;
                        for (int j = i + 1; j < input.Length; j++)
                        {
                            if (input[j] == '=')
                            {
                                aux = j;
                                aux2 = input.Substring(i, aux - i);
                                i = j;
                                //Si la variable ya esta declarada en ese ambito lanzar una excepcion
                                if (variables.ContainsKey(aux2.Trim()))
                                {
                                    throw new VariableAsignada(aux2);
                                }
                                //En caso contrario agregar al resultado
                                resultado += variableactual;
                                resultado += aux2;
                                resultado += input[j];
                                break;
                            }
                        }
                    }
                    else
                        resultado += variableactual + c;
                }
                //Si estamos leyendo variable
                else if (recorriendovariable)
                {
                    //Si nuestro diccionario actual contiene a la variable actual la sustituimos y la agregamos al resultado
                    if (variables.ContainsKey(variableactual))
                    {
                        resultado += variables[variableactual] + c;
                    }
                    else if (variableactual == "PI")
                    {
                        resultado += 3.14 + c;
                    }
                    else
                        resultado += variableactual + c;

                }
                else //Si no es una variable ni una palabra reservada la sumamos al resultado
                    resultado += c;
                recorriendovariable = false;
                variableactual = "";


            }
        }
        if (recorriendovariable) //Si termine de recorrer y me quede recorriendo una variable significa que se encontraba 
        {                        //al final de la linea por tanto hago el mismo procedimiento

            if (variables.ContainsKey(variableactual))
            {
                resultado += variables[variableactual];
            }
            else
                resultado += variableactual;
        }
        input = resultado;
        return input;
    }

    //Esto es un metodo secundario para Evaluar Variables, no se usa en el proyecto por ahora, pero como esta sujeto a 
    //cambios es posible que en un futuro se use
    /* public static List<string> EvaluadorVariable(List<string> partein,int empieza,int termina, Dictionary<string, string> Variables)
     {

         //Para saber si estamos recorriendo una variable
         // bool recorriendovariable = false;
         //Para saber si estamos recorriendo un string
         bool recorriendostring = false;
         //bool recorriendolet = false;
         //En caso de que estemos recorriendo una variable ir guardando su valor hasta el momento
         // string variableactual = "";
         //String final con las variables asignadas

         //Recorrer caracter a caracter el cuerpo del in
         for (int i = empieza; i <= termina; i++)
         {
             //Obtener el caracter actual
             string c = partein[i];
             if (c == "\"" && !recorriendostring) //Si es una comilla y no estamos recorriendo un string significa que es 
             {                                   //el comienzo de un string
                 partein[i] = c;
                 recorriendostring = true;
             }
             else if (c == "\"" && recorriendostring)//Si es una comilla y estamos recorriendo un string significa que es
             {                                      //el fin de un string
                 partein[i] = c;
                 recorriendostring = false;
             }
             else if (partein[i] == "let")
             {
                 partein[i] = c;
                 c = partein[i + 1];
                 if (Variables.ContainsKey(c))
                 {
                     throw new VariableAsignada(c);
                 }

             }
             else if (EsVariable(c) && !recorriendostring)
             {
                 //Si nuestro diccionario actual contiene a la variable actual la sustituimos y la agregamos al resultado
                 if (Variables.ContainsKey(c))
                 {
                     partein[i] = Variables[c];
                 }
                 //Si no lanzamos una excepcion
                 else
                     throw new VariableNoAsignada(c);
             }
             else
             {
                 partein[i] = c;
             }

         }
        // string result = string.Join(" ", partein);
         return partein;
     }*/

}

