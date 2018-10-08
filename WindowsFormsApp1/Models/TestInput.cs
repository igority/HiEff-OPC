using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MongoDB.Bson;
using MongoDB.Driver;

namespace OPCtoMongoDBService.Models
{
    public class TestInput
    {
        public ObjectId id { get; set; }
        public bool input_bool { get; set; }
        public int input_int { get; set; }

        public TestInput(BsonDocument result)
        {
            id = result["_id"].AsObjectId;
            input_bool = result["input_bool"].AsBoolean;
            input_int = result["input_int"].AsInt32;
        }
    }
}
