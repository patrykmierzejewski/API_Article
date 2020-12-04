using API_Article.Entities;
using API_Article.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDTO>
    {
        public RegisterUserValidator(ArticleContext articleContext)
        {
            EmailValidator(articleContext);
        }

        private void EmailValidator(ArticleContext articleContext)
        {
            //email
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.PasswordHash).MinimumLength(6);
            RuleFor(x => x.PasswordHash).Equal(x => x.ConfirmPassword);

            RuleFor(x => x.Email).Custom((value, context) =>
            {
                var userAllreadyExists = articleContext.Users.Any(user => user.Email == value);

                if (userAllreadyExists)
                {
                    context.AddFailure("Email", "That email address is taken");
                }
            });
        }
    }
}
