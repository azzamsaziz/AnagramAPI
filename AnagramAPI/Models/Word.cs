using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnagramAPI.Models
{
    public class Word
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Text { get; set; }
    }
}