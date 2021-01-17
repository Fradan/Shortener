using Application;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Shorter.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class LinkController : ControllerBase
    {
        private readonly IShortenerService _linkService;

        public LinkController(IShortenerService linkService)
        {
            _linkService = linkService;
        }

        /// <summary>
        /// создание сокращенной ссылки по полной
        /// </summary>
        /// <param name="sourceLink"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<string>> ShorterLink([FromForm]string sourceLink)
        {
            var shorterLink = await _linkService.CreateShortenerAsync(sourceLink);
            return shorterLink;
        }

        /// <summary>
        /// получение списка всех сокращенных ссылок с количеством переходов
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> ShortenerList()
        {
            var shortenerList = await _linkService.GetShortenerListAsync();
            throw new NotImplementedException();
        }

        /// <summary>
        /// получение оригинала по сокращенной, с увеличением счетчика посещений
        /// </summary>
        /// <param name="shorterLink"></param>
        /// <returns></returns>
        [HttpGet("{shorterLink}")]
        public async Task<IActionResult> SourceLink(string shorterLink)
        {
            var sourceLink = await _linkService.GetSourceLinkAsync(shorterLink);
            return RedirectPermanent(sourceLink);
        }
       
    }
}
