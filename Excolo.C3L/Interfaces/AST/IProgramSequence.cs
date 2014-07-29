using Excolo.C3L.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Interfaces.AST
{
    /// <summary>
    /// An interface defining the ProgramSequence sub-tree of the Excolo C3L AST.
    /// </summary>
    public interface IProgramSequence
    {
        /// <summary>
        /// Get the commands in this program sequence. 
        /// </summary>
        IEnumerable<ICommand> Commands { get; }

        /// <summary>
        /// Get a list of any error found during parsing of this AST.
        /// </summary>
        IEnumerable<ShellException> Errors { get; }
    }
}
