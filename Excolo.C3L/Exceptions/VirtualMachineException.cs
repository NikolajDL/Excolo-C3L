using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Exceptions
{
    /// <summary>
    /// An exception thrown when something goes awry in a virtual machine.
    /// </summary>
    public class VirtualMachineException : ShellException
    {
        /// <summary>
        /// A constructor for the VirtualMachineException
        /// </summary>
        public VirtualMachineException()
            : base()
        {
        }

        /// <summary>
        /// A constructor for the VirtualMachineException
        /// </summary>
        /// <param name="message">A message describing the error</param>
        public VirtualMachineException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// A constructor for the VirtualMachineException
        /// </summary>
        /// <param name="message">A message describing the error</param>
        /// <param name="innerException">An inner exception associated to this exception.</param>
        public VirtualMachineException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// A constructor for the VirtualMachineException
        /// </summary>
        /// <param name="position">The position of the cause of the error.</param>
        public VirtualMachineException(int position)
            : base(position)
        {
        }

        /// <summary>
        /// A constructor for the VirtualMachineException
        /// </summary>
        /// <param name="position">The position of the cause of the error.</param>
        /// <param name="message">A message describing the error</param>
        public VirtualMachineException(int position, string message)
            : base(message, position)
        {
        }

        /// <summary>
        /// A constructor for the VirtualMachineException
        /// </summary>
        /// <param name="position">The position of the cause of the error.</param>
        /// <param name="message">A message describing the error</param>
        /// <param name="innerException">An inner exception associated to this exception.</param>
        public VirtualMachineException(int position, string message, Exception innerException)
            : base(message, innerException, position)
        {
        }

        /// <summary>
        /// Get a message describing the error.
        /// </summary>
        public override string Message
        {
            get
            {
                return "Contextual Error: " + base.Message;
            }
        }
    }
}
