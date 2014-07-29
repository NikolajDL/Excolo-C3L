using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Exceptions
{
    /// <summary>
    /// An exception thrown when an unexpected character was encountered during syntax analysis.
    /// </summary>
    public class IllegalCharacterException : ScannerException
    {
        /// <summary>
        /// Get the expected character.
        /// </summary>
        public char Expected { get; private set; }

        /// <summary>
        /// Get the received/actual character.
        /// </summary>
        public char Received { get; private set; }

        /// <summary>
        /// A constructor for the IllegalCharacterException.
        /// </summary>
        /// <param name="expected">The expected character.</param>
        /// <param name="received">The received/actual character.</param>
        public IllegalCharacterException(char expected, char received)
            : base()
        {
            Expected = expected;
            Received = received;
        }

        /// <summary>
        /// A constructor for the IllegalCharacterException.
        /// </summary>
        /// <param name="expected">The expected character.</param>
        /// <param name="received">The received/actual character.</param>
        /// <param name="message">The exception message describing the exception.</param>
        public IllegalCharacterException(char expected, char received, string message)
            : base(message)
        {
            Expected = expected;
            Received = received;
        }

        /// <summary>
        /// A constructor for the IllegalCharacterException.
        /// </summary>
        /// <param name="expected">The expected character.</param>
        /// <param name="received">The received/actual character.</param>
        /// <param name="message">The exception message describing the exception.</param>
        /// <param name="innerException">The inner exception associated with this exception.</param>
        public IllegalCharacterException(char expected, char received, string message, Exception innerException)
            : base(message, innerException)
        {
            Expected = expected;
            Received = received;
        }


        /// <summary>
        /// A constructor for the IllegalCharacterException.
        /// </summary>
        /// <param name="position">The position of the character causing the exception.</param>
        /// <param name="expected">The expected character.</param>
        /// <param name="received">The received/actual character.</param>
        public IllegalCharacterException(int position, char expected, char received)
            : base(position)
        {
            Expected = expected;
            Received = received;
        }

        /// <summary>
        /// A constructor for the IllegalCharacterException.
        /// </summary>
        /// <param name="position">The position of the character causing the exception.</param>
        /// <param name="expected">The expected character.</param>
        /// <param name="received">The received/actual character.</param>
        /// <param name="message">The exception message describing the exception.</param>
        public IllegalCharacterException(int position, char expected, char received, string message)
            : base(position, message)
        {
            Expected = expected;
            Received = received;
        }
        /// <summary>
        /// A constructor for the IllegalCharacterException.
        /// </summary>
        /// <param name="position">The position of the character causing the exception.</param>
        /// <param name="expected">The expected character.</param>
        /// <param name="received">The received/actual character.</param>
        /// <param name="message">The exception message describing the exception.</param>
        /// <param name="innerException">The inner exception associated with this exception.</param>
        public IllegalCharacterException(int position, char expected, char received, string message, Exception innerException)
            : base(position, message, innerException)
        {
            Expected = expected;
            Received = received;
        }

        /// <summary>
        /// Get the exception message of the IllegalCharacterException.
        /// </summary>
        public override string Message
        {
            get
            {
                string result = base.Message + " Expected Token of kind: ";
                result += Expected;
                result += ". But received: " + Received;

                return result;
            }
        }
    }
}
