using System;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoSample.Models
{
    public class Contact
    {
        [BsonId]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public DateTime LastModified { get; set; }
    }
}
