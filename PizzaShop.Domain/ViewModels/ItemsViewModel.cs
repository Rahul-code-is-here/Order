using Microsoft.AspNetCore.Http;

namespace PizzaShop.Domain.ViewModels;

public class ItemsViewModel
{
    public int? CategoryId {get;set;}

    public int? ItemId {get;set;}
    public string? ItemName{get;set;}

    public string? ItemType{get;set;}
    
    public int? Rate{get;set;}

    public int? Quantity {get;set;}

    public bool? IsAvailable{get;set;}

    public string? Img{get;set;}

}
