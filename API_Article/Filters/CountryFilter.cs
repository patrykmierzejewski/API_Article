using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article.Filters
{
    public class CountryFilter : Attribute, IAuthorizationFilter
    {
        private string[] _countries;
        public CountryFilter(string countries)
        {
            _countries = countries.Split(",");
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var country = context.HttpContext.User.FindFirst(x => x.Type == "Country").Value;

            if (!_countries.Any(x=> x == country))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
