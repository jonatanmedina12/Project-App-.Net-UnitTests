using Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Entidades;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Data.Servicios
{

    public class TokenServicio : ITokenServices
    {
        private readonly SymmetricSecurityKey _key;

        // Constructor
        public TokenServicio(IConfiguration config)
        {
            // Se obtiene la clave de cifrado del token desde la configuración
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        // Método para crear un token JWT basado en la información del paciente
        public string CreartToken(Paciente paciente)
        {
            // Se crea una lista de reclamaciones (claims) para el token, en este caso solo el nombre del paciente
            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, paciente.Nombre)
        };

            // Se establecen las credenciales de firma utilizando una clave simétrica y el algoritmo de firma HmacSha512
            var creads = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Se crea un descriptor para el token de seguridad, que incluye las reclamaciones, la fecha de vencimiento (7 días desde la fecha actual) y las credenciales de firma
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creads,
            };

            // Se utiliza un manejador de tokens JWT para crear el token basado en el descriptor proporcionado
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // El método devuelve el token JWT generado como una cadena de texto
            return tokenHandler.WriteToken(token);
        }
    }

}
