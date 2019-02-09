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
        public double? ratio { get; set; }


        public Garnish(BsonDocument result)
        {
            id = result["id"].AsNullableInt32;
            ratio = result["ratio"].AsNullableDouble;

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
