using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Exceptions
{
    /// <summary>
    /// An exception thrown when a given command isn't available for executing in the virtual machine
    /// with the given configuration.
    /// </summary>
    public class UnknownCommandException : ShellException
    {
        /// <summary>
        /// Get the command that caused the exception.
        /// </summary>
        public virtual ICommand Command { get; protected set; }

        /// <summary>
        /// A constructor for the UnknownCommandException
        /// </summary>
        /// <param name="command">The missing command that caused the exception.</param>
        public UnknownCommandException(ICommand command)
            : base()
        {
            Command = command;
        }

        /// <summary>
        /// A constructor for the UnknownCommandException
        /// </summary>
        /// <param name="command">The missing command that caused the exception.</param>
        /// <param name="message">A message describing the exception.</param>
        public UnknownCommandException(ICommand command, string message)
            : base(message)
        {
            Command = command;
        }

        /// <summary>
        /// A constructor for the UnknownCommandException
        /// </summary>
        /// <param name="command">The missing command that caused the exception.</param>
        /// <param name="message">A message describing the exception.</param>
        /// <param name="innerException">An inner exception associated with this exception.</param>
        public UnknownCommandException(ICommand command, string message, Exception innerException)
            : base(message, innerException)
        {
            Command = command;
        }

        /// <summary>
        /// Get the position in the parsed string, where the command that caused the exception resides.
        /// </summary>
        public override int Position
        {
            get
            {
                if (Command != null)
                    return Command.Position;
                else
                    return base.Position;
            }
        }

        /// <summary>
        /// Get a message describing the exception.
        /// </summary>
        public override string Message
        {
            get
            {
                var result = base.Message;

                if (Command != null)
                    result += "Could not find command with name '" + Command.CommandName + "'. \n";
                else
                    result += "The command was unknown. \n";

                return result;
            }
        }
    }
}
