using System;

//Excepcion para cuando la declaracion de una variable es una palabra reservada del lenguaje
public class PalabrasResevadasExcepcion : Exception
{
    public PalabrasResevadasExcepcion(string palabra)
    {
        Console.WriteLine("!SYNTAX ERROR: The name " + "'" + palabra + "'" + " is a reserved word of the language.");
    }

}
//Excepcion que se lanza si se va a declarar una variable o funcion que ya esta definida
public class VariableAsignada : Exception
{
    public VariableAsignada(string variable)
    {
        Console.WriteLine("!SEMANTIC ERROR: The variable " + "'" + variable + "'" + " is already assigned.");
    }
}
public class FuncionAsignada : Exception
{
    public FuncionAsignada(string variable)
    {
        Console.WriteLine("!SEMANTIC ERROR: The function " + "'" + variable + "'" + " is already defined.");
    }
}
//Excepcion que se lanza cuando una variable esta asignada
public class VariableNoAsignada : Exception
{
    public VariableNoAsignada(string variable)
    {
        Console.WriteLine("!SEMANTIC ERROR: The variable or function '{0}' is not defined in this scope.", variable);
    }
}
//Excepcion que se lanza cuando en la declaracion de variables la variable no es correcta
public class VariableNoValida : Exception
{
    public VariableNoValida(string variable)
    {
        Console.WriteLine("!SYNTAX ERROR: Token " + "'" + variable + "'" + " is invalid for a variable declaration.");
    }
}

public class MalAsignacion : Exception
{
    public MalAsignacion(string variable)
    {
        Console.WriteLine("!SYNTAX ERROR: The variable " + "'" + variable + "'" + " did not get a value in the 'let-in' expression.");
    }
}
public class SinNombre : Exception
{
    public SinNombre(string value)
    {
        Console.WriteLine("!SYNTAX ERROR: There is no variable in the 'let-in' expression to which to assign the value {0}.", value);
    }
}
public class FaltaIgual : Exception
{
    public FaltaIgual()
    {
        Console.WriteLine("!SYNTAX ERROR: Variable assignment is incorrect because the '=' operator is missing.");
    }

}

public class ParentesisNoBalanceados : Exception
{
    public ParentesisNoBalanceados()
    {
        Console.WriteLine("!SYNTAX ERROR: The parentheses of the expression are not well balanced.");
    }
    public ParentesisNoBalanceados(string funcion)
    {
        Console.WriteLine("!SYNTAX ERROR: Parentheses of function {0} are not well balanced.", funcion);
    }
}
public class ErrorSintacticoLet : Exception
{
    public ErrorSintacticoLet(int pos)
    {
        Console.WriteLine("!SYNTAX ERROR: There is no 'in' expression to balance the 'let' expression at position {0}.", pos);
    }
}

public class ErrorSintacticoIn : Exception
{
    public ErrorSintacticoIn(int pos)
    {
        Console.WriteLine("!SYNTAX ERROR: There is no 'let' expression to balance the 'in' expression at position {0}.", pos);
    }
}

public class ParteLetVacia : Exception
{
    public ParteLetVacia()
    {
        Console.WriteLine("!SEMANTIC ERROR: The 'let-in' expression does not contain variables or is empty in the variable declaration.");
    }
}

public class ParteInVacia : Exception
{
    public ParteInVacia()
    {
        Console.WriteLine("!SEMANTIC ERROR: 'let-in' expression does not contain body.");
    }
}

public class Asignacion : Exception
{
    public Asignacion()
    {
        Console.WriteLine("!SEMANTIC ERROR: In the body of the 'let' there is no expression of the type 'variable'='expression'.");
    }
}

public class ErrorSintacticoIf : Exception
{
    public ErrorSintacticoIf()
    {
        Console.WriteLine("!SYNTAX ERROR: There is no 'else' expression to balance the 'if' expression.");
    }
}

public class ErrorSintacticoElse : Exception
{
    public ErrorSintacticoElse()
    {
        Console.WriteLine("!SYNTAX ERROR: There is no 'if' expression to balance the 'else' expression.");
    }
}

public class SeEspreaParentesis : Exception
{
    public SeEspreaParentesis(string f)
    {
        Console.WriteLine("!SYNTAX ERROR: After '{0}' a left parenthesis was expected '('", f);
    }
}

public class CantidadParametros : Exception
{
    public CantidadParametros(string funcion, int parametrosesperados, int parametrosintroducidos)
    {
        Console.WriteLine("!SYNTAX ERROR: The function '{0}' receives {1} parameters not {2}.", funcion, parametrosesperados, parametrosintroducidos);
    }
}
public class ParametroVacio : Exception
{
    public ParametroVacio(string funcion, int posicion)
    {
        Console.WriteLine("!SYNTAX ERROR: The parameter at position {0} of the function {1} is empty.", posicion, funcion);
    }
}
public class OperacionInvalida : Exception
{
    public OperacionInvalida(string operando)
    {
        Console.WriteLine("El operando {0} no es valido en una expresion aritmetica");
    }
}
public class ParteiFVacia : Exception
{
    public ParteiFVacia()
    {
        Console.WriteLine("!SEMANTIC ERROR: The body of the 'if' expression is empty.");
    }
}

public class ParteElseVacia : Exception
{
    public ParteElseVacia()
    {
        Console.WriteLine("!SEMANTIC ERROR: The body of the 'else' expression is empty.");
    }
}
public class ParteBooleanaIncorrecta : Exception
{
    public ParteBooleanaIncorrecta()
    {
        Console.WriteLine("!SYNTAX ERROR: The conditional boolean is incorrect");
    }
}










