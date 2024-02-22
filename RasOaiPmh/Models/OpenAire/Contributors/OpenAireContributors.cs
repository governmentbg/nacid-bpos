namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireContributors : BaseListElement<OpenAireContributorsContributor>
	{
		public OpenAireContributors() : base(OaiNamespaces.DataCiteNamespace + "contributors") { }
	}
}
