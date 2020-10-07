using FluentValidation.Results;
using MediatR;
using NSE.Clientes.API.Models;
using NSE.Core.Messages;
using System.Threading;
using System.Threading.Tasks;

namespace NSE.Clientes.API.Application.Commands
{
	public class ClienteCommandHandler : CommandHandler, IRequestHandler<RegistrarClienteCommand, ValidationResult>
	{
		public async Task<ValidationResult> Handle(RegistrarClienteCommand message, CancellationToken cancellationToken)
		{
			if (!message.EhValido()) return message.ValidationResult;

			var cliente = new Cliente(message.Id, message.Nome, message.Email, message.Cpf);

			// validações de negócio

			// persistir no banco!

			// verificar se cliente já existe no bancos
			if (true) // já existe um cliente com o CPF informado
			{
				AdicionarErro("Esse CPF já está em uso.");
				return ValidationResult;
			}

			return message.ValidationResult;
		}

	}
}
