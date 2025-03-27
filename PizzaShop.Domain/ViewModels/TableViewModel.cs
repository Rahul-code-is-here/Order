using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Domain.ViewModels;

public class TableViewModel
{
    [Required]
    public required string SectionId { get; set; }
    public string? TableId { get; set; }
    [Required]
    [MaxLength(20)]
    [RegularExpression(@"^\S.*$", ErrorMessage = "The field cannot start with a whitespace.")]
    public required string TableName { get; set; }
    [Required]
    [RegularExpression(@"^\s*-?[0-9]{1,2}\s*$", ErrorMessage = "capacity value must be Number and Limit is 2 digit")]
    public required string Capacity { get; set; }
    [Required]
    public required int status { get; set; }

    public string Newstatus { get; set; }


    public string? CreatedBy { get; set; }
    public string? EditedBy { get; set; }
}