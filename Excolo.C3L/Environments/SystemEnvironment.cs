using Excolo.C3L.Core;
using Excolo.C3L.Exceptions;
using Excolo.C3L.Executer;
using Excolo.C3L.Interfaces;
using Excolo.C3L.Interfaces.AST;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Environments
{
    public class SystemEnvironment : BaseEnvironment
    {
        /// <summary>
        /// Get the virtual machine this environment belongs to.
        /// </summary>
        protected virtual IVirtualMachine VirtualMachine { get; private set; }

        /// <summary>
        /// Get the name of the system environment.
        /// </summary>
        public override string Name
        {
            get { return "system"; }
        }

        /// <summary>
        /// A constructor for the system environment.
        /// </summary>
        /// <param name="vm"></param>
        public SystemEnvironment(IVirtualMachine vm = null)
        {
            VirtualMachine = vm;
            Bind(Time, "time", null, new { time = new[] { "datetime", "date", "now" } });
            Bind(Exit, "exit", null, new { exit = new[] { "quit" } });
        }

        private IEnumerable<IArgument> Time(ArgumentValueLookup args)
        {
            var time = DateTime.Now.ToString();

            VirtualMachine.Output.WriteLine(time);

            return new IArgument[] { new ValueArgument(time, typeof(string)) };
        }

        private IEnumerable<IArgument> Exit(ArgumentValueLookup args)
        {
            throw new ExitShellException("Bye!");
        }
    }
}
