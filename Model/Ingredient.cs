using System;
using System.Collections.Generic;

namespace GuCube.Model;

public partial class Ingredient
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public int Amount { get; set; }

    public string Unit { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
