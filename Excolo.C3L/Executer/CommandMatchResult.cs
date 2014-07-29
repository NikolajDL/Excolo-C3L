using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Executer
{
    /// <summary>
    /// A class containing the result after matching a set of arguments to a given command.
    /// </summary>
    public class CommandMatchResult
    {
        /// <summary>
        /// Get the matched command.
        /// </summary>
        public Command Command { get; private set; }

        /// <summary>
        /// Get the table of arguments.
        /// </summary>
        public ArgumentValueLookup Arguments { get; private set; }

        /// <summary>
        /// A constructor for the command matching result.
        /// </summary>
        /// <param name="command">The matched commands.</param>
        /// <param name="arguments">The mapped arguments, passed to the command when being resolved.</param>
        public CommandMatchResult(Command command, ArgumentValueLookup arguments)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (arguments == null)
                throw new ArgumentNullException("arguments");

            this.Command = command;
            Arguments = arguments;
        }

        /// <summary>
        /// A helper method to execute this command matching result.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IArgument> ExecuteCommand()
        {
            return this.Command(Arguments);
        }
    }
}
