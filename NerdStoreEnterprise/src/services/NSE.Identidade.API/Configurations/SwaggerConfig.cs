using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace NSE.Identidade.API.Configurations
{
	public static class SwaggerConfig
	{
		public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
		{
			services.AddSwaggerGen(c => {
				c.SwaggerDoc("v1", new OpenApiInfo()
				{
					Title = "Nerdstore Enterprise Identity API",
					Description = "Esta API faz parte do custo Asp.Net Core Enterprise Application.",
					Contact = new OpenApiContact() { Name = "Anderson Enterprise", Email = "anderson.silvait@outlook.com.br" },
					License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
				});
			});

			return services;
		}

		public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c => {
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
			});

			return app;
		}

	}
}
