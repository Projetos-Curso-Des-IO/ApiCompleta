using ApiComp.Data;
using ApiComp.Extenssions;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ApiComp.Configuration
{
	public static class IdentityConfig
	{
		public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
			IConfiguration configuration)
		{

			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
				sql => sql.MigrationsAssembly("ApiComp")));

			services.AddDefaultIdentity<IdentityUser>()
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddErrorDescriber<IdentityMensagensPortugues>()
				.AddDefaultTokenProviders();

			return services;
		}
		 
	}
}
