using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Task1
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string FirstName { get; set; }
        [BsonElement("second_name")]
        public string LastName { get; set; }
        [BsonElement("username")]
        public string UserName { get; set; }
        [BsonElement("password")]
        public string Password { get; set; }
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("date_of_birth")]
        public DateTime BirthDate { get; set; }
        [BsonElement("hobby")]
        public List<string> Interests { get; set; }
        [BsonElement("follows")]
        public List<string> Follows { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} {UserName}";
        }
    }
}
