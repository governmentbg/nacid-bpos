namespace OpenScience.Services.Classifications.Dtos
{
	public class FlatClassificationHierarchyItemDto
	{
		public int Id { get; set; }
		public int? ParentId { get; set; }

		public string Name { get; set; }

		public int Level { get; set; } = 0;

		public decimal ViewOrder { get; set; }

		public bool HasChildren { get; set; }
	}
}
