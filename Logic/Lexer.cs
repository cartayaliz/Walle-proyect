using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
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

        public Dictionary<string, TokenType> Operador = new Dictionary<String, TokenType>()
        {
            {  "Minus", TokenType.Minus },
            {  "Plus", TokenType.Plus },
            {  "Star", TokenType.Star },
            {  "TwoStar", TokenType.TwoStar },
            {  "Split", TokenType.Split },
            {  "Module", TokenType.Module },
       
        };     

        public Dictionary<string, TokenType> keywords = new Dictionary<String, TokenType>()
        {
            { "true", TokenType.True },
            { "false", TokenType.False },
            { "DrawLine", TokenType.Draw },
            { "DrawCircle", TokenType.Draw},
            { "DrawRectangle", TokenType.Draw },
            { "DrawTriangle", TokenType.Draw },
            { "DrawCuadrado", TokenType.Draw },
            { "DrawRombo", TokenType.Draw },
            { "DrawAsterisco", TokenType.Draw },
            { "Spawn", TokenType.Draw },
            { "Color", TokenType.Draw },
            { "GetActualX", TokenType.Request },
            { "GetActualY", TokenType.Request },
            { "GetCanvasSize", TokenType.Request },
            { "IsCanvasColor", TokenType.Request },
            { "IsBrushSize", TokenType.Request },
            { "IsBrushColor", TokenType.Request },
            { "GetCountColor", TokenType.Request },
            { "Size", TokenType.Request },
            { "Fill", TokenType.Draw },
            { "GoTo", TokenType.GoTo },


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
                logger.LogError("Lexer", "Unterminated string.", line);
                return;
            }
            // The closing ".
            advance();

            // Removing quotation marks

            string value = source.Substring(start + 1, current - 2 - start);
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

                
                case '[': tokens.Add(new Tokens(TokenType.Left_clasp, "[", null, line)); break;
                case ']': tokens.Add(new Tokens(TokenType.Right_clasp, "]", null, line)); break;
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
                case '&':
                    {
                        if (match('&'))
                        { tokens.Add(new Tokens(TokenType.And, "&", null, line)); break; }
                        else
                        {
                            logger.LogError("Lexer", "It was expected &&", line); 
                            return;
                        }
                    }
                case '|':
                    {

                        if (match('|'))
                            { tokens.Add(new Tokens(TokenType.Or, "|", null, line)); break; }
                        else
                        {
                           logger.LogError("Lexer", "It was expected ||", line); 
                            return;
                        }

                    }


                case '=':
                    {
                        if (match('='))
                        { tokens.Add(new Tokens(TokenType.Equal_equal, "==", null, line)); break; }
                        else
                        {
                            logger.LogError("Lexer", "It was expected == ", line);
                            return;
                        }
                    }
                case '>':
                    {
                        bool m = match('=');
                        tokens.Add(new Tokens(m ? TokenType.Greater_equal : TokenType.Greater, m ?  ">=" : ">", null, line)); break; }
                case '<':
                    {
                        bool m = match('=');
                        bool h = match('-');
                        tokens.Add(new Tokens(m? TokenType.Less_equal : h? TokenType.Less_minus : TokenType.Less, m? "<=" : "<", null, line)); break;
                    }
                case '%': tokens.Add(new Tokens(TokenType.Module, "%", null, line)); break;
                case '/': tokens.Add(new Tokens(TokenType.Split, "/", null, line)); break;
                case '\n': tokens.Add(new Tokens(TokenType.BackSlach_n, "\n", null, line)); line++; break;
                case ' ' : break; 
                case '\r': break;
                case '"': String(); break;

                default:


                    if (isDigit(c))
                    {
                        number();
                    }
                    else if (isAlpha(c))
                    {
                        while (isAlpha(peek()) || isDigit(peek()))
                        {
                            advance();
                        }
                 
                        string Text = source.Substring(start, current - start);
                        TokenType type;
                        if (keywords.TryGetValue(Text, out type))
                        {
                            tokens.Add(new Tokens(type, Text, null, line));
                        }
                        else
                        {
                            tokens.Add(new Tokens(TokenType.Identifier, Text, null, line));
                        }
                        
                    }
                    else
                    {
                        logger.LogError("Lexer", $"Unexpected character: [\'{c}\'] .", line);
                        return;
                    }
                    break;

            }

        }

    }
}

