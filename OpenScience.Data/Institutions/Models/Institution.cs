using OpenScience.Data.Base.Models;

namespace OpenScience.Data.Institutions.Models
{
	public class Institution : Entity
	{
		public string Name { get; private set; }

		public string NameEn { get; private set; }

		public string Identifier { get; private set; }

		public string RepositoryUrl { get; private set; }

		public bool AreCommonClassificationsVisible { get; private set; } = true;

		public bool IsActive { get; private set; } = true;

		private Institution()
		{

		}

		public Institution(
			string name, 
			string nameEn,
			string identifier, 
			string repositoryUrl,
			bool areCommonClassificationsVisible,
			bool isActive)
		{
			this.Name = name;
			this.NameEn = nameEn;
			this.Identifier = identifier;
			this.RepositoryUrl = repositoryUrl;
			this.AreCommonClassificationsVisible = areCommonClassificationsVisible;
			this.IsActive = isActive;
		}

		public void Deactivate()
		{
			this.IsActive = false;
		}
	}
}
