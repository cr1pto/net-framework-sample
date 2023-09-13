using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TimeCards.Lib;

namespace TimeCards.Web.Controllers
{
    public class TimeCardsController : ApiController
    {
        private readonly ITimeTrackingService timeTrackingService;

        public TimeCardsController(ITimeTrackingService timeTrackingService)
        {
            this.timeTrackingService = timeTrackingService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> Get(CancellationToken cancellationToken) {
            var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            IEnumerable<TimeCard> timeCards = await this.timeTrackingService.GetTimecards(cancellationToken);
            return Ok(timeCards);
        }

    }
}