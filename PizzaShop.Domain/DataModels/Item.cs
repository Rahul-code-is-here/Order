using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class Item
{
    public int Itemid { get; set; }

    public string Itemname { get; set; } = null!;

    public int? Rate { get; set; }

    public string Itemtype { get; set; } = null!;

    public int? Quantity { get; set; }

    public bool? Isavailable { get; set; }

    public bool? Isdeleted { get; set; }

    public string? Itemdescription { get; set; }

    public string? Itemimage { get; set; }

    public int Categoryid { get; set; }

    public int Taxesid { get; set; }

    public int Unitid { get; set; }

    public string? CreatedBy { get; set; }

    public string? EditedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? EditDate { get; set; }

    public bool Defaulttax { get; set; }

    public decimal? Taxpercentage { get; set; }

    public string? Shortcode { get; set; }
}
