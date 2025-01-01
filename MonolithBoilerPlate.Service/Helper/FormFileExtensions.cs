using MonolithBoilerPlate.Entity.ViewModels;
using Microsoft.AspNetCore.Http;

namespace MonolithBoilerPlate.Service.Helper
{
    public static class FormFileExtensions
    {
        public static IFormFile ToFormFile(this FileVm file)
        {
            var stream = new MemoryStream(file.Bytes);
            return new FormFile(stream, 0, stream.Length, "file", file.Name)
            {
                Headers = new HeaderDictionary(),
                ContentType = file.ContentType
            };
        }
    }
}
