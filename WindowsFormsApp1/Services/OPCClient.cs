using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using OPCAutomation;
using System.ComponentModel;
using WindowsFormsApp1.Helpers;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Workers;

namespace WindowsFormsApp1.Services
{
    class OPCClient
    {
        public TestInput CurrentTestInput { get; set; }
        public TestOutput CurrentTestOutput { get; set; }

        private TestInput previousTestInput = null;

        OPCServer ObjOPCServer;
        OPCGroups ObjOPCGroups;
        public OPCGroup writeGroup;
        public OPCGroup readGroup;

        int NoOfItems;
        string _serverName;
        public static int tagIndex;
        public static int tagIndexReader;

        public static Array writerOPCItemIDs;
        Array writerItemServerHandles;
        Array writerItemServerErrors;
        Array writerClientHandles;
        Array writerRequestedDataTypes;
        Array writerAccessPaths;
        public static Array ItemServerWriteValues;

        public static Array readerOPCItemIDs;
        Array readerItemServerHandles;
        Array readerItemServerErrors;
        Array readerClientHandles;
        Array readerRequestedDataTypes;
        Array readerAccessPaths;
        public static Array ItemServerReadValues;

        Thread Reader;
        Thread Writer;

        bool stopThreads;

        public OPCClient()
        {
            Init();
            ConnectToOPC();
            //AddTagsFromPLC();
            //SetupOPCGroup("Group01");
            //Reader = new Thread(() => ReadItems(ObjOPCGroup));
            //Writer = new Thread(() => WriteItems(ObjOPCGroup));

            //Reader.Start();
            //Writer.Start();
        }

        void Init()
        {
            //SETUP VARIABLES
            stopThreads = false;
            tagIndex = 1;
            tagIndexReader = 1;
            NoOfItems = 10000;

            writerOPCItemIDs = Array.CreateInstance(typeof(string), NoOfItems);
            writerItemServerHandles = Array.CreateInstance(typeof(Int32), NoOfItems);
            writerItemServerErrors = Array.CreateInstance(typeof(Int32), NoOfItems);
            writerClientHandles = Array.CreateInstance(typeof(Int32), NoOfItems);
            writerRequestedDataTypes = Array.CreateInstance(typeof(Int16), NoOfItems);
            writerAccessPaths = Array.CreateInstance(typeof(string), NoOfItems);
            ItemServerWriteValues = Array.CreateInstance(typeof(object), NoOfItems);

            readerOPCItemIDs = Array.CreateInstance(typeof(string), NoOfItems);
            readerItemServerHandles = Array.CreateInstance(typeof(Int32), NoOfItems);
            readerItemServerErrors = Array.CreateInstance(typeof(Int32), NoOfItems);
            readerClientHandles = Array.CreateInstance(typeof(Int32), NoOfItems);
            readerRequestedDataTypes = Array.CreateInstance(typeof(Int16), NoOfItems);
            readerAccessPaths = Array.CreateInstance(typeof(string), NoOfItems);
            ItemServerReadValues = Array.CreateInstance(typeof(string), NoOfItems);

        }

        void ConnectToOPC()
        {
            //CONNECT TO OPC SERVER
            ObjOPCServer = new OPCServer();
            _serverName = Globals.OPC_SERVER_NAME;
            //_serverName = "Matrikon.OPC.SiemensPLC.1";
            ObjOPCServer.Connect(_serverName, "");
            ObjOPCGroups = ObjOPCServer.OPCGroups;
        }

        public void AddReadGroupTags()
        {
            //tagIndex = 3; test
            //ADD TAGS FROM PLC TO OPC GROUP
            //OPCItemIDs.SetValue("Bucket Brigade.Int1", 1);
            //OPCItemIDs.SetValue("Bucket Brigade.Int2", 2);
            //OPCItemIDs.SetValue("Bucket Brigade.Real1", 3);
            //for (int i=1;i<=7700;i+=2)
            //{
            //    OPCItemIDs.SetValue("Bucket Brigade.Int1", i);
            //    ItemServerWriteValues.SetValue(78, i);
            //    OPCItemIDs.SetValue("Bucket Brigade.Int2", i+1);
            //    ItemServerWriteValues.SetValue(93, i+1);
            //}
            //readerOPCItemIDs.SetValue("Bucket Brigade.UInt1", tagIndexReader++);
            //readerOPCItemIDs.SetValue("Bucket Brigade.UInt2", tagIndexReader++);
            readerOPCItemIDs.SetValue(Globals.INPUT_TAG_1_NAME, tagIndexReader++);  //test
            readerOPCItemIDs.SetValue(Globals.OUTPUT_TAG_1_NAME, tagIndexReader++);  //test
            readerOPCItemIDs.SetValue(Globals.INPUT_TAG_2_NAME, tagIndexReader++);  //test
            readerOPCItemIDs.SetValue(Globals.OUTPUT_TAG_2_NAME, tagIndexReader++);  //test
            readerOPCItemIDs.SetValue(Globals.OUTPUT_TAG_3_NAME, tagIndexReader++);  //test

            // OPCItemIDs.SetValue("TCP CHANNEL>ISOTCP>DB100:BOOL:0.0", tagIndex++);

        }

        public OPCGroup SetupOPCReadGroup(string name)
        {
            OPCGroup _ObjOPCGroup;
            _ObjOPCGroup = ObjOPCGroups.Add(name);
            //_ObjOPCGroup.DataChange += new DIOPCGroupEvent_DataChangeEventHandler(ObjOPCGroup_DataChange);
            _ObjOPCGroup.UpdateRate = 200;
            _ObjOPCGroup.IsActive = true;
            _ObjOPCGroup.IsSubscribed = true;
            _ObjOPCGroup.OPCItems.AddItems(tagIndexReader - 1, ref readerOPCItemIDs, ref readerClientHandles, out readerItemServerHandles, out readerItemServerErrors, readerRequestedDataTypes, readerAccessPaths);
            return _ObjOPCGroup;
        }

        public OPCGroup SetupOPCWriteGroup(string name)
        {
            OPCGroup _ObjOPCGroup;
            _ObjOPCGroup = ObjOPCGroups.Add(name);
            _ObjOPCGroup.UpdateRate = 200;
            _ObjOPCGroup.IsActive = true;
            _ObjOPCGroup.IsSubscribed = true;
            _ObjOPCGroup.OPCItems.AddItems(tagIndex, ref writerOPCItemIDs, ref writerClientHandles, out writerItemServerHandles, out writerItemServerErrors, writerRequestedDataTypes, writerAccessPaths);

            //Array _ItemServerHandles=ItemServerHandles;
            //int itemsNum=0; //test
            //foreach(int item in ItemServerHandles)
            //{
            //    itemsNum++;
            //    if (item!=0)
            //    {
            //        _ItemServerHandles.SetValue(item, itemsNum);
            //    }
            //}
            return _ObjOPCGroup;
        }

        public void startReader(OPCGroup _ObjOPCGroup)
        {
            Reader = new Thread(() => ReadItems(_ObjOPCGroup));
            Reader.Start();
        }

        public void stopReader(OPCGroup _ObjOPCGroup)
        {
            stopThreads = true;
           // Reader.Abort();
        }

        public void startWriter(OPCGroup _ObjOPCGroup)
        {
            Writer = new Thread(() => WriteItems(_ObjOPCGroup));
            Writer.Start();
        }

        public void stopWriter(OPCGroup _ObjOPCGroup)
        {
            stopThreads = true;
           // Writer.Abort();
        }

        void ReadItems(OPCGroup ObjOPCGroup)
        {

            //READ ITEMS
            object qualities;
            object timestamp;

            if (CurrentTestOutput == null) CurrentTestOutput = new TestOutput();

            while (!stopThreads)
            {
                //DA SE NAJDE DOBAR NACIN ZA CITANJE NA TAGOVITE SO SERVER HANDLES
                ObjOPCGroup.SyncRead((short)OPCAutomation.OPCDataSource.OPCDevice, tagIndexReader - 1, ref readerItemServerHandles, out ItemServerReadValues, out readerItemServerErrors, out qualities, out timestamp);
                string message = "";
                for (int i = 1; i <= 5; i++)
                {
                    switch (i)
                    {
                        case 2:
                            CurrentTestOutput.output_bool = (bool)ItemServerReadValues.GetValue(i);
                            break;
                        case 4:
                            CurrentTestOutput.output_int = (int)ItemServerReadValues.GetValue(i);
                            break;
                        case 5:
                            CurrentTestOutput.output_random = (int)ItemServerReadValues.GetValue(i);
                            break;
                        default:
                            break;
                    }
                    message = message + i.ToString() + ": " + ItemServerReadValues.GetValue(i).ToString() + "\t";
                }

                //message = message + " " + ((Array)timestamp).GetValue(1).ToString() + " " + ((Array)timestamp).GetValue(2).ToString() + " " + ((Array)timestamp).GetValue(3).ToString();
                Console.WriteLine(message);
                //Workflow.LogMessage(message);
                //UPDATE WORKFLOW.VENDORS.ROBOT.ORDER.STATUS with OPC STATUSES!!! np tests
                //}
                /*
                foreach (Vendor vendor in Workflow.vendors)
                {
                    vendor.robot.test1 = ItemServerReadValues.GetValue(1).ToString();
                    vendor.robot.test2 = ItemServerReadValues.GetValue(2).ToString();
                    vendor.robot.test3 = ItemServerReadValues.GetValue(3).ToString();
                }
                */

                System.Threading.Thread.Sleep(500);
            }
        }

        void WriteItems(OPCGroup ObjOPCGroup)
        {
            while (!stopThreads)
            {
               
                if (Workflow.testInputs != null)
                {
                    CurrentTestInput = Workflow.testInputs.First();
                    if (previousTestInput == null || previousTestInput != CurrentTestInput)
                    {
                       // ItemServerWriteValues.SetValue(CurrentTestInput.input_bool, 0);
                        ItemServerWriteValues.SetValue(CurrentTestInput.input_bool, 1);
                        ItemServerWriteValues.SetValue(CurrentTestInput.input_int, 2);

                        ObjOPCGroup.SyncWrite(tagIndex - 1, ref writerItemServerHandles, ref ItemServerWriteValues, out writerItemServerErrors);
                        previousTestInput = CurrentTestInput;
                    }
                }

                //ItemServerWriteValues.SetValue(1.1, Array.IndexOf(OPCItemIDs,(object)("Bucket Brigade.Real1")));
                System.Threading.Thread.Sleep(1000);
            }
        }


        public void AddWriteGroupTags()
        {
            //writerOPCItemIDs.SetValue("Bucket Brigade.Int1", tagIndex++);
            //writerOPCItemIDs.SetValue("Bucket Brigade.Int2", tagIndex++);
            writerOPCItemIDs.SetValue(Globals.INPUT_TAG_1_NAME, tagIndex++);  //test
            writerOPCItemIDs.SetValue(Globals.INPUT_TAG_2_NAME, tagIndex++);  //test
            // writerOPCItemIDs.SetValue("AB:TESTCOMMS_DB_PLC:DINT:OPC_RANDOMOUTPUT.VALUE", tagIndex++);  //test

            //for(int k=1;k<=5;k++)
            //{
            //    string name = "robot[" + k.ToString() + "].";

            //    for (int i = 1; i <= 20; i++)
            //    {
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "]._id", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].userid", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].quantity", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].order_type", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].created_on", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].created_date", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].order_status", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].Payment", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].waiting_time", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].time", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].vendor_id", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].order_no", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].price", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].mode", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].drink_name", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].paymentinfo", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].glassid", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].categoryid", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "].productid", OPCClient.tagIndex++);

            //        for (int j = 1; j <= 10; j++)
            //        {
            //            OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "]." + "ingredient[" + j.ToString() + "].id", OPCClient.tagIndex++);
            //            OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "]." + "ingredient[" + j.ToString() + "].title", OPCClient.tagIndex++);
            //            OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "]." + "ingredient[" + j.ToString() + "].measure_in", OPCClient.tagIndex++);
            //            OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "]." + "ingredient[" + j.ToString() + "]._id", OPCClient.tagIndex++);
            //            OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "]." + "ingredient[" + j.ToString() + "].time", OPCClient.tagIndex++);
            //            OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "]." + "ingredient[" + j.ToString() + "].quantity", OPCClient.tagIndex++);

            //        }

            //        for (int j = 1; j <= 10; j++)
            //        {
            //            OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "]." + "garnish[" + j.ToString() + "].id", OPCClient.tagIndex++);
            //            OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "]." + "garnish[" + j.ToString() + "].title", OPCClient.tagIndex++);
            //            OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "]." + "garnish[" + j.ToString() + "].measure_in", OPCClient.tagIndex++);
            //            OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "]." + "garnish[" + j.ToString() + "]._id", OPCClient.tagIndex++);
            //            OPCClient.OPCItemIDs.SetValue(name + "order[" + i.ToString() + "]." + "garnish[" + j.ToString() + "].quantity", OPCClient.tagIndex++);

            //        }

            //    }


            //        OPCClient.OPCItemIDs.SetValue(name + "vendor._id", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "vendor.vendorno", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "vendor.have_orders", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "vendor.status", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "vendor.current_order_time", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "vendor.totaltime", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "vendor.title", OPCClient.tagIndex++);
            //        OPCClient.OPCItemIDs.SetValue(name + "status", OPCClient.tagIndex++);

            //}


        }
    }
}
