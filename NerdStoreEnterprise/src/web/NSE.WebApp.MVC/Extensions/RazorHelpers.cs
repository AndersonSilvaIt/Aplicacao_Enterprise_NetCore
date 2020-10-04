using Microsoft.AspNetCore.Mvc.Razor;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace NSE.WebApp.MVC.Extensions
{
	public static class RazorHelpers
	{
		public static string HashEmailForGravatar(this RazorPage page, string email)
		{
			var mdgHasher = MD5.Create();
			var data = mdgHasher.ComputeHash(Encoding.Default.GetBytes(email));
			var sBuilder = new StringBuilder();

			foreach (var item in data)
			{
				sBuilder.Append(item.ToString("x2"));
			}

			return sBuilder.ToString();
		}

		public static string FormatoMoeda(this RazorPage page, decimal valor)
		{
			return valor >  0 ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", valor) : "Gratuito";
		}

		public static string MensagemEstoque(this RazorPage page, int quantidade)
		{
			return quantidade > 0 ? $"Apenas {quantidade} em estoque!" : "Produto esgotado!";
		}

	}
}
