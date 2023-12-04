using System;
using System.Collections.Generic;

namespace GuCube.Model;

public partial class Coupon
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public int UserId { get; set; }

    public int MenuId { get; set; }

    public int? OrderId { get; set; }

    public double Discount { get; set; }

    public virtual Menu Menu { get; set; } = null!;

    public virtual Order? Order { get; set; }

    public virtual User User { get; set; } = null!;
}
