using Data.Servicios;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Data.Interfaces;
using API.Errores;

namespace WebApplication1.Extensiones
{
    /// <summary>
    /// Clase estática que contiene métodos de extensión para configurar servicios de aplicación.
    /// </summary>
    public static class ServicioAplicacionExtension
    {
        /// <summary>
        /// Método de extensión para agregar servicios de aplicación a IServiceCollection.
        /// </summary>
        /// <param name="services">La colección de servicios a la que se agregarán los servicios de aplicación.</param>
        /// <param name="config">La configuración de la aplicación.</param>
        /// <returns>La colección de servicios con los servicios de aplicación agregados.</returns>
        public static IServiceCollection AgregarServicioAplicacion(this IServiceCollection services, IConfiguration config)
        {
            // Agregar soporte para la exploración de API de puntos finales
            services.AddEndpointsApiExplorer();

            // Configurar Swagger
            services.AddSwaggerGen(options =>
            {
                // Agregar definición de seguridad para el token Bearer en Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Ingresar Bearer [espacion] token \r\n\r\n " +
                                  "Ejemplo: Bearer ejoy^8878899999990000",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });

                // Agregar requisito de seguridad para el token Bearer en Swagger
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

            // Obtener la cadena de conexión del archivo de configuración
            var connectionString = config.GetConnectionString("DefaultConnection");

            // Agregar el contexto de base de datos utilizando la cadena de conexión
            services.AddDbContext<ApplicationDbContext>(Options => Options.UseSqlServer(connectionString));

            // Agregar soporte para la política de CORS (Cross-Origin Resource Sharing)
            services.AddCors();

            // Agregar el servicio TokenServicio como un servicio de ámbito (Scoped)
            services.AddScoped<ITokenServices, TokenServicio>();

            // Configurar el comportamiento de la API para manejar los errores de validación del modelo
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

            // Devolver la colección de servicios con los servicios de aplicación agregados
            return services;
        }
    }
}
