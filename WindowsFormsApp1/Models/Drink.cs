using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;

namespace OPCtoMongoDBService.Models
{
    public class Drink
    {
        public int? id { get; set; }
        public List<Ingredient> ingredients { get; set; }
        public int? prep { get; set; }
        public Glass glass { get; set; }


        public Drink(BsonDocument result)
        {
            id = result["id"].AsNullableInt32;
            BsonArray bsonIngredients = result["ingredients"].AsBsonArray;
            if (bsonIngredients.Count > 0)
            {
                ingredients = new List<Ingredient>();
                foreach (var bsonIngredient in bsonIngredients)
                {
                    try
                    {
                        Ingredient ingredient = new Ingredient(bsonIngredient.AsBsonDocument);
                        ingredients.Add(ingredient);
                    }
                    catch (Exception ex)
                    {

                        throw new Exception("Error loading ingredients: " + ex.Message);
                    }

                }

            }
            prep = result["prep"].AsNullableInt32;
            if (result["glass"] != BsonNull.Value)
            {
                glass = new Glass(result["glass"].AsBsonDocument);
            }
        }

    }
}
