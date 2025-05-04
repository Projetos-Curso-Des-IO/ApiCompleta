using ApiComp.Data;
using ApiComp.Extenssions;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiComp.Configuration
{
	public static class IdentityConfig
	{
		public static IServiceCollection AddIdentityConfiguration(this 
			IServiceCollection services,
			IConfiguration configuration)
		{

			#region Configuração do Entity Framework DbContext - Banco
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
				sql => sql.MigrationsAssembly("ApiComp")));
			#endregion


			#region Configuração da Identity
			services.AddDefaultIdentity<IdentityUser>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddErrorDescriber<IdentityMensagensPortugues>()
				.AddDefaultTokenProviders();
			#endregion

			
			#region JWT
			var appSettingsSection = configuration.GetSection("AppSettings");
			services.Configure<AppSettings>(appSettingsSection);

			var appSettings = appSettingsSection.Get<AppSettings>();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);

			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Define o esquema padrão de autenticação como JWT
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;    // Define o esquema padrão de desafio como JWT
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = true; // Exige HTTPS para os tokens (mais seguro)
				x.SaveToken = true; // Salva o token no contexto de autenticação
				x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token
					IssuerSigningKey = new SymmetricSecurityKey(key), // Define a chave usada para validar o token
					ValidateIssuer = true, // Valida o emissor do token (Issuer)
					ValidateAudience = true, // Valida o público-alvo do token (Audience)
					ValidAudience = appSettings.ValidoEm, // Define o valor esperado para o público (Audience)
					ValidIssuer = appSettings.Emissor // Define o valor esperado para o emissor (Issuer)
				};
			});
			#endregion


			return services;
		}
		 
	}
}
