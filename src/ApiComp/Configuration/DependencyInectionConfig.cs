using DevIO.Business.Intefaces;
using DevIO.Data.Context;
using DevIO.Data.Repository;

namespace ApiComp.Configuration
{
	public static class DependencyInectionConfig
	{
		public static IServiceCollection ResolverDependencias(this IServiceCollection services)
		{
			services.AddScoped<MeuDbContext>();
			services.AddScoped<IFornecedorRepository, FornecedorRepository>();
			return services;
		}
	}
}
