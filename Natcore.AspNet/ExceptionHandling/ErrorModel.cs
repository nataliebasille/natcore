namespace Natcore.AspNet.ExceptionHandling
{
	public class ErrorModel
	{
		public string Type { get; set; }

		public int Status { get; set; }

		public string Title { get; set; }

		public string Detail { get; set; }

		public string Instance { get; set; }

		public string ReferenceID { get; set; }
	}
}
