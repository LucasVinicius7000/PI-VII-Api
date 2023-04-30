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

                if(usuarioCriado is null)
                {
                    throw new Exception("Ocorreu um erro ao criar o usuário.");
                }

                var estabelecimentoCriado = await _repositories.Estabelecimento.
                    InsertEstabelecimento(estabelecimento);

                if(estabelecimentoCriado == null)
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
                if(origem == null || double.IsNaN(origem.Longitude) || double.IsNaN(origem.Latitude))
                {
                    throw new Exception("As coordenadas informadas são invállidas.");
                }

                if (raio <= 0 || double.IsNaN(raio))
                {
                    throw new Exception("O raio da pesquisa informado não é válido.");
                }

                List<Estabelecimento> estabelecimentos = new List<Estabelecimento>();
                estabelecimentos =  await _repositories.Estabelecimento.ListaEstabelecimentosPorRaioECoordenadas(origem, raio);
                if(estabelecimentos.Count == 0 || estabelecimentos == null) throw new Exception("Nenhum estabelecimento próximo foi encontrado.");

                var geolocation = new Geolocation(_services.Configuration);

                var estabelecimentosComDistancia = new List<Estabelecimento>();


                foreach(var e in estabelecimentos)
                {
                    var destino = new Coordinates() { Latitude = e.Latitude, Longitude = e.Longitude };
                    var estabelecimentoTemp = e;
                    e.DistanciaEstabelecimentoUsuario = await geolocation.CalculateDistanceByCoordinates(origem, destino);
                    if(e.DistanciaEstabelecimentoUsuario <= raio)
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

    }
}
