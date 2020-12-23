using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NSE.Clientes.API.Application.Commands;
using NSE.Clientes.API.Models;
using NSE.Core.Mediator;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Usuario;

namespace NSE.Clientes.API.Controllers
{
    public class ClientesController : MainController
    {
        private readonly IMediatorHandler _mediator;
        private readonly IClienteRepository _clienteRepository;
        private readonly IAspNetUser _user;

        public ClientesController(IMediatorHandler mediatorHandler,
            IClienteRepository clienteRepository, IAspNetUser aspNetUser)
        {
            _mediator = mediatorHandler;
            _clienteRepository = clienteRepository;
            _user = aspNetUser;
        }

        [HttpGet("clientes")]
        public async Task<IActionResult> Index()
        {
            var resultado = await _mediator.EnviarComando(
                new RegistrarClienteCommand(Guid.NewGuid(), "Eduardo", "edu@edu.com", "30314299076"));

            return CustomResponse(resultado);
        }

        [HttpPost("cliente/endereco")]
        public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
        {
            endereco.ClienteId = _user.ObterUserId();
            return CustomResponse(await _mediator.EnviarComando(endereco));
        }

    }
}