namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireCreators : BaseListElement<OpenAireCreatorsCreator>
	{
		public OpenAireCreators() : base(OaiNamespaces.DataCiteNamespace + "creators") { }
	}
}
