using System.ComponentModel.DataAnnotations;

namespace AspnetCoreMvcFull.Models.ViewModels
{
  public class ConsumptionRequest
  {
    [Required]
    public DateTime From { get; set; }
    [Required]
    public DateTime To { get; set; }
  }
}
