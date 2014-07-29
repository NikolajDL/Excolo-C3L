using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Exceptions
{
    /// <summary>
    /// An exception throw when something went wrong during lexical scanning in the syntactical analysis.
    /// </summary>
    public class ScannerException : ShellException
    {
        /// <summary>
        /// A constructor for the scanner exception.
        /// </summary>
        /// <param name="position">The position of the character causing the exception.</param>
        public ScannerException(int position)
            : base(position)
        {
        }

        /// <summary>
        /// A constructor for the scanner exception.
        /// </summary>
        /// <param name="position">The position of the character causing the exception.</param>
        /// <param name="message">The message describing the exception.</param>
        public ScannerException(int position, string message)
            : base(message, position)
        {
        }

        /// <summary>
        /// A constructor for the scanner exception.
        /// </summary>
        /// <param name="position">The position of the character causing the exception.</param>
        /// <param name="message">The message describing the exception.</param>
        /// <param name="innerException">The inner exception associated with this exception.</param>
        public ScannerException(int position, string message, Exception innerException)
            : base(message, innerException, position)
        {
        }

        /// <summary>
        /// A constructor for the scanner exception.
        /// The position property is set to -1 by default.
        /// </summary>
        public ScannerException()
            : base(-1)
        {
        }

        /// <summary>
        /// A constructor for the scanner exception.
        /// The position property is set to -1 by default.
        /// </summary>
        /// <param name="message">The message describing the exception.</param>
        public ScannerException(string message)
            : base(message, -1)
        {
        }

        /// <summary>
        /// A constructor for the scanner exception.
        /// The position property is set to -1 by default.
        /// </summary>
        /// <param name="message">The message describing the exception.</param>
        /// <param name="innerException">The inner exception associated with this exception.</param>
        public ScannerException(string message, Exception innerException)
            : base(message, innerException, -1)
        {
        }

        /// <summary>
        /// Get the message describing the scanner exception.
        /// </summary>
        public override string Message
        {
            get
            {
                return "Syntactic Error: " + base.Message;
            }
        }
    }
}
