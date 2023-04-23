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

            // Configurando classe Configura��o

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

            // Injetando a camada de servi�os
            builder.Services.AddScoped<IServicesLayer, ServicesLayer>();

            // Injetando a camada de reposit�rios
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

            // Cria um escopo a parte para fornecer os servi�os que gerenciam a cria��o
            // de roles e do usu�rio padr�o. O escopo � descartado assim que as opera��es s�o finalizadas.
            using (var serviceScope = app.Services.CreateScope())
            {
                 // Cria as roles necess�rias para o projeto e o usu�rio administrador 
                 // padr�o se n�o existirem.
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
                // Verifica se as roles do array existem e cria novas caso n�o existam.
                var exists = await roleManager.RoleExistsAsync(role);
                if(!exists)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

        }

        private static async Task CreateDefaultAdminUser(IServiceProvider serviceProvider)
        {
            // Invoca as inst�ncias de servi�os necess�rios
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

            // Busca no banco o usu�rio administrador padr�o pelo seu email.
            var existUser = await userManager.FindByEmailAsync(adminUser.Email);

            // Se o usu�rio administrador padr�o n�o existir, cria um o atribui a role Admin.
            if(existUser == null)
            {
                try
                {  
                    // Cria o usu�rio.
                    IdentityResult userToCreateResult = await userManager.
                        CreateAsync(adminUser, configuration["Secrets:DefaultAdmin"]);

                    // Busca o usu�rio criado pelo email.
                    var createdUser = await userManager.FindByEmailAsync(adminUser.Email);

                    // Se o usu�rio for encontrado atribui a role de Admin ao usu�rio.
                    IdentityResult assignedRoleResult = new IdentityResult();
                    if (createdUser != null)
                    {
                        assignedRoleResult = await userManager.
                            AddToRoleAsync(createdUser, "Admin");
                    }
                    
                    // Se a cria��o de usu�rio e a atribui��o de role
                    // forem bem sucedidas, exibe o log.
                    if (userToCreateResult.Succeeded && assignedRoleResult.Succeeded)
                    {
                        logger.
                            LogInformation("O usu�rio administrador padr�o: "
                            + adminUser.UserName + " foi criado com sucesso.");

                    }

                }
                catch (Exception ex)
                {
                    logger.LogError("Falha ao criar usu�rio administrador padr�o: "
                            + adminUser.UserName + ". Erro: " + ex.Message);
                }

            }
            else
            {
                logger.LogInformation("J� existe um usu�rio administrador padr�o cadastrado!");
            }

       
        }
    }
}