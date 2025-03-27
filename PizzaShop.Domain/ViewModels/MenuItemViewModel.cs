using System.ComponentModel.DataAnnotations;

namespace PizzaShop.Domain.DataModels
{
    // public class MenuItemViewModel
    // {

    //     public int Id { get; set; }

    //     public int CategoryId { get; set; }

    //     public string ItemName { get; set; }

    //     [Required]
    //     public string ItemType { get; set; }


    //     public decimal Rate { get; set; }


    //     public int Quantity { get; set; }

    //     [Required]
    //     public string Unit { get; set; }

    //     public int UnitId { get; set; }

    //     public bool IsAvailable { get; set; }

    //     public bool IsTaxable { get; set; }

    //     [Range(0, 100)]
    //     public decimal? TaxPercentage { get; set; }

    //     [StringLength(10)]
    //     public string ShortCode { get; set; }

    //     [StringLength(500)]
    //     public string Description { get; set; }

    //     public IFormFile Image { get; set; }
    // }

    public class MenuItemViewModel
    {

        public int Id { get; set; }

        public int ModifiergroupId { get; set; }

        public int min { get; set; }

        public int max { get; set; }
        public int CategoryId { get; set; }
        [Required]
        [RegularExpression(@"^\S+$", ErrorMessage = "No whiteSpaces are allowed")]
        public string ItemName { get; set; } = null!;
        [Required]
        public string ItemType { get; set; } = null!;
        [Required]
        public decimal Rate { get; set; }
        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Unit { get; set; }

        public int UnitId { get; set; }
        [Required]
        public bool? IsAvailable { get; set; }

        public bool DefaultTax { get; set; }

        public decimal TaxPercentage { get; set; }

        public bool? IsFavourite { get; set; }

        public string? Description { get; set; }

        public string? Shortcode { get; set; }

        public IFormFile? Image { get; set; }

        public string? ImagePath { get; set; }

         public List<SelectedModifierList> selectedModifierLists { get; set; }

    public LinkedList<int> listofModifierGroupIds { get; set; }

    public List<Category> Categories { get; set; }

    public List<Modifiergroup> ModifierGroups { get; set; }

    public List<Unit> Units { get; set; }

    }
}


// ViewModel for mapping a menu item with a modifier group
 public class SelectedModifierList
  {
    public int ModifierGroupId {get; set;}

    // public int ModifierId { get; set; }

    public int MinValue { get; set; }

    public int MaxValue { get; set; }

        // public static implicit operator int(SelectedModifierList v)
        // {
        //     throw new NotImplementedException();
        // }
    }
