namespace FarazTechTest.Models
{
    public class FileDataDto
    {
        public string FileName { get; set; }
        public int? FolderId { get; set; }
        public IFormFile file { get; set; }
        public byte[] fileBytes { get; set; }
    }
}
