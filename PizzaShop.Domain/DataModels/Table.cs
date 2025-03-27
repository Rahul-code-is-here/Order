using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class Table
{
    public int Id { get; set; }

    public int SectionId { get; set; }

    public string Name { get; set; } = null!;

    public int Capacity { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public bool? Status { get; set; }

    public string? Newstatus { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Section Section { get; set; } = null!;

    public virtual ICollection<TableOrderMapping> TableOrderMappings { get; set; } = new List<TableOrderMapping>();

    public virtual ICollection<WaitingToken> WaitingTokens { get; set; } = new List<WaitingToken>();
}
