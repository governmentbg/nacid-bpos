using Newtonsoft.Json;
using OpenScience.Data.Base.Models;
using OpenScience.Data.Institutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenScience.Data.Users.Models
{
	public class User : Entity
	{
		public string Username { get; set; }

		[JsonIgnore]
		public string PasswordHash { get; set; }

		[JsonIgnore]
		public string PasswordSalt { get; set; }

		public string Fullname { get; set; }

		public string Email { get; set; }

		public int RoleId { get; set; }
		public Role Role { get; set; }

		public string Orcid { get; set; }

		public bool IsActive { get; set; }
		public bool IsLocked { get; set; }

		public DateTime? CreateDate { get; set; }

		public DateTime? UpdateDate { get; set; }

		public ICollection<UserInstitution> Institutions { get; set; } = new List<UserInstitution>();

		private User()
		{

		}

		public User(string username, string fullname, string email, int roleId, string orcid, List<int> institutionIds)
		{
			this.Username = username;
			this.Fullname = fullname;
			this.Email = email;
			this.RoleId = roleId;
			this.Orcid = orcid;

			if (institutionIds != null)
			{
				foreach (var institutionId in institutionIds)
				{
					this.Institutions.Add(new UserInstitution { InstitutionId = institutionId });
				}
			}

			this.IsActive = true;
			this.IsLocked = true;
			this.CreateDate = DateTime.UtcNow;
		}

		public void Activate(string passwordHash, string passwordSalt)
		{
			// TODO: Add locked check
			this.PasswordHash = passwordHash;
			this.PasswordSalt = passwordSalt;
			this.IsLocked = false;
			this.UpdateDate = DateTime.UtcNow;
		}

		public void Deactivate()
		{
			this.IsActive = false;
			this.UpdateDate = DateTime.UtcNow;
		}

		public void ChangePassword(string passwordHash, string passwordSalt)
		{
			this.PasswordHash = passwordHash;
			this.PasswordSalt = passwordSalt;
			this.UpdateDate = DateTime.UtcNow;
		}
	}
}
