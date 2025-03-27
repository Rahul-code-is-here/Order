using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class Modifiergroup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<MappingMenuItemWithModifier> MappingMenuItemWithModifiers { get; set; } = new List<MappingMenuItemWithModifier>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<Modifiergroupmodifier> Modifiergroupmodifiers { get; set; } = new List<Modifiergroupmodifier>();
}
