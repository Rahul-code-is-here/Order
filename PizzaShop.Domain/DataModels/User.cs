using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Adress { get; set; }

    public string? Phone { get; set; }

    public int RoleId { get; set; }

    public int CountryId { get; set; }

    public int StateId { get; set; }

    public int CityId { get; set; }

    public string? Zipcode { get; set; }

    public string? ProfileImage { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public bool IsDeleted { get; set; }

    public string? Resettoken { get; set; }

    public DateTime? Resettokenexpiry { get; set; }

    public string? Status { get; set; }

    public bool? Isfirstlogin { get; set; }

    public virtual ICollection<Account> AccountCreatedByNavigations { get; set; } = new List<Account>();

    public virtual ICollection<Account> AccountModifiedByNavigations { get; set; } = new List<Account>();

    public virtual ICollection<Category> CategoryCreatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<Category> CategoryModifiedByNavigations { get; set; } = new List<Category>();

    public virtual City City { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Customer> CustomerCreatedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Customer> CustomerModifiedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Feedback> FeedbackCreatedByNavigations { get; set; } = new List<Feedback>();

    public virtual ICollection<Feedback> FeedbackModifiedByNavigations { get; set; } = new List<Feedback>();

    public virtual ICollection<MappingMenuItemWithModifier> MappingMenuItemWithModifierCreatedByNavigations { get; set; } = new List<MappingMenuItemWithModifier>();

    public virtual ICollection<MappingMenuItemWithModifier> MappingMenuItemWithModifierModifiedByNavigations { get; set; } = new List<MappingMenuItemWithModifier>();

    public virtual ICollection<Menuitem> MenuitemCreatedByNavigations { get; set; } = new List<Menuitem>();

    public virtual ICollection<Menuitem> MenuitemModifiedByNavigations { get; set; } = new List<Menuitem>();

    public virtual ICollection<Modifier> ModifierCreatedByNavigations { get; set; } = new List<Modifier>();

    public virtual ICollection<Modifier> ModifierModifiedByNavigations { get; set; } = new List<Modifier>();

    public virtual ICollection<Modifiergroup> ModifiergroupCreatedByNavigations { get; set; } = new List<Modifiergroup>();

    public virtual ICollection<Modifiergroup> ModifiergroupModifiedByNavigations { get; set; } = new List<Modifiergroup>();

    public virtual ICollection<Order> OrderCreatedByNavigations { get; set; } = new List<Order>();

    public virtual ICollection<OrderItem> OrderItemCreatedByNavigations { get; set; } = new List<OrderItem>();

    public virtual ICollection<OrderItem> OrderItemModifiedByNavigations { get; set; } = new List<OrderItem>();

    public virtual ICollection<Order> OrderModifiedByNavigations { get; set; } = new List<Order>();

    public virtual ICollection<OrderTaxMapping> OrderTaxMappingCreatedByNavigations { get; set; } = new List<OrderTaxMapping>();

    public virtual ICollection<OrderTaxMapping> OrderTaxMappingModifiedByNavigations { get; set; } = new List<OrderTaxMapping>();

    public virtual ICollection<OrderedItem> OrderedItemCreatedByNavigations { get; set; } = new List<OrderedItem>();

    public virtual ICollection<OrderedItem> OrderedItemModifiedByNavigations { get; set; } = new List<OrderedItem>();

    public virtual ICollection<OrderedItemModifierMapping> OrderedItemModifierMappingCreatedByNavigations { get; set; } = new List<OrderedItemModifierMapping>();

    public virtual ICollection<OrderedItemModifierMapping> OrderedItemModifierMappingModifiedByNavigations { get; set; } = new List<OrderedItemModifierMapping>();

    public virtual ICollection<Permission> PermissionCreatedByNavigations { get; set; } = new List<Permission>();

    public virtual ICollection<Permission> PermissionModifiedByNavigations { get; set; } = new List<Permission>();

    public virtual ICollection<Role> RoleCreatedByNavigations { get; set; } = new List<Role>();

    public virtual ICollection<Role> RoleModifiedByNavigations { get; set; } = new List<Role>();

    public virtual ICollection<RolePermission> RolePermissionCreatedByNavigations { get; set; } = new List<RolePermission>();

    public virtual ICollection<RolePermission> RolePermissionModifiedByNavigations { get; set; } = new List<RolePermission>();

    public virtual ICollection<Section> SectionCreatedByNavigations { get; set; } = new List<Section>();

    public virtual ICollection<Section> SectionModifiedByNavigations { get; set; } = new List<Section>();

    public virtual State State { get; set; } = null!;

    public virtual ICollection<Table> TableCreatedByNavigations { get; set; } = new List<Table>();

    public virtual ICollection<Table> TableModifiedByNavigations { get; set; } = new List<Table>();

    public virtual ICollection<TableOrderMapping> TableOrderMappingCreatedByNavigations { get; set; } = new List<TableOrderMapping>();

    public virtual ICollection<TableOrderMapping> TableOrderMappingModifiedByNavigations { get; set; } = new List<TableOrderMapping>();

    public virtual ICollection<TaxesAndFee> TaxesAndFeeCreatedByNavigations { get; set; } = new List<TaxesAndFee>();

    public virtual ICollection<TaxesAndFee> TaxesAndFeeModifiedByNavigations { get; set; } = new List<TaxesAndFee>();

    public virtual ICollection<Unit> UnitCreatedByNavigations { get; set; } = new List<Unit>();

    public virtual ICollection<Unit> UnitModifiedByNavigations { get; set; } = new List<Unit>();

    public virtual ICollection<WaitingToken> WaitingTokenCreatedByNavigations { get; set; } = new List<WaitingToken>();

    public virtual ICollection<WaitingToken> WaitingTokenModifiedByNavigations { get; set; } = new List<WaitingToken>();
}
