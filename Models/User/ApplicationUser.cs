namespace AspnetCoreMvcFull.Models.User;

using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
  public string Gender { get; set; } // جنسیت
  public int GroupId { get; set; }  // گروه (کلید خارجی)
  public UserGroup Group { get; set; }  // رابطه با جدول گروه‌ها
}
