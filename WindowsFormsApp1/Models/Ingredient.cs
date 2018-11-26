using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;

namespace OPCtoMongoDBService.Models
{
    public class Ingredient
    {
        public IngredientDetail ingredient { get; set; }
        public int ratio_ml { get; set; }

        public Ingredient(BsonDocument result)
        {
            //_id = result["_id"].AsObjectId;
            //id = result["id"].AsInt32;
            ////creation_date = new BsonDateTime(result["creation_date"]);
            //creation_date = DateTime.Parse(result["creation_date"].ToString());
        }

    }
}
