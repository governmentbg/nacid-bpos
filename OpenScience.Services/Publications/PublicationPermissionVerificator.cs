using OpenScience.Common.Constants;
using OpenScience.Data.Publications.Models;
using System.Collections.Generic;
using System.Linq;

namespace OpenScience.Services.Publications
{
	public class PublicationPermissionVerificator
	{
		public bool CanCreatePublication(string userRoleAlias, List<int> userInstitutionIds, Publication publication)
		{
			if (userRoleAlias == UserRoleAliases.ADMINISTRATOR)
			{
				return true;
			}
			else
			{
				return userInstitutionIds.Contains(publication.ModeratorInstitutionId.Value);
			}
		}

		public bool CanPreviewPublication(int userId, string userRoleAlias, List<int> userInstitutionIds, Publication publication)
		{
			if (userRoleAlias == UserRoleAliases.SCIENTIST)
			{
				return publication.CreatedByUserId == userId;
			}
			else if (userRoleAlias == UserRoleAliases.MODERATOR || userRoleAlias == UserRoleAliases.ORGANIZATION_ADMINISTRATOR)
			{
				return userInstitutionIds.Contains(publication.ModeratorInstitutionId.Value);
			}

			return true;
		}

		public bool CanEditPublication(int userId, string userRoleAlias, List<int> userInstitutionIds, Publication publication)
		{
			if (userRoleAlias == UserRoleAliases.SCIENTIST)
			{
				return publication.CreatedByUserId == userId;
			}
			else if (userRoleAlias == UserRoleAliases.MODERATOR || userRoleAlias == UserRoleAliases.ORGANIZATION_ADMINISTRATOR)
			{
				return userInstitutionIds.Contains(publication.ModeratorInstitutionId.Value);
			}

			return true;
		}

		public bool CanSubmitPublication(string userRoleAlias, List<int> userInstitutionIds, Publication publication)
		{
			if(userRoleAlias == UserRoleAliases.ADMINISTRATOR)
			{
				return true;
			}

			if (userRoleAlias == UserRoleAliases.MODERATOR || userRoleAlias == UserRoleAliases.ORGANIZATION_ADMINISTRATOR)
			{
				return userInstitutionIds.Contains(publication.ModeratorInstitutionId.Value);
			}

			return false;
		}
	}
}
