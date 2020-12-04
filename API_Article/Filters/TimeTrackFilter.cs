using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article.Filters
{
    public class TimeTrackFilter : Attribute, IActionFilter
    {
        private Stopwatch _stopwatch;


        public void OnActionExecuted(ActionExecutedContext context)
        {
            _stopwatch.Stop();

            var milisec = _stopwatch.ElapsedMilliseconds;
            var action = context.ActionDescriptor.DisplayName; // jaka została wykonana akcja w jakim czasie

            Debug.WriteLine($"Akcja {action}, czas : {milisec} milisec");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }
    }
}
