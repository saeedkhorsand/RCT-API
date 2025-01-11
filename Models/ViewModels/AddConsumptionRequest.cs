namespace AspnetCoreMvcFull.Models.ViewModels
{
  public class AddConsumptionRequest
  {
    public int UserId { get; set; }
    public int? ProductId { get; set; }
    public DateTime ConsumptionTime { get; set; }
    public double Quantity { get; set; }
    public string? Dsc { get; set; }
  }
}
