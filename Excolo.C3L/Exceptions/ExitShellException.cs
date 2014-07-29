using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Exceptions
{
    /// <summary>
    /// An exception throw when exiting the shell.
    /// </summary>
    public class ExitShellException : ShellException
    {
        /// <summary>
        /// A constructor for ExitShellException.
        /// </summary>
        public ExitShellException() : base() { }

        /// <summary>
        /// A constructor for ExitShellException.
        /// </summary>
        /// <param name="message">The message associated with the exception.</param>
        public ExitShellException(string message) : base(message) { }

        /// <summary>
        /// A constructor for ExitShellException.
        /// </summary>
        /// <param name="message">The message describing the exception.</param>
        /// <param name="innerException">The innerexception associated with this exception.</param>
        public ExitShellException(string message, Exception innerException) : base(message, innerException) { }

    }
}
