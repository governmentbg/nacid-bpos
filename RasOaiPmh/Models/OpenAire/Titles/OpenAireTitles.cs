namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireTitles : BaseListElement<OpenAireTitlesTitle>
	{
		public OpenAireTitles() : base(OaiNamespaces.DataCiteNamespace + "titles") { }
	}
}
