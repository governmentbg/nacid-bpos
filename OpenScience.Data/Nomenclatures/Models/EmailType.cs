using OpenScience.Data.Base.Models;

namespace OpenScience.Data.Nomenclatures.Models
{
	public class EmailType : AliasNomenclature
	{
		public string Subject { get; set; }

		public string Body { get; set; }
	}

	public static class EmailTypeAlias
	{
		public const string UserActivation = "UserActivation";
		public const string ForgottenPassword = "ForgottenPassword";
		public const string Other = "Other";
	}
}
