using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OPCtoMongoDBService.Models;
using OPCtoMongoDBService.Workers;

namespace OPCtoMongoDBService.Services
{
    class DBClient
    {

        IMongoClient _client;
        IMongoDatabase _database;
        public List<PLCOutput> plcOutputs;
        public List<PLCInput> plcInputs;
        Thread dbWriter;
        Thread dbReader;
        //SshClient client;
        //ForwardedPortLocal port;


        public DBClient()
        {

            ConnectToDB();
            plcInputs = GetPLCInputs();
            plcOutputs = GetPLCOutputs();
            // vendors =GetVendors();

            dbWriter = new Thread(dbWrite);
            dbWriter.Start();

            dbReader = new Thread(dbRead);
            dbReader.Start();
        }

        void ConnectToDB()
        {

            // Start("hiefficiency.srmtechsol.com", "root", "Welcome@321", 22);
            _client = new MongoClient();
            _database = _client.GetDatabase("Hi-Eff");
            //_database = _client.GetDatabase("hiefficiency");
            //Stop();

        }

        public List<PLCInput> GetPLCInputs()
        {
            List<PLCInput> _plcInputs = new List<PLCInput>();
            var collection = _database.GetCollection<BsonDocument>("PLC_inputs");
            var filter = new BsonDocument();
            var results = collection.Find(filter).Limit(100).ToList();
            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    PLCInput plcInput = new PLCInput(result);
                    _plcInputs.Add(plcInput);
                }
            }
            return _plcInputs;
        }

        public List<PLCOutput> GetPLCOutputs()
        {
            List<PLCOutput> _plcOutputs = new List<PLCOutput>();
            var collection = _database.GetCollection<BsonDocument>("PLC_outputs");
            var filter = new BsonDocument();
            var results = collection.Find(filter).Limit(100).ToList();
            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    PLCOutput plcOutput = new PLCOutput(result);
                    _plcOutputs.Add(plcOutput);
                }
            }
            return _plcOutputs;
        }

        public void updatePLCOutput(PLCOutput plcOutput)
        {
            var collection = _database.GetCollection<BsonDocument>("PLC_outputs");
            var filter = Builders<BsonDocument>.Filter.Eq("id", plcOutput.id);
            var update = Builders<BsonDocument>.Update.Set("iPLC_STATUS", plcOutput.iPlc_Status);
            //  .Set("output_int", testOutput.output_int)
            //  .Set("output_random", testOutput.output_random);
            //var update = Builders<BsonDocument>.Update.Set("order_status", order.order_status);
            var result = collection.UpdateMany(filter, update);
        }

        public void updatePLCInput(PLCInput plcInput)
        {
            var collection = _database.GetCollection<BsonDocument>("PLC_inputs");
            var filter = Builders<BsonDocument>.Filter.Eq("id", plcInput.id);
        }

        void dbWrite()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(200);
                    if (Workflow.plcOutputs != null)
                    {
                        for (int i = 0; i < Workflow.plcOutputs.Count; i++)
                        {
                            updatePLCOutput(Workflow.plcOutputs[i]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                throw;
            }

        }

        void dbRead()
        {
            try
            {
                while (true)
                {
                    Thread.Sleep(200);
                    Workflow.plcInputs = GetPLCInputs();
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
