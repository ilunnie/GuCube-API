using System;
using System.Collections.Generic;

namespace GuCube.Model;

public partial class Image
{
    public int Id { get; set; }

    public byte[] Foto { get; set; } = null!;

    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
