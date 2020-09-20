using NSE.WebApp.MVC.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NSE.WebApp.MVC.Services
{
	public class AutenticacaoService : IAutenticacaoService
	{
		private readonly HttpClient _httpClient;
		public AutenticacaoService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<UsuarioRespostaLogin> Login(UsuarioLogin usuarioLogin)
		{
			var loginContent = new StringContent(
				JsonSerializer.Serialize(usuarioLogin),
				Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("https://localhost:44365/api/identidade/autenticar", loginContent);

			//var teste = response.Content.ReadAsStringAsync();

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};

			return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), options);
		}

		public async Task<UsuarioRespostaLogin> Registro(UsuarioRegistro usuarioRegistro)
		{
			var registroContent = new StringContent(
					JsonSerializer.Serialize(usuarioRegistro),
					Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("https://localhost:44365/api/identidade/nova-conta", registroContent);

			//var teste = await response.Content.ReadAsStringAsync();
			//Esse trecho de código serve para realizar a Serialização, isso no System.Text, pois o Objeto contem as propriedades 
			//com letras maiúsculas, e os dados que vem da API estão todas em letras minúsculas.
			// isso era tratado no NewtonSoft
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};

			return JsonSerializer.Deserialize<UsuarioRespostaLogin>(await response.Content.ReadAsStringAsync(), options);
		}
	}
}
