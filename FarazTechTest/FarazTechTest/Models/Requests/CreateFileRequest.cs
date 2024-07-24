using System.ComponentModel.DataAnnotations;

namespace FarazTechTest.Models.Requests
{
    public class CreateFileRequest
    {
        [Required(ErrorMessage = "ParentFolderId is required.")]
        public required int ParentFolderId { get; set; }
        public IFormFile File { get; set; }
    }
}
