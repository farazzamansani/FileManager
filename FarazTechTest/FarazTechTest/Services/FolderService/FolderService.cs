using FarazTechTest.Models.Requests;
using FileDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Models;

namespace FarazTechTest.Services.FolderService
{
    public class FolderService : IFolderService
    {
        private readonly FileDatabaseContext _context;
        public FolderService(FileDatabaseContext context)
        {
            _context = context;
        }

        public async Task<FolderDto[]> GetFoldersAsync()
        {
            var root = await _context.Folders.Where(f => f.Parentfolder == null).Include(f=>f.Files).ToArrayAsync();
            await LoadFolders(root);
            FolderDto[] response = root.Select(MapToDto).ToArray()!;
            return response;
        }

        /// <summary>
        /// Returning the full directory, need to load all subfolders in this method recursively as it is variable depth.
        /// </summary>
        /// <param name="root"></param>
        /// <returns></returns>
        private async Task LoadFolders(Folder[] root)
        {
            foreach (var rootFolder in root)
            {
                await LoadSubFolders(rootFolder);
            }
        }

        private async Task LoadSubFolders(Folder folder)
        {
            await _context.Folders.Entry(folder).Collection(f => f.InverseParentfolder).LoadAsync();
            await _context.Folders.Entry(folder).Collection(f => f.Files).LoadAsync();
            foreach (var subFolder in folder.InverseParentfolder)
            {
                await LoadSubFolders(subFolder);
            }
        }

        public async Task<CreateFolderResult> CreateFolderAsync(CreateFolderRequest createFolderRequest)
        {
            var parentIsNullOrExists = createFolderRequest.ParentFolderId == null || _context.Folders.Any(f => f.Folderid == createFolderRequest.ParentFolderId);
            if (!parentIsNullOrExists)
            {
                return new CreateFolderResult()
                {
                    ValidationError = true,
                    ErrorMessage = "Parent Folder does not exist"
                };
            }
            var folder = new Folder()
            {
                Name = createFolderRequest.FolderName,
                Parentfolderid = createFolderRequest.ParentFolderId
            };
            _context.Folders.Add(folder);
            await _context.SaveChangesAsync();

            return new CreateFolderResult()
            {
                ValidationError = false,
                FolderId = folder.Folderid
            };
        }

        private FolderDto? MapToDto(Folder? folder)
        {
            if (folder == null) return null;

            return new FolderDto
            {
                Name = folder.Name,
                FolderId = folder.Folderid,
                SubFolders = folder.InverseParentfolder.Select(MapToDto).Where(dto => dto != null).ToArray()!,
                Files = folder.Files.Select(f => new FileDto()
                {
                    FileName = f.Name,
                    FolderId = folder.Folderid,
                    FileId = f.Fileid
                }).ToArray()
            };
        }
    }


}
