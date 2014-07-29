using Excolo.C3L.SyntaxAnalysis.Scanning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Exceptions
{
    /// <summary>
    /// An exception thrown when encountering an expected token, but the token is malformed.
    /// This exception is usually an indication of errors in the scanner.
    /// </summary>
    public class MalformedTokenException : ParserException
    {

        /// <summary>
        /// Get the received token causing the exception.
        /// </summary>
        public Token Received { get; private set; }
        
        /// <summary>
        /// A constructor for the illegal token exception.
        /// </summary>
        /// <param name="received">The token encountered causing the exception.</param>
        public MalformedTokenException(Token received)
            : base()
        {
            Received = received;
        }

        /// <summary>
        /// A constructor for the MalformedTokenException.
        /// </summary>
        /// <param name="received">The token encountered causing the exception.</param>
        /// <param name="message">A message describing the exception</param>
        public MalformedTokenException(Token received, string message)
            : base(message)
        {
            Received = received;
        }

        /// <summary>
        /// A constructor for the MalformedTokenException.
        /// </summary>
        /// <param name="received">The token encountered causing the exception.</param>
        /// <param name="message">A message describing the exception</param>
        /// <param name="innerException">An inner exception associated with this exception.</param>
        public MalformedTokenException(Token received, string message, Exception innerException)
            : base(message, innerException)
        {
            Received = received;
        }

        /// <summary>
        /// Get the position of the encountered token.
        /// </summary>
        public override int Position
        {
            get
            {
                return Received.Position;
            }
        }

        /// <summary>
        /// Get the message describing the exception.
        /// </summary>
        public override string Message
        {
            get
            {
                var result = base.ToString();

                string receivedString = "'" + Received.ToString() + "'";
                if (Received.Kind == TokenKind.EOF)
                {
                    receivedString = "EOF token";
                }
                result += "Received: " + receivedString;

                return result;
            }
        }
    }
}
