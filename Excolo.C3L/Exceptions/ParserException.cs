using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Exceptions
{
    /// <summary>
    /// An exception throw when something went wrong during parsing of the syntactical analysis.
    /// </summary>
    public class ParserException : ShellException
    {
        /// <summary>
        /// A constructor for the parser exception.
        /// </summary>
        public ParserException()
            : base(-1)
        { }

        /// <summary>
        /// A constructor for the parser exception.
        /// </summary>
        /// <param name="message">A message describing the exception.</param>
        public ParserException(string message)
            : base(message)
        { }

        /// <summary>
        /// A constructor for the parser exception.
        /// </summary>
        /// <param name="message">A message describing the exception.</param>
        /// <param name="innerException">An inner exception associated with this exception</param>
        public ParserException(string message, Exception innerException)
            : base(message, innerException)
        { }


        /// <summary>
        /// A constructor for the parser exception.
        /// </summary>
        /// <param name="position">The position of the token that caused the exception.</param>
        public ParserException(int position)
            : base(position)
        {
        }

        /// <summary>
        /// A constructor for the parser exception.
        /// </summary>
        /// <param name="position">The position of the token that caused the exception.</param>
        /// <param name="message">A message describing the exception.</param>
        public ParserException(int position, string message)
            : base(message, position)
        {
        }

        /// <summary>
        /// A constructor for the parser exception.
        /// </summary>
        /// <param name="position">The position of the token that caused the exception.</param>
        /// <param name="message">A message describing the exception.</param>
        /// <param name="innerException">An inner exception associated with this exception</param>
        public ParserException(int position, string message, Exception innerException)
            : base(message, innerException, position)
        {
        }

        /// <summary>
        /// Get the message describing the exception.
        /// </summary>
        public override string Message
        {
            get
            {
                string innerMes = "";
                if (InnerException != null && InnerException.Message != null && InnerException.Message != "")
                {
                    innerMes = " Inner exception message: " + InnerException.Message;
                }
                return "Syntax Error: " + base.Message + innerMes;
            }
        }
    }
}
