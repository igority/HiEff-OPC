using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;

namespace OPCtoMongoDBService.Models
{
    public class IngredientDetail
    {
        public int? id { get; set; }
        public int? place_number { get; set; }

        public IngredientDetail(BsonDocument result)
        {
            id = result["id"].AsNullableInt32;
            place_number = result["place_number"].AsNullableInt32;
        }

    }
}
