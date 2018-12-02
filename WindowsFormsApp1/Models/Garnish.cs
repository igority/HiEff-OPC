using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;

namespace OPCtoMongoDBService.Models
{
    public class Garnish
    {
        public int? id { get; set; }
        //public GarnishDetail garnishDetail { get; set; }
        public int? ratio { get; set; }


        public Garnish(BsonDocument result)
        {
            id = result["id"].AsNullableInt32;
            id = result["ratio"].AsNullableInt32;

            //try
            //{
            //    garnishDetail = new GarnishDetail(result["garnish"].AsBsonDocument);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}

        }

    }
}
