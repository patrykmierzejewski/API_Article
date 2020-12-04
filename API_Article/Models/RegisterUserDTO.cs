using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Article.Models
{
    /// <summary>
    /// Register user - (Validators/RegistersUserValidator) validator include required rule
    /// </summary>
    public class RegisterUserDTO
    {
        public string Email { get; set; }
        
        public string PasswordHash { get; set; }

        public string ConfirmPassword { get; set; }

        public string Country { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public int RoleId { get; set; } = 1; //default value =1 (User)
    }
}
