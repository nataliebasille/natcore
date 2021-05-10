using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Natcore.Core.Upload
{
    public class AzureBlobFileUploader : IFileUploader
    {
		private readonly string _connectionString;
		public AzureBlobFileUploader(string connectionString)
		{
			_connectionString = connectionString;
		}

		public Task Delete(string basePath, string fileName)
		{
			throw new NotImplementedException();
		}

		public async Task<string> UploadAsync(string basePath, File file)
		{
			var client = new BlobServiceClient(_connectionString);
			string[] basePathParts = (basePath ?? "").Split('/');

			var container = client.GetBlobContainerClient(!string.IsNullOrEmpty(basePathParts[0]) ? basePathParts[0] : "container");
			await container.CreateIfNotExistsAsync(
				publicAccessType: PublicAccessType.Blob
			);

			basePath = string.Join("/", basePathParts.Skip(1));

			if (!string.IsNullOrEmpty(basePath))
				basePath += "/";

			BlobClient blob = container.GetBlobClient($"{basePath}{file.Name}");

			using (var stream = new MemoryStream(file.Content))
				await blob.UploadAsync(stream);

			return blob.Uri.AbsoluteUri;
		}
	}
}
