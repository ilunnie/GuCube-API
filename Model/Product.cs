using System;
using System.Collections.Generic;

namespace GuCube.Model;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int Image { get; set; }

    public virtual Image ImageNavigation { get; set; } = null!;

    public virtual ICollection<Ingredient> Ingredients { get; } = new List<Ingredient>();

    public virtual ICollection<Menu> Menus { get; } = new List<Menu>();

    public virtual ICollection<OrderProduct> OrderProducts { get; } = new List<OrderProduct>();
}
