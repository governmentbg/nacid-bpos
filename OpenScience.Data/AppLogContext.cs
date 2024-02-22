using Microsoft.EntityFrameworkCore;
using OpenScience.Data.Logs.Models;

namespace OpenScience.Data
{
	public class AppLogContext : DbContext
	{
		public DbSet<Log> Logs { get; set; }

		public AppLogContext(DbContextOptions<AppLogContext> options)
			: base(options) { }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			foreach (var entity in modelBuilder.Model.GetEntityTypes())
			{
				entity.Relational().TableName = entity.ClrType.Name.ToLower();

				foreach (var property in entity.GetProperties())
				{
					property.Relational().ColumnName = property.Name.ToLower();
				}
			}

			base.OnModelCreating(modelBuilder);
		}
	}
}
