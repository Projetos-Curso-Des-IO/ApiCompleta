using ApiComp.Configuration;
using DevIO.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

builder.Services.AddSwaggerConfig();

//builder.Services.AddSwaggerGen(c =>
//{
//	var provider = builder.Services.BuildServiceProvider()
//		.GetRequiredService<IApiVersionDescriptionProvider>();

//	foreach (var description in provider.ApiVersionDescriptions)
//	{
//		c.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo
//		{
//			Title = $"Api curso {description.ApiVersion}",
//			Version = description.ApiVersion.ToString()
//		});
//	}
//});



builder.Services.ResolverDependencias();

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddApiVersioning(option =>
{
	option.AssumeDefaultVersionWhenUnspecified = true;
	option.DefaultApiVersion = new ApiVersion(1, 0);
	option.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true;
});

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

//app.UseSwagger();

//var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

//app.UseSwaggerUI(c =>
//{
//	foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
//	{
//		c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
//			$"Minha API {description.GroupName.ToUpperInvariant()}");
//	}
//});
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerConfig(apiVersionDescriptionProvider);

app.MapControllers();

app.Run();
