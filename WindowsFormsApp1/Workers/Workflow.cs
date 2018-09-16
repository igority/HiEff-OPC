using System;
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
        public static List<TestInput> testInputs;
        public static List<TestOutput> testOutputs;
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

            opcClient.AddWriteGroupTags(); //test - uncomment in real application
            opcClient.AddReadGroupTags();

            opcClient.writeGroup = opcClient.SetupOPCWriteGroup("WriteGroup");
            opcClient.readGroup = opcClient.SetupOPCReadGroup("ReadGroup");



            //currentOrder = new List<Order>();
            //currentOrder = dbClient.GetOrders();
        }

        void MainLoop()
        {
            while (running)
            {
                // DELAY THE LOOP FOR 300 ms
                Thread.Sleep(100);
                //List<Vendor> _vendors = new List<Vendor>(); ;
                //CHECK IF ALL CONDITIONS ARE OK, IF NO - GO TO TOP OF LOOP
                if (!checkConditions()) continue;
                dbClient.updateTestOutput(opcClient.CurrentTestOutput);



                //GET ALL VENDORS FROM DB
                /*
                if (vendors.Count == 0) { vendors = dbClient.GetVendors(); continue; }

                _vendors = dbClient.GetVendors();
                //UPDATE PLC WITH VENDORS
                for (int i = 0; i < _vendors.Count;i++)
                {
                    if (vendors[i].orderids.Count == 0) continue;
                    //GET THE OLDEST PENDING ORDER FROM DB, IF NONE - GO TO TOP OF LOOP
                    currentOrder = dbClient.GetOrders(_vendors[i]);
                    if (currentOrder.Count == 0) continue;

                    _vendors[i].robot.test1 = vendors[i].robot.test1;
                    _vendors[i].robot.test2 = vendors[i].robot.test2;
                    _vendors[i].robot.test3 = vendors[i].robot.test3;
                    vendors[i] = _vendors[i];
                    
                    vendors[i].robot.order = currentOrder;
                    vendors[i].updatePLC();

                    

                    //GIVE THE ORDER TO THE ROBOT - UPDATE OPC SERVER AND PLC
                }
                */

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
