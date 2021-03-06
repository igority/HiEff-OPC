﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using OPCAutomation;
using System.ComponentModel;
using OPCtoMongoDBService.Helpers;
using OPCtoMongoDBService.Models;
using OPCtoMongoDBService.Workers;

namespace OPCtoMongoDBService.Services
{
    class OPCClient
    {
        public PLCInput CurrentPLCInput { get; set; }
        public PLCOutput CurrentPLCOutput { get; set; }

        private PLCInput previousPLCInput = null;

        OPCServer ObjOPCServer;
        OPCGroups ObjOPCGroups;
        public OPCGroup writeGroup;
        public OPCGroup readGroup;

        int NoOfItems;
        string _serverName;
        public static int tagIndexWriter;
        public static int tagIndexReader;

        public static Array writerOPCItemIDs;
        Array writerItemServerHandles;
        Array writerItemServerErrors;
        Array writerClientHandles;
        Array writerRequestedDataTypes;
        Array writerAccessPaths;
        public static Array ItemServerWriteValues;
        public static Array OrderServerWriteValues;

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
        }

        void Init()
        {
            //SETUP VARIABLES
            stopThreads = false;
            tagIndexWriter = 1;
            tagIndexReader = 1;
            NoOfItems = 10000;

            writerOPCItemIDs = Array.CreateInstance(typeof(string), NoOfItems);
            writerItemServerHandles = Array.CreateInstance(typeof(Int32), NoOfItems);
            writerItemServerErrors = Array.CreateInstance(typeof(Int32), NoOfItems);
            writerClientHandles = Array.CreateInstance(typeof(Int32), NoOfItems);
            writerRequestedDataTypes = Array.CreateInstance(typeof(Int16), NoOfItems);
            writerAccessPaths = Array.CreateInstance(typeof(string), NoOfItems);
            ItemServerWriteValues = Array.CreateInstance(typeof(object), NoOfItems);
            OrderServerWriteValues = Array.CreateInstance(typeof(object), NoOfItems);

            readerOPCItemIDs = Array.CreateInstance(typeof(string), NoOfItems);
            readerItemServerHandles = Array.CreateInstance(typeof(Int32), NoOfItems);
            readerItemServerErrors = Array.CreateInstance(typeof(Int32), NoOfItems);
            readerClientHandles = Array.CreateInstance(typeof(Int32), NoOfItems);
            readerRequestedDataTypes = Array.CreateInstance(typeof(Int16), NoOfItems);
            readerAccessPaths = Array.CreateInstance(typeof(string), NoOfItems);
            ItemServerReadValues = Array.CreateInstance(typeof(string), NoOfItems);

        }

        public int GetCurrentOrderStatus()
        {
            return GetInt32Value("ORDER_STATUS");
        }

        internal List<OrderUserDTO> GetCurrentOrderStatuses()
        {
            //we get all for now, and update them all regardless of change
            // TODO - detect change and update only if there is actual change
            int totalDrinks = 4;
            var orders = new List<OrderUserDTO>();
            for (int drinkIndex = 1; drinkIndex <= totalDrinks; drinkIndex++)
            {
                int drinkID = GetInt32Value("DRINK_" + drinkIndex + "_ID");
                if (drinkID != 0)
                {
                    var order = new OrderUserDTO();
                    order.order = GetInt32Value("ORDER_ID");
                    order.drink = drinkID;
                    order.status_drink = GetInt32Value("DRINK_" + drinkIndex + "_STATUS");
                    order.robot = 1;
                    //todo logic for ingredient and later for garnish
                    orders.Add(order);
                }
            }
            return orders;
            //OrderUserDTO orderUserDTO = new OrderUserDTO()
            //{
            //    order = 247,
            //    robot = 1,
            //    status_drink = 15,
            //    drink = 385
            //    // status_drink = 
            //    // drink = 
            //    // ingredient = 
            //};
        }

        public int GetCurrentOrderId()
        {
            return GetInt32Value("ORDER_ID");
        }

        private int GetInt32Value(string index_name)
        {
            object qualities;
            object timestamp;
            int return_value = 0;
            //return 0;
            ObjOPCGroups.GetOPCGroup("ReadGroup").SyncRead((short)OPCAutomation.OPCDataSource.OPCDevice,
                                                tagIndexReader - 1,
                                                ref readerItemServerHandles,
                                                out ItemServerReadValues,
                                                out readerItemServerErrors,
                                                out qualities,
                                                out timestamp);
            string message = "";
            for (int i = 1; i <= Globals.GetTotalNumberOfOutputTags(); i++)
            {
                int index = Globals.OUTPUT_TAGS.FirstOrDefault(x => x.Name == index_name).Index;
                if (index == i) return_value = (int)ItemServerReadValues.GetValue(i);
            }
            return return_value;
        }

        public void writeNewOrder(Order order)
        {
            try
            {

                int index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "ORDER_ID").Index;
                OrderServerWriteValues.SetValue(order.id, index);
                index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "ORDER_STATUS").Index;
                OrderServerWriteValues.SetValue(order.status, index);
                index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "TRAY_NUMBER").Index;
                OrderServerWriteValues.SetValue(order.tray_number, index);

                if (order.products != null)
                {
                    try
                    {
                        int productIndex = 1;
                        foreach (Product product in order.products)
                        {
                            index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "DRINK_" + productIndex + "_ID").Index;
                            OrderServerWriteValues.SetValue(product.id, index);
                            index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "DRINK_" + productIndex + "_ICE").Index;
                            OrderServerWriteValues.SetValue(product.ice, index);
                            index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "DRINK_" + productIndex + "_PREP").Index;
                            OrderServerWriteValues.SetValue(product.drink.prep, index);
                            index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "DRINK_" + productIndex + "_QUANTITY").Index;
                            OrderServerWriteValues.SetValue(product.quantity, index);
                            index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "DRINK_" + productIndex + "_STATUS").Index;
                            OrderServerWriteValues.SetValue(product.quantity, index);

                            if (product.garnishes != null) {
                                int garnishIndex = 1;
                                foreach (Garnish garnish in product.garnishes)
                                {
                                    index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "DRINK_" + productIndex + "_GARNISH_" + garnishIndex + "_ID").Index;
                                    OrderServerWriteValues.SetValue(garnish.id, index);
                                    index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "DRINK_" + productIndex + "_GARNISH_" + garnishIndex + "_RATIO").Index;
                                    OrderServerWriteValues.SetValue(garnish.ratio, index);
                                    garnishIndex++;
                                }
                            }

                            index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "DRINK_" + productIndex + "_GLASS_ID").Index;
                            OrderServerWriteValues.SetValue(product.drink.glass.id, index);
                            index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "DRINK_" + productIndex + "_GLASS_STATUS").Index;
                            OrderServerWriteValues.SetValue(product.drink.glass.status, index);

                            int ingredientIndex = 1;
                            foreach (Ingredient ingredient in product.drink.ingredients)
                            {
                                index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "DRINK_" + productIndex + "_INGREDIENT_" + ingredientIndex + "_ID").Index;
                                OrderServerWriteValues.SetValue(ingredient.ingredientDetail.id, index);
                                index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "DRINK_" + productIndex + "_INGREDIENT_" + ingredientIndex + "_PLACE_NO").Index;
                                OrderServerWriteValues.SetValue(ingredient.ingredientDetail.place_number, index);
                                index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "DRINK_" + productIndex + "_INGREDIENT_" + ingredientIndex + "_UNIT").Index;
                                OrderServerWriteValues.SetValue(ingredient.ratio_ml, index);
                                ingredientIndex++;
                            }
                            productIndex++;
                        }
                    }
                    catch (Exception ex)
                    {

                        throw new Exception("Error filling up indices for tags: " + ex);
                    }

                }
                //index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "QR_CODE").Index;
                //OrderServerWriteValues.SetValue(order.qr_code, index);

                try
                {
                    ObjOPCGroups.GetOPCGroup("WriteGroup").SyncWrite(tagIndexWriter - 1, 
                        ref writerItemServerHandles, 
                        ref OrderServerWriteValues, 
                        out writerItemServerErrors);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
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
            foreach (var output_tag in Globals.OUTPUT_TAGS)
            {
                readerOPCItemIDs.SetValue(output_tag.Value, tagIndexReader++);
            }

        }

        public void AddWriteGroupTags()
        {

            foreach (var input_tag in Globals.INPUT_TAGS)
            {
                 writerOPCItemIDs.SetValue(input_tag.Value, tagIndexWriter++);
            }
        }

        public OPCGroup SetupOPCReadGroup(string name)
        {
            OPCGroup _ObjOPCGroup;
            _ObjOPCGroup = ObjOPCGroups.Add(name);
            //_ObjOPCGroup.DataChange += new DIOPCGroupEvent_DataChangeEventHandler(ObjOPCGroup_DataChange);
            _ObjOPCGroup.UpdateRate = 200;
            _ObjOPCGroup.IsActive = true;
            _ObjOPCGroup.IsSubscribed = true;
            _ObjOPCGroup.OPCItems.AddItems(tagIndexReader - 1, 
                                           ref readerOPCItemIDs, 
                                           ref readerClientHandles, 
                                           out readerItemServerHandles, 
                                           out readerItemServerErrors, 
                                           readerRequestedDataTypes, 
                                           readerAccessPaths);
            return _ObjOPCGroup;
        }

        public OPCGroup SetupOPCWriteGroup(string name)
        {
            OPCGroup _ObjOPCGroup;
            _ObjOPCGroup = ObjOPCGroups.Add(name);
            _ObjOPCGroup.UpdateRate = 200;
            _ObjOPCGroup.IsActive = true;
            _ObjOPCGroup.IsSubscribed = true;
            _ObjOPCGroup.OPCItems.AddItems(tagIndexWriter, ref writerOPCItemIDs, ref writerClientHandles, out writerItemServerHandles, out writerItemServerErrors, writerRequestedDataTypes, writerAccessPaths);

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
            try
            {
                if (CurrentPLCOutput == null) CurrentPLCOutput = new PLCOutput();

                while (!stopThreads)
                {
                    //DA SE NAJDE DOBAR NACIN ZA CITANJE NA TAGOVITE SO SERVER HANDLES
                    ObjOPCGroup.SyncRead((short)OPCAutomation.OPCDataSource.OPCDevice, 
                                    tagIndexReader - 1, 
                                    ref readerItemServerHandles, 
                                    out ItemServerReadValues, 
                                    out readerItemServerErrors, 
                                    out qualities, 
                                    out timestamp);
                    string message = "";
                    for (int i = 1; i <= Globals.GetTotalNumberOfOutputTags(); i++)
                    {
                        int index = Globals.OUTPUT_TAGS.FirstOrDefault(x => x.Name == "iPLC_STATUS").Index;
                        if (index == i) CurrentPLCOutput.iPlc_Status = (int)ItemServerReadValues.GetValue(i);

                        //vaka i za drugite nadolu, nemoj so switch

                        //switch (i)
                        //{
                        //    case CurrentPLCOutput.indexes.IndexOf("iPLC_STATUS"):
                        //        CurrentPLCOutput.iPlc_Status = (int)ItemServerReadValues.GetValue(i);
                        //        break;
                        //    //case 2:
                        //    //    CurrentTestOutput.output_bool = (bool)ItemServerReadValues.GetValue(i);
                        //    //    break;
                        //    //case 4:
                        //    //    CurrentTestOutput.output_int = (int)ItemServerReadValues.GetValue(i);
                        //    //    break;
                        //    //case 5:
                        //    //    CurrentTestOutput.output_random = (int)ItemServerReadValues.GetValue(i);
                        //    //    break;
                        //    default:
                        //        break;
                        //}
                        Workflow.plcOutputs.Add(CurrentPLCOutput);
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
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }

        }

        void WriteItems(OPCGroup ObjOPCGroup)
        {
            try
            {
                while (!stopThreads)
                {

                    if (Workflow.plcInputs != null && Workflow.plcInputs.Count > 0)
                    {
                        CurrentPLCInput = Workflow.plcInputs.First();
                        if (previousPLCInput == null || !previousPLCInput.Equals(CurrentPLCInput))
                        {
                            int index = Globals.INPUT_TAGS.FirstOrDefault(x => x.Name == "iPLC_STATUS").Index;
                            ItemServerWriteValues.SetValue(CurrentPLCInput.iPlc_Status, index);
                            // ItemServerWriteValues.SetValue(CurrentTestInput.input_bool, 1);
                            // ItemServerWriteValues.SetValue(CurrentTestInput.input_int, 2);
                            try
                            {
                                ObjOPCGroup.SyncWrite(tagIndexWriter-1, ref writerItemServerHandles, ref ItemServerWriteValues, out writerItemServerErrors);

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                throw;
                            }
                            previousPLCInput = CurrentPLCInput;
                        }
                    }

                    //ItemServerWriteValues.SetValue(1.1, Array.IndexOf(OPCItemIDs,(object)("Bucket Brigade.Real1")));
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }
           
        }


    }
}
