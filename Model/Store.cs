using System;
using System.Collections.Generic;

namespace GuCube.Model;

public partial class Store
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Localization { get; set; } = null!;

    public virtual ICollection<Menu> Menus { get; } = new List<Menu>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<StoreManager> StoreManagers { get; } = new List<StoreManager>();
}
