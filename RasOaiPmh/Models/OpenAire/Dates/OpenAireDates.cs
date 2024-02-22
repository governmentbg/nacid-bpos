namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireDates : BaseListElement<OpenAireDatesDate>
	{
		public OpenAireDates() : base(OaiNamespaces.DataCiteNamespace + "dates") { }
	}
}
