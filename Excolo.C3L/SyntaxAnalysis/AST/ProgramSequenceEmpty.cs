using Excolo.C3L.Exceptions;
using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.SyntaxAnalysis.AST
{
    /// <summary>
    /// A class defining an empty sequence of commands, 
    /// meaning there are no more commands in the program.
    /// </summary>
    public class ProgramSequenceEmpty : BaseAST, IProgramSequence
    {
        /// <summary>
        /// Get the commands in this program sequence - which in this case will always be empty.
        /// </summary>
        public IEnumerable<ICommand> Commands
        {
            get
            {
                return new List<Command>();
            }
        }

        /// <summary>
        /// Get a list of any error found during parsing of this AST.
        /// </summary>
        public IEnumerable<ShellException> Errors { get; set; }

        /// <summary>
        /// A constructor for the command sequence.
        /// </summary>
        /// <param name="position">The position of the sequence.</param>
        public ProgramSequenceEmpty(int position)
            : base(position) { }

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
