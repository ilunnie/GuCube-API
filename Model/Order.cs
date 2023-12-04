using System;
using System.Collections.Generic;

namespace GuCube.Model;

public partial class Order
{
    public int Id { get; set; }

    public int StoreId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsDelivered { get; set; }

    public virtual ICollection<Coupon> Coupons { get; } = new List<Coupon>();

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();

    public virtual Store Store { get; set; } = null!;
}
