using System;
using System.Collections.Generic;

namespace Scanner
{
    public class MatchAutomat
    {
        public readonly string fragment;
        public readonly string position;
        public readonly string length;
        public MatchAutomat(string fragment, string position, string length)
        {
            this.fragment = fragment;
            this.position = position;
            this.length = length;
        }
    }
    public class Automat
    {
        private string text;
        private char liter;
        private int currentPosition = 0;
        private int positionLine = 0;
        private int currentLine = 1;
        private string buffer = "";
        private List<MatchAutomat> matchAutomat = new List<MatchAutomat>();
        public List<MatchAutomat> run(string inputText)
        {
            text = inputText;

            getNext();

            while (currentPosition <= text.Length)
            {
                if (char.IsLower(liter))
                {
                    buffer += liter;
                    bool hasUnderscore = false;

                    while (char.IsLower(liter = getChar()) || char.IsDigit(liter) || liter == '_')
                    {
                        buffer += liter;
                        if (liter == '_') hasUnderscore = true;
                    }

                    if (hasUnderscore && buffer[buffer.Length - 1] != '_' && !buffer.Contains("__"))
                    {
                        addMatchAutomat(buffer);
                    }
                    buffer = "";
                }
                getNext();
            }
            return matchAutomat;
        }
        private char getChar()
        {
            try
            {
                if (currentPosition >= text.Length)
                {
                    currentPosition++;
                    positionLine++;
                    return '\0';
                }
                char liter1 = text[currentPosition];
                currentPosition++;
                positionLine++;
                return liter1;
            }
            catch (IndexOutOfRangeException ex)
            {
                throw new Exception("В конце строки не обнаружено ;");
            }
        }
        private void getNext()
        {
            liter = getChar();
        }
        private void addMatchAutomat(string name)
        {
            int Length = name.Length;
            int leng = positionLine - Length;
            string loc = $"строка {currentLine}, {leng}";
            matchAutomat.Add(new MatchAutomat(name, loc, Length.ToString()));
        }
    }
}
