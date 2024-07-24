using FarazTechTest.Models;
using FarazTechTest.Models.Requests;
using FarazTechTest.Services.FolderService;
using Models;

namespace FarazTechTest.Services.FileService;

public interface IFileService
{
    Task<FileDto[]> GetFilesAsync(int folderId);
    Task<FileDataDto> GetFileDataAsync(int fileId);
    Task<CreateFileResult> CreateFileAsync(CreateFileRequest createFileRequest);
}