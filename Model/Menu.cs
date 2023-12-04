using System;
using System.Collections.Generic;

namespace GuCube.Model;

public partial class Menu
{
    public int Id { get; set; }

    public int StoreId { get; set; }

    public int ProductId { get; set; }

    public double Price { get; set; }

    public virtual ICollection<Coupon> Coupons { get; } = new List<Coupon>();

    public virtual Product Product { get; set; } = null!;

    public virtual Store Store { get; set; } = null!;
}
