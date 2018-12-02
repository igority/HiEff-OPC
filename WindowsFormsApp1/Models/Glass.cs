﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;

namespace OPCtoMongoDBService.Models
{
    public class Glass
    {
        public int id { get; set; }
        public int status { get; set; }

        public Glass(BsonDocument result)
        {
            id = result["id"].AsInt32;
            status = result["status"].AsInt32;

        }

    }
}
