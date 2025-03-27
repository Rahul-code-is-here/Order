using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class Unit
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Shortname { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Menuitem> Menuitems { get; set; } = new List<Menuitem>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<Modifier> Modifiers { get; set; } = new List<Modifier>();
}
