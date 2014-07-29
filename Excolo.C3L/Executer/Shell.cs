using Excolo.C3L.Exceptions;
using Excolo.C3L.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Excolo.C3L.Executer
{
    public class Shell
    {
        public Shell(StreamReader input, StreamWriter output, StreamWriter log = null)
        {
            Input = input;
            if (log == null) log = new StreamWriter(Stream.Null);
            if (output == null) output = new StreamWriter(Stream.Null);
            Running = true;
            VirtualMachine = new VirtualMachine(output, log);
        }

        public Shell(IVirtualMachine virtualMachine)
        {
            if (virtualMachine == null) throw new ArgumentNullException("virtualMachine");
            VirtualMachine = virtualMachine;
        }

        public StreamReader Input
        {
            get;
            set;
        }

        public StreamWriter Output
        {
            get
            {
                return VirtualMachine.Output;
            }
            set
            {
                VirtualMachine.Output = value;
            }
        }

        public StreamWriter Log
        {
            get
            {
                return VirtualMachine.Log;
            }
            set
            {
                VirtualMachine.Log = value;
            }
        }


        public IVirtualMachine VirtualMachine
        {
            get;
            set;
        }


        public void Start()
        {
            string cmd;
            bool firstException = true;
            Output.AutoFlush = true;
            VirtualMachine.HandleError = x =>
            {
                if (x.Position >= 0)
                {
                    if (firstException)
                    {
                        string toWrite = "-";
                        for (int i = 0; i < x.Position; ++i)
                        {
                            toWrite += '-';
                        }
                        Output.WriteLine(toWrite + '^');
                    }
                    else
                    {
                        Output.WriteLine("Error occured at position: " + x.Position);
                    }
                }
                Output.WriteLine(x.Message);
                firstException = false;
            };
            try
            {
                Running = true;
                Output.Write("> ");
                while ((cmd = Input.ReadLine()) != null && Running)
                {
                    if (cmd.Trim() != string.Empty)
                    {
                        firstException = true;
                        VirtualMachine.Execute(cmd);
                    }
                    Output.Write("> ");
                }
            }
            catch (ExitShellException)
            { }
            finally
            {
                Stop();
            }
        }

        public void Stop()
        {
            Running = false;
            Log.Flush();
        }

        public bool Running
        {
            get;
            private set;
        }
    }
}
