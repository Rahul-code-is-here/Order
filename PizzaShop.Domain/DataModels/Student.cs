using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class Student
{
    public int Rollnumber { get; set; }

    public string? Name { get; set; }

    public string? Mobilenumber { get; set; }

    public string? City { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Pincode { get; set; }
}
