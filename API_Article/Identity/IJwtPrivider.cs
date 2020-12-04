using API_Article.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API_Article.Identity
{
    public interface IJwtPrivider
    {
        string GenerateJwtToken(User user);

        List<Claim> GetClaims();
    }
}
