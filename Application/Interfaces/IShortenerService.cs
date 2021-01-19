using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public interface IShortenerService
    {
        public Task<string> CreateShortenerAsync(string sourceLink, string appUrl);

        public Task<List<Shortener>> GetShortenerListAsync();

        public Task<string> GetSourceLinkAsync(string shortetLink);
    }
}
