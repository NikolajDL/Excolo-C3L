using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Exceptions
{
    /// <summary>
    /// An exception throw when something is wrong with the command arguments.
    /// </summary>
    public class ArgumentException : ShellException
    {
        /// <summary>
        /// Get the argument that caused the exception.
        /// </summary>
        public IArgument Argument { get; private set; }

        /// <summary>
        /// Get the position of the argument that caused the error.
        /// </summary>
        public override int Position
        {
            get
            {
                if (Argument != null)
                    return Argument.Position;
                return base.Position;
            }
        }

        /// <summary>
        /// A constructor for the argument exception.
        /// </summary>
        /// <param name="argument">The argument cauring the exception.</param>
        public ArgumentException(IArgument argument)
            : base()
        {
            Argument = argument;
        }

        /// <summary>
        /// A constructor for the argument exception.
        /// </summary>
        /// <param name="argument">The argument cauring the exception.</param>
        /// <param name="message">A message describing the exception.</param>
        public ArgumentException(IArgument argument, string message)
            : base(message)
        {
            Argument = argument;
        }

        /// <summary>
        /// A constructor for the argument exception.
        /// </summary>
        /// <param name="message">A message describing the exception.</param>
        public ArgumentException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// A constructor for the argument exception.
        /// </summary>
        /// <param name="message">A message describing the exception.</param>
        /// <param name="innerException">An inner exception associated with this exception.</param>
        public ArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// A constructor for the argument exception.
        /// </summary>
        /// <param name="argument">The argument cauring the exception.</param>
        /// <param name="message">A message describing the exception.</param>
        /// <param name="innerException">An inner exception associated with this exception.</param>
        public ArgumentException(IArgument argument, string message, Exception innerException)
            : base(message, innerException)
        {
            Argument = argument;
        }

    }
}
