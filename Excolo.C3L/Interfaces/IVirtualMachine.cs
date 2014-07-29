using Excolo.C3L.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Interfaces
{
    /// <summary>
    /// A delegate method called for handling errors.
    /// </summary>
    /// <param name="e"></param>
    public delegate void ErrorHandler(ShellException e);

    /// <summary>
    /// An interface to represent a virtual machine used to parse and execute commands
    /// given to the Excolo CommandLine Language Library. 
    /// The environments associated with this virtual machine, defines the possible commands
    /// and their actions.
    /// </summary>
    public interface IVirtualMachine
    {

        /// <summary>
        /// Get or set the stream the output of the virtual machine is written to.
        /// </summary>
        StreamWriter Output { get; set; }

        /// <summary>
        /// Get or set the stream any logging messages of the virtual machine is written to.
        /// </summary>
        StreamWriter Log { get; set; }

        /// <summary>
        /// Get or set the error handler for the virtual machine.
        /// </summary>
        ErrorHandler HandleError { get; set; }

        /// <summary>
        /// Get or set the parser used to parse the program.
        /// </summary>
        IShellParser Parser { get; set; }

        /// <summary>
        /// Get the environments associated with this virtual machine.
        /// </summary>
        IEnumerable<IEnvironment> Environments { get; }

        /// <summary>
        /// A method to execute the given command in this virtual machine.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        void Execute(string command);

        /// <summary>
        /// A method to load an environment into the virtual machine.
        /// </summary>
        /// <param name="environment">The environment to load.</param>
        void LoadEnvironment(IEnvironment environment);

        /// <summary>
        /// A method to unload an environment associated with the virtual machine.
        /// </summary>
        /// <param name="environment">The environment to unload.</param>
        void UnloadEnvironment(IEnvironment environment);
    }
}
