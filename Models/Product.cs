using AspnetCoreMvcFull.Models.User;
using System.Text.RegularExpressions;

namespace AspnetCoreMvcFull.Models
{
  public class Product
  {
    public int Id { get; set; }
    public string Name { get; set; } // نام محصول (مثلاً شیر کم‌چرب یا ماست یونانی)
    public int GroupId { get; set; } // کلید خارجی گروه
    public UserGroup Group { get; set; } // رابطه با جدول گروه
  }
}
