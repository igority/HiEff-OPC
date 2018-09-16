using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Services;
using WindowsFormsApp1.Workers;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        private bool running = false;
        private Workflow worker;

        public Form1()
        {
            InitializeComponent();
        }

        private void stop_button_Click(object sender, EventArgs e)
        {
            running = false;
            if (worker != null)
            {
                worker.Stop_Process();
                lblStatus.Text = "Stopped";
                lblStatus.ForeColor = Color.Red;
            }
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            running = true;
            worker = new Workflow(this);
            worker.Start_Process();
            lblStatus.Text = "Running";
            lblStatus.ForeColor = Color.Green;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
        }
    }
}
