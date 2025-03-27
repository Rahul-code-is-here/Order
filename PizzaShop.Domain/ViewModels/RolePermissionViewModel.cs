namespace PizzaShop.Domain.ViewModels;
using System.Collections.Generic;
using PizzaShop.Domain.DataModels;


public class RolePermissionViewModel
{
//    public int RoleId { get; set; }
//         public List<Role> Roles { get; set; }
//         public List<Permission> Permissions { get; set; }
//         public List<RolePermission> RolePermissions { get; set; }

  public int? PermissionId { get; set; }
    public int RoleId { get; set; }
    public bool? CanView { get; set; }
    public bool? CanAddEdit { get; set; }
    public bool? CanDelete { get; set; }
    public string PermissionName { get; set; }

}


