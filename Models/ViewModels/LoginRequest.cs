using System.ComponentModel.DataAnnotations;

namespace AspnetCoreMvcFull.Models.ViewModels
{
  public class LoginRequest
  {
    [Required]
    public string Username { get; set; }
    [Required]
    public string Password { get; set; }

    public bool? RememberMe { get; set; }
  }
}
