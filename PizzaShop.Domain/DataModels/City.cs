using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class City
{
    public int CityId { get; set; }

    public int StateId { get; set; }

    public string CityName { get; set; } = null!;

    public virtual State State { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
