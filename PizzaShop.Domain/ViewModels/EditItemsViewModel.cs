using System.ComponentModel.DataAnnotations;
// using DataAccessLayer.ViewModels;
using PizzaShop.Domain.ViewModels;
using Microsoft.AspNetCore.Http;

namespace PizzaShop.Domain.ViewModels;

public class EditItemsViewModel
{
    [Required]
    [Display(Name = "Category")]
    public required int CategoryId { get; set; }

    [Required]
    public required int ItemId { get; set; }

    [Required]
    [MaxLength(20)]
    public required string ItemName { get; set; }

    [Required]
    public required string ItemType { get; set; }

    [Required]
    [RegularExpression(@"^0?\d+(\.\d+)?$", ErrorMessage = "only digits are allowed")]
    [Range(0.01,double.MaxValue, ErrorMessage ="Rate must be greater than 0")]
    [MaxLength(6)]
    public required string Rate { get; set; }

    [Required]
    [RegularExpression(@"^\d*$", ErrorMessage = "only digits are allowed")]
    [Range(0.01,double.MaxValue, ErrorMessage ="Quantity must be greater than 0")]
    [MaxLength(6)]
    public required string Quantity { get; set; }

    [Required]
    public required bool IsAvailable { get; set; }

    [Required]
    public required bool DefaultTax { get; set; }

    [Required]
    [RegularExpression(@"^0?\d+(\.\d+)?$", ErrorMessage = "only digits are allowed")]
    [MaxLength(6)]
    public required string TaxPercentage { get; set; }
    [MaxLength(5)]
    public string? ShortCode { get; set; }

    public string? Description { get; set; }

    public IFormFile? ImageUpload { get; set; }
    [Required]
    public required string Unit { get; set; }

    public string? EditedBy { get; set; }

    public List<DropDownViewModel>? Units { get; set; }

    public List<SelectedModifierDataForItemViewModel>? ModiferDatas { get; set; }

}
