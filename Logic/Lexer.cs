using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

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
        public string source { get; set; }
        public List<Tokens> tokens { get; set; }

        public int start {  get; set; }
        public int current {  get; set; }
        public int line {  get; set; }
       
        Ilogger logger;
        public Scanner(string source, Ilogger logger)
        {
            this.logger = logger;
            this.source = source;
            this.start = 0;
            this.current = 0;
            this.line = 1;
            this.tokens = new List<Tokens>();

        }
        public Dictionary<string, TokenType> keywords = new Dictionary<String, TokenType>()
        {
            { "true", TokenType.True },
            { "false", TokenType.False },
            { "DrawLine", TokenType.DrawLine },
            { "DrawCircle", TokenType.DrawCircle},
            { "DrawRectangle", TokenType.DrawRectangle },
            { "DrawTriangle", TokenType.DrawTriangle },
            { "DrawCuadrado", TokenType.DrawCruadado },
            { "DrawRombo", TokenType.DrawRombo },
            { "DrawAsterisco", TokenType.DrawAsterisco },
            { "Spawn", TokenType.Spawn },

        };

        public bool isAtEnd()
        {
            return (current >= source.Length);
        }

        
        public List<Tokens> scanTokens()
        {
            while (!isAtEnd())
            {
                // Start of the next lexeme

                start = current;
                scanToken();
            }
            tokens.Add(new Tokens(TokenType.EOF, "", null, line));
            return tokens;
        }
        public char advance()
        {
            current++;
            return source[current - 1];
        }
        public bool match(char expected)
        {
            if (isAtEnd()) return false;
            if (source[current] != expected) return false;
            current++;
            return true;
        }
       public void String()
        {
            while (peek() != '"' && !isAtEnd())
            {
                if (peek() == '\n') line++;
                advance();
            }
            if (isAtEnd())
            {
                logger.LogError(null, "Unterminated string.", line);
                return;
            }
            // The closing ".
            advance();

            // Removing quotation marks

            String value = source.Substring(start + 1, current - 1 - start);
            tokens.Add(new Tokens(TokenType.String, value, null, line));
        }
        public bool isDigit(char c)  
        {
            return (c >= '0' && c <= '9');
        }

        public char peek()
        {
            if (isAtEnd()) return ' ';
            return source[current];

        }
        public char peekNext()
        {
            if (current + 1 >= source.Length) return ' ';
            return source[current + 1];
        }
       public void number()
        { 
            while (isDigit(peek())) advance();
            // Look for a fractional part.
            if (peek() == '.' && isDigit(peekNext()))
            {
                // Consume the "."
                advance();
                while (isDigit(peek())) advance();
            }
            tokens.Add(new Tokens(TokenType.Number, source.Substring(start, current - start), null, line));
        }

        public void Identificador()                                                   
        {
            while (isAlpha(peek()) || isDigit(peek())) { advance(); }
            tokens.Add(new Tokens(TokenType.Identifier, "identificador", null, line));

        }
        public bool isAlpha(char c)
        {
            return ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_');
        }

        public void scanToken()
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
                case '*':
                    { 
                        bool m = match('*');
                        tokens.Add(new Tokens(m ? TokenType.TwoStar : TokenType.Star, m?"**":"*", null, line)); break;

                    }
                case '=':
                    {
                       
                        bool m = match('=');
                        tokens.Add(new Tokens(m ? TokenType.Equal_equal : TokenType.Equal, m?  "==" : "=", null, line)); break; }
                case '>':
                    {
                        bool m = match('=');
                        tokens.Add(new Tokens(m ? TokenType.Greater_equal : TokenType.Greater, m ?  "<=" : "<", null, line)); break; }
                case '<':
                    {
                        bool m = match('=');
                        tokens.Add(new Tokens(m? TokenType.Less_equal : TokenType.Less, m? ">=" : ">", null, line)); break;
                    }
                case '%': tokens.Add(new Tokens(TokenType.Module, "%", null, line)); break;
                case '/': tokens.Add(new Tokens(TokenType.Split, "/", null, line)); break;
                case '\n':line++; break;
                case ' ' : break; 
                case '"': String(); break;

                default:
                    if (isDigit(c))
                    {
                        number();
                    }
                    else if (isAlpha(c))
                    {
                        //Identificador();
                        string Text = source.Substring(start, current - start);
                        TokenType type;
                        if (!keywords.TryGetValue(Text, out type))
                        {
                            type = TokenType.Identifier;
                        }
                        //bool m = match('Identifier');
                        tokens.Add(new Tokens(TokenType.Identifier, Text, null, line));
                    }
                    else
                    {
                        logger.LogError(null, "Unexpected character.", line);
                    }
                    break;

            }

        }

    }
}

