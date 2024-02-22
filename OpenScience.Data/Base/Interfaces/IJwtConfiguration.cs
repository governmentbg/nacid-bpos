namespace OpenScience.Data.Base.Interfaces
{
	public interface IJwtConfiguration
	{
		string SecretKey { get; set; }
		string Issuer { get; set; }
		string Audience { get; set; }
		int ValidHours { get; set; }
	}
}
