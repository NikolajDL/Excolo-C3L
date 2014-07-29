using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.SyntaxAnalysis.AST
{
    /// <summary>
    /// A class defining an argument sequence in the AST, 
    /// which is a sequence of arguments associated with a command.
    /// </summary>
    public class ArgumentSequence : BaseAST, IArgumentSequence
    {
        /// <summary>
        /// Get the first argument of the argument sequence.
        /// </summary>
        public IArgument Argument { get; private set; }

        /// <summary>
        /// Get the rest of the argument sequence, as a sub-sequence.
        /// </summary>
        public IArgumentSequence Args { get; private set; }


        /// <summary>
        /// Get the arguments in this argument sequence. 
        /// </summary>
        public IEnumerable<IArgument> Arguments
        {
            get
            {
                yield return Argument;
                foreach (var arg in Args.Arguments)
                {
                    yield return arg;
                }
            }
        }

        /// <summary>
        /// Get the position of this argument sequence in the parsed string. 
        /// </summary>
        public override int Position
        {
            get
            {
                return Argument.Position;
            }
        }

        /// <summary>
        /// A constructor for the argument sequence.
        /// </summary>
        /// <param name="argument">The first argument of the sequence.</param>
        /// <param name="argmumentSequence">All subsequent arguments of the sequence.</param>
        public ArgumentSequence(IArgument argument, IArgumentSequence argmumentSequence)
        {
            Argument = argument;
            Args = argmumentSequence;
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
