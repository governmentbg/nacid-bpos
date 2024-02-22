namespace NacidRas.Integrations.OaiPmhProvider.Models.OpenAire
{

	public class OpenAireSubjects : BaseListElement<OpenAireSubjectsSubject>
	{
		public OpenAireSubjects() : base(OaiNamespaces.DataCiteNamespace + "subjects") { }
	}
}
