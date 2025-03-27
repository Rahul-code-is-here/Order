using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class Orderstatus
{
    public int Orderstatusid { get; set; }

    public bool Pending { get; set; }

    public bool Running { get; set; }

    public bool Inprogress { get; set; }

    public bool Completed { get; set; }

    public string Orderstatusname { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
