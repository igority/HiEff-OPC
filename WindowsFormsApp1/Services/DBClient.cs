using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WindowsFormsApp1.Models;
using WindowsFormsApp1.Workers;

namespace WindowsFormsApp1.Services
{
    class DBClient
    {

        IMongoClient _client;
        IMongoDatabase _database;
        public List<TestOutput> testOutputs;
        public List<TestInput> testInputs;
        Thread dbWriter;
        Thread dbReader;
        //SshClient client;
        //ForwardedPortLocal port;


        public DBClient()
        {

            ConnectToDB();
            testInputs = GetTestInputs();
            testOutputs = GetTestOutputs();
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

        public List<TestInput> GetTestInputs()
        {
            List<TestInput> _testInputs = new List<TestInput>();
            var collection = _database.GetCollection<BsonDocument>("Test-Input");
            var filter = new BsonDocument();
            var results = collection.Find(filter).Limit(100).ToList();
            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    TestInput testInput = new TestInput(result);
                    _testInputs.Add(testInput);
                }
            }
            return _testInputs;
        }

        public List<TestOutput> GetTestOutputs()
        {
            List<TestOutput> _testOutputs = new List<TestOutput>();
            var collection = _database.GetCollection<BsonDocument>("Test-Output");
            var filter = new BsonDocument();
            var results = collection.Find(filter).Limit(100).ToList();
            if (results.Count > 0)
            {
                foreach (var result in results)
                {
                    TestOutput testOutput = new TestOutput(result);
                    _testOutputs.Add(testOutput);
                }
            }
            return _testOutputs;
        }

        public void updateTestOutput(TestOutput testOutput)
        {
            var collection = _database.GetCollection<BsonDocument>("Test-Output");
            var filter = Builders<BsonDocument>.Filter.Eq("id", testOutput.id);
            var update = Builders<BsonDocument>.Update.Set("output_bool", testOutput.output_bool).Set("output_int", testOutput.output_int).Set("output_random", testOutput.output_random);
            //var update = Builders<BsonDocument>.Update.Set("order_status", order.order_status);
            var result = collection.UpdateMany(filter, update);
        }

        public void updateTestInput(TestInput testInput)
        {
            var collection = _database.GetCollection<BsonDocument>("Test-Input");
            var filter = Builders<BsonDocument>.Filter.Eq("id", testInput.id);
            var update = Builders<BsonDocument>.Update.Set("input_bool", testInput.input_bool).Set("input_int", testInput.input_int);
            //var update = Builders<BsonDocument>.Update.Set("order_status", order.order_status);
            var result = collection.UpdateMany(filter, update);
        }

        void dbWrite()
        {
            while (true)
            {
                Thread.Sleep(200);
                if (Workflow.testOutputs != null)
                {
                    for (int i = 0; i < Workflow.testOutputs.Count; i++)
                    {
                        updateTestOutput(Workflow.testOutputs[i]);
                    }
                }
            }
        }

        void dbRead()
        {
            while (true)
            {
                Thread.Sleep(200);
                Workflow.testInputs = GetTestInputs();
                //if (Workflow.testInputs != null)
                //{
                //    for (int i = 0; i < Workflow.testOutputs.Count; i++)
                //    {
                //        updateTestOutput(Workflow.testOutputs[i]);
                //    }
                //}
            }
        }


    }

}
