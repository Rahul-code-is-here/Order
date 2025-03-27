using PizzaShop.Domain.ViewModels;

namespace PizzaShop.Service.Interface;

public interface ITaxAndFeeService
{
   Task<Pagination<TaxAndFeeViewModel>> GetTaxList(Pagination<TaxAndFeeViewModel> taxList);
    Task<string> AddTax(TaxAndFeeViewModel addTax);
    Task<TaxAndFeeViewModel> GetTaxForEdit(string taxId);
    Task<string> EditTax(TaxAndFeeViewModel AddTax);
    Task<string> DeleteTax(string taxId);
}
