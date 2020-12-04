using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Article.Entities;
using API_Article.Identity;
using API_Article.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace API_Article.Controllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ArticleContext _articleContext;
        private readonly ILogger _logger;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtPrivider _jwtPrivider;

        public AccountController(ArticleContext articleContex, ILogger<InformationController> logger, IPasswordHasher<User> passwordHasher, IJwtPrivider jwtPrivider)
        {
            _articleContext = articleContex;
            _logger = logger;
            _passwordHasher = passwordHasher;
            _jwtPrivider = jwtPrivider;
        }

        [HttpPost("{register}")]
        public IActionResult Register([FromBody] RegisterUserDTO registerUserDTO)
        {
            if (!ModelState.IsValid)
               return BadRequest(ModelState);

            User newUser = new User()
            {
                Email = registerUserDTO.Email,
                Country = registerUserDTO.Country,
                DateOfBirth = registerUserDTO.DateOfBirth.HasValue ? registerUserDTO.DateOfBirth : DateTime.Now,
                RoleId = registerUserDTO.RoleId
            };

            var passwordHash = _passwordHasher.HashPassword(newUser, registerUserDTO.PasswordHash);
            newUser.PasswordHash = passwordHash;

            _articleContext.Users.Add(newUser);
            _articleContext.SaveChanges();

            _logger.LogWarning($"Dodano nowego uzytkownika - {newUser.Email}, RoleID - {newUser.RoleId}");

            return Ok();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var user = _articleContext.Users
                .Include(user => user.Role)
                .FirstOrDefault(user => user.Email == userLoginDTO.Email);

            if (user == null)
                return BadRequest("Invalid user name or password");

            var passwordVerificateResoult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, userLoginDTO.Password);
      
            if (passwordVerificateResoult == PasswordVerificationResult.Failed)
                return BadRequest("Invalid user name or password");

            string token = _jwtPrivider.GenerateJwtToken(user);
            var a = _jwtPrivider.GetClaims();

            return Ok(token);
        }
    }
}
