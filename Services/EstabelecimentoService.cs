using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LocalStore.Services.Interfaces;
using LocalStore.Application.Controllers;
using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;
using LocalStore.Infra.Data.Repositories.Interfaces;

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

        public async Task<Estabelecimento> CriarEstabelecimentoUsuario(Estabelecimento estabelecimento, UserDTO userDTO)
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


    }
}
