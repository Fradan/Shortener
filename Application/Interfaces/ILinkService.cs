using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public interface ILinkService
    {
        public Task<string> GetShorterLinkAsync(string sourceLink);

        public Task<List<Shortener>> GetShortenerListAsync();

        public Task GetSourceLinkAsync(string shortetLink);
    }
}
