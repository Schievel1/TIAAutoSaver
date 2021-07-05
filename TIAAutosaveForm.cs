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
        private List<TiaPortalProcess> noAutosaveProcesses = new List<TiaPortalProcess>();
        private List<TiaPortalProcess> autosaveProcesses = new List<TiaPortalProcess>();
        private List<TiaPortalProcess> startedAutosaveProcesses = new List<TiaPortalProcess>();
        public TIAAutosaveForm tIAAutosaveForm;
        TIAAutoSave tIAAutoSave;
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
            InitializeComponent();
            listViewProcessesWoAS.Columns.Add("Project", 230, HorizontalAlignment.Left);
            listViewProcessesWoAS.Columns.Add("PID", 50, HorizontalAlignment.Right);
            listViewProcessesWithAS.Columns.Add("Project", 230, HorizontalAlignment.Left);
            listViewProcessesWithAS.Columns.Add("PID", 50, HorizontalAlignment.Right);
            listViewProcessesWithAS.Columns.Add("Last time saved", 120, HorizontalAlignment.Right);
            // contruct a new TIAAutoSave Object and start it in its own thread
            tIAAutoSave = new TIAAutoSave(this);
            tIAAutoSave.setAutosaveInterval(numericUpDownASTime.Value);
            autoSaveThread = new Thread(new ThreadStart(tIAAutoSave.run));
            autoSaveThread.IsBackground = true;
            autoSaveThread.Start();
            delegateUpdateWithAutosaveList = new updateWithAutosaveList(fillListBoxInstWithAS);
            // start a thread to update the TIA portal processes
            updateTIAProcessesThread = new Thread(new ThreadStart(this.refreshTIAProcessesProc));
            updateTIAProcessesThread.IsBackground = true;
            updateTIAProcessesThread.Start();
            delegateRefreshNoAutosave = new refreshNoAutosave(refreshNoAutosaveProcesses);
            delegateRefreshAutosave = new refreshAutosave(refreshAutosaveProcesses);
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
        public List<TiaPortalProcess> getAutosaveProcesses()
        {
            return autosaveProcesses;
        }
        private void fillListBoxInstWoAS()
        {
            listViewProcessesWoAS.Items.Clear();
            foreach (var process in noAutosaveProcesses)
            {
                
                if (process != null && process.ProjectPath != null )
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
        private void fillListBoxInstWithAS()
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
                    }else
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
                    } else
                    {
                        lvsi.Text = "-";
                    }
                    lvi.SubItems.Add(lvsi);
                    listViewProcessesWithAS.Items.Add(lvi);
                }
            } 

        }
        public void addProcToASViaPid(int pid)
        {
            // find the process
            IList<TiaPortalProcess> allProcesses = TiaPortal.GetProcesses();


            foreach (TiaPortalProcess process in allProcesses)
            {
                if (process.Id == pid)
                {
                    startedAutosaveProcesses.Add(process);
                    autosaveProcesses.Add(process);
                }
            }
            this.Invoke(this.delegateUpdateWithAutosaveList);

        }
        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            refreshNoAutosaveProcesses();
            refreshAutosaveProcesses();
        }
        private void refreshTIAProcessesProc()
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
        private void refreshNoAutosaveProcesses()
        {
            // Get all TIA Portal processes from static method of TiaPortal and add then to the IList noAutosaveProcesses if they are not on the IList autosaveProcesses
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
            fillListBoxInstWoAS();
        }
        private void refreshAutosaveProcesses()
        {
            // Get all TIA Portal processes from static method of TiaPortal and add then to the IList AutosaveProcesses if they are in listViewProcessesWithAS
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
            fillListBoxInstWithAS();
        }
        private void buttonAddAll_Click(object sender, EventArgs e)
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
            fillListBoxInstWithAS();
            fillListBoxInstWoAS();
        }
        private void buttonAddSel_Click(object sender, EventArgs e)
        {
            if (listViewProcessesWoAS.SelectedItems.Count > 0)
            {
                ListViewItem lvi = listViewProcessesWoAS.SelectedItems[0];
                autosaveProcesses.Add((TiaPortalProcess)lvi.Tag);
                noAutosaveProcesses.Remove((TiaPortalProcess)lvi.Tag);
            }
            fillListBoxInstWithAS();
            fillListBoxInstWoAS();
        }
        private void buttonDelAll_Click(object sender, EventArgs e)
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
            fillListBoxInstWithAS();
            fillListBoxInstWoAS();
        }
        private void buttonDelSel_Click(object sender, EventArgs e)
        {
            if (listViewProcessesWithAS.SelectedItems.Count > 0)
            {
                ListViewItem lvi = listViewProcessesWithAS.SelectedItems[0];
                noAutosaveProcesses.Add((TiaPortalProcess)lvi.Tag);
                autosaveProcesses.Remove((TiaPortalProcess)lvi.Tag);
            }
            fillListBoxInstWithAS();
            fillListBoxInstWoAS();
        }
        private void numericUpDownASTime_ValueChanged(object sender, EventArgs e)
        {
            tIAAutoSave.setAutosaveInterval(numericUpDownASTime.Value);
        }

        public void createIPCServer()
        {
            // Create the server channel.
            IpcChannel serverChannel =
                new IpcChannel("localhost:9090");

            // Register the server channel.
            System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(
                serverChannel);

            // Show the name of the channel.
            Console.WriteLine("The name of the channel is {0}.",
                serverChannel.ChannelName);

            // Show the priority of the channel.
            Console.WriteLine("The priority of the channel is {0}.",
                serverChannel.ChannelPriority);

            // Show the URIs associated with the channel.
            System.Runtime.Remoting.Channels.ChannelDataStore channelData =
                (System.Runtime.Remoting.Channels.ChannelDataStore)
                serverChannel.ChannelData;
            foreach (string uri in channelData.ChannelUris)
            {
                Console.WriteLine("The channel URI is {0}.", uri);
            }

            // Expose an object for remote calls.
            System.Runtime.Remoting.RemotingConfiguration.
                RegisterWellKnownServiceType(
                    typeof(RemoteObject), "RemoteObject.rem",
                    System.Runtime.Remoting.WellKnownObjectMode.Singleton);

            // Parse the channel's URI.
            string[] urls = serverChannel.GetUrlsForUri("RemoteObject.rem");
            if (urls.Length > 0)
            {
                string objectUrl = urls[0];
                string objectUri;
                string channelUri = serverChannel.Parse(objectUrl, out objectUri);
                Console.WriteLine("The object URI is {0}.", objectUri);
                Console.WriteLine("The channel URI is {0}.", channelUri);
                Console.WriteLine("The object URL is {0}.", objectUrl);
            }

            // Wait for the user prompt.
            Console.WriteLine("Press ENTER to exit the server.");
 //           Console.ReadLine();
            Console.WriteLine("The server is exiting.");
        }
        private void OnApplicationExit(object sender, EventArgs e)
        {
            string createText = numericUpDownASTime.Value.ToString() + Environment.NewLine;
            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory+@"\TIAAutosave.ini", createText);
            Environment.Exit(Environment.ExitCode);
        }


        static void Main(String[] args)
        {

            TIAAutosaveForm tIAAutosaveForm = new TIAAutosaveForm();
            tIAAutosaveForm.createIPCServer();
            RemoteObject.tIAAutosaveForm = tIAAutosaveForm;
            Application.ApplicationExit += new EventHandler(tIAAutosaveForm.OnApplicationExit);
            Application.Run(tIAAutosaveForm);

        }
    }

}
