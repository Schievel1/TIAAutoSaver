using System.Windows.Forms;

namespace TIAAutoSave
{
    partial class TIAAutosaveForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonAddSel = new System.Windows.Forms.Button();
            this.buttonAddAll = new System.Windows.Forms.Button();
            this.buttonDelSel = new System.Windows.Forms.Button();
            this.buttonDelAll = new System.Windows.Forms.Button();
            this.labelTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownASTime = new System.Windows.Forms.NumericUpDown();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.listViewProcessesWoAS = new System.Windows.Forms.ListView();
            this.listViewProcessesWithAS = new System.Windows.Forms.ListView();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownASTime)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonAddSel
            // 
            this.buttonAddSel.Location = new System.Drawing.Point(305, 83);
            this.buttonAddSel.Name = "buttonAddSel";
            this.buttonAddSel.Size = new System.Drawing.Size(27, 23);
            this.buttonAddSel.TabIndex = 1;
            this.buttonAddSel.Text = ">";
            this.buttonAddSel.UseVisualStyleBackColor = true;
            this.buttonAddSel.Click += new System.EventHandler(this.buttonAddSel_Click);
            // 
            // buttonAddAll
            // 
            this.buttonAddAll.Location = new System.Drawing.Point(305, 54);
            this.buttonAddAll.Name = "buttonAddAll";
            this.buttonAddAll.Size = new System.Drawing.Size(27, 23);
            this.buttonAddAll.TabIndex = 2;
            this.buttonAddAll.Text = ">>";
            this.buttonAddAll.UseVisualStyleBackColor = true;
            this.buttonAddAll.Click += new System.EventHandler(this.buttonAddAll_Click);
            // 
            // buttonDelSel
            // 
            this.buttonDelSel.Location = new System.Drawing.Point(305, 112);
            this.buttonDelSel.Name = "buttonDelSel";
            this.buttonDelSel.Size = new System.Drawing.Size(27, 23);
            this.buttonDelSel.TabIndex = 3;
            this.buttonDelSel.Text = "<";
            this.buttonDelSel.UseVisualStyleBackColor = true;
            this.buttonDelSel.Click += new System.EventHandler(this.buttonDelSel_Click);
            // 
            // buttonDelAll
            // 
            this.buttonDelAll.Location = new System.Drawing.Point(305, 141);
            this.buttonDelAll.Name = "buttonDelAll";
            this.buttonDelAll.Size = new System.Drawing.Size(27, 23);
            this.buttonDelAll.TabIndex = 4;
            this.buttonDelAll.Text = "<<";
            this.buttonDelAll.UseVisualStyleBackColor = true;
            this.buttonDelAll.Click += new System.EventHandler(this.buttonDelAll_Click);
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(235, 213);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(81, 13);
            this.labelTime.TabIndex = 6;
            this.labelTime.Text = "Autosave every";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(368, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "minutes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "no autosave:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(335, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "autosave:";
            // 
            // numericUpDownASTime
            // 
            this.numericUpDownASTime.DecimalPlaces = 1;
            this.numericUpDownASTime.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDownASTime.Location = new System.Drawing.Point(322, 209);
            this.numericUpDownASTime.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDownASTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownASTime.Name = "numericUpDownASTime";
            this.numericUpDownASTime.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownASTime.TabIndex = 12;
            this.numericUpDownASTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDownASTime.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.numericUpDownASTime.ValueChanged += new System.EventHandler(this.numericUpDownASTime_ValueChanged);
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(305, 22);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(27, 23);
            this.buttonRefresh.TabIndex = 32;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // listViewProcessesWoAS
            // 
            this.listViewProcessesWoAS.FullRowSelect = true;
            this.listViewProcessesWoAS.HideSelection = false;
            this.listViewProcessesWoAS.Location = new System.Drawing.Point(15, 22);
            this.listViewProcessesWoAS.MultiSelect = false;
            this.listViewProcessesWoAS.Name = "listViewProcessesWoAS";
            this.listViewProcessesWoAS.Size = new System.Drawing.Size(284, 177);
            this.listViewProcessesWoAS.TabIndex = 33;
            this.listViewProcessesWoAS.UseCompatibleStateImageBehavior = false;
            this.listViewProcessesWoAS.View = System.Windows.Forms.View.Details;
            // 
            // listViewProcessesWithAS
            // 
            this.listViewProcessesWithAS.FullRowSelect = true;
            this.listViewProcessesWithAS.HideSelection = false;
            this.listViewProcessesWithAS.Location = new System.Drawing.Point(338, 23);
            this.listViewProcessesWithAS.MultiSelect = false;
            this.listViewProcessesWithAS.Name = "listViewProcessesWithAS";
            this.listViewProcessesWithAS.Size = new System.Drawing.Size(407, 176);
            this.listViewProcessesWithAS.TabIndex = 34;
            this.listViewProcessesWithAS.UseCompatibleStateImageBehavior = false;
            this.listViewProcessesWithAS.View = System.Windows.Forms.View.Details;
            // 
            // TIAAutosaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 235);
            this.Controls.Add(this.listViewProcessesWithAS);
            this.Controls.Add(this.listViewProcessesWoAS);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.numericUpDownASTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelTime);
            this.Controls.Add(this.buttonDelAll);
            this.Controls.Add(this.buttonDelSel);
            this.Controls.Add(this.buttonAddAll);
            this.Controls.Add(this.buttonAddSel);
            this.Name = "TIAAutosaveForm";
            this.Text = "TIA Autosave";
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownASTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonAddSel;
        private System.Windows.Forms.Button buttonAddAll;
        private System.Windows.Forms.Button buttonDelSel;
        private System.Windows.Forms.Button buttonDelAll;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownASTime;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.ListView listViewProcessesWoAS;
        private ListView listViewProcessesWithAS;
    }
}