using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.Clientes.API.Data;
using Microsoft.AspNetCore.Builder;
using NSE.WebAPI.Core.Identidade;

namespace NSE.Clientes.API.Configuration
{
	public static class ApiConfig
	{

		public static void AddConfiguration(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddDbContext<ClientesContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			services.AddControllers();

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

		public static void UseConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseCors("Total");
			//app.UseAuthentication();
			app.UseAuthConfiguration();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

		}

	}
}
