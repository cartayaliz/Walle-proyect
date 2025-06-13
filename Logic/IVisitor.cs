using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic;

namespace Logic
{
    public interface IVisitor <T>
    {

        T Visit(ASTNode node);
        T Visit(ASTRoot node);

        T Visit(ASTCall node);

        T Visit(ASTCte node);

        T Visit(ASTId node);

        T Visit(ASTAsignation node);

        T Visit(ASTBinaryExp node);

        T Visit(ASTUnary node);

        T Visit(ASTGoTo node);

    }
}

