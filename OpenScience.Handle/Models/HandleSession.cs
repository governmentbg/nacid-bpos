namespace OpenScience.Handle.Models
{
	public class HandleSession
	{
		public string Id { get; set; }
		public string SessionId { get; set; }
		public string Nonce { get; set; }
		public bool Authenticated { get; set; }
	}
}
