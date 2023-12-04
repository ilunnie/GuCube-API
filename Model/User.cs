using System;
using System.Collections.Generic;

namespace GuCube.Model;

public partial class User
{
    public int Id { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Salt { get; set; } = null!;

    public virtual ICollection<Coupon> Coupons { get; } = new List<Coupon>();

    public virtual ICollection<StoreManager> StoreManagers { get; } = new List<StoreManager>();
}
