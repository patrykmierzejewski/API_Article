using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article.Filters
{
    public class TimeTrackFilter : IActionFilter
    {
        private Stopwatch _stopwatch;
        private readonly ILogger _logger;

        public TimeTrackFilter(ILogger<TimeTrackFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();

            var milisec = _stopwatch.ElapsedMilliseconds;
            var action = context.ActionDescriptor.DisplayName; // jaka została wykonana akcja w jakim czasie

            _logger.LogInformation($"Akcja {action}, czas : {milisec} milisec");
            //Debug.WriteLine($"Akcja {action}, czas : {milisec} milisec");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }
    }
}
