// using DataAccessLayer.ViewModels;
using PizzaShop.Domain.ViewModels;

namespace PizzaShop.Repository.Interface;

public interface IOrdersRepository
{
    Task<List<DropDownViewModel>> GetAllStatus();
    // Task<List<DropDownViewModel>> GetAllTime();
    Task<Pagination<OrderViewModel>> OrderList(Pagination<OrderViewModel> orderList);
    Task<List<OrderViewModel>> OrderListForExport(string searchFilter,string dateFilter,string statusFilter);
}
