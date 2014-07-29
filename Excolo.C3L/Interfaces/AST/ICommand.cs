using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Interfaces.AST
{
    /// <summary>
    /// An interface defining the command sub-tree of the C3L AST.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Get the name of the command.
        /// </summary>
        string CommandName { get; }

        /// <summary>
        /// Get the position of the command in the parsed string.
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Get the arguments of the command.
        /// </summary>
        IEnumerable<IArgument> Arguments { get; }
    }
}
