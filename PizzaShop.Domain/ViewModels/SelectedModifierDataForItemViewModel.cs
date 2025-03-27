namespace PizzaShop.Domain.ViewModels;

public class SelectedModifierDataForItemViewModel
{
    public string? ModifierGroupId { get; set; }

    public required string MinValue{get;set;}

    public required string MaxValue{get;set;}
}
