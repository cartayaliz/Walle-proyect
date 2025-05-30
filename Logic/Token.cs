using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public enum TokenType
    {
        // Single-character tokens.
        Left_paren,
        Rigth_paren,
        Comma,
        Dot,
        Minus,
        Plus,
        Semicolon,
        Star,



        // One or two character tokens.
        Equal,
        Equal_equal,
        Greater,
        Greater_equal,
        Less,
        Less_equal,

        // Literals.
        Identifier,
        String,
        Number,

        // Methods Actions
        Fill,
        DrawLine,
        DrawRectangle,
        DrawCruadado,
        DrawTriangle,
        DrawCircle,
        //DrawRombo,
        Spawn,

        // Methods Information

        //GetActualX,
        //GetActualY,
        //GetCanvasSize,
        //GetColorCount,
        //IsCanvasColor,
        //IsBrushSize,
        //IsBrushColor,


        // Internal
        EOF,

      
    }
   
    public class Tokens
    {
        public int line { get; set; }
        public string lexeme { get; set; }
        public TokenType type { get; set; }
        public Object literal { get; set; }

        public Tokens(TokenType type, String lexeme, Object literal, int line)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
        }
        public String toString()
        {
            return type + " " + lexeme + " " + literal;
        }

        
    }

}