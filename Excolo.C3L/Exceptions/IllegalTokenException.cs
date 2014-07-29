using Excolo.C3L.SyntaxAnalysis.Scanning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Exceptions
{
    /// <summary>
    /// An exception throw when an illegal token was encountered during parsing.
    /// </summary>
    public class IllegalTokenException : ParserException
    {
        /// <summary>
        /// Get a list of expected tokens.
        /// </summary>
        public IEnumerable<TokenKind> Expected { get; private set; }

        /// <summary>
        /// Get the received token causing the exception.
        /// </summary>
        public Token Received { get; private set; }

        /// <summary>
        /// A constructor for the illegal token exception.
        /// </summary>
        /// <param name="received">The token encountered causing the exception.</param>
        /// <param name="expected">A parameterized list of expected tokens.</param>
        public IllegalTokenException(Token received, params TokenKind[] expected)
            : base()
        {
            Received = received;
            Expected = expected;
        }

        /// <summary>
        /// A constructor for the illegal token exception.
        /// </summary>
        /// <param name="received">The token encountered causing the exception.</param>
        /// <param name="message">A message describing the exception</param>
        /// <param name="expected">A parameterized list of expected tokens.</param>
        public IllegalTokenException(Token received, string message, params TokenKind[] expected)
            : base(message)
        {
            Received = received;
            Expected = expected;
        }

        /// <summary>
        /// A constructor for the illegal token exception.
        /// </summary>
        /// <param name="received">The token encountered causing the exception.</param>
        /// <param name="message">A message describing the exception</param>
        /// <param name="innerException">An inner exception associated with this exception.</param>
        /// <param name="expected">A parameterized list of expected tokens.</param>
        public IllegalTokenException(Token received, string message, Exception innerException,
                                     params TokenKind[] expected)
            : base(message, innerException)
        {
            Received = received;
            Expected = expected;
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
                string result = base.Message;
                if(Expected!= null && Expected.Count() > 0)
                {
                    result += " Expected Token of kind: ";
                    result += Expected.First().ToString();
                    foreach (var exp in Expected.Skip(1))
                    {
                        result += " or " + exp.ToString();
                    }
                    result += ". ";
                }

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
