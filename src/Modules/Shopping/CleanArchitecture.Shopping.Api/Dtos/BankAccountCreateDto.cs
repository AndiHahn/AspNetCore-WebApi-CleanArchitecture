using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Shopping.Api.Dtos
{
    public class BankAccountCreateDto
    {
        [Required]
        public string Name { get; set; }
    }
}
