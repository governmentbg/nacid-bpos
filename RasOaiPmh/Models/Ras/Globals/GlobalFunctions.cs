using NacidRas.Infrastructure.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NacidRas.Ras.Globals
{
	public static class GlobalFunctions
	{
		private static Expression<Func<Institution, bool>> GetUniversityTypesExpr()
		{
			var predicate = PredicateBuilder.True<Institution>();

			predicate = predicate.And(s => s.InstitutionType == Ras.InstitutionType.University ||
										   s.InstitutionType == Ras.InstitutionType.Agency ||
										   s.InstitutionType == Ras.InstitutionType.Faculty);

			return predicate;
		}

		private static Expression<Func<Institution, bool>> GetScientificOrganizationTypesExpr()
		{
			var predicate = PredicateBuilder.True<Institution>();

			predicate = predicate.And(s => s.InstitutionType == Ras.InstitutionType.ScientificOrganization ||
										   s.InstitutionType == Ras.InstitutionType.Station ||
										   s.InstitutionType == Ras.InstitutionType.AccreditedHospital ||
										   s.InstitutionType == Ras.InstitutionType.BotanicalGarden ||
										   s.InstitutionType == Ras.InstitutionType.Center ||
										   s.InstitutionType == Ras.InstitutionType.Institution);

			return predicate;
		}

		public static Expression<Func<Institution, bool>> GetInstitutionSubtypesByMainTypeExpr(InstitutionType? institutionType = null)
		{
			var predicate = PredicateBuilder.True<Institution>();

			if (institutionType == Ras.InstitutionType.University)
			{
				predicate = GetUniversityTypesExpr();
			}
			else if (institutionType == Ras.InstitutionType.ScientificOrganization)
			{
				predicate = GetScientificOrganizationTypesExpr();
			}

			return predicate;
		}


	}
}
