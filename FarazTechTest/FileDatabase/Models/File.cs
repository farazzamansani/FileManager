using System;
using System.Collections.Generic;

namespace FileDatabase.Models;

public partial class File
{
    public int Fileid { get; set; }

    public string Name { get; set; } = null!;

    public int Folderid { get; set; }

    public byte[] Filedata { get; set; } = null!;
    
    public string Filetype { get; set; } = null!;

    public DateTimeOffset UploadedDateTime { get; set; }

    public virtual Folder Folder { get; set; }
}
