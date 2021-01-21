using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Core.Models.Domain.User
{
    public class SignInModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}