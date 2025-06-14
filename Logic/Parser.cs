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
        public Context context;

        public Parser(Ilogger logger, Tokens b, Tokens e)
        {
            this.logger = logger;

            this.root = (logger.HasError) ? new ASTRoot(b, e, new List<ASTNode>()) : ParserRoot(b, e);

        }

        public Dictionary<string, List<(string, string)>> ArgsMetds = new Dictionary<string, List<(string, string)>>();
 
        public ASTNode ParserNode(Tokens b, Tokens e)
        {
            // Error handler created 

            var empty = new ASTNode(b, e, new List<ASTNode>(), "err");

            // stopper
            if (logger.HasError) return empty;

            if (b.orden > e.orden || b == null)
            {
                logger.LogError("Parser", "Missin Value", e.line);
            }

            if (b == e)
            {
                if (b.type == TokenType.Identifier)
                    return ParserId(b) ?? empty;

                return ParserCte(b) ?? empty;
            }
            
            else

            {
                if (IsOperador(b)) return ParserUnary(b, e) ?? empty;
                
                if (b.type == TokenType.GoTo)
                {
                    return ParserGoTo(b, e) ?? empty;
                }

                if (b.next != null && b.next.type == TokenType.Less_minus)
                    return ParserAsignation(b, e) ?? empty;

                if (b.type == TokenType.Draw || b.type == TokenType.Request)
                {
                    return ParserCall(b, e) ?? empty;
                }
                
                var token = GetSplitToken(b, e, logger);

                // stopper
                if (logger.HasError) return empty;


                if (token == null)
                {
                    if (b.type == TokenType.Left_paren && e.type == TokenType.Rigth_paren) 
                        return ParserNode(b.next, e.back) ?? empty;
                }

                return ParserBinaryExp(b, e) ?? empty;

            }
        }

        public ASTRoot ParserRoot(Tokens b, Tokens e)
        {
            // Error handler created 

            var childrens = new List<ASTNode>();
            var c = 0;
            var start = b;
            var current = b;
            while (current != null)
            {
                if (logger.HasError) break ;
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
                    c++;
                    if(c == 1)
                    {
                        var first = childrens[0] as ASTCall;
                        if(first == null || first.b.type != TokenType.Draw || first.b.lexeme != "Spawn")
                        {
                            logger.LogError("Parser", $"La primera instrucción debe ser una llamada a Spawn", b.line);
                        }
                    }
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
            if (b.type != TokenType.Number && b.type != TokenType.String && b.type != TokenType.True && b.type != TokenType.False)
            {
                logger.LogError("Parser", "Unexpected constante", b.line);
            }
            return new ASTCte(b);
        }
  
        public ASTCall ParserCall(Tokens b, Tokens e)
        {
            if (b.type != TokenType.Draw && b.type != TokenType.Request )
            {
                logger.LogError("Parser", $"Unexpected method {b.lexeme}", b.line);
                return null;
            }
            if (b.next.type != TokenType.Left_paren)
            {
              logger.LogError("Parser", "It was expected (", b.line);
              return null;
            }

            if (e.type != TokenType.Rigth_paren)
            {
                logger.LogError("Parser", "It was expected )", b.line);
                return null;
            }

            var childrens = new List<ASTNode>();

            var start = b.next.next;
            var current = start;

            // check if b is valid enum, start is ( and end is )

            while (current != null && start.type != TokenType.Rigth_paren)
            {
                if(current.type == TokenType.Comma || current == e)
                {
                    if (start == current)
                    {
                        logger.LogError("Parser", "Unexpected sintaxis[,,]. Faltan argumentos", start.line);
                       
                    }
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
            if (b.type != TokenType.Identifier)
            {
                logger.LogError("Parser", "It was expected a Identifier ", b.line);
            }
            return new ASTId(b);
        }
        public ASTAsignation ParserAsignation(Tokens b, Tokens e)
        {
            if (b.type != TokenType.Identifier)
            {
                logger.LogError("Parser", "It was expected a Identifier ", b.line);
                return null;
            }
            if (b.next.type != TokenType.Less_minus)
            {
                logger.LogError("Parser", "It was expected <- ", b.next.line);
                return null;
            }

            var id = ParserId(b);
            var exprr = ParserNode(b.next.next, e);

            return new ASTAsignation(id, exprr);
        }

        public ASTBinaryExp ParserBinaryExp(Tokens b, Tokens e)
        {
            var token = GetSplitToken(b, e, logger);
            if (token == null)
            {
                logger.LogError("Parser", "It was expected operator", b.line);
                return null;
            }
            if (!Dependencia.ContainsKey(token.type))
            {
                logger.LogError("Parser", "Unexpected operator", token.line);
                return null;
            }

            var left = ParserNode(b, token.back);
            var right = ParserNode(token.next, e);

            return new ASTBinaryExp(left, right, token);
        }
        public ASTUnary ParserUnary(Tokens op, Tokens e)
        {
            if (op.type != TokenType.Minus)
            {
                logger.LogError("Parser", "Unexpected operator", op.line);
                return null;
            }
            var right = ParserNode(op.next, e);

            return new ASTUnary(op, right);
        }

        public ASTGoTo ParserGoTo(Tokens b, Tokens e)
        {
            var current = b; // variable current para el tokens inicial
            if (current.type != TokenType.GoTo)
            { logger.LogError("Parser", "It was expected GoTo", current.line); 
                return null; 
            }
            current = current.next; // token [
            if (current.type != TokenType.Left_clasp)
            { logger.LogError("Parser", "It was expected [ ", current.line); 
                return null; 
            }
            current = current.next; // ID
            if (current.type != TokenType.Identifier) 
            {
                logger.LogError("Parser", "It was expected Identifier", current.line); 
                return null; 
            }
            var label = ParserId(current); // token identificador
            current = current.next; // token ]
            if (current.type != TokenType.Right_clasp)
            {
                logger.LogError("Parser", "It was expected ]", current.line);
                return null;
            }
            current = current.next; // token (
            if (current.type != TokenType.Left_paren)
            {
                logger.LogError("Parser", "It was expected ( ", current.line);
                return null;
            }
            current = current.next; // Expression
            if (e.type != TokenType.Rigth_paren)
            { logger.LogError("Parser", "It was expected ) ", current.line); 
                return null; 
            }
            var condicion = ParserNode(current, e.back);
            return new ASTGoTo(label, condicion);
        }


        public Dictionary<TokenType, int> Dependencia = new Dictionary<TokenType, int>()
        {
            {  TokenType.Minus, 1 },
            {  TokenType.Plus, 1},
            {  TokenType.Star, 2},
            {  TokenType.TwoStar, 2 },
            {  TokenType.Split, 2},
            {  TokenType.Module, 2 },
            {  TokenType.Or, 2 },
            {  TokenType.And, 1 },
            {  TokenType.Greater, 1 },
            {  TokenType.Less, 1},
            {  TokenType.Greater_equal, 1 },
            {  TokenType.Less_equal, 1},
            {  TokenType.Equal_equal, 1 },
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
                if (c < 0)
                {
                    logger.LogError("Parser", "unbalanced parentheses", start.line);
                    return null;
                }
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
            if (c  != 0)
            {
                logger.LogError("Parser", "unbalanced parentheses", start.line);
                return null;
            }
            return token;
        }


    }

}
