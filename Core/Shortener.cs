
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

        private Shortener()
        {
            
        }

        public static Shortener Create(string sourceLink, string targetUrl, string backHalf)
        {
            var shortener = new Shortener()
            {
                BackHalf = backHalf,
                SourceLink = CheckCompleteLink(sourceLink),
                ShortLink = $"{targetUrl}/{backHalf}"
            };
            return shortener;
        }

        private static string CheckCompleteLink(string sourceLink)
        {
            if (!sourceLink.Contains("https://") || !sourceLink.Contains("http://"))
            {
                return $"https://{sourceLink}";
            }
            return sourceLink;
        }
    }
}
