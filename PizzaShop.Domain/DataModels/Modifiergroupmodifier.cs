using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class Modifiergroupmodifier
{
    public int Id { get; set; }

    public int ModifierGroupId { get; set; }

    public int ModifierId { get; set; }

    public virtual Modifier Modifier { get; set; } = null!;

    public virtual Modifiergroup ModifierGroup { get; set; } = null!;
}
