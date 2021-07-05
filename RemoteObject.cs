using System;
using System.Reflection;

namespace TIAAutoSave
{
    public class RemoteObject : MarshalByRefObject
    {
        public static TIAAutosaveForm tIAAutosaveForm; // constructing object of this object (set manually when contstructing, because a non standard contructor cannot be called from client)
        public void addProcToAS(int pid)
        {
            tIAAutosaveForm.addProcToASViaPid(pid);
        }
        public void doSomething()
        {
            Console.WriteLine("GetCount has been called.");
        }
    }
}