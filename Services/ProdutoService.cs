using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LocalStore.Services.Interfaces;
using LocalStore.Application.Controllers;
using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;
using LocalStore.Infra.Data.Repositories.Interfaces;
using LocalStore.Infra.Services.DistanceMatrix.Implementations;

namespace LocalStore.Services
{
    public class ProdutoService
    {
        private readonly IServicesLayer _services;
        private readonly IRepositoryLayer _repositories;

        public ProdutoService(IServicesLayer services, IRepositoryLayer repositories)
        {
            _services = services;
            _repositories = repositories;
        }

        public async Task<Produto> CadastrarProduto(Produto produto)
        {
            try
            {
                if (produto is null) throw new Exception("Os dados do produto são inválidos.");
                if (produto.ValorUnitario <= 0) throw new Exception("O valor do produto deve ser maior que zero.");
                if (produto.QuantidadeEstoque <= 0) throw new Exception("Quantidade em estoque não pode ser menor ou igual a zero.");
                if (produto.VencimentoEm < DateTime.Now) throw new Exception("Não é possível cadastrar produtos vencidos.");
                if (produto.EstabelecimentoId <= 0) throw new Exception("Não foi possível vincular o produto ao estabelecimento. O id do estabelecimento é inválido.");
                var estabelecimentoExiste = await _services.Estabelecimento.VerificaEstabelecimentoExistePeloId(produto.EstabelecimentoId);
                if (!estabelecimentoExiste) throw new Exception("O id informado não corresponde a nenhum estabelecimento cadastrado, não foi possível cadastrar o produto.");
                        
                var produtoCadastrado = await _repositories.Produto.InsertProduto(produto);
                if (produtoCadastrado == null) throw new Exception("Ocorreu um desconhecido ao cadastrar o produto, tente novamente.");
                return produtoCadastrado;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}