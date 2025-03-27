using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class Orderapp
{
    public int Orderappid { get; set; }

    public int Orderid { get; set; }

    public int Itemid { get; set; }

    public int Quantity { get; set; }

    public string? Comment { get; set; }

    public string? ItemComment { get; set; }

    public int? Paymentid { get; set; }

    public virtual Payment? Payment { get; set; }
}
