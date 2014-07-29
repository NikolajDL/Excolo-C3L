using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Interfaces.AST
{
    /// <summary>
    /// An interface defining the ArgumentSequence sub-tree of the C3L AST.
    /// </summary>
    public interface IArgumentSequence
    {
        /// <summary>
        /// Get the arguments in this argument sequence. 
        /// </summary>
        IEnumerable<IArgument> Arguments { get; }
    }
}
