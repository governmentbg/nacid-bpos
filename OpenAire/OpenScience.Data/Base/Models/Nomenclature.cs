namespace OpenScience.Data.Base.Models
{
	public class Nomenclature : Entity
	{
		public string Name { get; set; }

		public int? ViewOrder { get; set; }

		public bool IsActive { get; set; }
	}
}
