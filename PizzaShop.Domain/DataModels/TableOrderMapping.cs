using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class TableOrderMapping
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int TableId { get; set; }

    public int Noofperson { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Table Table { get; set; } = null!;
}
