using FarazTechTest.Models.Requests;
using FarazTechTest.Services.FileService;
using FarazTechTest.Services.FolderService;
using Microsoft.AspNetCore.Mvc;


namespace FarazTechTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FoldersController : ControllerBase
    {
        private IFolderService _folderService;
        private readonly IFileService _fileService;
        private readonly ILogger<FilesController> _logger;

        public FoldersController(ILogger<FilesController> logger, IFolderService folderService, IFileService fileService)
        {
            _logger = logger;
            _folderService = folderService;
            _fileService = fileService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetFolders()
        {
            var root = await _folderService.GetFoldersAsync();
            return Ok(root);
        }

        [HttpGet("{folderId}/files")]
        public async Task<IActionResult> GetFiles([FromRoute] int folderId)
        {
            var files = await _fileService.GetFilesAsync(folderId);

            return Ok(files);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateFolder(CreateFolderRequest createFolderRequest)
        {
            var result = await _folderService.CreateFolderAsync(createFolderRequest);
            if (result.ValidationError)
            {
                return StatusCode(400, result.ErrorMessage);
            }
            return Ok(result.FolderId);
        }

    }
}
