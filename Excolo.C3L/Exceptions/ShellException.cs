using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Exceptions
{
    /// <summary>
    /// An abstract exception class throw when errors occur in the C3L shell.
    /// </summary>
    public abstract class ShellException : Exception
    {
        /// <summary>
        /// Get the position of the shell command causing the exception.
        /// </summary>
        public virtual int Position { get; private set; }

        /// <summary>
        /// A constructor for the ShellException.
        /// </summary>
        /// <param name="position">The position of the command causing the exception. Default is -1.</param>
        public ShellException(int position = -1)
            : base()
        {
            Position = position;
        }

        /// <summary>
        /// A constructor for the ShellException.
        /// </summary>
        /// <param name="message">The error message associated with this exception.</param>
        /// <param name="position">The position of the command causing the exception. Default is -1.</param>
        public ShellException(string message, int position = -1)
            : base(message)
        {
            Position = position;
        }

        /// <summary>
        /// A constructor for the ShellException.
        /// </summary>
        /// <param name="message">The error message associated with this exception.</param>
        /// <param name="innerException">The inner exception of this exception.</param>
        /// <param name="position">The position of the command causing the exception. Default is -1.</param>
        public ShellException(string message, Exception innerException, int position = -1)
            : base(message, innerException)
        {
            Position = position;
        }

        /// <summary>
        /// Get a message describing the exception.
        /// </summary>
        public override string Message
        {
            get
            {
                return base.Message + (Position < 0 ? "" : " [pos: " + Position + "]    ");
            }
        }
    }
}
