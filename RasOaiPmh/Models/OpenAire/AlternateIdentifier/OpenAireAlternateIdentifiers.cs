namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireAlternateIdentifiers : BaseListElement<OpenAireAlternateIdentifiersAlternateIdentifier>
	{
		public OpenAireAlternateIdentifiers() : base(OaiNamespaces.DataCiteNamespace + "alternateIdentifiers")
		{
		}
	}
}
