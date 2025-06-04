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
        string name { get; set; }

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
        public override T Visit <T>(IVisitor<T> visitor)

        {

            return visitor.Visit(this);

        }
    }

}
