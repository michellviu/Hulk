using System;


public class PalabrasResevadasExcepcion : Exception
{
    public PalabrasResevadasExcepcion(string palabra)
    {
        Console.WriteLine("El nombre " + "'" + palabra + "'" + " es una palabra reservada del lenguaje.");
    }

}
public class VariableAsignada : Exception
{
    public VariableAsignada(string variable)
    {
        Console.WriteLine("La variable " + "'" + variable + "'" + " ya esta asignada.");
    }
}

public class VariableNoAsignada : Exception
{
    public VariableNoAsignada(string variable)
    {
        Console.WriteLine("La variable o funcion '{0}' no esta definida en este ámbito.", variable);
    }
}
public class VariableNoValida : Exception
{
    public VariableNoValida(string variable)
    {
        Console.WriteLine("El nombre " + "'" + variable + "'" + " no es válido.");
    }
}

public class MalAsignacion : Exception
{
    public MalAsignacion(string variable)
    {
        Console.WriteLine("La variable " + "'" + variable + "'" + " no obtuvo valor.");
    }
}
public class SinNombre : Exception
{
    public SinNombre(string value)
    {
        Console.WriteLine("No existe una variable a la cual asignarle el valor {0}.", value);
    }
}
public class FaltaIgual : Exception
{
    public FaltaIgual()
    {
        Console.WriteLine("La asignacion de variable es incorrecta porque falta el operador '='.");
    }

}

public class ParentesisNoBalanceados : Exception
{
    public ParentesisNoBalanceados()
    {
        Console.WriteLine("Los parentesis de la expresion no estan bien balanceados.");
    }
    public ParentesisNoBalanceados(string funcion)
    {
        Console.WriteLine("Los parentesis de la funcion '{0}' no estan bien balanceados.", funcion);
    }
}
public class ErrorSintacticoLet : Exception
{
    public ErrorSintacticoLet(int pos)
    {
        Console.WriteLine("No existe una expresion 'in' para balancear la expresion 'let' de la posicion {0}.", pos);
    }
}

public class ErrorSintacticoIn : Exception
{
    public ErrorSintacticoIn(int pos)
    {
        Console.WriteLine("No existe una expresion 'let' para balancear la expresion 'in' de la posicion {0}.", pos);
    }
}

public class ParteLetVacia : Exception
{
    public ParteLetVacia()
    {
        Console.WriteLine("La asignacion de variables no contiene variables.");
    }
}

public class ParteInVacia : Exception
{
    public ParteInVacia()
    {
        Console.WriteLine("La asignacion de variables no contiene cuerpo.");
    }
}

public class Asignacion : Exception
{
    public Asignacion()
    {
        Console.WriteLine("En el cuerpo del 'let' no existe una expresion del tipo: 'variable'= 'expresion'.");
    }
}

public class ErrorSintacticoIf : Exception
{
    public ErrorSintacticoIf(int pos)
    {
        Console.WriteLine("No existe una expresion 'else' para balancear la expresion 'if' de la posicion {0}.", pos);
    }
}

public class ErrorSintacticoElse : Exception
{
    public ErrorSintacticoElse(int pos)
    {
        Console.WriteLine("No existe una expresion 'if' para balancear la expresion 'else' de la posicion {0}.", pos);
    }
}

public class SeEspreaParentesis : Exception
{
    public SeEspreaParentesis(string f)
    {
        Console.WriteLine("Despues de '{0}' se esperaba un parentesis izquierdo '('", f);
    }
}

public class CantidadParametros : Exception
{
    public CantidadParametros(string funcion, int parametrosesperados, int parametrosintroducidos)
    {
        Console.WriteLine("La funcion '{0}' recibe {1} parametros no {2}.", funcion, parametrosesperados, parametrosintroducidos);
    }
}
public class ParametroVacio : Exception
{
    public ParametroVacio(string funcion, int posicion)
    {
        Console.WriteLine("El parámetro de la posicion {0} de la funcion {1} está vacio.", posicion, funcion);
    }
}
public class OperacionInvalida : Exception
{
    public OperacionInvalida(string operando)
    {
        Console.WriteLine("El operando {0} no es valido en una expresion aritmetica");
    }
}






/*public class ParteiFVacia : Exception
{
    public ParteiFVacia()
    {
        Console.WriteLine("El cuerpo de la expresion 'if' esta vacio.");
    }
}

public class ParteElseVacia : Exception
{
    public ParteElseVacia()
    {
        Console.WriteLine("El cuerpo de la expresion 'else' esta vacio.");
    }
}*/










