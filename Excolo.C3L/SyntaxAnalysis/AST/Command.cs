using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.SyntaxAnalysis.AST
{
    /// <summary>
    /// A class defining the command in the AST.
    /// </summary>
    public class Command : BaseAST, ICommand
    {

        private IArgumentSequence _argumentSequence;

        /// <summary>
        /// Get the name of the command.
        /// </summary>
        public string CommandName { get; private set; }

        /// <summary>
        /// Get the arguments of the command.
        /// </summary>
        public IEnumerable<IArgument> Arguments
        {
            get
            {
                if (_argumentSequence != null && _argumentSequence.Arguments != null)
                    foreach (IArgument arg in _argumentSequence.Arguments)
                    {
                        yield return arg;
                    }
            }
        }
        
        /// <summary>
        /// A constructor for the command.
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="arguments">The arguments of the command.</param>
        public Command(string commandName, IArgumentSequence arguments)
        {
            CommandName = commandName;
            _argumentSequence = arguments;
        }

        /// <summary>
        /// A constructor for the command.
        /// </summary>
        /// <param name="position">The position of the command.</param>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="arguments">The arguments of the command.</param>
        public Command(int position, string commandName, IArgumentSequence arguments) 
            : base(position)
        {
            CommandName = commandName;
            _argumentSequence = arguments;
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
