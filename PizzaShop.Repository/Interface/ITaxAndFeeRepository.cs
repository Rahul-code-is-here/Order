// using DataAccessLayer.ViewModels;
using PizzaShop.Domain.ViewModels;

namespace PizzaShop.Repository.Interface
{
public interface ITaxAndFeeRepository
{
    Task<Pagination<TaxAndFeeViewModel>> GetTaxList(Pagination<TaxAndFeeViewModel> taxList);
    Task<string> AddTax(TaxAndFeeViewModel AddTax);
    Task<TaxAndFeeViewModel> GetTaxForEdit(string taxId);
    Task<string> EditTax(TaxAndFeeViewModel AddTax);
    Task<string> DeleteTax(string taxId);
    
}
}