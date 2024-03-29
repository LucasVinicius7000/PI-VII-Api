﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LocalStore.Services.Interfaces;
using LocalStore.Application.Controllers;
using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;
using LocalStore.Infra.Data.Repositories.Interfaces;
using LocalStore.Infra.Services.DistanceMatrix.Implementations;
using LocalStore.Application.Requests;
using LocalStore.Infra.Data.Context;
using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;
using Microsoft.EntityFrameworkCore;

namespace LocalStore.Services
{

    public class EstabelecimentoService
    {
        private readonly IServicesLayer _services;
        private readonly IRepositoryLayer _repositories;

        public EstabelecimentoService(IServicesLayer services, IRepositoryLayer repositories)
        {
            _services = services;
            _repositories = repositories;
        }

        public async Task<Estabelecimento> CriarEstabelecimentoUsuario(Estabelecimento estabelecimento, User userDTO)
        {
            try
            {
                if (estabelecimento == null) throw new Exception("O estabelecimento não pode ser nulo.");
                if (userDTO == null) throw new Exception("O usuário não pode ser nulo.");

                await _repositories.BeginTransaction();

                var usuarioCriado = await _services.User.CriarUsuario(userDTO);

                if (usuarioCriado is null)
                {
                    throw new Exception("Ocorreu um erro ao criar o usuário.");
                }

                var assignedRoleResult = await _services.UserManager.AddToRoleAsync(usuarioCriado, "Estabelecimento");

                if (!assignedRoleResult.Succeeded) throw new Exception("Ocorreu um erro ao definir usuário como estabelecimento.");

                estabelecimento.Aprovado = Domain.Enum.StatusAprovacao.PendenteFormulario;
                var estabelecimentoCriado = await _repositories.Estabelecimento.
                    InsertEstabelecimento(estabelecimento);

                if (estabelecimentoCriado == null)
                {
                    await _repositories.RollBackTransaction();
                    throw new Exception("Falha ao inserir estabelecimento. ");
                }

                await _repositories.CommitTransaction();
                return estabelecimentoCriado;

            }
            catch (Exception ex)
            {
                await _repositories.RollBackTransaction();
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Estabelecimento>> BuscarEstabelecimentosProximosPorRaio(Coordinates origem, double raio)
        {
            try
            {
                if (origem == null || double.IsNaN(origem.Longitude) || double.IsNaN(origem.Latitude))
                {
                    throw new Exception("As coordenadas informadas são invállidas.");
                }

                if (raio <= 0 || double.IsNaN(raio))
                {
                    throw new Exception("O raio da pesquisa informado não é válido.");
                }

                List<Estabelecimento> estabelecimentos = new List<Estabelecimento>();
                estabelecimentos = await _repositories.Estabelecimento.ListaEstabelecimentosPorRaioECoordenadas(origem, raio);
                if (estabelecimentos.Count == 0 || estabelecimentos == null) throw new Exception("Nenhum estabelecimento próximo foi encontrado.");
                estabelecimentos = estabelecimentos.Where(e => e.Aprovado == Domain.Enum.StatusAprovacao.Aprovado).ToList();
                var geolocation = new Geolocation(_services.Configuration);

                var estabelecimentosComDistancia = new List<Estabelecimento>();


                foreach (var e in estabelecimentos)
                {
                    var destino = new Coordinates() { Latitude = e.Latitude, Longitude = e.Longitude };
                    var estabelecimentoTemp = e;
                    e.DistanciaEstabelecimentoUsuario = await geolocation.CalculateDistanceByCoordinates(origem, destino);
                    if (e.DistanciaEstabelecimentoUsuario <= raio)
                    {
                        estabelecimentosComDistancia.Add(estabelecimentoTemp);
                    }
                }

                return estabelecimentosComDistancia;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> VerificaEstabelecimentoExistePeloId(int Id)
        {
            if (Id <= 0) throw new Exception("O id informado não é válido.");

            try
            {
                var estabelecimento = await  _repositories.Estabelecimento.BuscarEstabelecimentoPeloId(Id);
                if (estabelecimento == null) return false;
                else return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<Estabelecimento> BuscarEstabelecimentoPorId(int EstabelecimentoId)
        {
            try
            {
                if (EstabelecimentoId == null) throw new Exception("O id do estabelecimento não é válido.");

                var estabelecimentoEncontrado = await _repositories.Estabelecimento.BuscarEstabelecimentoPeloId(EstabelecimentoId);
                estabelecimentoEncontrado.Produtos = await _repositories.Produto.BuscarProdutosPorEstabelecimentoId(EstabelecimentoId);
                if (estabelecimentoEncontrado is null) throw new Exception("O estabelecimento informado não foi encontrado na base de dados.");
                return estabelecimentoEncontrado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Estabelecimento> BuscarEstabelecimentoPorEmail(string Email)
        {
            try
            {
                if (Email == null || Email == string.Empty || !Email.Contains('@')) throw new Exception("O email do usuário não corresponde a nenhum estabelecimento cadastrado.");

                var estabelecimentoEncontrado = await _repositories.Estabelecimento.BuscarEstabelecimentoPeloEmail(Email);

                if (estabelecimentoEncontrado is null) throw new Exception("O estabelecimento informado não foi encontrado na base de dados.");
                return estabelecimentoEncontrado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Estabelecimento> SubmeterFormularioDeAplicacao(Estabelecimento estabelecimento)
        {
            try
            {
                var estabelecimentoAtualizado = await _repositories.Estabelecimento.AtualizarEstabelecimento(estabelecimento);
                if (estabelecimentoAtualizado is null) throw new Exception("Ocorreu um erro ao submeter formulário de aplicação.");
                else return estabelecimentoAtualizado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Estabelecimento>> ListarEstabelecimentosPendentes()
        {
            try
            {
                var estabelecimentos = await _repositories.Estabelecimento._context.Set<Estabelecimento>()
                    .Where(e => e.Aprovado == Domain.Enum.StatusAprovacao.PendenteAprovacao)
                    .ToListAsync();
                return estabelecimentos;
                    
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Estabelecimento> AlterarStatusAprovacao(Boolean status, int idEstabelecimento)
        {
            try
            {
                var novoStatus = Domain.Enum.StatusAprovacao.Reprovado;
                if (status)
                {
                    novoStatus = Domain.Enum.StatusAprovacao.Aprovado;
                }

                var estabelecimentos = await _repositories.Estabelecimento.
                    _context.Set<Estabelecimento>()
                    .Where(e => e.Id == idEstabelecimento)
                    .FirstAsync();

                estabelecimentos.Aprovado = novoStatus;
                var newEmpresa = _repositories.Estabelecimento._context.Set<Estabelecimento>().Update(estabelecimentos);
                await _repositories.Estabelecimento._context.SaveChangesAsync();
                return newEmpresa.Entity;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
