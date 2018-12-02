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
        public IngredientDetail ingredientDetail { get; set; }
        public double? ratio_ml { get; set; }

        public Ingredient(BsonDocument result)
        {
            ratio_ml = result["ratio_ml"].AsNullableDouble;
            try
            {
                ingredientDetail = new IngredientDetail(result["ingredient"].AsBsonDocument);
            }
            catch (Exception ex)
            {

                throw;
            }
  
            
           
            ////creation_date = new BsonDateTime(result["creation_date"]);
            //creation_date = DateTime.Parse(result["creation_date"].ToString());
        }

    }
}
