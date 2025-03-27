using PizzaShop.Domain.ViewModels;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Interface;

namespace PizzaShop.Service.Implementaion;


public class TableAndSectionService: ITableAndSectionService
{

     private readonly ITableAndSectionRepository _tableAndSectionRepository;

    public TableAndSectionService(ITableAndSectionRepository tableAndSectionRepository)
    {
        _tableAndSectionRepository = tableAndSectionRepository;
    }

    public async Task<List<SectionViewModel>> GetSectionList()
    {
        return await _tableAndSectionRepository.GetSectionList();
    }

    public async Task<Pagination<TableViewModel>> GetTablesListForSection(Pagination<TableViewModel> tableList, string sectionId)
    {
        return await _tableAndSectionRepository.GetTablesListForSection(tableList, sectionId);
    }

    public async Task<string> AddSection(SectionViewModel addSection)
    {
        return await _tableAndSectionRepository.AddSection(addSection);
    }

    public async Task<SectionViewModel> GetSectionForEdit(string sectionId)
    {
        return await _tableAndSectionRepository.GetSectionForEdit(sectionId);
    }

    public async Task<string> EditSection(SectionViewModel addSection)
    {
        return await _tableAndSectionRepository.EditSection(addSection);
    }

    public async Task<string> DeleteSection(string sectionId)
    {
        return await _tableAndSectionRepository.DeleteSection(sectionId);
    }

    public async Task<string> AddTable(TableViewModel addTable)
    {
        return await _tableAndSectionRepository.AddTable(addTable);
    }

    public async Task<TableViewModel> GetTableForEdit(string tableId)
    {
        return await _tableAndSectionRepository.GetTableForEdit(tableId);
    }

    public async Task<string> EditTable(TableViewModel editTable)
    {
        return await _tableAndSectionRepository.EditTable(editTable);
    }

    public async Task<string> DeleteTable(List<string> tableIds)
    {
        return await _tableAndSectionRepository.DeleteTable(tableIds);
    }
      
}
