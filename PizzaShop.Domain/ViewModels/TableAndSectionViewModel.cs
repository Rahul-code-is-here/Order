namespace PizzaShop.Domain.ViewModels;

public class TableAndSectionViewModel
{
    public SectionViewModel? AddSection{get;set;}
    public TableViewModel? AddTable{get;set;}
    public List<SectionViewModel>? sectionList{get;set;}
}