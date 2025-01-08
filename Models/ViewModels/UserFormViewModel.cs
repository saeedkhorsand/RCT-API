using AspnetCoreMvcFull.Models.User;
using System.ComponentModel.DataAnnotations;

namespace AspnetCoreMvcFull.Models.ViewModels
{
  public class UserFormViewModel
  {
    public string? Id { get; set; } // Null for new users, set for existing ones.

    [Required]
    [Display(Name = "Username")]
    public string UserName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Phone]
    [Display(Name = "Phone Number")]
    public string? PhoneNumber { get; set; }

    [Display(Name = "Gender")]
    public string? Gender { get; set; }

    [Required]
    [Display(Name = "Group")]
    public int GroupId { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string? Password { get; set; } // Nullable for editing

    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string? ConfirmPassword { get; set; } // Nullable for editing

    [Display(Name = "Is Locked")]
    public bool IsLocked { get; set; }

    public IEnumerable<UserGroup> Groups { get; set; } = new List<UserGroup>();

    public bool IsNew => string.IsNullOrEmpty(Id); // Determines if it's a create or edit operation
  }

}
