using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Siemens.Engineering;

namespace TIAAutoSave
{
    public class TIAAutoSave
    {
        private decimal autoSaveInterval; //Interval of autosaves in minutes
        private object myConstructor; // constructing object of this object
        public TIAAutoSave(object constructor)
        {
            this.myConstructor = constructor;
        }
        public void run()
        {
            Console.WriteLine("TIAAutoSave: Thread started");
            //Endlessly run this instruction
            while (true)
            {
                try
                {
                    Console.WriteLine("TIAAutoSave: Started main while");
                    //Wait
                    Console.WriteLine("TIAAutoSave: going to sleep for " + autoSaveInterval + "minutes.");
                    System.Threading.Thread.Sleep((int)(autoSaveInterval * 1000 * 60));
                    Console.WriteLine("TIAAutoSave: Woke up");
                    //Call function to save portal projects
                    SavePortals();
                }
                catch (Exception)
                {
                }
                
            }
        }
        public decimal getAutoSaveInterval()
        {
            return this.autoSaveInterval;
        }
        public void setAutosaveInterval(decimal interval) // interval in minutes
        {
            this.autoSaveInterval = interval;
        }

        void SavePortals()
        {
            TIAAutosaveForm tIAAutosaveForm = (TIAAutosaveForm)myConstructor;
          IList<TiaPortalProcess> processes = tIAAutosaveForm.getAutosaveProcesses();
            TiaPortal MyTiaPortal;
            //Loop through all TIA portal processes and attempt to save
            foreach (var process in processes)
            {
                try
                {
                    DateTime localDate = DateTime.Now;
                    MyTiaPortal = process.Attach();
                    if (MyTiaPortal.Projects.FirstOrDefault() != null)
                    {
                        MyTiaPortal.Projects.FirstOrDefault().Save();
                        Console.WriteLine(MyTiaPortal.Projects.FirstOrDefault().Name + " saved: " + localDate.ToString()); 
                    }
                }
                catch (Exception e)
                {
                    //Print any error messages to the console
                    Console.WriteLine(e.Message);
                }
            }
            tIAAutosaveForm.Invoke(tIAAutosaveForm.delegateUpdateWithAutosaveList);

        }
    }

}