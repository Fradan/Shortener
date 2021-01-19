using Application;
using AutoMapper;
using Core;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShortenerApi
{
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly IShortenerService _linkService;
        private readonly IMapper _mapper;

        public LinkController(IShortenerService linkService, IMapper mapper)
        {
            _linkService = linkService;
            _mapper = mapper;
        }

        /// <summary>
        /// создание сокращенной ссылки по полной
        /// </summary>
        /// <param name="sourceLink"></param>
        /// <returns></returns>
        [Route("shorterLink"), HttpPost]
        public async Task<ActionResult<string>> ShorterLink([FromForm]string sourceLink)
        {
            if (sourceLink == null)
            {
                return BadRequest();
            }

            var shortLink = await _linkService.CreateShortenerAsync(sourceLink, $"{this.Request.Scheme}://{this.Request.Host}");
            return shortLink;
        }

        /// <summary>
        /// получение списка всех сокращенных ссылок с количеством переходов
        /// </summary>
        /// <returns></returns>
        [Route("[controller]"), HttpGet]
        public async Task<ActionResult<List<ShortenerDto>>> GetAll()
        {
            List<Shortener> shortenerList = await _linkService.GetShortenerListAsync();
            return _mapper.Map<List<ShortenerDto>>(shortenerList);
        }

        /// <summary>
        /// получение оригинала по сокращенной, с увеличением счетчика посещений
        /// </summary>
        /// <param name="backHalf"></param>
        /// <returns></returns>
        [Route("{backHalf}"), HttpGet]
        public async Task<IActionResult> SourceLink(string backHalf)
        {
            if (backHalf == null)
            {
                return BadRequest();
            }
                
            var sourceLink = await _linkService.GetSourceLinkAsync(backHalf);
            if (sourceLink != null)
            {
                return RedirectPermanent(sourceLink);
            }
            return NotFound();
            
        }
       
    }
}
