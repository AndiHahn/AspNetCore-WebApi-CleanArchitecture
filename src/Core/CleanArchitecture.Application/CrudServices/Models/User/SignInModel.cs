using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Application.CrudServices.Models.User
{
    public class SignInModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}