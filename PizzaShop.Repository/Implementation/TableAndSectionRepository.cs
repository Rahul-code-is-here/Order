using System.Security.Cryptography.X509Certificates;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Domain.ViewModels;
using PizzaShop.Domain.DataModels;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Domain.DataContext;
using Microsoft.VisualBasic;

namespace BussinessLogicLayer.Repository;

public class TableAndSectionRepository : ITableAndSectionRepository
{
    private readonly PizzaShemaContext _db;
    public TableAndSectionRepository(PizzaShemaContext db)
    {
        _db = db;
    }
    public async Task<List<SectionViewModel>> GetSectionList()
    {
        var SectionList = await _db.Sections.Where(i=>i.IsDeleted==false).OrderBy(i => i.SectionName).Select(i => new SectionViewModel()
        {
            SectionName = i.SectionName,
            SectionId = i.SectionId.ToString()
        }).ToListAsync();
        return SectionList;
    }

    public async Task<Pagination<TableViewModel>> GetTablesListForSection(Pagination<TableViewModel> tableList, string sectionId)
    {
        var items = _db.Tables.Where(i => i.SectionId.ToString() == sectionId && i.IsDeleted==false).OrderBy(i=>i.Name).Select(i => new TableViewModel()
        {
            SectionId = sectionId,
            TableName = i.Name,
            TableId = i.Id.ToString(),
            Capacity = i.Capacity.ToString(),
            status = (bool)i.Status?1:0,
            Newstatus=i.Newstatus

        });
        if (!string.IsNullOrEmpty(tableList.SearchFilter))
        {
            items = items.Where(i => i.TableName.ToLower().Contains(tableList.SearchFilter.Trim().ToLower()));
        }
        tableList.NumberOfItems = await items.CountAsync();

        var temp = (tableList.CurrentPage - 1) * tableList.PageSize;
        tableList.StartIndex = temp + 1;
        tableList.EndIndex = temp + tableList.PageSize;
        tableList.TotalPages = (int)Math.Ceiling((double)tableList.NumberOfItems / tableList.PageSize);
        tableList.Items = await items.Skip(temp).Take(tableList.PageSize).ToListAsync();
        return tableList;
    }
    public async Task<string> AddSection(SectionViewModel AddSection)
    {
        var IsSection = await _db.Sections.AnyAsync(u => u.SectionName.ToLower() == AddSection.SectionName.Trim().ToLower());
        if (IsSection)
        {
            return "Section already exist";
        }
        Section section = new Section()
        {
            SectionName = AddSection.SectionName,
            // CreatedBy = AddSection.CreatedBy,
        };
        if (AddSection.Description != null)
        {
            section.Description = AddSection.Description;
        }
        await _db.Sections.AddAsync(section);
        await _db.SaveChangesAsync();
        return "Section Added Successfully";
    }

    public async Task<SectionViewModel> GetSectionForEdit(string sectionId)
    {
        var section = await (from s in _db.Sections
                             where s.SectionId.ToString() == sectionId
                             select new SectionViewModel()
                             {
                                 SectionName = s.SectionName,
                                 Description = s.Description,
                                 SectionId = s.SectionId.ToString()
                             }).FirstOrDefaultAsync();
        return section;
    }

    public async Task<string> EditSection(SectionViewModel AddSection)
    {
        var IsSection = await _db.Sections.AnyAsync(u => u.SectionName.ToLower() == AddSection.SectionName.Trim().ToLower() && u.SectionId.ToString() != AddSection.SectionId);
        if (IsSection)
        {
            return "Section with the name already exist";
        }
        var section = await _db.Sections.FirstOrDefaultAsync(c => c.SectionId.ToString() == AddSection.SectionId);
        section.SectionName = AddSection.SectionName;
        // section.ModifiedBy = AddSection.EditedBy;
        // section.EditDate = DateTime.Now;
        if (AddSection.Description != null)
        {
            section.Description = AddSection.Description;
        }
        await _db.SaveChangesAsync();
        return "Section Edited Successfully";
    }

    public async Task<string> DeleteSection(string SectionId)
    {
        var section = await _db.Sections.FirstOrDefaultAsync(s => s.SectionId.ToString() == SectionId);
        if (section == null)
        {
            return "error while deleting section";
        }
        section.IsDeleted = true;
        var table = await _db.Tables.Where(t => t.SectionId.ToString() == SectionId).ToListAsync();
        foreach (var obj in table)
        {
            obj.IsDeleted = true;
        }
        await _db.SaveChangesAsync();
        return "section Deleted Successfully";
    }

    
    
    public async Task<string> AddTable(TableViewModel AddTable){
        var isTable = await _db.Tables.AnyAsync(i=>i.Name.ToLower()==AddTable.TableName.Trim().ToLower() && i.SectionId.ToString()==AddTable.SectionId);
        if(isTable){
            return "Table with the name already exist";
        }
        var statusMap =new Dictionary<int, String>{
           {0,"Available"},
           {1,"Assigned"},
           {2,"Reserved"},
           {3,"Selected"},
        };
        Table table = new Table(){
            Name=AddTable.TableName,
            SectionId = int.Parse(AddTable.SectionId),
            Capacity = int.Parse(AddTable.Capacity),
            Status = AddTable.status == 0 ? false : true, 
            Newstatus= statusMap[int.Parse(AddTable.Newstatus)]
        };
        await _db.Tables.AddAsync(table);
        await _db.SaveChangesAsync();
        return "Table Added Successfully";
    }

    public async Task<TableViewModel> GetTableForEdit(string tableId){
        var table = await (from t in _db.Tables
                             where t.Id.ToString() == tableId
                             select new TableViewModel()
                             {
                                TableId = t.Id.ToString(),
                                SectionId = t.SectionId.ToString(),
                                Capacity = t.Capacity.ToString(),
                                TableName = t.Name,
                                status = (bool)t.Status?1:0,
                                Newstatus= t.Newstatus
                             }).FirstOrDefaultAsync();
        return table;
    }

    public async Task<string> EditTable(TableViewModel editTable){
        var IsTable = await _db.Tables.AnyAsync(u => u.Name.ToLower() == editTable.TableName.Trim().ToLower() && u.Id.ToString() != editTable.TableId &&u.SectionId.ToString()==editTable.SectionId);
        if (IsTable)
        {
            return "Table with the name already exist";
        }
         var statusMap =new Dictionary<int, String>{
           {0,"Available"},
           {1,"Assigned"},
           {2,"Reserved"},
           {3,"Selected"},
        };
        var table = await _db.Tables.FirstOrDefaultAsync(t => t.Id.ToString() == editTable.TableId);
        table.Name = editTable.TableName;
        table.SectionId=int.Parse(editTable.SectionId);
        table.Capacity = int.Parse(editTable.Capacity);
        table.Status= editTable.status == 0 ? false : true;
        table.Newstatus= statusMap[int.Parse(editTable.Newstatus)];
        // table.EditedBy = editTable.EditedBy;
        // table.EditDate = DateTime.Now;
        await _db.SaveChangesAsync();
        return "Table Edited Successfully";
    }

    public async Task<string> DeleteTable(List<string> tableIds){
        var tables = await _db.Tables.Where(i => tableIds.Contains(i.Id.ToString())).ToListAsync();
        foreach (var table in tables)
        {
            table.IsDeleted = true;
        }
        await _db.SaveChangesAsync();
        return "Tables Deleted Successfully";
    }
}