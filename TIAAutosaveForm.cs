using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Remoting.Channels.Ipc;
using System.Threading;
using System.Windows.Forms;
using Siemens.Engineering;

namespace TIAAutoSave
{


    public partial class TIAAutosaveForm : Form
    {
        private TIAAutoSave tIAAutoSave;
        private List<TiaPortalProcess> noAutosaveProcesses = new List<TiaPortalProcess>();
        private List<TiaPortalProcess> autosaveProcesses = new List<TiaPortalProcess>();
        private List<TiaPortalProcess> startedAutosaveProcesses = new List<TiaPortalProcess>();

        // do some thread declarations
        Thread autoSaveThread;
        Thread updateTIAProcessesThread;
        public delegate void updateWithAutosaveList();
        public updateWithAutosaveList delegateUpdateWithAutosaveList;
        public delegate void refreshNoAutosave();
        public refreshNoAutosave delegateRefreshNoAutosave;
        public delegate void refreshAutosave();
        public refreshAutosave delegateRefreshAutosave;
        public TIAAutosaveForm()
        {
            // form work
            InitializeComponent();
            listViewProcessesWoAS.Columns.Add("Project", 230, HorizontalAlignment.Left);
            listViewProcessesWoAS.Columns.Add("PID", 50, HorizontalAlignment.Right);
            listViewProcessesWithAS.Columns.Add("Project", 230, HorizontalAlignment.Left);
            listViewProcessesWithAS.Columns.Add("PID", 50, HorizontalAlignment.Right);
            listViewProcessesWithAS.Columns.Add("Last time saved", 120, HorizontalAlignment.Right);

            // thread work
            // contruct a new TIAAutoSave Object and start it in its own thread
            tIAAutoSave = new TIAAutoSave(this);
            tIAAutoSave.SetAutosaveInterval(numericUpDownASTime.Value);
            autoSaveThread = new Thread(new ThreadStart(tIAAutoSave.Run));
            autoSaveThread.IsBackground = true;
            autoSaveThread.Start();
            delegateUpdateWithAutosaveList = new updateWithAutosaveList(FillListViewInstWithAS);
            // start a thread to update the TIA portal processes
            updateTIAProcessesThread = new Thread(new ThreadStart(this.RefreshTIAProcessesProc));
            updateTIAProcessesThread.IsBackground = true;
            updateTIAProcessesThread.Start();
            delegateRefreshNoAutosave = new refreshNoAutosave(RefreshNoAutosaveProcesses);
            delegateRefreshAutosave = new refreshAutosave(RefreshAutosaveProcesses);

            // settings work
            // get the autosave time interval from last time if the file exists
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\TIAAutosave.ini"))
            {
                numericUpDownASTime.Value = Decimal.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\TIAAutosave.ini"));
            }
            else
            {
                numericUpDownASTime.Value = 5;
            }
            Thread.Sleep(0);
        }
        public List<TiaPortalProcess> GetAutosaveProcesses()
        {
            return autosaveProcesses;
        }
        public void AddProcToASViaPid(int pid)// add a process to the autosaveProcesses list via the Process.Id
        {
            // find the process with the given pid in all tiaPortal-processes
            IList<TiaPortalProcess> allProcesses = TiaPortal.GetProcesses();
            foreach (TiaPortalProcess process in allProcesses)
            {
                if (process.Id == pid) // if found add ist to the list
                {
                    startedAutosaveProcesses.Add(process);
                    autosaveProcesses.Add(process);
                }
            }
            this.Invoke(this.delegateUpdateWithAutosaveList); // update view
        }
        private void RefreshTIAProcessesProc()
        {
            Thread.Sleep(1000); // thread needs to sleep until windowform is ready
            // Process to update the list auf TIA Portal processes every 5 seconds for eternity
            while (true)
            {
                try
                {
                    this.Invoke(this.delegateRefreshNoAutosave);
                    this.Invoke(this.delegateRefreshAutosave);
                    Thread.Sleep(5000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        private void RefreshNoAutosaveProcesses()
        {
            // Get all TIA Portal processes from static method of TiaPortal and add then to the IList noAutosaveProcesses if they are not on the IList autosaveProcesses
            // this is to refresh the AutosaveProcesses and to remove a process from this list in case the TIA Portal is closed
            IList<TiaPortalProcess> allProcesses = TiaPortal.GetProcesses();
            noAutosaveProcesses.Clear();
            foreach (var process in allProcesses)
            {
                if (!(process == null))
                {
                    bool found = false;
                    foreach (var process2 in autosaveProcesses)
                    {
                        if (!(process2 == null))
                        {
                            if (process.Id == process2.Id)
                            {
                                found = true;
                            }
                        }
                    }
                    if (!found)
                    {
                        noAutosaveProcesses.Add(process);
                    }
                }
            }
            FillViewBoxInstWoAS();
        }
        private void RefreshAutosaveProcesses()
        {
            // Get all TIA Portal processes from static method of TiaPortal and add them to the IList AutosaveProcesses if they are in listViewProcessesWithAS
            // this is to refresh the AutosaveProcesses and to remove a process from this list in case the TIA Portal is closed
            IList<TiaPortalProcess> allProcesses = TiaPortal.GetProcesses();
            autosaveProcesses.Clear();
            foreach (var process in allProcesses)
            {
                if (!(process == null))
                {
                    bool found = false;
                    bool isStartedAutosaveProcess = false;
                    foreach (ListViewItem lvi in listViewProcessesWithAS.Items)
                    {
                        TiaPortalProcess process2 = (TiaPortalProcess)lvi.Tag;
                        if (!(lvi.Tag == null))
                        {
                            if (process.Id == process2.Id)
                            {
                                found = true;
                            }
                        }
                        foreach (TiaPortalProcess process3 in startedAutosaveProcesses)
                        {
                            if (process2.Id == process3.Id)
                            {
                                isStartedAutosaveProcess = true;
                            }
                        }
                    }
                    if (found && (process.ProjectPath != null || isStartedAutosaveProcess))
                    {
                        autosaveProcesses.Add(process);
                    }
                }
            }
            FillListViewInstWithAS();
        }
        // Actionslisteners of the form
        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            RefreshNoAutosaveProcesses();
            RefreshAutosaveProcesses();
        }
        private void ButtonAddAll_Click(object sender, EventArgs e)
        {
            List<TiaPortalProcess> removedProcesses = new List<TiaPortalProcess>();
            foreach (var process in noAutosaveProcesses)
            {
                if (!(process == null))
                {
                    autosaveProcesses.Add(process);
                    removedProcesses.Add(process);
                }
            }
            foreach (var process in removedProcesses)
            {
                if (!(process == null))
                {
                    noAutosaveProcesses.Remove(process);
                }
            }
            FillListViewInstWithAS();
            FillViewBoxInstWoAS();
        }
        private void ButtonAddSel_Click(object sender, EventArgs e)
        {
            if (listViewProcessesWoAS.SelectedItems.Count > 0)
            {
                ListViewItem lvi = listViewProcessesWoAS.SelectedItems[0];
                autosaveProcesses.Add((TiaPortalProcess)lvi.Tag);
                noAutosaveProcesses.Remove((TiaPortalProcess)lvi.Tag);
            }
            FillListViewInstWithAS();
            FillViewBoxInstWoAS();
        }
        private void ButtonDelAll_Click(object sender, EventArgs e)
        {
            List<TiaPortalProcess> removedProcesses = new List<TiaPortalProcess>();
            foreach (var process in autosaveProcesses)
            {
                if (!(process == null))
                {
                    noAutosaveProcesses.Add(process);
                    removedProcesses.Add(process);
                }
            }
            foreach (var process in removedProcesses)
            {
                if (!(process == null))
                {
                    autosaveProcesses.Remove(process);
                }
            }
            FillListViewInstWithAS();
            FillViewBoxInstWoAS();
        }
        private void ButtonDelSel_Click(object sender, EventArgs e)
        {
            if (listViewProcessesWithAS.SelectedItems.Count > 0)
            {
                ListViewItem lvi = listViewProcessesWithAS.SelectedItems[0];
                noAutosaveProcesses.Add((TiaPortalProcess)lvi.Tag);
                autosaveProcesses.Remove((TiaPortalProcess)lvi.Tag);
            }
            FillListViewInstWithAS();
            FillViewBoxInstWoAS();
        }
        private void NumericUpDownASTime_ValueChanged(object sender, EventArgs e) => tIAAutoSave.SetAutosaveInterval(numericUpDownASTime.Value);
        // other form work
        private void FillViewBoxInstWoAS() // fill the listview for processes without autosave with entries for all processes in the List noAutosaveProcesses
        {
            listViewProcessesWoAS.Items.Clear();
            foreach (var process in noAutosaveProcesses)
            {

                if (process != null && process.ProjectPath != null)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = process;
                    lvi.Text = process.ProjectPath.Name;
                    ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
                    lvsi.Text = process.Id.ToString();
                    lvi.SubItems.Add(lvsi);
                    listViewProcessesWoAS.Items.Add(lvi);
                }
            }
        }
        private void FillListViewInstWithAS()// fill the listview for processes with autosave with entries for all processes in the List autosaveProcesses
        {
            bool isStartedAutosaveProcess = false;
            listViewProcessesWithAS.Items.Clear();
            foreach (var process in autosaveProcesses)
            {
                foreach (TiaPortalProcess process2 in startedAutosaveProcesses)
                {
                    if (process.Id == process2.Id)
                    {
                        isStartedAutosaveProcess = true;
                    }
                }
                if (process != null && process.ProjectPath != null || isStartedAutosaveProcess)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = process;
                    if (process.ProjectPath != null)
                    {
                        lvi.Text = process.ProjectPath.Name;
                    }
                    else
                    {
                        lvi.Text = "no project loaded in portal";
                    }
                    ListViewItem.ListViewSubItem lvsi = new ListViewItem.ListViewSubItem();
                    lvsi.Text = process.Id.ToString();
                    lvi.SubItems.Add(lvsi);
                    lvsi = new ListViewItem.ListViewSubItem();
                    if (process.ProjectPath != null)
                    {
                        lvsi.Text = process.ProjectPath.LastWriteTime.ToString();
                    }
                    else
                    {
                        lvsi.Text = "-";
                    }
                    lvi.SubItems.Add(lvsi);
                    listViewProcessesWithAS.Items.Add(lvi);
                }
            }

        }
        public void CreateIPCServer()
        {
            // Create the server channel.
            IpcChannel serverChannel =
                new IpcChannel("localhost:9090");
            // Register the server channel.
            System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(
                serverChannel, false);
            // Expose an object for remote calls.
            System.Runtime.Remoting.RemotingConfiguration.
                RegisterWellKnownServiceType(
                    typeof(TIAAutoSaveServer), "TIAAutoSaveServer.rem",
                    System.Runtime.Remoting.WellKnownObjectMode.Singleton);
        }// Inter process com. to add processes to the list of autosave processes remotely

        // Start and exit
        private void OnApplicationExit(object sender,
                                       EventArgs e)
        {
            string createText = numericUpDownASTime.Value.ToString() + Environment.NewLine;
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory+@"\TIAAutosave.ini", createText);
            Environment.Exit(Environment.ExitCode);
        }
        static void Main(String[] args)
        {
            TIAAutosaveForm tIAAutosaveForm = new TIAAutosaveForm();
            tIAAutosaveForm.CreateIPCServer();
            TIAAutoSaveServer.tIAAutosaveForm = tIAAutosaveForm;
            Application.ApplicationExit += new EventHandler(tIAAutosaveForm.OnApplicationExit);
            Application.Run(tIAAutosaveForm);
        }
    }

}
