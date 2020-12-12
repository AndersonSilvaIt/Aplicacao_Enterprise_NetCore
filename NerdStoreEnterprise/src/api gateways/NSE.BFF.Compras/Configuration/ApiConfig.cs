using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using NSE.BFF.Compras.Services;
using NSE.WebAPI.Core.Identidade;
using NSE.BFF.Compras.Extensions;

namespace NSE.BFF.Compras.Configuration
{
	public static class ApiConfig
	{
		public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddControllers();

			services.Configure<AppServicesSettings>(configuration);

			services.AddCors(options =>
			{
				options.AddPolicy("Total",
					builder =>
						builder
							.AllowAnyOrigin()
							.AllowAnyMethod()
							.AllowAnyHeader());
			});
		}

		public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseCors("Total");
			app.UseAuthConfiguration();

			app.UseEndpoints(endopoints =>
			{
				endopoints.MapControllers();
			});
		}
	}
}
