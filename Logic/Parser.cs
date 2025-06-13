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
                if (b.type == TokenType.Identifier)
                    return ParserId(b);
                return ParserCte(b);
            }
            
            else

            {
                if (IsOperador(b)) return ParserUnary(b, e);


                if (b.next != null && b.next.type == TokenType.Less && b.next.next.type == TokenType.Minus)
                    return ParserAsignation(b, e);
                
                var token = GetSplitToken(b, e, logger);
                
                if (token == null)
                {
                    if (b.type == TokenType.Left_paren) return ParserNode(b.next, e.back);
                    
                    return ParserCall(b, e);
                }

                return ParserBinaryExp(b, e);

            }
        }

        public ASTRoot ParserRoot(Tokens b, Tokens e)
        {
            var childrens = new List<ASTNode>();
            var start = b;
            var current = b;
            while (current != null)
            {
                if (start.type == TokenType.EOF) break;
                if(start.type == TokenType.BackSlach_n)
                {
                    current = start.next;
                    start = current;
                    continue;
                }
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

            while (current != null && start.type != TokenType.Rigth_paren)
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
        public ASTId ParserId(Tokens b)
        {
            return new ASTId(b);
        }
        public ASTAsignation ParserAsignation(Tokens b, Tokens e)
        {
            //if(b.type != TokenType.Identifier) da el berro

            var id = ParserId(b);
            var exprr = ParserNode(b.next.next.next, e);

            return new ASTAsignation(id, exprr);
        }

        public ASTBinaryExp ParserBinaryExp(Tokens b, Tokens e)
        {
            var token = GetSplitToken(b, e, logger);

            var left = ParserNode(b, token.back);
            var right = ParserNode(token.next, e);

            return new ASTBinaryExp(left, right, token);
        }
        public ASTUnary ParserUnary(Tokens op, Tokens e)
        {
            var right = ParserNode(op.next, e);

            return new ASTUnary(op, right);
        }

        public ASTGoTo ParserGoTo(ASTRoot Root, Tokens line)
        {
            for (int i = 0; i < Root.Childrens.Count; i++)
            {
                if (Root.Childrens[i].b.type == TokenType.Identifier)
                {
                    var etiqueta = Root.Childrens[i].b;
                    return new ASTGoTo(etiqueta, line);
                }
               
            }
            return null;
        }


        public Dictionary<TokenType, int> Dependencia = new Dictionary<TokenType, int>()
        {
            {  TokenType.Minus, 1 },
            {  TokenType.Plus, 1},
            {  TokenType.Star, 2},
            {  TokenType.TwoStar, 2 },
            {  TokenType.Split, 2},
            {  TokenType.Module, 2 },

        };
        public bool IsOperador(Tokens token)
        {
            return Dependencia.ContainsKey(token.type);
        }
        public Tokens GetSplitToken(Tokens start, Tokens end, Ilogger ilogger)
        {
            int c = 0;
            int p = (int)1e6;

            Tokens token = null;
            var node = start;

            while (node != end.next)
            {
                if (node.type == TokenType.Left_paren) c++;
                if (node.type == TokenType.Rigth_paren) c--;
                else if (IsOperador(node) && c == 0)
                {

                    int value = 0;
                    Dependencia.TryGetValue(node.type, out value);
                    int pn = value;
                    if (pn < p && value>0)
                    {
                        p = pn;
                        token = node;
                    }
                }
                node = node.next;
            }
            return token;
        }


    }

}
