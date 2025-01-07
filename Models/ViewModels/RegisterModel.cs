using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AspnetCoreMvcFull.Models.ViewModels
{
  public class RegisterModel
  {
    [Required]
    public string Username { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; }
  }
}
