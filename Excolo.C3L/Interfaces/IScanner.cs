using Excolo.C3L.SyntaxAnalysis.Scanning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Interfaces
{
    /// <summary>
    /// An interface defining the operations of a lexical scanner for Excolo C3L.
    /// </summary>
    public interface IScanner
    {
        /// <summary>
        /// Get the full string this scanner is scanning.
        /// </summary>
        string WorkingString { get; }

        /// <summary>
        /// A method to return a two-line string showing where in the input an error occured.
        /// </summary>
        /// <param name="position">The position of the error starting at 1</param>
        /// <returns></returns>
        string GetErrorPositionString(int position, int? consoleWidth = null);

        /// <summary>
        /// A method to scan for the next token in the string giving during initialization.
        /// </summary>
        Token Scan();
    }
}
