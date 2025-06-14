using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic
{
    public enum TokenType
    {
        // Single-character tokens.
        Left_paren,
        Rigth_paren,
        Left_clasp,
        Right_clasp,
        Comma,
        Dot,
        Semicolon,
        Plus,
        Minus,
        Star,
        Module,
        Split,
        TwoStar,

        // One or two character tokens.
        Equal,
        Equal_equal,
        Greater,
        Greater_equal,
        Less,
        Less_equal,
        BackSlach_n,
        And,
        Or,
        Less_minus,

        // Literals.
        Identifier,
        String,
        Number,


        // Methods Actions       
          Draw,
          GoTo,

        // Methods Information
        Request,      

        //  keywords
        True,
        False,
       
       // Internal
        EOF,

    }


    public class Tokens
    {
        public int line { get; set; }
        public string lexeme { get; set; }
        public TokenType type { get; set; }
        public Object literal { get; set; }
        public int orden { get; set; }

        public Tokens back { get; set; }
        public Tokens next { get; set; }


        public Tokens(TokenType type, String lexeme, Object literal, int line)
        {
            this.type = type;
            this.lexeme = lexeme;
            this.literal = literal;
            this.line = line;
       


        }
        public override string ToString()
        {
            return "<" + type + " " + lexeme + " " + literal + ">" + line;
            


        }

        
    }

}