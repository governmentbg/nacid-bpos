using OpenScience.Data.Publications.Enums;
using OpenScience.Data.Publications.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ServerApplication.PublicationsModule.Dtos
{
	public class PublicationSearchResultDto
	{
		public int Id { get; set; }

		public IEnumerable<string> Titles { get; set; }

		public IEnumerable<string> Authors { get; set; }

		public DateTime? ModificationDate { get; set; }

		public PublicationStatus PublicationStatus { get; set; }

		public static Expression<Func<Publication, PublicationSearchResultDto>> SelectExpression
		{
			get
			{
				return e => new PublicationSearchResultDto {
					Id = e.Id,
					Titles = e.Titles.OrderBy(t => t.ViewOrder).Select(t => t.Value),
					Authors = e.Creators.OrderBy(c => c.ViewOrder).Select(c => c.DisplayName),
					ModificationDate = e.ModificationDate,
					PublicationStatus = e.Status
				};
			}
		}
	}
}
