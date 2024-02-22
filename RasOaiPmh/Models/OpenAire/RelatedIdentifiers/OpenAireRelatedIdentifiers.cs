namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireRelatedIdentifiers : BaseListElement<OpenAireRelatedIdentifiersRelatedIdentifier>
	{
		public OpenAireRelatedIdentifiers() : base(OaiNamespaces.DataCiteNamespace + "relatedIdentifiers") { }
	}
}
