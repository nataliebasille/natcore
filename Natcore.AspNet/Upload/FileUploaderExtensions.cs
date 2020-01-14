using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Natcore.Core.Upload
{
    public static class FileUploaderExtensions
    {
		public static Task<string> UploadAsync(this IFileUploader uploader, IFormFile file)
			=> UploadAsync(uploader, null, file);

		public static async Task<string> UploadAsync(this IFileUploader uploader, string basePath, IFormFile file)
		{
			using (var memoryStream = new MemoryStream())
			using (var fileStream = file.OpenReadStream())
			{
				await fileStream.CopyToAsync(memoryStream);
				return await uploader.UploadAsync(basePath, new File
				{
					Name = file.FileName,
					Content = memoryStream.ToArray(),
					ContentType = file.ContentType
				});
			}
		}
	}
}
