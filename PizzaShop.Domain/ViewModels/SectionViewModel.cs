using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Domain.ViewModels;

public class SectionViewModel
{
    public string? SectionId{get;set;}
    [Required]
    [MaxLength(20)]
    [RegularExpression(@"^\S.*$", ErrorMessage = "The field cannot start with a whitespace.")]
    public required string SectionName{get;set;}
    [MaxLength(100)]
    [RegularExpression(@"^\S.*$", ErrorMessage = "The field cannot start with a whitespace.")]
    public string? Description{get;set;}
    public string? CreatedBy{get;set;}
    public string? EditedBy{get;set;}

    
}