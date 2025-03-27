namespace PizzaShop.Domain.ViewModels;

public class Pagination<T>
{
    public int PageSize { get; set; } = 5;
    public int CurrentPage { get; set; } = 1;
    public int? StartIndex { get; set; }
    public int? EndIndex { get; set; }
    public int? NumberOfItems { get; set; }
    public int? TotalPages { get; set; }
    public string? SearchFilter { get; set; }
    public string? SortColumn { get; set; }
    public string? SortOrder { get; set; }

    // for orderslistsorting
    public string? FromDate { get; set; }
    public string? ToDate { get; set; }

    public string? OrderStatusId { get; set; }

    // for existing modifiers
    public bool SelectExisitngModifier { get; set; } = false;

    public IEnumerable<T>? Items { get; set; }

}