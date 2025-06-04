using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public enum InstructionType
    {
        Draw,
        Request,
        Empty,
        Asignation,
        Loop,
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
        Instruction instruction;
        public GetValueVisitor(Ilogger logger, Instruction instruction)
        {
            this.logger = logger;
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
    }

    public class InstructionVisitor : IVisitor<Instruction>
    {

        Ilogger logger;
        public InstructionVisitor(Ilogger logger)
        {
            this.logger = logger;

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
            var inst =  new Instruction(InstructionType.Draw, node);

            inst.pasos.Add($"Getting args for {node.b.lexeme}");

            var args = "";

            var valueVisitor = new GetValueVisitor(logger, inst);

            if(node.Childrens != null)
            { 
                foreach(var child in node.Childrens)
                {
                    var value = child.Visit(valueVisitor);
                    args += $"{value},";
                    inst.argument.Add(value);
                }
            }


            inst.pasos.Add($"Call {node.b.lexeme}({args})");

            return inst;
        }

        public Instruction Visit(ASTCte node)
        {
            return new Instruction(InstructionType.Empty, node);
        }
    }
}
