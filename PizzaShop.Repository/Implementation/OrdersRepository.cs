using System.Runtime.InteropServices;
// using BussinessLogicLayer.Interface;
using PizzaShop.Repository.Interface;
// using DataAccessLayer.Models;
using PizzaShop.Domain.DataModels;
// using DataAccessLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;
using PizzaShop.Domain.DataContext;
using PizzaShop.Domain.ViewModels;

namespace PizzaShop.Repository.Implementations
{
public class OrdersRepository : IOrdersRepository
{
    private readonly PizzaShemaContext _db;
    public OrdersRepository(PizzaShemaContext db)
    {
        _db=db;
    }

    public async Task<List<DropDownViewModel>> GetAllStatus()
    {
        var allStatus = await _db.Orderstatuses.Select(i=>new DropDownViewModel(){
            Text = i.Orderstatusname,
            Value = i.Orderstatusid.ToString()
        }).ToListAsync();
        return allStatus;
    }

    public async Task<Pagination<OrderViewModel>> OrderList(Pagination<OrderViewModel> orderList){
            var orders = from o in _db.Orders
                            join c in _db.Customers
                            on o.CustomerId equals c.Id
                            join p in _db.Payments
                            on o.Paymentid equals p.Paymentid
                            join s in _db.Orderstatuses
                            on o.Orderstatusid equals s.Orderstatusid
                            orderby o.Id descending
                            select( new OrderViewModel(){
                                OrderId = o.Id.ToString(),
                                OrderDate = DateOnly.FromDateTime((DateTime)o.CreatedAt),
                                CustomerName = c.Name,
                                Orderstatus = s.Orderstatusname,
                                PaymentMode = p.Paymentmode,
                                Rating = (int)o.Rating,
                                TotalAmount = (double)o.TotalAmount,

                            });

            if(!string.IsNullOrEmpty(orderList.SearchFilter)){
                // orders = orders.Where(o=>o.CustomerName.ToLower().Contains(orderList.SearchFilter.ToLower()) || o.OrderId.Contains(orderList.SearchFilter.ToLower()));
                orders = orders.Where(u => u.CustomerName.ToLower().Contains(orderList.SearchFilter.Trim().ToLower()) ||
                                u.OrderId.Contains(orderList.SearchFilter.Trim().ToLower()));
            }

            switch(orderList.SortColumn){
                case "Orderid":
                    if(orderList.SortOrder == "asc"){
                        orders = orders.OrderBy(i=>i.OrderId);
                        break;
                    }
                    orders = orders.OrderByDescending(i=>i.OrderId);
                    break;
                case "Orderdate":
                if(orderList.SortOrder == "asc"){
                        orders = orders.OrderBy(i=>i.OrderDate);
                        break;
                    }
                    orders = orders.OrderByDescending(i=>i.OrderDate);
                    break;
                case "customerName":
                    if(orderList.SortOrder == "asc"){
                        orders = orders.OrderBy(i=>i.CustomerName);
                        break;
                    }
                    orders = orders.OrderByDescending(i=>i.CustomerName);
                    break;
                default :
                if(orderList.SortOrder == "asc"){
                        orders = orders.OrderBy(i=>i.TotalAmount);
                        break;
                    }
                    orders = orders.OrderByDescending(i=>i.TotalAmount);
                    break;
            }
            if (orderList.OrderStatusId != "All Status" && orderList.OrderStatusId != null)
            {
                orders = orders.Where(o=>o.Orderstatus == orderList.OrderStatusId);
            }
            //date filters
           if(orderList.ToDate !=null && orderList.FromDate!=null){
             orders = orders.Where(o=>o.OrderDate >=  DateOnly.Parse(orderList.FromDate) && o.OrderDate<= DateOnly.Parse(orderList.ToDate));
           }

            orderList.NumberOfItems = await orders.CountAsync();
            var temp = (orderList.CurrentPage-1)*orderList.PageSize;
            orderList.StartIndex = temp+1;
            orderList.EndIndex = temp + orderList.PageSize;
            orderList.TotalPages = (int)Math.Ceiling((double)orderList.NumberOfItems / orderList.PageSize);
            orderList.Items = await orders.Skip(temp).Take(orderList.PageSize).ToListAsync();
        return orderList;
    }

    public async Task<List<OrderViewModel>> OrderListForExport(string? searchFilter,string? dateFilter,string statusFilter ="All Status"){
        var orders =  await (from o in _db.Orders
                            join c in _db.Customers
                            on o.CustomerId equals c.Id
                            join p in _db.Payments
                            on o.Paymentid equals p.Paymentid
                            join s in _db.Orderstatuses
                            on o.Orderstatusid equals s.Orderstatusid
                            select( new OrderViewModel(){
                                OrderId = o.Id.ToString(),
                                OrderDate = DateOnly.FromDateTime((DateTime)o.CreatedAt),
                                CustomerName = c.Name,
                                Orderstatus = s.Orderstatusname,
                                PaymentMode = p.Paymentmode,
                                Rating = (int)o.Rating,
                                TotalAmount = (double)o.TotalAmount,
                            })).ToListAsync();
                        
        if(!string.IsNullOrEmpty(searchFilter)){
            orders = orders.Where(i=>i.CustomerName.Contains(searchFilter.ToLower()) || i.OrderId.Contains(searchFilter.ToLower())).ToList();
        } 
        if(!string.IsNullOrEmpty(dateFilter)){
             orders = orders.Where(o=>o.OrderDate>=DateOnly.Parse(dateFilter)).ToList();
        }
        if(statusFilter != "All Status"){
            orders = orders.Where(o=>o.Orderstatus == statusFilter).ToList();
        }
        return orders;

    }
}
}
