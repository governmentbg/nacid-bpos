using OpenScience.Data.Base.Interfaces;

namespace ServerApplication.Infrastructure.Auth
{
	public class AuthConfiguration : IJwtConfiguration
	{
		public string SecretKey { get; set; }

		public string Issuer { get; set; }

		public string Audience { get; set; }

		public int ValidHours { get; set; }
	}
}
