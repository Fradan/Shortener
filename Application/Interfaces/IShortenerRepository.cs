using Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application
{
    public interface IShortenerRepository
    {
        Task CreateShortenerAsync(Shortener shortener);

        Task<Shortener> GetByBackHalf(string shortLink);

        Task<List<Shortener>> GetAllAsync();
    }
}
