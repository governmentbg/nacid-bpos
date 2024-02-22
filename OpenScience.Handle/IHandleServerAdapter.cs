using OpenScience.Handle.Models;
using System.Threading.Tasks;

namespace OpenScience.Handle
{
	public interface IHandleServerAdapter
	{
		Task<HandleValuesResponse> GetHandleAsync(string handle);
		Task<HandleOperationResponse> CreateHandleAsync(HandleIdentifier handle);
		Task<HandleOperationResponse> CreateUrlHandleAsync(string url);
		Task<HandleOperationResponse> ModifyHandleAsync();
		Task<HandleOperationResponse> DeleteHandleAsync(string handle);
		Task<HandleListResponse> ListHandlesAsync();
		HandleSession Session { get; }
	}
}
