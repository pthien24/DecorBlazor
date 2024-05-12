using DecorWeb_Server.Service.IService;
using Microsoft.AspNetCore.Components.Forms;

namespace DecorWeb_Server.Service
{
    public class FileUpload : IFileUpload
    {
        private readonly IWebHostEnvironment _environment;
        public FileUpload(IWebHostEnvironment environment) 
        {
            _environment = environment;

        }
        public  bool DeleteFile(string filePath)
        {
            if (File.Exists(_environment.WebRootPath+ filePath))
            {
                File.Delete(_environment.WebRootPath+ filePath);
                return true;
            }
            return false;
        }

        public async Task<string> UploadFile(IBrowserFile file)
        {
            FileInfo fileInfo = new(file.Name);
            var fileName = Guid.NewGuid().ToString()+fileInfo.Extension;
            var folderDir = $"{_environment.WebRootPath}\\images\\Product";

            if (!Directory.Exists(folderDir))
            {
                Directory.CreateDirectory(folderDir);
            }
            var filePath = Path.Combine(folderDir, fileName);

            await using FileStream fs = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(fs);
            var fullPath = $"/images/product/{fileName}";
            return fullPath;
        }
    }
}
