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
        private readonly ILinkService _linkService;

        public LinkController(ILinkService linkService)
        {
            _linkService = linkService;
        }

        [HttpPost]
        public async Task<ActionResult<string>> ShorterLink(string sourceLink)
        {
            var shorterLink = await _linkService.GetShorterLinkAsync(sourceLink);
            return shorterLink;
        }

        [HttpGet]
        public async Task<ActionResult> ShortenerList()
        {
            var shortenerList = await _linkService.GetShortenerListAsync();
            throw new NotImplementedException();
        }

        [HttpGet("{shorterLink}")]
        public async Task<IActionResult> SourceLink(string shorterLink)
        {
            var sourceLink = await _linkService.GetSourceLinkAsync(shorterLink);
            return RedirectPermanent(sourceLink);
        }
       
    }
}
