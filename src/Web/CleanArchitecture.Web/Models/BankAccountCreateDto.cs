using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Web.Api.Models
{
    public class BankAccountCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
