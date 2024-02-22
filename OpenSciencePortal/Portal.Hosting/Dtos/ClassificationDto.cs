using System.Collections.Generic;

namespace Portal.Hosting.Dtos
{
	public class ClassificationDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Count { get; set; }
		public IEnumerable<ClassificationDto> Children { get; set; }
	}
}
