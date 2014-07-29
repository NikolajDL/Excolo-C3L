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
    /// <summary>
    /// An environment class to handle input-output of the virtual machine / shell.
    /// </summary>
    public class IOEnvironment : BaseEnvironment
    {
        /// <summary>
        /// Get the virtual machine this environment belongs to.
        /// </summary>
        protected virtual IVirtualMachine VirtualMachine { get; private set; }



        /// <summary>
        /// Get the name of the environment.
        /// </summary>
        public override string Name
        {
            get { return "io"; }
        }

        /// <summary>
        /// A constructor for the IOEnvironment.
        /// </summary>
        /// <param name="vm"></param>
        public IOEnvironment(IVirtualMachine vm = null)
        {
            VirtualMachine = vm;


            Bind(Echo, "echo message*", null, new { echo = new [] {"print"}});
            Bind(ClearScreen, "clearscreeen", 
                null,
                new { clearscreeen = new[] { "clear", "cls" } });
            Bind(Log, "log message");
        }

        private IEnumerable<IArgument> Echo(ArgumentValueLookup args)
        {
            foreach (var arg in args)
            {
                VirtualMachine.Output.Write(arg.Value.Value + " ");
            }
            VirtualMachine.Output.WriteLine();
            yield break;
        }

        private IEnumerable<IArgument> ClearScreen(ArgumentValueLookup args)
        {
            VirtualMachine.Output.Flush();
            VirtualMachine.Output.AutoFlush = false;
            for (int i = 0; i < 1000; ++i)
                VirtualMachine.Output.WriteLine();
            VirtualMachine.Output.Flush();
            VirtualMachine.Output.AutoFlush = true;

            yield break;
        }

        private IEnumerable<IArgument> Log(ArgumentValueLookup args)
        {
            string format = "[{0}] - {1}";
            string message = String.Join(" ", 
                args.Where(x => x.Key == "message").Select(x => x.Value.Value.ToString()));

            VirtualMachine.Log.WriteLine(string.Format(format, DateTime.Now.ToString(), message));

            yield break;
        }
    }
}
