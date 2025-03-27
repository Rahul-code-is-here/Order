namespace PizzaShop.Domain.ViewModels;
public class OrderViewModel
{
  public string? OrderId { get; set; }

  public DateOnly? OrderDate { get; set; }

  public string? CustomerName { get; set; }

  public string? Orderstatus { get; set; }

  public string? PaymentMode { get; set; }

  public double? Rating { get; set; }

  public double? TotalAmount { get; set; }

  public List<DropDownViewModel>? AllStatus { get; set; }
  public List<DropDownViewModel>? AllTime { get; set; }
}
