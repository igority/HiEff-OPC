﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;

namespace OPCtoMongoDBService.Models
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

        public bool Equals(PLCInput _plcInput)
        {
            if (this.iPlc_Status != _plcInput.iPlc_Status) return false;
            //site ostanati vamu
            return true;
        }
    }
}
