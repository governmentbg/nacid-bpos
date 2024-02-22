using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpenScience.Data.Classifications.Models
{
	public class ClassificationClosure
	{
		public int ChildId { get; set; }
		public Classification Child { get; set; }

		public int ParentId { get; set; }
		public Classification Parent { get; set; }
	}

	public class ClassificationItemClosureConfiguration : IEntityTypeConfiguration<ClassificationClosure>
	{
		public void Configure(EntityTypeBuilder<ClassificationClosure> builder)
		{
			builder.HasKey(e => new { e.ChildId, e.ParentId });

			builder.HasOne(e => e.Child)
				.WithMany()
				.HasForeignKey(e => e.ChildId);

			builder.HasOne(e => e.Parent)
				.WithMany()
				.HasForeignKey(e => e.ParentId);
		}
	}
}
