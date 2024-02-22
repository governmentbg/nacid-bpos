using OpenScience.Data.Classifications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ServerApplication.ClassificationsModule.Dtos
{
	public class ClassificationDto
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public int? OrganizationId { get; set; }
		public string OrganizationName { get; set; }

		public List<ClassificationDto> Children { get; set; } = new List<ClassificationDto>();

		public static Expression<Func<Classification, ClassificationDto>> SelectExpression
		{
			get
			{
				return e => new ClassificationDto {
					Id = e.Id,
					Name = e.Name,
					OrganizationId = e.OrganizationId,
					OrganizationName = e.Organization != null ? e.Organization.Name : null,
					Children = e.Children.Select(c => new ClassificationDto {
						Id = c.Id,
						Name = c.Name,
						OrganizationId = c.OrganizationId,
						OrganizationName = c.Organization != null ? c.Organization.Name : null
					})
					.OrderBy(c => c.Id)
					.ToList()
				};
			}
		}
	}
}
