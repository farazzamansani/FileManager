namespace FarazTechTest.Services.FolderService
{
    public class CreateFileResult
    {
        public bool ValidationError { get; set; }
        public string? ErrorMessage { get; set; }
        public int? FileId { get; set; }
    }
}
