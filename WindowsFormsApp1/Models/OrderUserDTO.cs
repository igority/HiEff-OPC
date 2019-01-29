using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;

namespace OPCtoMongoDBService.Models
{
    public class OrderUserDTO
    {
        public int order { get; set; }
        public int robot { get; set; }
        public int status_drink { get; set; }
        public int drink { get; set; }
        public int? ingredient { get; set; }
    }
}
