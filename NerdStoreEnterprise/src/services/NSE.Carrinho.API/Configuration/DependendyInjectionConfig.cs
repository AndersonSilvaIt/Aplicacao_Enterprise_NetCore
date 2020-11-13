using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using NSE.Carrinho.API.Data;

namespace NSE.Carrinho.API.Configuration
{
	public static class DependendyInjectionConfig
	{
		public static void RegisterServices(this IServiceCollection services)
		{
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<CarrinhoContext>();
		}
	}
}
