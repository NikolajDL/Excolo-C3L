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
    /// A class defining the program sequence in the AST, which is a sequence of commands.
    /// </summary>
    public class ProgramSequence : BaseAST, IProgramSequence
    {
        /// <summary>
        /// Get the first command of the program sequence.
        /// </summary>
        public ICommand Command { get; private set; }

        /// <summary>
        /// Get the rest of the program sequence as a sub-sequence.
        /// </summary>
        public IProgramSequence FollowingCommands { get; private set; }

        /// <summary>
        /// Get all commands of this program sequence.
        /// </summary>
        public virtual IEnumerable<ICommand> Commands
        {
            get
            {
                yield return Command;
                foreach (var com in FollowingCommands.Commands)
                {
                    yield return com;
                }
            }
        }

        /// <summary>
        /// Get a list of any error found during parsing of this AST.
        /// </summary>
        public IEnumerable<ShellException> Errors { get; set; }

        /// <summary>
        /// Get the position of the program sequence in the parsed string. 
        /// If this is the root program, the position will be '1'.
        /// </summary>
        public override int Position
        {
            get
            {
                return Command.Position;
            }
        }
        
        /// <summary>
        /// A constructor for the program sequence.
        /// </summary>
        /// <param name="command">The first command of the program sequence.</param>
        /// <param name="followingCommands">The rest of the program sequence, as a sub-sequence.</param>
        public ProgramSequence(ICommand command, IProgramSequence followingCommands = null)
        {
            Command = command;
            FollowingCommands = followingCommands;
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
