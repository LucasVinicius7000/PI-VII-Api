using LocalStore.Infra.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LocalStore.Infra.Services.BlobStorage.Implementations;
using LocalStore.Infra.Services.BlobStorage.Interfaces;
using LocalStore.Infra.Data.Repositories.Implementations;
using LocalStore.Infra.Data.Repositories.Interfaces;
using LocalStore.Services.Interfaces;
using LocalStore.Services.Implementations;
using Newtonsoft.Json;

namespace LocalStore
{
    public class Program
    {

        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Configurando classe Configuração

            var configuration = new ConfigurationBuilder()
            .SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddControllers().AddNewtonsoftJson();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();

            // Injetando o contexto do banco de dados
            builder.Services.AddDbContext<LocalStoreDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("LocalStoreDb"))
            );

            // Injetando a camada de serviços
            builder.Services.AddScoped<IServicesLayer, ServicesLayer>();

            // Injetando a camada de repositórios
            builder.Services.AddScoped<IRepositoryLayer, RepositoryLayer>();

            // Injetando o identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<LocalStoreDbContext>()
            .AddDefaultTokenProviders();

          
            var app = builder.Build();

            // Cria um escopo a parte para fornecer os serviços que gerenciam a criação
            // de roles e do usuário padrão. O escopo é descartado assim que as operações são finalizadas.
            using (var serviceScope = app.Services.CreateScope())
            {
                 // Cria as roles necessárias para o projeto e o usuário administrador 
                 // padrão se não existirem.
                var creatingRoles = CreateRoles(serviceScope.ServiceProvider);
                Task.WaitAny(creatingRoles);
                var creatingDefaultUser = CreateDefaultAdminUser(serviceScope.ServiceProvider);
                Task.WaitAny(creatingDefaultUser);
            
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }

        private static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] basicRoles = { "Admin", "Cliente", "Estabelecimento" };

            foreach(var role in basicRoles)
            {
                // Verifica se as roles do array existem e cria novas caso não existam.
                var exists = await roleManager.RoleExistsAsync(role);
                if(!exists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

        }

        private static async Task CreateDefaultAdminUser(IServiceProvider serviceProvider)
        {
            // Invoca as instâncias de serviços necessários
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            var adminUser = new IdentityUser()
            {
                Email = "lucascambraia@unipam.edu.br",
                UserName = "DefaultAdmin",
                EmailConfirmed = true,
            };

            // Busca no banco o usuário administrador padrão pelo seu email.
            var existUser = await userManager.FindByEmailAsync(adminUser.Email);

            // Se o usuário administrador padrão não existir, cria um o atribui a role Admin.
            if(existUser == null)
            {
                try
                {  
                    // Cria o usuário.
                    IdentityResult userToCreateResult = await userManager.
                        CreateAsync(adminUser, configuration["Secrets:DefaultAdmin"]);

                    // Busca o usuário criado pelo email.
                    var createdUser = await userManager.FindByEmailAsync(adminUser.Email);

                    // Se o usuário for encontrado atribui a role de Admin ao usuário.
                    IdentityResult assignedRoleResult = new IdentityResult();
                    if (createdUser != null)
                    {
                        assignedRoleResult = await userManager.
                            AddToRoleAsync(createdUser, "Admin");
                    }
                    
                    // Se a criação de usuário e a atribuição de role
                    // forem bem sucedidas, exibe o log.
                    if (userToCreateResult.Succeeded && assignedRoleResult.Succeeded)
                    {
                        logger.
                            LogInformation("O usuário administrador padrão: "
                            + adminUser.UserName + " foi criado com sucesso.");

                    }

                }
                catch (Exception ex)
                {
                    logger.LogError("Falha ao criar usuário administrador padrão: "
                            + adminUser.UserName + ". Erro: " + ex.Message);
                }

            }
            else
            {
                logger.LogInformation("Já existe um usuário administrador padrão cadastrado!");
            }

       
        }
    }
}