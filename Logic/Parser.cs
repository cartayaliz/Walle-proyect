using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
  

    public class Parser
    {
        public Ilogger logger;
        public ASTRoot root;

        public Parser(Ilogger logger, Tokens b, Tokens e)
        {
            this.logger = logger;

            this.root = ParserRoot(b, e);

        }

        public Dictionary<string, List<(string, string)>> ArgsMetds = new Dictionary<string, List<(string, string)>>();
 
        public ASTNode ParserNode(Tokens b, Tokens e)
        {
            if (b == e)
            {
                return ParserCte(b);
            }
            
            else

            {
                // TODO: Add here the others nodes
                return ParserCall(b, e);

            }
        }

        public ASTRoot ParserRoot(Tokens b, Tokens e)
        {
            var childrens = new List<ASTNode>();
            var start = b;
            var current = b;
            while (current != null)
            {
                if (current.type == TokenType.EOF || current.type == TokenType.BackSlach_n)
                {
                    childrens.Add(ParserNode(start, current.back));
                    current = current.next;
                    start = current;
                }
                else
                {
                    current = current.next;
                }
            }
            return new ASTRoot(b, e, childrens);
        }

        public ASTCte ParserCte(Tokens b)
        {
            return new ASTCte(b);
        }
  
        public ASTCall ParserCall(Tokens b, Tokens e)
        {
            var childrens = new List<ASTNode>();

            var start = b.next.next;
            var current = start;

            // check if b is valid enum, start is ( and end is )

            while (current != null)
            {
                if(current.type == TokenType.Comma || current == e)
                {
                    childrens.Add(ParserNode(start, current.back));
                    if(current == e)
                    {
                        break;
                    }
                    current = current.next;
                    start = current;

                }
                else
                {
                    current = current.next;
                }
            }



            return new ASTCall(b, e, childrens);
        }
    }
}
