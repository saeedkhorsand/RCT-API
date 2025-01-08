using System.ComponentModel.DataAnnotations;

namespace AspnetCoreMvcFull.Models.ViewModels
{
  public class ProductRequest
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } // نام محصول (مثلاً شیر کم‌چرب یا ماست یونانی)
    [Required]
    public int GroupId { get; set; } // کلید خارجی گروه
  }
}
