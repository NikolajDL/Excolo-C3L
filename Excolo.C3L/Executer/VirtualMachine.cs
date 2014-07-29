using Excolo.C3L.Core;
using Excolo.C3L.Environments;
using Excolo.C3L.Exceptions;
using Excolo.C3L.Interfaces;
using Excolo.C3L.Interfaces.AST;
using Excolo.C3L.SyntaxAnalysis.AST;
using Excolo.C3L.SyntaxAnalysis.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Executer
{
    /// <summary>
    /// A virtual machine class used as the default virtual machine. 
    /// This is meant to be sub-classes in each system it is system, but can also be used as it is.
    /// The default virtual machine already loads some common environments upon initialization.
    /// </summary>
    public class VirtualMachine : IVirtualMachine
    {
        #region Fields

        private IList<IEnvironment> _environments;
        private StreamWriter _output;
        private ErrorHandler _errorHandler;
        private static readonly string[] LockedEnvironments = { "system" };
        
        #endregion

        #region Properties

        /// <summary>
        /// Get or set the stream the output of the virtual machine is written to.
        /// Throws ArgumentNullException if value is null when setting the Output.
        /// </summary>
        public StreamWriter Output
        {
            get
            {
                return _output;
            }
            set
            {
                if (value == null) throw new ArgumentNullException("Output");
                _output = value;
            }
        }

        /// <summary>
        /// Get or set the stream any logging messages of the virtual machine is written to.
        /// </summary>
        public StreamWriter Log { get; set; }


        /// <summary>
        /// Get or set the error handler for the virtual machine.
        /// </summary>
        public ErrorHandler HandleError
        {
            get
            {
                if (_errorHandler == null)
                    _errorHandler = (ShellException e) => { throw e; };
                return _errorHandler;
            }
            set
            {
                _errorHandler = value;
            }
        }

        /// <summary>
        /// Get or set the parser used to parse the program.
        /// </summary>
        public IShellParser Parser { get; set; }

        /// <summary>
        /// Get the environments associated with this virtual machine.
        /// </summary>
        public IEnumerable<IEnvironment> Environments
        {
            get { return _environments; }
            protected set
            {
                if (value == null)
                    return;

                var conflicts = value.Where(x => value.Any(y => x != y && y.Name.ToLower() == x.Name.ToLower())).Select(x => x.Name);

                if (conflicts.Any())
                    throw new VirtualMachineException("Name conflict between environments on the names: " + String.Join(",", conflicts));

                _environments = value.Union(_environments.Where(x => LockedEnvironments.Any(y => x.Name.ToLower() == y.ToLower()))).ToList();
            }
        }
        
        #endregion

        /// <summary>
        /// A constructor for this virtual machine.
        /// </summary>
        /// <param name="output">The main output stream of the virtual machine.</param>
        /// <param name="environments">Any additional environments to load 
        /// into the virtual machine during initialization.</param>
        public VirtualMachine(StreamWriter output, params IEnvironment[] environments)
            : this(output, null, environments)
        { }

        /// <summary>
        /// A constructor for this virtual machine.
        /// </summary>
        /// <param name="output">The main output stream of the virtual machine.</param>
        /// <param name="log">The logging stream of the virtual machine.</param>
        /// <param name="environments">Any additional environments to load 
        /// into the virtual machine during initialization.</param>
        public VirtualMachine(StreamWriter output, StreamWriter log, params IEnvironment[] environments)
        {
            if (output == null)
                throw new ArgumentNullException("output");


            Output = output;
            Log = log ?? StreamWriter.Null;
            Parser = new Parser();
            _environments = new List<IEnvironment>(environments);

            // Add default environments.
            _environments.Add(new SystemEnvironment(this));
            _environments.Add(new IOEnvironment(this));
        }

        /// <summary>
        /// A method to execute the given command in this virtual machine.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        public virtual void Execute(string command)
        {
            try
            {
                InternExecute(command).ToArray();
            }
            catch (ExitShellException)
            {
                throw;
            }
            catch (ShellException e)
            {
                HandleError(e);
            }
            catch (Exception e)
            {
                HandleError(new VirtualMachineException("An unknown error occurred. Please visit the inner exception for details.", e));
            }
        }


        private IEnumerable<IArgument> InternExecute(string command)
        {

            var prog = Parser.Parse(command);
            if (prog.Errors == null || !prog.Errors.Any()) return InternExecute(prog);

            foreach (var error in prog.Errors)
            {
                HandleError(error);
            }
            return new IArgument[0];

        }

        private IEnumerable<IArgument> InternExecute(IProgramSequence progSeq)
        {
            IEnumerable<IArgument> result = new IArgument[0];

            foreach (ICommand comm in progSeq.Commands)
            {
                // Get command if exists.
                var match = Resolver.Resolve(comm, Environments);

                // Execute command and combine with other results.
                if (match != null){
                    result = result.Concat(match.ExecuteCommand());
                }
                else
                    throw new UnknownCommandException(comm);
            }

            return result;
        }

        /// <summary>
        /// A method to load an environment into the virtual machine.
        /// </summary>
        /// <param name="environment">The environment to load.</param>
        public void LoadEnvironment(IEnvironment environment)
        {
            if (_environments.Contains(environment))
                throw new Exceptions.ArgumentException("This environment is already loaded (" + environment.Name + ")");

            if (_environments.Any(x => x != environment && x.Name.ToLower() == environment.Name.ToLower()))
                throw new Exceptions.ArgumentException("Environment name conflict on name: " + environment.Name);

            _environments.Add(environment);
        }

        /// <summary>
        /// A method to unload an environment associated with the virtual machine.
        /// </summary>
        /// <param name="environment">The environment to unload.</param>
        public void UnloadEnvironment(IEnvironment environment)
        {
            if (!_environments.Contains(environment))
                throw new Exceptions.ArgumentException("This environment is not loaded (" + environment.Name + ")");

            if (LockedEnvironments.Any(x => x.ToLower() == environment.Name.ToLower()))
                throw new Exceptions.ArgumentException("Not allowed to remove locked environment, this is not legal!");

            _environments.Remove(environment);
        } 

    }
}
