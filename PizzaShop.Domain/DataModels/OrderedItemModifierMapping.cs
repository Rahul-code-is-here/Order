using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class OrderedItemModifierMapping
{
    public int Id { get; set; }

    public int OrderItemId { get; set; }

    public int ModifierId { get; set; }

    public int Quantityofmodifier { get; set; }

    public int? Quantity { get; set; }

    public decimal? Rateofmodifier { get; set; }

    public decimal? Totalamount { get; set; }

    public decimal? Totalmodifieramount { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Modifier Modifier { get; set; } = null!;

    public virtual OrderedItem OrderItem { get; set; } = null!;
}
