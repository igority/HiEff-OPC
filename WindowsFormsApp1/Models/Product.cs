using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;

namespace OPCtoMongoDBService.Models
{
    public class Product
    {
        public int? id { get; set; }
        public Drink drink { get; set; }
        public int? ice { get; set; }
        public List<Garnish> garnishes { get; set; }
        public int? quantity { get; set; }
        public string amount { get; set; }
        public int? status { get; set; }

        public Product(BsonDocument result)
        {
            id = result["id"].AsNullableInt32;
            BsonDocument bsonDrink = result["drink"].AsBsonDocument;
            if (bsonDrink != BsonNull.Value)
            {
                drink = new Drink(bsonDrink);
            } else
            {
                drink = null;
            }
            ice = result["ice"].AsNullableInt32;
            var bsonGarnishes = result["garnishes"].AsBsonArray;
            if (bsonGarnishes.Count > 0)
            {

                garnishes = new List<Garnish>();
                foreach (var bsonGarnish in bsonGarnishes)
                {
                    Garnish garnish = new Garnish(bsonGarnish.AsBsonDocument);
                    garnishes.Add(garnish);
                }

            }

            quantity = result["quantity"].AsNullableInt32;
            amount = result["amount"].AsString;
            status = result["status"].AsNullableInt32;

            ////creation_date = new BsonDateTime(result["creation_date"]);
            //creation_date = DateTime.Parse(result["creation_date"].ToString());
        }

    }
}
