using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int OrderNo { get; set; }

    public string Status { get; set; } = null!;

    public decimal TotalAmount { get; set; }

    public decimal? Tax { get; set; }

    public decimal? Discount { get; set; }

    public decimal? PaidAmount { get; set; }

    public string? Note { get; set; }

    public bool? IsSgstSelected { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public int? Paymentid { get; set; }

    public int? Orderstatusid { get; set; }

    public int? Rating { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<OrderTaxMapping> OrderTaxMappings { get; set; } = new List<OrderTaxMapping>();

    public virtual ICollection<OrderedItem> OrderedItems { get; set; } = new List<OrderedItem>();

    public virtual Orderstatus? Orderstatus { get; set; }

    public virtual Payment? Payment { get; set; }

    public virtual ICollection<TableOrderMapping> TableOrderMappings { get; set; } = new List<TableOrderMapping>();
}
