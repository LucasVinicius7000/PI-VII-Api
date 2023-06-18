using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LocalStore.Services.Interfaces;
using LocalStore.Application.Controllers;
using LocalStore.Domain.Model;
using LocalStore.Domain.DTO;
using LocalStore.Infra.Data.Repositories.Interfaces;
using LocalStore.Infra.Services.DistanceMatrix.Implementations;
using Microsoft.AspNetCore.Identity;

namespace LocalStore.Services
{
    public class ClienteService
    {
        private readonly IServicesLayer _services;
        private readonly IRepositoryLayer _repositories;

        public ClienteService(IServicesLayer services, IRepositoryLayer repositories)
        {
            _services = services;
            _repositories = repositories;
        }

        public async Task<Cliente> CriarUsuarioCliente(Cliente cliente, User userDto)
        {
            try
            {
                await _repositories.BeginTransaction();

                var userExists = await _services.User.ExisteUsuarioCadastradoComEmailInformado(userDto.Email);
                if (userExists) throw new Exception("Já existe um usuário cadastrado com esse email.");   

                var usuarioCriado = await _services.User.CriarUsuario(userDto);

                if (usuarioCriado is null)
                {
                    throw new Exception("Ocorreu um erro ao criar o usuário.");
                }

                var assignedRoleResult = await _services.UserManager.AddToRoleAsync(usuarioCriado, "Cliente");

                if (!assignedRoleResult.Succeeded) throw new Exception("Ocorreu um erro ao definir usuário como cliente.");

                cliente.UserId = usuarioCriado.Id;
                cliente.Nome = cliente.Nome == string.Empty ? userDto.UserName : cliente.Nome;
                var clienteCriado = await _repositories.Cliente.InsertCliente(cliente);

                if (clienteCriado == null)
                {
                    await _repositories.RollBackTransaction();
                    throw new Exception("Falha ao cadastrar cliente. ");
                }

                await _repositories.CommitTransaction();
                return clienteCriado;

            }
            catch (Exception ex)
            {
                await _repositories.RollBackTransaction();
                throw new Exception(ex.Message);
            }
        }

        public async Task<Cliente> BuscarClientePeloUserId(string userId)
        {
            try
            {
                if (userId == string.Empty) throw new Exception("O user id do cliente não é válido");
                var cliente = await _repositories.Cliente.BuscarClientePeloUserId(userId);
                if (cliente == null) throw new Exception("Cliente não encontrado na base de dados.");
                return cliente;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Cliente> BuscarClientePeloId(int Id)
        {
            try
            {
                if (Id == 0) throw new Exception("O user id do cliente não é válido");
                var cliente = await _repositories.Cliente.BuscarClientePeloId(Id);
                if (cliente == null) throw new Exception("Cliente não encontrado na base de dados.");
                return cliente;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
