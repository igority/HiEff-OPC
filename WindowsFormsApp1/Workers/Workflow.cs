﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Services;

namespace WindowsFormsApp1.Workers
{
    class Workflow
    {
        private Form1 form1;
        private bool running = false;
        DBClient dbClient;
        OPCClient opcClient;
        public static List<PLCInput> plcInputs;
        public static List<PLCOutput> plcOutputs;
        Thread Master;

        public Workflow(Form1 form1)
        {
            this.form1 = form1;
            Init();

            Master = new Thread(MainLoop);
            //Master.Start();

        }

        public void Start_Process()
        {
            running = true;
            plcInputs = new List<PLCInput>();
            plcOutputs = new List<PLCOutput>();
            opcClient.startReader(opcClient.readGroup);
            opcClient.startWriter(opcClient.writeGroup);
            Master.Start();
        }

        public void Stop_Process()
        {
            running = false;
            opcClient.stopReader(opcClient.readGroup);
            opcClient.stopWriter(opcClient.writeGroup);
            Master.Abort();
        }


        void Init()
        {
            dbClient = new DBClient();
            opcClient = new OPCClient();

            opcClient.AddWriteGroupTags(); 
            opcClient.AddReadGroupTags();

            opcClient.writeGroup = opcClient.SetupOPCWriteGroup("WriteGroup");
            opcClient.readGroup = opcClient.SetupOPCReadGroup("ReadGroup");

        }

        void MainLoop()
        {
            try
            {
                while (running)
                {
                    // DELAY THE LOOP FOR 300 ms
                    Thread.Sleep(100);

                    //CHECK IF ALL CONDITIONS ARE OK, IF NO - GO TO TOP OF LOOP
                    if (!checkConditions()) continue;
                    // dbClient.updatePLCOutput(opcClient.CurrentPLCOutput);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }

        }

        bool checkConditions()
        {

            return true;
        }


        //public static void LogMessage(string text)
        //{

        //    string path = @"d:\log.txt";
        //    // This text is added only once to the file.
        //    if (!File.Exists(path))
        //    {
        //        // Create a file to write to.
        //        using (StreamWriter sw = File.CreateText(path))
        //        {
        //            sw.WriteLine("Hello");

        //        }
        //    }

        //    // This text is always added, making the file longer over time
        //    // if it is not deleted.
        //    using (StreamWriter sw = File.AppendText(path))
        //    {
        //        sw.WriteLine(System.DateTime.Now.ToString() + " " + text);

        //    }
        //}
    }
}
