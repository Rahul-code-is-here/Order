using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class MappingMenuItemWithModifier
{
    public int Id { get; set; }

    public int MenuItemId { get; set; }

    public int ModifierGroupId { get; set; }

    public int MinSelectionRequired { get; set; }

    public int? MaxSelectionAllowed { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Menuitem MenuItem { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Modifiergroup ModifierGroup { get; set; } = null!;
}
