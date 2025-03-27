using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Domain.ViewModels;
//using it for adding and to show list of modifiers
public class ModifiersViewModel
{
    
    public string? ModifierGroupId{get;set;}

    [Required]
    public required string ModifierGroupName{get;set;}

    [MaxLength(100)]
    public string? Description{get;set;}

    public string? CreatedBy{get;set;}
    public string? EditedBy{get;set;}
    public DateTime? EditedDate{get;set;}

    public string? ModifierItemIds{get;set;}
    public List<string>? ModifierItems { get; set; }

    public List<ModifierItemValuesViewModel>? ModifierItemsData{get;set;}

}

