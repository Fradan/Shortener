using Microsoft.AspNetCore.Mvc;
using System;

namespace Shorter.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class LinkController : ControllerBase
    {
        public LinkController()
        {
            
        }

        [HttpPost]
        public IActionResult ShoterLink(string sourceLink)
        {
            throw new NotImplementedException();
        }

        public IActionResult ShortenerLinkList()
        {
            throw new NotImplementedException();
        }

        public IActionResult SourceLink()
        {
            throw new NotImplementedException();
        }
       
    }
}
