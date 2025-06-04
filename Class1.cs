using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel;
using static System.Net.Mime.MediaTypeNames;
using Logic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Net;

namespace Logic
{
    public class ASTNode
    {
        public Tokens b { get; set; }
        public Tokens e { get; set; }
        public List<ASTNode> Childrens { get; set; }

        public ASTNode(Tokens b, Tokens e, List<Tokens> Childrens) 
        {
            this.b = b;
            this.e = e;
            this.Childrens = new List<ASTNode>();

        }

    }

    public class ASTCall : ASTNode
    {
        public ASTCall(Tokens b, Tokens e, List<Tokens> Childrens) : base(b, e, Childrens) { }
    }

    public class ASTCte : ASTNode
    {
        public ASTCte(Tokens b, List<Tokens> Childrens) : base(b, b, Childrens) { }

    }

    public class ASTRoot : ASTNode 
    {
        public ASTRoot(Tokens b, Tokens e, List<Tokens> Childrens) : base(b, e, Childrens) { }
    }


   


    public class Interprete 
    {
        public string[] lines {  get; set; }
        public string text { get; set; }
        public int actualline { get; set; }
        
        public Scanner scannner;

        public Ilogger logger;
        public Interprete(string text, Ilogger logger)
        {
            this.text = text;
            this.logger = logger;
            scannner = new Scanner(text, logger);

            lines = text.Split('\n');

            var tokens = scannner.scanTokens();
            var message = "BEGIN TOKENS:\r\n";
            foreach (var item in tokens)
            {
                message += item.ToString() + "\r\n";

            }
            message += "END\r\n";
            logger.Log("Lexer", message, 0);
        }

    }
}
