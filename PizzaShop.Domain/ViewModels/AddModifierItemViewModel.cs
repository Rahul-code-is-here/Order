using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Domain.ViewModels;

public class AddModifierItemViewModel
{
    
    public  List<string>? ModifierGroupIds{get;set;}
    public  List<ModifierValue>? ModifierValues{get;set;}
    public  int? ModifierItemId{get;set;}

    [Required]
    public required string ModifierItemName{get;set;}
    
    [Required]
    public required int Rate{get;set;}
    
    [Required]
    public required int Quantity{get;set;}
    
    [Required]
    public required int Unit{get;set;}

    public string? Description{get;set;}

    public string? CreatedBy{get;set;}

    public string? EditedBy{get;set;}
}

public class ModifierValue{
    public string? ModifierGroupId{get;set;}

    public string? ModifierGroupName{get;set;}
}
