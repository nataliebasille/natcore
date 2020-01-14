using System.Threading.Tasks;

namespace Natcore.Core.Upload
{
	public interface IFileUploader
	{
		/// <summary>
		/// Perform upload to backend and return the url for the file
		/// </summary>
		/// <returns></returns>
		Task<string> UploadAsync(string basePath, File file);

		/// <summary>
		/// Remove uploaded file from backend
		/// </summary>
		/// <param name="basePath"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		Task Delete(string basePath, string fileName);
	}
}
