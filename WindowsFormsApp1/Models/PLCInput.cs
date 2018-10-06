using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.Models
{
    public class PLCInput
    {
        public ObjectId id { get; set; }
        public long iPlc_Status { get; set; }

        public PLCInput(BsonDocument result)
        {
            id = result["_id"].AsObjectId;
            iPlc_Status = result["iPLC_STATUS"].AsInt64;
        }
    }
}
