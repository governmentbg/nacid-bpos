using System;
using System.Collections.Generic;

namespace NacidRas.Integrations.OaiPmhProvider.Models.DublinCore
{
	/// <summary>
	/// Dublin Core Metadata Element Set, Version 1.1
	/// http://dublincore.org/documents/dces/
	/// </summary>
	public class DublinCoreMetadata : RecordMetadata
	{
		public static string MetadataFormatPrefix = "oai_dc";

		public DublinCoreMetadata()
		{
			this.MetadataFormat = MetadataFormatPrefix;
		}

		public IList<DublinCoreElement> Title { get; set; } = new List<DublinCoreElement>();
		public IList<DublinCoreElement> Creator { get; set; } = new List<DublinCoreElement>();
		public IList<DublinCoreElement> Subject { get; set; } = new List<DublinCoreElement>();
		public IList<DublinCoreElement> Description { get; set; } = new List<DublinCoreElement>();
		public IList<DublinCoreElement> Publisher { get; set; } = new List<DublinCoreElement>();
		public IList<DublinCoreElement> Contributor { get; set; } = new List<DublinCoreElement>();
		public IList<DateTime?> Date { get; set; } = new List<DateTime?>();
		public IList<DublinCoreElement> Type { get; set; } = new List<DublinCoreElement>();
		public IList<DublinCoreElement> Format { get; set; } = new List<DublinCoreElement>();
		public IList<DublinCoreElement> Identifier { get; set; } = new List<DublinCoreElement>();
		public IList<DublinCoreElement> Source { get; set; } = new List<DublinCoreElement>();
		public IList<DublinCoreElement> Language { get; set; } = new List<DublinCoreElement>();
		public IList<DublinCoreElement> Relation { get; set; } = new List<DublinCoreElement>();
		public IList<DublinCoreElement> Coverage { get; set; } = new List<DublinCoreElement>();
		public IList<DublinCoreElement> Rights { get; set; } = new List<DublinCoreElement>();
	}
}
