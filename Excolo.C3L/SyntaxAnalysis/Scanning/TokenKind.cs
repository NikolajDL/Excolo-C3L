using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.SyntaxAnalysis.Scanning
{
    /// <summary>
    /// An enum describing the different possible token kinds.
    /// </summary>
    public enum TokenKind
    {
        WORD,
        COMMAND_SEPARATOR,
        ARGUMENT_SEPARATOR,
        REAL,
        INTEGER,
        STRING,
        TRUE,
        FALSE,
        EOF
    }
}
