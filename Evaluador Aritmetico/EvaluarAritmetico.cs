
using System;
using System.Linq;
using System.Text.RegularExpressions;

public static class EvaluadorAritmetico
{

    //Metodo para determinar si una expresion aritmetica tiene los parentesis palanceados
    public static bool ParentesisBalanceados(string expresion)
    {
        Stack<char> parentesis = new Stack<char>(); //Pila para almacenar los parentesis izquierdos

        //Recorrer la expresion caracter por caracter
        for (int i = 0; i < expresion.Length; i++)
        {
            char c = expresion[i];//Obtener el caracter actual
            if (c == '(') // Si el caracter es una parentesis izquierdo añadirlo a la Pila
            {
                parentesis.Push(c);
            }
            else if (c == ')') //Si el caracter es un parentesis derecho, sacar un parentesis izquierdo de la pila si hay alguno
            {
                if (parentesis.Count > 0)//Si la pila no esta vacia, sacar un parentesis izquierdo
                {
                    parentesis.Pop();
                }
                else //Si la pila esta vacia significa que hay un parentesis derecho sin su correspondiente parentesis izquierdo
                    return false;
            }
        }
        if (parentesis.Count() == 0)//Si la pila esta vacia significa que todos los parentesis tiene su pareja
        {
            return true;
        }
        else //Si la pila no esta vacia significa que hay parentesis izquierdos que nunk se cierran
            return false;
    }

    //Metodo para determinar la precedencia de los operadores
    static int Precedencia(char op)
    {
        switch (op)
        {
            case '+': case '-': return 1;

            case '*': case '/': case '%': return 2;

            case '^': return 3;

            //Operador unario para el signo negativo
            case '_': return 4;

            default: return -1;
        }
    }

    //Metodo para convertir una expresion infija en una posfija

    public static string InfijaAPosfija(string infija)
    {
        List<string> tokens = Lexer.Tokenizador(infija);
        for (int i = 0; i < tokens.Count; i++)
        {
            if (Let_In.EsVariable(tokens[i]) && tokens[i] == "PI")
            {
                tokens[i] = 3.14.ToString();
            }
            else if (Let_In.EsVariable(tokens[i]))
            {
                throw new VariableNoAsignada(tokens[i]);
            }

        }
        infija = String.Join(" ", tokens);
        string posfija = ""; //Cadena para almacenar la cadena posfija

        Stack<char> pilaop = new Stack<char>(); //Pila para almacenar los operadores

        bool previoEsOperador = true; //Variable para indicar si el caracter previo es un operador o un parentesis izquierdo

        infija = Regex.Replace(infija, "\\s+", "");
        if (infija[0] == ' ')
            infija = infija.Trim();

        foreach (char c in infija) //Recorrer cada caracter de la expresion infija
        {
            if (char.IsDigit(c) || c == '.') //Si es un digito o un punto decimal agregarlo a la expresion posfija
            {
                if (previoEsOperador)
                    posfija += " " + c;
                else
                    posfija += c;

                previoEsOperador = false; //El operador previo no es un operador ni un parentesis izquierdo

            }
            else if (c == '(') //Si es un parentesis izquierdo apilarlo
            {
                pilaop.Push(c);
                previoEsOperador = true;//El operador previo es un parentesis izquierdo
            }
            else if (c == ')') //Si es un parentesis derecho, desapilar los operadores hasta encontrar el parentesis izquierdo correspondiente y agregarlos a la expresion posfija
            {
                while (pilaop.Count() > 0 && pilaop.Peek() != '(')
                {
                    posfija += " " + pilaop.Pop();
                }

                if (pilaop.Count() > 0 && pilaop.Peek() == '(') // Eliminar el parentesis izquierdo de la pila
                {
                    pilaop.Pop();
                }

                previoEsOperador = false; //El operador previo no es un operador ni un parentesis izquierdo
            }
            else if (c == '-')//Si es un signo negativo, verificar si es parte del operando o si es un operador unario
            {
                if (previoEsOperador)//Si el caracter previo es un operador o un parentesis izquierdo, tratar el signo negativo como parte del numero
                {
                    posfija += " ";
                    while (pilaop.Count() > 0 && Precedencia('_') <= Precedencia(pilaop.Peek())) //Desapilar los operadores de mayor o igual importancia y agregarlos a la expresion posfija
                    {
                        posfija += " " + pilaop.Pop();

                    }
                    pilaop.Push('_'); //Apilar el operador unario
                    previoEsOperador = true;
                }
                else //Si el caracter previo es parte del operando, tratar el signo negativo como el operador binario de resta
                {
                    posfija += " ";
                    while (pilaop.Count > 0 && Precedencia('-') <= Precedencia(pilaop.Peek())) //Desapilar los operadores de mayor o igual importancia y agregarlos a la expresion posfija
                    {
                        posfija += " " + pilaop.Pop();
                    }
                    pilaop.Push('-'); //Apilar el operador binario '-'
                    previoEsOperador = true; //El caracter previo es un operador
                }
            }

            else //Si es otro operador, desapilar los operadores de mayor o igual importancia y agregarlos a la expresion posfija, luego apilar el operador actual
            {
                posfija += " ";
                while (pilaop.Count() > 0 && Precedencia(c) <= Precedencia(pilaop.Peek()))
                {
                    posfija += " " + pilaop.Pop();
                }

                pilaop.Push(c);
                previoEsOperador = true; //El caracter previo es operador
            }
        }
        //Desapilar los operadores restantes y agregarlos a la expresion posfija
        while (pilaop.Count() > 0)
        {
            posfija += " " + pilaop.Pop();
        }

        return posfija; //Devolver la expresion posfija
    }


    //Metodo para evaluar una expresion posfija
    public static double EvaluarPosfija(string posfija)
    {
        posfija = InfijaAPosfija(posfija);
        posfija = Regex.Replace(posfija, "\\s+", " ");
        if (posfija[0] == ' ')
            posfija = posfija.Trim();

        Stack<double> pilaoperandos = new Stack<double>(); //Pila para almacenar los operandos
        foreach (string token in posfija.Split())//Recorrer cada token de la expresion posfija
        {
            if (double.TryParse(token, out double num))//Si es un numero apilarlo
            {
                pilaoperandos.Push(num);
            }
            else //Si es un operador, desapilar uno o dos operandos, realizar la operacion y apilar el resultado
            {
                double op1, op2, res = 0;

                try
                {

                    switch (token)
                    {
                        case "+":
                            op2 = pilaoperandos.Pop();
                            op1 = pilaoperandos.Pop();
                            res = op1 + op2;
                            break;
                        case "-":
                            op2 = pilaoperandos.Pop();
                            op1 = pilaoperandos.Pop();
                            res = op1 - op2;
                            break;
                        case "*":
                            op2 = pilaoperandos.Pop();
                            op1 = pilaoperandos.Pop();
                            res = op1 * op2;
                            break;
                        case "/":
                            op2 = pilaoperandos.Pop();
                            op1 = pilaoperandos.Pop();
                            res = op1 / op2;
                            break;
                        case "%":
                            op2 = pilaoperandos.Pop();
                            op1 = pilaoperandos.Pop();
                            res = op1 % op2;
                            break;
                        case "^":
                            op2 = pilaoperandos.Pop();
                            op1 = pilaoperandos.Pop();
                            res = Math.Pow(op1, op2);
                            break;
                        case "_":
                            op1 = pilaoperandos.Pop();
                            res = -op1;
                            break;
                        default:
                            break;
                    }
                }
                catch (InvalidOperationException e)
                {
                    throw e;
                }

                pilaoperandos.Push(res);
            }
        }
        return pilaoperandos.Pop(); //Devolver el resultado final
    }
}
