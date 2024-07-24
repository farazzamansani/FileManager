using System.ComponentModel.DataAnnotations;

namespace FarazTechTest.Models.Requests
{
    public class CreateFolderRequest
    {
        [Required(ErrorMessage = "FolderName is required.")]
        public required string FolderName { get; set; }
        public int? ParentFolderId { get; set; }
    }
}
