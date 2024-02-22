using System;
using System.Collections.Generic;
using NacidRas.Integrations.OaiPmhProvider.Models;

namespace NacidRas.Integrations.OaiPmhProvider
{
	public class OaiConfiguration
	{
		private static volatile OaiConfiguration instance;
		private static readonly object syncRoot = new object();

		private OaiConfiguration()
		{
			Identify = new Identify();
		}

		/// <summary>
		/// The singleton instance.
		/// </summary>
		public static OaiConfiguration Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new OaiConfiguration();
					}
				}

				return instance;
			}
		}

		public string FileStorageUrlTemplate => "https://openras.nacid.bg/api/FilesStorage?key={0}&dbId={1}";

		public Identify Identify { get; set; }

		#region Data Provider configuration

		public int PageSize { get; set; } = 100;

		public bool SupportSets { get; set; } = true;

		public TimeSpan ExpirationTimeSpan { get; set; } = TimeSpan.FromDays(1);

		#endregion

		#region Custom configuration

		public ISet<string> ResumptionTokenCustomParameterNames { get; set; } = new HashSet<string>();

		#endregion
	}
}
