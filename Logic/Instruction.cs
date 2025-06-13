using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logic
{
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
        public GetValueVisitor(Ilogger logger, Instruction instruction, ExeMemory exememory)
        {
            this.logger = logger;
            this.exememory = exememory;
            this.instruction = instruction;
        }
        public (string, string) Visit(ASTNode node)
        {
            return ("number", "0");
        }
        public (string, string) Visit(ASTRoot node)
        {
            return ("number", "0");
        }

        public (string, string) Visit(ASTCall node)
        {
            return ("number", "0");

        }

        public (string, string) Visit(ASTCte node)
        {

            var result = ("number", "0");

            if (node.e.type == TokenType.Number)
            {
                result=  ("number", node.e.lexeme);
            }
            if (node.e.type == TokenType.True)
            {
                result = ("number", "1");
            }
            if (node.e.type == TokenType.False)
            {
                result = ("number", "0");
            }
            if (node.e.type == TokenType.String)
            {
                result = ("string", node.e.lexeme);
            }

            this.instruction.pasos.Add($"get from cte the value: {result.ToString()}");

            return result;

        }
        public (string, string) Visit(ASTId node)
        {
            return exememory.getValue(node.b.lexeme);
        }

        public (string, string) Visit(ASTAsignation node)
        {
            return node.expression.Visit(this);
        }

        //public (string, string) Visit(AST)
        public (string, string) Visit(ASTBinaryExp node)
        {
            var left = node.left.Visit(this);
            var right = node.right.Visit(this);

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
                    return ("number", "0");
                }
            }
            else if (node.op.type == TokenType.Greater_equal)
            {
                if (int.Parse(left.Item2) >= int.Parse(right.Item2)) return ("number", "1");
                else
                {
                    return ("number", "0");
                }
            }
            else if (node.op.type == TokenType.Less)
            {
                if (int.Parse(left.Item2) < int.Parse(right.Item2)) return ("number", "1");
                else
                {
                    return ("number", "0");
                }
            }
            else if (node.op.type == TokenType.Less_equal)
            {
                if (int.Parse(left.Item2) <= int.Parse(right.Item2)) return ("number", "1");
                else
                {
                    return ("number", "0");
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


            return ("number", "0");
        }

        public (string, string) Visit(ASTUnary node)
        {
            var right = node.right.Visit(this);
            if (node.op.type == TokenType.Minus)
            {
                return ("number", (-1 * int.Parse(right.Item2)).ToString()  );
            }
            return ("number", "0");
        }

        public (string, string) Visit(ASTGoTo node)
        {
           return ("number", "0");
        }
    }

    public class InstructionVisitor : IVisitor<Instruction>
    {

        Ilogger logger;
        ExeMemory exememory;
        Dictionary<string, string> mapped;


        public InstructionVisitor(Ilogger logger, ExeMemory exememory, Dictionary<string, string> mapped)
        {
            this.logger = logger;
            this.exememory = exememory;
            this.mapped = mapped;

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

                var valueVisitor = new GetValueVisitor(logger, inst, this.exememory);

                if (node.Childrens != null)
                {
                    foreach (var child in node.Childrens)
                    {
                        var value = child.Visit(valueVisitor);
                        args += $"{value},";
                        inst.argument.Add(value);
                    }
                }

                inst.pasos.Add($"Call {node.b.lexeme}({args})");

                return inst;

            }
            else
            {
                var inst = new Instruction(InstructionType.Request, node);

                var args = "";

                var valueVisitor = new GetValueVisitor(logger, inst, this.exememory);

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

            var valueVisitor = new GetValueVisitor(logger, inst, this.exememory);

            var value = node.expression.Visit(valueVisitor);

            inst.pasos.Add($"{node.id.b.lexeme} = {value.ToString()}");

            exememory.setValue(node.id.b.lexeme, value);

            return inst;

        }

        public Instruction Visit(ASTBinaryExp node)
        {
            var inst = new Instruction(InstructionType.Empty, node);

            var valueVisitor = new GetValueVisitor(logger, inst, this.exememory);

            var left = node.left.Visit(valueVisitor);
            var right = node.right.Visit(valueVisitor);


            inst.pasos.Add($"{left.ToString()} {node.op.lexeme} {right.ToString()}");

            return inst;
        }

        public Instruction Visit(ASTUnary node)
        {
            var inst = new Instruction(InstructionType.Empty, node);

            var valueVisitor = new GetValueVisitor(logger, inst, this.exememory);

            var right = node.right.Visit(valueVisitor);

            inst.pasos.Add($"{node.op.lexeme} {right.ToString()}");

            return inst;
        }

        public Instruction Visit(ASTGoTo node)
        {
            var inst = new Instruction(InstructionType.Empty, node);

            var valueVisitor = new GetValueVisitor(logger, inst, this.exememory);

            var cond = node.expression.Visit(valueVisitor);

            var condV = int.Parse(cond.Item2);

            inst.pasos.Add($"{node.expression.ToString()} = {cond} ({condV > 0})");


            if (condV > 0)
            {
                inst.type = InstructionType.GoTo;

                var redirect = ("number", "0");

                if (mapped.ContainsKey(node.id.b.lexeme))
                    redirect.Item2 = mapped[node.id.b.lexeme];
                else
                {
                    // get a error here for not defining label
                }

                inst.argument.Add(redirect);

                inst.pasos.Add($"GOTO {node.id.b.lexeme} -> {redirect}");

            }
            return inst;
        }
    }
}
