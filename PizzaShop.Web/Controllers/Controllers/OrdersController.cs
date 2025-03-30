using System.Drawing;
using BussinessLogicLayer.Interface;
using DataAccessLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PizzaShopRMS.Helpers;

namespace PizzaShopRMS.Controllers;

public class OrdersController : Controller
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public OrdersController(IOrdersRepository ordersRepository, IWebHostEnvironment webHostEnvironment)
    {
        _ordersRepository = ordersRepository;
        _webHostEnvironment = webHostEnvironment;
    }
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    [CustomAuthorise(new string[] { "SuperAdmin", "AccountManager" }, "User", "Delete")]
    public async Task<IActionResult> OrdersView()
    {
        try
        {
            ViewData["ActivePage"] = "Orders";
            OrderViewModel orderViewModel = new OrderViewModel()
            {
                AllStatus = await _ordersRepository.GetAllStatus()
            };
            return View(orderViewModel);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from Ordercontroller :{ex.Message}");
            return RedirectToAction("OrdersView", "Orders");
        }
    }

    public async Task<IActionResult> OrdersList(Pagination<OrderViewModel> ordersList)
    {
        try
        {
            var data = await _ordersRepository.OrderList(ordersList);
            return PartialView("_OrderListPartialView", data);
        }
        catch (Exception ex)
        {
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/Orders/OrdersView" });
        }
    }
    [HttpPost]
    public async Task<IActionResult> ExportToExcel(string searchFilter, string fromDate, string statusFilter, string dateFilter)
    {
        try{
            var orders = await _ordersRepository.OrderListForExport(searchFilter, fromDate, statusFilter);
        var customColor = Color.FromArgb(0, 102, 167);
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Orders");
            var logoPath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "logos", "pizzashop_logo.png");

            if (System.IO.File.Exists(logoPath))
            {
                var logo = worksheet.Drawings.AddPicture("Logo", new FileInfo(logoPath));
                logo.SetPosition(1, 0, 14, 0);
                logo.SetSize(130, 100);
            }
            else
            {
                worksheet.Cells[2, 15].Value = "Logo not found";
            }

            worksheet.Cells[2, 1, 3, 2].Merge = true;
            worksheet.Cells[2, 1].Value = "Status:";
            worksheet.Cells[2, 3, 3, 6].Merge = true;
            worksheet.Cells[2, 3].Value = statusFilter;

            var statusCellRange = worksheet.Cells[2, 1, 3, 2];
            statusCellRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            statusCellRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            statusCellRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            statusCellRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            statusCellRange.Style.Fill.BackgroundColor.SetColor(customColor);
            statusCellRange.Style.Font.Color.SetColor(Color.White);

            var statusValueCellRange = worksheet.Cells[2, 3, 3, 6];
            statusValueCellRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            statusValueCellRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            statusValueCellRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            statusValueCellRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            statusValueCellRange.Style.Fill.BackgroundColor.SetColor(Color.White);
            statusValueCellRange.Style.Font.Color.SetColor(Color.Black);

            worksheet.Cells[2, 8, 3, 9].Merge = true;
            worksheet.Cells[2, 8].Value = "Search Text:";
            worksheet.Cells[2, 10, 3, 13].Merge = true;
            worksheet.Cells[2, 10].Value = searchFilter;

            var searchCellRange = worksheet.Cells[2, 8, 3, 9];
            searchCellRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            searchCellRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            searchCellRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            searchCellRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            searchCellRange.Style.Fill.BackgroundColor.SetColor(customColor);
            searchCellRange.Style.Font.Color.SetColor(Color.White);

            var searchTextCellRange = worksheet.Cells[2, 10, 3, 13];
            searchTextCellRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            searchTextCellRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            searchTextCellRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            searchTextCellRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            searchTextCellRange.Style.Fill.BackgroundColor.SetColor(Color.White);
            searchTextCellRange.Style.Font.Color.SetColor(Color.Black);


            worksheet.Cells[5, 1, 6, 2].Merge = true;
            worksheet.Cells[5, 1].Value = "Date:";
            worksheet.Cells[5, 3, 6, 6].Merge = true;
            worksheet.Cells[5, 3].Value = dateFilter;


            var dateCellRange = worksheet.Cells[5, 1, 6, 2];
            dateCellRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            dateCellRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            dateCellRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            dateCellRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            dateCellRange.Style.Fill.BackgroundColor.SetColor(customColor);
            dateCellRange.Style.Font.Color.SetColor(Color.White);

            var dateValueCellRange = worksheet.Cells[5, 3, 6, 6];
            dateValueCellRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            dateValueCellRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            dateValueCellRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            dateValueCellRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            dateValueCellRange.Style.Fill.BackgroundColor.SetColor(Color.White);
            dateValueCellRange.Style.Font.Color.SetColor(Color.Black);

            worksheet.Cells[5, 8, 6, 9].Merge = true;
            worksheet.Cells[5, 8].Value = "No. of Records:";
            worksheet.Cells[5, 10, 6, 13].Merge = true;
            worksheet.Cells[5, 10].Value = orders.Count;

            var recordCellRange = worksheet.Cells[5, 8, 6, 9];
            recordCellRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            recordCellRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            recordCellRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            recordCellRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            recordCellRange.Style.Fill.BackgroundColor.SetColor(customColor);
            recordCellRange.Style.Font.Color.SetColor(Color.White);

            var recordValueCellRange = worksheet.Cells[5, 10, 6, 13];
            recordValueCellRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            recordValueCellRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            recordValueCellRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
            recordValueCellRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            recordValueCellRange.Style.Fill.BackgroundColor.SetColor(Color.White);
            recordValueCellRange.Style.Font.Color.SetColor(Color.Black);

            worksheet.Cells[9, 1].Value = "Id";
            worksheet.Cells[9, 2, 9, 4].Merge = true;
            worksheet.Cells[9, 2].Value = "Date";
            worksheet.Cells[9, 5, 9, 7].Merge = true;
            worksheet.Cells[9, 5].Value = "Customer";
            worksheet.Cells[9, 8, 9, 10].Merge = true;
            worksheet.Cells[9, 8].Value = "Status";
            worksheet.Cells[9, 11, 9, 12].Merge = true;
            worksheet.Cells[9, 11].Value = "Payment Mode";
            worksheet.Cells[9, 13, 9, 14].Merge = true;
            worksheet.Cells[9, 13].Value = "Rating";
            worksheet.Cells[9, 15, 9, 16].Merge = true;
            worksheet.Cells[9, 15].Value = "Total Amount";


            var headerRange = worksheet.Cells[9, 1, 9, 16];
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            headerRange.Style.Fill.BackgroundColor.SetColor(customColor);
            headerRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            headerRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            headerRange.Style.Font.Color.SetColor(Color.White);
            headerRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);

            int row = 10;
            foreach (var order in orders)
            {
                worksheet.Cells[row, 1].Value = order.OrderId;
                worksheet.Cells[row, 2, row, 4].Merge = true;
                worksheet.Cells[row, 2].Value = order.OrderDate.ToString();
                worksheet.Cells[row, 5, row, 7].Merge = true;
                worksheet.Cells[row, 5].Value = order.CustomerName;
                worksheet.Cells[row, 8, row, 10].Merge = true;
                worksheet.Cells[row, 8].Value = order.Orderstatus;
                worksheet.Cells[row, 11, row, 12].Merge = true;
                worksheet.Cells[row, 11].Value = order.PaymentMode;
                worksheet.Cells[row, 13, row, 14].Merge = true;
                worksheet.Cells[row, 13].Value = order.Rating;
                worksheet.Cells[row, 15, row, 16].Merge = true;
                worksheet.Cells[row, 15].Value = order.TotalAmount.ToString();

                var dataRowRange = worksheet.Cells[row, 1, row, 16];
                dataRowRange.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                dataRowRange.Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                dataRowRange.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                row++;
            }
            var entireRange = worksheet.Cells[9, 1, row - 1, 16];
            entireRange.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

            using (var stream = new MemoryStream())
            {
                package.SaveAs(stream);
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Orders.xlsx"); // Return as byte array
            }
        }
        
        }
        catch(Exception ex){
            TempData["ToasterMessage"] = "An Unexpected error got occured";
            TempData["ToasterType"] = "error";
            Console.WriteLine($"Error from controller :{ex.Message}");
            return Json(new { success = false, redirectUrl = "/Orders/OrdersView" });
        } 
    }

    public async Task<IActionResult> InvoiceView(){
        return View();
    }
}
