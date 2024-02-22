using Polly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OpenScience.Common.Services
{
	public class RetryExecutionService
	{
		public async Task ExecuteAsync<TException>(int retryCount, Func<CancellationToken, Task> executeFunc)
			where TException : Exception
		{
			await Policy
				.Handle<TException>()
				.WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
				.ExecuteAsync(executeFunc, CancellationToken.None);
		}

		public async Task<(bool success, TResult result, Exception exception)> ExecuteAndCaptureAsync<TException, TResult>(int retryCount, Func<CancellationToken, Task<TResult>> executeFunc)
			where TException : Exception
		{
			var result = await Policy
				.Handle<TException>()
				.WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
				.ExecuteAndCaptureAsync(executeFunc, CancellationToken.None);

			return (result.Outcome == OutcomeType.Successful, result.Result, result.FinalException);
		}
	}
}
