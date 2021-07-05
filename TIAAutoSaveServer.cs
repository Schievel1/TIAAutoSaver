using System;
namespace TIAAutoSave
{
    public class TIAAutoSaveServer : MarshalByRefObject
    {
        public static TIAAutosaveForm tIAAutosaveForm; // constructing object of this object (set manually when contstructing, because a non standard contructor cannot be called from client)
        public void AddProcToAS(int pid)
        {
            tIAAutosaveForm.AddProcToASViaPid(pid);
        }
    }
}