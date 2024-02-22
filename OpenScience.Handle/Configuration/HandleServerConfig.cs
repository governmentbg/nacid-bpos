using System;

namespace OpenScience.Handle.Configuration
{
	public class HandleServerConfig
	{
		private string _httpServerAddress;
		public string HttpServerAddress => _httpServerAddress;

		private string _prefix;
		public string Prefix => _prefix;

		private int _adminHandleIndex;
		public int AdminHandleIndex => _adminHandleIndex;

		private string _adminHandle;
		public string AdminHandle => _adminHandle;

		private string _adminCertPath;
		public string AdminCertPath => _adminCertPath;

		private string _adminCertPass;
		public string AdminCertPass => _adminCertPass;

		public HandleServerConfig WithAddress(string httpServerAddress)
		{
			if (string.IsNullOrWhiteSpace(httpServerAddress))
			{
				throw new ArgumentNullException("Address must have a value");
			}

			this._httpServerAddress = httpServerAddress;

			return this;
		}

		public HandleServerConfig WithPrefix(string prefix)
		{
			if (string.IsNullOrWhiteSpace(prefix))
			{
				throw new ArgumentNullException("Prefix must have a value");
			}

			this._prefix = prefix;

			return this;
		}

		public HandleServerConfig WithCredentials(int adminHandleIndex, string adminHandle, string adminCertPath, string adminCertPass)
		{
			this._adminHandleIndex = adminHandleIndex;
			this._adminHandle = adminHandle;
			this._adminCertPath = adminCertPath;
			this._adminCertPass = adminCertPass;

			return this;
		}

		public bool IsInitialized()
		{
			return !string.IsNullOrEmpty(HttpServerAddress)
				&& !string.IsNullOrEmpty(Prefix)
				&& !string.IsNullOrEmpty(AdminHandle)
				&& !string.IsNullOrEmpty(AdminCertPath);
		}
	}
}
