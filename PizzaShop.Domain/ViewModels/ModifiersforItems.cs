using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Domain.ViewModels;

public class ModifiersforItems
{
    public string? ModifierGroupId{get;set;}

    public string? ModifierGroupName{get;set;}

    // [Required]
    public  string? MinValue{get;set;}

    // [Required]
    public  string? MaxValue{get;set;}

    public List<ModifierNameRate>? ModifierItems{get;set;}
}

public class ModifierNameRate{
    public string? ModifierItemName{get;set;}

    public string? Rate{get;set;}
}
