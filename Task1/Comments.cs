using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;


namespace Task1
{
    public class Comment
    {
        [BsonElement("username")]
        public string UserName { get; set; }
        [BsonElement("text")]
        public string CommentText { get; set; }
        [BsonElement("date")]
        public DateTime CreationDate { get; set; }
        public override string ToString()
        {
            return $"\nusername: {UserName}   date: {CreationDate.ToShortDateString()}\n\n{CommentText}";
        }
    }
}
