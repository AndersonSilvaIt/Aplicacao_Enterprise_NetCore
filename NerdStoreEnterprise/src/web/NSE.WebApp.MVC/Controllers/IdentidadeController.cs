using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Controllers
{
	public class IdentidadeController : Controller
	{
		[HttpGet]
		[Route("nova-conta")]
		public IActionResult Registro()
		{
			return View();
		}

		[HttpPost]
		[Route("nova-conta")]
		public async Task<IActionResult> Resgistro(UsuarioRegistro usuarioRegistro)
		{
			if (!ModelState.IsValid) return View(usuarioRegistro);

			// API - Registro

			if (false) return View(usuarioRegistro);

			// Realiza o login na APP
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[Route("login")]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> Login(UsuarioLogin usuarioLogin)
		{
			if (!ModelState.IsValid) return View(usuarioLogin);

			// API - Login

			if (false) return View(usuarioLogin);

			// Realiza o login na APP
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[Route("sair")]
		public async Task<IActionResult> Logout()
		{

			return RedirectToAction("Index", "Home");
		}

	}
}
