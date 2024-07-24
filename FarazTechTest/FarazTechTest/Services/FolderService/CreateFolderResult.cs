namespace FarazTechTest.Services.FolderService
{
    public class CreateFolderResult
    {
        public bool ValidationError { get; set; }
        public string? ErrorMessage { get; set; }
        public int? FolderId { get; set; }
    }
}
