using Chat.Api.FileStoreModule.Commands;
using Chat.Api.FileStoreModule.Interfaces;
using Chat.Api.FileStoreModule.Models;
using Chat.Api.IdentityModule.Interfaces;
using Chat.Framework.Attributes;
using Chat.Framework.CQRS;
using Chat.Framework.Extensions;
using Chat.Framework.Mediators;

namespace Chat.Api.FileStoreModule.CommandHandlers
{
    [ServiceRegister(typeof(IRequestHandler<UploadFileCommand, CommandResponse>), ServiceLifetime.Singleton)]
    public class UploadFileCommandHandler : ACommandHandler<UploadFileCommand>
    {
        private readonly IFileRepository _fileRepository;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UploadFileCommandHandler(IFileRepository fileRepository, ITokenService tokenService, IHttpContextAccessor httpContextAccessor)
        {
            _fileRepository = fileRepository;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
        }
        
        protected override async Task<CommandResponse> OnHandleAsync(UploadFileCommand command)
        {
            var response = command.CreateResponse();
            var file = command.FormFile;
            var pathToSave = "FileStoreModule\\Store";
            if (file.Length <= 0) 
            {
                throw new Exception("File Length 0");
            }
            var fileName = file.FileName;
            var fileId = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(fileName);
            var fullPath = Path.Combine(pathToSave, fileId + extension);
            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var requestContextFromAccessor = _httpContextAccessor.HttpContext;
            var currentUser = _tokenService.GetUserProfileFromAccessToken(requestContextFromAccessor?.GetAccessToken());
            var fileModel = new FileModel()
            {
                Id = fileId,
                Extension = extension,
                Url = fullPath,
                UploadedAt = DateTime.UtcNow,
                Name = fileName,
                UserId = currentUser?.Id ?? ""
            };
            if (!await _fileRepository.SaveFileModelAsync(fileModel)) 
            {
                throw new Exception("File Save error to db");
            }
            response.Message = "File uploaded successfully";
            response.SetData("FileId", fileModel.Id);
            return response;
        }
    }
}