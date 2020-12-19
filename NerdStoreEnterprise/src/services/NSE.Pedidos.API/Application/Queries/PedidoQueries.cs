﻿using Dapper;
using NSE.Pedidos.API.Application.DTO;
using NSE.Pedidos.Domain.Pedidos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.Pedidos.API.Application.Queries
{
	public interface IPedidoQueries
	{
		Task<PedidoDTO> ObterUltimoPedido(Guid clienteId);
		Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId(Guid clienteId);
	}

	public class PedidoQueries : IPedidoQueries
	{
		private readonly IPedidoRepository _pedidoRepository;

		public PedidoQueries(IPedidoRepository pedidoRepository)
		{
			_pedidoRepository = pedidoRepository;
		}

		public async Task<PedidoDTO> ObterUltimoPedido(Guid clienteId)
		{
			const string sql = @"SELECT
                                P.ID AS 'ProdutoId', P.CODIGO, P.VOUCHERUTILIZADO, P.DESCONTO, P.VALORTOTAL,P.PEDIDOSTATUS,
                                P.LOGRADOURO,P.NUMERO, P.BAIRRO, P.CEP, P.COMPLEMENTO, P.CIDADE, P.ESTADO,
                                PIT.ID AS 'ProdutoItemId',PIT.PRODUTONOME, PIT.QUANTIDADE, PIT.PRODUTOIMAGEM, PIT.VALORUNITARIO 
                                FROM PEDIDOS P 
                                INNER JOIN PEDIDOITEMS PIT ON P.ID = PIT.PEDIDOID 
                                WHERE P.CLIENTEID = @clienteId 
                                AND P.DATACADASTRO between DATEADD(minute, -3,  GETDATE()) and DATEADD(minute, 0,  GETDATE())
                                AND P.PEDIDOSTATUS = 1 
                                ORDER BY P.DATACADASTRO DESC";

			//var pedido = await _pedidoRepository.ObterConexao()
			//	.QueryAsync<Pedido>(sql);
			//
			//var connection = _pedidoRepository.ObterConexao();
			//var teste = await connection.QueryAsync<Pedido>(sql);

			// o clienteId é passado como parâmetro, de acordo com a ordem em que foi colocado, isso evita o SLQ Injection
			var pedido = await _pedidoRepository.ObterConexao()
				.QueryAsync<dynamic>(sql, new { clienteId });

			return MapearPedido(pedido);
		}

		public async Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId(Guid clienteId)
		{
			var pedidos = await _pedidoRepository.ObterListaPorClienteId(clienteId);
			return pedidos.Select(PedidoDTO.ParaPedidoDTO);
		}

		private PedidoDTO MapearPedido(dynamic result)
		{
			var pedido = new PedidoDTO
			{
				Codigo = result[0].CODIGO,
				Status = result[0].PEDIDOSTATUS,
				ValorTotal = result[0].VALORTOTAL,
				Desconto = result[0].DESCONTO,
				VoucherUtilizado = result[0].VOUCHERUTILIZADO,

				PedidoItems = new List<PedidoItemDTO>(),
				Endereco = new EnderecoDTO
				{
					Logradouro = result[0].LOGRADOURO,
					Bairro = result[0].BAIRRO,
					Cep = result[0].CEP,
					Cidade = result[0].CIDADE,
					Complemento = result[0].COMPLEMENTO,
					Estado = result[0].ESTADO,
					Numero = result[0].NUMERO
				}
			};

			foreach (var item in result)
			{
				var pedidoItem = new PedidoItemDTO
				{
					Nome = item.PRODUTONOME,
					Valor = item.VALORUNITARIO,
					Quantidade = item.QUANTIDADE,
					Imagem = item.PRODUTOIMAGEM,
				};

				pedido.PedidoItems.Add(pedidoItem);
			}

			return pedido;
		}

	}
}
