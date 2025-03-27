using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class RolePermission
{
    public int Id { get; set; }

    public int PermissionId { get; set; }

    public int RoleId { get; set; }

    public bool CanView { get; set; }

    public bool CanAddEdit { get; set; }

    public bool CanDelete { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Permission Permission { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;
}
