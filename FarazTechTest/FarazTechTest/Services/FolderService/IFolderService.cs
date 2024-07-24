using FarazTechTest.Models.Requests;
using Models;

namespace FarazTechTest.Services.FolderService;

public interface IFolderService
{
    Task<FolderDto[]> GetFoldersAsync();
    Task<CreateFolderResult> CreateFolderAsync(CreateFolderRequest createFolderRequest);
}