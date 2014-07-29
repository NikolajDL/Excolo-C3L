using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.SyntaxAnalysis.AST
{
    /// <summary>
    /// A class defining an empty argument sequence associated with a command, 
    /// meaning the command has no arguments.
    /// </summary>
    public class ArgumentSequenceEmpty : BaseAST, IArgumentSequence
    {
        /// <summary>
        /// Get the arguments in this sequence - which in this case will always be empty.
        /// </summary>
        public IEnumerable<IArgument> Arguments
        {
            get {  yield break; }
        }

        /// <summary>
        /// A constructor for the argument sequence.
        /// </summary>
        /// <param name="position">The position of the sequence.</param>
        public ArgumentSequenceEmpty(int position = -1)
            : base(position)
        {
        }

        /// <summary>
        /// A method to visit/accept a visitor, for the visitor pattern.
        /// </summary>
        /// <param name="visitor">The Visitor being accepted.</param>
        /// <param name="obj">The object calling the visit method - null if this is the root.</param>
        public override object Visit(Interfaces.IVisitor visitor, object obj)
        {
            return visitor.Visit(this, obj);
        }
    }
}
