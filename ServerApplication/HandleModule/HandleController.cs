using Microsoft.AspNetCore.Mvc;
using OpenScience.Handle;
using OpenScience.Handle.Models;
using System.Threading.Tasks;

namespace ServerApplication.HandleModule
{
	[ApiController]
	[Route("api/[controller]")]
	public class HandleController : ControllerBase
	{
		private readonly IHandleServerAdapter handleServerAdapter;

		public HandleController(
			IHandleServerAdapter handleServerAdapter
		)
		{
			this.handleServerAdapter = handleServerAdapter;
		}

		[HttpGet("getAll")]
		public Task<HandleListResponse> GetAll()
		{
			return handleServerAdapter.ListHandlesAsync();
		}

		[HttpGet("{suffix:required}")]
		public Task<HandleValuesResponse> Get([FromRoute] string suffix)
		{
			return handleServerAdapter.GetHandleAsync(suffix);
		}

		public class CreateUrlHandleDto
		{
			public string Url { get; set; }
		}

		[HttpPut("")]
		public Task<HandleOperationResponse> Create([FromBody] CreateUrlHandleDto data)
		{
			return handleServerAdapter.CreateUrlHandleAsync(data.Url);
		}

		[HttpDelete("{suffix:required}")]
		public Task<HandleOperationResponse> Delete([FromRoute] string suffix)
		{
			return handleServerAdapter.DeleteHandleAsync(suffix);
		}
	}
}
