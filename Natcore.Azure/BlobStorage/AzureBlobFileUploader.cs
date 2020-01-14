using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
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
			if (!CloudStorageAccount.TryParse(_connectionString, out CloudStorageAccount storage))
				throw new InvalidOperationException("Unable to create connection to cloud storage");

			string[] basePathParts = (basePath ?? "").Split('/');

			CloudBlobClient client = storage.CreateCloudBlobClient();
			CloudBlobContainer container = client.GetContainerReference(!string.IsNullOrEmpty(basePathParts[0]) ? basePathParts[0] : "container");
			await container.CreateIfNotExistsAsync();

			await container.SetPermissionsAsync(new BlobContainerPermissions
			{
				PublicAccess = BlobContainerPublicAccessType.Blob
			});

			basePath = string.Join("/", basePathParts.Skip(1));

			if (!string.IsNullOrEmpty(basePath))
				basePath += "/";

			CloudBlockBlob blob = container.GetBlockBlobReference($"{basePath}{file.Name}");

			using (var stream = new MemoryStream(file.Content))
				await blob.UploadFromStreamAsync(stream);

			return blob.Uri.AbsoluteUri;
		}
	}
}
