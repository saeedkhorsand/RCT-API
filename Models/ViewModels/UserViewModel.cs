namespace AspnetCoreMvcFull.Models.ViewModels
{
  public class UserViewModel
  {
    public string Id { get; set; } // شناسه کاربر
    public string UserName { get; set; } // نام کاربری
    public string? Email { get; set; } // ایمیل
    public string? PhoneNumber { get; set; } // شماره تماس
    public string? Gender { get; set; } // جنسیت
    public string? GroupName { get; set; } // نام گروه
    public bool IsLocked { get; set; } // وضعیت قفل بودن حساب
  }
}
