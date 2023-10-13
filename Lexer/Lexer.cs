public static class Lexer
{
    /*public List<string> tokens { get; private set; }

    public Lexer()
    {
        tokens = new List<string>();
    }*/

    public static List<string> Tokenizador(string input)
    {
        List<string> tokens = new List<string>();
        //  bool recorriendostring = false;
        string tokenactual = "";

        for (int i = 0; i < input.Length; i++)
        {
            if (Cambio(input[i]))
            {
                tokenactual.Trim();
                if (tokenactual != "")
                {
                    tokens.Add(tokenactual.Trim());
                    tokenactual = "";
                }
                if (!EsEspacio(input[i]))
                {
                    tokens.Add(input[i].ToString().Trim());
                    continue;
                }
                continue;
            }

            tokenactual += input[i];
            if (EsFinal(i, input.Length - 1))
            {
                tokens.Add(tokenactual.Trim());
            }
        }

        for (int i = 1; i < tokens.Count - 1; i++)
        {

            if (EsComparador(tokens[i - 1], tokens[i], tokens[i + 1]))
            {
                string cambio = tokens[i - 1].ToString() + tokens[i];
                tokens.RemoveRange(i - 1, 2);
                tokens.Insert(i - 1, cambio);
            }
        }

        return tokens;
        ///////////

    }

    static bool Cambio(char caracter)
    {
        switch (caracter)
        {
            case '(': return true;
            case ')': return true;
            case ',': return true;
            case '.': return true;
            case '-': return true;
            case '/': return true;
            case '^': return true;
            case '%': return true;
            case '+': return true;
            case '*': return true;
            case '!': return true;
            case '=': return true;
            case '<': return true;
            case '>': return true;
            case ' ': return true;
            case '"': return true;
        }
        return false;
    }

    static bool EsFinal(int posicion, int length)
    {
        return posicion == length;
    }
    static bool EsEspacio(char caracter)
    {
        if (caracter == ' ') return true;
        return false;
    }
    static bool EsComparador(string caracter1, string caracter2, string caracter3)
    {
        if (caracter1 == "!")
        {
            switch (caracter2)
            {
                case "<": return true;
                case ">": return true;
                case "=": return true;
            }
        }

        if (caracter2 == "=")
        {
            switch (caracter1)
            {
                case "<": return true;
                case ">": return true;
                case "=": return true;
            }
        }
        switch (caracter2)
        {
            case "<": return true;
            case ">": return true;

        }

        return false;

    }
}
