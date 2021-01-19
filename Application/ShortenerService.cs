using Application.Helpers;
using Core;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application
{
    public class ShortenerService : IShortenerService
    {
        private readonly IShortenerRepository _shortenerRepository;

        public ShortenerService(IShortenerRepository shotenerRepository)
        {
            _shortenerRepository = shotenerRepository;
        }

        public async Task<List<Shortener>> GetShortenerListAsync()
        {
            var shortenerList = await _shortenerRepository.GetAllAsync()
                ?? new List<Shortener>(0);

            return shortenerList;
        }

        public async Task<string> CreateShortenerAsync(string sourceLink, string appUrl)
        {
            if (sourceLink == null || !LinkHelper.IsLinkValid(sourceLink))
            {
                throw new BusinessRuleValidationException("Некорректный URL.");
            }

            string backHalf;
            do
            {
                backHalf = LinkHelper.GenerateBackHalf(sourceLink);
            }
            while ((await _shortenerRepository.GetByBackHalf(backHalf)) != null);

            var shortener = Shortener.Create(sourceLink, appUrl, backHalf);

            await _shortenerRepository.CreateShortenerAsync(shortener);
            return shortener.ShortLink;
        }

        public async Task<string> GetSourceLinkAsync(string backHalf)
        {
            var shortener = await _shortenerRepository.GetByBackHalf(backHalf);
            return shortener?.SourceLink;
        }
    }
}
