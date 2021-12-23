namespace ClickTrack.Controllers
{
    using ClickTrack.Repository;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("track")]
    public class TrackController : ControllerBase
    {
        private readonly ILogger<TrackController> m_logger;
        private readonly IClickRepository m_clickRepository;

        public TrackController(ILogger<TrackController> logger, IClickRepository clickRepository)
        {
            m_logger = logger;
            m_clickRepository = clickRepository;
        }

        [HttpGet("{**url}")]
        [ProducesResponseType(StatusCodes.Status301MovedPermanently)]
        public IActionResult Get(string url)
        {
            m_clickRepository.Add(url);
            return RedirectPermanent(url);
        }
    }
}
