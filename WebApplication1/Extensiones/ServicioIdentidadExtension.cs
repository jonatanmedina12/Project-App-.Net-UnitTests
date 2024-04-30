﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApplication1.Extensiones
{
    public static class ServicioIdentidadExtension
    {
        public static IServiceCollection AgregarServicioIdentidad(this IServiceCollection services, IConfiguration config)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                                                .AddJwtBearer(Options =>
                                                {
                                                    Options.TokenValidationParameters = new TokenValidationParameters
                                                    {
                                                        ValidateIssuerSigningKey = true,
                                                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                                                        ValidateIssuer = false,
                                                        ValidateAudience = false,

                                                    };
                                                });
            return services;
        }
    }
}
