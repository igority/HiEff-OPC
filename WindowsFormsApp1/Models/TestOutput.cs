using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
    class TestOutput
    {
        public ObjectId? id { get; set; }
        public bool? output_bool { get; set; }
        public int? output_int { get; set; }
        public int? output_random { get; set; }

        public TestOutput(BsonDocument result)
        {

            id = result["_id"].AsObjectId;
            if (!result["output_bool"].IsBsonNull) output_bool = result["output_bool"].AsBoolean;
            else output_bool = null;
            if (!result["output_int"].IsBsonNull) output_int = result["output_int"].AsInt32;
            else output_int = null;
            if (!result["output_random"].IsBsonNull) output_random = result["output_random"].AsInt32;
            else output_random = null;

            //output_int = result["output_int"].AsInt32;
            //output_random = result["output_random"].AsInt32;
        }

        public TestOutput()
        {
            id = null;
            output_bool = null;
            output_int = null;
            output_random = null;
        }
    }
}
