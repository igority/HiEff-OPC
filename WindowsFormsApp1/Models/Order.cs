﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;

namespace OPCtoMongoDBService.Models
{
    public class Order
    {
        public ObjectId _id { get; set; }
        public int? id { get; set; }
        public DateTime creation_date { get; set; }
        public int? tray_number { get; set; }
        public List<Product> products { get; set; }
        public string qr_code { get; set; }
        public int? status { get; set; }
        
        


        public Order(BsonDocument result)
        {
            _id = result["_id"].AsObjectId;
            id = result["id"].AsNullableInt32;
            //creation_date = new BsonDateTime(result["creation_date"]);
            creation_date = DateTime.Parse(result["creation_date"].AsString);
            tray_number = result["tray_number"].AsNullableInt32;
            qr_code = result["qr_code"].AsString;
            status = result["status"].AsNullableInt32;
            var bsonProducts = result["products"].AsBsonArray;
            if (bsonProducts.Count > 0)
            {
                products = new List<Product>();
                foreach (var bsonProduct in bsonProducts)
                {
                    Product product = new Product(new BsonDocument(bsonProduct.AsBsonDocument));
                    products.Add(product);
                }
            }
        }

    }
}
