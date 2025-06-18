using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class ASTNode
    {
        public Tokens b { get; set; }
        public Tokens e { get; set; }
        public List<ASTNode> Childrens { get; set; }
        public string name { get; set; }

        public ASTNode(Tokens b, Tokens e, List<ASTNode> Childrens, string name)

        {
            this.b = b;
            this.e = e;
            this.Childrens = Childrens;
            this.name = name;

        }

        public override string ToString()
        {
            var cp = Childrens != null ? Childrens.Count : 0;
            string message = $"[{this.name}] b({b.ToString()}) e({e.ToString()}) |{cp}|\r\n";
            if(cp > 0)
            {
                message += "[\r\n";

                foreach (ASTNode child in Childrens)
                {
                    message += $"- {child.ToString()}" ;
                }

                message += "]\r\n";

            }
            return message;
            
        }
        public virtual T Visit<T>(IVisitor<T> visitor)

        {

            return visitor.Visit(this);

        }

    }
    public class ASTCall : ASTNode
    {
        public ASTCall(Tokens b, Tokens e, List<ASTNode> Childrens) : base(b, e, Childrens, "AST Call") { }

        public override T Visit<T>(IVisitor<T> visitor)

        {

            return visitor.Visit(this);

        }
    }
    public class ASTCte : ASTNode
    {
        public ASTCte(Tokens b) : base(b, b, null, "AST Cte") { }
        public override T Visit<T>(IVisitor<T> visitor)

        {

            return visitor.Visit(this);

        }

    }
    public class ASTRoot : ASTNode
    {
        public ASTRoot(Tokens b, Tokens e, List<ASTNode> Childrens) : base(b, e, Childrens, "AST Root") { }
        public override T Visit<T>(IVisitor<T> visitor)

        {

            return visitor.Visit(this);

        }
    }
    public class ASTId : ASTNode
    {
        public bool isLabel = false;
        public ASTId(Tokens b) : base(b, b, null, "AST Id") { }
        public override T Visit<T>(IVisitor<T> visitor)

        {

            return visitor.Visit(this);

        }

    }
    public class ASTAsignation : ASTNode
    {
        public ASTId id {  get; set; }
        public ASTNode expression { get; set; }

        public ASTAsignation(ASTId id, ASTNode expression) : base(id.b, expression.e, new List<ASTNode>(), "AST Asignation") 
        {
            this.id = id;
            this.expression = expression;
            this.Childrens.Add(id);
            this.Childrens.Add(expression);
            
        }

        public override T Visit<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);

        }

    }
    public class ASTBinaryExp : ASTNode
    {
        public Tokens op { get; set; }
        public ASTNode left { get; set; }
        public ASTNode right { get; set; }


        public ASTBinaryExp(ASTNode left, ASTNode right, Tokens op) : base(left.b, right.e, new List<ASTNode>(), "AST Binary")
        {
            this.left = left;
            this.right = right;
            this.op = op;
            this.Childrens.Add(left);
            this.Childrens.Add(right);
        }
        
        public override T Visit<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);

        }

    }
    public class ASTGoTo : ASTNode
    {

        public ASTId id { get; set; }
        public ASTNode expression { get; set; }

        public ASTGoTo(ASTId id, ASTNode expression) : base(id.b.back.back, expression.e, new List<ASTNode>(), "AST GoTo")
        {
            this.id = id;
            this.expression = expression;
            this.Childrens.Add(id);
            this.Childrens.Add(expression);

        }

        public override T Visit<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);

        }


    }
    public class ASTUnary : ASTNode
    {
        public Tokens op { get; set; }
       
        public ASTNode right { get; set; }


        public ASTUnary(Tokens op, ASTNode right) : base(op, right.e, new List<ASTNode>(), "AST Unary")
        {
          
            this.right = right;
            this.op = op;
            
            this.Childrens.Add(right);
        }

        public override T Visit<T>(IVisitor<T> visitor)
        {
            return visitor.Visit(this);

        }

    }

}
