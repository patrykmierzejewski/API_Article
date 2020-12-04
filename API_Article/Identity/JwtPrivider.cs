using API_Article.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API_Article.Identity
{
    public class JwtPrivider : IJwtPrivider
    {
        private readonly JwtOptions _jwtOptions;
        private List<Claim> _claimsList;
        public JwtPrivider(JwtOptions jwtOptions)
        {
            _jwtOptions = jwtOptions;
        }
        public string GenerateJwtToken(User user)
        {
            _claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.RoleName),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("DateOfBirth", user.DateOfBirth.Value.ToString("dd-MM-yyyy"))
            };

            //dodanie własnych claimow potrzebnych do autentykacji
            SetExtensionsClaims(user);
            //key 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.JwtKey));

            // credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //ilosc dni ważnosci tokena pobrana z ustawień
            var expires = DateTime.Now.AddDays(_jwtOptions.JwtExpireDays);

            var token = new JwtSecurityToken(
                _jwtOptions.JwtIssuer,
                _jwtOptions.JwtIssuer,
                _claimsList,
                expires: expires,
                signingCredentials: creds);

            // konwertowanie całego tokena na string
            var tokenHendler = new JwtSecurityTokenHandler();
       
            return tokenHendler.WriteToken(token);
        }

        private void SetExtensionsClaims(User user)
        {
            if (!String.IsNullOrEmpty(user.isActive))
                _claimsList.Add(new Claim("isActive", user.isActive));

            //if not null - to authorize
            if (!String.IsNullOrEmpty(user.Country))
                _claimsList.Add(new Claim("Country", user.Country));
        }

        public List<Claim> GetClaims()
        {
            if (_claimsList != null)
                return _claimsList;
            else
                return null;
        }
    }
}
