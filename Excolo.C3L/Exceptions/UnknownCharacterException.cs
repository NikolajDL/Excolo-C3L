using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Exceptions
{
    /// <summary>
    /// An exception thrown when an unknown character was encountered during syntax analysis.
    /// </summary>
    public class UnknownCharacterException : ScannerException
    {
        /// <summary>
        /// Get the received/actual character encountered.
        /// </summary>
        public char Received { get; private set; }

        /// <summary>
        /// A constructor for the UnknownCharacterException.
        /// </summary>
        /// <param name="received">The received/actual character.</param>
        public UnknownCharacterException(char received)
            : base()
        {
            Received = received;
        }

        /// <summary>
        /// A constructor for the UnknownCharacterException.
        /// </summary>
        /// <param name="received">The received/actual character.</param>
        /// <param name="message">The exception message describing the exception.</param>
        public UnknownCharacterException(char received, string message)
            : base(message)
        {
            Received = received;
        }

        /// <summary>
        /// A constructor for the UnknownCharacterException.
        /// </summary>
        /// <param name="received">The received/actual character.</param>
        /// <param name="message">The exception message describing the exception.</param>
        /// <param name="innerException">The inner exception associated with this exception.</param>
        public UnknownCharacterException(char received, string message, Exception innerException)
            : base(message, innerException)
        {
            Received = received;
        }



        /// <summary>
        /// A constructor for the UnknownCharacterException.
        /// </summary>
        /// <param name="position">The position of the unknown character</param>
        /// <param name="received">The received/actual character.</param>
        public UnknownCharacterException(int position, char received)
            : base(position)
        {
            Received = received;
        }
        /// <summary>
        /// A constructor for the UnknownCharacterException.
        /// </summary>
        /// <param name="position">The position of the unknown character</param>
        /// <param name="received">The received/actual character.</param>
        /// <param name="message">The exception message describing the exception.</param>
        public UnknownCharacterException(int position, char received, string message)
            : base(position, message)
        {
            Received = received;
        }
        /// <summary>
        /// A constructor for the UnknownCharacterException.
        /// </summary>
        /// <param name="position">The position of the unknown character</param>
        /// <param name="received">The received/actual character.</param>
        /// <param name="message">The exception message describing the exception.</param>
        /// <param name="innerException">The inner exception associated with this exception.</param>
        public UnknownCharacterException(int position, char received, string message, Exception innerException)
            : base(position, message, innerException)
        {
            Received = received;
        }

        /// <summary>
        /// Get the exception message of the UnknownCharacterException.
        /// </summary>
        public override string Message
        {
            get
            {
                string result = base.Message + " Unknown character received: " + Received;
                return result;
            }
        }
    }
}
