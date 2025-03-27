using System.ComponentModel.DataAnnotations;
// using DataAccessLayer.ViewModels;
using PizzaShop.Domain.ViewModels;
using Microsoft.AspNetCore.Http;

namespace PizzaShop.Domain.ViewModels;

public class AddItemsViewModel
{
    [Required]
    [Display(Name = "Category")]
    public required int CategoryId { get; set; }
    [Required]
    [MaxLength(20)]
    public required string ItemName { get; set; }
    [Required]
    public required string ItemType { get; set; }
    [Required]
    [Range(0.01,double.MaxValue, ErrorMessage ="Rate must be greater than 0")]
    [RegularExpression(@"^0?\d+(\.\d+)?$", ErrorMessage = "only digits are allowed and must be greater than 0")]
    [MaxLength(6)]
    public required string Rate { get; set; }
    [Required]
    [Range(0.01,double.MaxValue, ErrorMessage ="Quantity must be greater than 0")]
    [RegularExpression(@"^\d*$", ErrorMessage = "only digits are allowed and must be greater than 0")]
    [MaxLength(6)]
    public required string Quantity { get; set; }
    [Required]
    public required bool IsAvailable { get; set; }
    [Required]
    //  [Range(0,100, ErrorMessage ="Tax must be between 0 to")]
    public required bool DefaultTax { get; set; }
    [Required]
    [RegularExpression(@"^0?\d+(\.\d+)?$", ErrorMessage = "only digits are allowed")]
    [MaxLength(6)]
    public required string TaxPercentage { get; set; }
    [MaxLength(5)]
    public string? ShortCode { get; set; }
    public string? Description { get; set; }
    [Required]
    public required IFormFile ImageUpload { get; set; }
    [Required]
    public required string Unit { get; set; }
    public string? CreatedBy { get; set; }
    public string? EditedBy { get; set; }
    public List<DropDownViewModel>? Units { get; set; }

    public List<SelectedModifierDataForItemViewModel>? ModiferDatas{get;set;}
}

// public class SelectedModiferData
// {
//     public string? ModifierGroupId { get; set; }

//     public required string MinValue{get;set;}

//     public required string MaxValue{get;set;}
// }
