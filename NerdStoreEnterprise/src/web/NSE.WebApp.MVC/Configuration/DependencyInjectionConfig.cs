using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSE.WebApp.MVC.Extensions;
using NSE.WebApp.MVC.Services;
using NSE.WebApp.MVC.Services.Handlers;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using System;
using System.Net.Http;

namespace NSE.WebApp.MVC.Configuration
{
	public static class DependencyInjectionConfig
	{
		public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IValidationAttributeAdapterProvider, CpfValidationAttributeAdapterProvider>();

			services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

			services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

			//Utilizado a biblioteca Polly
			services.AddHttpClient<ICatalogoService, CatalogoService>()
				.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
				//.AddTransientHttpErrorPolicy(
				//p=> p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(600)));// caso ocorrer algum erro, o sistema faz mais 3 tentativa antes de mostrar o erro na tela
				.AddPolicyHandler(PollyExtensions.TentarEsperar())
				.AddTransientHttpErrorPolicy(
				p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

				// 5 numero de vezes que a aplicação deve falhar

			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddScoped<IUser, AspNetUser>();

			//services.AddHttpClient("Refit", options => 
			//	{
			//		options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
			//	})
			//.AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
			//.AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>);
		}
	}

	public class PollyExtensions
	{
		public static AsyncRetryPolicy<HttpResponseMessage> TentarEsperar()
		{
			//Politica do plice, primeira ver caso ocorrer algum erro, irá aguardar 1 segundo para tentar novamente
			// ´pós espera 4 segundos para tentar novamente e etc. para no final caso ocorrer erro, ai sim mostrar o erro na tela.
			var retry = HttpPolicyExtensions
						.HandleTransientHttpError()
						.WaitAndRetryAsync(new[] {
							TimeSpan.FromSeconds(1),
							TimeSpan.FromSeconds(4),
							TimeSpan.FromSeconds(10)
						}, (outCome, timespan, retryCount, context) =>
						{
							Console.ForegroundColor = ConsoleColor.Blue;
							Console.WriteLine($"Tentando pela {retryCount} vez!");
							Console.ForegroundColor = ConsoleColor.White;
						});

			return retry;
		}
	}

}
