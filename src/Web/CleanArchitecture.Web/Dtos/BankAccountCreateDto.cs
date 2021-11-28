using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Web.Api.Dtos
{
    public class BankAccountCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
