using PizzaShop.Domain.ViewModels;
using PizzaShop.Repository.Interface;
using PizzaShop.Service.Interface;

namespace PizzaShop.Service.Implementaion;

public class TaxAndFeeService : ITaxAndFeeService
{

    private readonly ITaxAndFeeRepository _taxAndFeeRepository;

    public TaxAndFeeService(ITaxAndFeeRepository taxAndFeeRepository)
    {
        _taxAndFeeRepository = taxAndFeeRepository;
    }

    public async Task<Pagination<TaxAndFeeViewModel>> GetTaxList(Pagination<TaxAndFeeViewModel> taxList)
    {
        return await _taxAndFeeRepository.GetTaxList(taxList);
    }

   public async Task<string> AddTax(TaxAndFeeViewModel addTax){
    return await _taxAndFeeRepository.AddTax(addTax);
   }

   public async Task<TaxAndFeeViewModel> GetTaxForEdit(string taxId){
    return await _taxAndFeeRepository.GetTaxForEdit(taxId);
   }

   public async Task<string> EditTax(TaxAndFeeViewModel editTax){
    return await _taxAndFeeRepository.EditTax(editTax);
   }

   public async Task<string> DeleteTax(string taxId){
    return await _taxAndFeeRepository.DeleteTax(taxId);
   }

}
