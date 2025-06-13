using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;
using Microsoft.Extensions.DependencyInjection;

namespace ApiComp.Configuration
{

	public static class SwaggerConfig
	{
		public static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
		{
			services.AddSwaggerGen(static c =>
			{
				c.OperationFilter<SwaggerDefaultValue>();

				c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Description = "Informe o token JWT no formato: {seu token}",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					BearerFormat = "JWT"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Id = "Bearer",
								Type = ReferenceType.SecurityScheme
							}
						},
						Array.Empty<string>()
					}
				});
			
			});
			return services;
		}


		public static IApplicationBuilder UseSwaggerConfig(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
		{
			//app.UseMiddleware<SwaggerAuthorizationMiddleware>();
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				foreach (var description in provider.ApiVersionDescriptions)
				{
					c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
						$"Minha API {description.GroupName.ToUpperInvariant()}");
				}
			});

			return app;
		}

	}

	public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
	{
		readonly IApiVersionDescriptionProvider provider;

		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this.provider = provider;

		public void Configure(SwaggerGenOptions options)
		{
			foreach (var description in provider.ApiVersionDescriptions)
			{
				options.SwaggerDoc(description.GroupName, CreateInfoApiVersion(description));
			}
		}

		private OpenApiInfo CreateInfoApiVersion(ApiVersionDescription description)
		{
			var info = new OpenApiInfo()
			{
				Title = "Api - curso dev",
				Version = description.ApiVersion.ToString(),
				Description = "Esta API faz parte do cuso de REST com ASP.NET Core WebAPI",
				Contact = new OpenApiContact() 
				{ 
					Name = "Josiel Costa", 
					Email = "cjosiel2@gmail.com"
				},

				TermsOfService = new Uri("https://opensource.org/licences/MIT"),
				License = new OpenApiLicense				{ 
					Name = "MIT", 
					Url = new Uri("https://opensource.org/licences/MIT") 
				}
			};

			if (description.IsDeprecated)
			{
				info.Description += "Esta versão está obsoleta!";
			}

			return info;
		}
	}


	public class SwaggerDefaultValue : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			var apiDescription = context.ApiDescription;

			operation.Deprecated = apiDescription.IsDeprecated();

			if(operation.Parameters == null)
			{
				return;
			}

			foreach (var parameter in operation.Parameters)
			{
				var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

				if(parameter.Description == null)
				{
					parameter.Description = description.ModelMetadata?.Description;
				}

				if(parameter.Required == false)
				{
					parameter.Required = description.IsRequired;
				}

				parameter.Required |= description.IsRequired;
			}
		}
	}


	public class SwaggerAuthorizationMiddleware
	{
		private readonly RequestDelegate _next;

		public SwaggerAuthorizationMiddleware(RequestDelegate next)
		{
			_next = next;
		}
		
		public async Task Invoke(HttpContext context)
		{
			if (context.Request.Path.StartsWithSegments("/swagger")
				&& !context.User.Identity.IsAuthenticated)
			{
				context.Response.StatusCode = StatusCodes.Status401Unauthorized;
				return;
			}

			await _next.Invoke(context);
		}
	}

}

