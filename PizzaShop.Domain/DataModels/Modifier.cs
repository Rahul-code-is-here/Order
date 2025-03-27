using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class Modifier
{
    public int Id { get; set; }

    public string ModifierName { get; set; } = null!;

    public decimal Rate { get; set; }

    public int UnitId { get; set; }

    public int Quantity { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<Modifiergroupmodifier> Modifiergroupmodifiers { get; set; } = new List<Modifiergroupmodifier>();

    public virtual ICollection<OrderedItemModifierMapping> OrderedItemModifierMappings { get; set; } = new List<OrderedItemModifierMapping>();

    public virtual Unit Unit { get; set; } = null!;
}
