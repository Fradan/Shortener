using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public interface IShortenerRepository
    {
        Task CreateShortenerAsync(Shortener shortener);

        Task<Shortener> GetByShortLinkAsync(string shortLink);

        Task<List<Shortener>> GetAllAsync();
    }
}
