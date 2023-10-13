using System;
using System.Text.RegularExpressions;


Funciones funciones = new Funciones();

string[] parametros = new string[] { "x" };
funciones.NombreFunciones.Add("print");
funciones.Diccfunciones["print"] = "x";
funciones.Diccnameparametros["print"] = parametros;

string[] parametros2 = new string[] { "x", "y" };
funciones.NombreFunciones.Add("Sum");
funciones.Diccfunciones["Sum"] = "x+y";
funciones.Diccnameparametros["Sum"] = parametros2;


while (true)
{
    Console.WriteLine("Introduce tu codigo");
     string input = "if(let x=2 in x == 2) x else 2;";
    //Let_In v = new Let_In();
    //string input = Console.ReadLine()!;
    if (input == "s") break;

    input = Regex.Replace(input, "\\s+", " ").Trim();

    if (input.Last() != ';')
    {
        Console.WriteLine("!SYNTAX ERROR: missing character ';' at the end of the line ");
        continue;
    }
    else
    {
        if (input.Last() == ';')
            input = input.Remove(input.Length - 1);
    }
    try
    {
        Console.WriteLine(EvaluadorExpresiones.QueEs(input, funciones));
    }
    catch (ParametroVacio)
    {
        continue;
    }
    catch (VariableNoValida)
    {
        continue;
    }
    catch (CantidadParametros)
    {
        continue;
    }
    catch (ErrorSintacticoLet)
    {
        continue;
    }
    catch (ErrorSintacticoIn)
    {
        continue;
    }
    catch (Asignacion)
    {
        continue;
    }
    catch (PalabrasResevadasExcepcion)
    {
        continue;
    }
    catch (VariableNoAsignada)
    {
        continue;
    }
    catch (VariableAsignada)
    {
        continue;
    }
    catch (MalAsignacion)
    {
        continue;
    }
    catch (FaltaIgual)
    {
        continue;
    }
    catch (ParentesisNoBalanceados)
    {
        continue;
    }
    catch (InvalidOperationException)
    {
        Console.WriteLine("Operacion aritmetica invalida");
        continue;
    }


}



