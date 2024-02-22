namespace OpenScience.Common.Configuration
{
	public interface IEmailConfiguration : ISmtpConfiguration
	{
		string FromAddress { get; set; }
		string FromName { get; set; }
		bool JobEnabled { get; set; }
	}
}
