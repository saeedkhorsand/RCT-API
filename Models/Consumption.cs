using AspnetCoreMvcFull.Models.User;

namespace AspnetCoreMvcFull.Models
{
  public class Consumption
  {
    public int Id { get; set; }
    public string UserId { get; set; } // کلید خارجی کاربر
    public ApplicationUser User { get; set; } // رابطه با کاربر
    public int ProductId { get; set; } // کلید خارجی محصول
    public Product Product { get; set; } // رابطه با محصول
    public DateTime ConsumptionTime { get; set; } // تاریخ و ساعت مصرف
    public double Quantity { get; set; } // مقدار مصرف (مثلاً به میلی‌لیتر یا گرم)
    public string? Dsc { get; set; }
  }
}
