using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Interfaces
{
    /// <summary>
    /// An interface defining the operations of a the Excolo C3L shell parser.
    /// </summary>
    public interface IShellParser
    {
        /// <summary>
        /// A method to parse the given input string and return an AST accordingly.
        /// </summary>
        /// <param name="input">The input to parse.</param>
        /// <returns>An abstract syntax tree (AST) representing the parsed input.</returns>
        IProgramSequence Parse(string input);
    }
}
