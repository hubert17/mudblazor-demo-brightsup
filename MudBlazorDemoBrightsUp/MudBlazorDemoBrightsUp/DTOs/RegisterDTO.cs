using System.ComponentModel.DataAnnotations;

namespace MudBlazorDemoBrightsUp.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, Compare(nameof(Password)), DataType(DataType.Password)] 
        public string ConfirmPassword { get; set; }

        public string Role { get; set; } // comma-delimited
    }
}
