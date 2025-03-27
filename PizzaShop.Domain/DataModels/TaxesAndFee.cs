using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class TaxesAndFee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Type { get; set; }

    public decimal FlatAmount { get; set; }

    public decimal Percentage { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public bool IsDefault { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public double? TaxValue { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<OrderTaxMapping> OrderTaxMappings { get; set; } = new List<OrderTaxMapping>();
}
