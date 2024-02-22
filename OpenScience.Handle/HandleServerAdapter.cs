using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenScience.Handle.Configuration;
using OpenScience.Handle.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace OpenScience.Handle
{
	public class HandleServerAdapter : IHandleServerAdapter
	{
		private readonly IHttpClientFactory clientFactory;
		private readonly HandleServerConfig configuration;

		private HttpClient _httpClient;
		public HttpClient HttpClient
		{
			get
			{
				if (_httpClient == null)
				{
					this._httpClient = clientFactory.CreateClient("UntrustedClient");
					this._httpClient.DefaultRequestHeaders.Add("Authorization", $"Handle sessionId=\"{Session.SessionId}\"");
				}

				return _httpClient;
			}
		}

		private HandleSession _session;
		public HandleSession Session
		{
			get
			{
				if (_session == null)
				{
					HttpClient http = clientFactory.CreateClient("UntrustedClient");
					_session = JsonConvert.DeserializeObject<HandleSession>(http
							.PostAsync($"{configuration.HttpServerAddress}/api/sessions", null)
							.Result
							.Content
							.ReadAsStringAsync()
							.Result);

					var authRequest = new HttpRequestMessage {
						Method = HttpMethod.Put,
						RequestUri = new Uri($"{configuration.HttpServerAddress}/api/sessions/this")
					};

					//Create the Authorization header using the admin certificate
					authRequest.Headers.Add("Authorization", CreateInitialAuthHeader(_session));

					//Validate the signature and finish creation of session
					_session = JsonConvert.DeserializeObject<HandleSession>(http
							.SendAsync(authRequest)
							.Result
							.Content
							.ReadAsStringAsync()
							.Result);

					//Check if session is successfuly created
					if (!_session.Authenticated)
						throw new Exception("HandleServer: unable to create authenticated session");
				}

				return _session;
			}
		}

		public HandleServerAdapter(
			IHttpClientFactory clientFactory,
			IOptions<HandleServerConfig> handleServerOptions
			)
		{
			this.clientFactory = clientFactory;
			this.configuration = handleServerOptions.Value;
		}

		public Task<HandleValuesResponse> GetHandleAsync(string handle)
		{
			var url = $"{configuration.HttpServerAddress}/api/handles/{configuration.Prefix}/{handle}";
			return DeserializeAsync<HandleValuesResponse>(this.HttpClient.GetAsync(url));
		}

		public Task<HandleOperationResponse> CreateHandleAsync(HandleIdentifier handle)
		{
			var payload = Serialize(new {
				values = new List<HandleIdentifier> {
					CreateDefaultHandleAdminValue(),
					handle
				}
			});

			var url = $"{configuration.HttpServerAddress}/api/handles/{configuration.Prefix}/test-?mintNewSuffix=true";
			return DeserializeAsync<HandleOperationResponse>(this.HttpClient.PutAsync(url, payload));
		}

		public Task<HandleOperationResponse> CreateUrlHandleAsync(string url)
		{
			var handle = new HandleIdentifier {
				Index = 1,
				Type = "URL",
				Data = new {
					format = "string",
					value = url
				}
			};

			return CreateHandleAsync(handle);
		}

		public Task<HandleOperationResponse> ModifyHandleAsync()
		{
			throw new NotImplementedException();
		}

		public Task<HandleOperationResponse> DeleteHandleAsync(string handle)
		{
			var url = $"{configuration.HttpServerAddress}/api/handles/{configuration.Prefix}/{handle}";
			return DeserializeAsync<HandleOperationResponse>(this.HttpClient.DeleteAsync(url));
		}

		public Task<HandleListResponse> ListHandlesAsync()
		{
			var url = $"{configuration.HttpServerAddress}/api/handles?prefix={configuration.Prefix}";
			return DeserializeAsync<HandleListResponse>(this.HttpClient.GetAsync(url));
		}

		private async Task<T> DeserializeAsync<T>(Task<HttpResponseMessage> asyncCall)
		{
			var response = await asyncCall;

			response.EnsureSuccessStatusCode();

			var content = await response.Content.ReadAsStringAsync();

			//TODO: add checks for response code and potentially throw exception in some cases
			return JsonConvert.DeserializeObject<T>(content);
		}

		private StringContent Serialize<T>(T value) => new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/json");

		private string CreateInitialAuthHeader(HandleSession session)
		{
			//We are signing the raw bytes not the base64 encoding!!!
			byte[] nonce = Convert.FromBase64String(session.Nonce);
			byte[] cnonce = new byte[16];
			byte[] bytes = new byte[nonce.Length + cnonce.Length];
			using (var crypto = new RNGCryptoServiceProvider())
				crypto.GetBytes(cnonce);

			Buffer.BlockCopy(nonce, 0, bytes, 0, nonce.Length);
			Buffer.BlockCopy(cnonce, 0, bytes, nonce.Length, cnonce.Length);

			X509Certificate2 cert = new X509Certificate2(configuration.AdminCertPath, configuration.AdminCertPass);
			byte[] signature = cert.GetRSAPrivateKey().SignData(bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

			return $"Handle sessionId=\"{session.SessionId}\", id=\"{configuration.AdminHandleIndex}:{configuration.AdminHandle}\", type=\"HS_PUBKEY\", cnonce=\"{Convert.ToBase64String(cnonce)}\", alg=\"SHA256\", signature=\"{Convert.ToBase64String(signature)}\"";
		}

		private HandleIdentifier CreateDefaultHandleAdminValue()
		{
			return new HandleIdentifier {
				Index = 100,
				Type = "HS_ADMIN",
				Data = new {
					format = "admin",
					value = new {
						index = 200,
						handle = configuration.AdminHandle,
						permissions = "110001110011"
					}
				}
			};
		}
	}
}
