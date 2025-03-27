// using BussinessLogicLayer.Interface;
// using DataAccessLayer.Models;
using PizzaShop.Domain.DataModels;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Domain.ViewModels;
// using DataAccessLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Repository.Interface;
using PizzaShop.Domain.DataContext;

namespace PizzaShop.Repository.Implementation;

public class TaxAndFeeRepository : ITaxAndFeeRepository
{
    private readonly PizzaShemaContext _db;
    public TaxAndFeeRepository(PizzaShemaContext db)
    {
        _db = db;
    }
    public async Task<Pagination<TaxAndFeeViewModel>> GetTaxList(Pagination<TaxAndFeeViewModel> taxList)
    {
        var taxes = _db.TaxesAndFees.Where(i => i.IsDeleted == false).OrderBy(i => i.Name).Select(i => new TaxAndFeeViewModel()
        {
            TaxId = i.Id.ToString(),
            TaxName = i.Name,
            Taxtype = (bool)i.Type == true ? "Percentage" : "FlatAmount",
            TaxValue = (double)i.TaxValue,
            DefaultTax = (bool)i.IsDefault,
            IsEnabled = (bool)i.IsActive,

        });
        if (!string.IsNullOrEmpty(taxList.SearchFilter))
        {
            taxes = taxes.Where(i => i.TaxName.ToLower().Contains(taxList.SearchFilter.Trim().ToLower()));
        }
        taxList.NumberOfItems = await taxes.CountAsync();

        var temp = (taxList.CurrentPage - 1) * taxList.PageSize;
        taxList.StartIndex = temp + 1;
        taxList.EndIndex = temp + taxList.PageSize;
        taxList.TotalPages = (int)Math.Ceiling((double)taxList.NumberOfItems / taxList.PageSize);
        taxList.Items = await taxes.Skip(temp).Take(taxList.PageSize).ToListAsync();
        return taxList;
    }

    public async Task<string> AddTax(TaxAndFeeViewModel AddTaxAndFee)
    {
        var isTax = await _db.TaxesAndFees.AnyAsync(i => i.Name.ToLower() == AddTaxAndFee.TaxName.Trim().ToLower());
        if (isTax)
        {
            return "Tax with the name already exist";
        }
        TaxesAndFee tax = new TaxesAndFee()
        {
            Name = AddTaxAndFee.TaxName,
            // Type = AddTaxAndFee.Taxtype == true ? "percentage" : "FlatAmount",
            Type = AddTaxAndFee.Taxtype == "2" ? false : true,
            TaxValue = AddTaxAndFee.TaxValue,
            Percentage = AddTaxAndFee.Taxtype == "1" ? (decimal)AddTaxAndFee.TaxValue : (decimal)(AddTaxAndFee.TaxValue - AddTaxAndFee.TaxValue),
            FlatAmount = AddTaxAndFee.Taxtype == "2" ? (decimal)AddTaxAndFee.TaxValue : (decimal)(AddTaxAndFee.TaxValue - AddTaxAndFee.TaxValue),
            IsDefault = AddTaxAndFee.DefaultTax,
            IsActive = AddTaxAndFee.IsEnabled,
            // CreatedBy = AddTaxAndFee.CreatedBy,
        };
        await _db.AddAsync(tax);
        await _db.SaveChangesAsync();
        return "Tax Added Successfully";
    }

    public async Task<TaxAndFeeViewModel> GetTaxForEdit(string taxId)
    {
        var tax = await (from t in _db.TaxesAndFees
                         where t.Id.ToString() == taxId
                         select new TaxAndFeeViewModel()
                         {
                             TaxId = t.Id.ToString(),
                             TaxName = t.Name,
                             DefaultTax = (bool)t.IsDefault,
                             IsEnabled = (bool)t.IsActive,
                             Taxtype = t.Type == true ? "1" : "2",
                             TaxValue = (double)t.TaxValue
                         }).FirstOrDefaultAsync();
        return tax;
    }

    public async Task<string> EditTax(TaxAndFeeViewModel editTax)
    {
        var IsTax = await _db.TaxesAndFees.AnyAsync(u => u.Name.ToLower() == editTax.TaxName.Trim().ToLower() && u.Id.ToString() != editTax.TaxId);
        if (IsTax)
        {
            return "tax with the name already exist";
        }
        var tax = await _db.TaxesAndFees.FirstOrDefaultAsync(t => t.Id.ToString() == editTax.TaxId);
        tax.Name = editTax.TaxName;
        // tax.Type = editTax.Taxtype =="1"?"percentage":"FlatAmount";
        tax.Type = editTax.Taxtype == "1" ? true : false;
        tax.TaxValue = editTax.TaxValue;
        tax.IsActive = editTax.IsEnabled;
        tax.IsDefault = editTax.DefaultTax;
        tax.Percentage = editTax.Taxtype == "1" ? (decimal)editTax.TaxValue : (decimal)(editTax.TaxValue - editTax.TaxValue);
        tax.FlatAmount = editTax.Taxtype == "2" ? (decimal)editTax.TaxValue : (decimal)(editTax.TaxValue - editTax.TaxValue);

        // tax.EditedBy = editTax.EditedBy;
        // tax.EditDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return "tax Edited Successfully";
    }
    public async Task<string> DeleteTax(string taxId)
    {
        var tax = await _db.TaxesAndFees.FirstOrDefaultAsync(s => s.Id.ToString() == taxId);
        if (tax == null)
        {
            return "error while deleting tax";
        }
        tax.IsDeleted = true;
        await _db.SaveChangesAsync();
        return "tax Deleted Successfully";
    }
}