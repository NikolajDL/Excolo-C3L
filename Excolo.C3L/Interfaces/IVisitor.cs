using Excolo.C3L.SyntaxAnalysis.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Interfaces
{
    /// <summary>
    /// An interface for a visitor
    /// </summary>
    public interface IVisitor
    {
        object Visit(ProgramSequence arg, object obj);
        object Visit(ProgramSequenceEmpty arg, object obj);
        object Visit(Command arg, object obj);
        object Visit(ArgumentSequence arg, object obj);
        object Visit(ArgumentSequenceEmpty arg, object obj);
        object Visit(Argument arg, object obj);
    }
}
