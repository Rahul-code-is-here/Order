using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Domain.ViewModels;

public class AddCategoryViewModel
{
    public string? CategoryId{get;set;}
    
    [Required]
     [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "category Name must start with a letter and contain only letters (no spaces).")]

    [MaxLength(20)]
    public required string CategoryName{get;set;}
    
    [MaxLength(100)]
    public string? CategoryDescription{get;set;}
    public string? CreatedBy{get;set;}
    public string? EditedBy{get;set;}
}
