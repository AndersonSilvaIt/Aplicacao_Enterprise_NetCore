using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSE.BFF.Compras.Models;
using NSE.BFF.Compras.Services;
using NSE.WebAPI.Core.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NSE.BFF.Compras.Controllers
{
    [Authorize]
    public class CarrinhoController : MainController
    {
        private readonly ICarrinhoService _carrinhoSerivce;
        private readonly ICatalogoService _catalogoService;
        private readonly IPedidoService _pedidoService;


        public CarrinhoController(ICarrinhoService carrinhoService, 
                                  ICatalogoService catalogoService,
                                  IPedidoService pedidoService)
        {
            _carrinhoSerivce = carrinhoService;
            _catalogoService = catalogoService;
            _pedidoService = pedidoService;
        }

        [HttpGet]
        [Route("compras/carrinho")]
        public async Task<IActionResult> Index()
        {
            return CustomResponse(await _carrinhoSerivce.ObterCarrinho());
        }

        [HttpGet]
        [Route("compras/carrinho-quantidade")]
        public async Task<int> ObterQuantidadeCarrinho()
        {
            var quantidade = await _carrinhoSerivce.ObterCarrinho();
            return quantidade?.Itens.Sum(i => i.Quantidade) ?? 0;
        }

        [HttpPost]
        [Route("compras/carrinho/items")]
        public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoDTO itemProduto)
        {
            var produto = await _catalogoService.ObterPorId(itemProduto.ProdutoId);

            await ValidarItemCarrinho(produto, itemProduto.Quantidade);
            if (!OperacaoValida()) return CustomResponse();

            itemProduto.Nome = produto.Nome;
            itemProduto.Valor = produto.Valor;
            itemProduto.Imagem = produto.Imagem;

            var resposta = await _carrinhoSerivce.AdicionarItemCarrinho(itemProduto);

            return CustomResponse(resposta);
        }

        [HttpPut]
        [Route("compras/carrinho/items/{produtoid}")]
        public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO itemProduto)
        {
            var produto = await  _catalogoService.ObterPorId(produtoId);

            await ValidarItemCarrinho(produto, itemProduto.Quantidade);
            if (!OperacaoValida()) return CustomResponse();

            var resposta = await _carrinhoSerivce.AtualizarItemCarrinho(produtoId, itemProduto);

            return CustomResponse(resposta);
        }

        [HttpDelete]
        [Route("compras/carrinho/items/{produtoId}")]
        public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
        {
            var produto = await _catalogoService.ObterPorId(produtoId);
            if(produto == null)
            {
                AdicionarErroProcessamento("Produto ienxistente");
                return CustomResponse();
            }

            var resposta = await _carrinhoSerivce.RemoverItemCarrinho(produtoId);
            return CustomResponse(resposta);
        }

        [HttpPost]
        [Route("compras/carrinho/aplicar-voucher")]
        public async Task<IActionResult> AplicarVoucher([FromBody] string voucherCodigo)
        {
            var voucher = await _pedidoService.ObterVoucherPorCodigo(voucherCodigo);

            if(voucher == null)
            {
                AdicionarErroProcessamento("Voucher inválido ou não encontrado!");
                return CustomResponse();
            }

            var resposta = await _carrinhoSerivce.AplicarVoucherCarrinho(voucher);

            return CustomResponse(resposta);
        }

        private async Task ValidarItemCarrinho(ItemProdutoDTO produto, int quantidade)
        {
            if (produto == null) AdicionarErroProcessamento("Produto inexistente");
            if (quantidade < 1) AdicionarErroProcessamento($"Escolha ao menos uma unidade do produto {produto.Nome}");

            var carrinho = await _carrinhoSerivce.ObterCarrinho();

            var itemCarrinho = carrinho.Itens.FirstOrDefault(p => p.ProdutoId == produto.Id);

            if(itemCarrinho != null && itemCarrinho.Quantidade + quantidade < produto.QuantidadeEstoque)
            {
                AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade} unidades");
                return;
            }

            if (quantidade > produto.QuantidadeEstoque) AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades");
        }

    }
}