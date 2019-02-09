using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OPCtoMongoDBService.Models;
using OPCtoMongoDBService.Services;
using WindowsFormsApp1.Services;

namespace OPCtoMongoDBService.Workers
{
    class Workflow
    {
        private bool IsBusy;
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
            plcInputs = InitializePlcInputs();
            plcOutputs = InitializePlcOutputs();
            opcClient.startReader(opcClient.readGroup);
            opcClient.startWriter(opcClient.writeGroup);
            Master.Start();
        }

        private List<PLCOutput> InitializePlcOutputs()
        {
            return new List<PLCOutput>();
        }

        private List<PLCInput> InitializePlcInputs()
        {
            return new List<PLCInput>();
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
                    // DELAY THE LOOP FOR 100 ms
                    Thread.Sleep(100);
                    if (IsBusy) break;
                    IsBusy = true;
                    Order orderDb = dbClient.GetCurrentOrder();
                    int orderIdDb = 0;
                    if (orderDb != null)  orderIdDb = (int)orderDb.id;
                    int orderIdOPC = opcClient.GetCurrentOrderId();

                    if (orderIdDb != 0)
                    {
                        //There is an order in the database
                        if (orderIdOPC != 0)
                        {
                            //OPC has been processing an order.
                            //compare if it is the same order in queue. 
                            if (orderIdDb == orderIdOPC)
                            {

                                //Update status from opc to db
                                // int orderStatusOPC = opcClient.GetCurrentOrderStatus();

                                List<OrderUserDTO> ordersUserDTO = opcClient.GetCurrentOrderStatuses();
                                dbClient.updateOrderStatus(ordersUserDTO.First().order, orderIdDb);
                                //update status on outside API
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
                                //todo populate orderUserDTO
                                foreach (var order in ordersUserDTO)
                                {
                                    if (ChangeStatusDetected(order, orderDb))
                                    {
                                        dbClient.updateDrinkStatus(order);
                                        UpdateUserAPI(order);
                                    }
                                }
                                

                            } else
                            {
                                //this order has been completed. Update OPC with a new order
                                opcClient.writeNewOrder(orderDb);
                            }
                        } else 
                        {
                            //OPC is not processing any orders.
                            //TODO - Update OPC with a new order
                            opcClient.writeNewOrder(orderDb);
                        }
                    } else
                    {
                        //no orders in queue. Do nothing
                    }


                    IsBusy = false;
                    //CHECK IF ALL CONDITIONS ARE OK, IF NO - GO TO TOP OF LOOP
                    if (!checkConditions()) continue;

                    // dbClient.updatePLCOutput(opcClient.CurrentPLCOutput);

                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //throw;
            }

        }

        private bool ChangeStatusDetected(OrderUserDTO order, Order orderDb)
        {
            //find the drink and check if status is changed
            foreach (var product in orderDb.products)
            {
                if (product.id == order.drink)
                {
                    //found the drink, now compare statuses
                    if (product.status != order.status_drink)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private void UpdateUserAPI(OrderUserDTO orderUserDTO)
        {
            string json = JsonConvert.SerializeObject(orderUserDTO);
            UserAPIService userAPIService = new UserAPIService();
            var updateSuccess = userAPIService.UpdateOrder(json);
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
