using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
 
    public class Lexer
    {
        public Lexer() 
        { 
          
        }
    }

    public class Scanner
    {
        private String source { get; set; }
        private List<Tokens> tokens { get; set; }

        private int start {  get; set; }
        private int current {  get; set; }
        private int line {  get; set; }
        Scanner(String source)
        {
            this.source = source;
            this.start = 0;
            this.current = 0;
            this.line = 1;
            this.tokens = new List<Tokens>();
        }
        private bool isAtEnd()
        {
            return (current >= source.Length);
        }
        
        List<Tokens> scanTokens()
        {
            while (!isAtEnd())
            {
                // We are at the beginning of the next lexeme.

                start = current;
                scanToken();
            }
            tokens.Add(new Tokens(TokenType.EOF, "", null, line));
            return tokens;
        }
        private char advance()
        {
            current++;
            return source[current - 1];
        }
        private void scanToken()
        {
            char c = advance();
            switch (c)
            {
                case '(': tokens.Add(new Tokens(TokenType.Left_paren, "(", null, line)); break;
                case ')': tokens.Add(new Tokens(TokenType.Rigth_paren, ")", null, line)) ; break;
                case ',': tokens.Add(new Tokens(TokenType.Comma, ",", null, line)); break;
                case '.': tokens.Add(new Tokens(TokenType.Dot, ".", null, line)); break;
                case '-': tokens.Add(new Tokens(TokenType.Minus, "-", null, line)); break;
                case '+': tokens.Add(new Tokens(TokenType.Plus, "+", null, line)); break;
                case ';': tokens.Add(new Tokens(TokenType.Semicolon, ";", null, line)); break;
                case '*': tokens.Add(new Tokens(TokenType.Star, "*", null, line)); break;
                    //default:
                    //    .error(line, "Unexpected character.");
                    //break;
            }
        }
    }
}

