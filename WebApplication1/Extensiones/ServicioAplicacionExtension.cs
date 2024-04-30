using Data.Servicios;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Data.Interfaces;
using API.Errores;

namespace WebApplication1.Extensiones
{
    public  static class ServicioAplicacionExtension
    {

        public static IServiceCollection AgregarServicioAplicacion(this IServiceCollection services, IConfiguration config)
        {

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Ingresar Bearer [espacion] token \r\n\r\n " +
                                    "Ejemplo: Berear ejoy^8878899999990000",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                 {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme ="oauth2",
                        Name ="Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
            });

            var connectionString = config.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(Options => Options.UseSqlServer(connectionString));

            services.AddCors();

            services.AddScoped<ITokenServices, TokenServicio>();
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionsContext =>
                {
                    var errores = actionsContext.ModelState
                                .Where(e => e.Value.Errors.Count > 0)
                                .SelectMany(x => x.Value.Errors)
                                .Select(x => x.ErrorMessage).ToArray();
                    var errorResponse = new ApiValidacionErrorResponse
                    {
                        Errores = errores
                    };
                    return new BadRequestObjectResult(errorResponse);
                }; 
            });
            return services;
        }

    }
}
