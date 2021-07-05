using System;
using System.Diagnostics;
using System.Runtime.Remoting.Channels.Ipc;
using TIAAutoSave;

namespace TIAAutoSaveStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            string pathToTia = args[0];
            string pathToTiaAutosave = AppDomain.CurrentDomain.BaseDirectory + "TIAAutoSave.exe";
            Process tiaproc = System.Diagnostics.Process.Start(pathToTia);
            Process[] processes = System.Diagnostics.Process.GetProcessesByName("TIAAutosave");
            if (processes.Length == 0)
            {
                System.Diagnostics.Process.Start(pathToTiaAutosave);
            }
            addTIAProcViaIPCChannel(tiaproc.Id); 
        }
        public static void addTIAProcViaIPCChannel(int pid)
        {
            // Create the channel.
            IpcChannel channel = new IpcChannel();

            // Register the channel.
            System.Runtime.Remoting.Channels.ChannelServices.
                RegisterChannel(channel);

            // Register as client for remote object.
            System.Runtime.Remoting.WellKnownClientTypeEntry remoteType =
                new System.Runtime.Remoting.WellKnownClientTypeEntry(
                    typeof(RemoteObject),
                    "ipc://localhost:9090/RemoteObject.rem");
            System.Runtime.Remoting.RemotingConfiguration.
                RegisterWellKnownClientType(remoteType);

            // Create a message sink.
            string objectUri;
            System.Runtime.Remoting.Messaging.IMessageSink messageSink =
                channel.CreateMessageSink(
                    "ipc://localhost:9090/RemoteObject.rem", null,
                    out objectUri);
            Console.WriteLine("The URI of the message sink is {0}.",
                objectUri);
            if (messageSink != null)
            {
                Console.WriteLine("The type of the message sink is {0}.",
                    messageSink.GetType().ToString());
            }

            // Create an instance of the remote object.
            RemoteObject service = new RemoteObject();

            // Invoke a method on the remote object.
            Console.WriteLine("The client is invoking the remote object.");
            service.addProcToAS(pid);
        }
    }
}
