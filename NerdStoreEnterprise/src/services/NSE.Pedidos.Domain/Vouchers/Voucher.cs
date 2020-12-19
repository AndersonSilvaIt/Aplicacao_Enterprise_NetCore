﻿using NSE.Core.DomainObjects;
using NSE.Pedidos.Domain.Vouchers.Specs;
using System;
using System.Text;

namespace NSE.Pedidos.Domain
{
	public class Voucher : Entity, IAggregateRoot
	{
		public string Codigo { get; private set; }
		public decimal? Percentual { get; private set; }
		public decimal? ValorDesconto { get; private set; }
		public int Quantidade { get; private set; }
		public TipoDescontoVoucher TipoDesconto { get; private set; }
		public DateTime DataCriacao { get; private set; }
		public DateTime? DataUtilizacao { get; private set; }
		public DateTime DataValidade { get; private set; }
		public bool Ativo { get; private set; }
		public bool Utilizado { get; private set; }

		public bool EstaValidoParaUtilizacao()
		{

			//var spec = new VoucherAtivoSpecification()
			//	.And(new VoucherDataSpecification())
			//	.And(new VoucherQuantidadeSpecification());
			//
			//return spec.IsSatisfiedBy(this);

			return new VoucherAtivoSpecification()
				.And(new VoucherDataSpecification())
				.And(new VoucherQuantidadeSpecification()).IsSatisfiedBy(this);
		}

		public void MarcarComoUtilizado()
		{
			Ativo = false;
			Utilizado = true;
			Quantidade = 0;
		}

	}

}
