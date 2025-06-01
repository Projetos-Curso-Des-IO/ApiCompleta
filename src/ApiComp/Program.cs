using ApiComp.Configuration;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<MeuDbContext>(options =>
{
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection"),
		sql => sql.MigrationsAssembly("ApiComp")
	);
});

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddCors(options =>
{
	options.AddPolicy("Development", policy =>
	{
		policy.WithOrigins("" +
				"http://localhost:4200", 
				"https://localhost:7159", 
				"http://localhost:3001")
			  .AllowAnyMethod()
			  .AllowAnyHeader()
			  .AllowCredentials();
	});


	options.AddPolicy("Production", policy =>
	{
		policy.WithOrigins("" + "http://localhost:3001")
			  .AllowAnyMethod()
			  .AllowAnyHeader()
			  .AllowCredentials();
	});

});

builder.Services.ResolverDependencias();

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
	options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddOpenApi();

var app = builder.Build();

Console.WriteLine("Ambiente atual: " + app.Environment.EnvironmentName);

if (app.Environment.IsDevelopment())
{
	app.UseCors("Development");
	app.MapOpenApi();
	Console.WriteLine("Ambiente if: Development");
}
else
{
	app.UseCors("Production");
	app.UseHsts();
	app.UseHttpsRedirection();
	Console.WriteLine("Ambiente if: Production");
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
