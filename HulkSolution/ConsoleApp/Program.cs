using System;
using System.Text.RegularExpressions;
using ClassLibrary;


Funciones funciones = new Funciones();

string[] parametros = new string[] { "x" };
funciones.NombreFunciones.Add("print");
funciones.Diccfunciones["print"] = "x";
funciones.Diccnameparametros["print"] = parametros;

while (true)
{
    Console.WriteLine("Introduce tu codigo");
    //Console.Write(">");
    string input = Console.ReadLine()!;
    if (input == "S") break;

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
        string result = EvaluadorExpresiones.QueEs(input, funciones);
        if (Expresiones.EsString(result).Success)
        {
            result = result.Remove(0, 1).Trim();
            result = result.Remove(result.Length - 1).Trim();
            Console.WriteLine(result);
        }
        else
            Console.WriteLine(result.Trim());
    }
    catch (ParametroVacio)
    {
        continue;
    }
    catch (SeEspreaParentesis)
    {
        continue;
    }
    catch (FuncionAsignada)
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
    catch (ParteBooleanaIncorrecta)
    {
        continue;
    }
    catch (ParteiFVacia)
    {
        continue;
    }
    catch (ParteElseVacia)
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
    catch (ErrorSintacticoIf)
    {
        continue;
    }
    catch (ErrorSintacticoElse)
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



