﻿using Microsoft.AspNetCore.Http;
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

			}catch(CustomHttpResponseException ex)
			{
				HandleRequestExceptionsAsync(httpContext, ex);
			}
		}

		private static void HandleRequestExceptionsAsync(HttpContext context, CustomHttpResponseException httpRequestException)
		{
			if(httpRequestException.StatusCode == HttpStatusCode.Unauthorized)
			{
				context.Response.Redirect($"/login?ReturnUrl={context.Request.Path}");
				return;
			}

			context.Response.StatusCode = (int)httpRequestException.StatusCode;
		}

	}
}