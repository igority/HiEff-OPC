using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;

namespace OPCtoMongoDBService.Models
{
    public class PLCOutput
    {
        public ObjectId? id { get; set; }
        public long? iPlc_Status { get; set; }

        public PLCOutput(BsonDocument result)
        {
            id = result["_id"].AsObjectId;
            if (!result["iPLC_STATUS"].IsBsonNull) iPlc_Status = result["iPLC_STATUS"].AsInt64;
            else iPlc_Status = null;
        }

        public PLCOutput()
        {
        }
    }
}
