using System;
using System.Collections.Generic;

namespace GuCube.Model;

public partial class StoreManager
{
    public int Id { get; set; }

    public int StoreId { get; set; }

    public int UserId { get; set; }

    public virtual Store Store { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
