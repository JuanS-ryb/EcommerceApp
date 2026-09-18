using System;
using System.Collections.Generic;

namespace Ecommerce.server.Models;

public partial class SalesGoal
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public byte Quarter { get; set; }

    public short Year { get; set; }

    public decimal PesosGoal { get; set; }

    public int UnitsGoal { get; set; }

    public decimal ExecutedPesos { get; set; }

    public int ExecutedUnits { get; set; }

    public virtual User User { get; set; } = null!;
}
