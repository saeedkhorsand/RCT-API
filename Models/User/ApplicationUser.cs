namespace AspnetCoreMvcFull.Models.User;

using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

public class ApplicationUser : IdentityUser
{
  public string? Gender { get; set; } // جنسیت
  public int? GroupId { get; set; } // کلید خارجی گروه
  public UserGroup? Group { get; set; } // رابطه با جدول گروه
  public ICollection<Consumption> Consumptions { get; set; } // تاریخچه مصرف
}
