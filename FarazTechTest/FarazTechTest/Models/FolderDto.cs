namespace Models
{
    public class FolderDto
    {
        public string Name { get; set; }
        public int FolderId { get; set; }
        public FolderDto[] SubFolders { get; set; }
        public FileDto[] Files { get; set; }
    }
}
