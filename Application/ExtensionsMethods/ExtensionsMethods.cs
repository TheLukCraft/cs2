using Microsoft.AspNetCore.Http;

namespace Application.ExtensionsMethods
{
    public static class ExtensionsMethods
    {
        public static byte[] GetBytes(this IFormFile formFile)
        {
            using (var memoryStream = new MemoryStream())
            {
                formFile.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }

        public static string SaveFile(this IFormFile formFile)
        {
            string rootPath = @"C\CS2_Attachments";
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            string filePath = Path.Combine(rootPath, $"{Guid.NewGuid()}_{formFile.FileName}");

            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                formFile.CopyTo(fileStream);

            return filePath;
        }
    }
}