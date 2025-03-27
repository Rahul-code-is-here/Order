using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class Payment
{
    public int Paymentid { get; set; }

    public string Paymentmode { get; set; } = null!;

    public virtual ICollection<Orderapp> Orderapps { get; set; } = new List<Orderapp>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
