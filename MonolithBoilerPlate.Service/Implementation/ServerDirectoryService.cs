using MonolithBoilerPlate.Common;
using MonolithBoilerPlate.Service.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;


namespace MonolithBoilerPlate.Service.Implementation
{
    public class ServerDirectoryService : IServerDirectoryService
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly AppSettings _appSettings;
        public ServerDirectoryService(
            IWebHostEnvironment hostingEnvironment,
            IConfiguration configuration,
            IOptions<AppSettings> appSettings)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _appSettings = appSettings.Value;
        }

        public bool Exists(string path)
        {
            return System.IO.Directory.Exists(path);
        }

        public bool FileExists(string path)
        {
            return System.IO.File.Exists(path);
        }

        public string GetApplicationPath()
        {
            return this._hostingEnvironment.ContentRootPath;
        }
        public string GetWWWRootPath()
        {
            return _hostingEnvironment.WebRootPath;
        }

        public void CreateDirectory(string path)
        {
            var newPath = Path.Combine(this.GetWWWRootPath(), this._appSettings.DirectoryPath.Root, path);

            if (!System.IO.Directory.Exists(newPath))
            {
                System.IO.Directory.CreateDirectory(newPath);
            }
        }


        public string GetDirectory(string path)
        {
            var newPath = Path.Combine(this.GetWWWRootPath(), this._appSettings.DirectoryPath.Root, path);

            return newPath;
        }

        public void SaveFile(string path, byte[] buffer)
        {
            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                fs.Write(buffer, 0, buffer.Length);
            }
        }

        public void SaveFile(string path, IFormFile formFile)
        {
            var regex = new Regex(@"\.\.|\\|/|/");
            if (regex.IsMatch(formFile.FileName))
                throw new BadHttpRequestException("Invalid File Name");

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                formFile.CopyTo(fs);
            }
        }

        public async Task SaveFileAsync(string path, IFormFile formFile)
        {
            using var fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            await formFile.CopyToAsync(fs);
        }

        public void RemoveFile(string path)
        {
            if (!this.FileExists(path))
                throw new FileNotFoundException();

            System.IO.File.Delete(path);
        }

        public void CreateRootContentDirectory()
        {
            var newPath = Path.Combine(this.GetWWWRootPath(), this._appSettings.DirectoryPath.Root);

            if (!System.IO.Directory.Exists(newPath))
            {
                System.IO.Directory.CreateDirectory(newPath);
            }
        }

        public void MoveFileUsingRelativePath(string fromDirectoryPath, string toDirectoryPath)
        {
            fromDirectoryPath = Path.Combine(GetApplicationPath(), fromDirectoryPath);
            toDirectoryPath = Path.Combine(GetApplicationPath(), toDirectoryPath);

            var files = System.IO.Directory.GetFiles(fromDirectoryPath);

            if (files.Length > 0)
            {
                if (!Directory.Exists(toDirectoryPath))
                {
                    Directory.CreateDirectory(toDirectoryPath);
                }

                foreach (var file in files)
                {
                    var backupFileName = Path.GetFileName(file);
                    var destSource = Path.Combine(toDirectoryPath, backupFileName);
                    File.Move(file, destSource);
                }
            }
            if (Directory.Exists(fromDirectoryPath))
            {
                Directory.Delete(fromDirectoryPath, true);
            }
        }


        public void CopyFileUsingRelativePath(string fromDirectoryPath, string toDirectoryPath)
        {
            fromDirectoryPath = Path.Combine(GetApplicationPath(), fromDirectoryPath);
            toDirectoryPath = Path.Combine(GetApplicationPath(), toDirectoryPath);

            var files = System.IO.Directory.GetFiles(fromDirectoryPath);

            if (files.Length > 0)
            {
                if (!Directory.Exists(toDirectoryPath))
                {
                    Directory.CreateDirectory(toDirectoryPath);
                }

                foreach (var file in files)
                {
                    var backupFileName = Path.GetFileName(file);
                    var destSource = Path.Combine(toDirectoryPath, backupFileName);
                    File.Copy(file, destSource);
                }
            }
        }

        public void MoveFileFromPhysicalPathToServer(string fromDirectoryPath, string toDirectoryPath)
        {
            fromDirectoryPath = Path.Combine(GetApplicationPath(), fromDirectoryPath);

            var files = System.IO.Directory.GetFiles(fromDirectoryPath);

            if (files.Length > 0)
            {
                if (!Directory.Exists(toDirectoryPath))
                {
                    Directory.CreateDirectory(toDirectoryPath);
                }

                foreach (var file in files)
                {
                    var backupFileName = Path.GetFileName(file);
                    var destSource = Path.Combine(toDirectoryPath, backupFileName);
                    File.Move(file, destSource);
                }

            }
            if (Directory.Exists(fromDirectoryPath))
            {
                Directory.Delete(fromDirectoryPath, true);
            }

        }

        public List<string> GetAllFileName(string directory)
        {

            var fileNames = Directory.GetFiles(directory)
                                     .Select(Path.GetFileName)
                                     .ToList();
            return fileNames;
        }

        public List<string> GetAllFileName(string directory, string url)
        {

            var fileNames = GetAllFileName(directory)
                            .Select(c => url + "\\" + c)
                                     .ToList();
            return fileNames;
        }

        public List<string> GetAllFiles(string directory)
        {
            var files = Directory.GetFiles(directory)
                                    .ToList();
            return files;
        }
    }
}
