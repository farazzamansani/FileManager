using FarazTechTest.Models;
using FarazTechTest.Models.Requests;
using FarazTechTest.Services.FolderService;
using FileDatabase.Models;
using Microsoft.EntityFrameworkCore;
using Models;
using File = FileDatabase.Models.File;

namespace FarazTechTest.Services.FileService
{
    public class FileService : IFileService
    {
        private readonly FileDatabaseContext _context;
        //Ideally we have a FileTypes table and then rely on the types there to distinguish what we allow here, that would be a better way to fk the files to a type as well.
        private static readonly string[] ALLOWED_FILE_TYPES = new string[] { ".csv",".geojson" };

        public FileService(FileDatabaseContext context)
        {
            _context = context;
        }

        public async Task<FileDto[]> GetFilesAsync(int folderId)
        {
            var files = await _context.Files.Where(f => f.Folderid == folderId)
                .Select(f => new FileDto()
                {
                    FileName = f.Name,
                    FolderId = f.Folderid
                })
                .ToArrayAsync();
            return files;
        }

        public async Task<FileDataDto> GetFileDataAsync(int fileId)
        {
            var file = await _context.Files.SingleOrDefaultAsync(f => f.Fileid == fileId);

            var fileData = new FileDataDto()
            {
                FileName = file.Name,
                FolderId = file.Folderid,
                file = ByteArrayToIFormFile(file.Filedata, file.Name), //We are relying on the FormFile to determine the Content Type
                fileBytes = file.Filedata
            };
            return fileData;
        }

        public IFormFile ByteArrayToIFormFile(byte[] fileBytes, string fileName)
        {
            using var memoryStream = new MemoryStream(fileBytes);
            var file = new FormFile(memoryStream, 0, memoryStream.Length, fileName, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/octet-stream" // or set the correct content type
            };

            return file;
        }

        public async Task<CreateFileResult> CreateFileAsync(CreateFileRequest createFileRequest)
        {
            var fileType = Path.GetExtension(createFileRequest.File.FileName);
            if (!ALLOWED_FILE_TYPES.Contains(fileType.ToLower()))
            {
                return new CreateFileResult()
                {
                    ValidationError = true,
                    ErrorMessage = $"File Type {fileType} not valid"
                };
            }
            using var memoryStream = new MemoryStream();
            await createFileRequest.File.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            

            var file = new File()
            {
                Name = createFileRequest.File.FileName,
                Folderid = createFileRequest.ParentFolderId,
                Filedata = fileBytes,
                Filetype = fileType,
                UploadedDateTime = DateTimeOffset.Now
            };
            _context.Files.Add(file);
            await _context.SaveChangesAsync();

            return new CreateFileResult()
            {
                ValidationError = false,
                FileId = file.Fileid
            };
        }

    }


}
