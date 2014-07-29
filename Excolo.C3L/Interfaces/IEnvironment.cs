using Excolo.C3L.Executer;
using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Interfaces
{
    /// <summary>
    /// An interface to define an environment which can be associated with a virtual machine.
    /// The environment encapsulates commands and their actions. 
    /// </summary>
    public interface IEnvironment
    {
        /// <summary>
        /// Get the name of the environment.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Get whether unbinding is allowed by this environment. 
        /// </summary>
        bool AllowUnbinding { get; }

        /// <summary>
        /// Get a list of command names.
        /// </summary>
        IEnumerable<string> Commands { get; }

        /// <summary>
        /// A method to bind a command delegate to the given command/argument properties.
        /// </summary>
        /// <param name="comm">The delegate method that makes up the command.</param>
        /// <param name="commandTemplate">
        /// A string template describing the structure of the command.
        /// 
        ///     <example>
        ///     "sum a b"               // 'sum' is the commandname, and 'a' and 'b' are two arguments
        ///     "log type message*"     // 'log' is the commandname, 'type' is an argument and 'message*' 
        ///                             // is a catch-all argument, that will contain all 
        ///                             // of the remaining arguments (if any).
        ///     </example>
        /// </param>
        /// <param name="defaults">An anonymous object defining argument default values. 
        /// Each anonymous propertyname should correspond to an argument in the commandTemplate.
        /// By defining a default value, the given argument will be optional.</param>
        /// <param name="aliases">An anonymous object defining a set of allowed aliases for each argument.
        /// Each anonymous propertyname should correspond to an argument in the commandTemplate.
        /// When matching arguments to the binding, the aliases can be used in place of 
        /// the primary argument names.</param>
        void Bind(Command command, string commandTemplate, object defaults, object aliases);



        /// <summary>
        /// A method to bind a command delegate to the given command/argument properties.
        /// </summary>
        /// <param name="comm">The delegate method that makes up the command.</param>
        /// <param name="commandTemplate">
        /// A string template describing the structure of the command.
        /// 
        ///     <example>
        ///     "sum a b"               // 'sum' is the commandname, and 'a' and 'b' are two arguments
        ///     "log type message*"     // 'log' is the commandname, 'type' is an argument and 'message*' 
        ///                             // is a catch-all argument, that will contain all 
        ///                             // of the remaining arguments (if any).
        ///     </example>
        /// </param>
        /// <param name="defaults">A dictionary defining argument default values. 
        /// Each dictionary key should correspond to an argument in the commandTemplate.
        /// By defining a default value, the given argument will be optional.</param>
        /// <param name="aliases">A dictionary defining defining a set of allowed aliases for each argument.
        /// Each dictionary key should correspond to an argument in the commandTemplate.
        /// When matching arguments to the binding, the aliases can be used in place of 
        /// the primary argument names.</param>
        void Bind(Command comm, string commandTemplate, ArgumentValueDictionary defaults,
            ArgumentValueDictionary<string[]> aliases);

        /// <summary>
        /// A method to unbind a command given the name of the command. 
        /// </summary>
        /// <param name="commandName">The name of the command to unbind from this environment.</param>
        void UnBind(string commandName);

        /// <summary>
        /// A method to match a given command and its arguments to a command in the bindtable.
        /// Returns the found command delegate and the arguments mapped.
        /// </summary>
        /// <param name="commandName">The name of the command to match.</param>
        /// <param name="args">The list of arguments passed to the command.</param>
        /// <returns>A <see cref="CommandMatchResult"/> object, containing the command delegate,
        /// and the mapped arguments.</returns>
        CommandMatchResult Match(string commandName, IEnumerable<IArgument> args);

        /// <summary>
        /// A method to check whether the environment contains a given command.
        /// </summary>
        /// <param name="commandName">The name of the command to search for.</param>
        /// <returns>Returns true if the command name is bound to this environment.</returns>
        bool ContainsCommand(string commandName);
    }
}
