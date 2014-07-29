using Excolo.C3L.Interfaces;
using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Executer
{
    /// <summary>
    /// A delegate type defining a Command method, used to execute a command in a virtual machine.
    /// </summary>
    /// <param name="arguments">The arguments passed to the command method.</param>
    /// <returns>The arguments returned by the command.</returns>
    public delegate IEnumerable<IArgument> Command(ArgumentValueLookup arguments);

    /// <summary>
    /// A static class used to resolve command delegate methods based on the parsed commands
    /// and available environments.
    /// </summary>
    public static class Resolver
    {
        /// <summary>
        /// A method to resolve a command and get the commandmatching result from an environment.
        /// </summary>
        /// <param name="command">The AST command to resolve.</param>
        /// <param name="environments">The environments to search for the command.</param>
        /// <returns>Null if the command doesn't reside in any of the environments.</returns>
        public static CommandMatchResult Resolve(ICommand command, IEnumerable<IEnvironment> environments)
        {
            CommandMatchResult result = null;

            // Search for commandname in all given environments 
            foreach (var env in environments)
            {
                result = env.Match(command.CommandName, command.Arguments);
                if (result != null)
                    break;
            }

            return result;
        }

        /// <summary>
        /// A method to retrieve the environment that contains the given command.
        /// </summary>
        /// <param name="command">The AST command to search for in the environment.</param>
        /// <param name="environments">The environments to search for the command.</param>
        /// <returns>Null if none of the environments contains the given command.</returns>
        public static IEnvironment GetEnvironmentOf(ICommand command, IEnumerable<IEnvironment> environments)
        {
            // Search for commandname in all given environments 
            foreach (var env in environments)
            {
                if (env.ContainsCommand(command.CommandName))
                    return env;
            }

            // None found - return null;
            return null;
        }
    }
}
