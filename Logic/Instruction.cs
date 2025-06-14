using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic
{

    class GLOBALS
    {
        public static (string, string) EMPTY_VALUE = ("number", "0");

        public static string LOG_LEXER = "Lexer";
        public static string LOG_PARSER = "Parser";
        public static string LOG_EXECUTER = "Executer";

        public static string NUMBER_TYPE = "number";
        public static string STRING_TYPE = "string";

        public static Dictionary<string, string[]> ARGS_MAPPED = new Dictionary<string, string[]>()
        {
            { "Spawn", new string[2]{ NUMBER_TYPE, NUMBER_TYPE } },
            { "DrawLine", new string[3]{ NUMBER_TYPE, NUMBER_TYPE, NUMBER_TYPE } },
            { "DrawRectangle", new string[5]{ NUMBER_TYPE, NUMBER_TYPE, NUMBER_TYPE,  NUMBER_TYPE, NUMBER_TYPE } },
            { "DrawCuadrado", new string[4]{ NUMBER_TYPE, NUMBER_TYPE, NUMBER_TYPE,  NUMBER_TYPE } },
            { "DrawCircle",new string[3]{ NUMBER_TYPE, NUMBER_TYPE, NUMBER_TYPE } },
            { "DrawTriangle", new string[6]{ NUMBER_TYPE, NUMBER_TYPE, NUMBER_TYPE, NUMBER_TYPE, NUMBER_TYPE, NUMBER_TYPE } },
            { "DrawAsterisco", new string[3]{ NUMBER_TYPE, NUMBER_TYPE, NUMBER_TYPE } },
            { "DrawRombo", new string[3]{ NUMBER_TYPE, NUMBER_TYPE, NUMBER_TYPE } },
            { "Fill", new string[0]{ }  },
            
            { "Color", new string[1]{ STRING_TYPE } },
            { "Size", new string[1]{ NUMBER_TYPE }  },

            { "GetActualX", new string[0]{ } },
            { "GetActualY", new string[0]{ } },
            { "GetCanvasSize",  new string[0]{ } },
            { "GetCountColor", new string[5]{ STRING_TYPE, NUMBER_TYPE, NUMBER_TYPE,  NUMBER_TYPE, NUMBER_TYPE } },

            { "IsBrushSize", new string[1]{ NUMBER_TYPE } },
            { "IsBrushColor", new string[1]{ STRING_TYPE } },
            { "IsCanvasColor", new string[3]{ STRING_TYPE, NUMBER_TYPE, NUMBER_TYPE }},
        };

        public static bool MatchArgsCount(string key, int count, Ilogger logger)
        {
            if (ARGS_MAPPED.ContainsKey(key)) return ARGS_MAPPED[key].Length == count;

            logger.LogWarning("ARGS", $"MISSING KEY METHOD {key}", 0);

            return false;
        }

        public static bool MatchArgsValue(string key, List<(string, string)> values, Ilogger logger, Tokens b)
        {
            if (!MatchArgsCount(key, values.Count, logger)) return false;

            var args = ARGS_MAPPED[key];
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] != values[i].Item1)
                {
                    logger.LogError(LOG_EXECUTER, $"Invalid args in position {i} expected {args[i]} found {values[i].Item1}", b.line);
                    return false;
                }
            }
            return true;
        }
    }

    public enum InstructionType
    {
        Draw,
        Request,
        Empty,
        Asignation,
        GoTo,

    }
    public class Instruction
    {
        public InstructionType type;
        public  List<(string, string)> argument = new List<(string, string)>(); // Tipo, valor en string

        public ASTNode origin { get; set; }

        public List<string> pasos = new List<string>();

        public Instruction( InstructionType type , ASTNode origin)
        {
            this.type = type;
            this.origin = origin;
       
        }

    }

    public class GetValueVisitor : IVisitor<(string, string)>
    {

        Ilogger logger;
        ExeMemory exememory;
        Instruction instruction;
        Executer executer;
        public GetValueVisitor(Ilogger logger, Instruction instruction, ExeMemory exememory, Executer executer)
        {
            this.logger = logger;
            this.exememory = exememory;
            this.instruction = instruction;
            this.executer = executer;
        }
        public (string, string) Visit(ASTNode node)
        {
            return GLOBALS.EMPTY_VALUE;
        }
        public (string, string) Visit(ASTRoot node)
        {
            return GLOBALS.EMPTY_VALUE;
        }

        public (string, string) Visit(ASTCall node)
        {
            if(node.b.type == TokenType.Request)
            {
                var args = new List<(string, string)>();
                if (node.Childrens != null)
                {
                    foreach (var child in node.Childrens)
                    {
                        var value = child.Visit(this);
                        args.Add(value);
                    }
                }

                if (!GLOBALS.MatchArgsValue(node.b.lexeme, args, logger, node.b))
                {
                    return GLOBALS.EMPTY_VALUE;
                }

                return executer.GetRequestContext(node, args);
            }
            return GLOBALS.EMPTY_VALUE;
        }

        public (string, string) Visit(ASTCte node)
        {
           
            var result = GLOBALS.EMPTY_VALUE;

            if (node.e.type == TokenType.Number)
            {
                result=  (GLOBALS.NUMBER_TYPE, node.e.lexeme);
            }
            if (node.e.type == TokenType.True)
            {
                result = (GLOBALS.NUMBER_TYPE, "1");
            }
            if (node.e.type == TokenType.False)
            {
                result = (GLOBALS.NUMBER_TYPE, "0");
            }
            if (node.e.type == TokenType.String)
            {
                result = (GLOBALS.STRING_TYPE, node.e.lexeme);
            }

            this.instruction.pasos.Add($"get from cte the value: {result.ToString()}");

            return result;

        }
        public (string, string) Visit(ASTId node)
        {
            if(!exememory.hasKey(node.b.lexeme))
            {
                logger.LogError("Executer", $"Undefined variable [\'{node.b.lexeme}\']", node.b.line);
                return GLOBALS.EMPTY_VALUE;
            }
            return exememory.getValue(node.b.lexeme);
        }

        public (string, string) Visit(ASTAsignation node)
        {
            return node.expression.Visit(this);
        }

        //public (string, string) Visit(AST)
        public (string, string) Visit(ASTBinaryExp node)
        {
            // stopper
            if (logger.HasError) return GLOBALS.EMPTY_VALUE;

            var left = node.left.Visit(this);
            var right = node.right.Visit(this);
            if(left.Item1 != right.Item1)
            {
                logger.LogError("Executer", "type of variable is different", node.op.line);
                return GLOBALS.EMPTY_VALUE;
            }

            if(node.op.type == TokenType.Plus)
            {
                return ("number", (int.Parse(left.Item2) + int.Parse(right.Item2)).ToString());

            }
            else if (node.op.type == TokenType.Star)
            {
                return ("number", (int.Parse(left.Item2) * int.Parse(right.Item2)).ToString());
            }
            else if (node.op.type == TokenType.Minus)
            {
                return ("number", (int.Parse(left.Item2) - int.Parse(right.Item2)).ToString());
            }
            else if (node.op.type == TokenType.TwoStar)
            {
                return ("number", (Math.Pow(int.Parse(left.Item2), int.Parse(right.Item2)).ToString()));
            }
            else if (node.op.type == TokenType.Split)
            {
                if(int.Parse(right.Item2) == 0 || right.Item2 == null)
                {
                    logger.LogError("Executer", "Cannot be divided by zero", node.op.line);
                }
                return ("number", (int.Parse(left.Item2) / int.Parse(right.Item2)).ToString());
            }
            else if (node.op.type == TokenType.Module)
            {
                return ("number", (int.Parse(left.Item2) % int.Parse(right.Item2)).ToString());
            }
            else if(node.op.type == TokenType.Greater)
            {
                if (int.Parse(left.Item2) > int.Parse(right.Item2)) return ("number", "1");
                else
                {
                    return GLOBALS.EMPTY_VALUE;
                }
            }
            else if (node.op.type == TokenType.Greater_equal)
            {
                if (int.Parse(left.Item2) >= int.Parse(right.Item2)) return ("number", "1");
                else
                {
                    return GLOBALS.EMPTY_VALUE;
                }
            }
            else if (node.op.type == TokenType.Less)
            {
                if (int.Parse(left.Item2) < int.Parse(right.Item2)) return ("number", "1");
                else
                {
                    return GLOBALS.EMPTY_VALUE;
                }
            }
            else if (node.op.type == TokenType.Less_equal)
            {
                if (int.Parse(left.Item2) <= int.Parse(right.Item2)) return ("number", "1");
                else
                {
                    return GLOBALS.EMPTY_VALUE;
                }
            }
            else if (node.op.type == TokenType.And)
            {
                return ("number", (int.Parse(left.Item2) & int.Parse(right.Item2)).ToString());
            }
            else if (node.op.type == TokenType.Or)
            {
                return ("number", (int.Parse(left.Item2) | int.Parse(right.Item2)).ToString());
            }


            return GLOBALS.EMPTY_VALUE;
        }

        public (string, string) Visit(ASTUnary node)
        {
            var right = node.right.Visit(this);
            if (right.Item1 != "number")
            {
                logger.LogError("Executer", $"Debe ser numero { right.Item2 }", node.b.line);
                return GLOBALS.EMPTY_VALUE;
            }

            if (node.op.type == TokenType.Minus)
            {
                return ("number", (-1 * int.Parse(right.Item2)).ToString()  );
            }
            return GLOBALS.EMPTY_VALUE;
        }

        public (string, string) Visit(ASTGoTo node)
        {
           return GLOBALS.EMPTY_VALUE;
        }
    }

    public class InstructionVisitor : IVisitor<Instruction>
    {

        Ilogger logger;
        ExeMemory exememory;
        Dictionary<string, string> mapped;
        Executer executer;

        public InstructionVisitor(Ilogger logger, ExeMemory exememory, Dictionary<string, string> mapped, Executer executer)
        {
            this.logger = logger;
            this.exememory = exememory;
            this.mapped = mapped;
            this.executer = executer;
        }
        public Instruction Visit(ASTNode node)
        {
            return new Instruction(InstructionType.Empty, node);
        }

        public Instruction Visit(ASTRoot node)
        {
            return new Instruction(InstructionType.Empty, node);
        }

        public Instruction Visit(ASTCall node)
        {


            var type = node.b;
            if(type.type == TokenType.Draw )
            {
                var inst = new Instruction(InstructionType.Draw, node);

                inst.pasos.Add($"Getting args for {node.b.lexeme}");

                var args = "";

                var valueVisitor = new GetValueVisitor(logger, inst, this.exememory, executer);

                if (node.Childrens != null)
                {
                    foreach (var child in node.Childrens)
                    {
                        var value = child.Visit(valueVisitor);
                        args += $"{value},";
                        inst.argument.Add(value);
                    }
                }

                if (!GLOBALS.MatchArgsValue(node.b.lexeme, inst.argument, logger, node.b))
                {
                    return new Instruction(InstructionType.Empty, node);
                }


                inst.pasos.Add($"Call {node.b.lexeme}({args})");

                return inst;

            }
            else
            {
                var inst = new Instruction(InstructionType.Request, node);

                var args = "";

                var valueVisitor = new GetValueVisitor(logger, inst, this.exememory, executer);

                if (node.Childrens != null)
                {
                    foreach (var child in node.Childrens)
                    {
                        var value = child.Visit(valueVisitor);
                        args += $"{value},";
                        inst.argument.Add(value);
                    }
                }
                return inst;

            }


        }

        public Instruction Visit(ASTCte node)
        {
            return new Instruction(InstructionType.Empty, node);
        }

        public Instruction Visit(ASTId node)
        {
            var inst =  new Instruction(InstructionType.Empty, node);

            return inst;

        }

        public Instruction Visit(ASTAsignation node)
        {
            var inst =  new Instruction(InstructionType.Empty, node);

            var valueVisitor = new GetValueVisitor(logger, inst, this.exememory, executer);

            var value = node.expression.Visit(valueVisitor);

            inst.pasos.Add($"{node.id.b.lexeme} = {value.ToString()}");

            exememory.setValue(node.id.b.lexeme, value);

            return inst;

        }

        public Instruction Visit(ASTBinaryExp node)
        {
            var inst = new Instruction(InstructionType.Empty, node);

            var valueVisitor = new GetValueVisitor(logger, inst, this.exememory, executer);

            var left = node.left.Visit(valueVisitor);
            var right = node.right.Visit(valueVisitor);


            inst.pasos.Add($"{left.ToString()} {node.op.lexeme} {right.ToString()}");

            return inst;
        }

        public Instruction Visit(ASTUnary node)
        {
            var inst = new Instruction(InstructionType.Empty, node);

            var valueVisitor = new GetValueVisitor(logger, inst, this.exememory, executer);

            var right = node.right.Visit(valueVisitor);

            inst.pasos.Add($"{node.op.lexeme} {right.ToString()}");

            return inst;
        }

        public Instruction Visit(ASTGoTo node)
        {
            var inst = new Instruction(InstructionType.Empty, node);

            var valueVisitor = new GetValueVisitor(logger, inst, this.exememory, executer);

            var cond = node.expression.Visit(valueVisitor);

            var condV = int.Parse(cond.Item2);

            inst.pasos.Add($"{node.expression.ToString()} = {cond} ({condV > 0})");


            if (condV > 0)
            {
                inst.type = InstructionType.GoTo;

                var redirect = GLOBALS.EMPTY_VALUE;

                if (mapped.ContainsKey(node.id.b.lexeme))
                    redirect.Item2 = mapped[node.id.b.lexeme];
                else
                {
                    logger.LogError("Executer", "Dont exist label", node.b.line);
                }

                inst.argument.Add(redirect);

                inst.pasos.Add($"GOTO {node.id.b.lexeme} -> {redirect}");

            }
            return inst;
        }
    }
}
