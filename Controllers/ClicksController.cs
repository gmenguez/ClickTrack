namespace ClickTrack.Controllers
{
    using System.Collections.Generic;
    using ClickTrack.Model;
    using ClickTrack.Repository;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("clicks")]
    public class ClicksController : ControllerBase
    {
        private readonly ILogger<ClicksController> m_logger;
        private readonly IClickRepository m_clickRepository;

        public ClicksController(ILogger<ClicksController> logger, IClickRepository clickRepository)
        {
            m_logger = logger;
            m_clickRepository = clickRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Click> Get([FromQuery] int limit, [FromQuery] int page)
        {
            if (limit <= 0 || page <= 0)
            {
                return m_clickRepository.GetAll();
            }

            return m_clickRepository.GetByPage(limit, page);
        }

        [HttpGet("{**url}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Click> GetByUrl(string url)
        {
            Click click = m_clickRepository.GetByUrl(url);
            if (click != null)
            {
                return click;
            }

            return NotFound();
        }
    }
}
