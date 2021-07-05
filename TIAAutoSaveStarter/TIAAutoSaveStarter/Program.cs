using System;
using System.Diagnostics;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using TIAAutoSave;

namespace TIAAutoSaveStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            // start TIA and start TIAAutosave if necessary
            string pathToTia = args[0];
            string pathToTiaAutosave = AppDomain.CurrentDomain.BaseDirectory + "TIAAutoSave.exe";
            Process tiaproc = System.Diagnostics.Process.Start(pathToTia);
            Process[] processes = System.Diagnostics.Process.GetProcessesByName("TIAAutosave");
            if (processes.Length == 0)
            {
                System.Diagnostics.Process.Start(pathToTiaAutosave);
            }
            Thread.Sleep(10000); // wait for Windows to start up the process etc. (otherwise we could end up with a missung process id etc.)

            // start the ipc client
            // Create the channel.
            IpcChannel channel = new IpcChannel();
            // Register the channel.
            System.Runtime.Remoting.Channels.ChannelServices.
                RegisterChannel(channel, false);
            // Register as client for remote object.
            System.Runtime.Remoting.WellKnownClientTypeEntry remoteType =
                new System.Runtime.Remoting.WellKnownClientTypeEntry(
                    typeof(TIAAutoSaveServer),
                    "ipc://localhost:9090/TIAAutoSaveServer.rem");
            System.Runtime.Remoting.RemotingConfiguration.
                RegisterWellKnownClientType(remoteType);
            // Create a message sink.
            string objectUri;
            System.Runtime.Remoting.Messaging.IMessageSink messageSink =
                channel.CreateMessageSink(
                    "ipc://localhost:9090/TIAAutoSaveServer.rem", null,
                    out objectUri);
            // Create an instance of the remote object.
            TIAAutoSaveServer tIAAutoSaveServer = new TIAAutoSaveServer();

            // start a method on the server that lets TIAAutoSaveForm add the process with the given Id to the processes with autosave
            tIAAutoSaveServer.AddProcToAS(tiaproc.Id);
        }
    }
}
