using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Executer
{

    /// <summary>
    /// A class defining the binding between a command and its name and arguments.
    /// </summary>
    public class CommandBindData
    {
        /// <summary>
        /// Get the command which is bound.
        /// </summary>
        public Command Command { get; private set; }

        /// <summary>
        /// Get the parsed command template.
        /// </summary>
        public IEnumerable<ArgumentTemplateSegment> CommandTemplate { get; private set; }

        /// <summary>
        /// Get the default values of this command bind.
        /// </summary>
        public ArgumentValueDictionary Defaults { get; private set; }

        /// <summary>
        /// Get the aliases of the commandname and arguments in the commandtemplate.
        /// </summary>
        public ArgumentValueDictionary<string[]> Aliases { get; private set; }

        public CommandBindData(Command command, IEnumerable<ArgumentTemplateSegment> templateSegments,
            ArgumentValueDictionary defaults, ArgumentValueDictionary<string[]> aliases)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (templateSegments == null)
                throw new ArgumentNullException("templateSegments", 
                    "The collection of template segments cannot be null.");

            Command = command;
            CommandTemplate = templateSegments;
            Defaults = defaults;
            Aliases = aliases;
        }
    }
}
