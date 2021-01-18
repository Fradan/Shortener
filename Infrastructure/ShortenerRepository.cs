using Application;
using Core;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ShortenerRepository : IShortenerRepository
    {
        IGridFSBucket gridFS;
        IMongoCollection<Shortener> Shorteners;

        public ShortenerRepository()
        {
            string connectionString = "mongodb://localhost:27017/ShortenerDB";
            var connection = new MongoUrlBuilder(connectionString);

            MongoClient client = new MongoClient(connectionString);

            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);

            gridFS = new GridFSBucket(database);

            Shorteners = database.GetCollection<Shortener>("Shorteners");
        }

        public async Task CreateShortenerAsync(Shortener shortener)
        {
            await Shorteners.InsertOneAsync(shortener);
        }

        public async Task<List<Shortener>> GetAllAsync()
        {
            return await Shorteners.Find(_ => true)
                .ToListAsync();
        }

        public async Task<Shortener> GetByBackHalf(string backHalf)
        {
            var record = await Shorteners.FindOneAndUpdateAsync(
                Builders<Shortener>.Filter.Where(rec => rec.BackHalf == backHalf),
                Builders<Shortener>.Update.Inc(rec => rec.Counter, 1),
                options: new FindOneAndUpdateOptions<Shortener> 
                { 
                    ReturnDocument = ReturnDocument.After,
                    IsUpsert = false
                });

            return record;
        }
    }
}
