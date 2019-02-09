using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace OPCtoMongoDBService.Models
{
    public class Order
    {
        [BsonId]
        public ObjectId _id { get; set; }
        [BsonElement("id")]
        public int? id { get; set; }
        [BsonElement("creation_date")]
        public DateTime creation_date { get; set; }
        [BsonElement("tray_number")]
        public int? tray_number { get; set; }
        [BsonElement("products")]
        public List<Product> products { get; set; }
        [BsonElement("qr_code")]
        public string qr_code { get; set; }
        [BsonElement("status")]
        public int? status { get; set; }
        [BsonElement("status_view")]
        public string status_view { get; set; }




        public Order(BsonDocument result)
        {
            _id = result["_id"].AsObjectId;
            id = result["id"].AsNullableInt32;
            //creation_date = new BsonDateTime(result["creation_date"]);
            creation_date = DateTime.Parse(result["creation_date"].AsString);
            tray_number = result["tray_number"].AsNullableInt32;
            qr_code = result["qr_code"].AsString;
            status = result["status"].AsNullableInt32;
            products = Products(result["products"].AsBsonArray);
            //var bsonProducts = result["products"].AsBsonArray;
            //if (bsonProducts.Count > 0)
            //{
            //    products = new List<Product>();
            //    foreach (var bsonProduct in bsonProducts)
            //    {
            //        Product product = new Product(new BsonDocument(bsonProduct.AsBsonDocument));
            //        products.Add(product);
            //    }
            //}
        }

        public static List<Product> Products(BsonArray productsArray)
        {
            var products = new List<Product>();
            if (productsArray.Count > 0)
            {
                foreach (var productEl in productsArray)
                {
                    Product product = new Product(new BsonDocument(productEl.AsBsonDocument));
                    products.Add(product);
                }
            }
            return products;
        }

    }
}
