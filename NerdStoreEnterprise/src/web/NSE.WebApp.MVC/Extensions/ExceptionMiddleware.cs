using Microsoft.AspNetCore.Http;
using Polly.CircuitBreaker;
using Refit;
using System.Net;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Extensions
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		public ExceptionMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await _next(httpContext);

			}
			catch(CustomHttpResponseException ex)
			{
				HandleRequestExceptionsAsync(httpContext, ex.StatusCode);

			}
			catch(ValidationApiException ex)
			{
				HandleRequestExceptionsAsync(httpContext, ex.StatusCode);
			}
			catch(ApiException ex)
			{
				HandleRequestExceptionsAsync(httpContext, ex.StatusCode);
			}
			catch(BrokenCircuitException)
			{
				HandleCircuitBrakerExceptionAsync(httpContext);
			}
		}

		private static void HandleRequestExceptionsAsync(HttpContext context, HttpStatusCode statusCode)
		{
			if(statusCode == HttpStatusCode.Unauthorized)
			{
				context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
				return;
			}

			context.Response.StatusCode = (int)statusCode;
		}

		private static void HandleCircuitBrakerExceptionAsync(HttpContext context)
		{
			context.Response.Redirect("/sistema-indisponivel");
		}

	}
}
