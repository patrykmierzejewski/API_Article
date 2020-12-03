using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Article.Entities;
using API_Article.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API_Article.Controllers
{
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ArticleContext _articleContext;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(ArticleContext articleContex, ILogger<InformationController> logger, IPasswordHasher<User> passwordHasher)
        {
            _articleContext = articleContex;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("{register}")]
        public IActionResult Register([FromBody]RegisterUserDTO registerUserDTO)
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
    }
}
