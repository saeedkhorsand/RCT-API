namespace AspnetCoreMvcFull.Models.User;
public class UserGroup
{
  public int Id { get; set; }
  public string Name { get; set; }
  public ICollection<ApplicationUser>? Users { get; set; } // کاربران مرتبط با این گروه
  public ICollection<Product>? Products { get; set; } // محصولات مرتبط (مثلاً انواع شیر یا ماست)
}
