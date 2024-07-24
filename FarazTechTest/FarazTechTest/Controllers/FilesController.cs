using FarazTechTest.Models.Requests;
using FarazTechTest.Services.FileService;
using Microsoft.AspNetCore.Mvc;

namespace FarazTechTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly ILogger<FilesController> _logger;

        public FilesController(ILogger<FilesController> logger, IFileService fileService)
        {
            _logger = logger;
            _fileService = fileService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateFile(CreateFileRequest createFileRequest)
        {
            var result = await _fileService.CreateFileAsync(createFileRequest);
            if (result.ValidationError)
            {
                return StatusCode(400, result.ErrorMessage);
            }
            return Ok(result.FileId);
        }
        
        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetFileData([FromRoute] int fileId)
        {
            var files = await _fileService.GetFileDataAsync(fileId);

            return File(files.fileBytes, files.file.ContentType, files.FileName);
        }
    }
}
