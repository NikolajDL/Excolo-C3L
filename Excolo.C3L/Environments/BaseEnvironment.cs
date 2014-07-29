using Excolo.C3L.Core;
using Excolo.C3L.Exceptions;
using Excolo.C3L.Executer;
using Excolo.C3L.Interfaces;
using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Environments
{

    /// <summary>
    /// A base environment, implementing some of the basic required properties 
    /// and methods in <see cref="IEnvironment"/>.
    /// </summary>
    public abstract class BaseEnvironment : IEnvironment
    {
        private IDictionary<string, CommandBindData> _boundCommands = new Dictionary<string, CommandBindData>();
        private IDictionary<string, string[]> _commandNameAliases = new Dictionary<string, string[]>();


        /// <summary>
        /// Get the name of the environment.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Get whether unbinding is allowed by this environment. 
        /// </summary>
        public bool AllowUnbinding { get; protected set; }

        /// <summary>
        /// Get a list of command names.
        /// </summary>
        public IEnumerable<string> Commands
        {
            get
            {
                return _commandNameAliases.Keys.AsEnumerable();
            }
        }

        /// <summary>
        /// A constructor for the BaseEnvironment.
        /// </summary>
        /// <param name="allowUnbinding">Set whether it is possible to unbind commands from this environment. 
        /// False by default.</param>
        protected BaseEnvironment(bool allowUnbinding = false)
        {
            AllowUnbinding = allowUnbinding;

        }


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
        public void Bind(Command comm, string commandTemplate, object defaults = null, object aliases = null)
        {
            Bind(comm, commandTemplate, new ArgumentValueDictionary(defaults),
                new ArgumentValueDictionary<string[]>(aliases));
        }

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
        public void Bind(Command command, string commandTemplate, ArgumentValueDictionary defaults,
            ArgumentValueDictionary<string[]> aliases)
        {
            var segments = Parse(commandTemplate);
            var commandName = segments.First().Name;
            string[] commandAliases = new string[0];
            aliases.TryGetValue(commandName, out commandAliases);

            // Bind each command name.
            _boundCommands.Add(commandName, new CommandBindData(command, segments.Where(x => x != segments.First()), defaults, aliases));
            if (commandAliases != null)
                foreach (var commName in commandAliases)
                {
                    _boundCommands.Add(commName, new CommandBindData(command, segments.Where(x => x != segments.First()), defaults, aliases));
                }

            // Add command to commandname-aliase mapping
            _commandNameAliases.Add(commandName, commandAliases);
        }


        /// <summary>
        /// A method to unbind a command given the name of the command. 
        /// </summary>
        /// <param name="commandName">The name of the command to unbind from this environment.</param>
        public virtual void UnBind(string commandName)
        {
            if (!AllowUnbinding)
                throw new VirtualMachineException("This environment (" + Name + ") does not support unbinding of commands.");

            string[] aliases = new string[0];
            _commandNameAliases.TryGetValue(commandName, out aliases);

            _boundCommands.Remove(commandName);
            if (aliases != null)
                foreach (var alias in aliases)
                    _boundCommands.Remove(alias);

            _commandNameAliases.Remove(commandName);
        }

        /// <summary>
        /// A method to check whether the environment contains a given command.
        /// </summary>
        /// <param name="commandName">The name of the command to search for.</param>
        /// <returns>Returns true if the command name is bound to this environment.</returns>
        public bool ContainsCommand(string commandName)
        {
            return _boundCommands.ContainsKey(commandName);
        }

        /// <summary>
        /// A method to match a given command and its arguments to a command in the bindtable.
        /// Returns the found command delegate and the arguments mapped.
        /// </summary>
        /// <param name="commandName">The name of the command to match.</param>
        /// <param name="args">The list of arguments passed to the command.</param>
        /// <returns>A <see cref="CommandMatchResult"/> object, containing the command delegate,
        /// and the mapped arguments. 
        /// If command doesn't exist, it returns null.</returns>
        public CommandMatchResult Match(string commandName, IEnumerable<IArgument> args)
        {
            // Find binding by commandname - return null if not found.
            CommandBindData binding = null;
            if (!_boundCommands.TryGetValue(commandName, out binding))
                return null;

            // Map arguments
            var argumentLookup = MapArguments(args, binding.CommandTemplate.ToArray(),
                binding.Defaults, binding.Aliases);

            return new CommandMatchResult(binding.Command, argumentLookup);
        }


        private ArgumentTemplateSegment[] Parse(string commandTemplate)
        {
            // Split the template string - it's whitespace separated. 
            var args = commandTemplate.Split(' ').Select(x => new ArgumentTemplateSegment(x.Trim()));

            // Check for at least one argument
            if (!args.Any())
                throw new Excolo.C3L.Exceptions.ArgumentException(
                    "The commandTemplate must contain at least a commandname.");

            // Check for duplicate names
            var dup = args.GroupBy(n => n.Name).FirstOrDefault(c => c.Count() > 1);
            if (dup != null)
                throw new Excolo.C3L.Exceptions.ArgumentException(
                    "Duplicate keys in the commandTemplate is not allowed. The duplicate found is '" +
                    dup.First().Name + "'.");

            // Check for too many catch-all attributes.
            if (args.Count(x => x.IsCatchAll) > 1)
                throw new Excolo.C3L.Exceptions.ArgumentException(
                    "Only one segment of the commandTemplate may be catch-all.");

            // Check that the catch-all attribute is the last.
            if (args.Any(x => x.IsCatchAll)
                && args.First().Name == args.SingleOrDefault(x => x.IsCatchAll).Name)
                throw new Excolo.C3L.Exceptions.ArgumentException(
                    "The first element is the commandname and cannot be a catch-all segment.");

            // Check that the catch-all attribute is the last.
            if (args.Any(x => x.IsCatchAll)
                && args.Last().Name != args.SingleOrDefault(x => x.IsCatchAll).Name)
                throw new Excolo.C3L.Exceptions.ArgumentException(
                    "The catch-all segment of the commandTemplate must be the last.");

            // return as array.
            return args.ToArray();
        }

        private ArgumentValueLookup MapArguments(
            IEnumerable<IArgument> args,
            ArgumentTemplateSegment[] templateSegments,
            ArgumentValueDictionary defaults,
            ArgumentValueDictionary<string[]> aliases)
        {
            var arguments = args.ToList();

            var result = new List<KeyValuePair<string, IArgument>>();
            var missingSegments = new List<ArgumentTemplateSegment>();

            // Run through each template segment
            for (int i = 0; i < templateSegments.Count(); i++)
            {
                var segment = templateSegments[i];

                // If catch-all 
                if (segment.IsCatchAll)
                {
                    var catchAllArgs = arguments.Select(
                        x => new KeyValuePair<string, IArgument>(segment.Name, x));
                    result.AddRange(catchAllArgs);
                    arguments.Clear();
                    break;
                }

                // Get list with all aliases.
                string[] segmentAliases = null;
                if (aliases != null)
                    aliases.TryGetValue(segment.Name, out segmentAliases);

                // Check for argumentname - if it exists add it.
                var arg = arguments.SingleOrDefault(x => x.ArgumentName == segment.Name
                        || (segmentAliases != null && segmentAliases.Contains(x.ArgumentName)));
                if (arg != null)
                {
                    result.Add(new KeyValuePair<string, IArgument>(segment.Name, arg));
                    arguments.Remove(arg);
                }
                // If it doesn't exist, check for any empty arguments and add them.
                // If one doesn't exist, use the default value or throw an exception.
                else
                {
                    var missingArg = arguments.FirstOrDefault(x => String.IsNullOrEmpty(x.ArgumentName));
                    if (missingArg != null)
                    {
                        result.Add(new KeyValuePair<string, IArgument>(segment.Name, missingArg));
                        arguments.Remove(missingArg);
                    }
                    else
                    {
                        // Try to get default - if not possible, throw exception.
                        object defaultValue = null;
                        if (defaults != null && defaults.TryGetValue(segment.Name, out defaultValue))
                            result.Add(new KeyValuePair<string, IArgument>(segment.Name,
                                new ValueArgument(defaultValue, defaultValue.GetType())));
                        else
                            throw new Excolo.C3L.Exceptions.ArgumentException(
                                "Too few arguments. Expected the argument with name '" + segment.Name + "'.");
                    }
                }
            }

            // Check if every argument was processed. If not throw an exception.
            if (arguments.Any())
                throw new Excolo.C3L.Exceptions.ArgumentException(
                    "Too many arguments. Didn't excpect arguments: " + String.Join(", ", arguments.Select(x => x.Value)) + ".");

            return new ArgumentValueLookup(result);
        }
    }
}
