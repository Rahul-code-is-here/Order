using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class Menuitem
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string ItemName { get; set; } = null!;

    public string ItemType { get; set; } = null!;

    public decimal Rate { get; set; }

    public int Quantity { get; set; }

    public int UnitId { get; set; }

    public bool? IsAvailable { get; set; }

    public bool DefaultTax { get; set; }

    public decimal TaxPercentage { get; set; }

    public bool? IsFavourite { get; set; }

    public string? Description { get; set; }

    public string? Shortcode { get; set; }

    public string? ImagePath { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<MappingMenuItemWithModifier> MappingMenuItemWithModifiers { get; set; } = new List<MappingMenuItemWithModifier>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<OrderedItem> OrderedItems { get; set; } = new List<OrderedItem>();

    public virtual Unit Unit { get; set; } = null!;
}
