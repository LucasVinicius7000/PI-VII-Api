using LocalStore.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using LocalStore.Domain.DTO;
using LocalStore.Infra.Data.Repositories.Interfaces;

namespace LocalStore.Services
{
    public class UserService
    {
        private readonly IServicesLayer _services;
        private readonly IRepositoryLayer _repositories;

        public UserService(IServicesLayer services, IRepositoryLayer repositories)
        {
            _services = services;
            _repositories = repositories;
        }

        public async Task<IdentityUser> CriarUsuario(User user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(user.Senha))
                {
                    var usuarioBuscado = await _services.User.ExisteUsuarioCadastradoComEmailInformado(user.Email);
                    if (usuarioBuscado) throw new Exception("Já existe um usuário cadastrado com esse email.");

                    var usuarioParaCadastrar = new IdentityUser()
                    {
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber,
                        UserName = user.UserName,
                    };

                    var usuarioResult = await _services.UserManager.CreateAsync(usuarioParaCadastrar, user.Senha);
                    var usuarioCadastrado = await _services.UserManager.FindByEmailAsync(user.Email);

                    if (usuarioResult.Succeeded && usuarioCadastrado != null)
                    {
                        return usuarioCadastrado;
                    }
                    else throw new Exception("Ocorreu um erro ao criar o usuário.");

                }
                else throw new Exception("Email ou senha inválidos.");


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> ExisteUsuarioCadastradoComEmailInformado(string email)
        {
            try
            {
                if (!string.IsNullOrEmpty(email))
                {
                    var usuario = await _services.UserManager.FindByEmailAsync(email);
                    if (usuario != null) return true;
                    else return false;
                }
                else throw new Exception("Email inválido. Não foi possível verificar se existe um usuário cadastrado com o email informado.");
            }
            catch (Exception ex)
            {

                throw new Exception("Erro ao verificar se existe um usuário cadastrado com o email informado. " + ex.Message);
            }
        }

    }
}
