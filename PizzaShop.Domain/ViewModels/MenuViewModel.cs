namespace PizzaShop.Domain.ViewModels;

public class MenuViewModel
{
    public AddCategoryViewModel? AddCategoryViewModel { get; set; }
    public ItemsViewModel? ItemsViewModel { get; set; }
    public AddItemsViewModel? AddItemsViewModel { get; set; }
    public EditItemsViewModel? EditItemsViewModel { get; set; }
    public ModifiersViewModel? AddModifiers { get; set; }
    public AddModifierItemViewModel? AddModifierItemViewModel { get; set; }
    public List<AddCategoryViewModel>? CategoryList { get; set; }
    public List<ModifiersViewModel>? ModifiersList { get; set; }
    public List<DropDownViewModel>? Units { get; set; }
    public Pagination<ItemsViewModel>? ItemList { get; set; }
    public Pagination<ModifiersViewModel>? ModifierItemList { get; set; }

}
