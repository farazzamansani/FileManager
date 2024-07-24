using System;
using System.Collections.Generic;

namespace FileDatabase.Models;

public partial class Folder
{
    public int Folderid { get; set; }

    public string Name { get; set; } = null!;

    public int? Parentfolderid { get; set; }

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<Folder> InverseParentfolder { get; set; } = new List<Folder>();

    public virtual Folder? Parentfolder { get; set; }
}
