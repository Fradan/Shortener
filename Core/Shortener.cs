
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Core
{
    public class Shortener
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id;

        public string ShortLink;

        public string SourceLink;

        public string BackHalf;

        public int Counter;
    }
}
