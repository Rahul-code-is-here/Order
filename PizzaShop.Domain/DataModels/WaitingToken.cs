using System;
using System.Collections.Generic;

namespace PizzaShop.Domain.DataModels;

public partial class WaitingToken
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public int NoOfPersons { get; set; }

    public int SectionId { get; set; }

    public int? TableId { get; set; }

    public bool? IsDeleted { get; set; }

    public bool? IsAssign { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? ModifiedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual User? ModifiedByNavigation { get; set; }

    public virtual Section Section { get; set; } = null!;

    public virtual Table? Table { get; set; }
}
