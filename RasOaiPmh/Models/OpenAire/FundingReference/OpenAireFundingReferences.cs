namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{
	public class OpenAireFundingReferences : BaseListElement<OpenAireFundingReferencesFundingReference>
	{
		public OpenAireFundingReferences() : base(OaiNamespaces.DataCiteNamespace + "fundingReferences") { }
	}
}
