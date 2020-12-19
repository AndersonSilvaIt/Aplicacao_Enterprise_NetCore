using FluentValidation;
using NSE.Core.Messages;
using NSE.Pedidos.API.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NSE.Pedidos.API.Application.Commands
{
	public class AdicionarPedidoCommand : Command
	{
		//Pedido
		public Guid ClienteId { get; set; }
		public decimal ValorTotal { get; set; }
		public List<PedidoItemDTO> PedidoItems { get; set; }

		// Voucher
		public string VoucherCodigo { get; set; }
		public bool VoucherUtilizado { get; set; }
		public decimal Desconto { get; set; }

		// Endereco
		public EnderecoDTO Endereco { get; set; }

		public override bool EhValido()
		{
			ValidationResult = new AdicionarPedidoValidation().Validate(this);
			return ValidationResult.IsValid;
		}

		//Cartao
		public string NumeroCartao { get; set; }
		public string NomerCartao { get; set; }
		public string ExpiracaoCartao { get; set; }
		public string CvvCartao { get; set; }
	}

	public class AdicionarPedidoValidation : AbstractValidator<AdicionarPedidoCommand>
	{
		public AdicionarPedidoValidation()
		{
			RuleFor(c => c.ClienteId)
				.NotEqual(Guid.Empty)
				.WithMessage("Id do cliente inválido");

			RuleFor(c => c.PedidoItems.Count())
				.GreaterThan(0)
				.WithMessage("O pedido precisa ter no mínimo 1 item");

			RuleFor(c => c.ValorTotal)
				.GreaterThan(0)
				.WithMessage("Valor do pedido inválido");

			RuleFor(c => c.NumeroCartao)
				.CreditCard()
				.WithMessage("Número de cartão inválido");

			RuleFor(c => c.NomerCartao)
				.NotNull()
				.WithMessage("Nome do portador do cartão requerido");

			RuleFor(c => c.CvvCartao.Length)
				.GreaterThan(2)
				.LessThan(5)
				.WithMessage("o CVV do cartão precisa ter entre 3 ou números.");

			RuleFor(c => c.ExpiracaoCartao)
				.NotNull()
				.WithMessage("Data expiração do cartão requerida.");
		}
	}

}
